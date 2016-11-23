using System;
using System.IO;
using System.Windows.Forms;

namespace pk3DS
{
    public partial class MartEditor7 : Form
    {
        private readonly string CROPath = Path.Combine(Main.RomFSPath, "Shop.cro");
        public MartEditor7()
        {
            if (!File.Exists(CROPath))
            {
                Util.Error("CRO does not exist! Closing.", CROPath);
                Close();
            }
            InitializeComponent();

            data = File.ReadAllBytes(CROPath);
            offset = Util.IndexOfBytes(data, Signature, 0x5000, 0) + Signature.Length;
            offsetBP = Util.IndexOfBytes(data, BPSignature, 0x5000, 0) + BPSignature.Length;
            
            itemlist[0] = "";
            setupDGV();
            foreach (string s in locations) CB_Location.Items.Add(s);
            foreach (string s in locationsBP) CB_LocationBP.Items.Add(s);
            CB_Location.SelectedIndex = 0;
            CB_LocationBP.SelectedIndex = 0;
        }
        
        private readonly string[] itemlist = Main.getText(TextName.ItemNames);
        private readonly byte[] data;

        #region Tables
        private readonly byte[] Signature = // Leadup to the Shop Data, the shop arrays are the 3rd data array in the rodata section.
        {
            0x2D, 0x00, 0x00, 0x00, 0x3B, 0x00, 0x00, 0x00, 0x2F, 0x00, 0x00, 0x00, 0x3D, 0x00, 0x00, 0x00,
            0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0xFF, 0xFF, 0xFF, 0xFF, 0x00, 0x00, 0x00, 0x00,
            0x10, 0x00, 0x00, 0x00, 0x0E, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x01, 0x00,
        };
        private readonly byte[] BPSignature = // 2 arrays after the regular shops, the BP shops start. Skip over the second one to get BP offset.
        {
            0x09, 0x0B, 0x0D, 0x0F, 0x11, 0x13, 0x14, 0x15, 0x09, 0x04, 0x08, 0x0C, 0x05, 0x04, 0x0B, 0x03,
            0x0A, 0x06, 0x0A, 0x06, 0x04, 0x05, 0x07, 0x01
        };
        private readonly byte[] entries =
        {
            9, 11, 13, 15, 17, 19, 20, 21, // Regular Mart
            9, // KoniKoni Incense
            4, // KoniKoni Herb
            8, // Hau'oli Battle
            12, // Route 2
            5, // Heahea
            4, // Royal Avenue
            11, // Route 8
            3, // Paniola
            10, // Malie TMs
            6, // Unused
            10, // Seafolk TM
            6, // KoniKoni TM
            4, // KoniKoni Jewelry
            5, // Thrifty 1
            7, // Thrifyt 2
            1, // Thrifty 3 (Souvenir)
        };
        private readonly string[] locations =
        {
            "No Trials", "1 Trial", "2 Trials", "3 Trials", "4 Trials", "5 Trials", "6 Trials", "7 Trials",
            "KoniKoni Incense",
            "KoniKoni Herb",
            "Hau'oli Battle",
            "Route 2",
            "Heahea",
            "Royal Avenue",
            "Route 8",
            "Paniola",
            "Malie TMs",
            "Unused",
            "Seafolk TM",
            "KoniKoni TM",
            "KoniKoni Jewelry",
            "Thrifty 1",
            "Thrifyt 2",
            "Thrifty 3 (Souvenir)"
        };

        private readonly int[] entriesBP =
        {
            8, // Royal 1 (Abil Capsule)
            7, // Royal 2
            18, // Royal 3
            12, // Tree 1
            21, // Tree 2
            16, // Tree 3
        };
        private readonly string[] locationsBP =
        {
            "Royal 1",
            "Royal 2",
            "Royal 3",
            "Battle Tree 1",
            "Battle Tree 2",
            "Battle Tree 3",
        };
        #endregion

        private void B_Save_Click(object sender, EventArgs e)
        {
            if (entry > -1) setList();
            if (entryBP > -1) setListBP();
            File.WriteAllBytes(CROPath, data);
        }
        private void B_Cancel_Click(object sender, EventArgs e)
        {
            Close();
        }

        private readonly int offset;
        private int dataoffset;
        private void getDataOffset(int index)
        {
            dataoffset = offset; // reset
            for (int i = 0; i < index; i++)
                dataoffset += 2 * entries[i];
        }

        private void setupDGV()
        {
            foreach (string t in itemlist)
                dgvItem.Items.Add(t); // add only the Names
            foreach (string t in itemlist)
                dgvItemBP.Items.Add(t); // add only the Names
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
        private void B_Randomize_Click(object sender, EventArgs e)
        {
            if (DialogResult.Yes != Util.Prompt(MessageBoxButtons.YesNoCancel, "Randomize mart inventories?"))
                return;

            int[] validItems = Randomizer.getRandomItemList();

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

        private void getDataOffsetBP(int index)
        {
            dataoffsetBP = offsetBP; // reset
            for (int i = 0; i < index; i++)
                dataoffsetBP += 4 * entriesBP[i];
        }
        private readonly int offsetBP;
        private int dataoffsetBP;
        private int entryBP = -1;
        private void changeIndexBP(object sender, EventArgs e)
        {
            if (entryBP > -1) setListBP();
            entryBP = CB_LocationBP.SelectedIndex;
            getListBP();
        }
        private void getListBP()
        {
            dgvbp.Rows.Clear();
            int count = entriesBP[entryBP];
            dgvbp.Rows.Add(count);
            getDataOffsetBP(entryBP);
            for (int i = 0; i < count; i++)
            {
                dgvbp.Rows[i].Cells[0].Value = i.ToString();
                dgvbp.Rows[i].Cells[1].Value = itemlist[BitConverter.ToUInt16(data, dataoffsetBP + 4 * i)];
                dgvbp.Rows[i].Cells[2].Value = BitConverter.ToUInt16(data, dataoffsetBP + 4 * i + 2).ToString();
            }
        }
        private void setListBP()
        {
            int count = dgvbp.Rows.Count;
            for (int i = 0; i < count; i++)
            {
                int item = Array.IndexOf(itemlist, dgvbp.Rows[i].Cells[1].Value);
                Array.Copy(BitConverter.GetBytes((ushort)item), 0, data, dataoffsetBP + 4 * i, 2);
                int price; string p = dgvbp.Rows[i].Cells[2].Value.ToString();
                if (int.TryParse(p, out price))
                    Array.Copy(BitConverter.GetBytes((ushort)price), 0, data, dataoffsetBP + 4 * i + 2, 2);
            }
        }
        private void B_RandomizeBP_Click(object sender, EventArgs e)
        {
            if (DialogResult.Yes != Util.Prompt(MessageBoxButtons.YesNoCancel, "Randomize BP inventories?"))
                return;

            int[] validItems = Randomizer.getRandomItemList();

            int ctr = 0;
            Util.Shuffle(validItems);

            for (int i = 0; i < CB_LocationBP.Items.Count; i++)
            {
                CB_LocationBP.SelectedIndex = i;
                for (int r = 0; r < dgvbp.Rows.Count; r++)
                {
                    dgvbp.Rows[r].Cells[1].Value = itemlist[validItems[ctr++]];
                    if (ctr <= validItems.Length) continue;
                    Util.Shuffle(validItems); ctr = 0;
                }
            }
        }
    }
}