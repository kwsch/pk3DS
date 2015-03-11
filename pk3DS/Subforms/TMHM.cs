using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;
using System.Linq;

namespace pk3DS
{
    public partial class TMHM : Form
    {
        public TMHM()
        {
            InitializeComponent();
            if (Main.ExeFS == null) { Util.Alert("No exeFS code to load."); Close(); }
            string[] files = Directory.GetFiles(Main.ExeFS);
            if (!File.Exists(files[0]) || !Path.GetFileNameWithoutExtension(files[0]).Contains("code")) { Util.Alert("No .code.bin detected."); Close(); }
            data = File.ReadAllBytes(files[0]);
            if (data.Length % 0x200 != 0) { Util.Alert(".code.bin not decompressed. Aborting."); Close(); }
            codebin = files[0];
            movelist[0] = "";
            setupDGV();
            getList();
        }
        string codebin;
        string[] movelist = Main.getText((Main.oras) ? 14 : 13);
        int offset = (Main.oras) ? 0x004A67EE : 0x00464796;
        byte[] data;
        int dataoffset;
        private void getDataOffset()
        {
            dataoffset = offset; // reset
        }
        private void setupDGV()
        {
            dgvTM.Columns.Clear(); dgvHM.Columns.Clear();
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
            dgvHM.Columns.Add((DataGridViewColumn)dgvIndex.Clone());
            dgvHM.Columns.Add((DataGridViewColumn)dgvMove.Clone());
        }

        List<ushort> tms = new List<ushort>();
        List<ushort> hms = new List<ushort>();

        private void getList()
        {
            tms = new List<ushort>();
            hms = new List<ushort>();
            dgvTM.Rows.Clear();

            getDataOffset();
            for (int i = 0; i < 92; i++) // 1-92 TMs stored sequentially
                tms.Add(BitConverter.ToUInt16(data, dataoffset + 2 * i));
            for (int i = 92; i < 92 + 5; i++)
                hms.Add(BitConverter.ToUInt16(data, dataoffset + 2 * i));
            if (Main.oras)
            {
                hms.Add(BitConverter.ToUInt16(data, dataoffset + 2 * 97));
                for (int i = 98; i < 106; i++)
                    tms.Add(BitConverter.ToUInt16(data, dataoffset + 2 * i));
                hms.Add(BitConverter.ToUInt16(data, dataoffset + 2 * 106));
            }
            else
            {
                for (int i = 97; i < 105; i++)
                    tms.Add(BitConverter.ToUInt16(data, dataoffset + 2 * i));
            }

            ushort[] tmlist = tms.ToArray();
            ushort[] hmlist = hms.ToArray();
            for (int i = 0; i < tmlist.Length; i++)
            { dgvTM.Rows.Add(); dgvTM.Rows[i].Cells[0].Value = (i + 1).ToString(); dgvTM.Rows[i].Cells[1].Value = movelist[tmlist[i]]; }
            for (int i = 0; i < hmlist.Length; i++)
            { dgvHM.Rows.Add(); dgvHM.Rows[i].Cells[0].Value = (i + 1).ToString(); dgvHM.Rows[i].Cells[1].Value = movelist[hmlist[i]]; }
        }
        private void setList()
        {
            tms = new List<ushort>();
            hms = new List<ushort>();
            for (int i = 0; i < dgvTM.Rows.Count; i++)
                tms.Add((ushort)Array.IndexOf(movelist, dgvTM.Rows[i].Cells[1].Value));

            for (int i = 0; i < dgvHM.Rows.Count; i++)
                hms.Add((ushort)Array.IndexOf(movelist, dgvHM.Rows[i].Cells[1].Value));

            ushort[] tmlist = tms.ToArray();
            ushort[] hmlist = hms.ToArray();

            for (int i = 0; i < 92; i++)
                Array.Copy(BitConverter.GetBytes(tmlist[i]), 0, data, offset + 2 * i, 2);
            for (int i = 92; i < 92 + 5; i++)
                Array.Copy(BitConverter.GetBytes(hmlist[i - 92]), 0, data, offset + 2 * i, 2);
            if (Main.oras)
            {
                Array.Copy(BitConverter.GetBytes(hmlist[5]), 0, data, offset + 2 * 97, 2);
                for (int i = 98; i < 106; i++)
                    Array.Copy(BitConverter.GetBytes(tmlist[i - 6]), 0, data, offset + 2 * i, 2);
                Array.Copy(BitConverter.GetBytes(hmlist[6]), 0, data, offset + 2 * 106, 2);
            }
            else
            {
                for (int i = 97; i < 105; i++)
                    Array.Copy(BitConverter.GetBytes(tmlist[i - 5]), 0, data, offset + 2 * i, 2);
            }
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

            int[] banned = { 15, 19, 57, 70, 127, 249, 291, 148 };
            int ctr = 0;

            for (int i = 0; i < dgvTM.Rows.Count; i++)
            {
                int val = Array.IndexOf(movelist, dgvTM.Rows[i].Cells[1].Value);
                if (banned.Contains(val)) continue;
                while (banned.Contains(randomMoves[ctr])) ctr++;

                dgvTM.Rows[i].Cells[1].Value = movelist[randomMoves[ctr++]];
            }
        }
    }
}