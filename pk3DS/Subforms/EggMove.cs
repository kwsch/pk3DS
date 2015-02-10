using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;

namespace pk3DS
{
    public partial class EggMove : Form
    {
        public EggMove()
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
        private string[] files = Directory.GetFiles("eggmove");
        private int entry = -1;
        private string[] movelist = Main.getText((Main.oras) ? 14 : 13);
        private string[] specieslist = Main.getText((Main.oras) ? 98 : 80);
        private string[] sortedmoves;
        private string[] sortedspecies;
        bool dumping = false;
        private void setupDGV()
        {
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
        private void getList()
        {
            entry = Array.IndexOf(specieslist, CB_Species.Text);
            dgv.Rows.Clear();
            byte[] input = File.ReadAllBytes(files[entry]);
            if (input.Length == 0) return;
            int count = BitConverter.ToUInt16(input, 0);
            if (count < 1) { File.WriteAllBytes(files[entry], new byte[0]); return; }
            dgv.Rows.Add(count);

            // Fill Entries
            for (int i = 0; i < count; i++)
                dgv.Rows[i].Cells[0].Value = movelist[BitConverter.ToUInt16(input, 2 + i * 2)];

            dgv.CancelEdit();
        }
        private void setList()
        {
            if (entry < 1 || dumping) return;
            List<ushort> moves = new List<ushort>();
            for (int i = 0; i < dgv.Rows.Count - 1; i++)
            {
                int move = Array.IndexOf(movelist, dgv.Rows[i].Cells[0].Value);
                if (move > 0) moves.Add((ushort)move);
            }
            ushort[] movevals = moves.ToArray(); if (movevals.Length == 0) { File.WriteAllBytes(files[entry], new byte[0]); return; }
            byte[] movedata = new byte[2 + movevals.Length * 2];
            Array.Copy(BitConverter.GetBytes((ushort)movevals.Length), movedata, 2);
            for (int i = 0; i < movevals.Length; i++)
                Array.Copy(BitConverter.GetBytes((ushort)movevals[i]), 0, movedata, 2 + 2 * i, 2);

            File.WriteAllBytes(files[entry], movedata);
        }

        private void changeEntry(object sender, EventArgs e)
        {
            setList();
            getList();
        }

        private void B_RandAll_Click(object sender, EventArgs e)
        {
            Random rnd = new Random();
            for (int i = 0; i < CB_Species.Items.Count; i++)
            {
                CB_Species.SelectedIndex = i; // Get new Species
                getList();
                int count = dgv.Rows.Count - 1;
                for (int j = 0; j < count; j++)
                    dgv.Rows[j].Cells[0].Value = movelist[rnd.Next(1, movelist.Length)];
            }
            setList();
            Util.Alert("All Pokemon's Egg Moves have been randomized!");
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
            SaveFileDialog sfd = new SaveFileDialog();
            sfd.FileName = "Egg Moves.txt";
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