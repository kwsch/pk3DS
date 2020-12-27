using System;
using System.IO;
using pk3DS.Core.CTR;

namespace pk3DS.Core
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
        public byte[] GetFile(int file, int subfile = 0) { return GARC.GetFile(file, subfile); }
        public byte[][] Files { get => GARC.Files; set => GARC.Files = value; }
        public int FileCount => GARC.FileCount;

        public void Save()
        {
            File.WriteAllBytes(Path, GARC.Data);
            Console.WriteLine($"Wrote {Reference.Name} to {Reference.Reference}");
        }
    }

    public class LazyGARCFile
    {
        private readonly GARC.LazyGARC GARC;
        private readonly GARCReference Reference;
        private readonly string Path;

        public LazyGARCFile(GARC.LazyGARC g, GARCReference r, string p)
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
            get => GARC[file];
            set => GARC[file] = value;
        }

        public void Save()
        {
            File.WriteAllBytes(Path, GARC.Save());
            Console.WriteLine($"Wrote {Reference.Name} to {Reference.Reference}");
        }
    }
}
