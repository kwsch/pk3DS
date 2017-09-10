using pk3DS.Core;
using System;
using System.IO;
using System.Linq;
using System.Windows.Forms;

using pk3DS.Core.Randomizers;
using pk3DS.Core.Structures;

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
                WinFormsUtil.Error("CRO does not exist! Closing.", FieldPath);
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
        private readonly string[] itemlist = Main.Config.getText(TextName.ItemNames);
        private readonly string[] specieslist = Main.Config.getText(TextName.SpeciesNames);
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
            if (WinFormsUtil.Prompt(MessageBoxButtons.YesNo, "Randomize all? Cannot undo.", "Double check Randomization settings in the Randomizer Options tab.") != DialogResult.Yes) return;

            var formrand = new FormRandomizer(Main.Config) { AllowMega = false, AllowAlolanForm = false };
            var specrand = new SpeciesRandomizer(Main.Config)
            {
                G1 = CHK_G1.Checked,
                G2 = CHK_G2.Checked,
                G3 = CHK_G3.Checked,
                G4 = CHK_G4.Checked,
                G5 = CHK_G5.Checked,
                G6 = CHK_G6.Checked,
                G7 = false,

                E = CHK_E.Checked,
                L = CHK_L.Checked,

                rBST = CHK_BST.Checked,
            };
            specrand.Initialize();
            for (int i = 0; i < LB_Encounters.Items.Count; i++)
            {
                LB_Encounters.SelectedIndex = i;

                int species = CB_Species.SelectedIndex;
                species = specrand.GetRandomSpecies(species);
                CB_Species.SelectedIndex = species;
                NUD_Form.Value = formrand.GetRandomForme(species);
                NUD_Gender.Value = 0; // random

                if (CHK_Level.Checked)
                    NUD_Level.Value = Randomizer.getModifiedLevel((int)NUD_Level.Value, NUD_LevelBoost.Value);
            }
            WinFormsUtil.Alert("Randomized all Static Encounters according to specification!");
        }

        private void changeSpecies(object sender, EventArgs e)
        {
            int index = LB_Encounters.SelectedIndex;
            LB_Encounters.Items[index] = index.ToString("00") + " - " + CB_Species.Text;
        }
    }
}