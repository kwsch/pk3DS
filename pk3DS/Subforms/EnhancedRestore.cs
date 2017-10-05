using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Windows.Forms;

using pk3DS.Core;

namespace pk3DS
{
    public partial class EnhancedRestore : Form
    {
        public EnhancedRestore(GameConfig config)
        {
            InitializeComponent();
            LoadBackupFileInfo(config);
        }

        private void LoadBackupFileInfo(GameConfig config)
        {
            var gamePath = new DirectoryInfo(config.RomFS).Parent;
            string gameFolder = gamePath.Name;
            string gameBackup = Path.Combine(GameBackup.bakpath, gameFolder);

            string bak_a = Path.Combine(gameBackup, GameBackup.baka);
            string bak_exefs = Path.Combine(gameBackup, GameBackup.bakexefs);
            string bak_dll = Path.Combine(gameBackup, GameBackup.bakdll);

            var garcs = GetRestorableGarcs(config, bak_a);
            var exefs = GetRestorableExeFS(config, bak_exefs);
            var cros = GetRestorableCROs(config, bak_dll);

            var files = new[] { garcs, exefs, cros };

            // load files to UI
            for (int i = 0; i < files.Length; i++)
            {
                Items.Add(new List<RestoreInfo>());
                var tab = tabControl1.TabPages[i];
                var clb = new CheckedListBox
                {
                    Dock = DockStyle.Fill,
                    CheckOnClick = true,
                    Margin = new Padding(0),
                    Padding = new Padding(0)
                };
                foreach (var z in files[i])
                {
                    Items[i].Add(z);
                    clb.Items.Add(z.DisplayName, true);
                }
                List.Add(clb);
                tab.Controls.Add(clb);
            }
        }

        private static IEnumerable<RestoreInfo> GetRestorableCROs(GameConfig config, string bak_dll)
        {
            string path = config.RomFS;
            string[] files = Directory.GetFiles(path);
            string[] CROs = files.Where(x => new FileInfo(x).Name.Contains("Dll")).ToArray();
            string[] CRSs = files.Where(x => new FileInfo(x).Extension.Contains("crs")).ToArray();
            var CRRs = Directory.Exists(Path.Combine(path, ".crr"))
                ? Directory.EnumerateFiles(Path.Combine(path, ".crr"))
                : new string[0];
            string CRRBAKPATH = Path.Combine(bak_dll, ".crr");

            foreach (string src in CROs.Concat(CRSs))
            {
                string dest = Path.Combine(bak_dll, Path.GetFileName(src));
                if (File.Exists(dest))
                    yield return new RestoreInfo(dest, src);
            }

            // Separate folder for the .crr
            foreach (string src in CRRs)
            {
                string dest = Path.Combine(CRRBAKPATH, Path.GetFileName(src));
                if (File.Exists(dest))
                    yield return new RestoreInfo(dest, src);
            }
        }

        private static IEnumerable<RestoreInfo> GetRestorableGarcs(GameConfig config, string bak_a)
        {
            var files = config.Files.Select(file => file.Name);
            foreach (var f in files)
            {
                string GARC = config.getGARCFileName(f);
                string name =  $"{f} ({GARC.Replace(Path.DirectorySeparatorChar.ToString(), "")})";

                string src = Path.Combine(config.RomFS, GARC);
                string dest = Path.Combine(bak_a, name);

                if (!File.Exists(dest))
                    continue;
                string dispname = Path.GetFileNameWithoutExtension(dest);
                var split = dispname.Split(' ');
                dispname = $"{split[1]} {split[0]}";
                yield return new RestoreInfo(dest, src, dispname);
            }
        }

        private static IEnumerable<RestoreInfo> GetRestorableExeFS(GameConfig config, string bak_exefs)
        {
            var files = Directory.GetFiles(config.ExeFS);
            foreach (var src in files)
            {
                string dest = Path.Combine(bak_exefs, Path.GetFileName(src));

                if (File.Exists(dest))
                    yield return new RestoreInfo(dest, src);
            }
        }

        private class RestoreInfo
        {
            public readonly string DisplayName;
            public readonly string FileLocation;
            public readonly string Destination;

            public RestoreInfo(string src, string dest, string disp = null)
            {
                FileLocation = src;
                Destination = dest;
                DisplayName = disp ?? Path.GetFileName(src);
            }
        }

        private readonly List<List<RestoreInfo>> Items = new List<List<RestoreInfo>>();
        private readonly List<CheckedListBox> List = new List<CheckedListBox>();

        private void B_Go_Click(object sender, EventArgs e)
        {
            // restore files that are selected
            int count = 0;
            for (int i = 0; i < List.Count; i++)
            {
                for (int j = 0; j < List[i].Items.Count; j++)
                {
                    if (!List[i].GetItemChecked(j))
                        continue;

                    var item = Items[i][j];
                    string dest = item.Destination;
                    string src = item.FileLocation;

                    try
                    {
                        if (File.Exists(dest)) // only restore files that exist
                            File.Copy(dest, src, overwrite: true); count++;
                    }
                    catch { Debug.WriteLine("Unable to overwrite backup: " + dest); }
                }
            }

            WinFormsUtil.Alert($"Restored {count} file(s).", "The program will now close.");
            Application.Exit(); // do not call closing events that repackage personal/gametext
        }

        private void B_All_Click(object sender, EventArgs e)
        {
            var clb = List[tabControl1.SelectedIndex];
            for (int i = 0; i < clb.Items.Count; i++)
                clb.SetItemChecked(i, ModifierKeys != Keys.Control);
        }
    }
}
