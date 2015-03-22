/*----------------------------------------------------------------------------*/
/*--  This program is free software: you can redistribute it and/or modify  --*/
/*--  it under the terms of the GNU General Public License as published by  --*/
/*--  the Free Software Foundation, either version 3 of the License, or     --*/
/*--  (at your option) any later version.                                   --*/
/*--                                                                        --*/
/*--  This program is distributed in the hope that it will be useful,       --*/
/*--  but WITHOUT ANY WARRANTY; without even the implied warranty of        --*/
/*--  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the          --*/
/*--  GNU General Public License for more details.                          --*/
/*--                                                                        --*/
/*--  You should have received a copy of the GNU General Public License     --*/
/*--  along with this program. If not, see <http://www.gnu.org/licenses/>.  --*/
/*----------------------------------------------------------------------------*/

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Media;
using System.Threading;
using System.Windows.Forms;
using blz;

namespace pk3DS
{
    public sealed partial class Main : Form
    {
        public Main()
        {
            // Initialize the Main Form
            InitializeComponent();

            // Set the Current Language to English
            CB_Lang.SelectedIndex = 2;

            // Prepare DragDrop Functionality
            AllowDrop = GB_RomFS.AllowDrop = TB_Path.AllowDrop = true;
            DragEnter += tabMain_DragEnter;
            DragDrop += tabMain_DragDrop;
            GB_RomFS.DragEnter += tabMain_DragEnter;
            GB_RomFS.DragDrop += tabMain_DragDrop;
            TB_Path.DragEnter += tabMain_DragEnter;
            TB_Path.DragDrop += tabMain_DragDrop;

            // Rebuilding Functionality
            GB_RomFS.Click += rebuildRomFS; // Not tested, need some verification first.
            GB_ExeFS.Click += rebuildExeFS;

            // Reload Previous Editing Files if the file exists
            if (File.Exists("config.ini"))
            {
                string path = File.ReadAllText("config.ini");
                if (Directory.Exists("personal")) { Directory.Delete("personal", true); } // Clear data on form load.
                if (path.Length > 0) openQuick(path);
            }
        }
        public static bool oras;
        public static string RomFS;
        public static string ExeFS;
        public volatile int threads;
        private static string[] allGARCs = { "gametext", "storytext", "personal", "trpoke", "trdata", "evolution", "megaevo", "levelup", "eggmove", "item", "move", "maisonpkS", "maisontrS", "maisonpkN", "maisontrN" };

        // Main Form Methods
        private void L_About_Click(object sender, EventArgs e)
        {
            if (ModifierKeys == Keys.Control && RomFS != null)
            {
                string s = "Game Type: " + ((oras) ? "ORAS" : "XY") + Environment.NewLine;
                s = allGARCs.Aggregate(s, (current, t) => current + String.Format(Environment.NewLine + "{0} - {1}", t, getGARCFileName(t)));

                if (DialogResult.Yes != Util.Prompt(MessageBoxButtons.YesNo, s, "Copy to Clipboard?")) return;

                try { Clipboard.SetText(s); }
                catch { Util.Alert("Unable to copy to Clipboard."); }
            }
            else
                Util.Alert
                    (
                    "pk3DS: A package of Pokémon X/Y/OR/AS tools by various contributors.",

                    "GARCTool (Backbone): Kaphotics" + Environment.NewLine +
                    "Text Editing (xytext): Kaphotics" + Environment.NewLine +
                    "Wild Editor (**WE): SciresM & Kaphotics" + Environment.NewLine +
                    "Trainer Editor (**TE): SciresM, Kaphotics, and KazoWAR" + Environment.NewLine +
                    "Personal Editor: SciresM" + Environment.NewLine +
                    "Mega Evolution Editor (MEE): Huntereb & SciresM",

                    "After integrating standalone tools, the following editors were implemented:" + Environment.NewLine +
                    "Evolutions, Moves, Items, Maison, and ExeFS Editors: Kaphotics",

                    "Big thanks to the ProjectPokemon community!"
                    );
        }
        private void L_Game_Click(object sender, EventArgs e)
        {
            if (GB_RomFS.Enabled && RomFS != null && ModifierKeys == Keys.Control)
                if (DialogResult.Yes == Util.Prompt(MessageBoxButtons.YesNo, "Restore Original Files?"))
                    restoreGARCs(oras, allGARCs);
        }
        private void B_Open_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog fbd = new FolderBrowserDialog();
            if (fbd.ShowDialog() == DialogResult.OK)
                openQuick(fbd.SelectedPath);
        }
        private void changeLanguage(object sender, EventArgs e)
        {
            if (!GB_RomFS.Enabled) return;
            new Thread(() =>
            {
                // Gather the Text Language Strings
                updateStatus(String.Format("GARC Get: {0} @ {1}... ", "gametext", getGARCFileName("gametext")));
                threadGet(RomFS + getGARCFileName("gametext"), "gametext", true, true);
                while (threads > 0) Thread.Sleep(50);
                if (!Directory.Exists("personal"))
                {
                    updateStatus(String.Format("GARC Get: {0} @ {1}... ", "personal", getGARCFileName("personal")));
                    threadGet(RomFS + getGARCFileName("personal"), "personal", true, true);
                }
            }).Start();
        }
        private void formClosing(object sender, FormClosingEventArgs e)
        {
            while (threads > 0) { Util.Alert("Please wait for all operations to finish first."); }

            updateStatus(Environment.NewLine + Environment.NewLine + "Saving data and closing the program...");
            try
            {
                if (TB_Path.Text.Length > 0) File.WriteAllText("config.ini", TB_Path.Text);
                if (!GB_RomFS.Enabled) return; // No data/threads need to be addressed if we haven't loaded anything.

                // Set the GameText back as other forms may have edited it.
                updateStatus(String.Format("GARC Get: {0} @ {1}... ", "gametext", getGARCFileName("gametext")));
                threadSet(RomFS + getGARCFileName("gametext"), "gametext", false);
                while (threads > 0) Thread.Sleep(100);

                Thread.Sleep(200); // Small gap between beeps for faster computers.

                updateStatus(String.Format("GARC Get: {0} @ {1}... ", "personal", getGARCFileName("personal")));
                threadSet(RomFS + getGARCFileName("personal"), "personal", false);
                while (threads > 0) Thread.Sleep(100);

                if (Directory.Exists("gametext")) Directory.Delete("gametext", true);
                if (Directory.Exists("personal")) Directory.Delete("personal", true);
            }
            catch { }
        }
        private void openQuick(string path)
        {
            FileInfo fi = new FileInfo(path);
            if (!Directory.Exists(path)) return;

            if (threads > 0) { Util.Alert("Please wait for all operations to finish first."); return; }
            if (fi.Name.Contains("code.bin"))
            {
                if (fi.Length % 0x200 == 0 && (Util.Prompt(MessageBoxButtons.YesNo, "Detected Decompressed code.bin.", "Compress? File will be replaced.") == DialogResult.Yes))
                    new Thread(() => { threads++; new BLZCoder(new[] { "-en", path }, pBar1); threads--; Util.Alert("Compressed!"); }).Start();
                else if (Util.Prompt(MessageBoxButtons.YesNo, "Detected Compressed code.bin.", "Decompress? File will be replaced.") == DialogResult.Yes)
                    new Thread(() => { threads++; new BLZCoder(new[] { "-d", path }, pBar1); threads--; Util.Alert("Decompressed!"); }).Start();
            }
            else
            {
                // Check for ROMFS/EXEFS
                RomFS = ExeFS = null; // Reset
                string[] folders = Directory.GetDirectories(path);
                int count = folders.Length;
                if (count != 2 && count != 1) return; // Only want exefs & romfs (can have exheader there too, it's not a folder)
                {
                    // First file should be 'exe'
                    if (new FileInfo(folders[0]).Name.ToLower().Contains("exe") && Directory.Exists(folders[0]))
                        checkIfExeFS(folders[0]);
                    if (new FileInfo(folders[count - 1]).Name.ToLower().Contains("rom") && Directory.Exists(folders[count - 1]))
                        checkIfRomFS(folders[count - 1]);

                    GB_RomFS.Enabled = (RomFS != null);
                    GB_ExeFS.Enabled = (RomFS != null && ExeFS != null);
                    B_MoveTutor.Enabled = oras; // Default false unless loaded
                    if (RomFS != null)
                    {
                        if (L_Game.Text == "Game Loaded: ORAS" || L_Game.Text == "Game Loaded: XY")
                        { Directory.Delete("personal", true); } // Force reloading of personal data if the game is switched.
                        L_Game.Text = (oras) ? "Game Loaded: ORAS" : "Game Loaded: XY"; TB_Path.Text = path; 
                    }
                    else if (ExeFS != null)
                    { L_Game.Text = "ExeFS loaded - no RomFS"; TB_Path.Text = path; }
                    else
                    { L_Game.Text = "No Game Loaded"; TB_Path.Text = ""; }

                    if (RomFS != null)
                    {
                        // Trigger Data Loading
                        if (RTB_Status.Text.Length > 0) RTB_Status.Clear();
                        updateStatus("Data found! Loading persistent data for subforms...", false);
                        changeLanguage(null, null);
                    }

                    // Method finished.
                    SystemSounds.Asterisk.Play();
                }
            }
        }
        private bool checkIfRomFS(string path)
        {
            string[] top = Directory.GetDirectories(path);
            FileInfo fi = new FileInfo(top[(top.Length > 1) ? 1 : 0]);
            // Check to see if the folder is romfs
            if (fi.Name == "a")
            {
                string[] files = Directory.GetFiles(path, "*", SearchOption.AllDirectories);
                if (files.Length == 299) // ORAS
                    oras = true;
                else if (files.Length == 301) // ORAS demo
                    oras = true;
                else if (files.Length == 271) // XY
                    oras = false;
                else // Allow Override
                {
                    DialogResult dr = Util.Prompt(MessageBoxButtons.YesNoCancel, "Loading Override Options:", "Yes - OR/AS" + Environment.NewLine + "No - X/Y" + Environment.NewLine + "Cancel - Abort", "Path:" + Environment.NewLine + path);
                    if (dr == DialogResult.Yes) { oras = true; }
                    else if (dr == DialogResult.No) { oras = false; }
                    else { RomFS = null; oras = false; return false; }
                }
                RomFS = path;
                backupGARCs(false, allGARCs);
                return true;
            }
            RomFS = null; 
            oras = false; 
            return false;
        }
        private bool checkIfExeFS(string path)
        {
            string[] files = Directory.GetFiles(path);
            if (files.Length != 3) return false;

            FileInfo fi = new FileInfo(files[0]);
            if (fi.Name.Contains("code"))
            {
                if (fi.Length % 0x200 != 0 && (Util.Prompt(MessageBoxButtons.YesNo, "Detected Compressed code.bin.", "Decompress? File will be replaced.") == DialogResult.Yes))
                    new Thread(() => { threads++; new BLZCoder(new[] { "-d", files[0] }, pBar1); threads--; Util.Alert("Decompressed!"); }).Start();

                ExeFS = path;
                return true;
            }
            return false;
        }
        private void tabMain_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop)) e.Effect = DragDropEffects.Copy;
        }
        private void tabMain_DragDrop(object sender, DragEventArgs e)
        {
            string[] files = (string[])e.Data.GetData(DataFormats.FileDrop);
            string path = files[0]; // open first D&D
            openQuick(path);
        }

        // RomFS Subform Items
        private void rebuildRomFS(object sender, EventArgs e)
        {
            if (threads > 0) { Util.Alert("Please wait for all operations to finish first."); return; }
            if (RomFS == null) return;
            if (Util.Prompt(MessageBoxButtons.YesNo, "Rebuild RomFS?") != DialogResult.Yes) return;

            SaveFileDialog sfd = new SaveFileDialog {FileName = "romfs.bin", Filter = "Binary File|*.*"};
            if (sfd.ShowDialog() == DialogResult.OK)
            {
                new Thread(() =>
                {
                    updateStatus(Environment.NewLine + "Building RomFS binary. Please wait until the program finishes.");

                    threads++;
                    RomFSTool.BuildRomFS(sfd.FileName, RomFS, RTB_Status, pBar1);
                    threads--;

                    updateStatus("RomFS binary saved." + Environment.NewLine);
                    Util.Alert("Wrote RomFS binary:", sfd.FileName);
                }).Start();
            }
        }
        private void B_GameText_Click(object sender, EventArgs e)
        {
            if (threads > 0) { Util.Alert("Please wait for all operations to finish first."); return; }
            new Thread(() =>
            {
                string[] files = { "gametext" };
                fileGet(files, false, true);
                Invoke((Action)(() => new xytext(Directory.GetFiles("gametext")).ShowDialog()));
                fileSet(files, true);
            }).Start();
        }
        private void B_StoryText_Click(object sender, EventArgs e)
        {
            if (threads > 0) { Util.Alert("Please wait for all operations to finish first."); return; }
            new Thread(() =>
            {
                string[] files = { "storytext" };
                fileGet(files);
                Invoke((Action)(() => new xytext(Directory.GetFiles("storytext")).ShowDialog()));
                fileSet(files);
            }).Start();
        }
        private void B_Maison_Click(object sender, EventArgs e)
        {
            if (threads > 0) { Util.Alert("Please wait for all operations to finish first."); return; }
            DialogResult dr = Util.Prompt(MessageBoxButtons.YesNoCancel, "Edit Super Maison instead of Normal Maison?", "Yes = Super, No = Normal, Cancel = Abort");
            if (dr == DialogResult.Cancel) return;

            new Thread(() =>
            {
                bool super = (dr == DialogResult.Yes);
                string[] files = { (super) ? "maisontrS" : "maisontrN", (super) ? "maisonpkS" : "maisonpkN" };
                fileGet(files);
                Invoke((Action)(() => new Maison(super).ShowDialog()));
                fileSet(files);
            }).Start();
        }
        private void B_Personal_Click(object sender, EventArgs e)
        {
            if (threads > 0) { Util.Alert("Please wait for all operations to finish first."); return; }
            new Thread(() =>
            {
                string[] files = { "personal" };
                fileGet(files, false, true);
                Invoke((Action)(() => new Personal().ShowDialog()));
                fileSet(files, true);
            }).Start();
        }
        private void B_Trainer_Click(object sender, EventArgs e)
        {
            if (threads > 0) { Util.Alert("Please wait for all operations to finish first."); return; }
            new Thread(() =>
            {
                string[] files = { "trdata", "trpoke" };
                fileGet(files);
                Invoke((Action)(() => new RSTE().ShowDialog()));
                fileSet(files);
            }).Start();
        }
        private void B_Wild_Click(object sender, EventArgs e)
        {
            if (threads > 0) { Util.Alert("Please wait for all operations to finish first."); return; }
            new Thread(() =>
            {
                string[] files = { "encdata" };
                fileGet(files, false);
                if (oras) { Invoke((Action)(() => new RSWE().ShowDialog())); }
                else { Invoke((Action)(() => new XYWE().ShowDialog())); }
                fileSet(files);
            }).Start();
        }
        private void B_Evolution_Click(object sender, EventArgs e)
        {
            if (threads > 0) { Util.Alert("Please wait for all operations to finish first."); return; }
            new Thread(() =>
            {
                string[] files = { "evolution" };
                fileGet(files);
                Invoke((Action)(() => new Evolution().ShowDialog()));
                fileSet(files);
            }).Start();
        }
        private void B_MegaEvo_Click(object sender, EventArgs e)
        {
            if (threads > 0) { Util.Alert("Please wait for all operations to finish first."); return; }
            new Thread(() =>
            {
                string[] files = { "megaevo" };
                fileGet(files);
                Invoke((Action)(() => new MEE().ShowDialog()));
                fileSet(files);
            }).Start();
        }
        private void B_Item_Click(object sender, EventArgs e)
        {
            if (threads > 0) { Util.Alert("Please wait for all operations to finish first."); return; }
            new Thread(() =>
            {
                string[] files = { "item" };
                fileGet(files);
                Invoke((Action)(() => new Item().ShowDialog()));
                fileSet(files);
            }).Start();
        }
        private void B_Move_Click(object sender, EventArgs e)
        {
            if (threads > 0) { Util.Alert("Please wait for all operations to finish first."); return; }
            new Thread(() =>
            {
                string[] files = { "move" };
                fileGet(files);
                Invoke((Action)(() => new Moves().ShowDialog()));
                fileSet(files);
            }).Start();
        }
        private void B_LevelUp_Click(object sender, EventArgs e)
        {
            if (threads > 0) { Util.Alert("Please wait for all operations to finish first."); return; }
            new Thread(() =>
            {
                string[] files = { "levelup" };
                fileGet(files);
                Invoke((Action)(() => new LevelUp().ShowDialog()));
                fileSet(files);
            }).Start();
        }
        private void B_EggMove_Click(object sender, EventArgs e)
        {
            if (threads > 0) { Util.Alert("Please wait for all operations to finish first."); return; }
            new Thread(() =>
            {
                string[] files = { "eggmove" };
                fileGet(files);
                Invoke((Action)(() => new EggMove().ShowDialog()));
                fileSet(files);
            }).Start();
        }
        // RomFS File Requesting Method Wrapper
        private void fileGet(IEnumerable<string> files, bool skipDecompression = true, bool skipGet = false)
        {
            if (skipGet) return;
            foreach (string toEdit in files)
            {
                string GARC = getGARCFileName(toEdit);
                updateStatus(String.Format("GARC Get: {0} @ {1}... ", toEdit, GARC));
                threadGet(RomFS + GARC, toEdit, true, skipDecompression);
                while (threads > 0) Thread.Sleep(50);
            }
        }
        private void fileSet(IEnumerable<string> files, bool keep = false)
        {
            foreach (string toEdit in files)
            {
                string GARC = getGARCFileName(toEdit);
                updateStatus(String.Format("GARC Set: {0} @ {1}... ", toEdit, GARC));
                threadSet(RomFS + GARC, toEdit);
                while (threads > 0) Thread.Sleep(50);
                if (!keep && Directory.Exists(toEdit)) Directory.Delete(toEdit, true);
            }
        }

        // ExeFS Subform Items
        private void rebuildExeFS(object sender, EventArgs e)
        {
            if (ExeFS == null) return;
            if (Util.Prompt(MessageBoxButtons.YesNo, "Rebuild ExeFS?") != DialogResult.Yes) return;

            string[] files = Directory.GetFiles(ExeFS);
            int file = 0; if (files[1].Contains("code")) file = 1;
            SaveFileDialog sfd = new SaveFileDialog {FileName = "exefs.bin", Filter = "Binary File|*.*"};
            if (sfd.ShowDialog() == DialogResult.OK)
            {
                new Thread(() =>
                {
                    threads++;
                    new BLZCoder(new[] { "-en", files[file] }, pBar1);
                    Util.Alert("Compressed!");
                    ExeFSTool.set(Directory.GetFiles(ExeFS), sfd.FileName);
                    threads--;
                }).Start();
            }
        }
        private void B_Pickup_Click(object sender, EventArgs e)
        {
            if (threads > 0) { Util.Alert("Please wait for all operations to finish first."); return; }
            if (ExeFS != null) new Pickup().Show();
        }
        private void B_TMHM_Click(object sender, EventArgs e)
        {
            if (threads > 0) { Util.Alert("Please wait for all operations to finish first."); return; }
            if (ExeFS != null) new TMHM().Show();
        }
        private void B_Mart_Click(object sender, EventArgs e)
        {
            if (threads > 0) { Util.Alert("Please wait for all operations to finish first."); return; }
            if (ExeFS != null) new Mart().Show();
        }
        private void B_MoveTutor_Click(object sender, EventArgs e)
        {
            if (threads > 0) { Util.Alert("Please wait for all operations to finish first."); return; }
            if (!oras) { Util.Alert("No Tutors for X/Y."); return; } // Already disabled button...
            if (ExeFS != null) new Tutors().Show();
        }

        // GARC Requests
        public string getGARCFileName(string requestedGARC)
        {
            int lang = 0;
            if (CB_Lang.InvokeRequired)
                CB_Lang.Invoke((MethodInvoker)delegate { lang = CB_Lang.SelectedIndex; });
            else lang = CB_Lang.SelectedIndex;

            string ans = "";
            switch (requestedGARC)
            {
                case "encdata": ans = (oras) ? getGARCPath(0, 1, 3) : getGARCPath(0, 1, 2); break;
                case "trdata": ans = (oras) ? getGARCPath(0, 3, 6) : getGARCPath(0, 3, 8); break;
                case "trpoke": ans = (oras) ? getGARCPath(0, 3, 8) : getGARCPath(0, 4, 0); break;
                case "gametext": ans = (oras) ? getGARCPath(0, 7, 1 + lang) : getGARCPath(0, 7, 2 + lang); break;
                case "storytext": ans = (oras) ? getGARCPath(0, 7 + ((lang + 9) / 10), (10 + (lang + 9)) % 10) : getGARCPath(0, 8, lang); break;
                case "maisonpkN": ans = (oras) ? getGARCPath(1, 8, 2) : getGARCPath(2, 0, 3); break;
                case "maisontrN": ans = (oras) ? getGARCPath(1, 8, 3) : getGARCPath(2, 0, 4); break;
                case "maisonpkS": ans = (oras) ? getGARCPath(1, 8, 4) : getGARCPath(2, 0, 5); break;
                case "maisontrS": ans = (oras) ? getGARCPath(1, 8, 5) : getGARCPath(2, 0, 6); break;
                case "move": ans = (oras) ? getGARCPath(1, 8, 9) : getGARCPath(2, 1, 2); break;
                case "eggmove": ans = (oras) ? getGARCPath(1, 9, 0) : getGARCPath(2, 1, 3); break;
                case "levelup": ans = (oras) ? getGARCPath(1, 9, 1) : getGARCPath(2, 1, 4); break;
                case "evolution": ans = (oras) ? getGARCPath(1, 9, 2) : getGARCPath(2, 1, 5); break;
                case "megaevo": ans = (oras) ? getGARCPath(1, 9, 3) : getGARCPath(2, 1, 6); break;
                case "personal": ans = (oras) ? getGARCPath(1, 9, 5) : getGARCPath(2, 1, 8); break;
                case "item": ans = (oras) ? getGARCPath(1, 9, 7) : getGARCPath(2, 2, 0); break;
            }
            return ans;
        }
        public string getGARCPath(int A, int B, int C)
        {
            return String.Format("{0}a{0}{1}{0}{2}{0}{3}", Path.DirectorySeparatorChar, A, B, C);
        }
        public bool getGARC(string infile, string outfolder, bool PB, bool bypassExt = false)
        {
            try
            {
                bool success = GARCTool.garcUnpack(infile, outfolder, bypassExt, (PB) ? pBar1 : null, null, true, bypassExt);
                updateStatus(String.Format(success ? "Success!" : "Failed!"), false);
                threads--;
                return success;
            }
            catch (Exception e) { Util.Error("Could not get the GARC:", e.ToString()); threads--; return false; }
        }
        public bool setGARC(string outfile, string infolder, bool PB)
        {
            if (ModifierKeys == Keys.Control && Util.Prompt(MessageBoxButtons.YesNo, "Cancel writing data back to GARC?") == DialogResult.Yes)
            { threads--; updateStatus(String.Format("Aborted!"), false); return false; }

            try
            {
                bool success = GARCTool.garcPackMS(infolder, outfile, (PB) ? pBar1 : null, null, true);
                threads--;
                updateStatus(String.Format(success ? "Success!" : "Failed!"), false);
                return success;
            }
            catch (Exception e) { Util.Error("Could not set the GARC back:", e.ToString()); threads--; return false; }
        }
        public void threadGet(string infile, string outfolder, bool PB = true, bool bypassExt = false)
        {
            threads++;
            if (Directory.Exists(outfolder)) try { Directory.Delete(outfolder, true); }
                catch { }
            Thread thread = new Thread(() => getGARC(infile, outfolder, PB, bypassExt));
            thread.Start();
        }
        public void threadSet(string outfile, string infolder, bool PB = true)
        {
            threads++;
            Thread thread = new Thread(() => setGARC(outfile, infolder, PB));
            thread.Start();
        }
        public void backupGARCs(bool overwrite, params string[] g)
        {
            if (!Directory.Exists("backup")) Directory.CreateDirectory("backup");
            foreach (string s in g)
            {
                string GARC = getGARCFileName(s);
                string dest = "backup" + Path.DirectorySeparatorChar + s + String.Format(" ({0})", GARC.Replace(Path.DirectorySeparatorChar.ToString(), ""));
                if (overwrite || !File.Exists(dest))
                    File.Copy(RomFS + GARC, dest);
            }
        }
        public void restoreGARCs(bool oras_define, params string[] g)
        {
            oras = oras_define;
            foreach (string s in g)
            {
                string dest = RomFS + getGARCFileName(s);
                string src = "backup" + Path.DirectorySeparatorChar + s + String.Format(" ({0})", getGARCFileName(s).Replace(Path.DirectorySeparatorChar.ToString(), ""));
                File.Copy(src, dest, true);
            }
            Util.Alert(g.Length + " files restored.");
        }

        // Text Requests
        internal static string[] getText(int file)
        {
            return xytext.getStringsFromFile("gametext" + Path.DirectorySeparatorChar + file.ToString("000") + ".bin");
        }
        internal static bool setText(int file, string[] strings)
        {
            byte[] data = xytext.getBytesForFile(strings);
            string path = "text" + Path.DirectorySeparatorChar + file.ToString("000") + ".bin";
            File.WriteAllBytes(path, data);
            return true;
        }

        // Update RichTextBox
        public void updateStatus(string status, bool preBreak = true)
        {
            try
            {
                if (RTB_Status.InvokeRequired)
                    RTB_Status.Invoke((MethodInvoker)delegate
                    {
                        RTB_Status.AppendText(((preBreak) ? Environment.NewLine : "") + status);
                        RTB_Status.SelectionStart = RTB_Status.Text.Length;
                        RTB_Status.ScrollToCaret();
                    });
                else
                {
                    RTB_Status.AppendText(status + Environment.NewLine);
                    RTB_Status.SelectionStart = RTB_Status.Text.Length;
                    RTB_Status.ScrollToCaret();
                }
            }
            catch { }
        }
    }
}