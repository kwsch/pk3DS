using System;
using System.IO;
using System.Linq;
using System.Windows.Forms;

using pk3DS.Core;
using pk3DS.Core.Structures;

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
        private readonly string[] itemlist = Main.Config.getText(TextName.ItemNames);
        private readonly string[] itemflavor = Main.Config.getText(TextName.ItemFlavor);

        private void Setup()
        {
            foreach (string s in itemlist) CB_Item.Items.Add(s);
            CB_Item.SelectedIndex = 1;
        }
        private int entry = -1;
        private void changeEntry(object sender, EventArgs e)
        {
            setEntry();
            entry = CB_Item.SelectedIndex;
            L_Index.Text = $"Index: {entry:000}";
            getEntry();
        }
        private void getEntry()
        {
            if (entry < 1) return;
            Grid.SelectedObject = new Item(files[entry]);

            RTB.Text = itemflavor[entry].Replace("\\n", Environment.NewLine);
        }
        private void setEntry()
        {
            if (entry < 1) return;
            files[entry] = ((Item)Grid.SelectedObject).Write();
        }
        private void formClosing(object sender, FormClosingEventArgs e)
        {
            setEntry();
        }

        private static readonly byte[] ItemIconTableSignature =
        {
            0x00, 0x00, 0x00, 0x00, 0xFF, 0xFF, 0xFF, 0xFF,
            0x00, 0x00, 0x00, 0x00, 0x01, 0x00, 0x00, 0x00,
            0x07, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
            0x00, 0x01, 0x01, 0x00
        };

        private static int getItemMapOffset()
        {
            if (Main.ExeFSPath == null) { WinFormsUtil.Alert("No exeFS code to load."); return -1; }
            string[] exefsFiles = Directory.GetFiles(Main.ExeFSPath);
            if (!File.Exists(exefsFiles[0]) || !Path.GetFileNameWithoutExtension(exefsFiles[0]).Contains("code")) { WinFormsUtil.Alert("No .code.bin detected."); return -1; }
            byte[] data = File.ReadAllBytes(exefsFiles[0]);

            byte[] reference = ItemIconTableSignature;

            int ptr = Util.IndexOfBytes(data, reference, 0x400000, 0) - 2 + reference.Length;
            return ptr;
        }

        private void B_Table_Click(object sender, EventArgs e)
        {
            var items = files.Select(z => new Item(z));
            Clipboard.SetText(TableUtil.GetTable(items, itemlist));
            System.Media.SystemSounds.Asterisk.Play();
        }
    }
}