using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Threading;

namespace pk3DS
{
    public partial class Main : Form
    {
        public Main()
        {
            InitializeComponent();
            CB_Lang.SelectedIndex = 2;
            // restoreGARCs(true, "gametext", "storytext", "personal", "trpoke", "trdata", "evolution", "megaevo", "levelup", "eggmove", "item", "move", "maisonpkS", "maisontrS", "maisonpkN", "maisontrN");
            this.AllowDrop = GB_Tools.AllowDrop = TB_Path.AllowDrop = true;
            this.DragEnter += tabMain_DragEnter;
            this.DragDrop += tabMain_DragDrop;
            GB_Tools.DragEnter += tabMain_DragEnter;
            GB_Tools.DragDrop += tabMain_DragDrop;
            TB_Path.DragEnter += tabMain_DragEnter;
            TB_Path.DragDrop += tabMain_DragDrop;
            if (File.Exists("config.ini"))
            {
                string path = File.ReadAllText("config.ini");
                if (path.Length > 0) openQuick(path);
                
            }
        }
        public bool oras = false;
        public string ExeFS = null;
        public volatile int threads = 0;
        private void B_Open_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog fbd = new FolderBrowserDialog();
            if (fbd.ShowDialog() == DialogResult.OK)
                openQuick(fbd.SelectedPath);
        }
        private void openQuick(string path)
        {
            // Check to see if the folder is romfs
            FileInfo fi = new FileInfo(path);
            if (fi.Name.Contains("a"))
            {
                string[] files = Directory.GetFiles(path, "*", SearchOption.AllDirectories);
                System.Media.SystemSounds.Asterisk.Play();
                if (files.Length == 299) // ORAS
                    oras = true;
                else if (files.Length == 301) // ORAS demo
                    oras = true;
                else if (files.Length == 271)
                    oras = false;
                else
                {
                    TB_Path.Text = ""; GB_Tools.Enabled = false;
                    L_Game.Text = "No Game Loaded";
                    return;
                }

                TB_Path.Text = path; GB_Tools.Enabled = true;
                backupGARCs(false, "gametext", "storytext", "personal", "trpoke", "trdata", "evolution", "megaevo", "levelup", "eggmove", "item", "move", "maisonpkS", "maisontrS", "maisonpkN", "maisontrN");

                L_Game.Text = (oras) ? "Game Loaded: ORAS" : "Game Loaded: XY";
                string exefs = Util.getExeFSFolder(path);
                changeLanguage(null, null); // Trigger Text Loading
            }
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

        // Subform Items
        private void B_GameText_Click(object sender, EventArgs e)
        {
            new Thread(() =>
            {
                string toEdit = "gametext";
                string GARC = getGARCFileName(toEdit);
                // threadGet(TB_Path.Text + getGARCFileName(toEdit), toEdit); // We don't need this because loading and changing language does it for us.

                Invoke((Action)(() => { new xytext(Directory.GetFiles(toEdit)).ShowDialog(); }));
                // When closed, create a new thread to set the GARC back.
                threadSet(TB_Path.Text + GARC, toEdit);
            }).Start();
        }
        private void B_StoryText_Click(object sender, EventArgs e)
        {
            if (threads > 0) { Util.Alert("Please wait for all operations to finish first."); return; }
            new Thread(() =>
            {
                string toEdit = "storytext";
                string GARC = getGARCFileName(toEdit);
                threadGet(TB_Path.Text + GARC, toEdit, true, true);
                while (threads > 0) // Let threads complete
                    Thread.Sleep(100);

                Invoke((Action)(() => { new xytext(Directory.GetFiles(toEdit)).ShowDialog(); }));
                // When closed, create a new thread to set the GARC back.
                threadSet(TB_Path.Text + GARC, toEdit);
                while (threads > 0) // Let threads complete
                    Thread.Sleep(100);
                if (Directory.Exists(toEdit)) Directory.Delete(toEdit, true);
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
                    string mtr = (super) ? "maisontrS" : "maisontrN";
                    string mtrGARC = getGARCFileName(mtr);
                    threadGet(TB_Path.Text + mtrGARC, mtr, true, true);
                    while (threads > 0) // Let threads complete
                        Thread.Sleep(50);

                    string mpk = (super) ? "maisonpkS" : "maisonpkN";
                    string mpkGARC = getGARCFileName(mpk);
                    threadGet(TB_Path.Text + mpkGARC, mpk, true, true);
                    while (threads > 0) // Let threads complete
                        Thread.Sleep(50);

                    Invoke((Action)(() => { new Maison(oras, super).ShowDialog(); }));

                    threadSet(TB_Path.Text + mtrGARC, mtr);
                    while (threads > 0) // Let threads complete
                        Thread.Sleep(100);
                    if (Directory.Exists(mtr)) Directory.Delete(mtr, true);

                    threadSet(TB_Path.Text + mpkGARC, mpk);
                    while (threads > 0) // Let threads complete
                        Thread.Sleep(100);
                    if (Directory.Exists(mpk)) Directory.Delete(mpk, true);

                }).Start();
        }
        private void B_Personal_Click(object sender, EventArgs e)
        {
            if (threads > 0) { Util.Alert("Please wait for all operations to finish first."); return; }
            new Thread(() =>
            {
                string toEdit = "personal";
                string GARC = getGARCFileName(toEdit);
                threadGet(TB_Path.Text + GARC, toEdit, true, true);
                while (threads > 0) // Let threads complete
                    Thread.Sleep(100);

                Invoke((Action)(() => { new Personal(oras).ShowDialog(); }));
                // When closed, create a new thread to set the GARC back.
                threadSet(TB_Path.Text + GARC, toEdit);
                while (threads > 0) // Let threads complete
                    Thread.Sleep(100);
                if (Directory.Exists(toEdit)) Directory.Delete(toEdit, true);
            }).Start();
        }
        private void B_Trainer_Click(object sender, EventArgs e)
        {
            if (threads > 0) { Util.Alert("Please wait for all operations to finish first."); return; }
            new Thread(() =>
            {
                string trdata = "trdata";
                string trdataGARC = getGARCFileName(trdata);
                threadGet(TB_Path.Text + trdataGARC, trdata, true, true);
                while (threads > 0) // Let threads complete
                    Thread.Sleep(50);

                string trpoke = "trpoke";
                string trpokeGARC = getGARCFileName(trpoke);
                threadGet(TB_Path.Text + trpokeGARC, trpoke, true, true);
                while (threads > 0) // Let threads complete
                    Thread.Sleep(50);

                string personal = "personal";
                string personalGARC = getGARCFileName(personal);
                threadGet(TB_Path.Text + personalGARC, personal, true, true);
                while (threads > 0) // Let threads complete
                    Thread.Sleep(100);

                Invoke((Action)(() => { new RSTE(oras).ShowDialog(); }));
                // When closed, create a new thread to set the GARC back.

                threadSet(TB_Path.Text + trdataGARC, trdata);
                while (threads > 0) // Let threads complete
                    Thread.Sleep(100);
                if (Directory.Exists(trdata)) Directory.Delete(trdata, true);

                threadSet(TB_Path.Text + trpokeGARC, trpoke);
                while (threads > 0) // Let threads complete
                    Thread.Sleep(100);
                if (Directory.Exists(trpoke)) Directory.Delete(trpoke, true);
            }).Start();
        }
        private void B_Wild_Click(object sender, EventArgs e)
        {
            if (threads > 0) { Util.Alert("Please wait for all operations to finish first."); return; }
            new Thread(() =>
            {
                string encdata = "encdata";
                string encdataGARC = getGARCFileName(encdata);
                threadGet(TB_Path.Text + encdataGARC, encdata, true, false);
                while (threads > 0) // Let threads complete
                    Thread.Sleep(50);

                if (oras)
                { Invoke((Action)(() => { new RSWE(oras).ShowDialog(); })); }
                else
                { Invoke((Action)(() => { new XYWE(oras).ShowDialog(); })); }
                // When closed, create a new thread to set the GARC back.

                threadSet(TB_Path.Text + encdataGARC, encdata);
                while (threads > 0) // Let threads complete
                    Thread.Sleep(100);
                if (Directory.Exists(encdata)) Directory.Delete(encdata, true);
            }).Start();
        }
        private void B_Evolution_Click(object sender, EventArgs e)
        {
            if (threads > 0) { Util.Alert("Please wait for all operations to finish first."); return; }
            new Thread(() =>
            {
                string toEdit = "evolution";
                string GARC = getGARCFileName(toEdit);
                threadGet(TB_Path.Text + GARC, toEdit, true, true);
                while (threads > 0) // Let threads complete
                    Thread.Sleep(100);

                Invoke((Action)(() => { new Evolution(oras).ShowDialog(); }));
                // When closed, create a new thread to set the GARC back.
                threadSet(TB_Path.Text + GARC, toEdit);
                while (threads > 0) // Let threads complete
                    Thread.Sleep(100);
                if (Directory.Exists(toEdit)) Directory.Delete(toEdit, true);
            }).Start();
        }
        private void B_MegaEvo_Click(object sender, EventArgs e)
        {
            if (threads > 0) { Util.Alert("Please wait for all operations to finish first."); return; }
            new Thread(() =>
            {
                string megaevo = "megaevo";
                string GARC = getGARCFileName(megaevo);
                threadGet(TB_Path.Text + GARC, megaevo, true, true);
                while (threads > 0) // Let threads complete
                    Thread.Sleep(50);

                string personal = "personal";
                string personalGARC = getGARCFileName(personal);
                threadGet(TB_Path.Text + personalGARC, personal, true, true);
                while (threads > 0) // Let threads complete
                    Thread.Sleep(50);

                Invoke((Action)(() => { new MEE(oras).ShowDialog(); }));
                // When closed, create a new thread to set the GARC back.

                threadSet(TB_Path.Text + GARC, megaevo);
                while (threads > 0) // Let threads complete
                    Thread.Sleep(50);
                if (Directory.Exists(megaevo)) Directory.Delete(megaevo, true);
            }).Start();
        }
        private void B_Item_Click(object sender, EventArgs e)
        {
            Util.Alert("Not implemented yet.");
        }
        private void B_Move_Click(object sender, EventArgs e)
        {
            if (threads > 0) { Util.Alert("Please wait for all operations to finish first."); return; }
            new Thread(() =>
            {
                string toEdit = "move";
                string GARC = getGARCFileName(toEdit);
                threadGet(TB_Path.Text + GARC, toEdit, true, true);
                while (threads > 0) // Let threads complete
                    Thread.Sleep(100);

                Invoke((Action)(() => { new Moves(oras).ShowDialog(); }));

                // When closed, create a new thread to set the GARC back.
                threadSet(TB_Path.Text + GARC, toEdit);
                while (threads > 0) // Let threads complete
                    Thread.Sleep(100);
                if (Directory.Exists(toEdit)) Directory.Delete(toEdit, true);
            }).Start();
        }
        private void B_LevelUp_Click(object sender, EventArgs e)
        {
            if (threads > 0) { Util.Alert("Please wait for all operations to finish first."); return; }
            new Thread(() =>
            {
                string toEdit = "levelup";
                string GARC = getGARCFileName(toEdit);
                threadGet(TB_Path.Text + GARC, toEdit, true, true);
                while (threads > 0) // Let threads complete
                    Thread.Sleep(100);

                Invoke((Action)(() => { new LevelUp(oras).ShowDialog(); }));
                // When closed, create a new thread to set the GARC back.
                threadSet(TB_Path.Text + GARC, toEdit);
                while (threads > 0) // Let threads complete
                    Thread.Sleep(100);
                if (Directory.Exists(toEdit)) Directory.Delete(toEdit, true);
            }).Start();
        }
        private void B_EggMove_Click(object sender, EventArgs e)
        {
            if (threads > 0) { Util.Alert("Please wait for all operations to finish first."); return; }
            new Thread(() =>
            {
                string toEdit = "eggmove";
                string GARC = getGARCFileName(toEdit);
                threadGet(TB_Path.Text + GARC, toEdit, true, true);
                while (threads > 0) // Let threads complete
                    Thread.Sleep(100);

                Invoke((Action)(() => { new EggMove(oras).ShowDialog(); }));
                // When closed, create a new thread to set the GARC back.
                threadSet(TB_Path.Text + GARC, toEdit);
                while (threads > 0) // Let threads complete
                    Thread.Sleep(100);
                if (Directory.Exists(toEdit)) Directory.Delete(toEdit, true);
            }).Start();
        }

        // GARC Requests
        public bool getGARC(string infile, string outfolder, bool PB, bool bypassExt = false)
        {
            try
            {
                bool success = GARCTool.garcUnpack(infile, outfolder, bypassExt, (PB) ? pBar1 : null, null, true, bypassExt);
                Console.WriteLine("Get Status: " + success.ToString());
                threads--;
                return success;
            }
            catch (Exception e) { Util.Error("Could not get the GARC:", e.ToString()); threads--; return false; }
        }
        public bool setGARC(string outfile, string infolder, bool PB)
        {
            try
            {
                bool success = GARCTool.garcPackMS(infolder, outfile, (PB) ? pBar1 : null, null, true);
                Console.WriteLine("Set Status: " + success.ToString());
                threads--;
                return success;
            }
            catch (Exception e) { Util.Error("Could not set the GARC back:", e.ToString()); threads--; return false; }
        }
        public void threadGet(string infile, string outfolder, bool PB = true, bool bypassExt = false)
        {
            if (Directory.Exists(outfolder)) try { Directory.Delete(outfolder, true); }
                catch { }
            Thread thread = new Thread(() => getGARC(infile, outfolder, PB, bypassExt));
            thread.Start(); threads++;
        }
        public void threadSet(string outfile, string infolder, bool PB = true)
        {
            Thread thread = new Thread(() => setGARC(outfile, infolder, PB));
            thread.Start(); threads++;
        }
        public string getGARCFileName(string request)
        {
            int lang = 0;
            if (CB_Lang.InvokeRequired)
                CB_Lang.Invoke((MethodInvoker)delegate { lang = CB_Lang.SelectedIndex; });
            else lang = CB_Lang.SelectedIndex;

            string ans = "";
            switch (request)
            {
                case "encdata": ans = (oras) ? getFileName(0, 1, 3) : getFileName(0, 1, 2); break;
                case "gametext": ans = (oras) ? getFileName(0, 7, 1 + lang) : getFileName(0, 7, 2 + lang); break;
                case "storytext": ans = (oras) ? getFileName(0, 7 + ((lang + 9) / 10), (10 + (lang + 9)) % 10) : getFileName(0, 8, lang); break;
                case "trdata": ans = (oras) ? getFileName(0, 3, 6) : getFileName(0, 3, 8); break;
                case "trpoke": ans = (oras) ? getFileName(0, 3, 8) : getFileName(0, 4, 0); break;
                case "move": ans = (oras) ? getFileName(1, 8, 9) : getFileName(2, 1, 2); break;
                case "eggmove": ans = (oras) ? getFileName(1, 9, 0) : getFileName(2, 1, 3); break;
                case "levelup": ans = (oras) ? getFileName(1, 9, 1) : getFileName(2, 1, 4); break;
                case "evolution": ans = (oras) ? getFileName(1, 9, 2) : getFileName(2, 1, 5); break;
                case "megaevo": ans = (oras) ? getFileName(1, 9, 3) : getFileName(2, 1, 6); break;
                case "personal": ans = (oras) ? getFileName(1, 9, 5) : getFileName(2, 1, 8); break;
                case "item": ans = (oras) ? getFileName(1, 9, 7) : getFileName(2, 2, 0); break;
                case "maisonpkN": ans = (oras) ? getFileName(1, 8, 2) : getFileName(2, 0, 3); break;
                case "maisontrN": ans = (oras) ? getFileName(1, 8, 3) : getFileName(2, 0, 4); break;
                case "maisonpkS": ans = (oras) ? getFileName(1, 8, 4) : getFileName(2, 0, 5); break;
                case "maisontrS": ans = (oras) ? getFileName(1, 8, 5) : getFileName(2, 0, 6); break;
            }
            return ans;
        }
        public void backupGARCs(bool overwrite, params string[] g)
        {
            if (!Directory.Exists("backup")) Directory.CreateDirectory("backup");
            foreach (string s in g)
            {
                string GARC = getGARCFileName(s);
                string dest = "backup" + Path.DirectorySeparatorChar + s + String.Format(" (a{0})", GARC.Replace(Path.DirectorySeparatorChar.ToString(), ""));
                if (overwrite || !File.Exists(dest))
                    File.Copy(TB_Path.Text + GARC, dest);
            }
        }
        public void restoreGARCs(bool ggg, params string[] g)
        {
            oras = ggg;
            GB_Tools.Enabled = false;
                foreach (string s in g)
                {
                    string dest = File.ReadAllText("config.ini") + getGARCFileName(s);
                    string src = "backup" + Path.DirectorySeparatorChar + s + String.Format(" (a{0})", getGARCFileName(s).Replace(Path.DirectorySeparatorChar.ToString(), ""));
                    File.Copy(src, dest, true);
                }
        }

        // Text Requests
        internal static string[] getText(int file)
        {
            return xytext.getStringsFromFile("gametext" + Path.DirectorySeparatorChar + file.ToString("000") + ".bin");
        }
        public bool setText(int file, string[] strings)
        {
            byte[] data = xytext.getBytesForFile(strings);
            string path = "text" + Path.DirectorySeparatorChar + file.ToString("000") + ".bin";
            File.WriteAllBytes(path, data);
            return true;
        }

        private void changeLanguage(object sender, EventArgs e)
        {
            if (!GB_Tools.Enabled) return;
            // Gather the Text Language Strings
            threadGet(TB_Path.Text + getGARCFileName("gametext"), "gametext", true, true);
        }
        private string getFileName(int A, int B, int C)
        {
            return Path.DirectorySeparatorChar + A.ToString() + Path.DirectorySeparatorChar + B.ToString() + Path.DirectorySeparatorChar + C.ToString();
        }

        private void formClosing(object sender, FormClosingEventArgs e)
        {
            this.Hide();
            if (TB_Path.Text.Length > 0) File.WriteAllText("config.ini", TB_Path.Text);
            if (!GB_Tools.Enabled) return; // No data/threads need to be addressed if we haven't loaded anything.
            // Set the GameText back as other forms may have edited it.
            try
            {
                threadSet(TB_Path.Text + getGARCFileName("gametext"), "gametext", false);
                if (Directory.Exists("personal")) threadSet(TB_Path.Text + getGARCFileName("personal"), "personal", false);
                int timeout = 0; // Time out after 7 seconds.
                while (threads > 0 && timeout++ < 70)
                    Thread.Sleep(100);
            }
            catch { }
            if (Directory.Exists("gametext")) Directory.Delete("gametext", true);
            if (Directory.Exists("personal")) Directory.Delete("personal", true);
            if (Directory.Exists("temp")) Directory.Delete("temp", true);
        }
        private void L_About_Click(object sender, EventArgs e)
        {
            Util.Alert(
                "pk3DS: A package of Pokémon X/Y/OR/AS tools by various contributors.",
                "GARCTool (Backbone): Kaphotics" + Environment.NewLine +
                "Text Editing (xytext): Kaphotics" + Environment.NewLine +
                "Wild Editor (**WE): SciresM & Kaphotics" + Environment.NewLine +
                "Trainer Editor (**TE): SciresM, Kaphotics, and KazoWAR" + Environment.NewLine +
                "Personal Editor: SciresM" + Environment.NewLine +
                "Mega Evolution Editor (MEE): Huntereb & SciresM" + Environment.NewLine +
                "Evolutions, Moves, and Maison Editor: Kaphotics" + Environment.NewLine +
                "Item Editor and more have yet to be implemented.",
                "Big thanks to the ProjectPokemon community!");
        }
    }
}