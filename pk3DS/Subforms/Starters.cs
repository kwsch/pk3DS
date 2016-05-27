using System;
using System.IO;
using System.Windows.Forms;

namespace pk3DS
{
    public partial class Starters : Form
    {
        private readonly byte[][] personal;
        public Starters()
        {
            specieslist[0] = "---";
            Array.Resize(ref specieslist, 722);

            string[] personalList = Directory.GetFiles("personal");
            personal = new byte[personalList.Length][];
            for (int i = 0; i < personalList.Length; i++)
                personal[i] = File.ReadAllBytes("personal" + Path.DirectorySeparatorChar + i.ToString("000") + ".bin");
            if (!File.Exists(CROPath))
            {
                Util.Error("CRO does not exist! Closing.", CROPath);
                Close();
            }
            if (!File.Exists(FieldPath))
            {
                Util.Error("CRO does not exist! Closing.", FieldPath);
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

            Width = Main.oras ? Width : Width/2 + 2;
            loadData();
        }
        private readonly string CROPath = Path.Combine(Main.RomFSPath, "DllPoke3Select.cro");
        private readonly string FieldPath = Path.Combine(Main.RomFSPath, "DllField.cro");
        private readonly string[] specieslist = Main.getText(Main.oras ? 98 : 80);
        private readonly ComboBox[][] Choices;
        private readonly PictureBox[][] Previews;
        private readonly Label[] Labels;
        private readonly string[] StarterSummary = Main.oras
            ? new[] { "Gen 3 Starters", "Gen 2 Starters", "Gen 4 Starters", "Gen 5 Starters" }
            : new[] { "Gen 6 Starters", "Gen 1 Starters" };
        private byte[] Data;
        private byte[] FieldData;
        private readonly int Count = Main.oras ? 4 : 2;
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
            if (!Main.oras) // XY have 0x10 bytes of zeroes
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
            int fieldOffset = Main.oras ? 0xF906C : 0xF805C;
            int fieldSize = Main.oras ? 0x24 : 0x18;
            int[] entries = Main.oras
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
            Previews[group][index].Image = Util.scaleImage(Util.getSprite(species, 0, 0, 0), 3);
        }

        private void B_Randomize_Click(object sender, EventArgs e)
        {
            bool blind = DialogResult.Yes ==
                         Util.Prompt(MessageBoxButtons.YesNo, "Hide randomization, save, and close?",
                             "If you want the Starters to be a surprise :)");
            if (blind)
                Hide();

            // Iterate for each group of Starters
            for (int i = 0; i < Count; i++)
            {
                // Get Species List
                int gen = int.Parse(Labels[i].Text[4]+"");
                int[] sL = CHK_Gen.Checked
                    ? Randomizer.getSpeciesList(gen==1, gen==2, gen==3, gen==4, gen==5, gen==6, false, false, false)
                    : Randomizer.getSpeciesList(true, true, true, true, true, true, false, false, false);
                int ctr = 0;
                // Assign Species
                for (int j = 0; j < 3; j++)
                {
                    int species = Randomizer.getRandomSpecies(ref sL, ref ctr);

                    if (CHK_BST.Checked) // Enforce BST
                    {
                        PersonalInfo oldpkm = new PersonalInfo(personal[BitConverter.ToUInt16(Data, offset + (i * 3 + j) * 0x54)]); // Use original species cuz why not.
                        PersonalInfo pkm = new PersonalInfo(personal[species]);

                        while (!(pkm.BST * 5 / 6 < oldpkm.BST && pkm.BST * 6 / 5 > oldpkm.BST))
                        { species = Randomizer.getRandomSpecies(ref sL, ref ctr); pkm = new PersonalInfo(personal[species]); }
                    }

                    Choices[i][j].SelectedIndex = species;
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