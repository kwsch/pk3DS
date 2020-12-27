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
            //len_BPTutor = data.Skip(0x52D2).Take(4).ToArray();
            len_BPItem = data.Skip(0x52D2 + 4).Take(7).ToArray();
            len_Items = data.Skip(0x52D2 + 4 + 7).TakeWhile(z => (sbyte) z > 0).ToArray();

            itemlist[0] = "";
            SetupDGV();
            foreach (string s in locations) CB_Location.Items.Add(s);
            foreach (string s in locationsBP) CB_LocationBPItem.Items.Add(s);
            CB_Location.SelectedIndex =
            CB_LocationBPItem.SelectedIndex = 0;
        }

        private const int ofs_Item = 0x50BC;
        private const int ofs_BPItem = 0x52FA;
        //private const int ofs_BPTutor = 0x54DE;
        private readonly byte[] len_Items;
        private readonly byte[] len_BPItem;
        //private readonly byte[] len_BPTutor;

        private readonly string[] itemlist = Main.Config.GetText(TextName.ItemNames);
        //private readonly string[] movelist = Main.Config.GetText(TextName.MoveNames);
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
            "Thrifty Megamart, Left [Poké Balls]",
            "Thrifty Megamart, Middle [Misc]",
            "Thrifty Megamart, Right [Strange Souvenir]",
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
            "Beaches [Medicine]"
        };
        #endregion

        private void B_Save_Click(object sender, EventArgs e)
        {
            if (entryItem > -1) SetListItem();
            if (entryBPItem > -1) SetListBPItem();
            File.WriteAllBytes(CROPath, data);
            Close();
        }

        private void B_Cancel_Click(object sender, EventArgs e) => Close();

        private void SetupDGV()
        {
            foreach (string t in itemlist)
                dgvItem.Items.Add(t); // add only the Names
            foreach (string t in itemlist)
                dgvItemBP.Items.Add(t); // add only the Names
        }

        private int entryItem = -1;
        private int entryBPItem = -1;

        private void ChangeIndexItem(object sender, EventArgs e)
        {
            if (entryItem > -1) SetListItem();
            entryItem = CB_Location.SelectedIndex;
            GetListItem();
        }

        private void ChangeIndexBPItem(object sender, EventArgs e)
        {
            if (entryBPItem > -1) SetListBPItem();
            entryBPItem = CB_LocationBPItem.SelectedIndex;
            GetListBPItem();
        }

        private void GetListItem()
        {
            dgv.Rows.Clear();
            int count = len_Items[entryItem];
            dgv.Rows.Add(count);
            var ofs = ofs_Item + (len_Items.Take(entryItem).Sum(z => z) * 2);
            for (int i = 0; i < count; i++)
            {
                dgv.Rows[i].Cells[0].Value = i.ToString();
                dgv.Rows[i].Cells[1].Value = itemlist[BitConverter.ToUInt16(data, ofs + (2 * i))];
            }
        }

        private void GetListBPItem()
        {
            dgvbp.Rows.Clear();
            int count = len_BPItem[entryBPItem];
            dgvbp.Rows.Add(count);
            var ofs = ofs_BPItem + (len_BPItem.Take(entryBPItem).Sum(z => z) * 4);
            for (int i = 0; i < count; i++)
            {
                dgvbp.Rows[i].Cells[0].Value = i.ToString();
                dgvbp.Rows[i].Cells[1].Value = itemlist[BitConverter.ToUInt16(data, ofs + (4 * i))];
                dgvbp.Rows[i].Cells[2].Value = BitConverter.ToUInt16(data, ofs + (4 * i) + 2).ToString();
            }
        }

        private void SetListItem()
        {
            int count = dgv.Rows.Count;
            var ofs = ofs_Item + (len_Items.Take(entryItem).Sum(z => z) * 2);
            for (int i = 0; i < count; i++)
                Array.Copy(BitConverter.GetBytes((ushort)Array.IndexOf(itemlist, dgv.Rows[i].Cells[1].Value)), 0, data, ofs + (2 * i), 2);
        }

        private void SetListBPItem()
        {
            int count = dgvbp.Rows.Count;
            var ofs = ofs_BPItem + (len_BPItem.Take(entryBPItem).Sum(z => z) * 4);
            for (int i = 0; i < count; i++)
            {
                int item = Array.IndexOf(itemlist, dgvbp.Rows[i].Cells[1].Value);
                Array.Copy(BitConverter.GetBytes((ushort)item), 0, data, ofs + (4 * i), 2);
                string p = dgvbp.Rows[i].Cells[2].Value.ToString();
                if (int.TryParse(p, out var price))
                    Array.Copy(BitConverter.GetBytes((ushort)price), 0, data, ofs + (4 * i) + 2, 2);
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
            }
        }

        private void RandomizeItems()
        {
            if (DialogResult.Yes != WinFormsUtil.Prompt(MessageBoxButtons.YesNoCancel, "Randomize mart inventories?"))
                return;

            int[] validItems = Randomizer.GetRandomItemList();

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
                    if (CHK_XItems.Checked && XItems.Contains(currentItem))
                        continue;
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

            int[] validItems = Randomizer.GetRandomItemList();

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
        internal static readonly HashSet<int> BannedItems = new()
        {
            328, 329, 330, 331, 332, 333, 334, 335, 336, 337, 338, 339, 340, 341, 342, 343, 344, 345, 346, 347, 348,
            349, 350, 351, 352, 353, 354, 355, 356, 357, 358, 359, 360, 361, 362, 363, 364, 365, 366, 367, 368, 369,
            370, 371, 372, 373, 374, 375, 376, 377, 378, 379, 380, 381, 382, 383, 384, 385, 386, 387, 388, 389, 390,
            391, 392, 393, 394, 395, 396, 397, 398, 399, 400, 401, 402, 403, 404, 405, 406, 407, 408, 409, 410, 411,
            412, 413, 414, 415, 416, 417, 418, 419, 420, 421, 422, 423, 424, 425, 426, 427, 618, 619, 620, 690, 691,
            692, 693, 694, 701, 737
        };

        /// <summary>
        /// All X Items usable in Generations 6 and 7. Speedrunners utilize these Items a lot, so make sure they are still available.
        /// </summary>
        internal static readonly HashSet<int> XItems = new()
        {
            055, 056, 057, 058, 059, 060, 061, 062
        };
    }
}
