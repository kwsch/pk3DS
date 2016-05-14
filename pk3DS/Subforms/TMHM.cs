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
            if (Main.ExeFSPath == null) { Util.Alert("No exeFS code to load."); Close(); }
            string[] files = Directory.GetFiles(Main.ExeFSPath);
            if (!File.Exists(files[0]) || !Path.GetFileNameWithoutExtension(files[0]).Contains("code")) { Util.Alert("No .code.bin detected."); Close(); }
            data = File.ReadAllBytes(files[0]);
            if (data.Length % 0x200 != 0) { Util.Alert(".code.bin not decompressed. Aborting."); Close(); }
            offset = Util.IndexOfBytes(data, new byte[] { 0xD4, 0x00, 0xAE, 0x02, 0xAF, 0x02, 0xB0, 0x02 }, 0x400000, 0) + 8;
            codebin = files[0];
            movelist[0] = "";
            setupDGV();
            getList();
        }

        private readonly string codebin;
        private readonly string[] movelist = Main.getText(Main.oras ? 14 : 13);
        private readonly int offset = Main.oras ? 0x004A67EE : 0x00464796; // Default
        private readonly byte[] data;
        private int dataoffset;
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

        private List<ushort> tms = new List<ushort>();
        private List<ushort> hms = new List<ushort>();

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
            // Gather TM/HM list.
            tms = new List<ushort>();
            hms = new List<ushort>();
            for (int i = 0; i < dgvTM.Rows.Count; i++)
                tms.Add((ushort)Array.IndexOf(movelist, dgvTM.Rows[i].Cells[1].Value));

            for (int i = 0; i < dgvHM.Rows.Count; i++)
                hms.Add((ushort)Array.IndexOf(movelist, dgvHM.Rows[i].Cells[1].Value));

            ushort[] tmlist = tms.ToArray();
            ushort[] hmlist = hms.ToArray();

            // Set TM/HM list in
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

            // Set Move Text Descriptions back into Item Text File
            int itemFile = Main.oras ? 117 : 99;
            string[] itemDescriptions = Main.getText(itemFile);
            string[] moveDescriptions = Main.getText(Main.oras ? 16 : 15);
            for (int i = 1 - 1; i <= 92 - 1; i++) // TM01 - TM92
                itemDescriptions[328 + i] = moveDescriptions[tmlist[i]];
            for (int i = 93 - 1; i <= 95 - 1; i++) // TM92 - TM95
                itemDescriptions[618 + i - 92] = moveDescriptions[tmlist[i]];
            for (int i = 96 - 1; i <= 100 - 1; i++) // TM96 - TM100
                itemDescriptions[690 + i - 95] = moveDescriptions[tmlist[i]];
            for (int i = 1 - 1; i <= 5 - 1; i++) // HM01 - HM05
                itemDescriptions[420 + i] = moveDescriptions[hmlist[i]];
            if (Main.oras)
            {
                itemDescriptions[425] = moveDescriptions[hmlist[5]]; // HM06
                itemDescriptions[737] = moveDescriptions[hmlist[6]]; // HM07
            }
            Main.setText(itemFile, itemDescriptions);
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

        internal static void getTMHMList(bool oras, ref ushort[] TMs, ref ushort[] HMs)
        {
            if (Main.ExeFSPath == null) return;
            string[] files = Directory.GetFiles(Main.ExeFSPath);
            if (!File.Exists(files[0]) || !Path.GetFileNameWithoutExtension(files[0]).Contains("code")) return;
            byte[] data = File.ReadAllBytes(files[0]);
            int dataoffset = Util.IndexOfBytes(data, new byte[] { 0xD4, 0x00, 0xAE, 0x02, 0xAF, 0x02, 0xB0, 0x02 }, 0x400000, 0) + 8;
            if (data.Length % 0x200 != 0) return;

            List<ushort> tms = new List<ushort>();
            List<ushort> hms = new List<ushort>();

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

            TMs = tms.ToArray();
            HMs = hms.ToArray();
        }
    }
}