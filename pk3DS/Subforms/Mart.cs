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
            if (Main.ExeFSPath == null) { Util.Alert("No exeFS code to load."); Close(); }
            string[] files = Directory.GetFiles(Main.ExeFSPath);
            if (!File.Exists(files[0]) || !Path.GetFileNameWithoutExtension(files[0]).Contains("code")) { Util.Alert("No .code.bin detected."); Close(); }
            data = File.ReadAllBytes(files[0]);
            if (data.Length % 0x200 != 0) { Util.Alert(".code.bin not decompressed. Aborting."); Close(); }
            offset = Util.IndexOfBytes(data, new byte[] { 0x00, 0x72, 0x6F, 0x6D, 0x3A, 0x2F, 0x44, 0x6C, 0x6C, 0x53, 0x74, 0x61, 0x72, 0x74, 0x4D, 0x65, 0x6E, 0x75, 0x2E, 0x63, 0x72, 0x6F, 0x00 }, 0x400000, 0) + 0x17;
            codebin = files[0];
            itemlist[0] = "";
            setupDGV();
            foreach (string s in locations) CB_Location.Items.Add(s);
            CB_Location.SelectedIndex = 0;
        }

        private readonly string codebin;
        private readonly string[] itemlist = Main.getText(Main.oras ? 114 : 96);
        private readonly byte[] data;

        private readonly byte[] entries = Main.oras
            ? new byte[] // ORAS
            {
                3, 10, 14, 17, 18, 19, 19, 19, 19, // General
                3, // Unused
                7, 6, 4, 3, 8,
                8, 3, 3, 4,
                3, 6, 6,
                7, 4
            }
            : new byte[] // XY
            {
                3, 10, 14, 17, 18, 19, 19, 19, 19, // General
                1, // Unused
                4, 10, 3, 9, 1, 1, // Misc
                3, 3, // Balls
                5, 5, // TMs
                6, // Vitamins
                7, // Balls
                5, // TMs
                5, // TMs
                8, // Battle
                3, // Balls
            };

        private readonly int offset = Main.oras ? 0x0047AB58 : 0x0043C89E;
        private int dataoffset;

        readonly string[] locations = Main.oras 
            ? new[] // ORAS
            { 
                "No Badges", "1 Badge", "2 Badges", "3 Badges", "4 Badges", "5 Badges", "6 Badges", "7 Badges", "8 Badges",
                "Unused",
                "Slateport Market [Incenses]", "Slateport Market [Vitamins]", "Slateport Market [TMs]", "Rustboro Mart [Poke Balls]", "Slateport Mart [Misc]",
                "Mauville Mart [TMs]", "Verdanturf Mart [Poke Balls]", "Fallarbor Mart [Poke Balls]", "Lavaridge Town [Herbs]", 
                "Lilycove Dept Store, 2F Left [Run Away Items]", "Lilycove Dept Store, 3F Left [Vitamins]", "Lilycove Dept Store, 3F Right [Misc]",
                "Lilycove Dept Store, 4F Left [Offensive TMs]", "Lilycove Dept Store, 4F Right [Defensive TMs]" 
            }
            : new[] // XY
            {
                "No Badges", "1 Badge", "2 Badges", "3 Badges", "4 Badges", "5 Badges", "6 Badges", "7 Badges", "8 Badges",
                "Unused",
                "Herbs", "Balls", "Stones", "Incence", "Aquacorde Balls", "Aquacorde Potion",
                "Lumiose North Boulevard [Balls]", "Cyllage City [Balls]", 
                "Shalour City [TMs]", "Lumiose South Boulevard [TMs]",
                "Laverre City [Vitamins]",
                "Snowbelle City [Balls]",
                "Kiloude City [TMs]",
                "Anistar City [TMs]",
                "Santalune City [X-Stat]",
                "Coumarine City [Balls]"
            };

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
                foreach (string t in itemlist)
                    dgvItem.Items.Add(t); // add only the Names

                dgvItem.Width = 135;
                dgvItem.FlatStyle = FlatStyle.Flat;
            }
            dgv.Columns.Add(dgvIndex);
            dgv.Columns.Add(dgvItem);
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
                dgv.Rows[i].Cells[1].Value = itemlist[BitConverter.ToUInt16(data, dataoffset + 2 * i)];
            }
        }
        private void setList()
        {
            int count = dgv.Rows.Count;
            for (int i = 0; i < count; i++)
                Array.Copy(BitConverter.GetBytes((ushort)Array.IndexOf(itemlist, dgv.Rows[i].Cells[1].Value)), 0, data, dataoffset + 2 * i, 2);
        }

        private void B_Save_Click(object sender, EventArgs e)
        {
            if (entry > -1) setList();
            File.WriteAllBytes(codebin, data);
        }
        private void B_Cancel_Click(object sender, EventArgs e)
        {
            Close();
        }
        private void B_Randomize_Click(object sender, EventArgs e)
        {
            if (DialogResult.Yes != Util.Prompt(MessageBoxButtons.YesNoCancel, "Randomize mart inventories?"))
                return;

            int[] validItems = Randomizer.getRandomItemList(Main.oras);

            int ctr = 0;
            Util.Shuffle(validItems);

            for (int i = 0; i < CB_Location.Items.Count; i++)
            {
                CB_Location.SelectedIndex = i;
                for (int r = 0; r < dgv.Rows.Count; r++)
                {
                    dgv.Rows[r].Cells[1].Value = itemlist[validItems[ctr++]];
                    if (ctr <= validItems.Length) continue;
                    Util.Shuffle(validItems); ctr = 0;
                }
            }
        }
    }
}