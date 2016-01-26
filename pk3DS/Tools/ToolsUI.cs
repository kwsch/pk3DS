using System;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading;
using System.Windows.Forms;

namespace pk3DS
{
    public partial class ToolsUI : Form
    {
        public ToolsUI()
        {
            InitializeComponent();
            AllowDrop = PB_Unpack.AllowDrop = PB_Repack.AllowDrop = PB_BCLIM.AllowDrop = true;
            DragEnter += tabMain_DragEnter;
            DragDrop += tabMain_DragDrop;
            PB_Unpack.DragEnter += tabMain_DragEnter;
            PB_Unpack.DragDrop += tabMain_DragDrop;
            PB_Repack.DragEnter += tabMain_DragEnter;
            PB_Repack.DragDrop += tabMain_DragDrop;
            PB_BCLIM.DragEnter += tabMain_DragEnter;
            PB_BCLIM.DragDrop += tabMain_DragDrop;
            CLIMWindow = PB_BCLIM.Size;
            CB_Repack.Items.Add("Autodetect");
            CB_Repack.Items.Add("GARC Pack");
            CB_Repack.Items.Add("DARC Pack (use filenames)");
            CB_Repack.Items.Add("Mini Pack (from Name)");
            CB_Repack.SelectedIndex = 0;
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
            else try {
                CTR.LZSS.Decompress(path, Path.Combine(Path.GetDirectoryName(path), "dec_" + Path.GetFileName(path)));
                File.Delete(path);
                System.Media.SystemSounds.Asterisk.Play();
            } catch { try { if (threads < 1)
                new Thread(() => { threads++; new CTR.BLZCoder(new[] { "-d", path }, pBar1); threads--; Util.Alert("Decompressed!"); }).Start();
            } catch { Util.Error("Unable to process file."); threads = 0; } }
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

        private int threads;
        private void openARC(string path, bool recursing = false)
        {
            string newFolder = "";
            try
            {
                // Pre-check file length to see if it is at least valid.
                FileInfo fi = new FileInfo(path);
                if (fi.Length > 1.6 * (1<<30)) { Util.Error("File is too big!"); return; } // 1.6 GB
                string folderPath = Path.Combine(Path.GetDirectoryName(path), Path.GetFileNameWithoutExtension(path));

                byte[] first4 = new byte[4];
                try
                {
                    using (BinaryReader bw = new BinaryReader(new FileStream(path, FileMode.Open)))
                        first4 = bw.ReadBytes(4);
                }
                catch (Exception e)
                {
                    Util.Error("Cannot open file!", e.ToString());
                }

                // Determine if it is a DARC or a Mini
                // Check if Mini first
                string fx = fi.Length > 10 * (1<<20) ? null : CTR.mini.getIsMini(path); // no mini is above 10MB
                if (fx != null) // Is Mini Packed File
                {
                    newFolder = folderPath + "_" + fx;
                    // Fetch Mini File Contents
                    CTR.mini.unpackMini(path, fx, newFolder, false);
                    // Recurse throught the extracted contents if they extract successfully
                    if (Directory.Exists(newFolder))
                    {   
                        foreach (string file in Directory.GetFiles(newFolder))
                            openARC(file, true);
                        batchRenameExtension(newFolder);
                    }
                }
                else if (first4.SequenceEqual(BitConverter.GetBytes(0x47415243))) // GARC
                {
                    if (threads > 0) { Util.Alert("Please wait for all operations to finish first."); return; }
                    bool SkipDecompression = ModifierKeys == Keys.Control;
                    new Thread(() =>
                    {
                        threads++;
                        bool r = CTR.GARC.garcUnpack(path, folderPath + "_g", SkipDecompression, pBar1);
                        threads--;
                        if (r)
                            batchRenameExtension(newFolder);
                        else
                        { Util.Alert("Unpacking failed."); return; }
                        System.Media.SystemSounds.Asterisk.Play();
                    }).Start();
                    return;
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
                    newFolder = folderPath + "_d";
                    bool r = CTR.DARC.darc2files(darcData, newFolder);
                    if (!r)
                    { Util.Alert("Unpacking failed."); return; }
                }
                else if (!recursing)
                { Util.Alert("File is not a darc or a mini packed file:" + Environment.NewLine + path); return;}

            }
            catch (Exception e)
            {
                if (!recursing)
                    Util.Error("File error:" + Environment.NewLine + path, e.ToString());
                threads = 0;
            }
            System.Media.SystemSounds.Asterisk.Play();
        }
        private void saveARC(string path)
        {
            if (!Directory.Exists(path)) { Util.Error("Input path is not a Folder", path); return; }
            string folderName = Path.GetFileName(path);
            if (folderName == null) return;
            string parentName = Directory.GetParent(path).FullName;
            int type = CB_Repack.SelectedIndex;
            switch (type)
            {
                case 0: // AutoDetect
                {
                    if (!folderName.Contains("_"))
                    { Util.Alert("Unable to autodetect pack type."); return; }

                    if (folderName.Contains("_g"))
                        goto case 1;
                    if (folderName.Contains("_d"))
                        goto case 2;
                    // else
                        goto case 3;
                }
                case 1: // GARC Pack
                {
                    if (threads > 0) { Util.Alert("Please wait for all operations to finish first."); return; }
                    new Thread(() =>
                    {
                        bool r = CTR.GARC.garcPackMS(path, folderName + ".garc", pBar1);
                        if (!r) { Util.Alert("Packing failed."); return; }
                        // Delete path after repacking
                        if (CHK_Delete.Checked && Directory.Exists(path))
                            Directory.Delete(path, true);

                        System.Media.SystemSounds.Asterisk.Play();
                    }).Start();
                    return;
                }
                case 2: // DARC Pack (from existing if exists)
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
                case 3: // Mini Pack
                {
                    // Get Folder Name
                    string fileName = Path.GetFileName(path);
                    if (fileName.Length < 3) { Util.Error("Mini Folder name not valid:", path); return; }

                    int index = fileName.LastIndexOf('_');
                    string fileNum = fileName.Substring(0, index);
                    string fileExt = fileName.Substring(index + 1);
                    
                    // Find old file for reference...
                    string file;
                    if (File.Exists(Path.Combine(parentName, fileNum + ".bin")))
                        file = Path.Combine(parentName, fileNum + ".bin");
                    else if (File.Exists(Path.Combine(parentName, fileNum + "." + fileExt)))
                        file = Path.Combine(parentName, fileNum + "." + fileExt);
                    else
                        file = null;

                    byte[] oldData = file != null ? File.ReadAllBytes(file) : null;
                    bool r = CTR.mini.packMini2(path, fileExt, Path.Combine(parentName, fileNum + "." + fileExt));
                    if (!r)
                    {
                        Util.Alert("Packing failed.");
                        break;
                    }

                    // Check to see if the header size is different...
                    if (oldData == null) // No data to compare to.
                        break;

                    byte[] newData = File.ReadAllBytes(Path.Combine(parentName, fileNum + "." + fileExt));
                    if (newData[2] == oldData[2])
                    {
                        int newPtr = BitConverter.ToInt32(newData, 4);
                        int oldPtr = BitConverter.ToInt32(oldData, 4);
                        if (newPtr != oldPtr) // Header size is different. Prompt repointing.
                        {
                            if (DialogResult.Yes !=
                                Util.Prompt(MessageBoxButtons.YesNo, "Header size of existing file is nonstandard.",
                                    "Adjust newly packed file to have the same header size as old file? Data pointers will be updated accordingly."))
                                break;

                            // Fix pointers
                            byte[] update = CTR.mini.adjustMiniHeader(newData, oldPtr);
                            File.WriteAllBytes(Path.Combine(parentName, fileNum + "." + fileExt), update);
                        }                        
                    }

                    break;
                }
                default: Util.Alert("Repacking not implemented." + Environment.NewLine + path);
                    return;
            }
            // Delete path after repacking
            if (CHK_Delete.Checked && Directory.Exists(path))
                Directory.Delete(path, true);
            System.Media.SystemSounds.Asterisk.Play();
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
        private readonly Size CLIMWindow;
        private void B_Reset_Click(object sender, EventArgs e)
        {
            PB_BCLIM.Size = CLIMWindow;
        }

        private void batchRenameExtension(string Folder)
        {
            if (!Directory.Exists(Folder)) 
                return;

            foreach (string f in Directory.GetFiles(Folder, "*", SearchOption.AllDirectories))
            try {
                string ext = Path.GetExtension(f);
                string newExt = CTR.FileFormat.Guess(f);
                if (ext != newExt)
                    File.Move(f, Path.Combine(Path.GetDirectoryName(f), Path.GetFileNameWithoutExtension(f)) + newExt);
            } catch { }
        }

        private void closeForm(object sender, FormClosingEventArgs e)
        {
            if (threads > 0 && DialogResult.Yes != Util.Prompt(MessageBoxButtons.YesNo, "Currently processing files.", "Abort?"))
                e.Cancel = true;
        }
    }
}