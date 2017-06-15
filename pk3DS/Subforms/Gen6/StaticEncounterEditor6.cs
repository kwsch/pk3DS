﻿using pk3DS.Core;
using pk3DS.Core.Structures.Gen6;
using System;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace pk3DS
{
    public partial class StaticEncounterEditor6 : Form
    {
        public StaticEncounterEditor6()
        {
            specieslist[0] = "---";
            Array.Resize(ref specieslist, Main.Config.MaxSpeciesID + 1);
            if (!File.Exists(FieldPath))
            {
                Util.Error("CRO does not exist! Closing.", FieldPath);
                Close();
            }
            InitializeComponent();

            specieslist[0] = "---";
            itemlist[0] = "(None)"; // blank == -1

            CB_Species.Items.Clear();
            foreach (string s in specieslist)
                CB_Species.Items.Add(s);
            CB_HeldItem.Items.Clear();
            foreach (string s in itemlist)
                CB_HeldItem.Items.Add(s);
            loadData();
        }
        private readonly string FieldPath = Path.Combine(Main.RomFSPath, "DllField.cro");
        private byte[] FieldData;
        private readonly int fieldOffset = Main.Config.ORAS ? 0xF1B20 : 0xEE478;
        private const int fieldSize = 0xC;
        private readonly int count = Main.Config.ORAS ? 0x3B : 0xC;
        private EncounterStatic6[] EncounterData;
        private readonly string[] itemlist = Main.getText(TextName.ItemNames);
        private readonly string[] specieslist = Main.getText(TextName.SpeciesNames);
        private void B_Save_Click(object sender, EventArgs e)
        {
            saveEntry();
            saveData();
            Close();
        }
        private void B_Cancel_Click(object sender, EventArgs e)
        {
            Close();
        }
        private void loadData()
        {
            FieldData = File.ReadAllBytes(FieldPath);
            EncounterData = new EncounterStatic6[count];
            LB_Encounters.Items.Clear();
            for (int i = 0; i < EncounterData.Length; i++)
            {
                EncounterData[i] = new EncounterStatic6(FieldData.Skip(fieldOffset + i * fieldSize).Take(fieldSize).ToArray());
                LB_Encounters.Items.Add($"{i.ToString("00")} - {specieslist[EncounterData[i].Species]}");
            }
            loaded = true;
            LB_Encounters.SelectedIndex = 0;
        }
        private void saveData()
        {
            for (int i = 0; i < EncounterData.Length; i++)
            {
                int offset = fieldOffset + i*fieldSize;
                // Write new data
                Array.Copy(EncounterData[i].Write(), 0, FieldData, offset, fieldSize);
            }
            File.WriteAllBytes(FieldPath, FieldData);
        }

        private int entry = -1;
        private bool loaded;
        private void changeIndex(object sender, EventArgs e)
        {
            if (LB_Encounters.SelectedIndex < 0)
                return;
            if (!loaded)
                return;
            if (entry != -1) 
                saveEntry();
            entry = LB_Encounters.SelectedIndex;
            loadEntry();
        }
        private void loadEntry()
        {
            bool oldloaded = loaded;
            loaded = false;
            CB_Species.SelectedIndex = EncounterData[entry].Species;
            CB_HeldItem.SelectedIndex = EncounterData[entry].HeldItem;
            NUD_Level.Value = EncounterData[entry].Level;
            NUD_Form.Value = EncounterData[entry].Form;
            NUD_Ability.Value = EncounterData[entry].Ability;
            NUD_Gender.Value = EncounterData[entry].Gender;

            CHK_NoShiny.Checked = EncounterData[entry].ShinyLock;
            CHK_3IV.Checked = EncounterData[entry].IV3;
            CHK_3IV_2.Checked = EncounterData[entry].IV3_1;
            loaded |= oldloaded;
        }
        private void saveEntry()
        {
            EncounterData[entry].Species = (ushort)CB_Species.SelectedIndex;
            EncounterData[entry].HeldItem = CB_HeldItem.SelectedIndex;
            EncounterData[entry].Level = (byte)NUD_Level.Value;
            EncounterData[entry].Form = (byte)NUD_Form.Value;
            EncounterData[entry].Ability = (sbyte)NUD_Ability.Value;
            EncounterData[entry].Gender = (sbyte)NUD_Gender.Value;

            EncounterData[entry].ShinyLock = CHK_NoShiny.Checked;
            EncounterData[entry].IV3 = CHK_3IV.Checked;
            EncounterData[entry].IV3_1 = CHK_3IV_2.Checked;
        }

        private void B_RandAll_Click(object sender, EventArgs e)
        {
            if (Util.Prompt(MessageBoxButtons.YesNo, "Randomize all? Cannot undo.", "Double check Randomization settings in the Randomizer Options tab.") != DialogResult.Yes) return;

            // Randomize by BST
            bool bst = CHK_BST.Checked;
            int[] sL = Randomizer.getSpeciesList(CHK_G1.Checked, CHK_G2.Checked, CHK_G3.Checked, CHK_G4.Checked, CHK_G5.Checked, CHK_G6.Checked, false,
                CHK_L.Checked, CHK_E.Checked);
            int ctr = 0;
            for (int i = 0; i < LB_Encounters.Items.Count; i++)
            {
                LB_Encounters.SelectedIndex = i;

                int species = CB_Species.SelectedIndex;
                species = Randomizer.getRandomSpecies(ref sL, ref ctr, species, bst, Main.SpeciesStat);
                CB_Species.SelectedIndex = species;
                NUD_Form.Value = Randomizer.GetRandomForme(species, false, true);
                NUD_Gender.Value = 0; // random

                if (CHK_Level.Checked)
                    NUD_Level.Value = Randomizer.getModifiedLevel((int)NUD_Level.Value, NUD_LevelBoost.Value);
            }
            Util.Alert("Randomized all Static Encounters according to specification!");
        }

        private void changeSpecies(object sender, EventArgs e)
        {
            int index = LB_Encounters.SelectedIndex;
            LB_Encounters.Items[index] = index.ToString("00") + " - " + CB_Species.Text;
        }
    }
}