using System;
using System.IO;
using System.Windows.Forms;

namespace pk3DS
{
    public partial class ItemEditor7 : Form
    {
        public ItemEditor7(byte[][] infiles)
        {
            files = infiles;
            itemlist[0] = "";

            InitializeComponent();
            Setup();
        }

        private readonly byte[][] files;
        private readonly string[] itemlist = Main.getText(TextName.ItemNames);
        private readonly string[] itemflavor = Main.getText(TextName.ItemFlavor);

        private void Setup()
        {
            foreach (string s in itemlist) CB_Item.Items.Add(s);
            CB_Item.SelectedIndex = 1;
        }
        private int entry = -1;
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
            item = new Item(files[entry]);

            RTB.Text = itemflavor[entry].Replace("\\n", Environment.NewLine);
            MT_Price.Text = item.BuyPrice.ToString();
            NUD_UseEffect.Value = item.UseEffect;
        }
        private void setEntry()
        {
            if (entry < 1) return;

            item.Price = (ushort)(Util.ToInt32(MT_Price)/10);
            item.UseEffect = (byte)(int)NUD_UseEffect.Value;

            files[entry] = item.Write();
        }
        private void formClosing(object sender, FormClosingEventArgs e)
        {
            setEntry();
        }

        private void changePrice(object sender, EventArgs e)
        {
            MT_Sell.Text = (Math.Min(Util.ToUInt32(MT_Price) / 10, 0x7FFF) * 10 / 2).ToString();
        }

        private readonly byte[] ItemIconTableSignature =
        {
            0x00, 0x00, 0x00, 0x00, 0xFF, 0xFF, 0xFF, 0xFF,
            0x00, 0x00, 0x00, 0x00, 0x01, 0x00, 0x00, 0x00,
            0x07, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
            0x00, 0x01, 0x01, 0x00
        };

        private int getItemMapOffset()
        {
            if (Main.ExeFSPath == null) { Util.Alert("No exeFS code to load."); return -1; }
            string[] exefsFiles = Directory.GetFiles(Main.ExeFSPath);
            if (!File.Exists(exefsFiles[0]) || !Path.GetFileNameWithoutExtension(exefsFiles[0]).Contains("code")) { Util.Alert("No .code.bin detected."); return -1; }
            byte[] data = File.ReadAllBytes(exefsFiles[0]);

            byte[] reference = ItemIconTableSignature;

            int ptr = Util.IndexOfBytes(data, reference, 0x400000, 0) - 2 + reference.Length;
            return ptr;
        }
    }
}