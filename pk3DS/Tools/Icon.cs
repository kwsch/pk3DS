using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Windows.Forms;

namespace pk3DS
{
    public partial class Icon : Form
    {
        private CTR.SMDH SMDH;
        public Icon()
        {
            InitializeComponent();
            SMDH = Main.SMDH;
            if (SMDH?.AppSettings == null || SMDH.LargeIcon.Bytes == null)
            {
                byte[] data = new byte[0x3C0]; // Feed a blank SMDH
                Array.Copy(BitConverter.GetBytes(0x48444D53), data, 4); // SMDH header
                SMDH = new CTR.SMDH(data);
                B_Save.Enabled = false;
            }
            for (int i = 0; i < 16; i++)
                CB_AppInfo.Items.Add(i);

            LoadSMDH();

            AllowDrop = true;
            DragEnter += tabMain_DragEnter;
            DragDrop += tabMain_DragDrop;
        }
        private void tabMain_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop)) e.Effect = DragDropEffects.Copy;
        }
        private void tabMain_DragDrop(object sender, DragEventArgs e)
        {
            string[] files = (string[])e.Data.GetData(DataFormats.FileDrop);
            string path = files[0]; // open first D&D

            openFile(path, true);
        }
        private void LoadSMDH()
        {
            PB_Large.Image = SMDH.LargeIcon.Icon;
            PB_Small.Image = SMDH.SmallIcon.Icon;
            CB_AppInfo.SelectedIndex = 0;
            CB_AppInfo_SelectedIndexChanged(null, null);
        }
        private void SaveSMDH()
        {
            Main.SMDH = SMDH;
            File.WriteAllBytes(Path.Combine(Main.ExeFSPath, "icon.bin"), Main.SMDH.Write());
        }

        private void openFile(string path, bool drop = false)
        {
            FileInfo fi = new FileInfo(path);
            if (fi.Length > 1024 * 1024 * 5)
                return;

            byte[] data = File.ReadAllBytes(path);
            if (data.Length == 0x36C0) // SMDH
                importSMDH(data, true);
            else importIcon(data, drop);
        }
        private void B_Save_Click(object sender, EventArgs e)
        {
            CB_AppInfo_SelectedIndexChanged(null, null); // Force re-save
            if (DialogResult.Yes == Util.Prompt(MessageBoxButtons.YesNo, "Save changes?"))
            {
                SaveSMDH();
                Close();
            }
        }
        private void B_Cancel_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void B_ExportSMDH_Click(object sender, EventArgs e)
        {
            exportSMDH();
        }
        private void B_ExportSmallIcon_Click(object sender, EventArgs e)
        {
            exportIcon(false);
        }
        private void B_ExportLargeIcon_Click(object sender, EventArgs e)
        {
            exportIcon(true);
        }
        private void exportSMDH()
        {
            var sfd = new SaveFileDialog
            {
                FileName = "icon.bin",
                Filter = "System Menu Data Header|*.*"
            };
            if (sfd.ShowDialog() != DialogResult.Yes) return;
            CB_AppInfo_SelectedIndexChanged(null, null); // Force re-save
            File.WriteAllBytes(sfd.FileName, SMDH.Write());
        }
        private void exportIcon(bool large)
        {
            var sfd = new SaveFileDialog
            {
                FileName = large ? "Large Icon.png" : "Small Icon.png",
                Filter = "Icon Image " + (large ? "48x48" : "24x24") + "|*.png"
            };
            if (sfd.ShowDialog() != DialogResult.OK) 
                return;

            using (MemoryStream ms = new MemoryStream())
            {
                //error will throw from here
                (large ? SMDH.LargeIcon.Icon : SMDH.SmallIcon.Icon).Save(ms, ImageFormat.Png);
                byte[] data = ms.ToArray();
                File.WriteAllBytes(sfd.FileName, data);
            }
        }

        private void B_ImportSMDH_Click(object sender, EventArgs e)
        {
            var ofd = new OpenFileDialog
            {
                FileName = "icon.bin",
                Filter = "System Menu Data Header|*.*"
            };
            if (ofd.ShowDialog() != DialogResult.OK) return;

            openFile(ofd.FileName);
        }
        private void B_ImportSmallIcon_Click(object sender, EventArgs e)
        {
            var ofd = new OpenFileDialog
            {
                FileName = "small.png",
                Filter = "Small Icon Image|*.png"
            };
            if (ofd.ShowDialog() != DialogResult.OK) return;

            openFile(ofd.FileName);
        }
        private void B_ImportLargeIcon_Click(object sender, EventArgs e)
        {
            var ofd = new OpenFileDialog
            {
                FileName = "large.png",
                Filter = "Large Icon Image|*.png"
            };
            if (ofd.ShowDialog() != DialogResult.OK) return;

            openFile(ofd.FileName);
        }
        private void importSMDH(byte[] data, bool prompt = false)
        {
            if (prompt && DialogResult.Yes != Util.Prompt(MessageBoxButtons.YesNo, "Replace SMDH?"))
                return;

            CTR.SMDH newSMDH = new CTR.SMDH(data);
            if (newSMDH.LargeIcon.Icon == null) return;

            SMDH = newSMDH;
            entry = -1; // allow proper refreshing
            LoadSMDH();
        }
        private void importIcon(byte[] data, bool prompt = false)
        {
            try
            {
                using (Stream BitmapStream = new MemoryStream(data)) // Open the file, even if it is in use.
                {
                    Image img = Image.FromStream(BitmapStream);
                    Bitmap mBitmap = new Bitmap(img);

                    bool small = img.Width == 24 && img.Height == 24;
                    bool large = img.Width == 48 && img.Height == 48;

                    if (!small && !large)
                        Util.Alert("Image size is not correct.",
                            $"Width: {img.Width}\nHeight: {img.Height}",
                            "Expected Dimensions (24x24 or 48x48)");
                    if (prompt && DialogResult.Yes != Util.Prompt(MessageBoxButtons.YesNo, "Import image?", small ? "Small Icon" : "Large Icon"))
                        return;
                    if (small)
                        SMDH.SmallIcon.ChangeIcon(mBitmap);
                    if (large)
                        SMDH.LargeIcon.ChangeIcon(mBitmap);
                }
            }
            catch
            { Util.Error("Invalid image format?"); }
        }

        private int entry = -1;
        private void CB_AppInfo_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (entry > -1)
            {
                SMDH.AppInfo[entry].ShortDescription = TB_Short.Text;
                SMDH.AppInfo[entry].LongDescription = TB_Long.Text;
                SMDH.AppInfo[entry].Publisher = TB_Publisher.Text;
            }
            entry = CB_AppInfo.SelectedIndex;
            TB_Short.Text = SMDH.AppInfo[entry].ShortDescription;
            TB_Long.Text = SMDH.AppInfo[entry].LongDescription;
            TB_Publisher.Text = SMDH.AppInfo[entry].Publisher;
        }
    }
}