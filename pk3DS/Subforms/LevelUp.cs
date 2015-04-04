using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Media;
using System.Text;
using System.Windows.Forms;

namespace pk3DS
{
    public partial class LevelUp : Form
    {
        public LevelUp()
        {
            InitializeComponent();

            specieslist = Personal.getSpeciesIndexStrings(Main.oras);

            specieslist[0] = movelist[0] = "";

            sortedmoves = (string[])movelist.Clone();
            Array.Sort(sortedmoves);

            // Sort Species list but only for the regular dex entries.
            string[] sortedspecies = (string[])specieslist.Clone();
            Array.Resize(ref sortedspecies, 722);
            Array.Sort(sortedspecies);
            Array.Resize(ref sortedspecies, specieslist.Length);
            Array.Copy(specieslist, 722, sortedspecies, 722, specieslist.Length - 722);
            
            foreach (string s in sortedspecies) CB_Species.Items.Add(s);
            CB_Species.Items.RemoveAt(0);

            setupDGV();
            CB_Species.SelectedIndex = 0;
        }
        private string[] files = Directory.GetFiles("levelup");
        private int entry = -1;
        private string[] movelist = Main.getText((Main.oras) ? 14 : 13);
        private string[] sortedmoves;
        private string[] specieslist = Main.getText((Main.oras) ? 98 : 80);
        bool dumping;
        private void setupDGV()
        {
            DataGridViewColumn dgvLevel = new DataGridViewTextBoxColumn();
            {
                dgvLevel.HeaderText = "Level";
                dgvLevel.DisplayIndex = 0;
                dgvLevel.Width = 45;
                dgvLevel.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            }
            DataGridViewComboBoxColumn dgvMove = new DataGridViewComboBoxColumn();
            {
                dgvMove.HeaderText = "Move";
                dgvMove.DisplayIndex = 1;
                for (int i = 0; i < movelist.Length; i++)
                    dgvMove.Items.Add(sortedmoves[i]); // add only the Names

                dgvMove.Width = 135;
                dgvMove.FlatStyle = FlatStyle.Flat;
            }
            dgv.Columns.Add(dgvLevel);
            dgv.Columns.Add(dgvMove);
        }
        private void getList()
        {
            entry = Array.IndexOf(specieslist, CB_Species.Text);
            dgv.Rows.Clear();
            byte[] input = File.ReadAllBytes(files[entry]);
            if (input.Length <= 4) { File.WriteAllBytes(files[entry], BitConverter.GetBytes(-1)); return; }

            List<short> moves = new List<short>();
            List<byte> levels = new List<byte>();
            for (int i = 0; i < (input.Length / 4) - 1; i++)
            {
                short move = BitConverter.ToInt16(input, i * 4);
                if (move < 0) continue;

                moves.Add(move);
                levels.Add(input[i * 4 + 2]);
            }
            short[] moveList = moves.ToArray();
            byte[] levelList = levels.ToArray();

            dgv.Rows.Add(moveList.Length);

            // Fill Entries
            for (int i = 0; i < moveList.Length; i++)
            {
                dgv.Rows[i].Cells[0].Value = levelList[i];
                dgv.Rows[i].Cells[1].Value = movelist[moveList[i]];
            }

            dgv.CancelEdit();
        }
        private void setList()
        {
            if (entry < 1 || dumping) return;
            List<ushort> moves = new List<ushort>();
            List<ushort> levels = new List<ushort>();
            for (int i = 0; i < dgv.Rows.Count - 1; i++)
            {
                int move = Array.IndexOf(movelist, dgv.Rows[i].Cells[1].Value);
                if (move < 1) continue;

                moves.Add((ushort)move);
                string level = (dgv.Rows[i].Cells[0].Value ?? 0).ToString();
                ushort lv;
                UInt16.TryParse(level, out lv);
                if (lv > 100) lv = 100;
                else if (lv == 0) lv = 1;
                levels.Add(lv);
            }
            ushort[] movevals = moves.ToArray(); if (movevals.Length == 0) { File.WriteAllBytes(files[entry], BitConverter.GetBytes(-1)); return; }
            ushort[] levelvals = levels.ToArray();

            byte[] data = new byte[4 + 4 * movevals.Length];
            for (int i = 0; i < movevals.Length; i++)
            {
                Array.Copy(BitConverter.GetBytes(movevals[i]), 0, data, 4 * i, 2);
                Array.Copy(BitConverter.GetBytes(levelvals[i]), 0, data, 2 + 4 * i, 2);
            }

            // Cap data
            Array.Copy(BitConverter.GetBytes(-1), 0, data, data.Length - 4, 4);

            File.WriteAllBytes(files[entry], data);
        }

        private void changeEntry(object sender, EventArgs e)
        {
            setList();
            getList();
        }

        private void B_RandAll_Click(object sender, EventArgs e)
        {
            // ORAS: 10682 moves learned on levelup/birth. 
            // 5593 are STAB. 52.3% are STAB. 
            // Steelix learns the most @ 25 (so many level 1)!
            Random rnd = new Random();

            int[] firstMoves = { 1, 40, 52, 55, 64, 71, 84, 98, 122, 141 };
            // Pound, Poison Sting, Ember, Water Gun, Peck, Absorb, Thunder Shock, Quick Attack, Lick, Leech Life

            ushort[] HMs = { 15, 19, 57, 70, 127, 249, 291 };
            ushort[] TMs = {};
            if (CHK_HMs.Checked && Main.ExeFS != null)
                TMHM.getTMHMList(Main.oras, ref TMs, ref HMs);

            int[] banned = new int[HMs.Length];
            for (int i = 0; i < banned.Length; i++) 
                banned[i] = HMs[i];

            // Move Stats
            int[] moveTypes = Moves.getTypes();

            // Personal Stats
            byte[] personalData = File.ReadAllBytes(Directory.GetFiles("personal").Last());

            // Set up Randomized Moves
            int[] randomMoves = Enumerable.Range(1, movelist.Length - 1).Select(i => i).ToArray();
            Util.Shuffle(randomMoves);
            int ctr = 0;

            for (int i = 0; i < CB_Species.Items.Count; i++)
            {
                CB_Species.SelectedIndex = i; // Get new Species
                int count = dgv.Rows.Count - 1;

                if (CHK_Expand.Checked && (int)NUD_Moves.Value > count)
                    dgv.Rows.AddCopies(count, (int)NUD_Moves.Value - count);

                // Default First Move
                dgv.Rows[0].Cells[0].Value = 1;
                dgv.Rows[0].Cells[1].Value = movelist[firstMoves[rnd.Next(0, firstMoves.Length)]];
                for (int j = 1; j < dgv.Rows.Count - 1; j++)
                {
                    // Assign New Moves
                    bool forceSTAB = (CHK_STAB.Checked && rnd.Next(0, 99) < NUD_STAB.Value);
                    int move = Randomizer.getRandomSpecies(ref randomMoves, ref ctr);
                    while ( // Move is invalid
                        (!CHK_HMs.Checked && banned.Contains(move)) // HM Moves Not Allowed
                        || (forceSTAB && // STAB is required
                            !(
                            moveTypes[move] == personalData[6 + (Main.oras ? 0x50 : 0x40) * i] // Type 1
                            ||
                            moveTypes[move] == personalData[7 + (Main.oras ? 0x50 : 0x40) * i] // Type 2
                            )
                            )
                            )
                            { move = Randomizer.getRandomSpecies(ref randomMoves, ref ctr); }

                    // Assign Move
                    dgv.Rows[j].Cells[1].Value = movelist[move];
                    // Assign Level
                    if (j >= count)
                    {
                        string level = (dgv.Rows[count - 1].Cells[0].Value ?? 0).ToString();
                        ushort lv;
                        UInt16.TryParse(level, out lv);
                        if (lv > 100) lv = 100;
                        else if (lv == 0) lv = 1;
                        dgv.Rows[j].Cells[0].Value = lv + (j - count) + 1;
                    }
                    if (CHK_Spread.Checked)
                        dgv.Rows[j].Cells[0].Value = (j * (NUD_Level.Value / (dgv.Rows.Count - 1))).ToString();
                }
            }
            CB_Species.SelectedIndex = 0;
            Util.Alert("All Pokemon's Level Up Moves have been randomized!");
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
                    result += dgv.Rows[j].Cells[0].Value + " - " + dgv.Rows[j].Cells[1].Value + Environment.NewLine;

                result += Environment.NewLine;
            }
            SaveFileDialog sfd = new SaveFileDialog {FileName = "Level Up Moves.txt", Filter = "Text File|*.txt"};

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

        private void CHK_TypeBias_CheckedChanged(object sender, EventArgs e)
        {
            NUD_STAB.Enabled = CHK_STAB.Checked;
            NUD_STAB.Value = Convert.ToDecimal(CHK_STAB.Checked) * 20;
        }

        public void calcStats() // Debug Function
        {
            int[] moveTypes = Moves.getTypes();

            byte[] personalData = File.ReadAllBytes(Directory.GetFiles("personal").Last());

            int movectr = 0;
            int max = 0;
            int spec = 0;
            int stab = 0;
            for (int i = 0; i < 722; i++)
            {
                byte[] movedata = File.ReadAllBytes(files[i]);
                int movecount = (movedata.Length - 4) / 4;
                if (movecount == 65535)
                    continue;
                movectr += movecount; // Average Moves
                if (max < movecount) { max = movecount; spec = i; } // Max Moves (and species)
                for (int m = 0; m < movedata.Length / 4; m++)
                {
                    int move = BitConverter.ToUInt16(movedata, m*4);
                    if (move == 65535)
                    {
                        movectr--;
                        continue;
                    }
                    int movetype = moveTypes[move];
                    if (movetype == personalData[6 + (Main.oras ? 0x50 : 0x40)*i] ||
                        movetype == personalData[7 + (Main.oras ? 0x50 : 0x40)*i])
                        stab++;
                }
            }
            Util.Alert(String.Format("Moves Learned: {0}\r\nMost Learned: {1} @ {2}\r\nStab Count: {3}", 
                movectr, max,
                spec, stab));
        }
    }
}