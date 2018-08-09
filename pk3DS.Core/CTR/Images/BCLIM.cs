using System;
using System.Drawing;
using System.IO;
using pk3DS.Core.CTR.Images;

namespace pk3DS.Core.CTR
{
    public class BCLIM : BXLIM
    {
        public BCLIM(Stream data) => ReadBCLIM(data);

        public BCLIM(byte[] data)
        {
            using (var ms = new MemoryStream(data))
                ReadBCLIM(ms);
        }

        public BCLIM(string path)
        {
            var data = File.ReadAllBytes(path);
            using (var ms = new MemoryStream(data))
                ReadBCLIM(ms);
        }

        private void ReadBCLIM(Stream ms)
        {
            PixelData = new byte[ms.Length - FLIMHeader.SIZE];
            ms.Read(PixelData, 0, PixelData.Length);
            var footer = new byte[FLIMHeader.SIZE];
            ms.Read(footer, 0, footer.Length);
            Footer = footer.ToStructure<CLIMHeader>();
        }

        public override uint[] GetPixels()
        {
            if (Format == (XLIMEncoding)7 && BitConverter.ToUInt16(PixelData, 0) == 2) // Gen6 Palette
                return GetPixelsViaPalette();
            return base.GetPixels();
        }

        private uint[] GetPixelsViaPalette()
        {
            using (var ms = new MemoryStream(PixelData))
            using (var br = new BinaryReader(ms))
            {
                if (br.ReadUInt16() != 2) return null;

                // read palette
                int count = br.ReadUInt16();
                uint[] colors = new uint[count];
                for (int i = 0; i < colors.Length; i++)
                    colors[i] = PixelConverter.GetDecodedPixelValue(br.ReadUInt16(), XLIMEncoding.RGB565);

                // read pixels
                bool half = colors.Length < 0x10;
                uint[] pixels = new uint[BaseSize * BaseSize];
                for (int i = 0; i < pixels.Length; i++)
                {
                    var b = br.ReadByte();
                    if (!half)
                    {
                        pixels[i] = colors[b];
                    }
                    else
                    {
                        pixels[i++] = colors[b & 0xF];
                        pixels[i] = colors[b >> 4];
                    }
                }
                return pixels;
            }
        }

        // todo: move System.Drawing utilization out, make encoding generic for bflim
        public static byte[] IMGToBCLIM(Image img, char fc)
        {
            Bitmap mBitmap = new Bitmap(img);
            MemoryStream ms = new MemoryStream();
            int bclimformat = 7; // Init to default (for X)

            if (fc == 'X')
                write16BitColorPalette(mBitmap, ref ms);
            else
            {
                bclimformat = Convert.ToInt16(fc.ToString(), 16);
                try
                {
                    writeGeneric(bclimformat, mBitmap, ref ms);
                }
                catch (Exception e)
                {
                    System.Media.SystemSounds.Beep.Play();
                    System.Diagnostics.Debug.WriteLine(e.ToString());
                }
            }

            long datalength = ms.Length;
            // Write the CLIM + imag data.
            using (BinaryWriter bw = new BinaryWriter(ms))
            {
                bw.Write((uint)0x4D494C43); // CLIM
                bw.Write((ushort)0xFEFF);   // BOM
                bw.Write((uint)0x14);
                bw.Write((ushort)0x0202);   // 2 2 
                bw.Write((uint)(datalength + 0x28));
                bw.Write((uint)1);
                bw.Write((uint)0x67616D69);
                bw.Write((uint)0x10);
                bw.Write((ushort)mBitmap.Width);
                bw.Write((ushort)mBitmap.Height);
                bw.Write((uint)bclimformat);
                bw.Write((uint)datalength);
            }
            return ms.ToArray();
        }

        public static byte[] getBCLIM(string path, char fc)
        {
            byte[] byteArray = File.ReadAllBytes(path);
            using (Stream BitmapStream = new MemoryStream(byteArray)) // Open the file, even if it is in use.
            {
                Image img = Image.FromStream(BitmapStream);
                return IMGToBCLIM(img, fc);
            }
        }

        public static Image makeBCLIM(string path, char fc)
        {
            byte[] bclim = getBCLIM(path, fc);
            string fp = Path.GetFileNameWithoutExtension(path);
            fp = "new_" + fp.Substring(fp.IndexOf('_') + 1);
            string pp = Path.GetDirectoryName(path);
            string newPath = Path.Combine(pp, fp + ".bclim");
            File.WriteAllBytes(newPath, bclim);

            return makeBMP(newPath);
        }

        public static Image makeBMP(string path, bool autosave = false, bool crop = true)
        {
            BCLIM bclim = analyze(path);
            if (bclim.Magic != 0x4D494C43)
            {
                System.Media.SystemSounds.Beep.Play();
                return null;
            }

            Bitmap img = bclim.GetBitmap(crop);
            if (img == null)
                return null;
            if (crop)
                img = ImageUtil.CropBMP(bclim, img);
            if (autosave)
                img.Save(Path.Combine(bclim.FilePath, $"{bclim.FileName}.png"));
            return img;
        }

        // BCLIM Data Writing
        public static int write16BitColorPalette(Bitmap img, ref MemoryStream ms)
        {
            using (Stream pixelcolors = new MemoryStream())
            using (BinaryWriter bz = new BinaryWriter(pixelcolors))
            {
                // Set up our basis.
                bool under16colors = false;
                int colors = getColorCount(img);
                Color[] pcs = new Color[colors];
                if (colors < 16) under16colors = true;
                uint div = 1;
                if (under16colors)
                    div = 2;

                if (colors > 70) throw new Exception("Too many colors");

                // Set up a new reverse image to build into.
                int w = XLIMUtil.gcm(img.Width, 8);
                int h = XLIMUtil.gcm(img.Height, 8);
                w = Math.Max(XLIMUtil.nlpo2(w), XLIMUtil.nlpo2(h));
                h = w;
                byte[] pixelarray = new byte[w * h];

                const int colorformat = 2;
                int ctr = 1;

                pcs[0] = Color.FromArgb(0, 0xFF, 0xFF, 0xFF);

                int p = XLIMUtil.gcm(w, 8) / 8;
                if (p == 0) p = 1;
                int d = 0;
                for (uint i = 0; i < pixelarray.Length; i++)
                {
                    d = (int)(i / div);
                    // Get Tile Coordinate
                    uint x;
                    uint y;
                    XLIMOrienter.d2xy(i % 64, out x, out y);

                    // Get Shift Tile
                    uint tile = i / 64;

                    // Shift Tile Coordinate into Tilemap
                    x += (uint)(tile % p) * 8;
                    y += (uint)(tile / p) * 8;
                    if (x >= img.Width || y >= img.Height) // Don't try to access any pixel data outside of our bounds.
                    { i++; continue; } // Goto next tile.

                    // Get Color of Pixel
                    Color c = img.GetPixel((int)x, (int)y);

                    // Color Table Building Logic
                    int index = Array.IndexOf(pcs, c);
                    if (c.A == 0) index = 0;
                    if (index < 0)                          // If new color
                    { pcs[ctr] = c; index = ctr; ctr++; }   // Add it to color list

                    // Add pixel to pixeldata
                    if (under16colors) index = index << 4;
                    pixelarray[i / div] = (byte)index;
                    if (!under16colors) continue;

                    c = img.GetPixel((int)x + 1, (int)y);
                    index = Array.IndexOf(pcs, c);
                    if (c.A == 0) index = 0;
                    if (index < 0)  // If new color
                    { pcs[ctr] = c; index = ctr; ctr++; }
                    pixelarray[i / div] |= (byte)index;
                    i++;
                }

                // Write Intro
                bz.Write((ushort)colorformat); bz.Write((ushort)ctr);
                // Write Colors
                for (int i = 0; i < ctr; i++)
                    bz.Write((ushort)GetRGBA5551(pcs[i]));      // Write byte array.
                // Write Pixel Data
                for (uint i = 0; i < d; i++)
                    bz.Write(pixelarray[i]);
                // Write Padding
                while (pixelcolors.Length < XLIMUtil.nlpo2((int)pixelcolors.Length))
                    bz.Write((byte)0);
                // Copy to main CLIM.
                pixelcolors.Position = 0; pixelcolors.CopyTo(ms);
            }
            return 7;
        }

        public static void writeGeneric(int format, Bitmap img, ref MemoryStream ms, bool rectangle = true)
        {
            BinaryWriter bz = new BinaryWriter(ms);
            bz.Write(getPixelData(img, format, rectangle));
            bz.Flush();
        }

        public static byte[] MakePixelData(byte[] input, ref int w, ref int h, XLIMOrientation x = XLIMOrientation.None)
        {
            int width = w;
            w = XLIMUtil.nlpo2(w);
            h = XLIMUtil.nlpo2(h);
            if (!(Math.Min(w, h) < 32))
                w = h = Math.Max(w, h); // resize

            byte[] pixels = new byte[w * h * 4];
            var orienter = new XLIMOrienter(w, h, x);
            for (uint i = 0; i < pixels.Length / 4; i++)
            {
                var c = orienter.Get(i);
                var offset = (c.X * 4) + (c.Y * width);
                Array.Copy(input, offset, pixels, i * 4, 4);
            }
            return pixels;
        }

        public static byte[] getPixelData(Bitmap img, int format, bool rectangle = true)
        {
            int w = img.Width;
            int h = img.Height;

            bool perfect = w == h && w != 0 && (w & (w - 1)) == 0;
            if (!perfect) // Check if square power of two, else resize
            {
                // Square Format Checks
                if (rectangle && Math.Min(img.Width, img.Height) < 32)
                {
                    w = XLIMUtil.nlpo2(img.Width);
                    h = XLIMUtil.nlpo2(img.Height);
                }
                else
                {
                    w = h = Math.Max(XLIMUtil.nlpo2(w), XLIMUtil.nlpo2(h)); // else resize
                }
            }

            using (MemoryStream mz = new MemoryStream())
            using (BinaryWriter bz = new BinaryWriter(mz))
            {
                int p = XLIMUtil.gcm(w, 8) / 8;
                if (p == 0) p = 1;
                for (uint i = 0; i < w * h; i++)
                {
                    XLIMOrienter.d2xy(i % 64, out uint x, out uint y);

                    // Get Shift Tile
                    uint tile = i / 64;

                    // Shift Tile Coordinate into Tilemap
                    x += (uint)(tile % p) * 8;
                    y += (uint)(tile / p) * 8;

                    // Don't write data
                    Color c;
                    if (x >= img.Width || y >= img.Height)
                    { c = Color.FromArgb(0, 0, 0, 0); }
                    else
                    { c = img.GetPixel((int)x, (int)y); if (c.A == 0) c = Color.FromArgb(0, 86, 86, 86); }

                    switch (format)
                    {
                        case 0: bz.Write((byte)GetL8(c)); break;                // L8
                        case 1: bz.Write((byte)GetA8(c)); break;                // A8
                        case 2: bz.Write((byte)GetLA4(c)); break;               // LA4(4)
                        case 3: bz.Write((ushort)GetLA8(c)); break;             // LA8(8)
                        case 4: bz.Write((ushort)GetHILO8(c)); break;           // HILO8
                        case 5: bz.Write((ushort)GetRGB565(c)); break;          // RGB565
                        case 6:
                        {
                            bz.Write(c.B);
                            bz.Write(c.G);
                            bz.Write(c.R); break;
                        }
                        case 7: bz.Write((ushort)GetRGBA5551(c)); break;        // RGBA5551
                        case 8: bz.Write((ushort)GetRGBA4444(c)); break;        // RGBA4444
                        case 9: bz.Write((uint)GetRGBA8888(c)); break;          // RGBA8
                        case 10: throw new Exception("ETC1 not supported.");
                        case 11: throw new Exception("ETC1A4 not supported.");
                        case 12:
                        {
                            byte val = (byte)(GetL8(c) / 0x11); // First Pix    // L4
                            { c = img.GetPixel((int)x, (int)y); if (c.A == 0) c = Color.FromArgb(0, 0, 0, 0); }
                            val |= (byte)((GetL8(c) / 0x11) << 4); i++;
                            bz.Write(val); break;
                        }
                        case 13:
                        {
                            byte val = (byte)(GetA8(c) / 0x11); // First Pix    // L4
                            { c = img.GetPixel((int)x, (int)y); }
                            val |= (byte)((GetA8(c) / 0x11) << 4); i++;
                            bz.Write(val); break;
                        }
                    }
                }
                if (!perfect)
                    while (mz.Length < XLIMUtil.nlpo2((int)mz.Length)) // pad
                        bz.Write((byte)0);
                return mz.ToArray();
            }
        }

        public static int getColorCount(Bitmap img)
        {
            Color[] colors = new Color[img.Width * img.Height];
            int colorct = 1;

            for (int i = 0; i < colors.Length; i++)
            {
                Color c = img.GetPixel(i % img.Width, i / img.Width);
                int index = Array.IndexOf(colors, c);
                if (c.A == 0) index = 0;
                if (index >= 0) continue;

                colors[colorct] = c;
                colorct++;
            }
            return colorct;
        }

        // Color Conversion
        internal static byte GetL8(Color c)
        {
            byte red = c.R;
            byte green = c.G;
            byte blue = c.B;
            // Luma (Y’) = 0.299 R’ + 0.587 G’ + 0.114 B’ from wikipedia
            return (byte)((((0x4CB2 * red) + (0x9691 * green) + (0x1D3E * blue)) >> 16) & 0xFF);
        }        // L8

        internal static byte GetA8(Color c)
        {
            return c.A;
        }        // A8

        internal static byte GetLA4(Color c)
        {
            return (byte)((c.A / 0x11) + (c.R / 0x11) << 4);
        }       // LA4

        internal static ushort GetLA8(Color c)
        {
            return (ushort)(c.A + (c.R << 8));
        }     // LA8

        internal static ushort GetHILO8(Color c)
        {
            return (ushort)(c.G + (c.R << 8));
        }   // HILO8

        internal static ushort GetRGB565(Color c)
        {
            int val = 0;
            // val += c.A >> 8; // unused
            val += convert8to5(c.B) >> 3;
            val += (c.G >> 2) << 5;
            val += convert8to5(c.R) << 10;
            return (ushort)val;
        }  // RGB565
        // RGB8
        internal static ushort GetRGBA5551(Color c)
        {
            int val = 0;
            val += (byte)(c.A > 0x80 ? 1 : 0);
            val += convert8to5(c.R) << 11;
            val += convert8to5(c.G) << 6;
            val += convert8to5(c.B) << 1;
            ushort v = (ushort)val;

            return v;
        }// RGBA5551

        internal static ushort GetRGBA4444(Color c)
        {
            int val = 0;
            val += c.A / 0x11;
            val += (c.B / 0x11) << 4;
            val += (c.G / 0x11) << 8;
            val += (c.R / 0x11) << 12;
            return (ushort)val;
        }// RGBA4444

        internal static uint GetRGBA8888(Color c)     // RGBA8888
        {
            uint val = 0;
            val += c.A;
            val += (uint)(c.B << 8);
            val += (uint)(c.G << 16);
            val += (uint)(c.R << 24);
            return val;
        }

        // Unit Conversion
        internal static byte convert8to5(int colorval)
        {
            byte[] Convert8to5 = { 0x00,0x08,0x10,0x18,0x20,0x29,0x31,0x39,
                0x41,0x4A,0x52,0x5A,0x62,0x6A,0x73,0x7B,
                0x83,0x8B,0x94,0x9C,0xA4,0xAC,0xB4,0xBD,
                0xC5,0xCD,0xD5,0xDE,0xE6,0xEE,0xF6,0xFF };
            byte i = 0;
            while (colorval > Convert8to5[i]) i++;
            return i;
        }

        public static BCLIM analyze(byte[] data, string shortPath)
        {
            BCLIM bclim = new BCLIM(data)
            {
                FileName = Path.GetFileNameWithoutExtension(shortPath),
                FilePath = Path.GetDirectoryName(shortPath),
                Extension = Path.GetExtension(shortPath)
            };
            return bclim;
        }

        public static BCLIM analyze(string path)
        {
            byte[] data = File.ReadAllBytes(path);
            return analyze(data, path);
        }
    }
}