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
    public partial class Tutors : Form
    {
        public Tutors()
        {
            InitializeComponent();
            if (Main.ExeFS == null) { Util.Alert("No exeFS code to load."); this.Close(); }
            string[] files = Directory.GetFiles(Main.ExeFS);
            if (!File.Exists(files[0]) || !Path.GetFileNameWithoutExtension(files[0]).Contains("code")) { Util.Alert("No .code.bin detected."); this.Close(); }
            data = File.ReadAllBytes(files[0]);
            if (data.Length % 0x200 != 0) { Util.Alert(".code.bin not decompressed. Aborting."); this.Close(); }
            codebin = files[0];
            movelist[0] = "";
            setupDGV();
            foreach (string s in locations) CB_Location.Items.Add(s);
            CB_Location.SelectedIndex = 0;
        }
        string codebin = null;
        string[] movelist = Main.getText((Main.oras) ? 14 : 13);
        byte[] data;
        byte[] entries = { 0xF, 0x11, 0x10, 0xF }; // Entries per Tutor
        int offset = 0x004960F8;
        int dataoffset = 0;
        string[] locations = { "1", "2", "3", "4" };
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
                for (int i = 0; i < movelist.Length; i++)
                    dgvMove.Items.Add(movelist[i]); // add only the Names

                dgvMove.Width = 135;
                dgvMove.FlatStyle = FlatStyle.Flat;
            }
            dgv.Columns.Add(dgvIndex);
            dgv.Columns.Add(dgvMove);
        }

        int entry = -1;
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
            ushort[] items = new ushort[count];
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
