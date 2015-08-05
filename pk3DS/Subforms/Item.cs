using System;
using System.IO;
using System.Windows.Forms;

namespace pk3DS
{
    public partial class ItemEditor : Form
    {
        public ItemEditor()
        {
            itemlist[0] = "";

            InitializeComponent();
            Setup();
        }
        string[] files = Directory.GetFiles("item");
        string[] itemlist = Main.getText((Main.oras) ? 114 : 96);
        string[] itemflavor = Main.getText((Main.oras) ? 117 : 99);

        private void Setup()
        {
            foreach (string s in itemlist) CB_Item.Items.Add(s);
            CB_Item.SelectedIndex = 1;
        }
        int entry = -1;
        private Item item;
        private void changeEntry(object sender, EventArgs e)
        {
            setEntry();
            entry = CB_Item.SelectedIndex;
            L_Index.Text = "Index: " + entry.ToString("000");
            getEntry();
        }
        private void getEntry()
        {
            if (entry < 1) return;
            item = new Item(File.ReadAllBytes(files[entry]));

            RTB.Text = itemflavor[entry].Replace("\\n", Environment.NewLine);
            MT_Price.Text = item.BuyPrice.ToString();
            NUD_UseEffect.Value = item.UseEffect;
        }
        private void setEntry()
        {
            if (entry < 1) return;

            item.Price = (ushort)(Util.ToInt32(MT_Price)/10);
            item.UseEffect = (byte)(int)NUD_UseEffect.Value;

            File.WriteAllBytes(files[entry], item.Write());
        }
        private void formClosing(object sender, FormClosingEventArgs e)
        {
            setEntry();
        }

        private void changePrice(object sender, EventArgs e)
        {
            MT_Sell.Text = ((Math.Min(Util.ToUInt32(MT_Price) / 10, 0x7FFF)) * 10 / 2).ToString();
        }
    }
}