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

            // Prepare DragDrop Functionality
            AllowDrop = TB_Path.AllowDrop = true;
            DragEnter += tabMain_DragEnter;
            DragDrop += tabMain_DragDrop;
            TB_Path.DragEnter += tabMain_DragEnter;
            TB_Path.DragDrop += tabMain_DragDrop;
            foreach (var t in TC_RomFS.TabPages.OfType<TabPage>())
            {
                t.AllowDrop = true;
                t.DragEnter += tabMain_DragEnter;
                t.DragDrop += tabMain_DragDrop;
            }

            // Reload Previous Editing Files if the file exists

            CB_Lang.SelectedIndex = Properties.Settings.Default.Language;
            if (!string.IsNullOrWhiteSpace(Properties.Settings.Default.GamePath))
                openQuick(Properties.Settings.Default.GamePath);

            string[] args = Environment.GetCommandLineArgs();
            string filename = args.Length > 0 ? Path.GetFileNameWithoutExtension(args[0])?.ToLower() : "";
            skipBoth = filename.IndexOf("3DSkip", StringComparison.Ordinal) >= 0;
        }
        internal static GameConfig Config;
        public static string RomFSPath;
        public static string ExeFSPath;
        public static string ExHeaderPath;
        private volatile int threads;
        internal static volatile int Language;
        internal static CTR.SMDH SMDH;
        private uint HANSgameID; // for exporting RomFS/ExeFS with correct X8 gameID
        private readonly bool skipBoth;
        public static PersonalInfo[] SpeciesStat => Config.Personal.Table;

        // Main Form Methods
        private void L_About_Click(object sender, EventArgs e)
        {
            new About().ShowDialog();
        }
        private void L_GARCInfo_Click(object sender, EventArgs e)
        {
            if (RomFSPath != null)
            {
                string s = "Game Type: " + Config.Version + Environment.NewLine;
                s = Config.Files.Select(file => file.Name).Aggregate(s, (current, t) => current + string.Format(Environment.NewLine + "{0} - {1}", t, Config.getGARCFileName(t)));

                if (DialogResult.Yes != Util.Prompt(MessageBoxButtons.YesNo, s, "Copy to Clipboard?")) return;

                try { Clipboard.SetText(s); }
                catch { Util.Alert("Unable to copy to Clipboard."); }
            }
        }
        private void L_Game_Click(object sender, EventArgs e)
        {
            if (DialogResult.Yes == Util.Prompt(MessageBoxButtons.YesNo, "Restore Original Files?"))
                restoreGARCs(Config.Files.Select(file => file.Name).ToArray());
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
            if (Config != null)
                Config.Language = Language;
            Menu_Options.DropDown.Close();
            if (!Tab_RomFS.Enabled || Config == null)
                return;

            if ((Config.XY || Config.ORAS) && Language > 7)
            {
                Util.Alert("Language not available for games. Defaulting to English.");
                if (InvokeRequired)
                    Invoke((MethodInvoker)delegate { CB_Lang.SelectedIndex = 2; });
                else CB_Lang.SelectedIndex = 2;
                return; // set event re-triggers this method
            }

            updateGameInfo();
            Config.InitializeGameText();
            Properties.Settings.Default.Language = Language;
            Properties.Settings.Default.Save();
        }
        private void Menu_Exit_Click(object sender, EventArgs e)
        {
            Close();
        }
        private void formClosing(object sender, FormClosingEventArgs e)
        {
            if (Config == null) return;
            var g = Config.GARCGameText;
            string[][] files = Config.GameTextStrings;
            g.Files = files.Select(TextFile.getBytes).ToArray();
            g.Save();
        }

        private void openQuick(string path)
        {
            if (threadActive()) return;

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
                else
                {
                    DialogResult dr = Util.Prompt(MessageBoxButtons.YesNoCancel, "Unpack sub-files?", "Cancel: Abort");
                    if (dr == DialogResult.Cancel)
                        return;
                    bool recurse = dr == DialogResult.Yes;
                    ToolsUI.openARC(path, pBar1, recurse);
                }
            }
            else // Directory
            {
                // Check for ROMFS/EXEFS/EXHEADER
                RomFSPath = ExeFSPath = null; // Reset
                Config = null;
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
                Tab_RomFS.Enabled = Menu_Restore.Enabled = Tab_CRO.Enabled = Menu_CRO.Enabled = Menu_Shuffler.Enabled = RomFSPath != null;
                Tab_ExeFS.Enabled = RomFSPath != null && ExeFSPath != null;
                if (RomFSPath != null)
                {
                    toggleSubEditors();
                    string newtext = $"Game Loaded: {Config.Version}";
                    if (L_Game.Text != newtext && Directory.Exists("personal"))
                    { Directory.Delete("personal", true); } // Force reloading of personal data if the game is switched.
                    L_Game.Text = newtext; TB_Path.Text = path; 
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
                    Config.Initialize(RomFSPath, ExeFSPath, Language);
                    backupGARCs(false, Config.Files.Select(file => file.Name).ToArray());
                    backupCROs(false, RomFSPath);
                }

                // Enable Rebuilding options if all files have been found
                checkIfExHeader(path);
                Menu_ExeFS.Enabled = ExeFSPath != null;
                Menu_RomFS.Enabled = Menu_Restore.Enabled = Menu_GARCs.Enabled = RomFSPath != null;
                Menu_Patch.Enabled = RomFSPath != null && ExeFSPath != null;
                Menu_3DS.Enabled = 
                    ExHeaderPath != null && RomFSPath != null && ExeFSPath != null;

                // Change L_Game if RomFS and ExeFS exists to a better descriptor
                SMDH = ExeFSPath != null ? File.Exists(Path.Combine(ExeFSPath, "icon.bin")) ? new CTR.SMDH(Path.Combine(ExeFSPath, "icon.bin")) : null : null;
                HANSgameID = SMDH != null ? (SMDH.AppSettings?.StreetPassID ?? 0) : 0;
                L_Game.Visible = SMDH == null && RomFSPath != null;
                updateGameInfo();
                TB_Path.Select(TB_Path.TextLength, 0);
                // Method finished.
                System.Media.SystemSounds.Asterisk.Play();
                resetStatus();
                Properties.Settings.Default.GamePath = path;
                Properties.Settings.Default.Save();
            }
        }

        private void toggleSubEditors()
        {
            // Hide all buttons
            foreach (var f in from TabPage t in TC_RomFS.TabPages from f in t.Controls.OfType<FlowLayoutPanel>() select f)
                for (int i = f.Controls.Count - 1; i >= 0; i--)
                    f.Controls.Remove(f.Controls[i]);

            B_MoveTutor.Visible = Config.ORAS; // Default false unless loaded

            Control[] romfs, exefs, cro;

            switch (Config.Generation)
            {
                case 6:
                    romfs = new Control[] {B_GameText, B_StoryText, B_Personal, B_Evolution, B_LevelUp, B_Wild, B_MegaEvo, B_EggMove, B_Trainer, B_Item, B_Move, B_Maison, B_TitleScreen, B_OWSE};
                    exefs = new Control[] {B_MoveTutor, B_TMHM, B_Mart, B_Pickup, B_OPower};
                    cro = new Control[] {B_TypeChart, B_Starter, B_Gift, B_Static};
                    B_MoveTutor.Visible = Config.ORAS; // Default false unless loaded
                    break;
                case 7:
                    romfs = new Control[] {B_GameText, B_StoryText, B_Personal, B_Evolution, B_LevelUp, B_Wild, B_MegaEvo, B_EggMove, B_Trainer, B_Item, B_Move, B_Maison};
                    exefs = new Control[] {B_TMHM, B_TypeChart};
                    cro = new Control[] {B_Mart};

                    if (Config.Version != GameVersion.SMDEMO)
                        romfs = romfs.Concat(new[] {B_Static}).ToArray();
                    break;
                default:
                    romfs = exefs = cro = new Control[] {new Label {Text = "No editors available."}};
                    break;
            }

            FLP_RomFS.Controls.AddRange(romfs);
            FLP_ExeFS.Controls.AddRange(exefs);
            FLP_CRO.Controls.AddRange(cro);
        }
        private void updateGameInfo()
        {
            // 0 - JP
            // 1 - EN
            // 2 - FR
            // 3 - DE
            // 4 - IT
            // 5 - ES
            // 6 - CHS
            // 7 - KO
            // 8 - 
            // 11 - CHT
            int[] AILang = { 0, 0, 1, 2, 4, 3, 5, 7, 8, 9, 6, 11 };
            Text = SMDH?.AppSettings == null
                ? "pk3DS" // nothing else
                : "pk3DS - " + SMDH.AppInfo[AILang[Language]].ShortDescription;
        }
        private static GameConfig checkGameType(string[] files)
        {
            try
            {
                if (files.Length > 1000)
                    return null;
                string[] fileArr = Directory.GetFiles(Path.Combine(Directory.GetParent(files[0]).FullName, "a"), "*", SearchOption.AllDirectories);
                var afiles = fileArr.Where(file => Path.GetFileName(file)?.Length == 1).ToArray();
                int fileCount = fileArr.Count(file => Path.GetFileName(file)?.Length == 1);
                return new GameConfig(fileCount);
            }
            catch { }
            return null;
        }
        private bool checkIfRomFS(string path)
        {
            string[] top = Directory.GetDirectories(path);
            FileInfo fi = new FileInfo(top[top.Length > 1 ? 1 : 0]);
            // Check to see if the folder is romfs
            if (fi.Name == "a")
            {
                string[] files = Directory.GetFiles(path, "*", SearchOption.AllDirectories);
                var cfg = checkGameType(files);

                if (cfg == null)
                {
                    RomFSPath = null;
                    Config = null;
                    Util.Error("File count does not match expected game count.", "Files: " + files.Length);
                    return false;
                }

                RomFSPath = path;
                Config = cfg;
                TextFile.Config = cfg;
                Randomizer.MaxSpeciesID = cfg.MaxSpeciesID;
                return true;
            }
            Util.Error("Folder does not contain an 'a' folder in the top level.");
            RomFSPath = null;
            return false;
        }
        private bool checkIfExeFS(string path)
        {
            string[] files = Directory.GetFiles(path);
            if (files.Length == 1 && Path.GetFileName(files[0]).ToLower() == "exefs.bin")
            {
                // Prompt if the user wants to unpack the ExeFS.
                if (DialogResult.Yes != Util.Prompt(MessageBoxButtons.YesNo, "Detected ExeFS binary.", "Unpack?"))
                    return false;

                // User wanted to unpack. Unpack.
                if (!CTR.ExeFS.get(files[0], path))
                    return false; // on unpack fail

                // Remove ExeFS binary after unpacking
                File.Delete(files[0]);

                files = Directory.GetFiles(path);
                // unpack successful, continue onward!
            }

            if (files.Length != 3 && files.Length != 4) 
                return false;

            FileInfo fi = new FileInfo(files[0]);
            if (!fi.Name.Contains("code"))
            {
                if (new FileInfo(files[1]).Name == "code.bin")
                {
                    File.Move(files[1], Path.Combine(Path.GetDirectoryName(files[1]), ".code.bin"));
                    files = Directory.GetFiles(path);
                    fi = new FileInfo(files[0]);
                }
                else
                    return false;
            }
            if (fi.Length % 0x200 != 0 && (Util.Prompt(MessageBoxButtons.YesNo, "Detected Compressed code binary.", "Decompress? File will be replaced.") == DialogResult.Yes))
                new Thread(() => { threads++; new CTR.BLZCoder(new[] { "-d", files[0] }, pBar1); threads--; Util.Alert("Decompressed!"); }).Start();

            ExeFSPath = path;
            return true;
        }
        private bool checkIfExHeader(string path)
        {
            ExHeaderPath = null;
            // Input folder path should contain the ExHeader.
                string[] files = Directory.GetFiles(path);
			foreach (string fp in from s in files let f = new FileInfo(s) where (f.Name.ToLower().StartsWith("exh") || f.Name.ToLower().StartsWith("decryptedexh")) && f.Length == 0x800 select s)
                ExHeaderPath = fp;

            return ExHeaderPath != null;
        }
        private bool threadActive()
        {
            if (threads <= 0) return false;
            Util.Alert("Please wait for all operations to finish first."); return true;
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
            if (threadActive()) return;
            if (RomFSPath == null) return;
            if (Util.Prompt(MessageBoxButtons.YesNo, "Rebuild RomFS?") != DialogResult.Yes) return;

            SaveFileDialog sfd = new SaveFileDialog
            {
                FileName = HANSgameID != 0 ? HANSgameID.ToString("X8") + ".romfs" : "romfs.bin",
                Filter = "HANS RomFS|*.romfs" + "|Binary File|*.bin" + "|All Files|*.*"
            };
            sfd.FilterIndex = HANSgameID != 0 ? 0 : sfd.Filter.Length - 1;

            if (sfd.ShowDialog() == DialogResult.OK)
            {
                new Thread(() =>
                {
                    updateStatus(Environment.NewLine + "Building RomFS binary. Please wait until the program finishes.");

                    threads++;
                    CTR.RomFS.BuildRomFS(RomFSPath, sfd.FileName, RTB_Status, pBar1);
                    threads--;

                    updateStatus("RomFS binary saved." + Environment.NewLine);
                    Util.Alert("Wrote RomFS binary:", sfd.FileName);
                }).Start();
            }
        }
        private void B_GameText_Click(object sender, EventArgs e)
        {
            if (threadActive()) return;
            new Thread(() =>
            {
                var g = Config.GARCGameText;
                string[][] files = Config.GameTextStrings;
                Invoke((Action)(() => new TextEditor(files, "gametext").ShowDialog()));
                g.Files = files.Select(TextFile.getBytes).ToArray();
                g.Save();
            }).Start();
        }
        private void B_StoryText_Click(object sender, EventArgs e)
        {
            if (threadActive()) return;
            new Thread(() =>
            {
                var g = Config.getGARCData("storytext");
                string[][] files = g.Files.Select(file => new TextFile(file).Lines).ToArray();
                Invoke((Action)(() => new TextEditor(files, "storytext").ShowDialog()));
                g.Files = files.Select(TextFile.getBytes).ToArray();
                g.Save();
            }).Start();
        }
        private void B_Maison_Click(object sender, EventArgs e)
        {
            if (threadActive()) return;
            DialogResult dr;
            switch (Config.Generation)
            {
                case 6:
                    dr = Util.Prompt(MessageBoxButtons.YesNoCancel, "Edit Super Maison instead of Normal Maison?", "Yes = Super, No = Normal, Cancel = Abort");
                    break;
                case 7:
                    dr = Util.Prompt(MessageBoxButtons.YesNoCancel, "Edit Battle Royal instead of Battle Tree?", "Yes = Royal, No = Tree, Cancel = Abort");
                    break;
                default:
                    return;
            }
            if (dr == DialogResult.Cancel) return;

            new Thread(() =>
            {
                bool super = dr == DialogResult.Yes;
                string c = super ? "S" : "N";
                var trdata = Config.getGARCData("maisontr"+c);
                var trpoke = Config.getGARCData("maisonpk"+c);
                byte[][] trd = trdata.Files;
                byte[][] trp = trpoke.Files;
                switch (Config.Generation)
                {
                    case 6:
                        Invoke((Action)(() => new MaisonEditor6(trd, trp, super).ShowDialog()));
                        break;
                    case 7:
                        Invoke((Action)(() => new MaisonEditor7(trd, trp, super).ShowDialog()));
                        break;
                }
                trdata.Files = trd;
                trpoke.Files = trp;
                trdata.Save();
                trpoke.Save();
            }).Start();
        }
        private void B_Personal_Click(object sender, EventArgs e)
        {
            if (threadActive()) return;
            new Thread(() =>
            {
                byte[][] d = Config.GARCPersonal.Files;
                switch (Config.Generation)
                {
                    case 6:
                        Invoke((Action)(() => new PersonalEditor6(d).ShowDialog()));
                        break;
                    case 7:
                        Invoke((Action)(() => new PersonalEditor7(d).ShowDialog()));
                        break;
                }
                // Set Master Table back
                for (int i = 0; i < d.Length - 1; i++)
                    d[i].CopyTo(d[d.Length-1], i * d[i].Length);

                Config.GARCPersonal.Files = d;
                Config.GARCPersonal.Save();
                Config.InitializePersonal();

            }).Start();
        }
        private void B_Trainer_Click(object sender, EventArgs e)
        {
            if (threadActive()) return;
            new Thread(() =>
            {
                var trclass = Config.getGARCData("trclass");
                var trdata = Config.getGARCData("trdata");
                var trpoke = Config.getGARCData("trpoke");
                byte[][] trc = trclass.Files;
                byte[][] trd = trdata.Files;
                byte[][] trp = trpoke.Files;

                switch (Config.Generation)
                {
                    case 6:
                        Invoke((Action)(() => new RSTE(trc, trd, trp).ShowDialog()));
                        break;
                    case 7:
                        Invoke((Action)(() => new SMTE(trc, trd, trp).ShowDialog()));
                        break;
                }
                trclass.Files = trc;
                trdata.Files = trd;
                trpoke.Files = trp;
                trclass.Save();
                trdata.Save();
                trpoke.Save();
            }).Start();
        }
        private void B_Wild_Click(object sender, EventArgs e)
        {
            if (threadActive()) return;
            new Thread(() =>
            {
                string[] files;
                Action action;
                switch (Config.Generation)
                {
                    case 6:
                        files = new[] { "encdata" };
                        if (Config.ORAS)
                            action = () => new RSWE().ShowDialog();
                        else if (Config.XY)
                            action = () => new XYWE().ShowDialog();
                        else return;

                        fileGet(files, false);
                        Invoke(action);
                        fileSet(files);
                        break;
                    case 7:
                        Invoke((MethodInvoker)delegate { Enabled = false; });
                        threads++;

                        files = new [] { "encdata", "zonedata", "worlddata" };
                        updateStatus($"GARC Get: {files[0]}... ");
                        var ed = Config.getlzGARCData(files[0]);
                        updateStatus($"GARC Get: {files[1]}... ");
                        var zd = Config.getlzGARCData(files[1]);
                        updateStatus($"GARC Get: {files[2]}... ");
                        var wd = Config.getlzGARCData(files[2]);
                        updateStatus("Running SMWE... ");
                        action = () => new SMWE(ed, zd, wd).ShowDialog();
                        Invoke(action);

                        updateStatus($"GARC Set: {files[0]}... ");
                        ed.Save();
                        resetStatus();
                        threads--;
                        Invoke((MethodInvoker)delegate { Enabled = true; });
                        break;
                    default:
                        return;
                }
            }).Start();
        }
        private void B_OWSE_Click(object sender, EventArgs e)
        {
            if (threadActive()) return;
            switch (Config.Generation)
            {
                case 6:
                    runOWSE6();
                    return;
                case 7:
                    runOWSE7();
                    return;
            }
        }
        private void runOWSE6()
        {
            Enabled = false;
            new Thread(() =>
            {
                bool reload = (ModifierKeys == Keys.Control) || ModifierKeys == (Keys.Alt | Keys.Control);
                string[] files = {"encdata", "storytext", "mapGR", "mapMatrix"};
                if (reload || files.Sum(t => Directory.Exists(t) ? 0 : 1) != 0) // Dev bypass if all exist already
                    fileGet(files, false);

                // Don't set any data back. Just view.
                {
                    var g = Config.getGARCData("storytext");
                    string[][] tfiles = g.Files.Select(file => new TextFile(file).Lines).ToArray();
                    Invoke((Action)(() => new OWSE().Show()));
                    Invoke((Action)(() => new TextEditor(tfiles, "storytext").Show()));
                    while (Application.OpenForms.Count > 1)
                        Thread.Sleep(200);
                }
                Invoke((MethodInvoker) delegate { Enabled = true; });
                fileSet(files);
            }).Start();
        }
        private void runOWSE7()
        {
            Enabled = false;
            new Thread(() =>
            {
                var files = new[] { "encdata", "zonedata", "worlddata" };
                updateStatus($"GARC Get: {files[0]}... ");
                var ed = Config.getlzGARCData(files[0]);
                updateStatus($"GARC Get: {files[1]}... ");
                var zd = Config.getlzGARCData(files[1]);
                updateStatus($"GARC Get: {files[2]}... ");
                var wd = Config.getlzGARCData(files[2]);

                var g = Config.getGARCData("storytext");
                string[][] tfiles = g.Files.Select(file => new TextFile(file).Lines).ToArray();
                Invoke((Action)(() => new TextEditor(tfiles, "storytext").Show()));
                Invoke((Action)(() => new OWSE7(ed, zd, wd).Show()));
                while (Application.OpenForms.Count > 1)
                    Thread.Sleep(200);
                Invoke((MethodInvoker)delegate { Enabled = true; });
            }).Start();
        }
        private void B_Evolution_Click(object sender, EventArgs e)
        {
            if (threadActive()) return;
            new Thread(() =>
            {
                var g = Config.getGARCData("evolution");
                byte[][] d = g.Files;
                switch (Config.Generation)
                {
                    case 6:
                        Invoke((Action)(() => new EvolutionEditor6(d).ShowDialog()));
                        break;
                    case 7:
                        Invoke((Action)(() => new EvolutionEditor7(d).ShowDialog()));
                        break;
                }
                g.Files = d;
                g.Save();
            }).Start();
        }
        private void B_MegaEvo_Click(object sender, EventArgs e)
        {
            if (threadActive()) return;
            new Thread(() =>
            {
                var g = Config.getGARCData("megaevo");
                byte[][] d = g.Files;
                switch (Config.Generation)
                {
                    case 6:
                        Invoke((Action)(() => new MegaEvoEditor6(d).ShowDialog()));
                        break;
                    case 7:
                        Invoke((Action)(() => new MegaEvoEditor7(d).ShowDialog()));
                        break;
                }
                g.Files = d;
                g.Save();
            }).Start();
        }
        private void B_Item_Click(object sender, EventArgs e)
        {
            if (threadActive()) return;
            new Thread(() =>
            {
                var g = Config.getGARCData("item");
                byte[][] d = g.Files;
                switch (Config.Generation)
                {
                    case 6:
                        Invoke((Action)(() => new ItemEditor6(d).ShowDialog()));
                        break;
                    case 7:
                        Invoke((Action)(() => new ItemEditor7(d).ShowDialog()));
                        break;
                }
                g.Files = d;
                g.Save();
            }).Start();
        }
        private void B_Move_Click(object sender, EventArgs e)
        {
            if (threadActive()) return;
            new Thread(() =>
            {
                var g = Config.GARCMoves;
                byte[][] Moves;
                switch (Config.Generation)
                {
                    case 6:
                        bool mini = Config.ORAS;
                        Moves = mini ? CTR.mini.unpackMini(g.getFile(0), "WD") : g.Files;
                        Invoke((Action)(() => new MoveEditor6(Moves).ShowDialog()));
                        g.Files = mini ? new[] { CTR.mini.packMini(Moves, "WD") } : Moves;
                        break;
                    case 7:
                        Moves = CTR.mini.unpackMini(g.getFile(0), "WD");
                        Invoke((Action)(() => new MoveEditor7(Moves).ShowDialog()));
                        g.Files = new[] {CTR.mini.packMini(Moves, "WD")};
                        break;
                }
                g.Save();
            }).Start();
        }
        private void B_LevelUp_Click(object sender, EventArgs e)
        {
            if (threadActive()) return;
            new Thread(() =>
            {
                var g = Config.getGARCData("levelup");
                byte[][] d = g.Files;
                switch (Config.Generation)
                {
                    case 6:
                        Invoke((Action)(() => new LevelUpEditor6(d).ShowDialog()));
                        break;
                    case 7:
                        Invoke((Action)(() => new LevelUpEditor7(d).ShowDialog()));
                        break;
                }
                g.Files = d;
                g.Save();
            }).Start();
        }
        private void B_EggMove_Click(object sender, EventArgs e)
        {
            if (threadActive()) return;
            new Thread(() =>
            {
                var g = Config.getGARCData("eggmove");
                byte[][] d = g.Files;
                switch (Config.Generation)
                {
                    case 6:
                        Invoke((Action)(() => new EggMoveEditor6(d).ShowDialog()));
                        break;
                    case 7:
                        Invoke((Action)(() => new EggMoveEditor7(d).ShowDialog()));
                        break;
                }
                g.Files = d;
                g.Save();
            }).Start();
        }
        private void B_TitleScreen_Click(object sender, EventArgs e)
        {
            if (threadActive()) return;
            new Thread(() =>
            {
                string[] files = { "titlescreen" };
                fileGet(files); // Compressed files exist, handled in the other form since there's so many
                Invoke((Action)(() => new TitleScreenEditor6().ShowDialog()));
                fileSet(files);
            }).Start();
        }
        // RomFS File Requesting Method Wrapper
        private void fileGet(string[] files, bool skipDecompression = true, bool skipGet = false)
        {
            if (ModifierKeys == (Keys.Control | Keys.Shift)) restoreGARCs(files.ToArray());
            if (skipGet || skipBoth) return;
            foreach (string toEdit in files)
            {
                string GARC = Config.getGARCFileName(toEdit);
                updateStatus($"GARC Get: {toEdit} @ {GARC}... ");
                threadGet(Path.Combine(RomFSPath, GARC), toEdit, true, skipDecompression);
                while (threads > 0) Thread.Sleep(50);
                resetStatus();
            }
        }
        private void fileSet(IEnumerable<string> files, bool keep = false)
        {
            if (skipBoth) return;
            foreach (string toEdit in files)
            {
                string GARC = Config.getGARCFileName(toEdit);
                updateStatus($"GARC Set: {toEdit} @ {GARC}... ");
                threadSet(Path.Combine(RomFSPath, GARC), toEdit, 4); // 4 bytes for Gen6
                while (threads > 0) Thread.Sleep(50);
                if (!keep && Directory.Exists(toEdit)) Directory.Delete(toEdit, true);
                resetStatus();
            }
        }

        // ExeFS Subform Items
        private void rebuildExeFS(object sender, EventArgs e)
        {
            if (ExeFSPath == null) return;
            if (Util.Prompt(MessageBoxButtons.YesNo, "Rebuild ExeFS?") != DialogResult.Yes) return;

            string[] files = Directory.GetFiles(ExeFSPath);
            int file = 0; if (files[1].Contains("code")) file = 1;

            SaveFileDialog sfd = new SaveFileDialog
            {
                FileName = HANSgameID != 0 ? HANSgameID.ToString("X8") + ".exefs" : "exefs.bin",
                Filter = "HANS ExeFS|*.exefs" + "|Binary File|*.bin" + "|All Files|*.*"
            };
            sfd.FilterIndex = HANSgameID != 0 ? 0 : sfd.Filter.Length - 1;

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
            if (threadActive()) return;
            if (ExeFSPath != null) new PickupEditor6().Show();
        }
        private void B_TMHM_Click(object sender, EventArgs e)
        {
            if (threadActive()) return;
            if (ExeFSPath != null)
                switch (Config.Generation)
                {
                    case 6:
                        new TMHMEditor6().Show();
                        break;
                    case 7:
                        new TMEditor7().Show();
                        break;
                }
        }
        private void B_Mart_Click(object sender, EventArgs e)
        {
            if (threadActive()) return;
            switch (Config.Generation)
            {
                case 6:
                    if (ExeFSPath != null) new MartEditor6().Show();
                    break;

                case 7:
                    if (RomFSPath != null) new MartEditor7().Show();
                    break;
            }
        }
        private void B_MoveTutor_Click(object sender, EventArgs e)
        {
            if (threadActive()) return;
            if (Config.XY) { Util.Alert("No Tutors for X/Y."); return; } // Already disabled button...
            if (ExeFSPath != null) new TutorEditor6().Show();
        }
        private void B_OPower_Click(object sender, EventArgs e)
        {
            if (threadActive()) return;
            if (ExeFSPath != null) new OPower().Show();
        }

        // CRO Subform Items
        private void patchCRO_CRR(object sender, EventArgs e)
        {
            if (threadActive()) return;
            if (RomFSPath == null) return;
            if (DialogResult.Yes != Util.Prompt(MessageBoxButtons.YesNo, "Rebuilding CRO/CRR is not necessary if you patch RO.", "Continue?"))
                return;
            new Thread(() =>
            {
                threads++;
                CTR.CRO.rehashCRR(Path.Combine(RomFSPath, ".crr", "static.crr"), RomFSPath, true, /* true // don't patch crr for now */ false, RTB_Status, pBar1);
                threads--;

                Util.Alert("CRO's and CRR have been updated.",
                        "If you have made any modifications, it is required that the RSA Verification check be patched on the system in order for the modified CROs to load (ie, no file redirection like NTR's layeredFS).");
            }).Start();

        }
        private void B_Starter_Click(object sender, EventArgs e)
        {
            if (threadActive()) return;
            if (DialogResult.Yes != Util.Prompt(MessageBoxButtons.YesNo,
                "CRO Editing causes crashes if you do not patch the RO module.", "Continue anyway?"))
                return;
            string CRO = Path.Combine(RomFSPath, "DllPoke3Select.cro");
            string CRO2 = Path.Combine(RomFSPath, "DllField.cro");
            if (!File.Exists(CRO))
            {
                Util.Error("File Missing!", "DllPoke3Select.cro was not found in your RomFS folder!");
                return;
            }
            if (!File.Exists(CRO2))
            {
                Util.Error("File Missing!", "DllField.cro was not found in your RomFS folder!");
                return;
            }
            new StarterEditor6().ShowDialog();
        }
        private void B_TypeChart_Click(object sender, EventArgs e)
        {
            if (threadActive()) return;

            switch (Config.Generation)
            {
                case 6:
                    if (DialogResult.Yes != Util.Prompt(MessageBoxButtons.YesNo,
                        "CRO Editing causes crashes if you do not patch the RO module.", "Continue anyway?"))
                        return;
                    string CRO = Path.Combine(RomFSPath, "DllBattle.cro");
                    if (!File.Exists(CRO))
                    {
                        Util.Error("File Missing!", "DllBattle.cro was not found in your RomFS folder!");
                        return;
                    }
                    new TypeChart6().ShowDialog();
                    break;
                case 7:
                    new TypeChart7().ShowDialog();
                    break;

            }
        }
        private void B_Gift_Click(object sender, EventArgs e)
        {
            if (threadActive()) return;
            if (DialogResult.Yes != Util.Prompt(MessageBoxButtons.YesNo,
                "CRO Editing causes crashes if you do not patch the RO module.", "Continue anyway?"))
                return;
            string CRO = Path.Combine(RomFSPath, "DllField.cro");
            if (!File.Exists(CRO))
            {
                Util.Error("File Missing!", "DllField.cro was not found in your RomFS folder!");
                return;
            }
            new GiftEditor6().ShowDialog();
        }
        private void B_Static_Click(object sender, EventArgs e)
        {
            if (threadActive()) return;

            if (Config.Generation == 7)
            {
                new Thread(() =>
                {
                    var esg = Config.getGARCData("encounterstatic");
                    byte[][] es = esg.Files;
                    
                    Invoke((Action)(() => new StaticEncounterEditor7(es).ShowDialog()));
                    esg.Files = es;
                    esg.Save();
                }).Start();
                return;
            }

            if (DialogResult.Yes != Util.Prompt(MessageBoxButtons.YesNo,
                "CRO Editing causes crashes if you do not patch the RO module.", "Continue anyway?"))
                return;
            string CRO = Path.Combine(RomFSPath, "DllField.cro");
            if (!File.Exists(CRO))
            {
                Util.Error("File Missing!", "DllField.cro was not found in your RomFS folder!");
                return;
            }
            new StaticEncounterEditor6().ShowDialog();
        }
        private void backupCROs(bool overwrite, string path)
        {
            if (!Directory.Exists(path))
                return;

            string[] files = Directory.GetFiles(path);
            string[] CROs = files.Where(x => new FileInfo(x).Name.Contains("Dll")).ToArray();
            string[] CRSs = files.Where(x => new FileInfo(x).Extension.Contains("crs")).ToArray();
            string[] CRRs = Directory.Exists(Path.Combine(path, ".crr"))
                ? Directory.GetFiles(Path.Combine(path, ".crr"))
                : new string[0];

            int count = CROs.Length + CRSs.Length + CRRs.Length;
            if (count <= 0)
                return;

            // Somewhat unique ID for the dlls to separate backup folders between versions
            string CROBAKPATH = Path.Combine("backup", "DLL_" + count);

            if (!Directory.Exists(CROBAKPATH))
                Directory.CreateDirectory(CROBAKPATH);

            foreach (string file in CROs.Concat(CRSs).Where(file => overwrite || !File.Exists(Path.Combine(CROBAKPATH, Path.GetFileName(file)))))
                File.Copy(file, Path.Combine(CROBAKPATH, Path.GetFileName(file)));

            if (CRRs.Length <= 0)
                return;

            // Separate folder for the .crr
            string CRRBAKPATH = Path.Combine(CROBAKPATH, ".crr");
            if (!Directory.Exists(CRRBAKPATH))
                Directory.CreateDirectory(CRRBAKPATH);

            foreach (string file in CRRs.Where(file => overwrite || !File.Exists(Path.Combine(CRRBAKPATH, Path.GetFileName(file)))))
                File.Copy(file, Path.Combine(CRRBAKPATH, Path.GetFileName(file)));
        }

        // 3DS Building
        private void B_Rebuild3DS_Click(object sender, EventArgs e)
        {
            // Ensure that the romfs paths are valid
            string[] files = Directory.GetFiles(TB_Path.Text, "*", SearchOption.AllDirectories);
            if (!Config.IsRebuildable(files.Length))
            {
                Util.Error("RomFS file count does not match the default game file count.");
                return;
            }
            if (threadActive()) return;

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
            if (DialogResult.OK != ofd.ShowDialog()) return;

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
            if (DialogResult.OK != ofd.ShowDialog()) return;

            string path = ofd.FileName;
            FileInfo fi = new FileInfo(path);
            if (fi.Length > 15*1024*1024) // 15MB
            { Util.Error("File too big!", fi.Length + " bytes."); return; }

            byte[] data = File.ReadAllBytes(path);
            string predict = data[0] == 0x11 ? "compressed" : "decompressed";
            var dr = Util.Prompt(MessageBoxButtons.YesNoCancel, $"Detected {predict} file. Do what?",
                "Yes = Decompress\nNo = Compress\nCancel = Abort");
            new Thread(() =>
            {
                threads++; 
                if (dr == DialogResult.Yes)
                {
                    try
                    {
                        CTR.LZSS.Decompress(path, Path.Combine(Directory.GetParent(path).FullName, "dec_" + Path.GetFileNameWithoutExtension(path) + ".bin"));
                    } catch (Exception err) { Util.Alert("Tried decompression, may have worked:", err.ToString()); }
                    Util.Alert("File Decompressed!", path);
                }
                if (dr == DialogResult.No)
                {
                    CTR.LZSS.Compress(path, Path.Combine(Directory.GetParent(path).FullName, Path.GetFileNameWithoutExtension(path).Replace("_dec", "") + ".lz"));
                    Util.Alert("File Compressed!", path);
                }
                threads--;
            }).Start();
        }
        private void Menu_SMDH_Click(object sender, EventArgs e)
        {
            new Icon().ShowDialog();
        }
        private void Menu_Shuffler_Click(object sender, EventArgs e)
        {
            new Shuffler().ShowDialog();
        }

        // GARC Requests
        internal static string getGARCFileName(string requestedGARC, int lang)
        {
            var garc = Config.getGARCReference(requestedGARC);
            if (garc.LanguageVariant)
                garc = garc.getRelativeGARC(lang);

            return garc.Reference;
        }

        private bool getGARC(string infile, string outfolder, bool PB, bool bypassExt = false)
        {
            if (skipBoth && Directory.Exists(outfolder))
            {
                updateStatus("Skipped - Exists!", false);
                threads--;
                return true;
            }
            try
            {
                bool success = CTR.GARC.garcUnpack(infile, outfolder, bypassExt, PB ? pBar1 : null, null, true, bypassExt);
                updateStatus(string.Format(success ? "Success!" : "Failed!"), false);
                threads--;
                return success;
            }
            catch (Exception e) { Util.Error("Could not get the GARC:", e.ToString()); threads--; return false; }
        }
        private bool setGARC(string outfile, string infolder, int padBytes, bool PB)
        {
            if (skipBoth || (ModifierKeys == Keys.Control && Util.Prompt(MessageBoxButtons.YesNo, "Cancel writing data back to GARC?") == DialogResult.Yes))
            { threads--; updateStatus("Aborted!", false); return false; }

            try
            {
                bool success = CTR.GARC.garcPackMS(infolder, outfile, Config.GARCVersion, padBytes, PB ? pBar1 : null, null, true);
                threads--;
                updateStatus(string.Format(success ? "Success!" : "Failed!"), false);
                return success;
            }
            catch (Exception e) { Util.Error("Could not set the GARC back:", e.ToString()); threads--; return false; }
        }
        private void threadGet(string infile, string outfolder, bool PB = true, bool bypassExt = false)
        {
            threads++;
            if (Directory.Exists(outfolder)) try { Directory.Delete(outfolder, true); }
                catch { }
            new Thread(() => getGARC(infile, outfolder, PB, bypassExt)).Start();
        }
        private void threadSet(string outfile, string infolder, int padBytes, bool PB = true)
        {
            threads++;
            new Thread(() => setGARC(outfile, infolder, padBytes, PB)).Start();
        }

        private static void backupGARCs(bool overwrite, params string[] g)
        {
            if (!Directory.Exists("backup")) Directory.CreateDirectory("backup");
            foreach (string s in g)
            {
                string GARC = Config.getGARCFileName(s);
                string dest = "backup" + Path.DirectorySeparatorChar + s +
                              $" ({GARC.Replace(Path.DirectorySeparatorChar.ToString(), "")})";
                if (overwrite || !File.Exists(dest))
                    File.Copy(Path.Combine(RomFSPath, GARC), dest);
            }
        }
        private static void restoreGARCs(params string[] g)
        {
            foreach (string s in g)
            {
                string dest = Path.Combine(RomFSPath, Config.getGARCFileName(s));
                string src = "backup" + Path.DirectorySeparatorChar + s +
                             $" ({Config.getGARCFileName(s).Replace(Path.DirectorySeparatorChar.ToString(), "")})";
                File.Copy(src, dest, true);
                if (s == "personal" || s == "gametext")
                    Util.Alert("In order to restore " + s + ", restart the program. While exiting, hold the Control Key to prevent writebacks.");
            }
            Util.Alert(g.Length + " files restored.");
        }

        // Text Requests
        internal static string[] getText(TextName file)
        {
            return (string[])Config.GameTextStrings[Config.getGameText(file).Index].Clone();
        }
        internal static bool setText(TextName file, string[] strings)
        {
            Config.GameTextStrings[Config.getGameText(file).Index] = strings;
            return true;
        }

        // Update RichTextBox
        private void updateStatus(string status, bool preBreak = true)
        {
            string newtext = (preBreak ? Environment.NewLine : "") + status;
            try
            {
                if (RTB_Status.InvokeRequired)
                    RTB_Status.Invoke((MethodInvoker)delegate
                    {
                        RTB_Status.AppendText(newtext);
                        RTB_Status.SelectionStart = RTB_Status.Text.Length;
                        RTB_Status.ScrollToCaret();
                        L_Status.Text = RTB_Status.Lines.Last().Split(new[] {" @"}, StringSplitOptions.None)[0];
                    });
                else
                {
                    RTB_Status.AppendText(newtext);
                    RTB_Status.SelectionStart = RTB_Status.Text.Length;
                    RTB_Status.ScrollToCaret();
                    L_Status.Text = RTB_Status.Lines.Last().Split(new[] { " @" }, StringSplitOptions.None)[0];
                }
            }
            catch { }
        }
        private void resetStatus()
        {
            try
            {
                if (L_Status.InvokeRequired)
                    L_Status.Invoke((MethodInvoker)delegate
                    {
                        L_Status.Text = "";
                    });
                else
                {
                    L_Status.Text = "";
                }
            }
            catch { }
        }
    }
}