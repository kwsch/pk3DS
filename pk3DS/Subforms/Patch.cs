using System;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace pk3DS
{
    public partial class Patch : Form
    {
        public Patch()
        {
            InitializeComponent();
            RTB_GARCs.Clear();
            CHKLB_GARCs.Items.Clear();
            foreach (string s in Main.allGARCs)
                CHKLB_GARCs.Items.Add(s);

            if (File.Exists("patch.ini"))
                RTB_GARCs.Lines = File.ReadAllLines("patch.txt", Encoding.Unicode);
        }

        internal static bool patchExeFS(string path, string[] oldstr, string[] newstr, string oldROM, string newROM, ref string result, string outPath = null)
        {
            int ctr = 0;
            if (oldstr.Length != newstr.Length)
            {
                result = "Input replacements do not match output replacements.";
                return false;
            }

            string text = File.ReadAllText(path, Encoding.Unicode);
            if (!text.Contains(newROM))
            {
                result = "ExeFS\\.code.bin is not a patchable ExeFS (no rom2: found).";
                return false;
            }
            for (int i = 0; i < oldstr.Length; i++)
            {
                string oldString = (oldROM + oldstr[i]).Replace(Path.DirectorySeparatorChar, '/');
                string patchedStr = (newROM + oldstr[i]).Replace(Path.DirectorySeparatorChar, '/');
                string newString = (newROM + newstr[i]).Replace(Path.DirectorySeparatorChar, '/');

                bool old = text.Contains(oldString);
                bool patched = text.Contains(patchedStr);
                if (!old && !patched)
                    result += "Does not contain " + oldstr + Environment.NewLine;
                else
                    ctr++;

                if (old)
                    text = text.Replace(oldString, newString);
                if (patched)
                    text = text.Replace(patchedStr, newString + "\0");
            }

            if (ctr == 0)
            { result = "Did not find the old path strings to replace."; return false; }
            result += $"Redirected {ctr} file paths.";
            Directory.CreateDirectory(Directory.GetParent(outPath).Name);
            File.WriteAllText(outPath ?? path, text, Encoding.Unicode);
            return true;
        }
        internal static string exportGARCs(string[] garcPaths, string[] newPaths, string parentRomFS, string patchFolder)
        {
            // Stuff files into new patch folder
            for (int i = 0; i < garcPaths.Length; i++)
            {
                if ((garcPaths[i] ?? "").Length == 0) continue;
                string oldPath = parentRomFS + garcPaths[i];
                string newPath = patchFolder + newPaths[i];
                string folder = Path.GetDirectoryName(newPath);
                Directory.CreateDirectory(folder);
                File.Copy(oldPath, newPath);
            }
            return patchFolder;
        }

        private void B_PatchCIA_Click(object sender, EventArgs e)
        {
            string patchFolder = $"{"Patch"} ({DateTime.Now.ToString("yy-MM-dd@HH-mm-ss")})";
            try
            {
                string[] garcs = getGARCs();
                string[] garcPaths = getPaths(garcs);

                const string oldROM = "rom:";
                const string newROM = "rom2:";
                const string oldA = "\\a\\";
                const string newA = "\\a";

                string[] newPaths = (string[]) garcPaths.Clone();

                // Patch the reference
                for (int i = 0; i < newPaths.Length; i++)
                {
                    int posA = newPaths[i].LastIndexOf(oldA, StringComparison.Ordinal);
                    newPaths[i] = posA == -1 ? null : newPaths[i].Remove(posA, oldA.Length).Insert(posA, newA);
                }
                string result = "";
                string ExeFS = Directory.GetFiles(Main.ExeFSPath)[0];
                if (!File.Exists(ExeFS) || !Path.GetFileNameWithoutExtension(ExeFS).Contains("code")) { throw new Exception("No .code.bin detected."); }
                if (!patchExeFS(ExeFS, garcPaths, newPaths, oldROM, newROM, ref result, Path.Combine(patchFolder, ".code.bin")))
                    throw new Exception(result);

                Util.Alert("Patch contents saved to:" + Environment.NewLine + exportGARCs(garcPaths, newPaths, Main.RomFSPath, patchFolder), result);
            }
            catch (Exception ex)
            { 
                Util.Error("Could not create patch:", ex.ToString());
                Directory.Delete(patchFolder, true);
            }
        }

        private string[] getGARCs()
        {
            StringCollection sc = new StringCollection();
            foreach (int indexChecked in CHKLB_GARCs.CheckedIndices)
                sc.Add(CHKLB_GARCs.Items[indexChecked].ToString());

            string[] rtbLines = RTB_GARCs.Lines;
            foreach (string s in rtbLines.Where(s => s.Length == 7 && !sc.Contains(s.Replace('/', Path.DirectorySeparatorChar))))
                sc.Add(s.Replace('/', Path.DirectorySeparatorChar));

            string[] garcs = new string[sc.Count];
            sc.CopyTo(garcs, 0);
            return garcs.Distinct().ToArray();
        }
        private string[] getPaths(string[] sc)
        {
            bool languages = CHK_Lang.Checked;
            StringCollection paths = new StringCollection();
            foreach (string s in sc)
                if (!languages || (s != "gametext" && s != "storytext"))
                    paths.Add(Main.getGARCFileName(s, Main.Language));
                else
                    for (int l = 0; l < 8; l++)
                        paths.Add(Main.getGARCFileName(s, l));

            string[] garcs = new string[paths.Count];
            paths.CopyTo(garcs, 0);
            return garcs;
        }

        private void B_CheckAll_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < CHKLB_GARCs.Items.Count; i++)
                CHKLB_GARCs.SetItemChecked(i, true);
        }
        private void B_CheckNone_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < CHKLB_GARCs.Items.Count; i++)
                CHKLB_GARCs.SetItemChecked(i, false);
        }

        private void savePatch(object sender, FormClosingEventArgs e)
        {
            if (RTB_GARCs.Text.Length > 0)
                try { File.WriteAllLines("patch.ini", RTB_GARCs.Lines, Encoding.Unicode); } catch {}
        }
    }
}
