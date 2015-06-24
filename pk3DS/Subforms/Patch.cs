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
        }

        internal static bool patchExeFS(string path, string[] oldstr, string[] newstr, ref string result, string outPath = null)
        {
            int ctr = 0;
            if (oldstr.Length != newstr.Length)
            {
                result = "Input replacements do not match output replacements.";
                return false;
            }

            string text = File.ReadAllText(path, Encoding.Unicode);
            for (int i = 0; i < oldstr.Length; i++)
            {
                if (!text.Contains(oldstr[i])) 
                    result += "Does not contain " + oldstr + Environment.NewLine;
                else
                    ctr++;

                text = text.Replace(oldstr[i], newstr[i]);
            }

            if (ctr == 0)
            { result = "Did not find the old path strings to replace."; return false; }
            result += String.Format("Redirected {0} file paths.", ctr);
            File.WriteAllText(outPath ?? path, text, Encoding.Unicode);
            return true;
        }
        internal static string exportGARCs(string[] garcPaths, string[] newPaths, string parentRomFS, string patchFolder)
        {
            if (Directory.Exists(patchFolder))
                Directory.Delete(patchFolder, true);
            Directory.CreateDirectory(patchFolder);

            // Stuff files into new patch folder
            for (int i = 0; i < garcPaths.Length; i++)
            {
                if ((garcPaths[i] ?? "").Length == 0) continue;
                string folder = Path.GetDirectoryName(newPaths[i]);
                Directory.CreateDirectory(folder);
                File.Copy(Path.Combine(parentRomFS, garcPaths[i]), folder);
            }
            return patchFolder;
        }

        private void B_PatchCIA_Click(object sender, EventArgs e)
        {
            try
            {
                string[] garcs = getGARCs();
                string[] garcPaths = getPaths(garcs);

                const string oldROM = "rom:/a/";
                const string newROM = "rom2:/a";

                string[] newPaths = (string[]) garcPaths.Clone();

                // Patch the reference
                for (int i = 0; i < newPaths.Length; i++)
                {
                    int posA = newPaths[i].LastIndexOf(oldROM, StringComparison.Ordinal);
                    newPaths[i] = (posA == -1) ? null : newPaths[i].Remove(posA, oldROM.Length).Insert(posA, newROM);
                }
                string patchFolder = String.Format("{0} ({1})", "Patch", new DateTime().ToString("yyMMdd@HHmmss"));
                string result = "";
                if (!patchExeFS(Main.ExeFSPath, garcPaths, newPaths, ref result, Path.Combine(patchFolder, ".code.bin")))
                    return;

                Util.Alert("Patch contents saved to:" + Environment.NewLine + exportGARCs(garcPaths, newPaths, Main.RomFSPath, patchFolder), result);
            }
            catch (Exception ex)
            { Util.Error("Could not create patch:", ex.ToString()); }
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
            return garcs;
        }
        private string[] getPaths(string[] sc)
        {
            bool languages = CHK_Lang.Checked;
            StringCollection paths = new StringCollection();
            foreach (string s in sc)
                if (!languages || (s != "gametext" || s != "storytext"))
                    paths.Add(Main.getGARCFileName(s, Main.Language));
                else
                    for (int l = 0; l < 8; l++)
                        paths.Add(Main.getGARCFileName(s, l));

            string[] garcs = new string[paths.Count];
            paths.CopyTo(garcs, 0);
            return garcs;
        }
    }
}
