﻿using pk3DS.Core.CTR;
using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using pk3DS.Core;

namespace pk3DS
{
    public sealed partial class TitleScreenEditor6 : Form
    {
        private readonly bool compressed = Main.Config.ORAS;

        public TitleScreenEditor6()
        {
            InitializeComponent();
            AllowDrop = true;
            DragEnter += TC_Main_DragEnter;
            DragDrop += TC_Main_DragDrop;
            PB_Image.AllowDrop = true;
            PB_Image.DragEnter += TC_Main_DragEnter;
            PB_Image.DragDrop += TC_Main_DragDrop;

            // Add tooltip to image
            new ToolTip().SetToolTip(PB_Image, "Click to toggle Green Screen\nRightClick for I/O\nCTRL+Click for Copy->Clipboard.");

            // Add context menus
            ContextMenuStrip mnu = new ContextMenuStrip();
            ToolStripMenuItem mnuR = new ToolStripMenuItem("Replace with...");
            ToolStripMenuItem mnuS = new ToolStripMenuItem("Save as...");
            // Assign event handlers
            mnuR.Click += ClickOpen;
            mnuS.Click += ClickSave;
            // Add to main context menu
            mnu.Items.AddRange(new ToolStripItem[] { mnuR, mnuS, });

            // Assign
            PB_Image.ContextMenuStrip = mnu;

            // Set up languages
            string[] languages = (Main.Config.ORAS ? new[] {"JP1"} : Array.Empty<string>()).Concat(new[] {"DE", "ES", "FR", "IT", "JP", "KO", "EN"}).ToArray();
            string[] games = Main.Config.ORAS ? new[] {"OR", "AS"} : new[] {"X", "Y"};
            for (int i = 0; i < darcs.Length/2; i++)
                CB_DARC.Items.Add($"{games[0]} - {languages[i]}");
            for (int i = darcs.Length/2; i < darcs.Length; i++)
                CB_DARC.Items.Add($"{games[1]} - {languages[i - (darcs.Length/2)]}");

            // Load darcs
            for (int i = 0; i < darcs.Length; i++)
            {
                // Get DARC name and assign the decompressed name
                usedFiles[i] = "titlescreen\\" + (compressed ? "dec_" : "") + Path.GetFileName(files[darcFiles[i]]);
                if (compressed) // Decompress file (XY does not compress)
                    LZSS.Decompress(files[darcFiles[i]], usedFiles[i]);
                // Read decompressed file
                var data = File.ReadAllBytes(usedFiles[i]);

                // Find darc data offset (ignore header)
                int pos = 0;
                while (BitConverter.ToUInt32(data, pos) != 0x63726164)
                {
                    pos += 4;
                    if (pos >= data.Length)
                        throw new Exception("Invalid DARC?\n\n" + usedFiles[i]);
                }
                var darcData = data.Skip(pos).ToArray();
                darcs[i] = new DARC(darcData);
            }

            CB_DARC.SelectedIndex = CB_DARC.Items.Count - 1; // last (english game2)
        }

        private readonly string[] files = Directory.GetFiles("titlescreen");
        private readonly DARC[] darcs = new DARC[2 * (Main.Config.ORAS ? 8 : 7)];
        private readonly string[] usedFiles = new string[2 * (Main.Config.ORAS ? 8 : 7)];

        private readonly int[] darcFiles = Main.Config.ORAS
            ? new[]
            {
                1120, 1121, 1122, 1123, 1124, 1125, 1126, 1127,
                1128, 1129, 1130, 1131, 1132, 1133, 1134, 1135,
            }: new[]
            {
                467, 468, 469, 470, 471, 472, 473,
                474, 475, 476, 477, 478, 479, 480,
            };

        private void ChangeDARC(object sender, EventArgs e)
        {
            // When the darc is changed, we need to re-load the files
            CB_File.Items.Clear();
            var darc = darcs[CB_DARC.SelectedIndex];
            for (int i = 0; i < darc.Entries.Length; i++)
            {
                if (darc.FileNameTable[i].FileName.Contains(".bclim"))
                    CB_File.Items.Add(darc.FileNameTable[i].FileName);
            }

            CB_File.SelectedIndex = CB_File.Items.Count - 1; // Load last (version)
        }

        private void ChangeFile(object sender, EventArgs e)
        {
            // When the file is changed, we need to display the new file.
            string filename = CB_File.Text;
            int entry = -1;
            // Find entry in darc
            var darc = darcs[CB_DARC.SelectedIndex];
            for (int i = 0; i < darc.Entries.Length; i++)
            {
                if (darc.FileNameTable[i].FileName != filename)
                    continue;
                entry = i;
                break;
            }

            if (entry < 0) throw new Exception("File not found!?");

            // Load file
            var en = darc.Entries[entry];
            var data = new byte[en.DataLength];
            var ofs = en.DataOffset - darc.Header.FileDataOffset;
            Array.Copy(darc.Data, ofs, data, 0, data.Length);
            BCLIM bclim = BCLIM.Analyze(data, filename);
            Image CropBMP = bclim.GetBitmap();

            PB_Image.Image = CropBMP;
            // store image locally for saving if need be
            currentBytes = data;

            L_Dimensions.Text = $"Dimensions: {PB_Image.Width}w && {PB_Image.Height}h";
        }

        private byte[] currentBytes;

        private void InsertFile(string path)
        {
            if (DialogResult.Yes != WinFormsUtil.Prompt(MessageBoxButtons.YesNo, "Overwrite image?"))
                return;
            byte[] data = File.ReadAllBytes(path);
            byte[] bclim;

            if (Path.GetExtension(path) == ".bclim") // bclim opened
            {
                var img = BCLIM.Analyze(data, path);
                if (img.Width != PB_Image.Width || img.Height != PB_Image.Height)
                {
                    WinFormsUtil.Alert("Image sizes do not match.",
                        $"Width: {img.Width} - {PB_Image.Width}\nHeight: {img.Height} - {PB_Image.Height}");
                    return;
                }
                bclim = data;
            }
            else // image
            {
                using Stream BitmapStream = new MemoryStream(data);
                Image img = Image.FromStream(BitmapStream);
                if (img.Width != PB_Image.Width || img.Height != PB_Image.Height)
                {
                    WinFormsUtil.Alert("Image sizes do not match.",
                        $"Width: {img.Width} - {PB_Image.Width}\nHeight: {img.Height} - {PB_Image.Height}");
                    return;
                }
                bclim = BCLIM.IMGToBCLIM(img, '9');
            }

            string filename = CB_File.Text;
            int entry = -1;
            // Find entry in darc
            var darc = darcs[CB_DARC.SelectedIndex];
            for (int i = 0; i < darc.Entries.Length; i++)
            {
                if (darc.FileNameTable[i].FileName == filename)
                {
                    entry = i;
                    break;
                }
            }

            if (entry < 0) throw new Exception("File not found!?");

            DARC.InsertFile(ref darc, entry, bclim);
            darcs[CB_DARC.SelectedIndex] = darc;

            // Trigger reloading of the image
            ChangeFile(null, null);
        }

        // Dropping file in
        private void TC_Main_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop)) e.Effect = DragDropEffects.Copy;
        }

        private void TC_Main_DragDrop(object sender, DragEventArgs e)
        {
            string path = ((string[])e.Data.GetData(DataFormats.FileDrop))[0]; // open first D&D
            InsertFile(path);
        }

        private void Form_Closing(object sender, FormClosingEventArgs e)
        {
            if (compressed)
                WinFormsUtil.Alert("Recompressing may take some time...", "Don't panic if the Progress Bar doesn't move!");
            // Write darcs
            for (int i = 0; i < darcs.Length; i++)
            {
                var data = File.ReadAllBytes(usedFiles[i]);
                int pos = 0;
                while (BitConverter.ToUInt32(data, pos) != 0x63726164)
                {
                    pos += 4;
                    if (pos >= data.Length) return;
                }
                byte[] preData = data.Take(pos).ToArray();
                byte[] darcData = DARC.SetDARC(darcs[i]);
                byte[] newData = preData.Concat(darcData).ToArray();

                byte[] oldDarc = File.ReadAllBytes(usedFiles[i]);
                if (newData.SequenceEqual(oldDarc)) // if same, just continue.
                {
                    if (compressed)
                        File.Delete(usedFiles[i]); // Use old compressed file (speedup)
                }
                else // File is different, replace and allow repacking to compress.
                {
                    if (compressed)
                        File.Delete(files[darcFiles[i]]); // delete the old compressed file
                    File.WriteAllBytes(usedFiles[i], newData); // write the new edited (uncompressed) file
                }
            }
        }

        private void ClickSave(object sender, EventArgs e)
        {
            var sfd = new SaveFileDialog
            {
                FileName = Path.GetFileNameWithoutExtension(CB_File.Text),
                Filter = "PNG Image|*.png|BCLIM Image|*.bclim"
            };
            if (sfd.ShowDialog() != DialogResult.OK) return;
            if (sfd.FilterIndex == 2) // BCLIM
            {
                byte[] data = currentBytes;
                File.WriteAllBytes(sfd.FileName, data);
            }
            else // PNG
            {
                Image img = PB_Image.Image;
                using MemoryStream ms = new MemoryStream();
                //error will throw from here
                img.Save(ms, ImageFormat.Png);
                byte[] data = ms.ToArray();
                File.WriteAllBytes(sfd.FileName, data);
            }
        }

        private void ClickOpen(object sender, EventArgs e)
        {
            var ofd = new OpenFileDialog
            {
                FileName = Path.GetFileNameWithoutExtension(CB_File.Text),
                Filter = "PNG Image|*.png|BCLIM Image|*.bclim"
            };
            if (ofd.ShowDialog() != DialogResult.OK) return;

            InsertFile(ofd.FileName);
        }

        private void PB_Image_Click(object sender, EventArgs e)
        {
            if (ModifierKeys == Keys.Control && WinFormsUtil.Prompt(MessageBoxButtons.YesNo, "Copy image to clipboard?") == DialogResult.Yes)
                Clipboard.SetImage(PB_Image.BackgroundImage);
            else if (PB_Image.BackColor == Color.Transparent)
                PB_Image.BackColor = Color.GreenYellow;
            else PB_Image.BackColor = Color.Transparent;
        }
    }
    #region Documentation
    /* Files: 
         * X
         * 467 - X (DE)
         * 468 - X (ES)
         * 469 - X (FR)
         * 470 - X (IT)
         * 471 - X (JP)
         * 472 - X (KO)
         * 473 - X (EN)
         * Y
         * 474 - Y (DE)
         * 475 - Y (ES)
         * 476 - Y (FR)
         * 477 - Y (IT)
         * 478 - Y (JP)
         * 479 - Y (KO)
         * 480 - Y (EN)
         * 
         * Ruby
         * 1120 - オメガルビー (JP) [Single File]
         * 1121 - Omega Rubin (DE)
         * 1122 - Rubí Omega (ES)
         * 1123 - Rubis Oméga (FR)
         * 1124 - Rubino Omega (IT)
         * 1125 - オメガルビー (JP)
         * 1126 - 오메가루비 (KO)
         * 1127 - Omega Ruby (EN)
         * Sapphire
         * 1128 - アルファサファイア (JP) [Single File]
         * 1129 - Alpha Saphir (DE)
         * 1130 - Zafiro Alfa (ES)
         * 1131 - Saphir Alpha (FR)
         * 1132 - Zaffiro Alpha (IT)
         * 1133 - アルファサファイア (JP)
         * 1134 - 알파사파이어 (KO)
         * 1135 - Alpha Sapphire (EN)
        */
    /* X/Y Title Logos
     * \timg\logo_*00.bclim - White Blurred GameVersion & Colored VersionXY
     * \timg\logo_*01.bclim - Blurred GameVersion
     * \timg\logo_*02.bclim - Blurred GameVersion & Blurred Colored VersionXY
     * \timg\logo_*03.bclim - Version
    */
    /* OR/AS Title Logos [Single File]
     * \timg\title_logo_sapphire.bclim
    */
    /* OR/AS Title Logos (Separate)
     * \timg\titlelogo_*02.bclim - Blurred Logo
     * \timg\titlelogo_*04.bclim - Pokémon whiteback
     * \timg\titlelogo_*05.bclim - GameVersion Blurred 1
     * \timg\titlelogo_*06.bclim - GameVersion Blurred 2
     * \timg\title_logo_*01.bclim - Top half of logo (Pokémon)
     * \timg\title_logo_*02.bclim - Bottom half of logo (VERSION)
     * 
     * Note: JP/KO are appended with _j or _jp (_ko etc)
    */
    #endregion
}