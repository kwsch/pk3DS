using pk3DS.Core;
using System;
using System.IO;
using System.Windows.Forms;

namespace pk3DS
{
    public partial class TutorEditor6 : Form
    {
        public TutorEditor6()
        {
            InitializeComponent();
            if (Main.ExeFSPath == null) { WinFormsUtil.Alert("No exeFS code to load."); Close(); }
            string[] files = Directory.GetFiles(Main.ExeFSPath);
            if (!File.Exists(files[0]) || !Path.GetFileNameWithoutExtension(files[0]).Contains("code")) { WinFormsUtil.Alert("No .code.bin detected."); Close(); }
            data = File.ReadAllBytes(files[0]);
            if (data.Length % 0x200 != 0) { WinFormsUtil.Alert(".code.bin not decompressed. Aborting."); Close(); }
            offset = GetDataOffset(data);
            codebin = files[0];
            movelist[0] = "";
            setupDGV();
            foreach (string s in locations) CB_Location.Items.Add(s);
            CB_Location.SelectedIndex = 0;
            WinFormsUtil.Alert("Changes made do not reflect ingame.", "Still needs more research.");
        }

        private static int GetDataOffset(byte[] data)
        {
            byte[] vanilla =
            {
                0x00, 0x46, 0x6F, 0x72, 0x6D, 0x61, 0x74, 0x54, 0x79, 0x70, 0x65, 0x00, 0x00, 0x45, 0x64, 0x67,
                0x65, 0x49, 0x44, 0x00, 0xFF
            };
            int offset = Util.IndexOfBytes(data, vanilla, 0x400000, 0);
            if (offset >= 0)
                return offset + vanilla.Length;

            byte[] patched =
            {
                0x00, 0x46, 0x6F, 0x72, 0x6D, 0x61, 0x74, 0x54, 0x79, 0x70, 0x65, 0x00, 0x00, 0x45, 0x64, 0x67,
                0x65, 0x49, 0x44, 0x00, 0x00, 0x63, 0x3A, 0x5C, 0x72, 0x65, 0x76, 0x69, 0x73, 0x69, 0x6F, 0x6E,
                0x31, 0x5F, 0x73, 0x61, 0x6E, 0x67, 0x6F, 0x5C, 0x73, 0x61, 0x6E, 0x67, 0x6F, 0x5F, 0x70, 0x72,
                0x6F, 0x6A, 0x65, 0x63, 0x74, 0x5C, 0x70, 0x72, 0x6F, 0x67, 0x5C, 0x73, 0x72, 0x63, 0x2F, 0x73,
                0x79, 0x73, 0x74, 0x65, 0x6D, 0x2F, 0x6D, 0x6F, 0x74, 0x69, 0x6F, 0x6E, 0x2F, 0x4D, 0x6F, 0x74,
                0x69, 0x6F, 0x6E, 0x2E, 0x63, 0x70, 0x70, 0x00, 0x00
            };
            offset = Util.IndexOfBytes(data, patched, 0x400000, 0);

            if (offset >= 0)
                return offset + patched.Length;

            return -1;
        }

        private readonly string codebin;
        private readonly string[] movelist = Main.Config.getText(TextName.MoveNames);
        private readonly byte[] data;
        private readonly byte[] entries = { 0xF, 0x11, 0x10, 0xF }; // Entries per Tutor
        private readonly int offset;
        private int dataoffset;
        readonly string[] locations = { "1", "2", "3", "4" };
        private void getDataOffset(int index)
        {
            dataoffset = offset; // reset
            for (int i = 0; i < index; i++)
                dataoffset += 2 * entries[i] + 2; // There's a EndCap
        }
        private void setupDGV()
        {
            DataGridViewColumn dgvIndex = new DataGridViewTextBoxColumn();
            {
                dgvIndex.HeaderText = "Index";
                dgvIndex.DisplayIndex = 0;
                dgvIndex.Width = 45;
                dgvIndex.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            }
            DataGridViewComboBoxColumn dgvMove = new DataGridViewComboBoxColumn();
            {
                dgvMove.HeaderText = "Move";
                dgvMove.DisplayIndex = 1;
                foreach (string t in movelist)
                    dgvMove.Items.Add(t); // add only the Names

                dgvMove.Width = 135;
                dgvMove.FlatStyle = FlatStyle.Flat;
            }
            dgv.Columns.Add(dgvIndex);
            dgv.Columns.Add(dgvMove);
        }

        private int entry = -1;
        private void changeIndex(object sender, EventArgs e)
        {
            if (entry > -1) setList();
            entry = CB_Location.SelectedIndex;
            getList();
        }
        private void getList()
        {
            dgv.Rows.Clear();
            int count = entries[entry];
            dgv.Rows.Add(count);
            getDataOffset(entry);
            for (int i = 0; i < count; i++)
            {
                dgv.Rows[i].Cells[0].Value = i.ToString();
                dgv.Rows[i].Cells[1].Value = movelist[BitConverter.ToUInt16(data, dataoffset + 2 * i)];
            }
        }
        private void setList()
        {
            int count = dgv.Rows.Count;
            for (int i = 0; i < count; i++)
                Array.Copy(BitConverter.GetBytes((ushort)Array.IndexOf(movelist, dgv.Rows[i].Cells[1].Value)), 0, data, dataoffset + 2 * i, 2);
        }

        private void formClosing(object sender, FormClosingEventArgs e)
        {
            if (entry > -1) setList();
            File.WriteAllBytes(codebin, data);
        }
    }
}