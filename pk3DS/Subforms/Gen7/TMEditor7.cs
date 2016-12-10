using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;
using System.Linq;

namespace pk3DS
{
    public partial class TMEditor7 : Form
    {
        public TMEditor7()
        {
            InitializeComponent();
            if (Main.ExeFSPath == null) { Util.Alert("No exeFS code to load."); Close(); }
            string[] files = Directory.GetFiles(Main.ExeFSPath);
            if (!File.Exists(files[0]) || !Path.GetFileNameWithoutExtension(files[0]).Contains("code")) { Util.Alert("No .code.bin detected."); Close(); }
            data = File.ReadAllBytes(files[0]);
            if (data.Length % 0x200 != 0) { Util.Alert(".code.bin not decompressed. Aborting."); Close(); }
            offset = Util.IndexOfBytes(data, Signature, 0x400000, 0) + Signature.Length;
            codebin = files[0];
            movelist[0] = "";
            setupDGV();
            getList();
        }

        private static readonly byte[] Signature = {0x03, 0x40, 0x03, 0x41, 0x03, 0x42, 0x03, 0x43, 0x03}; // tail end of item::ITEM_CheckBeads
        private readonly string codebin;
        private readonly string[] movelist = Main.getText(TextName.MoveNames);
        private readonly int offset = 0x0059795A; // Default
        private readonly byte[] data;
        private int dataoffset;
        private void getDataOffset()
        {
            dataoffset = offset; // reset
        }
        private void setupDGV()
        {
            dgvTM.Columns.Clear();
            DataGridViewColumn dgvIndex = new DataGridViewTextBoxColumn();
            {
                dgvIndex.HeaderText = "Index";
                dgvIndex.DisplayIndex = 0;
                dgvIndex.Width = 45;
                dgvIndex.ReadOnly = true;
                dgvIndex.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                dgvIndex.SortMode = DataGridViewColumnSortMode.NotSortable;
            }
            DataGridViewComboBoxColumn dgvMove = new DataGridViewComboBoxColumn();
            {
                dgvMove.HeaderText = "Move";
                dgvMove.DisplayIndex = 1;
                foreach (string t in movelist)
                    dgvMove.Items.Add(t); // add only the Names

                dgvMove.Width = 133;
                dgvMove.FlatStyle = FlatStyle.Flat;
                dgvIndex.SortMode = DataGridViewColumnSortMode.NotSortable;
            }
            dgvTM.Columns.Add(dgvIndex);
            dgvTM.Columns.Add(dgvMove);
        }

        private List<ushort> tms = new List<ushort>();

        private void getList()
        {
            tms = new List<ushort>();
            dgvTM.Rows.Clear();

            getDataOffset();
            for (int i = 0; i < 100; i++) // TMs stored sequentially
                tms.Add(BitConverter.ToUInt16(data, dataoffset + 2 * i));

            ushort[] tmlist = tms.ToArray();
            for (int i = 0; i < tmlist.Length; i++)
            { dgvTM.Rows.Add(); dgvTM.Rows[i].Cells[0].Value = (i + 1).ToString(); dgvTM.Rows[i].Cells[1].Value = movelist[tmlist[i]]; }
        }
        private void setList()
        {
            // Gather TM/HM list.
            tms = new List<ushort>();
            for (int i = 0; i < dgvTM.Rows.Count; i++)
                tms.Add((ushort)Array.IndexOf(movelist, dgvTM.Rows[i].Cells[1].Value));

            ushort[] tmlist = tms.ToArray();

            // Set TM/HM list in
            for (int i = 0; i < 100; i++)
                Array.Copy(BitConverter.GetBytes(tmlist[i]), 0, data, offset + 2 * i, 2);
        }

        private void formClosing(object sender, FormClosingEventArgs e)
        {
            setList();
            File.WriteAllBytes(codebin, data);
        }

        private void B_RandomTM_Click(object sender, EventArgs e)
        {
            int[] randomMoves = Enumerable.Range(1, movelist.Length - 1).Select(i => i).ToArray();
            Util.Shuffle(randomMoves);

            int[] banned = Legal.Z_Moves;
            int ctr = 0;

            for (int i = 0; i < dgvTM.Rows.Count; i++)
            {
                int val = Array.IndexOf(movelist, dgvTM.Rows[i].Cells[1].Value);
                if (banned.Contains(val)) continue;
                while (banned.Contains(randomMoves[ctr])) ctr++;

                dgvTM.Rows[i].Cells[1].Value = movelist[randomMoves[ctr++]];
            }
        }

        internal static void getTMHMList(ref ushort[] TMs)
        {
            if (Main.ExeFSPath == null) return;
            string[] files = Directory.GetFiles(Main.ExeFSPath);
            if (!File.Exists(files[0]) || !Path.GetFileNameWithoutExtension(files[0]).Contains("code")) return;
            byte[] data = File.ReadAllBytes(files[0]);
            int dataoffset = Util.IndexOfBytes(data, Signature, 0x400000, 0) + Signature.Length;
            if (data.Length % 0x200 != 0) return;

            List<ushort> tms = new List<ushort>();

            for (int i = 0; i < 100; i++) // TMs stored sequentially
                tms.Add(BitConverter.ToUInt16(data, dataoffset + 2 * i));
            TMs = tms.ToArray();
        }
    }
}