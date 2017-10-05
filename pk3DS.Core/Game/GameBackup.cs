using System;
using System.IO;
using System.Linq;

namespace pk3DS.Core
{
    public static class GameBackup
    {
        public const string bakpath = "backup";
        public const string bakexefs = "exefs";
        public const string baka = "a";
        public const string bakdll = "dll";

        public static void backupFiles(this GameConfig config, bool overwrite = false)
        {
            // Users may use pk3DS for multiple games, and even the same game but from different paths.
            // A simple way is to create a backup for each unique game, but... some carts may be pre-patched.
            // Just save the backup based on the folder name, as the user may move that parent folder.
            // Store a text file in each backup to keep track of its origin in case they rename the folder.

            if (!Directory.Exists(bakpath))
                Directory.CreateDirectory(bakpath);

            var gamePath = new DirectoryInfo(config.RomFS).Parent;
            string gameFolder = gamePath.Name;
            string gameBackup = Path.Combine(bakpath, gameFolder);
            if (!Directory.Exists(gameBackup))
                Directory.CreateDirectory(gameBackup);

            string bak_exefs = Path.Combine(gameBackup, bakexefs);
            string bak_a = Path.Combine(gameBackup, baka);
            string bak_dll = Path.Combine(gameBackup, bakdll);
            if (!Directory.Exists(bak_exefs))
                Directory.CreateDirectory(bak_exefs);
            if (!Directory.Exists(bak_a))
                Directory.CreateDirectory(bak_a);
            if (!Directory.Exists(bak_dll))
                Directory.CreateDirectory(bak_dll);

            // Backup files
            if (config.ExeFS != null) // exefs
                backupExeFS(config, overwrite, bak_exefs);
            if (config.RomFS != null) // a
                backupGARC(config, overwrite, bak_a);
            if (config.RomFS != null) // dll
                backupDLL(config, overwrite, bak_dll);

            File.WriteAllText(Path.Combine(gameBackup, "bakinfo.txt"), "Backup created from the following location:" + Environment.NewLine + gamePath.FullName);
        }
        private static void backupExeFS(GameConfig config, bool overwrite, string bak_exefs)
        {
            var files = Directory.GetFiles(config.ExeFS);
            foreach (var f in files)
            {
                string dest = Path.Combine(bak_exefs, Path.GetFileName(f));
                if (overwrite || !File.Exists(dest))
                    File.Copy(f, dest);
            }
        }
        private static void backupGARC(GameConfig config, bool overwrite, string bak_a)
        {
            var files = config.Files.Select(file => file.Name);
            foreach (var f in files)
            {
                string GARC = config.getGARCFileName(f);
                string name = f + $" ({GARC.Replace(Path.DirectorySeparatorChar.ToString(), "")})";
                string src = Path.Combine(config.RomFS, GARC);
                string dest = Path.Combine(bak_a, name);
                if (overwrite || !File.Exists(dest))
                    File.Copy(src, dest);
            }
        }
        private static void backupDLL(GameConfig config, bool overwrite, string bak_dll)
        {
            string path = config.RomFS;
            string[] files = Directory.GetFiles(path);
            string[] CROs = files.Where(x => new FileInfo(x).Name.Contains("Dll")).ToArray();
            string[] CRSs = files.Where(x => new FileInfo(x).Extension.Contains("crs")).ToArray();
            string[] CRRs = Directory.Exists(Path.Combine(path, ".crr"))
                ? Directory.GetFiles(Path.Combine(path, ".crr"))
                : new string[0];

            int count = CROs.Length + CRSs.Length + CRRs.Length;
            if (count <= 0)
                return;

            if (!Directory.Exists(bak_dll))
                Directory.CreateDirectory(bak_dll);

            foreach (string src in CROs.Concat(CRSs))
            {
                string dest = Path.Combine(bak_dll, Path.GetFileName(src));
                if (overwrite || !File.Exists(dest))
                    File.Copy(src, dest);
            }

            if (CRRs.Length <= 0)
                return;

            // Separate folder for the .crr
            string CRRBAKPATH = Path.Combine(bak_dll, ".crr");
            if (!Directory.Exists(CRRBAKPATH))
                Directory.CreateDirectory(CRRBAKPATH);

            foreach (string src in CRRs)
            {
                string dest = Path.Combine(CRRBAKPATH, Path.GetFileName(src));
                if (overwrite || !File.Exists(dest))
                    File.Copy(src, dest);
            }
        }

        public static string[] restoreFiles(this GameConfig config)
        {
            // Do the same process as backing up, but copy files in the opposite direction.

            string gameFolder = new DirectoryInfo(config.RomFS).Parent.Name;
            string gameBackup = Path.Combine(bakpath, gameFolder);
            if (!Directory.Exists(gameBackup))
                return new[] {"Unable to find the backup folder for this game.", $"Expected:\n{gameBackup}"};

            string bak_exefs = Path.Combine(gameBackup, bakexefs);
            string bak_a = Path.Combine(gameBackup, baka);
            string bak_dll = Path.Combine(gameBackup, bakdll);

            int[] count = new int[3];

            // restore exefs
            if (Directory.Exists(bak_exefs))
                count[0] = restoreExeFS(config, bak_exefs);
            if (Directory.Exists(bak_a))
                count[1] = restoreGARC(config, bak_a);
            if (Directory.Exists(bak_dll))
                count[2] = restoreDLL(config, bak_dll);

            string[] sources = { "ExeFS", "'a'", "CRO" };
            var info = count.Select((c, i) => $"{sources[i]}: {c}");
            var result = string.Join(Environment.NewLine, info);
            return new[] {result};
        }
        private static int restoreExeFS(GameConfig config, string bak_exefs)
        {
            int count = 0;
            var files = Directory.GetFiles(config.ExeFS);
            foreach (var src in files)
            {
                string dest = Path.Combine(bak_exefs, Path.GetFileName(src));
                if (File.Exists(dest))
                {
                    try { File.Copy(dest, src, overwrite: true); count++; }
                    catch { Console.WriteLine("Unable to overwrite backup: " + dest); }
                }
                else
                    Console.WriteLine("Unable to find backup: " + dest);
            }
            return count;
        }
        private static int restoreGARC(GameConfig config, string bak_a)
        {
            int count = 0;
            var files = config.Files.Select(file => file.Name);
            foreach (var f in files)
            {
                string GARC = config.getGARCFileName(f);
                string name = f + $" ({GARC.Replace(Path.DirectorySeparatorChar.ToString(), "")})";
                string src = Path.Combine(config.RomFS, GARC);
                string dest = Path.Combine(bak_a, name);
                if (File.Exists(dest))
                {
                    try { File.Copy(dest, src, overwrite: true); count++; }
                    catch { Console.WriteLine("Unable to overwrite backup: " + dest); }
                }
                else
                    Console.WriteLine("Unable to find backup: " + dest);
            }
            return count;
        }
        private static int restoreDLL(GameConfig config, string bak_dll)
        {
            int count = 0;

            string path = config.RomFS;
            string[] files = Directory.GetFiles(path);
            string[] CROs = files.Where(x => new FileInfo(x).Name.Contains("Dll")).ToArray();
            string[] CRSs = files.Where(x => new FileInfo(x).Extension.Contains("crs")).ToArray();
            string[] CRRs = Directory.Exists(Path.Combine(path, ".crr"))
                ? Directory.GetFiles(Path.Combine(path, ".crr"))
                : new string[0];

            if (CROs.Length + CRSs.Length + CRRs.Length <= 0)
                return 0;

            foreach (string src in CROs.Concat(CRSs))
            {
                string dest = Path.Combine(bak_dll, Path.GetFileName(src));
                if (File.Exists(dest))
                {
                    try { File.Copy(dest, src, overwrite: true); count++; }
                    catch { Console.WriteLine("Unable to overwrite backup: " + dest); }
                }
                else
                    Console.WriteLine("Unable to find backup: " + dest);
            }

            if (CRRs.Length <= 0)
                return count;

            // Separate folder for the .crr
            string CRRBAKPATH = Path.Combine(bak_dll, ".crr");
            foreach (string src in CRRs)
            {
                string dest = Path.Combine(CRRBAKPATH, Path.GetFileName(src));
                if (File.Exists(dest))
                {
                    File.Copy(dest, src, true);
                    count++;
                }
                else
                    Console.WriteLine("Unable to find backup: " + dest);
            }
            return count;
        }
    }
}
