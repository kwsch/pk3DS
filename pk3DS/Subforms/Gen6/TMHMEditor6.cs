using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;
using System.Linq;
using pk3DS.Core;

namespace pk3DS
{
    public partial class TMHMEditor6 : Form
    {
        public TMHMEditor6()
        {
            InitializeComponent();
            if (Main.ExeFSPath == null) { WinFormsUtil.Alert("No exeFS code to load."); Close(); }
            string[] files = Directory.GetFiles(Main.ExeFSPath);
            if (!File.Exists(files[0]) || !Path.GetFileNameWithoutExtension(files[0]).Contains("code")) { WinFormsUtil.Alert("No .code.bin detected."); Close(); }
            data = File.ReadAllBytes(files[0]);
            if (data.Length % 0x200 != 0) { WinFormsUtil.Alert(".code.bin not decompressed. Aborting."); Close(); }
            offset = Util.IndexOfBytes(data, Signature, 0x400000, 0) + 8;
            codebin = files[0];
            movelist[0] = "";
            SetupDGV();
            GetList();
            RandSettings.GetFormSettings(this, groupBox1.Controls);
        }

        private static readonly byte[] Signature = {0xD4, 0x00, 0xAE, 0x02, 0xAF, 0x02, 0xB0, 0x02};
        private readonly string codebin;
        private readonly string[] movelist = Main.Config.GetText(TextName.MoveNames);
        private readonly int offset = Main.Config.ORAS ? 0x004A67EE : 0x00464796; // Default
        private readonly byte[] data;
        private int dataoffset;

        private void GetDataOffset()
        {
            dataoffset = offset; // reset
        }

        private void SetupDGV()
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

        private List<ushort> tms = new();
        private List<ushort> hms = new();

        private void GetList()
        {
            tms = new List<ushort>();
            hms = new List<ushort>();
            dgvTM.Rows.Clear();

            GetDataOffset();
            for (int i = 0; i < 92; i++) // 1-92 TMs stored sequentially
                tms.Add(BitConverter.ToUInt16(data, dataoffset + (2 * i)));
            for (int i = 92; i < 92 + 5; i++)
                hms.Add(BitConverter.ToUInt16(data, dataoffset + (2 * i)));
            if (Main.Config.ORAS)
            {
                hms.Add(BitConverter.ToUInt16(data, dataoffset + (2 * 97)));
                for (int i = 98; i < 106; i++)
                    tms.Add(BitConverter.ToUInt16(data, dataoffset + (2 * i)));
                hms.Add(BitConverter.ToUInt16(data, dataoffset + (2 * 106)));
            }
            else
            {
                for (int i = 97; i < 105; i++)
                    tms.Add(BitConverter.ToUInt16(data, dataoffset + (2 * i)));
            }

            ushort[] tmlist = tms.ToArray();
            ushort[] hmlist = hms.ToArray();
            for (int i = 0; i < tmlist.Length; i++)
            { dgvTM.Rows.Add(); dgvTM.Rows[i].Cells[0].Value = (i + 1).ToString(); dgvTM.Rows[i].Cells[1].Value = movelist[tmlist[i]]; }
            for (int i = 0; i < hmlist.Length; i++)
            { dgvHM.Rows.Add(); dgvHM.Rows[i].Cells[0].Value = (i + 1).ToString(); dgvHM.Rows[i].Cells[1].Value = movelist[hmlist[i]]; }
        }

        private void SetList()
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
                Array.Copy(BitConverter.GetBytes(tmlist[i]), 0, data, offset + (2 * i), 2);
            for (int i = 92; i < 92 + 5; i++)
                Array.Copy(BitConverter.GetBytes(hmlist[i - 92]), 0, data, offset + (2 * i), 2);
            if (Main.Config.ORAS)
            {
                Array.Copy(BitConverter.GetBytes(hmlist[5]), 0, data, offset + (2 * 97), 2);
                for (int i = 98; i < 106; i++)
                    Array.Copy(BitConverter.GetBytes(tmlist[i - 6]), 0, data, offset + (2 * i), 2);
                Array.Copy(BitConverter.GetBytes(hmlist[6]), 0, data, offset + (2 * 106), 2);
            }
            else
            {
                for (int i = 97; i < 105; i++)
                    Array.Copy(BitConverter.GetBytes(tmlist[i - 5]), 0, data, offset + (2 * i), 2);
            }

            // Set Move Text Descriptions back into Item Text File
            string[] itemDescriptions = Main.Config.GetText(TextName.ItemFlavor);
            string[] moveDescriptions = Main.Config.GetText(TextName.MoveFlavor);
            for (int i = 1 - 1; i <= 92 - 1; i++) // TM01 - TM92
                itemDescriptions[328 + i] = moveDescriptions[tmlist[i]];
            for (int i = 93 - 1; i <= 95 - 1; i++) // TM92 - TM95
                itemDescriptions[618 + i - 92] = moveDescriptions[tmlist[i]];
            for (int i = 96 - 1; i <= 100 - 1; i++) // TM96 - TM100
                itemDescriptions[690 + i - 95] = moveDescriptions[tmlist[i]];
            for (int i = 1 - 1; i <= 5 - 1; i++) // HM01 - HM05
                itemDescriptions[420 + i] = moveDescriptions[hmlist[i]];
            if (Main.Config.ORAS)
            {
                itemDescriptions[425] = moveDescriptions[hmlist[5]]; // HM06
                itemDescriptions[737] = moveDescriptions[hmlist[6]]; // HM07
            }
            Main.Config.SetText(TextName.ItemFlavor, itemDescriptions);
        }

        private void Form_Closing(object sender, FormClosingEventArgs e)
        {
            SetList();
            File.WriteAllBytes(codebin, data);
            RandSettings.SetFormSettings(this, groupBox1.Controls);
        }

        private void B_RandomTM_Click(object sender, EventArgs e)
        {
            if (WinFormsUtil.Prompt(MessageBoxButtons.YesNo, "Randomize TMs? Cannot undo.", "Move compatibility will be the same as the base TMs.") != DialogResult.Yes) return;
            if (CHK_RandomizeHM.Checked && WinFormsUtil.Prompt(MessageBoxButtons.YesNo, "Randomizing HMs can halt story progression!", "Continue anyway?") != DialogResult.Yes) return;

            int[] randomMoves = Enumerable.Range(1, movelist.Length - 1).Select(i => i).ToArray();
            Util.Shuffle(randomMoves);

            int[] hm_xy = { 015, 019, 057, 070, 127 };
            int[] hm_ao = hm_xy.Concat(new[] { 249, 291 }).ToArray();
            int[] field = { 148, 249, 290 }; // TMs with field effects
            int[] banned = { 165, 621 }; // Struggle and Hyperspace Fury
            int ctr = 0;

            for (int i = 0; i < dgvTM.Rows.Count; i++)
            {
                // randomize all TMs
                if (CHK_RandomizeField.Checked)
                {
                    while (banned.Contains(randomMoves[ctr])) ctr++;
                    dgvTM.Rows[i].Cells[1].Value = movelist[randomMoves[ctr++]];
                }

                // randomize all TMs, no Field Moves
                else
                {
                    int val = Array.IndexOf(movelist, dgvTM.Rows[i].Cells[1].Value);
                    if (hm_xy.Contains(val) || hm_ao.Contains(val) || field.Contains(val)) continue; // skip banned moves
                    while (hm_xy.Contains(randomMoves[ctr]) || hm_ao.Contains(randomMoves[ctr]) || field.Contains(randomMoves[ctr]) || banned.Contains(randomMoves[ctr])) ctr++;
                    dgvTM.Rows[i].Cells[1].Value = movelist[randomMoves[ctr++]];
                }
            }

            if (CHK_RandomizeHM.Checked)
            {
                for (int j = 0; j < dgvHM.Rows.Count; j++)
                {
                    while (banned.Contains(randomMoves[ctr])) ctr++;
                    dgvHM.Rows[j].Cells[1].Value = movelist[randomMoves[ctr++]];
                }
            }
            WinFormsUtil.Alert("Randomized!");
        }

        internal static void GetTMHMList(out ushort[] TMs, out ushort[] HMs)
        {
            TMs = Array.Empty<ushort>();
            HMs = Array.Empty<ushort>();
            if (Main.ExeFSPath == null) return;
            string[] files = Directory.GetFiles(Main.ExeFSPath);
            if (!File.Exists(files[0]) || !Path.GetFileNameWithoutExtension(files[0]).Contains("code")) return;
            byte[] data = File.ReadAllBytes(files[0]);
            int dataoffset = Util.IndexOfBytes(data, Signature, 0x400000, 0) + 8;
            if (data.Length % 0x200 != 0) return;

            List<ushort> tms = new List<ushort>();
            List<ushort> hms = new List<ushort>();

            for (int i = 0; i < 92; i++) // 1-92 TMs stored sequentially
                tms.Add(BitConverter.ToUInt16(data, dataoffset + (2 * i)));
            for (int i = 92; i < 92 + 5; i++)
                hms.Add(BitConverter.ToUInt16(data, dataoffset + (2 * i)));
            if (Main.Config.ORAS)
            {
                hms.Add(BitConverter.ToUInt16(data, dataoffset + (2 * 97)));
                for (int i = 98; i < 106; i++)
                    tms.Add(BitConverter.ToUInt16(data, dataoffset + (2 * i)));
                hms.Add(BitConverter.ToUInt16(data, dataoffset + (2 * 106)));
            }
            else
            {
                for (int i = 97; i < 105; i++)
                    tms.Add(BitConverter.ToUInt16(data, dataoffset + (2 * i)));
            }

            TMs = tms.ToArray();
            HMs = hms.ToArray();
        }
    }
}
