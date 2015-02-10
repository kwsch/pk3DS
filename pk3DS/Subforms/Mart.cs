using System;
using System.IO;
using System.Windows.Forms;

namespace pk3DS
{
    public partial class Mart : Form
    {
        public Mart()
        {
            InitializeComponent();
            if (Main.ExeFS == null) { Util.Alert("No exeFS code to load."); this.Close(); }
            string[] files = Directory.GetFiles(Main.ExeFS);
            if (!File.Exists(files[0]) || !Path.GetFileNameWithoutExtension(files[0]).Contains("code")) { Util.Alert("No .code.bin detected."); this.Close(); }
            data = File.ReadAllBytes(files[0]);
            if (data.Length % 0x200 != 0) { Util.Alert(".code.bin not decompressed. Aborting."); this.Close(); }
            codebin = files[0];
            itemlist[0] = "";
            setupDGV();
            foreach (string s in locations) CB_Location.Items.Add(s);
            CB_Location.SelectedIndex = 0;
        }
        string codebin = null;
        string[] itemlist = Main.getText((Main.oras) ? 114 : 96);
        byte[] data;
        byte[] entries = { 7, 6, 4, 3, 8, 
                             8, 3, 3, 4, 
                             3, 6, 6, 
                             7, 4 };
        int offset = 0x0047AB58;
        int dataoffset = 0;
        string[] locations = { "Slateport Market [Incenses]", "Slateport Market [Vitamins]", "Slateport Market [TMs]", "Rustboro Mart [Poke Balls]", "Slateport Mart [Misc]",
                               "Mauville Mart [TMs]", "Verdanturf Mart [Poke Balls]", "Fallarbor Mart [Poke Balls]", "Lavaridge Town [Herbs]", 
                               "Lilycove Dept Store, 2F Left [Run Away Items]", "Lilycove Dept Store, 3F Left [Vitamins]", "Lilycove Dept Store, 3F Right [Misc]",
                               "Lilycove Dept Store, 4F Left [Offensive TMs]", "Lilycove Dept Store, 4F Right [Defensive TMs]" };
        private void getDataOffset(int index)
        {
            dataoffset = offset; // reset
            for (int i = 0; i < index; i++)
                dataoffset += 2 * entries[i];
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
            DataGridViewComboBoxColumn dgvItem = new DataGridViewComboBoxColumn();
            {
                dgvItem.HeaderText = "Item";
                dgvItem.DisplayIndex = 1;
                for (int i = 0; i < itemlist.Length; i++)
                    dgvItem.Items.Add(itemlist[i]); // add only the Names

                dgvItem.Width = 135;
                dgvItem.FlatStyle = FlatStyle.Flat;
            }
            dgv.Columns.Add(dgvIndex);
            dgv.Columns.Add(dgvItem);
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
                dgv.Rows[i].Cells[1].Value = itemlist[BitConverter.ToUInt16(data, dataoffset + 2 * i)];
            }
        }
        private void setList()
        {
            int count = dgv.Rows.Count;
            for (int i = 0; i < count; i++)
                Array.Copy(BitConverter.GetBytes((ushort)Array.IndexOf(itemlist, dgv.Rows[i].Cells[1].Value)), 0, data, dataoffset + 2 * i, 2);
        }

        private void formClosing(object sender, FormClosingEventArgs e)
        {
            if (entry > -1) setList();
            File.WriteAllBytes(codebin, data);
        }
    }
}