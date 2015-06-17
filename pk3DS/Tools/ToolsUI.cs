using System;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace pk3DS
{
    public partial class ToolsUI : Form
    {
        public ToolsUI()
        {
            InitializeComponent();
            AllowDrop = PB_Unpack.AllowDrop = PB_Repack.AllowDrop = PB_BCLIM.AllowDrop = true;
            PB_Unpack.DragEnter += tabMain_DragEnter;
            PB_Unpack.DragDrop += tabMain_DragDrop;
            PB_Repack.DragEnter += tabMain_DragEnter;
            PB_Repack.DragDrop += tabMain_DragDrop;
            PB_BCLIM.DragEnter += tabMain_DragEnter;
            PB_BCLIM.DragDrop += tabMain_DragDrop;
            CLIMWindow = PB_BCLIM.Size;
            CB_Repack.Items.Add("GARC Pack");
            CB_Repack.Items.Add("DARC Pack (use filenames)");
            CB_Repack.Items.Add("Mini Pack (from Name)");
            CB_Repack.SelectedIndex = 2;
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
            else if (sender == PB_Repack)
                saveARC(path);
        }
        private void dropHover(object sender, EventArgs e)
        {
            (sender as Panel).BackColor = Color.Gray;
        }
        private void dropLeave(object sender, EventArgs e)
        {
            (sender as Panel).BackColor = Color.Transparent;
        }
        private void openIMG(string path)
        {
            var img = CTR.BCLIM.makeBMP(path, CHK_PNG.Checked);
            if (img == null) return;
            PB_BCLIM.Size = new Size(img.Width + 2, img.Height + 2);
            PB_BCLIM.BackgroundImage = img;
        }
        private void openARC(string path, bool recursing = false)
        {
            try
            {
                // Pre-check file length to see if it is at least valid.
                FileInfo fi = new FileInfo(path);
                if (fi.Length > 50000000) { Util.Error("File is too big!"); return; }
                string folderPath = Path.Combine(Path.GetDirectoryName(path), Path.GetFileNameWithoutExtension(path));

                // Determine if it is a DARC or a Mini
                // Check if Mini first
                string fx = CTR.mini.getIsMini(path);
                string newFolder = folderPath + "_" + fx;
                if (fx != null) // Is Mini Packed File
                {
                    // Fetch Mini File Contents
                    CTR.mini.unpackMini(path, fx, newFolder, false);
                    // Recurse throught the extracted contents if they extract successfully
                    if (Directory.Exists(newFolder))
                        foreach (string file in Directory.GetFiles(newFolder))
                            openARC(file, true);
                }
                else if (BitConverter.ToUInt32(File.ReadAllBytes(path), 0) == 0x47415243) // GARC
                {
                    bool r = CTR.GARC.garcUnpack(path, folderPath + "_g", false);
                    if (!r) Util.Alert("Unpacking failed.");
                }
                else if (ARC.analyze(path).valid) // DARC
                {
                    var data = File.ReadAllBytes(path);
                    int pos = 0;
                    while (BitConverter.ToUInt32(data, pos) != 0x63726164)
                    {
                        pos += 4;
                        if (pos >= data.Length) return;
                    }
                    var darcData = data.Skip(pos).ToArray();
                    bool r = CTR.DARC.darc2files(darcData, folderPath + "_d");
                    if (!r) Util.Alert("Unpacking failed.");
                }
                else if (!recursing)
                { Util.Alert("File is not a darc or a mini packed file:" + Environment.NewLine + path); return ;}

                System.Media.SystemSounds.Asterisk.Play();
            }
            catch (Exception e)
            {
                if (!recursing)
                    Util.Error("File error:" + Environment.NewLine + path, e.ToString());
            }
        }
        private void saveARC(string path)
        {
            if (!Directory.Exists(path)) { Util.Error("Input path is not a Folder", path); return; }
            string folderName = Path.GetDirectoryName(path);
            string parentName = Directory.GetParent(path).FullName;

            int type = CB_Repack.SelectedIndex;
            switch (type)
            {
                case 0: // GARC Pack
                {
                    bool r = CTR.GARC.garcPackMS(path, folderName + ".garc");
                    if (!r) Util.Alert("Packing failed.");
                    break;
                }
                case 1: // DARC Pack (from existing if exists)
                {
                    string oldFile = path.Replace("_d", "");
                    if (File.Exists(Path.Combine(parentName, oldFile)))
                        oldFile = Path.Combine(parentName, oldFile);
                    else if (File.Exists(Path.Combine(parentName, oldFile + ".bin")))
                        oldFile = Path.Combine(parentName, oldFile + ".bin");
                    else if (File.Exists(Path.Combine(parentName, oldFile + ".darc")))
                        oldFile = Path.Combine(parentName, oldFile + ".darc");
                    else oldFile = null;

                    bool r = CTR.DARC.files2darc(path, false, oldFile);
                    if (!r) Util.Alert("Packing failed.");
                    break;
                }
                case 2: // Mini Pack
                {
                    string fileName = folderName.Replace("_", "");
                    if (fileName.Length < 2) { Util.Error("Mini Folder name not valid:", path); return; }

                    string fileExt = fileName.Substring(fileName.Length - 2);
                    string fileNum = fileName.Substring(0, fileName.Length - 3); // ignore "."

                    bool r = CTR.mini.packMini2(path, fileExt, parentName + fileNum + "." + fileExt);
                    if (!r) Util.Alert("Packing failed.");
                    break;
                }
                default: Util.Alert("Repacking not implemented." + Environment.NewLine + path);
                    return;
            }
            // Delete path after repacking
            if (CHK_Delete.Checked && Directory.Exists(path))
                Directory.Delete(path, true);
        }
        private void PB_BCLIM_Click(object sender, EventArgs e)
        {
            if (ModifierKeys == Keys.Control && Util.Prompt(MessageBoxButtons.YesNo, "Copy image to clipboard?") == DialogResult.Yes)
                Clipboard.SetImage(PB_BCLIM.BackgroundImage);
            else if (PB_BCLIM.BackColor == Color.Transparent)
                PB_BCLIM.BackColor = Color.GreenYellow;
            else PB_BCLIM.BackColor = Color.Transparent;
        }

        // Utility
        private Size CLIMWindow;
        private void B_Reset_Click(object sender, EventArgs e)
        {
            PB_BCLIM.Size = CLIMWindow;
        }
    }
}