﻿using pk3DS.Core;
using pk3DS.Core.Structures.Gen6;
using System;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace pk3DS
{
    public partial class GiftEditor6 : Form
    {
        public GiftEditor6()
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
            abilitylist[0] = itemlist[0] = movelist[0] = "(None)"; // blank == -1

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
        private readonly int fieldOffset = Main.Config.ORAS ? 0xF906C : 0xF805C;
        private readonly int fieldSize = Main.Config.ORAS ? 0x24 : 0x18;
        private readonly int count = Main.Config.ORAS ? 0x25 : 0x13;
        private EncounterGift6[] GiftData;
        private readonly string[] abilitylist = Main.getText(TextName.AbilityNames);
        private readonly string[] movelist = Main.getText(TextName.MoveNames);
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
            GiftData = new EncounterGift6[count];
            LB_Gifts.Items.Clear();
            for (int i = 0; i < GiftData.Length; i++)
            {
                GiftData[i] = new EncounterGift6(FieldData.Skip(fieldOffset + i * fieldSize).Take(fieldSize).ToArray(), Main.Config.ORAS);
                LB_Gifts.Items.Add($"{i.ToString("00")} - {specieslist[GiftData[i].Species]}");
            }
            loaded = true;
            LB_Gifts.SelectedIndex = 0;
        }
        private void saveData()
        {
            // Check to see if a starter has been modified right before we write data.
            bool starters = false;
            int[] entries = Main.Config.ORAS
                ? new[]
                {
                    0, 1, 2, // Gen 3
                    28, 29, 30, // Gen 2
                    31, 32, 33, // Gen 4
                    34, 35, 36 // Gen 5
                }
                : new[]
                {
                    0, 1, 2, // Gen 6
                    3, 4, 5, // Gen 1
                };

            for (int i = 0; i < GiftData.Length; i++)
            {
                int offset = fieldOffset + i*fieldSize;

                // Check too see if starters got modified
                if (Array.IndexOf(entries, i) > - 1 && BitConverter.ToUInt16(FieldData, offset) != GiftData[i].Species)
                    starters = true;
                
                // Write new data
                Array.Copy(GiftData[i].Write(), 0, FieldData, offset, fieldSize);
            }

            if (starters) // are modified
                Util.Alert("Starters have been modified.", 
                    "Be sure to update the Starters in DllPoke3Select.cro by updating via the Starter Editor.");

            File.WriteAllBytes(FieldPath, FieldData);
        }

        private int entry = -1;
        private bool loaded;
        private void changeIndex(object sender, EventArgs e)
        {
            if (LB_Gifts.SelectedIndex < 0)
                return;
            if (!loaded)
                return;
            if (entry != -1) 
                saveEntry();
            entry = LB_Gifts.SelectedIndex;
            loadEntry();
        }
        private void loadEntry()
        {
            bool oldloaded = loaded;
            loaded = false;
            CB_Species.SelectedIndex = GiftData[entry].Species;
            CB_HeldItem.SelectedIndex = GiftData[entry].HeldItem;
            NUD_Level.Value = GiftData[entry].Level;
            NUD_Form.Value = GiftData[entry].Form;
            NUD_Nature.Value = GiftData[entry].Nature;
            NUD_Ability.Value = GiftData[entry].Ability;
            NUD_Gender.Value = GiftData[entry].Gender;

            NUD_IV0.Value = GiftData[entry].IVs[0];
            NUD_IV1.Value = GiftData[entry].IVs[1];
            NUD_IV2.Value = GiftData[entry].IVs[2];
            NUD_IV3.Value = GiftData[entry].IVs[3];
            NUD_IV4.Value = GiftData[entry].IVs[4];
            NUD_IV5.Value = GiftData[entry].IVs[5];
            loaded |= oldloaded;
        }
        private void saveEntry()
        {
            GiftData[entry].Species = (ushort)CB_Species.SelectedIndex;
            GiftData[entry].HeldItem = CB_HeldItem.SelectedIndex;
            GiftData[entry].Level = (byte)NUD_Level.Value;
            GiftData[entry].Form = (byte)NUD_Form.Value;
            GiftData[entry].Nature = (sbyte)NUD_Nature.Value;
            GiftData[entry].Ability = (sbyte)NUD_Ability.Value;
            GiftData[entry].Gender = (sbyte)NUD_Gender.Value;

            GiftData[entry].IVs[0] = (sbyte)NUD_IV0.Value;
            GiftData[entry].IVs[1] = (sbyte)NUD_IV1.Value;
            GiftData[entry].IVs[2] = (sbyte)NUD_IV2.Value;
            GiftData[entry].IVs[3] = (sbyte)NUD_IV3.Value;
            GiftData[entry].IVs[4] = (sbyte)NUD_IV4.Value;
            GiftData[entry].IVs[5] = (sbyte)NUD_IV5.Value;
        }

        private void B_RandAll_Click(object sender, EventArgs e)
        {
            if (Util.Prompt(MessageBoxButtons.YesNo, "Randomize all? Cannot undo.", "Double check Randomization settings in the Randomizer Options tab.") != DialogResult.Yes) return;

            // Randomize by BST
            bool bst = CHK_BST.Checked;
            int[] sL = Randomizer.getSpeciesList(CHK_G1.Checked, CHK_G2.Checked, CHK_G3.Checked, CHK_G4.Checked, CHK_G5.Checked, CHK_G6.Checked, false,
                CHK_L.Checked, CHK_E.Checked);
            int ctr = 0;
            for (int i = 0; i < LB_Gifts.Items.Count; i++)
            {
                LB_Gifts.SelectedIndex = i;
                int species = CB_Species.SelectedIndex;
                if (species == 448)
                    continue; // skip Lucario, battle needs to mega evolve
                
                species = Randomizer.getRandomSpecies(ref sL, ref ctr, species, bst, Main.SpeciesStat);
                CB_Species.SelectedIndex = species;
                NUD_Form.Value = Randomizer.GetRandomForme(species, false, true);
                NUD_Gender.Value = 0; // random

                if (CHK_Level.Checked)
                    NUD_Level.Value = Randomizer.getModifiedLevel((int)NUD_Level.Value, NUD_LevelBoost.Value);
            }
            Util.Alert("Randomized all Gift Pokémon according to specification!");
        }

        private void changeSpecies(object sender, EventArgs e)
        {
            int index = LB_Gifts.SelectedIndex;
            LB_Gifts.Items[index] = index.ToString("00") + " - " + CB_Species.Text;
        }
    }
}