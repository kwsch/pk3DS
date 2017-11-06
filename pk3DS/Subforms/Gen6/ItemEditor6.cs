using System;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using pk3DS.Core;
using pk3DS.Core.Structures;

namespace pk3DS
{
    public partial class ItemEditor6 : Form
    {
        public ItemEditor6(byte[][] infiles)
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
            L_Index.Text = "Index: " + entry.ToString("000");
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

        private int getItemMapOffset()
        {
            if (Main.ExeFSPath == null) { WinFormsUtil.Alert("No exeFS code to load."); return -1; }
            string[] exefsFiles = Directory.GetFiles(Main.ExeFSPath);
            if (!File.Exists(exefsFiles[0]) || !Path.GetFileNameWithoutExtension(exefsFiles[0]).Contains("code")) { WinFormsUtil.Alert("No .code.bin detected."); return -1; }
            byte[] data = File.ReadAllBytes(exefsFiles[0]);

            byte[] reference = Main.Config.ORAS
                ? new byte[] { 0x92, 0x0A, 0x06, 0x3F, 0x75, 0x02 } // ORAS (vanilla @ 47C640)
                : new byte[] { 0x92, 0x0A, 0x06, 0x3F, 0x41, 0x02 }; // XY (vanilla @ 43DB74)

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