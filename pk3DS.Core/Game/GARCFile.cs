using System;
using System.IO;
using pk3DS.Core.CTR;

namespace pk3DS.Core;

public class GARCFile(GARC.MemGARC g, GARCReference r, string p)
{
    // Shorthand Alias
    public byte[] GetFile(int file, int subfile = 0) { return g.GetFile(file, subfile); }
    public byte[][] Files { get => g.Files; set => g.Files = value; }
    public int FileCount => g.FileCount;

    public void Save()
    {
        File.WriteAllBytes(p, g.Data);
        Console.WriteLine($"Wrote {r.Name} to {r.Reference}");
    }
}

public class LazyGARCFile(GARC.LazyGARC g, GARCReference r, string p)
{
    public int FileCount => g.FileCount;

    public byte[][] Files
    {
        get
        {
            byte[][] data = new byte[FileCount][];
            for (int i = 0; i < data.Length; i++)
                data[i] = g[i];
            return data;
        }
        set
        {
            for (int i = 0; i < value.Length; i++)
                g[i] = value[i];
        }
    }

    public byte[] this[int file]
    {
        get => g[file];
        set => g[file] = value;
    }

    public void Save()
    {
        File.WriteAllBytes(p, g.Save());
        Console.WriteLine($"Wrote {r.Name} to {r.Reference}");
    }
}