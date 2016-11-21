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
}
