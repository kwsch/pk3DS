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
            LoadData();
            RandSettings.GetFormSettings(this, tabPage2.Controls);
        }

        private readonly string FieldPath = Path.Combine(Main.RomFSPath, "DllField.cro");
        private byte[] FieldData;
        private readonly int fieldOffset = Main.Config.ORAS ? 0xF1B20 : 0xEE478;
        private const int fieldSize = 0xC;
        private readonly int count = Main.Config.ORAS ? 0x3B : 0xC;
        private EncounterStatic6[] EncounterData;
        private readonly string[] itemlist = Main.Config.GetText(TextName.ItemNames);
        private readonly string[] specieslist = Main.Config.GetText(TextName.SpeciesNames);
        private static int[] FinalEvo = Legal.FinalEvolutions_6;
        private static readonly int[] Legendary = Legal.Legendary_6;
        private static readonly int[] Mythical = Legal.Mythical_6;
        private static readonly int[] ReplaceLegend = Legendary.Concat(Mythical).ToArray();

        private readonly string[] ability =
        {
            "Any (1 or 2)",
            "Ability 1",
            "Ability 2",
            "Hidden Ability",
        };

        private void B_Save_Click(object sender, EventArgs e)
        {
            SaveEntry();
            SaveData();
            RandSettings.SetFormSettings(this, tabPage2.Controls);
            Close();
        }

        private void B_Cancel_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void LoadData()
        {
            FieldData = File.ReadAllBytes(FieldPath);
            EncounterData = new EncounterStatic6[count];
            LB_Encounters.Items.Clear();
            for (int i = 0; i < EncounterData.Length; i++)
            {
                EncounterData[i] = new EncounterStatic6(FieldData.Skip(fieldOffset + (i * fieldSize)).Take(fieldSize).ToArray());
                LB_Encounters.Items.Add($"{i:00} - {specieslist[EncounterData[i].Species]}");
            }
            foreach (var s in ability) CB_Ability.Items.Add(s);

            CB_Gender.Items.Clear();
            CB_Gender.Items.Add("- / Genderless/Random");
            CB_Gender.Items.Add("♂ / Male");
            CB_Gender.Items.Add("♀ / Female");

            loaded = true;
            LB_Encounters.SelectedIndex = 0;
        }

        private void SaveData()
        {
            for (int i = 0; i < EncounterData.Length; i++)
            {
                int offset = fieldOffset + (i * fieldSize);
                // Write new data
                Array.Copy(EncounterData[i].Write(), 0, FieldData, offset, fieldSize);
            }
            File.WriteAllBytes(FieldPath, FieldData);
        }

        private int entry = -1;
        private bool loaded;

        private void ChangeIndex(object sender, EventArgs e)
        {
            if (LB_Encounters.SelectedIndex < 0)
                return;
            if (!loaded)
                return;
            if (entry != -1)
                SaveEntry();
            entry = LB_Encounters.SelectedIndex;
            LoadEntry();
        }

        private void LoadEntry()
        {
            bool oldloaded = loaded;
            loaded = false;

            CB_Species.SelectedIndex = EncounterData[entry].Species;
            CB_HeldItem.SelectedIndex = EncounterData[entry].HeldItem;
            NUD_Level.Value = EncounterData[entry].Level;
            NUD_Form.Value = EncounterData[entry].Form;
            CB_Ability.SelectedIndex = EncounterData[entry].Ability;
            CB_Gender.SelectedIndex = EncounterData[entry].Gender;
            CHK_ShinyLock.Checked = EncounterData[entry].ShinyLock;
            CHK_IV3.Checked = EncounterData[entry].IV3;

            if (EncounterData[entry].HeldItem < 0)
                CB_HeldItem.SelectedIndex = 0; // no item = 0xFFFF, set to 0 (None)

            loaded |= oldloaded;
        }

        private void SaveEntry()
        {
            EncounterData[entry].Species = (ushort)CB_Species.SelectedIndex;
            EncounterData[entry].HeldItem = CB_HeldItem.SelectedIndex;
            EncounterData[entry].Level = (byte)NUD_Level.Value;
            EncounterData[entry].Form = (byte)NUD_Form.Value;
            EncounterData[entry].Ability = (sbyte)CB_Ability.SelectedIndex;
            EncounterData[entry].Gender = (sbyte)CB_Gender.SelectedIndex;
            EncounterData[entry].ShinyLock = CHK_ShinyLock.Checked;
            EncounterData[entry].IV3 = CHK_IV3.Checked;
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

            // add Legendary/Mythical to final evolutions if checked
            if (CHK_L.Checked) FinalEvo = FinalEvo.Concat(Legendary).ToArray();
            if (CHK_E.Checked) FinalEvo = FinalEvo.Concat(Mythical).ToArray();

            var items = Randomizer.GetRandomItemList();
            for (int i = 0; i < LB_Encounters.Items.Count; i++)
            {
                LB_Encounters.SelectedIndex = i;
                int species = CB_Species.SelectedIndex;

                // replace Legendaries with another Legendary
                if (CHK_ReplaceLegend.Checked && ReplaceLegend.Contains(species))
                {
                    int randLegend() => (int)(Util.Random32() % ReplaceLegend.Length);
                    species = ReplaceLegend[randLegend()];
                }

                // every other entry
                else
                {
                    species = specrand.GetRandomSpecies(species);
                }

                if (CHK_AllowMega.Checked)
                    formrand.AllowMega = true;

                if (CHK_Item.Checked)
                    CB_HeldItem.SelectedIndex = items[Util.Random32() % items.Length];

                if (CHK_Level.Checked)
                    NUD_Level.Value = Randomizer.GetModifiedLevel((int)NUD_Level.Value, NUD_LevelBoost.Value);

                if (CHK_RemoveShinyLock.Checked)
                    CHK_ShinyLock.Checked = false;

                if (CHK_RandomAbility.Checked)
                    CB_Ability.SelectedIndex = (Util.Rand.Next(1, 4)); // 1, 2 , or H

                if (CHK_ForceFullyEvolved.Checked && NUD_Level.Value >= NUD_ForceFullyEvolved.Value && !FinalEvo.Contains(species))
                {
                    int randFinalEvo() => (int)(Util.Random32() % FinalEvo.Length);
                    species = FinalEvo[randFinalEvo()];
                }

                CB_Species.SelectedIndex = species;
                NUD_Form.Value = formrand.GetRandomForme(species);
                CB_Gender.SelectedIndex = 0; // random
            }
            WinFormsUtil.Alert("Randomized all Static Encounters according to specification!");
        }

        private void ChangeSpecies(object sender, EventArgs e)
        {
            int index = LB_Encounters.SelectedIndex;
            LB_Encounters.Items[index] = index.ToString("00") + " - " + CB_Species.Text;
        }

        private void ModifyLevels(object sender, EventArgs e)
        {
            if (WinFormsUtil.Prompt(MessageBoxButtons.YesNo, "Modify all current Levels?", "Cannot undo.") != DialogResult.Yes) return;

            for (int i = 0; i < LB_Encounters.Items.Count; i++)
            {
                LB_Encounters.SelectedIndex = i;
                NUD_Level.Value = Randomizer.GetModifiedLevel((int)NUD_Level.Value, NUD_LevelBoost.Value);
            }
            WinFormsUtil.Alert("Modified all Levels according to specification!");
        }
    }
}