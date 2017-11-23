using pk3DS.Core;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace pk3DS
{
    public partial class MartEditor7UU : Form
    {
        private readonly string CROPath = Path.Combine(Main.RomFSPath, "Shop.cro");
        public MartEditor7UU()
        {
            if (!File.Exists(CROPath))
            {
                WinFormsUtil.Error("CRO does not exist! Closing.", CROPath);
                Close();
            }
            InitializeComponent();

            
            data = File.ReadAllBytes(CROPath);
            len_BPTutor = data.Skip(0x52D2).Take(4).ToArray();
            len_BPItem = data.Skip(0x52D2 + 4).Take(7).ToArray();
            len_Items = data.Skip(0x52D2 + 4 + 7).TakeWhile(z => (sbyte) z > 0).ToArray();
            
            itemlist[0] = "";
            setupDGV();
            foreach (string s in locations) CB_Location.Items.Add(s);
            foreach (string s in locationsBP) CB_LocationBPItem.Items.Add(s);
            foreach (string s in locationsTutor) CB_LocationBPMove.Items.Add(s);
            CB_Location.SelectedIndex =
            CB_LocationBPItem.SelectedIndex =
            CB_LocationBPMove.SelectedIndex = 0;
        }
        private const int ofs_Item = 0x50BC;
        private const int ofs_BPItem = 0x52FA;
        private const int ofs_BPTutor = 0x54DE;
        private readonly byte[] len_Items;
        private readonly byte[] len_BPItem;
        private readonly byte[] len_BPTutor;

        private readonly string[] itemlist = Main.Config.getText(TextName.ItemNames);
        private readonly string[] movelist = Main.Config.getText(TextName.MoveNames);
        private readonly byte[] data;

        #region Tables
        private readonly string[] locations =
        {
            "No Trials", "1 Trial", "2 Trials", "3 Trials", "4 Trials", "5 Trials", "6 Trials", "7 Trials",
            "Konikoni City [Incenses]",
            "Konikoni City [Herbs]",
            "Hau'oli City [X Items]",
            "Route 2 [Misc]",
            "Heahea City [TMs]",
            "Royal Avenue [TMs]",
            "Route 8 [Misc]",
            "Paniola Town [Poké Balls]",
            "Malie City [TMs]",
            "Mount Hokulani [Vitamins]",
            "Seafolk Village [TMs]",
            "Konikoni City [TMs]",
            "Konikoni City [Stones]",
            "Thrifty Megamart 1 [Poké Balls]",
            "Thrifty Megamart 2 [Misc]",
            "Thrifty Megamart 3 [Strange Souvenir]",
            "Route 3 [X Items]",
            "Konikoni City [X Items]",
            "Tapu Village [X Items]",
            "Mount Lanakila [X Items]",
        };
        private readonly string[] locationsBP =
        {
            "Battle Royal Dome [Medicine]",
            "Battle Royal Dome [EV Training]",
            "Battle Royal Dome [Held Items]",
            "Battle Tree [Trade Evolution Items]",
            "Battle Tree [Held Items]",
            "Battle Tree [Mega Stones]",
            "Medicine"
        };
        private readonly string[] locationsTutor =
        {
            "Big Wave Beach",
            "Heahea Beach",
            "Ula'ula Beach",
            "Battle Tree",
        };
        #endregion

        private void B_Save_Click(object sender, EventArgs e)
        {
            if (entryItem > -1) setListItem();
            if (entryBPItem > -1) setListBPItem();
            if (entryBPMove > -1) setListBPMove();
            File.WriteAllBytes(CROPath, data);
            Close();
        }
        private void B_Cancel_Click(object sender, EventArgs e) => Close();

        private void setupDGV()
        {
            foreach (string t in itemlist)
                dgvItem.Items.Add(t); // add only the Names
            foreach (string t in itemlist)
                dgvItemBP.Items.Add(t); // add only the Names
            foreach (string t in movelist)
                dgvmvMove.Items.Add(t); // add only the Names
        }

        private int entryItem = -1;
        private int entryBPItem = -1;
        private int entryBPMove = -1;

        private void changeIndexItem(object sender, EventArgs e)
        {
            if (entryItem > -1) setListItem();
            entryItem = CB_Location.SelectedIndex;
            getListItem();
        }
        private void changeIndexBPItem(object sender, EventArgs e)
        {
            if (entryBPItem > -1) setListBPItem();
            entryBPItem = CB_LocationBPItem.SelectedIndex;
            getListBPItem();
        }
        private void changeIndexBPMove(object sender, EventArgs e)
        {
            if (entryBPMove > -1) setListBPMove();
            entryBPMove = CB_LocationBPMove.SelectedIndex;
            getListBPMove();
        }
        
        private void getListItem()
        {
            dgv.Rows.Clear();
            int count = len_Items[entryItem];
            dgv.Rows.Add(count);
            var ofs = ofs_Item + len_Items.Take(entryItem).Sum(z => z) * 2;
            for (int i = 0; i < count; i++)
            {
                dgv.Rows[i].Cells[0].Value = i.ToString();
                dgv.Rows[i].Cells[1].Value = itemlist[BitConverter.ToUInt16(data, ofs + 2 * i)];
            }
        }
        private void getListBPItem()
        {
            dgvbp.Rows.Clear();
            int count = len_BPItem[entryBPItem];
            dgvbp.Rows.Add(count);
            var ofs = ofs_BPItem + len_BPItem.Take(entryBPItem).Sum(z => z) * 4;
            for (int i = 0; i < count; i++)
            {
                dgvbp.Rows[i].Cells[0].Value = i.ToString();
                dgvbp.Rows[i].Cells[1].Value = itemlist[BitConverter.ToUInt16(data, ofs + 4 * i)];
                dgvbp.Rows[i].Cells[2].Value = BitConverter.ToUInt16(data, ofs + 4 * i + 2).ToString();
            }
        }
        private void getListBPMove()
        {
            dgvmv.Rows.Clear();
            int count = len_BPTutor[entryBPMove];
            dgvmv.Rows.Add(count);
            var ofs = ofs_BPTutor + len_BPTutor.Take(entryBPMove).Sum(z => z) * 4;
            for (int i = 0; i < count; i++)
            {
                dgvmv.Rows[i].Cells[0].Value = i.ToString();
                dgvmv.Rows[i].Cells[1].Value = movelist[BitConverter.ToUInt16(data, ofs + 4 * i)];
                dgvmv.Rows[i].Cells[2].Value = BitConverter.ToUInt16(data, ofs + 4 * i + 2).ToString();
            }
        }

        private void setListItem()
        {
            int count = dgv.Rows.Count;
            var ofs = ofs_Item + len_Items.Take(entryItem).Sum(z => z) * 2;
            for (int i = 0; i < count; i++)
                Array.Copy(BitConverter.GetBytes((ushort)Array.IndexOf(itemlist, dgv.Rows[i].Cells[1].Value)), 0, data, ofs + 2 * i, 2);
        }
        private void setListBPItem()
        {
            int count = dgvbp.Rows.Count;
            var ofs = ofs_BPItem + len_BPItem.Take(entryBPItem).Sum(z => z) * 4;
            for (int i = 0; i < count; i++)
            {
                int item = Array.IndexOf(itemlist, dgvbp.Rows[i].Cells[1].Value);
                Array.Copy(BitConverter.GetBytes((ushort)item), 0, data, ofs + 4 * i, 2);
                int price; string p = dgvbp.Rows[i].Cells[2].Value.ToString();
                if (int.TryParse(p, out price))
                    Array.Copy(BitConverter.GetBytes((ushort)price), 0, data, ofs + 4 * i + 2, 2);
            }
        }
        private void setListBPMove()
        {
            int count = dgvmv.Rows.Count;
            var ofs = ofs_BPTutor + len_BPTutor.Take(entryBPMove).Sum(z => z) * 4;
            for (int i = 0; i < count; i++)
            {
                int item = Array.IndexOf(movelist, dgvmv.Rows[i].Cells[1].Value);
                Array.Copy(BitConverter.GetBytes((ushort)item), 0, data, ofs + 4 * i, 2);
                int price; string p = dgvmv.Rows[i].Cells[2].Value.ToString();
                if (int.TryParse(p, out price))
                    Array.Copy(BitConverter.GetBytes((ushort)price), 0, data, ofs + 4 * i + 2, 2);
            }
        }

        private void B_Randomize_Click(object sender, EventArgs e)
        {
            switch (tabControl1.SelectedIndex)
            {
                case 0:
                    RandomizeItems();
                    break;
                case 1:
                    RandomizeBPItems();
                    break;
                default:
                    WinFormsUtil.Alert("Not implemented");
                    break;
            }
        }
        private void RandomizeItems()
        {
            if (DialogResult.Yes != WinFormsUtil.Prompt(MessageBoxButtons.YesNoCancel, "Randomize mart inventories?"))
                return;

            int[] validItems = Randomizer.getRandomItemList();

            int ctr = 0;
            Util.Shuffle(validItems);

            bool specialOnly = DialogResult.Yes == WinFormsUtil.Prompt(MessageBoxButtons.YesNo, "Randomize only special marts?", "Will leave regular necessities intact.");
            int start = specialOnly ? 8 : 0;
            for (int i = start; i < CB_Location.Items.Count; i++)
            {
                CB_Location.SelectedIndex = i;
                for (int r = 0; r < dgv.Rows.Count; r++)
                {
                    int currentItem = Array.IndexOf(itemlist, dgv.Rows[r].Cells[1].Value);
                    if (BannedItems.Contains(currentItem))
                        continue;
                    dgv.Rows[r].Cells[1].Value = itemlist[validItems[ctr++]];
                    if (ctr <= validItems.Length) continue;
                    Util.Shuffle(validItems); ctr = 0;
                }
            }
            WinFormsUtil.Alert("Randomized!");
        }
        private void RandomizeBPItems()
        {
            if (DialogResult.Yes != WinFormsUtil.Prompt(MessageBoxButtons.YesNoCancel, "Randomize BP inventories?"))
                return;

            int[] validItems = Randomizer.getRandomItemList();

            int ctr = 0;
            Util.Shuffle(validItems);

            for (int i = 0; i < CB_LocationBPItem.Items.Count; i++)
            {
                CB_LocationBPItem.SelectedIndex = i;
                for (int r = 0; r < dgvbp.Rows.Count; r++)
                {
                    dgvbp.Rows[r].Cells[1].Value = itemlist[validItems[ctr++]];
                    if (ctr <= validItems.Length) continue;
                    Util.Shuffle(validItems); ctr = 0;
                }
            }
            WinFormsUtil.Alert("Randomized!");
        }

        /// <summary>
        /// Just TMs & HMs; don't want these to be changed; if changed, they are not available elsewhere ingame.
        /// </summary>
        internal static readonly HashSet<int> BannedItems = new HashSet<int>
        {
            328, 329, 330, 331, 332, 333, 334, 335, 336, 337, 338, 339, 340, 341, 342, 343, 344, 345, 346, 347, 348,
            349, 350, 351, 352, 353, 354, 355, 356, 357, 358, 359, 360, 361, 362, 363, 364, 365, 366, 367, 368, 369,
            370, 371, 372, 373, 374, 375, 376, 377, 378, 379, 380, 381, 382, 383, 384, 385, 386, 387, 388, 389, 390,
            391, 392, 393, 394, 395, 396, 397, 398, 399, 400, 401, 402, 403, 404, 405, 406, 407, 408, 409, 410, 411,
            412, 413, 414, 415, 416, 417, 418, 419, 420, 421, 422, 423, 424, 425, 426, 427, 618, 619, 620, 690, 691,
            692, 693, 694, 701, 737
        };
    }
}
