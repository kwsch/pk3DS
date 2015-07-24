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

            // Reload Previous Editing Files if the file exists
            if (File.Exists("config.ini"))
            {
                string[] lines = File.ReadAllLines("config.ini");
                string path = lines[0];
                int lang;
                if (lines.Length > 1 && int.TryParse(lines[1], out lang)) 
                    CB_Lang.SelectedIndex = lang;
                if (Directory.Exists("personal") && !skipBoth) { Directory.Delete("personal", true); } // Clear data on form load.
                if (path.Length > 0) openQuick(path);
            } 
            string filename = Path.GetFileNameWithoutExtension(System.Diagnostics.Process.GetCurrentProcess().MainModule.FileName);
            skipBoth = (filename.IndexOf("3DSkip", StringComparison.Ordinal) >= 0);
        }
        public static bool oras;
        public static string RomFSPath;
        public static string ExeFSPath;
        public static string ExHeaderPath;
        public volatile int threads;
        internal volatile static int Language;
        internal static CTR.SMDH SMDH;
        internal static string[] allGARCs = { "gametext", "storytext", "personal", "trpoke", "trdata", "evolution", "megaevo", "levelup", "eggmove", "item", "move", "maisonpkS", "maisontrS", "maisonpkN", "maisontrN", "titlescreen", "mapMatrix", "mapGR" };
        private bool skipBoth;

        // Main Form Methods
        private void L_About_Click(object sender, EventArgs e)
        {
            Util.Alert
                (
                "pk3DS: A package of Pokémon X/Y/OR/AS tools by various contributors.",

                "GARCTool (Backbone): Kaphotics" + Environment.NewLine +
                "Text Editing (xytext): Kaphotics" + Environment.NewLine +
                "Wild Editor (**WE): SciresM & Kaphotics" + Environment.NewLine +
                "Trainer Editor (**TE): SciresM, Kaphotics, and KazoWAR" + Environment.NewLine +
                "Personal Editor: SciresM" + Environment.NewLine +
                "Mega Evolution Editor (MEE): SciresM" + Environment.NewLine +
                "Evolutions, Moves, Items, Maison, Title Screen Editors: Kaphotics",
                "ExeFS Editors: Kaphotics (thanks Drayano and magical!)",

                "Big thanks to the ProjectPokemon community!"
                );
        }
        private void L_GARCInfo_Click(object sender, EventArgs e)
        {
            if (RomFSPath != null)
            {
                string s = "Game Type: " + ((oras) ? "ORAS" : "XY") + Environment.NewLine;
                s = allGARCs.Aggregate(s, (current, t) => current + String.Format(Environment.NewLine + "{0} - {1}", t, getGARCFileName(t)));

                if (DialogResult.Yes != Util.Prompt(MessageBoxButtons.YesNo, s, "Copy to Clipboard?")) return;

                try { Clipboard.SetText(s); }
                catch { Util.Alert("Unable to copy to Clipboard."); }
            }
        }
        private void L_Game_Click(object sender, EventArgs e)
        {
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
            if (InvokeRequired)
                Invoke((MethodInvoker)delegate { Language = CB_Lang.SelectedIndex; });
            else Language = CB_Lang.SelectedIndex;
            Menu_Options.DropDown.Close();
            if (!GB_RomFS.Enabled) return;

            updateGameInfo();
            new Thread(() =>
            {
                // Let all other operations finish first (ie, if the user quickly switches languages on load)
                while (threads > 0) Thread.Sleep(50);
                // Gather the Text Language Strings
                updateStatus(String.Format("GARC Get: {0} @ {1}... ", "gametext", getGARCFileName("gametext")));
                threadGet(RomFSPath + getGARCFileName("gametext"), "gametext", true, true);
                while (threads > 0) Thread.Sleep(50);
                if (Directory.Exists("personal")) return;
                updateStatus(String.Format("GARC Get: {0} @ {1}... ", "personal", getGARCFileName("personal")));
                threadGet(RomFSPath + getGARCFileName("personal"), "personal", true, true);
            }).Start();
        }
        private void Menu_Exit_Click(object sender, EventArgs e)
        {
            Close();
        }
        private void formClosing(object sender, FormClosingEventArgs e)
        {
            while (threads > 0) { Util.Alert("Please wait for all operations to finish first."); }

            updateStatus(Environment.NewLine + Environment.NewLine + "Saving data and closing the program...");
            try
            {
                if (TB_Path.Text.Length > 0) File.WriteAllLines("config.ini", new[] { TB_Path.Text, CB_Lang.SelectedIndex.ToString() });
                if (!GB_RomFS.Enabled || skipBoth) return; // No data/threads need to be addressed if we haven't loaded anything.

                // Set the GameText back as other forms may have edited it.
                updateStatus(String.Format("GARC Get: {0} @ {1}... ", "gametext", getGARCFileName("gametext")));
                threadSet(RomFSPath + getGARCFileName("gametext"), "gametext", false);
                while (threads > 0) Thread.Sleep(100);

                Thread.Sleep(200); // Small gap between beeps for faster computers.

                updateStatus(String.Format("GARC Get: {0} @ {1}... ", "personal", getGARCFileName("personal")));
                threadSet(RomFSPath + getGARCFileName("personal"), "personal", false);
                while (threads > 0) Thread.Sleep(100);

                if (Directory.Exists("gametext")) Directory.Delete("gametext", true);
                if (Directory.Exists("personal")) Directory.Delete("personal", true);
            }
            catch { }
        }
        private void openQuick(string path)
        {
            if (threads > 0) { Util.Alert("Please wait for all operations to finish first."); return; }

            if (!Directory.Exists(path)) // File
            {
                FileInfo fi = new FileInfo(path);
                if (fi.Name.Contains("code.bin")) // Compress/Decompress .code.bin
                {
                    if (fi.Length % 0x200 == 0 && (Util.Prompt(MessageBoxButtons.YesNo, "Detected Decompressed code.bin.", "Compress? File will be replaced.") == DialogResult.Yes))
                        new Thread(() => { threads++; new CTR.BLZCoder(new[] { "-en", path }, pBar1); threads--; Util.Alert("Compressed!"); }).Start();
                    else if (Util.Prompt(MessageBoxButtons.YesNo, "Detected Compressed code.bin.", "Decompress? File will be replaced.") == DialogResult.Yes)
                        new Thread(() => { threads++; new CTR.BLZCoder(new[] { "-d", path }, pBar1); threads--; Util.Alert("Decompressed!"); }).Start();
                }
                else if (fi.Name.ToLower().Contains("exe")) // Unpack exefs
                {
                    if (fi.Length % 0x200 == 0 && (Util.Prompt(MessageBoxButtons.YesNo, "Detected ExeFS.bin.", "Unpack?") == DialogResult.Yes))
                        new Thread(() => { threads++; CTR.ExeFS.get(path, Path.GetDirectoryName(path)); threads--; Util.Alert("Unpacked!"); }).Start();
                }
                else if (fi.Name.ToLower().Contains("rom"))
                {
                    Util.Alert("RomFS unpacking not implemented.");
                }
            }
            else // Directory
            {
                // Check for ROMFS/EXEFS/EXHEADER
                RomFSPath = ExeFSPath = null; // Reset
                string[] folders = Directory.GetDirectories(path);
                int count = folders.Length;

                // Find RomFS folder
                foreach (string f in folders.Where(f => new DirectoryInfo(f).Name.ToLower().Contains("rom") && Directory.Exists(f)))
                    checkIfRomFS(f);
                // Find ExeFS folder
                foreach (string f in folders.Where(f => new DirectoryInfo(f).Name.ToLower().Contains("exe") && Directory.Exists(f)))
                    checkIfExeFS(f);

                if (count > 3)
                    Util.Alert("pk3DS will function best if you keep your Game Files folder clean and free of unnecessary folders.");

                // Enable buttons if applicable
                GB_RomFS.Enabled = (RomFSPath != null);
                GB_ExeFS.Enabled = (RomFSPath != null && ExeFSPath != null);
                B_MoveTutor.Enabled = oras; // Default false unless loaded
                if (RomFSPath != null)
                {
                    if (L_Game.Text == "Game Loaded: ORAS" || L_Game.Text == "Game Loaded: XY")
                    { Directory.Delete("personal", true); } // Force reloading of personal data if the game is switched.
                    L_Game.Text = (oras) ? "Game Loaded: ORAS" : "Game Loaded: XY"; TB_Path.Text = path; 
                }
                else if (ExeFSPath != null)
                { L_Game.Text = "ExeFS loaded - no RomFS"; TB_Path.Text = path; }
                else
                { L_Game.Text = "No Game Loaded"; TB_Path.Text = ""; }

                if (RomFSPath != null)
                {
                    // Trigger Data Loading
                    if (RTB_Status.Text.Length > 0) RTB_Status.Clear();
                    updateStatus("Data found! Loading persistent data for subforms...", false);
                    changeLanguage(null, null);
                }

                // Enable Rebuilding options if all files have been found
                checkIfExHeader(path);
                Menu_ExeFS.Enabled = ExeFSPath != null;
                Menu_RomFS.Enabled = Menu_Restore.Enabled = Menu_GARCs.Enabled = RomFSPath != null;
                Menu_Patch.Enabled = RomFSPath != null && ExeFSPath != null;
                Menu_3DS.Enabled = 
                    (ExHeaderPath != null && RomFSPath != null && ExeFSPath != null);

                // Change L_Game if RomFS and ExeFS exists to a better descriptor
                SMDH = ExeFSPath != null ? (File.Exists(Path.Combine(ExeFSPath, "icon.bin"))) ? new CTR.SMDH(Path.Combine(ExeFSPath, "icon.bin")) : null : null;
                L_Game.Visible = (SMDH == null && RomFSPath != null);
                updateGameInfo();
                TB_Path.Select(TB_Path.TextLength, 0);
                // Method finished.
                SystemSounds.Asterisk.Play();
            }
        }

        private void updateGameInfo()
        {
            // 0 - JP
            // 1 - EN
            // 2 - FR
            // 3 - DE
            // 4 - IT
            // 5 - ES
            // 6 - XX
            // 7 - KO
            if (SMDH == null) return;
            int[] AILang = { 0, 0, 1, 2, 4, 3, 5, 7 };
            Text = "pk3DS - " + SMDH.AppInfo[AILang[Language]].ShortDescription;
        }
        private int checkGameType(string[] files)
        {
            switch (files.Length)
            {
                case 299: // ORAS
                    return 1;
                case 301: // ORAS Demo
                    return 1;
                case 271: // XY
                    return 0;
                default:  // Unknown
                    return -1;
            }
        }
        private bool checkIfRomFS(string path)
        {
            string[] top = Directory.GetDirectories(path);
            FileInfo fi = new FileInfo(top[(top.Length > 1) ? 1 : 0]);
            // Check to see if the folder is romfs
            if (fi.Name == "a")
            {
                int game = checkGameType(Directory.GetFiles(path, "*", SearchOption.AllDirectories));
                if (game < 0) // Unknown
                {
                    DialogResult dr = Util.Prompt(MessageBoxButtons.YesNoCancel, "Loading Override Options:", "Yes - OR/AS" + Environment.NewLine + "No - X/Y" + Environment.NewLine + "Cancel - Abort", "Path:" + Environment.NewLine + path);
                    if (dr == DialogResult.Yes) { oras = true; }
                    else if (dr == DialogResult.No) { oras = false; }
                    else { RomFSPath = null; oras = false; return false; }
                }
                else
                    oras = (game == 1);
                RomFSPath = path;
                backupGARCs(false, allGARCs);
                return true;
            }
            RomFSPath = null; 
            oras = false; 
            return false;
        }
        private bool checkIfExeFS(string path)
        {
            string[] files = Directory.GetFiles(path);
            if (files.Length != 3 && files.Length != 4) return false;

            FileInfo fi = new FileInfo(files[0]);
            if (!fi.Name.Contains("code")) return false;
            if (fi.Length % 0x200 != 0 && (Util.Prompt(MessageBoxButtons.YesNo, "Detected Compressed code.bin.", "Decompress? File will be replaced.") == DialogResult.Yes))
                new Thread(() => { threads++; new CTR.BLZCoder(new[] { "-d", files[0] }, pBar1); threads--; Util.Alert("Decompressed!"); }).Start();

            ExeFSPath = path;
            return true;
        }
        private bool checkIfExHeader(string path)
        {
            ExHeaderPath = null;
            // Input folder path should contain the ExHeader.
                string[] files = Directory.GetFiles(path);
                foreach (string fp in (from s in files let f = new FileInfo(s) where (f.Name.ToLower().StartsWith("exh") && f.Length == 0x800) select s))
                ExHeaderPath = fp;

            return ExHeaderPath != null;
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
            if (RomFSPath == null) return;
            if (Util.Prompt(MessageBoxButtons.YesNo, "Rebuild RomFS?") != DialogResult.Yes) return;

            SaveFileDialog sfd = new SaveFileDialog {FileName = "romfs.bin", Filter = "Binary File|*.*"};
            if (sfd.ShowDialog() == DialogResult.OK)
            {
                new Thread(() =>
                {
                    updateStatus(Environment.NewLine + "Building RomFS binary. Please wait until the program finishes.");

                    threads++;
                    CTR.RomFS.BuildRomFS(sfd.FileName, RomFSPath, RTB_Status, pBar1);
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
                Invoke((Action)(() => new MaisonEditor(super).ShowDialog()));
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
                string[] files = { "trdata", "trpoke", "move" }; // Moves required for smart randomization
                fileGet(files);
                Invoke((Action)(() => new RSTE().ShowDialog()));
                fileSet(files);
            }).Start();
        }
        private void B_Wild_Click(object sender, EventArgs e)
        {
            if (threads > 0) { Util.Alert("Please wait for all operations to finish first."); return; }
            bool advanced = (ModifierKeys == Keys.Alt);
            new Thread(() =>
            {
                if (advanced)
                {
                    string[] files = { "encdata", "storytext", "mapGR", "mapMatrix" };
                    if (files.Sum(t => Directory.Exists(t) ? 0 : 1) != 0) // Dev bypass if all exist already
                        fileGet(files, false);
                    Invoke((MethodInvoker)delegate { Enabled = false; });
                    {
                        Invoke((Action)(() => new xytext(Directory.GetFiles("storytext")).Show()));
                        Invoke((Action)(() => new OWSE().Show()));
                        while (Application.OpenForms.Count > 1)
                            Thread.Sleep(200);
                    }
                    Invoke((MethodInvoker)delegate { Enabled = true; });
                    fileSet(files);
                }
                else
                {
                    string[] files = { "encdata" };
                    fileGet(files, false);
                    if (oras) 
                    { Invoke((Action)(() => new RSWE().ShowDialog())); }
                    else 
                    { Invoke((Action)(() => new XYWE().ShowDialog())); }
                    fileSet(files);
                }
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
                Invoke((Action)(() => new ItemEditor().ShowDialog()));
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
                string[] files = { "levelup", "move" };
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
                string[] files = { "eggmove", "move" };
                fileGet(files);
                Invoke((Action)(() => new EggMove().ShowDialog()));
                fileSet(files);
            }).Start();
        }
        private void B_Wallpaper_Click(object sender, EventArgs e)
        {
            if (threads > 0) { Util.Alert("Please wait for all operations to finish first."); return; }
            new Thread(() =>
            {
                string[] files = { "titlescreen" };
                fileGet(files); // Compressed files exist, handled in the other form since there's so many
                Invoke((Action)(() => new TitleScreen().ShowDialog()));
                fileSet(files);
            }).Start();
        }
        // RomFS File Requesting Method Wrapper
        private void fileGet(string[] files, bool skipDecompression = true, bool skipGet = false)
        {
            if (ModifierKeys == (Keys.Control | Keys.Shift)) restoreGARCs(oras, files.ToArray());
            if (skipGet || skipBoth) return;
            foreach (string toEdit in files)
            {
                string GARC = getGARCFileName(toEdit);
                updateStatus(String.Format("GARC Get: {0} @ {1}... ", toEdit, GARC));
                threadGet(RomFSPath + GARC, toEdit, true, skipDecompression);
                while (threads > 0) Thread.Sleep(50);
            }
        }
        private void fileSet(IEnumerable<string> files, bool keep = false)
        {
            if (skipBoth) return;
            foreach (string toEdit in files)
            {
                string GARC = getGARCFileName(toEdit);
                updateStatus(String.Format("GARC Set: {0} @ {1}... ", toEdit, GARC));
                threadSet(RomFSPath + GARC, toEdit);
                while (threads > 0) Thread.Sleep(50);
                if (!keep && Directory.Exists(toEdit)) Directory.Delete(toEdit, true);
            }
        }

        // ExeFS Subform Items
        private void rebuildExeFS(object sender, EventArgs e)
        {
            if (ExeFSPath == null) return;
            if (Util.Prompt(MessageBoxButtons.YesNo, "Rebuild ExeFS?") != DialogResult.Yes) return;

            string[] files = Directory.GetFiles(ExeFSPath);
            int file = 0; if (files[1].Contains("code")) file = 1;
            SaveFileDialog sfd = new SaveFileDialog {FileName = "exefs.bin", Filter = "Binary File|*.*"};
            if (sfd.ShowDialog() == DialogResult.OK)
            {
                new Thread(() =>
                {
                    threads++;
                    new CTR.BLZCoder(new[] { "-en", files[file] }, pBar1);
                    Util.Alert("Compressed!");
                    CTR.ExeFS.set(Directory.GetFiles(ExeFSPath), sfd.FileName);
                    threads--;
                }).Start();
            }
        }
        private void B_Pickup_Click(object sender, EventArgs e)
        {
            if (threads > 0) { Util.Alert("Please wait for all operations to finish first."); return; }
            if (ExeFSPath != null) new Pickup().Show();
        }
        private void B_TMHM_Click(object sender, EventArgs e)
        {
            if (threads > 0) { Util.Alert("Please wait for all operations to finish first."); return; }
            if (ExeFSPath != null) new TMHM().Show();
        }
        private void B_Mart_Click(object sender, EventArgs e)
        {
            if (threads > 0) { Util.Alert("Please wait for all operations to finish first."); return; }
            if (ExeFSPath != null) new Mart().Show();
        }
        private void B_MoveTutor_Click(object sender, EventArgs e)
        {
            if (threads > 0) { Util.Alert("Please wait for all operations to finish first."); return; }
            if (!oras) { Util.Alert("No Tutors for X/Y."); return; } // Already disabled button...
            if (ExeFSPath != null) new Tutors().Show();
        }
        private void B_OPower_Click(object sender, EventArgs e)
        {
            if (threads > 0) { Util.Alert("Please wait for all operations to finish first."); return; }
            if (ExeFSPath != null) new OPower().Show();
        }

        // 3DS Building
        private void B_Rebuild3DS_Click(object sender, EventArgs e)
        {
            // Ensure that the romfs paths are valid
            if (checkGameType(Directory.GetFiles(TB_Path.Text, "*", SearchOption.AllDirectories)) >= 0)
            {
                Util.Error("RomFS file count does not match the default game file count.");
                return;
            }
            if (threads > 0) { Util.Alert("Please wait for all operations to finish first."); return; }

            SaveFileDialog sfd = new SaveFileDialog
            {
                FileName = "newROM.3ds",
                Filter = "Binary File|*.*"
            };
            if (sfd.ShowDialog() != DialogResult.OK) return;
            string path = sfd.FileName;

            new Thread(() =>
            {
                threads++;
                CTR.Exheader exh = new CTR.Exheader(ExHeaderPath);
                CTR.CTR.buildROM(true, "Nintendo", ExeFSPath, RomFSPath, ExHeaderPath, exh.GetSerial(), path, pBar1,
                    RTB_Status);
                threads--;
            }).Start();
        }

        // Extra Tools
        private void L_SubTools_Click(object sender, EventArgs e)
        {
            new ToolsUI().ShowDialog();
        }
        private void B_Patch_Click(object sender, EventArgs e)
        {
            new Patch().ShowDialog();
        }
        private void Menu_BLZ_Click(object sender, EventArgs e)
        {
            var ofd = new OpenFileDialog();
            if (DialogResult.Yes != ofd.ShowDialog()) return;

            string path = ofd.FileName;
            FileInfo fi = new FileInfo(path);
            if (fi.Length > 15 * 1024 * 1024) // 15MB
            { Util.Error("File too big!", fi.Length + " bytes."); return; }

            if (ModifierKeys != Keys.Control && fi.Length % 0x200 == 0 && (Util.Prompt(MessageBoxButtons.YesNo, "Detected Decompressed Binary.", "Compress? File will be replaced.") == DialogResult.Yes))
                new Thread(() => { threads++; new CTR.BLZCoder(new[] { "-en", path }, pBar1); threads--; Util.Alert("Compressed!"); }).Start();
            else if (Util.Prompt(MessageBoxButtons.YesNo, "Detected Compressed Binary", "Decompress? File will be replaced.") == DialogResult.Yes)
                new Thread(() => { threads++; new CTR.BLZCoder(new[] { "-d", path }, pBar1); threads--; Util.Alert("Decompressed!"); }).Start();
        }
        private void Menu_LZ11_Click(object sender, EventArgs e)
        {
            var ofd = new OpenFileDialog();
            if (DialogResult.Yes != ofd.ShowDialog()) return;

            string path = ofd.FileName;
            FileInfo fi = new FileInfo(path);
            if (fi.Length > 15*1024*1024) // 15MB
            { Util.Error("File too big!", fi.Length + " bytes."); return; }

            byte[] data = File.ReadAllBytes(path);
            string predict = data[0] == 0x11 ? "compressed" : "decompressed";
            var dr = Util.Prompt(MessageBoxButtons.YesNoCancel, String.Format("Detected {0} file. Do what?", predict),
                "Yes = Decompress\nNo = Compress\nCancel = Abort");
            new Thread(() =>
            {
                threads++; 
                if (dr == DialogResult.Yes)
                {
                    CTR.LZSS.Decompress(path, path);
                    Util.Alert("File Decompressed!", path);
                }
                if (dr == DialogResult.No)
                {
                    CTR.LZSS.Compress(path, path);
                    Util.Alert("File Compressed!", path);
                }
                threads--;
            }).Start();
        }
        private void Menu_SMDH_Click(object sender, EventArgs e)
        {
            new Icon().ShowDialog();
        }

        // GARC Requests
        private string getGARCFileName(string requestedGARC)
        {
            return getGARCFileName(requestedGARC, Language);
        }
        internal static string getGARCFileName(string requestedGARC, int lang)
        {
            string ans = "";
            switch (requestedGARC)
            {
                case "movesprite": ans = getGARCPath(0, 0, 5); break;
                case "encdata": ans = (oras) ? getGARCPath(0, 1, 3) : getGARCPath(0, 1, 2); break;
                case "trdata": ans = (oras) ? getGARCPath(0, 3, 6) : getGARCPath(0, 3, 8); break;
                case "trpoke": ans = (oras) ? getGARCPath(0, 3, 8) : getGARCPath(0, 4, 0); break;
                case "mapGR": ans = (oras) ? getGARCPath(0, 3, 9) : getGARCPath(0, 4, 1); break;
                case "mapMatrix": ans = (oras) ? getGARCPath(0, 4, 0) : getGARCPath(0, 4, 2); break;
                case "gametext": ans = (oras) ? getGARCPath(0, 7, 1 + lang) : getGARCPath(0, 7, 2 + lang); break;
                case "storytext": ans = (oras) ? getGARCPath(0, 7 + ((lang + 9) / 10), (10 + (lang + 9)) % 10) : getGARCPath(0, 8, lang); break;
                case "wallpaper": ans = (oras) ? getGARCPath(1, 0, 3) : getGARCPath(1, 0, 4); break;
                case "titlescreen": ans = (oras) ? getGARCPath(1, 5, 2) : getGARCPath(1, 6, 5); break;
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
        internal static string getGARCPath(int A, int B, int C)
        {
            return String.Format("{0}a{0}{1}{0}{2}{0}{3}", Path.DirectorySeparatorChar, A, B, C);
        }
        public bool getGARC(string infile, string outfolder, bool PB, bool bypassExt = false)
        {
            if (skipBoth && Directory.Exists(outfolder))
            {
                updateStatus(String.Format("Skipped - Exists!"), false);
                threads--;
                return true;
            }
            try
            {
                bool success = CTR.GARC.garcUnpack(infile, outfolder, bypassExt, (PB) ? pBar1 : null, null, true, bypassExt);
                updateStatus(String.Format(success ? "Success!" : "Failed!"), false);
                threads--;
                return success;
            }
            catch (Exception e) { Util.Error("Could not get the GARC:", e.ToString()); threads--; return false; }
        }
        public bool setGARC(string outfile, string infolder, bool PB)
        {
            if (skipBoth || (ModifierKeys == Keys.Control && Util.Prompt(MessageBoxButtons.YesNo, "Cancel writing data back to GARC?") == DialogResult.Yes))
            { threads--; updateStatus(String.Format("Aborted!"), false); return false; }

            try
            {
                bool success = CTR.GARC.garcPackMS(infolder, outfile, (PB) ? pBar1 : null, null, true);
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
                    File.Copy(RomFSPath + GARC, dest);
            }
        }
        public void restoreGARCs(bool oras_define, params string[] g)
        {
            oras = oras_define;
            foreach (string s in g)
            {
                string dest = RomFSPath + getGARCFileName(s);
                string src = "backup" + Path.DirectorySeparatorChar + s + String.Format(" ({0})", getGARCFileName(s).Replace(Path.DirectorySeparatorChar.ToString(), ""));
                File.Copy(src, dest, true);
                if (s == "personal" || s == "gametext")
                    Util.Alert("In order to restore " + s + ", restart the program. While exiting, hold the Control Key to prevent writebacks.");
                        // Reload the persistent data.
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
            string path = "gametext" + Path.DirectorySeparatorChar + file.ToString("000") + ".bin";
            File.WriteAllBytes(path, data);
            return true;
        }

        // Update RichTextBox
        public void updateStatus(string status, bool preBreak = true)
        {
            string newtext = ((preBreak) ? Environment.NewLine : "") + status;
            try
            {
                if (RTB_Status.InvokeRequired)
                    RTB_Status.Invoke((MethodInvoker)delegate
                    {
                        RTB_Status.AppendText(newtext);
                        RTB_Status.SelectionStart = RTB_Status.Text.Length;
                        RTB_Status.ScrollToCaret();
                    });
                else
                {
                    RTB_Status.AppendText(newtext);
                    RTB_Status.SelectionStart = RTB_Status.Text.Length;
                    RTB_Status.ScrollToCaret();
                }
            }
            catch { }
        }
    }
}