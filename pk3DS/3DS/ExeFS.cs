using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Security.Cryptography;

namespace CTR
{
    public class ExeFS
    {
        public byte[] Data;
        public byte[] SuperBlockHash;

        // Return an object with data stored in a byte array
        public ExeFS(string EXEFS_PATH)
        {
            if (Directory.Exists(EXEFS_PATH))
            {
                var files = new DirectoryInfo(EXEFS_PATH).GetFiles().Select(f => f.FullName).ToArray();
                setData(files);
            }
            else
            {
                Data = File.ReadAllBytes(EXEFS_PATH);
            }
            getSuperBlockHash();
        }

        public void getSuperBlockHash()
        {
            using (SHA256Managed sha = new SHA256Managed())
                SuperBlockHash = sha.ComputeHash(Data, 0, 0x200);
        }

        // Overall R/W files (wrapped)
        internal static bool get(string inFile, string outPath)
        {
            try
            {
                byte[] data = File.ReadAllBytes(inFile);
                if (!Directory.Exists(outPath)) Directory.CreateDirectory(outPath);
                for (int i = 0; i < 10; i++)
                {
                    // Get File Name String; if exists we have a file to extract.
                    string fileName = Encoding.ASCII.GetString(data.Skip(0x10 * i).Take(0x8).ToArray()).TrimEnd((char)0);
                    if (fileName.Length > 0)
                        File.WriteAllBytes(
                            // New File Path
                            outPath + Path.DirectorySeparatorChar + fileName + ".bin",
                            // Get New Data from Offset after 0x200 Header.
                            data.Skip(0x200 + BitConverter.ToInt32(data, 0x8 + 0x10 * i)).Take(BitConverter.ToInt32(data, 0xC + 0x10 * i)).ToArray()
                            );
                }
                return true;
            }
            catch { return false; }
        }
        internal static bool set(string[] files, string outFile)
        {
            if (files.Length > 10) { Console.WriteLine("Cannot package more than 10 files to exefs."); return false; }

            try
            {
                // Set up the Header
                byte[] headerData = new byte[0x200];
                uint offset = 0;
                SHA256 sha = SHA256.Create();

                // Get the Header
                for (int i = 0; i < files.Length; i++)
                {
                    // Do the Top (File Info)
                    string fileName = Path.GetFileNameWithoutExtension(files[i]);
                    byte[] nameData = Encoding.ASCII.GetBytes(fileName); Array.Resize(ref nameData, 0x8);
                    Array.Copy(nameData, 0, headerData, i * 0x10, 0x8);

                    FileInfo fi = new FileInfo(files[i]);
                    uint size = (uint)fi.Length;
                    Array.Copy(BitConverter.GetBytes(offset), 0, headerData, 0x8 + i * 0x10, 0x4);
                    Array.Copy(BitConverter.GetBytes(size), 0, headerData, 0xC + i * 0x10, 0x4);
                    offset += 0x200 - size % 0x200 + size;

                    // Do the Bottom (Hashes)
                    byte[] hash = sha.ComputeHash(File.ReadAllBytes(files[i]));
                    Array.Copy(hash, 0, headerData, 0x200 - 0x20 * (i + 1), 0x20);
                }

                // Set in the Data
                using (MemoryStream newFile = new MemoryStream())
                {
                    new MemoryStream(headerData).CopyTo(newFile);
                    foreach (string s in files)
                    {
                        using (MemoryStream loadFile = new MemoryStream(File.ReadAllBytes(s)))
                            loadFile.CopyTo(newFile);
                        new MemoryStream(new byte[0x200 - newFile.Length % 0x200]).CopyTo(newFile);
                    }

                    File.WriteAllBytes(outFile, newFile.ToArray());
                }
                return true;
            }
            catch { return false; }
        }

        public void setData(string[] files)
        {
            // Set up the Header
            byte[] headerData = new byte[0x200];
            uint offset = 0;
            SHA256 sha = SHA256.Create();

            // Get the Header
            for (int i = 0; i < files.Length; i++)
            {
                // Do the Top (File Info)
                string fileName = Path.GetFileNameWithoutExtension(files[i]);
                byte[] nameData = Encoding.ASCII.GetBytes(fileName); Array.Resize(ref nameData, 0x8);
                Array.Copy(nameData, 0, headerData, i * 0x10, 0x8);

                FileInfo fi = new FileInfo(files[i]);
                uint size = (uint)fi.Length;
                Array.Copy(BitConverter.GetBytes(offset), 0, headerData, 0x8 + i * 0x10, 0x4);
                Array.Copy(BitConverter.GetBytes(size), 0, headerData, 0xC + i * 0x10, 0x4);
                offset += 0x200 - size % 0x200 + size;

                // Do the Bottom (Hashes)
                byte[] hash = sha.ComputeHash(File.ReadAllBytes(files[i]));
                Array.Copy(hash, 0, headerData, 0x200 - 0x20 * (i + 1), 0x20);
            }

            // Set in the Data
            using (MemoryStream newFile = new MemoryStream())
            {
                new MemoryStream(headerData).CopyTo(newFile);
                foreach (string s in files)
                {
                    using (MemoryStream loadFile = new MemoryStream(File.ReadAllBytes(s)))
                        loadFile.CopyTo(newFile);
                    new MemoryStream(new byte[0x200 - newFile.Length % 0x200]).CopyTo(newFile);
                }

                Data = newFile.ToArray();
            }
        }
    }
}
