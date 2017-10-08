using pk3DS.Core;
using System;
using System.IO;
using System.Windows.Forms;
using pk3DS.Core.Randomizers;

namespace pk3DS
{
    public partial class StarterEditor6 : Form
    {
        public StarterEditor6()
        {
            specieslist[0] = "---";
            Array.Resize(ref specieslist, Main.Config.MaxSpeciesID + 1);
            
            if (!File.Exists(CROPath))
            {
                WinFormsUtil.Error("CRO does not exist! Closing.", CROPath);
                Close();
            }
            if (!File.Exists(FieldPath))
            {
                WinFormsUtil.Error("CRO does not exist! Closing.", FieldPath);
                Close();
            }
            InitializeComponent();

            // 2 sets of Starters for X/Y
            // 4 sets of Starters for OR/AS
            Choices = new[]
            {
                new[] {CB_G1_0, CB_G1_1, CB_G1_2},
                new[] {CB_G2_0, CB_G2_1, CB_G2_2},
                new[] {CB_G3_0, CB_G3_1, CB_G3_2},
                new[] {CB_G4_0, CB_G4_1, CB_G4_2},
            };
            Previews = new[]
            {
                new[] {PB_G1_0, PB_G1_1, PB_G1_2},
                new[] {PB_G2_0, PB_G2_1, PB_G2_2},
                new[] {PB_G3_0, PB_G3_1, PB_G3_2},
                new[] {PB_G4_0, PB_G4_1, PB_G4_2},
            };
            Labels = new[] { L_Set1, L_Set2, L_Set3, L_Set4 };

            Width = Main.Config.ORAS ? Width : Width/2 + 2;
            loadData();
        }
        private readonly string CROPath = Path.Combine(Main.RomFSPath, "DllPoke3Select.cro");
        private readonly string FieldPath = Path.Combine(Main.RomFSPath, "DllField.cro");
        private readonly string[] specieslist = Main.Config.getText(TextName.SpeciesNames);
        private readonly ComboBox[][] Choices;
        private readonly PictureBox[][] Previews;
        private readonly Label[] Labels;
        private readonly string[] StarterSummary = Main.Config.ORAS
            ? new[] { "Gen 3 Starters", "Gen 2 Starters", "Gen 4 Starters", "Gen 5 Starters" }
            : new[] { "Gen 6 Starters", "Gen 1 Starters" };
        private byte[] Data;
        private byte[] FieldData;
        private readonly int Count = Main.Config.ORAS ? 4 : 2;
        private int offset;
        private void B_Save_Click(object sender, EventArgs e)
        {
            saveData();
            Close();
        }
        private void B_Cancel_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void loadData()
        {
            Data = File.ReadAllBytes(CROPath);
            FieldData = File.ReadAllBytes(FieldPath);
            offset = BitConverter.ToInt32(Data, 0xb8);
            if (!Main.Config.ORAS) // XY have 0x10 bytes of zeroes
                offset += 0x10;
            for (int i = 0; i < Count; i++)
            {
                Labels[i].Visible = true;
                Labels[i].Text = StarterSummary[i];
                for (int j = 0; j < 3; j++)
                {
                    foreach (string s in specieslist)
                        Choices[i][j].Items.Add(s);
                    int species = BitConverter.ToUInt16(Data, offset + (i*3 + j)*0x54);
                    Choices[i][j].SelectedIndex = species; // changing index prompts loading of sprite

                    Choices[i][j].Visible = Previews[i][j].Visible = true;
                }
            }
        }
        private void saveData()
        {
            for (int i = 0; i < Count; i++)
                for (int j = 0; j < 3; j++)
                    Array.Copy(BitConverter.GetBytes((ushort)Choices[i][j].SelectedIndex), 0, Data, offset + (i*3 + j)*0x54, 2);

            // Set the choices back
            int fieldOffset = Main.Config.ORAS ? 0xF906C : 0xF805C;
            int fieldSize = Main.Config.ORAS ? 0x24 : 0x18;
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

            for (int i = 0; i < Count; i++)
                for (int j = 0; j < 3; j++)
                    Array.Copy(BitConverter.GetBytes((ushort)Choices[i][j].SelectedIndex), 0, FieldData, fieldOffset + entries[i*3 + j]*fieldSize, 2);

            File.WriteAllBytes(CROPath, Data); // poke3
            File.WriteAllBytes(FieldPath, FieldData); // field
        }

        private void changeSpecies(object sender, EventArgs e)
        {
            // Fetch the corresponding PictureBox to update
            string name = (sender as ComboBox).Name;
            int group = int.Parse(name[4]+"") - 1;
            int index = int.Parse(name[6]+"");

            int species = (sender as ComboBox).SelectedIndex;
            Previews[group][index].Image = WinFormsUtil.scaleImage(WinFormsUtil.getSprite(species, 0, 0, 0, Main.Config), 3);
        }

        private void B_Randomize_Click(object sender, EventArgs e)
        {
            bool blind = DialogResult.Yes ==
                         WinFormsUtil.Prompt(MessageBoxButtons.YesNo, "Hide randomization, save, and close?",
                             "If you want the Starters to be a surprise :)");
            if (blind)
                Hide();

            // Iterate for each group of Starters
            for (int i = 0; i < Count; i++)
            {
                // Get Species List

                int gen = int.Parse(Labels[i].Text[4] + "");
                var rand = new SpeciesRandomizer(Main.Config)
                {
                    G1 = !CHK_Gen.Checked || gen == 1,
                    G2 = !CHK_Gen.Checked || gen == 2,
                    G3 = !CHK_Gen.Checked || gen == 3,
                    G4 = !CHK_Gen.Checked || gen == 4,
                    G5 = !CHK_Gen.Checked || gen == 5,
                    G6 = !CHK_Gen.Checked || gen == 6,

                    L = false,
                    E = false,
                    Shedinja = false,

                    rBST = CHK_BST.Checked,
                };
                rand.Initialize();
                // Assign Species
                for (int j = 0; j < 3; j++)
                {
                    int oldSpecies = BitConverter.ToUInt16(Data, offset + (i * 3 + j) * 0x54);
                    Choices[i][j].SelectedIndex = rand.GetRandomSpecies(oldSpecies);
                }
            }

            if (blind)
            {
                saveData();
                Close();
            }
        }
    }
}