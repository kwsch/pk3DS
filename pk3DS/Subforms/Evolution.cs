using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Media;
using System.Text;
using System.Windows.Forms;
using pk3DS.Properties;

namespace pk3DS
{
    public partial class Evolution : Form
    {
        public Evolution()
        {
            InitializeComponent();

            specieslist[0] = movelist[0] = itemlist[0] = "";
            Array.Resize(ref specieslist, 722);

            string[] evolutionMethods =
            { 
                "",
                "Level Up with Friendship",
                "Level Up at Morning",
                "Level Up at Night",
                "Level Up",
                "Trade",
                "Trade with Held Item",
                String.Format("Trade for opposite {0}/{1}", specieslist[588], specieslist[616]), // Shelmet&Karrablast
                "Used Item",
                "Level Up (Attack > Defense)",
                "Level Up (Attack = Defense)",
                "Level Up (Attack < Defense)",
                "Level Up (Random < 5)",
                "Level Up (Random > 5)",
                String.Format("Level Up ({0})", specieslist[291]), // Ninjask
                String.Format("Level Up ({0})", specieslist[292]), // Shedinja
                "Level Up (Beauty)",
                "Level Up with Held Item (Male)",
                "Level Up with Held Item (Female)",
                "Level Up with Held Item (Day)",
                "Level Up with Held Item (Night)",
                "Level Up with Move",
                "Level up with Party",
                "Level Up Male",
                "Level Up Female",
                "Level Up at Electric",
                "Level Up at Forest",
                "Level Up at Cold",
                "Level Up with 3DS Upside Down",
                "Level Up with 50 Affection + MoveType",
                String.Format("{0} Type in Party", typelist[16]),
                "Overworld Rain",
                "Level Up (@) at Night",
                "Level Up (@) at Night",
                "Level Up Female (SetForm 1)",
            };

            mb = new[] { CB_M1, CB_M2, CB_M3, CB_M4, CB_M5, CB_M6, CB_M7, CB_M8 };
            pb = new[] { CB_P1, CB_P2, CB_P3, CB_P4, CB_P5, CB_P6, CB_P7, CB_P8 };
            rb = new[] { CB_I1, CB_I2, CB_I3, CB_I4, CB_I5, CB_I6, CB_I7, CB_I8 };
            pic = new[] { PB_1, PB_2, PB_3, PB_4, PB_5, PB_6, PB_7, PB_8 };

            foreach (ComboBox cb in mb) { foreach (string s in evolutionMethods) cb.Items.Add(s); }
            foreach (ComboBox cb in rb) { foreach (string s in specieslist) cb.Items.Add(s); }

            sortedspecies = (string[])specieslist.Clone();
            Array.Sort(sortedspecies);

            CB_Species.Items.Clear();
            foreach (string s in sortedspecies) CB_Species.Items.Add(s);
            CB_Species.Items.RemoveAt(0);

            CB_Species.SelectedIndex = 0;
        }
        private string[] files = Directory.GetFiles("evolution");
        private ComboBox[] pb, rb, mb;
        private PictureBox[] pic;
        private int entry = -1;
        private string[] sortedspecies;
        private string[] specieslist = Main.getText((Main.oras) ? 98 : 80);
        private string[] movelist = Main.getText((Main.oras) ? 14 : 13);
        private string[] itemlist = Main.getText((Main.oras) ? 114 : 96);
        private string[] typelist = Main.getText((Main.oras) ? 18 : 17);
        bool dumping;
        private void getList()
        {
            entry = Array.IndexOf(specieslist, CB_Species.Text);
            byte[] input = File.ReadAllBytes(files[entry]);
            if (input.Length != 0x30) return; // error

            for (int i = 0; i < 8; i++)
            {
                int method = BitConverter.ToUInt16(input, 0 + 6 * i);
                int param = BitConverter.ToUInt16(input, 2 + 6 * i);
                int poke = BitConverter.ToUInt16(input, 4 + 6 * i);
                if (method > 34) return; // Invalid!

                mb[i].SelectedIndex = method; // Which will trigger the params cb to reload the valid params list
                pb[i].SelectedIndex = param;
                rb[i].SelectedIndex = poke;
            }
        }
        private void setList()
        {
            if (entry < 1 || dumping) return;

            List<byte[]> methods = new List<byte[]>();
            for (int i = 0; i < 8; i++)
            {
                // Each Evolution Method is comprised of 6 bytes.
                byte[] method = new byte[6];
                int methodval = mb[i].SelectedIndex;
                int param = pb[i].SelectedIndex;
                int poke = rb[i].SelectedIndex;

                if (poke > 0 && methodval > 0)
                {
                    Array.Copy(BitConverter.GetBytes((ushort)methodval), method, 2);
                    Array.Copy(BitConverter.GetBytes((ushort)param), 0, method, 2, 2);
                    Array.Copy(BitConverter.GetBytes((ushort)poke), 0, method, 4, 2);
                    methods.Add(method);
                }
            }

            byte[] data = new byte[0x30];
            byte[][] methodList = methods.ToArray();
            for (int i = 0; i < methodList.Length; i++)
                Array.Copy(methodList[i], 0, data, i * 6, 6);

            File.WriteAllBytes(files[entry], data);
        }

        private void changeEntry(object sender, EventArgs e)
        {
            setList();
            getList();
        }

        public static int[] sL; // Random Species List
        byte[][] personal;
        private void B_RandAll_Click(object sender, EventArgs e)
        {
            if (DialogResult.Yes != Util.Prompt(MessageBoxButtons.YesNo, "Randomize all resulting species?", "Evolution methods and parameters will stay the same.")) return;
            Random rnd = new Random();

            // Set up advanced randomization options
            bool rBST = CHK_BST.Checked;
            bool rEXP = CHK_Exp.Checked;
            bool rType = CHK_Type.Checked;
            if (rBST || rEXP || rType)
            {
                // initialize personal data
                string[] personalList = Directory.GetFiles("personal");
                personal = new byte[personalList.Length][];
                for (int i = 0; i < personalList.Length; i++)
                    personal[i] = File.ReadAllBytes("personal" + Path.DirectorySeparatorChar + i.ToString("000") + ".bin");
            }
            int ctr = 0;
            sL = Randomizer.RandomSpeciesList;

            for (int i = 0; i < CB_Species.Items.Count; i++)
            {
                CB_Species.SelectedIndex = i;
                for (int j = 0; j < mb.Length; j++)
                    if (mb[j].SelectedIndex > 0)
                    {
                        // Get a new random species
                        int oldSpecies = rb[j].SelectedIndex;
                        int currentSpecies = Array.IndexOf(specieslist, CB_Species.Text);
                        int loopctr = 0; // altering calculatiosn to prevent infinite loops
                    defspecies:
                        int newSpecies = Randomizer.getRandomSpecies(ref sL, ref ctr);
                        loopctr++;

                        // Verify it meets specifications
                        if (newSpecies == currentSpecies // no A->A evolutions
                            || (newSpecies == oldSpecies && loopctr < 722 * 5)) // prevent runaway calcs
                        { goto defspecies; }
                        if (rEXP) // Experience Growth Rate matches
                        {
                            if (personal[oldSpecies][0x15] != personal[newSpecies][0x15])
                            { goto defspecies; }
                        }
                        if (rType) // Type has to be somewhat similar
                        {
                            int o1 = personal[oldSpecies][6];
                            int o2 = personal[oldSpecies][7];
                            int n1 = personal[newSpecies][6];
                            int n2 = personal[newSpecies][7];
                            if (o1 != n1 && o1 != n2 && o2 != n2 && o2 != n1)
                            { goto defspecies; }
                        }
                        if (rBST) // Base stat total has to be close
                        {
                            const int l = 5; // tweakable scalars
                            const int h = 6;
                            int oldBST = personal[oldSpecies].Take(6).Sum(b => (ushort)b);
                            int newBST = personal[newSpecies].Take(6).Sum(b => (ushort)b);
                            if (!(newBST * l / (h + loopctr/722) < oldBST && newBST * h / l > oldBST))
                            { goto defspecies; }
                        }
                        // assign random val
                        rb[j].SelectedIndex = newSpecies;
                    }
            }
            setList();
            Util.Alert("All Pokemon's Evolutions have been randomized!");
        }
        private void B_Dump_Click(object sender, EventArgs e)
        {
            if (DialogResult.Yes != Util.Prompt(MessageBoxButtons.YesNo, "Dump all Evolutions to Text File?"))
                return;

            dumping = true;
            string result = "";
            for (int i = 0; i < CB_Species.Items.Count; i++)
            {
                CB_Species.SelectedIndex = i; // Get new Species
                result += "======" + Environment.NewLine + entry + " " + CB_Species.Text + Environment.NewLine + "======" + Environment.NewLine;
                for (int j = 0; j < 8; j++)
                {
                    int methodval = mb[j].SelectedIndex;
                    // int param = pb[j].SelectedIndex;
                    int poke = rb[j].SelectedIndex;
                    if (poke > 0 && methodval > 0)
                        result += mb[j].Text + ((pb[j].Visible) ? " [" + pb[j].Text + "]" : "") + " into " + rb[j].Text + Environment.NewLine;
                }

                result += Environment.NewLine;
            }
            SaveFileDialog sfd = new SaveFileDialog {FileName = "Evolutions.txt", Filter = "Text File|*.txt"};

            SystemSounds.Asterisk.Play();
            if (sfd.ShowDialog() == DialogResult.OK)
            {
                string path = sfd.FileName;
                File.WriteAllText(path, result, Encoding.Unicode);
            }
            dumping = false;
        }

        private void formClosing(object sender, FormClosingEventArgs e)
        {
            setList();
        }

        private void changeMethod(object sender, EventArgs e)
        {
            int op = Array.IndexOf(mb, sender as ComboBox);
            ushort[] methodCase =
            { 
                0,0,0,0,1,0,2,0,2,1,1,1,1,1,1,1,5,2,2,2,2,3,4,1,1,0,0,0, // 27, Past Methods
                // New Methods
                1, // 28 - Dark Type Party
                6, // 29 - Affection + MoveType
                1, // 30 - Dark Type
                1, // 31 - Overworld Rain
                1, // 32 - Level @ Day
                1, // 33 - Level @ Night
                1, // 34 - Gender Branch
            };

            pb[op].Visible = pic[op].Visible = rb[op].Visible = (mb[op].SelectedIndex > 0);

            pb[op].Items.Clear();
            int cv = methodCase[mb[op].SelectedIndex];
            switch (cv)
            {
                case 0: // No Parameter Required
                    { pb[op].Visible = false; pb[op].Items.Add(""); break; }
                case 1: // Level
                    { for (int i = 0; i <= 100; i++) pb[op].Items.Add(i.ToString()); break; }
                case 2: // Items
                    {  foreach (string t in itemlist) pb[op].Items.Add(t); break; }
                case 3: // Moves
                    { foreach (string t in movelist) pb[op].Items.Add(t); break; }
                case 4: // Species
                    { for (int i = 0; i < sortedspecies.Length; i++) pb[op].Items.Add(specieslist[i]); break; }
                case 5: // 0-255 (Beauty)
                    { for (int i = 0; i <= 255; i++) pb[op].Items.Add(i.ToString()); break; }
                case 6:
                    { foreach (string t in typelist) pb[op].Items.Add(t); break; }
            }
            pb[op].SelectedIndex = 0;
        }
        private void changeInto(object sender, EventArgs e)
        {
            pic[Array.IndexOf(rb, sender as ComboBox)].Image = (Bitmap)Resources.ResourceManager.GetObject("_" + Array.IndexOf(specieslist, (sender as ComboBox).Text));
        }
    }
}