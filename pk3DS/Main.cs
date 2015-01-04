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
            this.AllowDrop = GB_Tools.AllowDrop = TB_Path.AllowDrop = true;
            this.DragEnter += tabMain_DragEnter;
            this.DragDrop += tabMain_DragDrop;
            GB_Tools.DragEnter += tabMain_DragEnter;
            GB_Tools.DragDrop += tabMain_DragDrop;
            TB_Path.DragEnter += tabMain_DragEnter;
            TB_Path.DragDrop += tabMain_DragDrop;
            CB_Lang.SelectedIndex = 2;
        }
        public bool oras = false;
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
                L_Game.Text = (oras) ? "Game Loaded: ORAS" : "Game Loaded: XY";
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
        } volatile bool StoryTextOpen = false;
        private void B_Wild_Click(object sender, EventArgs e)
        {
            Util.Alert("Not implemented yet.");
        }
        private void B_Personal_Click(object sender, EventArgs e)
        {
            Util.Alert("Not implemented yet.");
        }
        private void B_Evolution_Click(object sender, EventArgs e)
        {
            Util.Alert("Not implemented yet.");
        }
        private void B_MegaEvo_Click(object sender, EventArgs e)
        {
            Util.Alert("Not implemented yet.");
        }
        private void B_Move_Click(object sender, EventArgs e)
        {
            Util.Alert("Not implemented yet.");
        }
        private void B_Item_Click(object sender, EventArgs e)
        {
            Util.Alert("Not implemented yet.");
        }
        private void B_Trainer_Click(object sender, EventArgs e)
        {
            if (threads > 0) { Util.Alert("Please wait for all operations to finish first."); return; }
            if (!oras) { Util.Alert("X/Y not supported yet."); return; }
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

                Invoke((Action)(() => { new RSTE(oras, Directory.GetFiles(trdata), Directory.GetFiles(trpoke)).ShowDialog(); }));
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

        // GARC Requests
        public bool getGARC(string infile, string outfolder, bool PB, bool bypassExt = false)
        {
            bool success = GARCTool.garcUnpack(infile, outfolder, bypassExt, (PB) ? pBar1 : null, null, true, bypassExt);
            Console.WriteLine("Get Status: " + success.ToString());
            threads--;
            return success;
        }
        public bool setGARC(string outfile, string infolder, bool PB)
        {
            string temp = Util.getRandomFileName();
            bool success = GARCTool.garcPack(infolder, temp, (PB) ? pBar1 : null, null, true);
            Console.WriteLine("Set Status: " + success.ToString());
            try { File.Delete(outfile); File.Move(temp, outfile); }
            catch (Exception e) { Util.Error("Could not set the GARC back:", e.ToString()); }
            threads--;
            return success;
        }
        public void threadGet(string infile, string outfolder, bool PB = true, bool bypassExt = false)
        {
            if (Directory.Exists(outfolder)) Directory.Delete(outfolder, true);
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
                case "gametext": ans = (oras) ? getFileName(0, 7, 1 + lang) : getFileName(0, 7, 2 + lang);
                    break;
                case "storytext": ans = (oras) ? getFileName(0, 7 + ((lang + 9) / 10), (10 + (lang + 9)) % 10) : getFileName(0, 8, lang);
                    break;
                case "personal": ans = (oras) ? getFileName(1, 9, 5) : getFileName(2, 1, 8);
                    break;
                case "trdata": ans = (oras) ? getFileName(0, 3, 6) : getFileName(0, 3, 8);
                    break;
                case "trpoke": ans = (oras) ? getFileName(0, 3, 8) : getFileName(0, 4, 0);
                    break;
            }
            return ans;
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
            if (!GB_Tools.Enabled) return; // No data/threads need to be addressed if we haven't loaded anything.
            // Set the GameText back as other forms may have edited it.
            threadSet(TB_Path.Text + getGARCFileName("gametext"), "gametext", false);
            threadSet(TB_Path.Text + getGARCFileName("personal"), "personal", false);
            int timeout = 0; // Time out after 7 seconds.
            while (threads > 0 && timeout++ < 70)
                Thread.Sleep(100);

            if (Directory.Exists("gametext")) Directory.Delete("gametext", true);
            if (Directory.Exists("personal")) Directory.Delete("personal", true);
        }
        private void L_About_Click(object sender, EventArgs e)
        {
            Util.Alert(
                "pk3DS: A package of Pokémon X/Y/OR/AS tools by various contributors.",
                "GARCTool (Backbone): Kaphotics" + Environment.NewLine +
                "Text Editing (xytext): Kaphotics" + Environment.NewLine +
                "Wild Editor (**WE): SciresM & Kaphotics" + Environment.NewLine +
                "Trainer Editor (**TE): SciresM & Kaphotics" + Environment.NewLine +
                "Personal Editor: SciresM" + Environment.NewLine +
                "Mega Evolution Editor (MEE): Huntereb & SciresM" + Environment.NewLine +
                "Evolutions, Moves, and Items and more have yet to be implemented.",
                "Big thanks to the ProjectPokemon community!");
        }
    }
}