using System;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace pk3DS
{
    public partial class StaticEncounters : Form
    {
        public StaticEncounters()
        {
            specieslist[0] = "---";
            Array.Resize(ref specieslist, 722);
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
        private readonly int fieldOffset = Main.oras ? 0xF1B20 : 0xEE478;
        private const int fieldSize = 0xC;
        private readonly int count = Main.oras ? 0x3B : 0xC;
        private EncounterStatic[] EncounterData;
        private readonly string[] itemlist = Main.getText(Main.oras ? 114 : 96);
        private readonly string[] specieslist = Main.getText(Main.oras ? 98 : 80);
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
            EncounterData = new EncounterStatic[count];
            LB_Encounters.Items.Clear();
            for (int i = 0; i < EncounterData.Length; i++)
            {
                EncounterData[i] = new EncounterStatic(FieldData.Skip(fieldOffset + i * fieldSize).Take(fieldSize).ToArray());
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
            DialogResult ync = Util.Prompt(MessageBoxButtons.YesNoCancel,
                "Randomize by BST: Yes" + Environment.NewLine + 
                "Randomize Randomly: No" + Environment.NewLine +
                "Abort: Cancel");
            if (ync != DialogResult.Yes && ync != DialogResult.No)
                return;
            if (ync == DialogResult.No)
            {
                for (int i = 0; i < LB_Encounters.Items.Count; i++)
                {
                    LB_Encounters.SelectedIndex = i;
                    int species = Util.rand.Next(1, 721);
                    CB_Species.SelectedIndex = species;
                }
                return;
            }

            // Randomize by BST
            int[] sL = Randomizer.getSpeciesList(G1: true, G2: true, G3: true, G4: true, G5: true, G6: true, L: false, E: false, Shedinja: false);
            int ctr = 0;
            for (int i = 0; i < LB_Encounters.Items.Count; i++)
            {
                LB_Encounters.SelectedIndex = i;
                int species = CB_Species.SelectedIndex;

                int bst = Main.SpeciesStat[species].BST;
                int tries = 0;
                var pkm = Main.SpeciesStat[species = Randomizer.getRandomSpecies(ref sL, ref ctr)];
                while (!((pkm.BST*(5 - ++tries/722)/6 < bst) && pkm.BST*(6 + ++tries/722)/5 > bst))
                    pkm = Main.SpeciesStat[species = Randomizer.getRandomSpecies(ref sL, ref ctr)];

                CB_Species.SelectedIndex = species;
            }
        }

        private void changeSpecies(object sender, EventArgs e)
        {
            int index = LB_Encounters.SelectedIndex;
            LB_Encounters.Items[index] = index.ToString("00") + " - " + CB_Species.Text;
        }
    }
}