using System.Drawing;
using System.Drawing.Imaging;
using System.IO;

namespace pk3DS
{
    class mapCollisions
    {
        internal static Bitmap makeIMG(string path, bool save, int shift = 4, int s = 8)
        {
            byte[] byteArray = File.ReadAllBytes(path);
            using (Stream dataStream = new MemoryStream(byteArray)) // Open the file, even if it is in use.
            using (BinaryReader br = new BinaryReader(dataStream))
            {
                ushort width = br.ReadUInt16();
                ushort height = br.ReadUInt16();

                Bitmap img = new Bitmap(width * s, height * s);
                for (int i = 0; i < width * height; i++)
                {
                    uint color = br.ReadUInt32();
                    Color c;
                    if (color == 0x01000021)
                        c = Color.Black;
                    else
                    {
                        color = LCRNG(color, shift);
                        c = Color.FromArgb(0xFF, 0xFF - (byte)(color & 0xFF), 0xFF - (byte)((color >> 8) & 0xFF), 0xFF - (byte)(color >> 24 & 0xFF));
                    }
                    try
                    {
                        for (int x = 0; x < s; x++)
                            for (int y = 0; y < s; y++)
                                img.SetPixel((x + (i * s) % (img.Width)), y + ((i / width) * s), c);
                    }
                    catch { }
                }
                if (!save) return img;

                using (MemoryStream ms = new MemoryStream())
                {
                    //error will throw from here
                    img.Save(ms, ImageFormat.Png);
                    byte[] data = ms.ToArray();
                    string parent = Directory.GetParent(path).Name;
                    File.WriteAllBytes(Path.Combine(Path.GetDirectoryName(path), parent + ".png"), data);
                }
                return img;
            }
        }
        public static uint LCRNG(uint seed, int ctr)
        {
            for (int i = 0; i < ctr; i++)
            {
                seed *= 0x41C64E6D;
                seed += 0x00006073;
            }
            return seed;
        }
    }
}
