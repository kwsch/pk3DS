using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;

namespace pk3DS
{
    public partial class LevelUp : Form
    {
        public LevelUp()
        {
            InitializeComponent();
            specieslist[0] = movelist[0] = "";
            Array.Resize(ref specieslist, 722);

            sortedmoves = (string[])movelist.Clone();
            Array.Sort(sortedmoves);
            sortedspecies = (string[])specieslist.Clone();
            Array.Sort(sortedspecies);

            foreach (string s in sortedspecies) CB_Species.Items.Add(s);
            CB_Species.Items.RemoveAt(0);

            setupDGV();
            CB_Species.SelectedIndex = 0;
        }
        private string[] files = Directory.GetFiles("levelup");
        private int entry = -1;
        private string[] movelist = Main.getText((Main.oras) ? 14 : 13);
        private string[] sortedmoves;
        private string[] sortedspecies;
        private string[] specieslist = Main.getText((Main.oras) ? 98 : 80);
        bool dumping = false;
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
            if (input.Length <= 4) { File.WriteAllBytes(files[entry], BitConverter.GetBytes((int)-1)); return; }

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
                string level = (string)dgv.Rows[i].Cells[0].Value.ToString();
                ushort lv = 1;
                UInt16.TryParse(level, out lv);
                if (lv > 100) lv = 100;
                else if (lv == 0) lv = 1;
                levels.Add(lv);
            }
            ushort[] movevals = moves.ToArray(); if (movevals.Length == 0) { File.WriteAllBytes(files[entry], BitConverter.GetBytes((int)-1)); return; }
            ushort[] levelvals = levels.ToArray();

            byte[] data = new byte[4 + 4 * movevals.Length];
            for (int i = 0; i < movevals.Length; i++)
            {
                Array.Copy(BitConverter.GetBytes((ushort)movevals[i]), 0, data, 4 * i, 2);
                Array.Copy(BitConverter.GetBytes((ushort)levelvals[i]), 0, data, 2 + 4 * i, 2);
            }

            // Cap data
            Array.Copy(BitConverter.GetBytes((int)-1), 0, data, data.Length - 4, 4);

            File.WriteAllBytes(files[entry], data);
        }

        private void changeEntry(object sender, EventArgs e)
        {
            setList();
            getList();
        }

        private void B_RandAll_Click(object sender, EventArgs e)
        {
            Random rnd = new Random();

            int[] firstMoves = new int[] { 1, 40, 52, 55, 64, 71, 84, 98, 122, 141 };
            // Pound, Poison Sting, Ember, Water Gun, Peck, Absorb, Thunder Shock, Quick Attack, Lick, Leech Life
            
            for (int i = 0; i < CB_Species.Items.Count; i++)
            {
                CB_Species.SelectedIndex = i; // Get new Species
                getList();
                int count = dgv.Rows.Count - 1;
                dgv.Rows[0].Cells[1].Value = movelist[firstMoves[rnd.Next(0, firstMoves.Length)]];
                for (int j = 1; j < count; j++)
                    dgv.Rows[j].Cells[1].Value = movelist[rnd.Next(1, movelist.Length)];
            }
            setList();
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
                    result += dgv.Rows[j].Cells[0].Value.ToString() + " - " + dgv.Rows[j].Cells[1].Value.ToString() + Environment.NewLine;

                result += Environment.NewLine;
            }
            SaveFileDialog sfd = new SaveFileDialog();
            sfd.FileName = "Level Up Moves.txt";
            sfd.Filter = "Text File|*.txt";

            System.Media.SystemSounds.Asterisk.Play();
            if (sfd.ShowDialog() == DialogResult.OK)
            {
                string path = sfd.FileName;
                File.WriteAllText(path, result, System.Text.Encoding.Unicode);
            }
            dumping = false;
        }

        private void formClosing(object sender, FormClosingEventArgs e)
        {
            setList();
        }
    }
}