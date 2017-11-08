using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;
using System.Linq;
using pk3DS.Core;

namespace pk3DS
{
    public partial class TMEditor7 : Form
    {
        public TMEditor7()
        {
            InitializeComponent();
            if (Main.ExeFSPath == null) { WinFormsUtil.Alert("No exeFS code to load."); Close(); }
            string[] files = Directory.GetFiles(Main.ExeFSPath);
            if (!File.Exists(files[0]) || !Path.GetFileNameWithoutExtension(files[0]).Contains("code")) { WinFormsUtil.Alert("No .code.bin detected."); Close(); }
            data = File.ReadAllBytes(files[0]);
            if (data.Length % 0x200 != 0) { WinFormsUtil.Alert(".code.bin not decompressed. Aborting."); Close(); }
            offset = Util.IndexOfBytes(data, Signature, 0x400000, 0) + Signature.Length;
            if (Main.Config.USUM)
                offset += 0x22;
            codebin = files[0];
            movelist[0] = "";
            setupDGV();
            getList();
        }

        private static readonly byte[] Signature = {0x03, 0x40, 0x03, 0x41, 0x03, 0x42, 0x03, 0x43, 0x03}; // tail end of item::ITEM_CheckBeads
        private readonly string codebin;
        private readonly string[] movelist = Main.Config.getText(TextName.MoveNames);
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

            // Set Move Text Descriptions back into Item Text File
            string[] itemDescriptions = Main.Config.getText(TextName.ItemFlavor);
            string[] moveDescriptions = Main.Config.getText(TextName.MoveFlavor);
            for (int i = 1 - 1; i <= 92 - 1; i++) // TM01 - TM92
                itemDescriptions[328 + i] = moveDescriptions[tmlist[i]];
            for (int i = 93 - 1; i <= 95 - 1; i++) // TM92 - TM95
                itemDescriptions[618 + i - 92] = moveDescriptions[tmlist[i]];
            for (int i = 96 - 1; i <= 100 - 1; i++) // TM96 - TM100
                itemDescriptions[690 + i - 95] = moveDescriptions[tmlist[i]];
            Main.Config.setText(TextName.ItemFlavor, itemDescriptions);
        }

        private void formClosing(object sender, FormClosingEventArgs e)
        {
            setList();
            File.WriteAllBytes(codebin, data);
        }

        private void B_RandomTM_Click(object sender, EventArgs e)
        {
            if (WinFormsUtil.Prompt(MessageBoxButtons.YesNo, "Randomize TMs?", "Move compatibility will be the same as the base TMs.") != DialogResult.Yes) return;

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
            WinFormsUtil.Alert("Randomized!");
        }

        internal static void getTMHMList(ref ushort[] TMs)
        {
            if (Main.ExeFSPath == null) return;
            string[] files = Directory.GetFiles(Main.ExeFSPath);
            if (!File.Exists(files[0]) || !Path.GetFileNameWithoutExtension(files[0]).Contains("code")) return;
            byte[] data = File.ReadAllBytes(files[0]);
            int dataoffset = Util.IndexOfBytes(data, Signature, 0x400000, 0) + Signature.Length;
            if (data.Length % 0x200 != 0) return;

            if (Main.Config.USUM)
                dataoffset += 0x22;
            List<ushort> tms = new List<ushort>();

            for (int i = 0; i < 100; i++) // TMs stored sequentially
                tms.Add(BitConverter.ToUInt16(data, dataoffset + 2 * i));
            TMs = tms.ToArray();
        }
    }
}