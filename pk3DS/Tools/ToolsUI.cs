using System;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace pk3DS
{
    public partial class ToolsUI : Form
    {
        public ToolsUI()
        {
            InitializeComponent();

            PB_Unpack.DragEnter += tabMain_DragEnter;
            PB_Unpack.DragDrop += tabMain_DragDrop;
        }
        private void tabMain_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop)) e.Effect = DragDropEffects.Copy;
        }
        private void tabMain_DragDrop(object sender, DragEventArgs e)
        {
            string[] files = (string[])e.Data.GetData(DataFormats.FileDrop);
            string path = files[0]; // open first D&D

            if (sender == PB_Unpack)
                openARC(path);
            else if (sender == PB_BCLIM)
                openIMG(path);
            else if (sender == panel1)
                saveARC(path);
        }

        private void saveARC(string path)
        {
            int type = comboBox1.SelectedIndex;
            Util.Alert("Not implemented." + Environment.NewLine + path);
        }
        private void openIMG(string path)
        {
            Util.Alert("Not implemented." + Environment.NewLine + path);
        }
        private void openARC(string path, bool recursing = false)
        {
            try
            {
                // Pre-check file length to see if it is at least valid.
                FileInfo fi = new FileInfo(path);
                if (fi.Length > 900000) { Util.Error("File is too big!"); return; }

                string newFolder = Path.Combine(Path.GetDirectoryName(path), Path.GetFileNameWithoutExtension(path));
                // Determine if it is a DARC or a Mini
                using (var s = new FileStream(path, FileMode.Open))
                using (var br = new BinaryReader(s))
                {
                    // Check if Mini first
                    // ushort magic = br.ReadUInt16();
                    string fx = new string(br.ReadChars(2));
                    newFolder += "." + fx + "-";
                    ushort count = br.ReadUInt16();
                    uint[] offsets = new uint[count + 1];
                    for (int i = 0; i < count; i++)
                    {
                        offsets[i] = br.ReadUInt32();
                    }
                    uint length = br.ReadUInt32();
                    offsets[offsets.Length - 1] = length;
                    if (fi.Length == length)
                    {
                        // Fetch Mini File Contents
                        ARC.unpackMini(path, fx, newFolder);
                        // Recurse throught the extracted contents if they extract successfully
                        if (Directory.Exists(newFolder))
                            foreach (string file in Directory.GetFiles(newFolder))
                                openARC(file, true);
                    }
                    else if (false) // DARC
                    {
                        
                    }
                    else if (!recursing)
                        Util.Alert("File is not a darc or a mini packed file:" + Environment.NewLine + path);
                }
            }
            catch (Exception e)
            {
                Util.Error("File error:" + Environment.NewLine + path, e.ToString());
            }
        }

        private void PB_BCLIM_Paint(object sender, PaintEventArgs e)
        {
            if (ModifierKeys == Keys.Control && Util.Prompt(MessageBoxButtons.YesNo, "Copy image to clipboard?") == DialogResult.Yes)
                Clipboard.SetImage(PB_BCLIM.BackgroundImage);
            else if (PB_BCLIM.BackColor == Color.Transparent)
                PB_BCLIM.BackColor = Color.GreenYellow;
            else PB_BCLIM.BackColor = Color.Transparent;
        }
    }
}