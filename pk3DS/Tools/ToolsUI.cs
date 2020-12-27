using pk3DS.ARCUtil;
using pk3DS.Core.CTR;
using System;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading;
using System.Windows.Forms;
using pk3DS.Core;

namespace pk3DS
{
    public sealed partial class ToolsUI : Form
    {
        public ToolsUI()
        {
            InitializeComponent();
            AllowDrop = PB_Unpack.AllowDrop = PB_Repack.AllowDrop = PB_BCLIM.AllowDrop = true;
            DragEnter += TabMain_DragEnter;
            DragDrop += TabMain_DragDrop;
            PB_Unpack.DragEnter += TabMain_DragEnter;
            PB_Unpack.DragDrop += TabMain_DragDrop;
            PB_Repack.DragEnter += TabMain_DragEnter;
            PB_Repack.DragDrop += TabMain_DragDrop;
            PB_BCLIM.DragEnter += TabMain_DragEnter;
            PB_BCLIM.DragDrop += TabMain_DragDrop;
            CLIMWindow = PB_BCLIM.Size;
            CB_Repack.Items.Add("Autodetect");
            CB_Repack.Items.Add("GARC Pack");
            CB_Repack.Items.Add("DARC Pack (use filenames)");
            CB_Repack.Items.Add("Mini Pack (from Name)");
            CB_Repack.SelectedIndex = 0;
        }

        private void TabMain_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop)) e.Effect = DragDropEffects.Copy;
        }

        private void TabMain_DragDrop(object sender, DragEventArgs e)
        {
            string[] files = (string[])e.Data.GetData(DataFormats.FileDrop);
            foreach (var path in files)
                HandleDrop(sender, path);
            System.Media.SystemSounds.Asterisk.Play();
        }

        private void HandleDrop(object sender, string path)
        {
            if (sender == PB_Unpack)
                OpenARC(path, pBar1);
            else if (sender == PB_BCLIM)
                OpenIMG(path);
            else if (sender == PB_Repack)
                SaveARC(path);
            else
                DecompressLZSS_BLZ(path);
        }

        private void DecompressLZSS_BLZ(string path)
        {
            try
            {
                LZSS.Decompress(path, Path.Combine(Path.GetDirectoryName(path), "dec_" + Path.GetFileName(path)));
                File.Delete(path);
            }
            catch
            {
                try
                {
                    if (threads < 1)
                        new Thread(() => { Interlocked.Increment(ref threads); new BLZCoder(new[] { "-d", path }, pBar1); Interlocked.Decrement(ref threads); WinFormsUtil.Alert("Decompressed!"); }).Start();
                }
                catch { WinFormsUtil.Error("Unable to process file."); threads = 0; }
            }
        }

        private void DropHover(object sender, EventArgs e) => ((Panel) sender).BackColor = Color.Gray;
        private void DropLeave(object sender, EventArgs e) => ((Panel) sender).BackColor = Color.Transparent;

        private void OpenIMG(string path)
        {
            var img = BCLIM.MakeBMP(path, CHK_PNG.Checked);
            if (img == null)
            {
                try
                {
                    var flim = new BFLIM(path);
                    if (!flim.Footer.Valid)
                        return;
                    img = flim.GetBitmap();
                }
                catch (Exception ree)
                {
                    Console.WriteLine(ree.Message);
                    return;
                }
                if (CHK_PNG.Checked)
                {
                    var dir = Path.GetDirectoryName(path);
                    var fn = Path.GetFileNameWithoutExtension(path);
                    var outpath = Path.Combine(dir, $"{fn}.png");
                    img.Save(outpath);
                }
            }
            PB_BCLIM.Size = new Size(img.Width + 2, img.Height + 2);
            PB_BCLIM.BackgroundImage = img;
            int leftpad = PB_BCLIM.Location.X;
            int suggestedWidth = (leftpad * 2) + PB_BCLIM.Width + 10;
            if (Width < suggestedWidth)
                Width = suggestedWidth;

            int suggestedHeight = PB_BCLIM.Location.Y + PB_BCLIM.Height + leftpad + 30;
            if (Height < suggestedHeight)
                Height = suggestedHeight;
        }

        internal static volatile int threads;

        internal static void OpenARC(string path, ProgressBar pBar1, bool recursing = false)
        {
            string newFolder = "";
            try
            {
                // Pre-check file length to see if it is at least valid.
                FileInfo fi = new FileInfo(path);
                if (fi.Length > (long)2 * (1<<30)) { WinFormsUtil.Error("File is too big!"); return; } // 2 GB
                string folderPath = Path.Combine(Path.GetDirectoryName(path), Path.GetFileNameWithoutExtension(path));

                byte[] first4 = new byte[4];
                try
                {
                    using var fs = new FileStream(path, FileMode.Open);
                    using var bw = new BinaryReader(fs);
                    first4 = bw.ReadBytes(4);
                }
                catch (Exception e)
                {
                    WinFormsUtil.Error("Cannot open file!", e.ToString());
                }

                // Determine if it is a DARC or a Mini
                // Check if Mini first
                string fx = fi.Length > 10 * (1<<20) ? null : Mini.GetIsMini(path); // no mini is above 10MB
                if (fx != null) // Is Mini Packed File
                {
                    newFolder = folderPath + "_" + fx;
                    // Fetch Mini File Contents
                    Mini.UnpackMini(path, fx, newFolder, false);
                    // Recurse throught the extracted contents if they extract successfully
                    if (Directory.Exists(newFolder))
                    {
                        foreach (string file in Directory.GetFiles(newFolder))
                            OpenARC(file, pBar1, true);
                        BatchRenameExtension(newFolder);
                    }
                }
                else if (first4.SequenceEqual(BitConverter.GetBytes(0x54594C41))) // ALYT
                {
                    if (threads > 0) { WinFormsUtil.Alert("Please wait for all operations to finish first."); return; }
                    new Thread(() =>
                    {
                        Interlocked.Increment(ref threads);
                        var alyt = new ALYT(File.ReadAllBytes(path));
                        var sarc = new SARC(alyt.Data) // rip out sarc
                        {
                            FileName = Path.GetFileNameWithoutExtension(path) + "_sarc",
                            FilePath = Path.GetDirectoryName(path)
                        };
                        if (!sarc.Valid)
                            return;
                        var files = sarc.Dump();
                        foreach (var _ in files)
                        {
                            // openARC(file, pBar1, true);
                        }
                        Interlocked.Decrement(ref threads);
                    }).Start();
                }
                else if (first4.SequenceEqual(BitConverter.GetBytes(0x47415243))) // GARC
                {
                    if (threads > 0) { WinFormsUtil.Alert("Please wait for all operations to finish first."); return; }
                    bool SkipDecompression = ModifierKeys == Keys.Control;
                    new Thread(() =>
                    {
                        Interlocked.Increment(ref threads);
                        bool r = GarcUtil.UnpackGARC(path, folderPath + "_g", SkipDecompression, pBar1);
                        Interlocked.Decrement(ref threads);
                        if (r)
                        {
                            BatchRenameExtension(newFolder);
                        }
                        else
                        { WinFormsUtil.Alert("Unpacking failed."); return; }
                        System.Media.SystemSounds.Asterisk.Play();
                    }).Start();
                }
                else if (ARC.Analyze(path).valid) // DARC
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
                    bool r = Core.CTR.DARC.Darc2files(darcData, newFolder);
                    if (!r)
                    { WinFormsUtil.Alert("Unpacking failed.");  }
                }
                else if (ARC.AnalyzeSARC(path).Valid)
                {
                    var sarc = ARC.AnalyzeSARC(path);
                    Console.WriteLine($"New SARC with {sarc.SFAT.EntryCount} files.");
                    foreach (var _ in sarc.Dump(path))
                    {
                    }
                }
                else if (!recursing)
                { WinFormsUtil.Alert("File is not a darc or a mini packed file:" + Environment.NewLine + path); }
            }
            catch (Exception e)
            {
                if (!recursing)
                    WinFormsUtil.Error("File error:" + Environment.NewLine + path, e.ToString());
                threads = 0;
            }
        }

        private void SaveARC(string path)
        {
            if (!Directory.Exists(path)) { WinFormsUtil.Error("Input path is not a Folder", path); return; }
            string folderName = Path.GetFileName(path);
            string parentName = Directory.GetParent(path).FullName;
            int type = CB_Repack.SelectedIndex;
            switch (type)
            {
                case 0: // AutoDetect
                {
                    if (!folderName.Contains("_"))
                    { WinFormsUtil.Alert("Unable to autodetect pack type."); return; }

                    if (folderName.Contains("_g"))
                        goto case 1;
                    if (folderName.Contains("_d"))
                        goto case 2;
                    // else
                        goto case 3;
                }
                case 1: // GARC Pack
                {
                    if (threads > 0) { WinFormsUtil.Alert("Please wait for all operations to finish first."); return; }
                    DialogResult dr = WinFormsUtil.Prompt(MessageBoxButtons.YesNoCancel, "Format Selection:",
                        "Yes: Sun/Moon (Version 6)\nNo: XY/ORAS (Version 4)");
                    if (dr == DialogResult.Cancel)
                        return;

                    var version = dr == DialogResult.Yes ? GARC.VER_6 : GARC.VER_4;
                    int padding = (int)NUD_Padding.Value;
                    if (version == GARC.VER_4)
                        padding = 4;

                    string outfolder = Directory.GetParent(path).FullName;
                    new Thread(() =>
                    {
                        bool r = GarcUtil.PackGARC(path, Path.Combine(outfolder, folderName + ".garc"), version, padding, pBar1);
                        if (!r) { WinFormsUtil.Alert("Packing failed."); return; }
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

                    bool r = Core.CTR.DARC.Files2darc(path, false, oldFile);
                    if (!r) WinFormsUtil.Alert("Packing failed.");
                    break;
                }
                case 3: // Mini Pack
                {
                    // Get Folder Name
                    string fileName = Path.GetFileName(path);
                    if (fileName.Length < 3) { WinFormsUtil.Error("Mini Folder name not valid:", path); return; }

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
                    bool r = Mini.PackMini2(path, fileExt, Path.Combine(parentName, fileNum + "." + fileExt));
                    if (!r)
                    {
                            WinFormsUtil.Alert("Packing failed.");
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
                            if (DialogResult.Yes != WinFormsUtil.Prompt(MessageBoxButtons.YesNo, "Header size of existing file is nonstandard.", "Adjust newly packed file to have the same header size as old file? Data pointers will be updated accordingly."))
                                break;

                            // Fix pointers
                            byte[] update = Mini.AdjustMiniHeader(newData, oldPtr);
                            File.WriteAllBytes(Path.Combine(parentName, fileNum + "." + fileExt), update);
                        }
                    }

                    break;
                }
                default:
                    WinFormsUtil.Alert("Repacking not implemented." + Environment.NewLine + path);
                    return;
            }
            // Delete path after repacking
            if (CHK_Delete.Checked && Directory.Exists(path))
                Directory.Delete(path, true);
            System.Media.SystemSounds.Asterisk.Play();
        }

        private void PB_BCLIM_Click(object sender, EventArgs e)
        {
            if (ModifierKeys == Keys.Control && WinFormsUtil.Prompt(MessageBoxButtons.YesNo, "Copy image to clipboard?") == DialogResult.Yes)
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

        private static void BatchRenameExtension(string Folder)
        {
            if (!Directory.Exists(Folder))
                return;

            foreach (string f in Directory.GetFiles(Folder, "*", SearchOption.AllDirectories))
            {
                try
                {
                    string ext = Path.GetExtension(f);
                    string newExt = FileFormat.Guess(f);
                    if (ext != newExt)
                        File.Move(f, Path.Combine(Path.GetDirectoryName(f), Path.GetFileNameWithoutExtension(f)) + newExt);
                }
                catch { }
            }
        }

        private void CloseForm(object sender, FormClosingEventArgs e)
        {
            if (threads > 0 && DialogResult.Yes != WinFormsUtil.Prompt(MessageBoxButtons.YesNo, "Currently processing files.", "Abort?"))
                e.Cancel = true;
        }
    }
}