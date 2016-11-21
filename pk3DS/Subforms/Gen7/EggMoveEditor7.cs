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
    public partial class EggMoveEditor7 : Form
    {
        public EggMoveEditor7(byte[][] infiles)
        {
            InitializeComponent();
            files = infiles;
            string[] species = Main.getText(TextName.SpeciesNames);
            string[][] AltForms = Main.Config.Personal.getFormList(species, Main.Config.MaxSpeciesID);
            string[] specieslist = Main.Config.Personal.getPersonalEntryList(AltForms, species, Main.Config.MaxSpeciesID, out baseForms, out formVal);
            specieslist[0] = movelist[0] = "";
            
            setupDGV();

            var newlist = new List<Util.cbItem>();
            for (int i = 0; i < species.Length; i++) // add all species & forms
                newlist.Add(new Util.cbItem { Text = species[i] + $" ({i})", Value = i });
            newlist = newlist.OrderBy(item => item.Text).ToList();
            for (int i = species.Length; i < files.Length; i++)
                newlist.Add(new Util.cbItem { Text = $"{i.ToString("0000")} - Extra", Value = i });
            NUD_FormTable.Maximum = files.Length;

            CB_Species.DisplayMember = "Text";
            CB_Species.ValueMember = "Value";
            CB_Species.DataSource = newlist;


            CB_Species.SelectedIndex = 0;
        }
        private readonly byte[][] files;
        private int entry = -1;
        private readonly string[] movelist = Main.getText(TextName.MoveNames);
        private bool dumping;
        private readonly int[] baseForms, formVal;
        private void setupDGV()
        {
            string[] sortedmoves = (string[])movelist.Clone();
            Array.Sort(sortedmoves);
            DataGridViewComboBoxColumn dgvMove = new DataGridViewComboBoxColumn();
            {
                dgvMove.HeaderText = "Move";
                dgvMove.DisplayIndex = 0;
                for (int i = 0; i < movelist.Length; i++)
                    dgvMove.Items.Add(sortedmoves[i]); // add only the Names

                dgvMove.Width = 135;
                dgvMove.FlatStyle = FlatStyle.Flat;
            }
            dgv.Columns.Add(dgvMove);
        }

        private EggMoves pkm = new EggMoves7(new byte[0]);
        private void getList()
        {
            entry = Util.getIndex(CB_Species);
            int s = 0, f = 0;
            if (entry <= Main.Config.MaxSpeciesID)
            {
                s = entry;
            }
            int[] specForm = { s, f };
            string filename = "_" + specForm[0] + (entry > Main.Config.MaxSpeciesID ? "_" + (specForm[1] + 1) : "");
            PB_MonSprite.Image = (Bitmap)Resources.ResourceManager.GetObject(filename);

            dgv.Rows.Clear();
            byte[] input = files[entry];
            pkm = new EggMoves7(input);
            NUD_FormTable.Value = pkm.FormTableIndex;
            if (pkm.Count < 1) { files[entry] = new byte[0]; return; }
            dgv.Rows.Add(pkm.Count);

            // Fill Entries
            for (int i = 0; i < pkm.Count; i++)
                dgv.Rows[i].Cells[0].Value = movelist[pkm.Moves[i]];

            dgv.CancelEdit();
        }
        private void setList()
        {
            if (entry < 1 || dumping) return;
            List<int> moves = new List<int>();
            for (int i = 0; i < dgv.Rows.Count - 1; i++)
            {
                int move = Array.IndexOf(movelist, dgv.Rows[i].Cells[0].Value);
                if (move > 0 && !moves.Contains((ushort)move)) moves.Add(move);
            }
            pkm.Moves = moves.ToArray();

            files[entry] = pkm.Write();
        }

        private void changeEntry(object sender, EventArgs e)
        {
            setList();
            getList();
        }

        private void B_RandAll_Click(object sender, EventArgs e)
        {
            /*
             * 3111 Egg Moves Learned by 290 Species (10.73 avg)
             * 18 is the most
             * 1000 moves learned were STAB (32.1%)
             */
            Random rnd = new Random();

            int[] banned = new[] { 165, 621, 464 }.Concat(Legal.Z_Moves).ToArray(); // Struggle, Hyperspace Fury, Dark Void

            // Move Stats
            Move[] moveTypes = Main.Config.Moves;
            
            // Set up Randomized Moves
            int[] randomMoves = Enumerable.Range(1, movelist.Length - 1).Select(i => i).ToArray();
            Util.Shuffle(randomMoves);
            int ctr = 0;

            for (int i = 0; i < CB_Species.Items.Count; i++)
            {
                CB_Species.SelectedIndex = i; // Get new Species
                int count = dgv.Rows.Count - 1;
                int species = Util.getIndex(CB_Species);
                if (CHK_Expand.Checked && (int)NUD_Moves.Value > count)
                    dgv.Rows.AddCopies(count, (int)NUD_Moves.Value - count);
                
                for (int j = 1; j < dgv.Rows.Count - 1; j++)
                {
                    // Assign New Moves
                    bool forceSTAB = CHK_STAB.Checked && rnd.Next(0, 99) < NUD_STAB.Value;
                    int move = Randomizer.getRandomSpecies(ref randomMoves, ref ctr);

                    while (banned.Contains(move) /* Invalid */
                        || (forceSTAB && !Main.SpeciesStat[species].Types.Contains(moveTypes[move].Type))) // STAB is required
                        move = Randomizer.getRandomSpecies(ref randomMoves, ref ctr);

                    // Assign Move
                    dgv.Rows[j].Cells[0].Value = movelist[move];
                }
            }
            CB_Species.SelectedIndex = 0;
            Util.Alert("All Pokemon's Egg Up Moves have been randomized!");
        }
        private void B_Dump_Click(object sender, EventArgs e)
        {
            if (DialogResult.Yes != Util.Prompt(MessageBoxButtons.YesNo, "Dump all Egg Moves to Text File?"))
                return;

            dumping = true;
            string result = "";
            for (int i = 0; i < CB_Species.Items.Count; i++)
            {
                CB_Species.SelectedIndex = i; // Get new Species
                result += "======" + Environment.NewLine + entry + " " + CB_Species.Text + Environment.NewLine + "======" + Environment.NewLine;
                for (int j = 0; j < dgv.Rows.Count - 1; j++)
                    result += dgv.Rows[j].Cells[0].Value + Environment.NewLine;

                result += Environment.NewLine;
            }
            SaveFileDialog sfd = new SaveFileDialog {FileName = "Egg Moves.txt", Filter = "Text File|*.txt"};

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

        private void B_Goto_Click(object sender, EventArgs e)
        {
            CB_Species.SelectedValue = (int)NUD_FormTable.Value;
        }

        private void calcStats()
        {
            Move[] MoveData = Main.Config.Moves;
            int movectr = 0;
            int max = 0;
            int spec = 0;
            int stab = 0;
            for (int i = 0; i < Main.Config.MaxSpeciesID; i++)
            {
                byte[] movedata = files[i];
                int movecount = BitConverter.ToUInt16(movedata, 2);
                if (movecount == 65535)
                    continue;
                movectr += movecount; // Average Moves
                if (max < movecount) { max = movecount; spec = i; } // Max Moves (and species)
                for (int m = 0; m < movecount; m++)
                {
                    int move = BitConverter.ToUInt16(movedata, m * 2 + 4);
                    if (Main.SpeciesStat[i].Types.Contains(MoveData[move].Type))
                        stab++;
                }
            }
            Util.Alert($"Egg Moves: {movectr}\r\nMost Moves: {max} @ {spec}\r\nSTAB Count: {stab}");
        }
    }
}