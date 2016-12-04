using System;
using System.IO;
using CTR;

namespace pk3DS
{
    public class GARCFile
    {
        private readonly GARC.MemGARC GARC;
        private readonly GARCReference Reference;
        private readonly string Path;

        public GARCFile(GARC.MemGARC g, GARCReference r, string p)
        {
            GARC = g;
            Reference = r;
            Path = p;
        }

        // Shorthand Alias
        public byte[] getFile(int file, int subfile = 0) { return GARC.getFile(file, subfile); }
        public byte[][] Files { get { return GARC.Files; } set { GARC.Files = value; } }
        public int FileCount => GARC.FileCount;

        public void Save()
        {
            File.WriteAllBytes(Path, GARC.Data);
            Console.WriteLine($"Wrote {Reference.Name} to {Reference.Reference}");
        }
    }
    public class lzGARCFile
    {
        private readonly GARC.lzGARC GARC;
        private readonly GARCReference Reference;
        private readonly string Path;

        public lzGARCFile(GARC.lzGARC g, GARCReference r, string p)
        {
            GARC = g;
            Reference = r;
            Path = p;
        }
        
        public int FileCount => GARC.FileCount;

        public byte[][] Files
        {
            get
            {
                byte[][] data = new byte[FileCount][];
                for (int i = 0; i < data.Length; i ++)
                    data[i] = GARC[i];
                return data;
            }
            set
            {
                for (int i = 0; i < value.Length; i++)
                    GARC[i] = value[i];
            }
        }

        public byte[] this[int file]
        {
            get { return GARC[file]; }
            set { GARC[file] = value; }
        }
        public void Save()
        {
            File.WriteAllBytes(Path, GARC.Save());
            Console.WriteLine($"Wrote {Reference.Name} to {Reference.Reference}");
        }
    }
}
