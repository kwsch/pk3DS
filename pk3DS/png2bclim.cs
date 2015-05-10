using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace pk3DS
{
    class png2bclim
    {
        internal static void openFile(string path, bool autosave = false, bool crop = true, char format = 'X')
        {
            // Handle file
            if (!File.Exists(path)) throw new Exception("Can only accept files, not folders");
            string ext = Path.GetExtension(path);
            if (ext == ".png")
                makeBCLIM(path, format);
            else if (ext == ".bin" || ext == ".bclim")
                makeBMP(path, autosave, crop);
        }

        internal static Image makeBCLIM(string path, char fc)
        {
            byte[] byteArray = File.ReadAllBytes(path);
            using (Stream BitmapStream = new MemoryStream(byteArray)) // Open the file, even if it is in use.
            {
                Image img = Image.FromStream(BitmapStream);
                Bitmap mBitmap = new Bitmap(img);
                // Pixel format acquired. Now to determine how we want to save the data.
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
                    bw.Write((uint)(datalength+0x28));
                    bw.Write((uint)1);
                    bw.Write((uint)0x67616D69);
                    bw.Write((uint)0x10);
                    bw.Write((ushort)mBitmap.Width);
                    bw.Write((ushort)mBitmap.Height);
                    bw.Write((uint)bclimformat);
                    bw.Write((uint)datalength);
                }
                string fp = Path.GetFileNameWithoutExtension(path);
                fp = "new_" + fp.Substring(fp.IndexOf('_') + 1);
                string pp = Path.GetDirectoryName(path);
                string newPath = Path.Combine(pp, fp + ".bclim");
                File.WriteAllBytes(newPath, ms.ToArray());

                return makeBMP(newPath);
            }
        }
        internal static Image makeBMP(string path, bool autosave = false, bool crop = true)
        {
            CLIM bclim = BCLIM.analyze(path);
            if (bclim.Magic != 0x4D494C43)
            {
                System.Media.SystemSounds.Beep.Play(); 
                return null;
            }

            // Interpret data.
            int f = bclim.FileFormat;
            if (f > 13)
            {
                System.Media.SystemSounds.Exclamation.Play(); 
                return null; 
            }

            Bitmap img;
            if (f == 7 && BitConverter.ToUInt16(bclim.Data, 0) == 2)
                // PKM XY Format 7 (Color Palette)
                img = getIMG_XY7(bclim);
            else if (f == 10 || f == 11)
                img = getIMG_ETC(bclim);
            else
                img = getIMG(bclim);

            Rectangle cropRect = new Rectangle(0, 0, bclim.Width, bclim.Height);
            Bitmap CropBMP = new Bitmap(cropRect.Width, cropRect.Height);
            using (Graphics g = Graphics.FromImage(CropBMP))
            {
                g.DrawImage(img, 
                            new Rectangle(0, 0, CropBMP.Width, CropBMP.Height),
                            cropRect,
                            GraphicsUnit.Pixel);
            }
            if (!autosave) return !crop ? img : CropBMP;

            using (MemoryStream ms = new MemoryStream())
            {
                //error will throw from here
                CropBMP.Save(ms, ImageFormat.Png);
                byte[] data = ms.ToArray();
                File.WriteAllBytes(bclim.FilePath + "\\" + bclim.FileName + ".png", data);
            }
            return !crop ? img : CropBMP;
        }
        // Bitmap Data Writing
        internal static Bitmap getIMG(CLIM bclim)
        {
            // New Image
            Bitmap img = new Bitmap(nlpo2(gcm(bclim.Width,8)), nlpo2(gcm(bclim.Height,8)));
            int f = bclim.FileFormat;
            int area = img.Width * img.Height;
            if (f == 9 && area > bclim.Data.Length/4)
            {
                img = new Bitmap(gcm(bclim.Width, 8), gcm(bclim.Height, 8)); 
                area = img.Width * img.Height;
            }
            // Coordinates
            // Colors
            // Tiles Per Width
            int p = gcm(img.Width, 8) / 8;
            if (p == 0) p = 1;

            // Build Image
            using (Stream BitmapStream = new MemoryStream(bclim.Data))
            using (BinaryReader br = new BinaryReader(BitmapStream))
            for (uint i = 0; i < area; i++) // for every pixel
            {
                uint x;
                uint y;
                d2xy(i % 64, out x, out y);
                uint tile = i / 64;

                // Shift Tile Coordinate into Tilemap
                x += (uint)(tile % p) * 8;
                y += (uint)(tile / p) * 8;

                // Get Color
                Color c;
                switch (f)
                {
                    case 0x0:  // L8        // 8bit/1 byte
                    case 0x1:  // A8
                    case 0x2:  // LA4
                        c = DecodeColor(br.ReadByte(), f);
                        break;
                    case 0x3:  // LA8       // 16bit/2 byte
                    case 0x4:  // HILO8
                    case 0x5:  // RGB565
                    case 0x8:  // RGBA4444
                    case 0x7:  // RGBA5551
                        c = DecodeColor(br.ReadUInt16(), f);
                        break;
                    case 0x6:  // RGB8:     // 24bit
                        byte[] data = br.ReadBytes(3); Array.Resize(ref data, 4);
                        c = DecodeColor(BitConverter.ToUInt32(data, 0), f);
                        break;
                    case 0x9:  // RGBA8888
                        c = DecodeColor(br.ReadUInt32(), f);
                        break;
                    case 0xC:  // L4
                    case 0xD:  // A4        // 4bit - Do 2 pixels at a time.
                        uint val = br.ReadByte();
                        img.SetPixel((int)x, (int)y, DecodeColor(val & 0xF, f)); // lowest bits for the low pixel
                        i++; x++;
                        c = DecodeColor(val >> 4, f);   // highest bits for the high pixel
                        break;
                    default: throw new Exception("Invalid FileFormat.");
                }
                img.SetPixel((int)x, (int)y, c);
            }
            return img;
        }
        internal static Bitmap getIMG_XY7(CLIM bclim)
        {
            Bitmap img = new Bitmap(bclim.BaseSize, bclim.BaseSize);
            using (Stream BitmapStream = new MemoryStream(bclim.Data))
            using (BinaryReader br = new BinaryReader(BitmapStream))
            {
                // Fetch Color stuff.
                if (br.ReadUInt16() != 2) return null;
                ushort colors = br.ReadUInt16();
                Color[] ca = new Color[colors];
                for (int i = 0; i < colors; i++)
                    ca[i] = DecodeColor(br.ReadUInt16(), 7);

                // Coordinates
                // Colors
                // Tiles Per Width
                int p = gcm(img.Width, 8) / 8;
                if (p == 0) p = 1;

                for (uint i = 0; i < bclim.BaseSize * bclim.BaseSize; i++) // for every pixel
                {
                    uint x;
                    uint y;
                    d2xy(i % 64, out x, out y);
                    uint tile = i / 64;

                    // Shift Tile Coordinate into Tilemap
                    x += (uint)(tile % p) * 8;
                    y += (uint)(tile / p) * 8;

                    byte val = br.ReadByte();
                    if (colors <= 0x10) // Handle 2 pixels at a time
                    {
                        img.SetPixel((int)x, (int)y, ca[val >> 4]);
                        x++; i++; val &= 0xF;
                        img.SetPixel((int)x, (int)y, ca[val]);
                    }
                    else //1bpp instead of .5, handle 2 pixels at a time the same way for no reason
                    {
                        img.SetPixel((int)x, (int)y, ca[val]);
                        x++; i++; val = br.ReadByte();
                        img.SetPixel((int)x, (int)y, ca[val]);
                    }
                }
            }
            return img;
        }
        internal static Bitmap getIMG_ETC(CLIM bclim)
        {
            int format = bclim.FileFormat;
            if (format != 10 && format != 11)
                return null;

            byte[] data = ETC1_Decompress(bclim.Data, bclim.Width, bclim.Height, format + 2);
            Bitmap img = new Bitmap(bclim.Width, bclim.Height);

            for (int i = 0; i < bclim.Width * bclim.Height; i++) // for every pixel
            {
                Color c = DecodeColor(BitConverter.ToUInt32(data, i * 4), 9);
                int x = i % bclim.Width;
                int y = i / bclim.Width;
                img.SetPixel(x, y, c);
            }
            return img;
        }

        // BCLIM Data Writing
        internal static int write16BitColorPalette(Bitmap img, ref MemoryStream ms)
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
                int w = gcm(img.Width, 8);
                int h = gcm(img.Height, 8);
                    w = Math.Max(nlpo2(w), nlpo2(h));
                    h = w;
                byte[] pixelarray = new Byte[w * h];

                const int colorformat = 2;
                int ctr = 1;

                pcs[0] = Color.FromArgb(0, 0xFF, 0xFF, 0xFF);

                int p = gcm(w, 8) / 8;
                if (p == 0) p = 1;
                int d = 0;
                for (uint i = 0; i < pixelarray.Length; i++)
                {
                    d = (int)(i / div);
                    // Get Tile Coordinate
                    uint x;
                    uint y;
                    d2xy(i % 64, out x, out y);

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
                    pixelarray[i / div] |= (byte)(index);
                    i++;
                }

                // Write Intro
                bz.Write((ushort)colorformat); bz.Write((ushort)ctr);
                // Write Colors
                for (int i = 0; i < ctr; i++)
                    bz.Write(GetRGBA5551(pcs[i]));      // Write byte array.
                // Write Pixel Data
                for (uint i = 0; i < d; i++)
                    bz.Write(pixelarray[i]);
                // Write Padding
                while (pixelcolors.Length < nlpo2((int)pixelcolors.Length))
                    bz.Write((byte)0);
                // Copy to main CLIM.
                pixelcolors.Position = 0; pixelcolors.CopyTo(ms);
            }
            return 7;
        }
        internal static void writeGeneric(int format, Bitmap img, ref MemoryStream ms)
        {
            int w = img.Width;
            int h = img.Height; 
            
            // square format. Yay.
            w = h = Math.Max(nlpo2(w),nlpo2(h));
            if (format == 0 && Math.Min(img.Width, img.Height) > 64)
            {
                w = nlpo2(img.Width);
                h = nlpo2(img.Height);
            }
            using (MemoryStream mz = new MemoryStream())
            using (BinaryWriter bz = new BinaryWriter(mz))
            {
                int p = gcm(w, 8) / 8;
                if (p == 0) p = 1;
                for (uint i = 0; i < w * h; i++)
                {
                    uint x;
                    uint y;
                    d2xy(i % 64, out x, out y);

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
                        case 0: bz.Write(GetL8(c)); break;                // L8
                        case 1: bz.Write(GetA8(c)); break;                // A8
                        case 2: bz.Write(GetLA4(c)); break;               // LA4(4)
                        case 3: bz.Write(GetLA8(c)); break;             // LA8(8)
                        case 4: bz.Write(GetHILO8(c)); break;           // HILO8
                        case 5: bz.Write(GetRGB565(c)); break;          // RGB565
                        case 6:
                            {
                                bz.Write(c.B); 
                                bz.Write(c.G); 
                                bz.Write(c.R); break;
                            }
                        case 7: bz.Write(GetRGBA5551(c)); break;        // RGBA5551
                        case 8: bz.Write(GetRGBA4444(c)); break;        // RGBA4444
                        case 9: bz.Write(GetRGBA8888(c)); break;          // RGBA8
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
                while (mz.Length < nlpo2((int)mz.Length)) // pad
                    bz.Write((byte)0);
                mz.Position = 0;
                mz.CopyTo(ms);
            }            
        }
        internal static int getColorCount(Bitmap img)
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


        internal static int[] Convert5To8 = { 0x00,0x08,0x10,0x18,0x20,0x29,0x31,0x39,
                                              0x41,0x4A,0x52,0x5A,0x62,0x6A,0x73,0x7B,
                                              0x83,0x8B,0x94,0x9C,0xA4,0xAC,0xB4,0xBD,
                                              0xC5,0xCD,0xD5,0xDE,0xE6,0xEE,0xF6,0xFF };

        private static Color DecodeColor(uint val, int format)
        {
            int alpha = 0xFF, red, green, blue;
            switch (format)
            {
                case 0: // L8
                    return Color.FromArgb(alpha, (byte)val, (byte)val, (byte)val);
                case 1: // A8
                    return Color.FromArgb((byte)val, alpha, alpha, alpha);
                case 2: // LA4
                    red = (byte)(val >> 4);
                    alpha = (byte)(val & 0x0F);
                    return Color.FromArgb(alpha, red, red, red);
                case 3: // LA8
                    red = (byte)((val >> 8 & 0xFF));
                    alpha = (byte)(val & 0xFF);
                    return Color.FromArgb(alpha, red, red, red);
                case 4: // HILO8
                    red = (byte)(val >> 8);
                    green = (byte)(val & 0xFF);
                    return Color.FromArgb(alpha, red, green, 0xFF);
                case 5: // RGB565
                    red = Convert5To8[(val >> 11) & 0x1F];
                    green = (byte)(((val >> 5) & 0x3F) * 4);
                    blue = Convert5To8[val & 0x1F];
                    return Color.FromArgb(alpha, red, green, blue);
                case 6: // RGB8
                    red = (byte)((val >> 16) & 0xFF);
                    green = (byte)((val >> 8) & 0xFF);
                    blue = (byte)(val & 0xFF);
                    return Color.FromArgb(alpha, red, green, blue);
                case 7: // RGBA5551
                    red = Convert5To8[(val >> 11) & 0x1F];
                    green = Convert5To8[(val >> 6) & 0x1F];
                    blue = Convert5To8[(val >> 1) & 0x1F];
                    alpha = (val & 0x0001) == 1 ? 0xFF : 0x00;
                    return Color.FromArgb(alpha, red, green, blue);
                case 8: // RGBA4444
                    alpha = (byte)(0x11 * (val & 0xf));
                    red = (byte)(0x11 * ((val >> 12) & 0xf));
                    green = (byte)(0x11 * ((val >> 8) & 0xf));
                    blue = (byte)(0x11 * ((val >> 4) & 0xf));
                    return Color.FromArgb(alpha, red, green, blue);
                case 9: // RGBA8888
                    red = (byte)((val >> 24) & 0xFF);
                    green = (byte)((val >> 16) & 0xFF);
                    blue = (byte)((val >> 8) & 0xFF);
                    alpha = (byte)(val & 0xFF);
                    return Color.FromArgb(alpha, red, green, blue);
                // case 10:
                // case 11:
                case 12: // L4
                    return Color.FromArgb(alpha, (byte)(val * 0x11), (byte)(val * 0x11), (byte)(val * 0x11));
                case 13: // A4
                    return Color.FromArgb((byte)(val * 0x11), alpha, alpha, alpha);
                default:
                    return Color.White;
            }
        }

        // Color Conversion
        internal static byte GetL8(Color c)
        {
            byte red = c.R;
            byte green = c.G;
            byte blue = c.B;
            // Luma (Y’) = 0.299 R’ + 0.587 G’ + 0.114 B’ from wikipedia
            return (byte)(((0x4CB2 * red + 0x9691 * green + 0x1D3E * blue) >> 16) & 0xFF);
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
            return (ushort)((c.A) + ((c.R) << 8));
        }     // LA8
        internal static ushort GetHILO8(Color c)
        {
            return (ushort)((c.G) + ((c.R) << 8));
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
            val += (c.A / 0x11);
            val += ((c.B / 0x11) << 4);
            val += ((c.G / 0x11) << 8);
            val += ((c.R / 0x11) << 12);
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
        internal static UInt32 DM2X(UInt32 code)
        {
            return C11(code >> 0);
        }
        internal static UInt32 DM2Y(UInt32 code)
        {
            return C11(code >> 1);
        }
        internal static UInt32 C11(UInt32 x)
        {
            x &= 0x55555555;                  // x = -f-e -d-c -b-a -9-8 -7-6 -5-4 -3-2 -1-0
            x = (x ^ (x >> 1)) & 0x33333333; // x = --fe --dc --ba --98 --76 --54 --32 --10
            x = (x ^ (x >> 2)) & 0x0f0f0f0f; // x = ---- fedc ---- ba98 ---- 7654 ---- 3210
            x = (x ^ (x >> 4)) & 0x00ff00ff; // x = ---- ---- fedc ba98 ---- ---- 7654 3210
            x = (x ^ (x >> 8)) & 0x0000ffff; // x = ---- ---- ---- ---- fedc ba98 7654 3210
            return x;
        }
        
        /// <summary>
        /// Greatest common multiple (to round up)
        /// </summary>
        /// <param name="n">Number to round-up.</param>
        /// <param name="m">Multiple to round-up to.</param>
        /// <returns>Rounded up number.</returns>
        internal static int gcm(int n, int m)
        {
            return ((n + m - 1) / m) * m;
        }
        /// <summary>
        /// Next Largest Power of 2
        /// </summary>
        /// <param name="x">Input to round up to next 2^n</param>
        /// <returns>2^n > x && x > 2^(n-1) </returns>
        internal static int nlpo2(int x)
        {
            x--; // comment out to always take the next biggest power of two, even if x is already a power of two
            x |= (x >> 1);
            x |= (x >> 2);
            x |= (x >> 4);
            x |= (x >> 8);
            x |= (x >> 16);
            return (x+1);
        }

        // Morton Translation
        /// <summary>
        /// Combines X/Y Coordinates to a decimal ordinate.
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        internal static uint xy2d(uint x, uint y)
        {
	        x &= 0x0000ffff;
	        y &= 0x0000ffff;
	        x |= (x << 8);
	        y |= (y << 8);
	        x &= 0x00ff00ff;
	        y &= 0x00ff00ff;
	        x |= (x << 4);
	        y |= (y << 4);
	        x &= 0x0f0f0f0f;
	        y &= 0x0f0f0f0f;
	        x |= (x << 2);
	        y |= (y << 2);
	        x &= 0x33333333;
	        y &= 0x33333333;
	        x |= (x << 1);
	        y |= (y << 1);
	        x &= 0x55555555;
	        y &= 0x55555555;
	        return x | (y << 1);
        }
        /// <summary>
    /// Decimal Ordinate In to X / Y Coordinate Out
    /// </summary>
    /// <param name="d">Loop integer which will be decoded to X/Y</param>
    /// <param name="x">Output X coordinate</param>
    /// <param name="y">Output Y coordinate</param>
        internal static void d2xy(uint d, out uint x, out uint y)
        {
	        x = d;
	        y = (x >> 1);
	        x &= 0x55555555;
	        y &= 0x55555555;
	        x |= (x >> 1);
	        y |= (y >> 1);
	        x &= 0x33333333;
	        y &= 0x33333333;
	        x |= (x >> 2);
	        y |= (y >> 2);
	        x &= 0x0f0f0f0f;
	        y &= 0x0f0f0f0f;
	        x |= (x >> 4);
	        y |= (y >> 4);
	        x &= 0x00ff00ff;
	        y &= 0x00ff00ff;
	        x |= (x >> 8);
	        y |= (y >> 8);
	        x &= 0x0000ffff;
	        y &= 0x0000ffff;
        }

        public class BCLIM
        {
            public static CLIM analyze(string path)
            {
                CLIM bclim = new CLIM
                {
                    FileName = Path.GetFileNameWithoutExtension(path),
                    FilePath = Path.GetDirectoryName(path),
                    Extension = Path.GetExtension(path)
                };
                byte[] byteArray = File.ReadAllBytes(path);
                using (BinaryReader br = new BinaryReader(new MemoryStream(byteArray)))
                {
                    br.BaseStream.Seek(br.BaseStream.Length - 0x28, SeekOrigin.Begin);
                    bclim.Magic = br.ReadUInt32();

                    bclim.BOM = br.ReadUInt16();
                    bclim.CLIMLength = br.ReadUInt32();
                    bclim.TileWidth = 2 << br.ReadByte();
                    bclim.TileHeight = 2 << br.ReadByte();
                    bclim.totalLength = br.ReadUInt32();
                    bclim.Count = br.ReadUInt32();

                    bclim.imag = br.ReadChars(4);
                    bclim.imagLength = br.ReadUInt32();
                    bclim.Width = br.ReadUInt16();
                    bclim.Height = br.ReadUInt16();
                    bclim.FileFormat = br.ReadInt32();
                    bclim.dataLength = br.ReadUInt32();

                    bclim.BaseSize = Math.Max(nlpo2(bclim.Width), nlpo2(bclim.Height));

                    br.BaseStream.Seek(0, SeekOrigin.Begin);
                    bclim.Data = br.ReadBytes((int)bclim.dataLength);

                    return bclim;
                }
            }
        }
        public struct CLIM
        {
            public UInt32 Magic;        // CLIM = 0x4D494C43
            public UInt16 BOM;          // 0xFFFE
            public UInt32 CLIMLength;   // HeaderLength - 14
            public int TileWidth;       // 1<<[[n]]
            public int TileHeight;      // 1<<[[n]]
            public UInt32 totalLength;  // Total Length of file
            public UInt32 Count;        // "1" , guessing it's just Count.

            public char[] imag;         // imag = 0x67616D69
            public UInt32 imagLength;   // HeaderLength - 10
            public UInt16 Width;        // Final Dimensions
            public UInt16 Height;       // Final Dimensions
            public Int32 FileFormat;    // ??
            public UInt32 dataLength;   // Pixel Data Region Length

            public byte[] Data;

            public Int32 BaseSize;

            //// Contained Data
            //public int ColorFormat;
            //public int ColorCount;
            //public Color[] Colors;

            //public int[] Pixels;

            public string FileName;
            public string FilePath;
            public string Extension;
        }

        // Ericsson Texture Compression 1 - From Ohana3DS
        // https://github.com/gdkchan/Ohana3DS
        // By gdkchan & Reisyukaku, ported to C#.
        // Porting didn't do everything right; TODO: fix.
        internal static byte[] ETC1_Decompress(byte[] Data, int Width, int Height, int Format = 12, int Offset = 0)
        {
            byte[] Temp_Buffer = new byte[((Width * Height) / 2)];
            byte[] Alphas = new byte[Temp_Buffer.Length];
            if (Format == 12)
            {
                Buffer.BlockCopy(Data, Offset, Temp_Buffer, 0, Temp_Buffer.Length);
                for (int j = 0; j <= Alphas.Length - 1; j++)
                {
                    Alphas[j] = 0xff;
                }
            }
            else
            {
                int k = 0;
                for (int j = 0; j <= (Width * Height) - 1; j++)
                {
                    Buffer.BlockCopy(Data, Offset + j + 8, Temp_Buffer, k, 8);
                    Buffer.BlockCopy(Data, Offset + j, Alphas, k, 8);
                    k += 8;
                    j += 15;
                }
            }
            byte[] Out = new byte[(Width * Height * 4)];
            Offset = 0;
            for (int Y = 0; Y <= (Height / 4) - 1; Y++)
            {
                for (int X = 0; X <= (Width / 4) - 1; X++)
                {
                    byte[] Block = new byte[8];
                    byte[] Alphas_Block = new byte[8];
                    for (int i = 0; i <= 7; i++)
                    {
                        Block[7 - i] = Data[Offset + i];
                        Alphas_Block[i] = Alphas[Offset + i];
                    }
                    Offset += 8;
                    Block = ETC1_Decompress_Block(Block);

                    bool Low_High_Toggle = false;
                    int Alpha_Offset = 0;
                    for (int TX = 0; TX <= 3; TX++)
                    {
                        for (int TY = 0; TY <= 3; TY++)
                        {
                            int Out_Offset = (X * 4 + TX + ((Y * 4 + TY) * Width)) * 4;
                            int Block_Offset = (TX + (TY * 4)) * 4;
                            Buffer.BlockCopy(Block, Block_Offset, Out, Out_Offset, 3);

                            int Alpha_Data = 0;
                            if (Low_High_Toggle)
                            {
                                Alpha_Data = (Alphas_Block[Alpha_Offset] & 0xf0) >> 4;
                                Alpha_Offset += 1;
                            }
                            else
                            {
                                Alpha_Data = Alphas_Block[Alpha_Offset] & 0xf;
                            }
                            Low_High_Toggle = !Low_High_Toggle;
                            Out[Out_Offset + 3] = Convert.ToByte((Alpha_Data << 4) + Alpha_Data);
                        }
                    }
                }
            }
            return Out;
        }
        internal static byte[] ETC1_Decompress_Block(byte[] Data)
        {
            //Ericsson Texture Compression
            int Block_Top = BitConverter.ToInt32(Data, 0);
            int Block_Bottom = BitConverter.ToInt32(Data, 4);

            bool Flip = (Block_Top & 0x1000000) > 0;
            bool Difference = (Block_Top & 0x2000000) > 0;

            int R1, G1, B1;
            int R2, G2, B2;

            if (Difference)
            {
                R1 = Block_Top & 0xf8;
                G1 = (Block_Top & 0xf800) >> 8;
                B1 = (Block_Top & 0xf80000) >> 16;

                int R = Signed_Byte(Convert.ToByte(R1 >> 3)) + (Signed_Byte(Convert.ToByte((Block_Top & 7) << 5)) >> 5);
                int G = Signed_Byte(Convert.ToByte(G1 >> 3)) + (Signed_Byte(Convert.ToByte((Block_Top & 0x700) >> 3)) >> 5);
                int B = Signed_Byte(Convert.ToByte(B1 >> 3)) + (Signed_Byte(Convert.ToByte((Block_Top & 0x70000) >> 11)) >> 5);

                R2 = R;
                G2 = G;
                B2 = B;

                R1 = R1 + (R1 >> 5);
                G1 = G1 + (G1 >> 5);
                B1 = B1 + (B1 >> 5);

                R2 = (R2 << 3) + (R2 >> 2);
                G2 = (G2 << 3) + (G2 >> 2);
                B2 = (B2 << 3) + (B2 >> 2);
            }
            else
            {
                R1 = Block_Top & 0xf0;
                R1 = R1 + (R1 >> 4);
                G1 = (Block_Top & 0xf000) >> 8;
                G1 = G1 + (G1 >> 4);
                B1 = (Block_Top & 0xf00000) >> 16;
                B1 = B1 + (B1 >> 4);

                R2 = (Block_Top & 0xf) << 4;
                R2 = R2 + (R2 >> 4);
                G2 = (Block_Top & 0xf00) >> 4;
                G2 = G2 + (G2 >> 4);
                B2 = (Block_Top & 0xf0000) >> 12;
                B2 = B2 + (B2 >> 4);
            }

            int Mod_Table_1 = (Block_Top >> 29) & 7;
            int Mod_Table_2 = (Block_Top >> 26) & 7;

            byte[] Out = new byte[(4 * 4 * 4)];
            if (Flip == false)
            {
                for (int Y = 0; Y <= 3; Y++)
                {
                    for (int X = 0; X <= 1; X++)
                    {
                        Color Col_1 = Modify_Pixel(R1, G1, B1, X, Y, Block_Bottom, Mod_Table_1);
                        Color Col_2 = Modify_Pixel(R2, G2, B2, X + 2, Y, Block_Bottom, Mod_Table_2);
                        Out[(Y * 4 + X) * 4] = Col_1.R;
                        Out[((Y * 4 + X) * 4) + 1] = Col_1.G;
                        Out[((Y * 4 + X) * 4) + 2] = Col_1.B;
                        Out[(Y * 4 + X + 2) * 4] = Col_2.R;
                        Out[((Y * 4 + X + 2) * 4) + 1] = Col_2.G;
                        Out[((Y * 4 + X + 2) * 4) + 2] = Col_2.B;
                    }
                }
            }
            else
            {
                for (int Y = 0; Y <= 1; Y++)
                {
                    for (int X = 0; X <= 3; X++)
                    {
                        Color Col_1 = Modify_Pixel(R1, G1, B1, X, Y, Block_Bottom, Mod_Table_1);
                        Color Col_2 = Modify_Pixel(R2, G2, B2, X, Y + 2, Block_Bottom, Mod_Table_2);
                        Out[(Y * 4 + X) * 4] = Col_1.R;
                        Out[((Y * 4 + X) * 4) + 1] = Col_1.G;
                        Out[((Y * 4 + X) * 4) + 2] = Col_1.B;
                        Out[((Y + 2) * 4 + X) * 4] = Col_2.R;
                        Out[(((Y + 2) * 4 + X) * 4) + 1] = Col_2.G;
                        Out[(((Y + 2) * 4 + X) * 4) + 2] = Col_2.B;
                    }
                }
            }

            return Out;
        }
        internal static byte[] ETC1_Compress(Bitmap Img, int Width, int Height, int Format = 12, bool Show_Warning = true)
        {
            if ((Img.Width != Width) | (Img.Height != Height))
            {
                if (Show_Warning)
                    MessageBox.Show("Images need to have the same resolution!", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return null;
            }

            BitmapData ImgData = Img.LockBits(new Rectangle(0, 0, Img.Width, Img.Height), ImageLockMode.ReadOnly, Img.PixelFormat);
            byte[] Data = new byte[(ImgData.Height * ImgData.Stride)];
            Marshal.Copy(ImgData.Scan0, Data, 0, Data.Length);
            Img.UnlockBits(ImgData);

            int BPP = (Img.PixelFormat == PixelFormat.Format32bppArgb) ? 32 : 24;
            byte[] Out_Data = null;

            #region Conversion
            switch (Format)
            {
                case 12:
                case 13:
                    //Os tiles com compressão ETC1 no 3DS estão embaralhados
                    byte[] Out = new byte[(Img.Width * Img.Height * 4)];
                    int[] Tile_Scramble = Get_ETC1_Scramble(Img.Width, Img.Height);

                    int i = 0;
                    for (int Tile_Y = 0; Tile_Y <= (Img.Height / 4) - 1; Tile_Y++)
                    {
                        for (int Tile_X = 0; Tile_X <= (Img.Width / 4) - 1; Tile_X++)
                        {
                            int TX = Tile_Scramble[i] % (Img.Width / 4);
                            int TY = (Tile_Scramble[i] - TX) / (Img.Width / 4);
                            for (int Y = 0; Y <= 3; Y++)
                            {
                                for (int X = 0; X <= 3; X++)
                                {
                                    int Out_Offset = ((TX * 4) + X + ((((TY * 4) + Y)) * Img.Width)) * 4;
                                    int Image_Offset = ((Tile_X * 4) + X + (((Tile_Y * 4) + Y) * Img.Width)) * (BPP / 8);

                                    Out[Out_Offset] = Data[Image_Offset + 2];
                                    Out[Out_Offset + 1] = Data[Image_Offset + 1];
                                    Out[Out_Offset + 2] = Data[Image_Offset];
                                    if (BPP == 32)
                                        Out[Out_Offset + 3] = Data[Image_Offset + 3];
                                    else
                                        Out[Out_Offset + 3] = 0xff;
                                }
                            }
                            i += 1;
                        }
                    }


                    Out_Data = new byte[((Img.Width * Img.Height) / Format == 12 ? 2 : 1)];
                    int Out_Data_Offset = 0;

                    for (int Tile_Y = 0; Tile_Y <= (Img.Height / 4) - 1; Tile_Y++)
                    {
                        for (int Tile_X = 0; Tile_X <= (Img.Width / 4) - 1; Tile_X++)
                        {
                            bool Flip = false;
                            bool Difference = false;
                            int Block_Top = 0;
                            int Block_Bottom = 0;

                            //Teste do Difference Bit
                            int Diff_Match_V = 0;
                            int Diff_Match_H = 0;
                            for (int Y = 0; Y <= 3; Y++)
                            {
                                for (int X = 0; X <= 1; X++)
                                {
                                    int Image_Offset_1 = ((Tile_X * 4) + X + (((Tile_Y * 4) + Y) * Img.Width)) * 4;
                                    int Image_Offset_2 = ((Tile_X * 4) + (2 + X) + (((Tile_Y * 4) + Y) * Img.Width)) * 4;

                                    byte Bits_R1 = Convert.ToByte(Out[Image_Offset_1] & 0xf8);
                                    byte Bits_G1 = Convert.ToByte(Out[Image_Offset_1 + 1] & 0xf8);
                                    byte Bits_B1 = Convert.ToByte(Out[Image_Offset_1 + 2] & 0xf8);

                                    byte Bits_R2 = Convert.ToByte(Out[Image_Offset_2] & 0xf8);
                                    byte Bits_G2 = Convert.ToByte(Out[Image_Offset_2 + 1] & 0xf8);
                                    byte Bits_B2 = Convert.ToByte(Out[Image_Offset_2 + 2] & 0xf8);

                                    if ((Bits_R1 == Bits_R2) & (Bits_G1 == Bits_G2) & (Bits_B1 == Bits_B2))
                                        Diff_Match_V += 1;
                                }
                            }
                            for (int Y = 0; Y <= 1; Y++)
                            {
                                for (int X = 0; X <= 3; X++)
                                {
                                    int Image_Offset_1 = ((Tile_X * 4) + X + (((Tile_Y * 4) + Y) * Img.Width)) * 4;
                                    int Image_Offset_2 = ((Tile_X * 4) + X + (((Tile_Y * 4) + (2 + Y)) * Img.Width)) * 4;

                                    byte Bits_R1 = Convert.ToByte(Out[Image_Offset_1] & 0xf8);
                                    byte Bits_G1 = Convert.ToByte(Out[Image_Offset_1 + 1] & 0xf8);
                                    byte Bits_B1 = Convert.ToByte(Out[Image_Offset_1 + 2] & 0xf8);

                                    byte Bits_R2 = Convert.ToByte(Out[Image_Offset_2] & 0xf8);
                                    byte Bits_G2 = Convert.ToByte(Out[Image_Offset_2 + 1] & 0xf8);
                                    byte Bits_B2 = Convert.ToByte(Out[Image_Offset_2 + 2] & 0xf8);

                                    if ((Bits_R1 == Bits_R2) & (Bits_G1 == Bits_G2) & (Bits_B1 == Bits_B2))
                                        Diff_Match_H += 1;
                                }
                            }
                            //Difference + Flip
                            if (Diff_Match_H == 8)
                            {
                                Difference = true;
                                Flip = true;
                                //Difference
                            }
                            else if (Diff_Match_V == 8)
                            {
                                Difference = true;
                                //Individual
                            }
                            else
                            {
                                int Test_R1 = 0;
                                int Test_G1 = 0;
                                int Test_B1 = 0;
                                int Test_R2 = 0;
                                int Test_G2 = 0;
                                int Test_B2 = 0;
                                for (int Y = 0; Y <= 1; Y++)
                                {
                                    for (int X = 0; X <= 1; X++)
                                    {
                                        int Image_Offset_1 = ((Tile_X * 4) + X + (((Tile_Y * 4) + Y) * Img.Width)) * 4;
                                        int Image_Offset_2 = ((Tile_X * 4) + (2 + X) + (((Tile_Y * 4) + (2 + Y)) * Img.Width)) * 4;

                                        Test_R1 += Out[Image_Offset_1];
                                        Test_G1 += Out[Image_Offset_1 + 1];
                                        Test_B1 += Out[Image_Offset_1 + 2];

                                        Test_R2 += Out[Image_Offset_2];
                                        Test_G2 += Out[Image_Offset_2 + 1];
                                        Test_B2 += Out[Image_Offset_2 + 2];
                                    }
                                }

                                Test_R1 /= 8;
                                Test_G1 /= 8;
                                Test_B1 /= 8;

                                Test_R2 /= 8;
                                Test_G2 /= 8;
                                Test_B2 /= 8;

                                int Test_Luma_1 = Convert.ToInt32(0.299f * Test_R1 + 0.587f * Test_G1 + 0.114f * Test_B1);
                                int Test_Luma_2 = Convert.ToInt32(0.299f * Test_R2 + 0.587f * Test_G2 + 0.114f * Test_B2);
                                int Test_Flip_Diff = Math.Abs(Test_Luma_1 - Test_Luma_2);
                                if (Test_Flip_Diff > 48)
                                    Flip = true;
                            }

                            int Avg_R1 = 0;
                            int Avg_G1 = 0;
                            int Avg_B1 = 0;
                            int Avg_R2 = 0;
                            int Avg_G2 = 0;
                            int Avg_B2 = 0;

                            //Primeiro, cálcula a média de cores de cada bloco
                            if (Flip)
                            {
                                for (int Y = 0; Y <= 1; Y++)
                                {
                                    for (int X = 0; X <= 3; X++)
                                    {
                                        int Image_Offset_1 = ((Tile_X * 4) + X + (((Tile_Y * 4) + Y) * Img.Width)) * 4;
                                        int Image_Offset_2 = ((Tile_X * 4) + X + (((Tile_Y * 4) + (2 + Y)) * Img.Width)) * 4;

                                        Avg_R1 += Out[Image_Offset_1];
                                        Avg_G1 += Out[Image_Offset_1 + 1];
                                        Avg_B1 += Out[Image_Offset_1 + 2];

                                        Avg_R2 += Out[Image_Offset_2];
                                        Avg_G2 += Out[Image_Offset_2 + 1];
                                        Avg_B2 += Out[Image_Offset_2 + 2];
                                    }
                                }
                            }
                            else
                            {
                                for (int Y = 0; Y <= 3; Y++)
                                {
                                    for (int X = 0; X <= 1; X++)
                                    {
                                        int Image_Offset_1 = ((Tile_X * 4) + X + (((Tile_Y * 4) + Y) * Img.Width)) * 4;
                                        int Image_Offset_2 = ((Tile_X * 4) + (2 + X) + (((Tile_Y * 4) + Y) * Img.Width)) * 4;

                                        Avg_R1 += Out[Image_Offset_1];
                                        Avg_G1 += Out[Image_Offset_1 + 1];
                                        Avg_B1 += Out[Image_Offset_1 + 2];

                                        Avg_R2 += Out[Image_Offset_2];
                                        Avg_G2 += Out[Image_Offset_2 + 1];
                                        Avg_B2 += Out[Image_Offset_2 + 2];
                                    }
                                }
                            }

                            Avg_R1 /= 8;
                            Avg_G1 /= 8;
                            Avg_B1 /= 8;

                            Avg_R2 /= 8;
                            Avg_G2 /= 8;
                            Avg_B2 /= 8;

                            if (Difference)
                            {
                                //+============+
                                //| Difference |
                                //+============+
                                if ((Avg_R1 & 7) > 3) { Avg_R1 = Clip(Avg_R1 + 8); Avg_R2 = Clip(Avg_R2 + 8); }
                                if ((Avg_G1 & 7) > 3) { Avg_G1 = Clip(Avg_G1 + 8); Avg_G2 = Clip(Avg_G2 + 8); }
                                if ((Avg_B1 & 7) > 3) { Avg_B1 = Clip(Avg_B1 + 8); Avg_B2 = Clip(Avg_B2 + 8); }

                                Block_Top = (Avg_R1 & 0xf8) | (((Avg_R2 - Avg_R1) / 8) & 7);
                                Block_Top = Block_Top | (((Avg_G1 & 0xf8) << 8) | ((((Avg_G2 - Avg_G1) / 8) & 7) << 8));
                                Block_Top = Block_Top | (((Avg_B1 & 0xf8) << 16) | ((((Avg_B2 - Avg_B1) / 8) & 7) << 16));

                                //Vamos ter certeza de que os mesmos valores obtidos pelo descompressor serão usados na comparação (modo Difference)
                                Avg_R1 = Block_Top & 0xf8;
                                Avg_G1 = (Block_Top & 0xf800) >> 8;
                                Avg_B1 = (Block_Top & 0xf80000) >> 16;

                                int R = Signed_Byte(Convert.ToByte(Avg_R1 >> 3)) + (Signed_Byte(Convert.ToByte((Block_Top & 7) << 5)) >> 5);
                                int G = Signed_Byte(Convert.ToByte(Avg_G1 >> 3)) + (Signed_Byte(Convert.ToByte((Block_Top & 0x700) >> 3)) >> 5);
                                int B = Signed_Byte(Convert.ToByte(Avg_B1 >> 3)) + (Signed_Byte(Convert.ToByte((Block_Top & 0x70000) >> 11)) >> 5);

                                Avg_R2 = R;
                                Avg_G2 = G;
                                Avg_B2 = B;

                                Avg_R1 = Avg_R1 + (Avg_R1 >> 5);
                                Avg_G1 = Avg_G1 + (Avg_G1 >> 5);
                                Avg_B1 = Avg_B1 + (Avg_B1 >> 5);

                                Avg_R2 = (Avg_R2 << 3) + (Avg_R2 >> 2);
                                Avg_G2 = (Avg_G2 << 3) + (Avg_G2 >> 2);
                                Avg_B2 = (Avg_B2 << 3) + (Avg_B2 >> 2);
                            }
                            else
                            {
                                //+============+
                                //| Individual |
                                //+============+
                                if ((Avg_R1 & 0xf) > 7)
                                    Avg_R1 = Clip(Avg_R1 + 0x10);
                                if ((Avg_G1 & 0xf) > 7)
                                    Avg_G1 = Clip(Avg_G1 + 0x10);
                                if ((Avg_B1 & 0xf) > 7)
                                    Avg_B1 = Clip(Avg_B1 + 0x10);
                                if ((Avg_R2 & 0xf) > 7)
                                    Avg_R2 = Clip(Avg_R2 + 0x10);
                                if ((Avg_G2 & 0xf) > 7)
                                    Avg_G2 = Clip(Avg_G2 + 0x10);
                                if ((Avg_B2 & 0xf) > 7)
                                    Avg_B2 = Clip(Avg_B2 + 0x10);

                                Block_Top = ((Avg_R2 & 0xf0) >> 4) | (Avg_R1 & 0xf0);
                                Block_Top = Block_Top | (((Avg_G2 & 0xf0) << 4) | ((Avg_G1 & 0xf0) << 8));
                                Block_Top = Block_Top | (((Avg_B2 & 0xf0) << 12) | ((Avg_B1 & 0xf0) << 16));

                                //Vamos ter certeza de que os mesmos valores obtidos pelo descompressor serão usados na comparação (modo Individual)
                                Avg_R1 = (Avg_R1 & 0xf0) + ((Avg_R1 & 0xf0) >> 4);
                                Avg_G1 = (Avg_G1 & 0xf0) + ((Avg_G1 & 0xf0) >> 4);
                                Avg_B1 = (Avg_B1 & 0xf0) + ((Avg_B1 & 0xf0) >> 4);

                                Avg_R2 = (Avg_R2 & 0xf0) + ((Avg_R2 & 0xf0) >> 4);
                                Avg_G2 = (Avg_G2 & 0xf0) + ((Avg_G2 & 0xf0) >> 4);
                                Avg_B2 = (Avg_B2 & 0xf0) + ((Avg_B2 & 0xf0) >> 4);
                            }

                            if (Flip)
                                Block_Top = Block_Top | 0x1000000;
                            if (Difference)
                                Block_Top = Block_Top | 0x2000000;

                            //Seleciona a melhor tabela para ser usada nos blocos
                            int Mod_Table_1 = 0;
                            int[] Min_Diff_1 = new int[8];
                            for (int a = 0; a <= 7; a++)
                            {
                                Min_Diff_1[a] = 0;
                            }
                            for (int Y = 0; Y <= (Flip ? 1 : 3); Y++)
                            {
                                for (int X = 0; X <= (Flip ? 3 : 1); X++)
                                {
                                    int Image_Offset = ((Tile_X * 4) + X + (((Tile_Y * 4) + Y) * Img.Width)) * 4;
                                    int Luma = Convert.ToInt32(0.299f * Out[Image_Offset] + 0.587f * Out[Image_Offset + 1] + 0.114f * Out[Image_Offset + 2]);

                                    for (int a = 0; a <= 7; a++)
                                    {
                                        int Optimal_Diff = 255 * 4;
                                        for (int b = 0; b <= 3; b++)
                                        {
                                            int CR = Clip(Avg_R1 + Modulation_Table[a, b]);
                                            int CG = Clip(Avg_G1 + Modulation_Table[a, b]);
                                            int CB = Clip(Avg_B1 + Modulation_Table[a, b]);

                                            int Test_Luma = Convert.ToInt32(0.299f * CR + 0.587f * CG + 0.114f * CB);
                                            int Diff = Math.Abs(Luma - Test_Luma);
                                            if (Diff < Optimal_Diff)
                                                Optimal_Diff = Diff;
                                        }
                                        Min_Diff_1[a] += Optimal_Diff;
                                    }
                                }
                            }

                            int Temp_1 = 255 * 8;
                            for (int a = 0; a <= 7; a++)
                            {
                                if (Min_Diff_1[a] < Temp_1)
                                {
                                    Temp_1 = Min_Diff_1[a];
                                    Mod_Table_1 = a;
                                }
                            }

                            int Mod_Table_2 = 0;
                            int[] Min_Diff_2 = new int[8];
                            for (int a = 0; a <= 7; a++)
                            {
                                Min_Diff_2[a] = 0;
                            }
                            for (int Y = Flip ? 2 : 0; Y <= 3; Y++)
                            {
                                for (int X = Flip ? 0 : 2; X <= 3; X++)
                                {
                                    int Image_Offset = ((Tile_X * 4) + X + (((Tile_Y * 4) + Y) * Img.Width)) * 4;
                                    int Luma = Convert.ToInt32(0.299f * Out[Image_Offset] + 0.587f * Out[Image_Offset + 1] + 0.114f * Out[Image_Offset + 2]);

                                    for (int a = 0; a <= 7; a++)
                                    {
                                        int Optimal_Diff = 255 * 4;
                                        for (int b = 0; b <= 3; b++)
                                        {
                                            int CR = Clip(Avg_R2 + Modulation_Table[a, b]);
                                            int CG = Clip(Avg_G2 + Modulation_Table[a, b]);
                                            int CB = Clip(Avg_B2 + Modulation_Table[a, b]);

                                            int Test_Luma = Convert.ToInt32(0.299f * CR + 0.587f * CG + 0.114f * CB);
                                            int Diff = Math.Abs(Luma - Test_Luma);
                                            if (Diff < Optimal_Diff)
                                                Optimal_Diff = Diff;
                                        }
                                        Min_Diff_2[a] += Optimal_Diff;
                                    }
                                }
                            }

                            int Temp_2 = 255 * 8;
                            for (int a = 0; a <= 7; a++)
                            {
                                if (Min_Diff_2[a] < Temp_2)
                                {
                                    Temp_2 = Min_Diff_2[a];
                                    Mod_Table_2 = a;
                                }
                            }

                            Block_Top = Block_Top | (Mod_Table_1 << 29);
                            Block_Top = Block_Top | (Mod_Table_2 << 26);

                            //Seleciona o melhor valor da tabela que mais se aproxima com a cor original
                            for (int Y = 0; Y <= (Flip ? 1 : 3); Y++)
                            {
                                for (int X = 0; X <= (Flip ? 3 : 1); X++)
                                {
                                    int Image_Offset = ((Tile_X * 4) + X + (((Tile_Y * 4) + Y) * Img.Width)) * 4;
                                    int Luma = Convert.ToInt32(0.299f * Out[Image_Offset] + 0.587f * Out[Image_Offset + 1] + 0.114f * Out[Image_Offset + 2]);

                                    int Col_Diff = 255;
                                    int Pix_Table_Index = 0;
                                    for (int b = 0; b <= 3; b++)
                                    {
                                        int CR = Clip(Avg_R1 + Modulation_Table[Mod_Table_1, b]);
                                        int CG = Clip(Avg_G1 + Modulation_Table[Mod_Table_1, b]);
                                        int CB = Clip(Avg_B1 + Modulation_Table[Mod_Table_1, b]);

                                        int Test_Luma = Convert.ToInt32(0.299f * CR + 0.587f * CG + 0.114f * CB);
                                        int Diff = Math.Abs(Luma - Test_Luma);
                                        if (Diff < Col_Diff)
                                        {
                                            Col_Diff = Diff;
                                            Pix_Table_Index = b;
                                        }
                                    }

                                    int Index = X * 4 + Y;
                                    if (Index < 8)
                                    {
                                        Block_Bottom = Block_Bottom | (((Pix_Table_Index & 2) >> 1) << (Index + 8));
                                        Block_Bottom = Block_Bottom | ((Pix_Table_Index & 1) << (Index + 24));
                                    }
                                    else
                                    {
                                        Block_Bottom = Block_Bottom | (((Pix_Table_Index & 2) >> 1) << (Index - 8));
                                        Block_Bottom = Block_Bottom | ((Pix_Table_Index & 1) << (Index + 8));
                                    }
                                }
                            }

                            for (int Y = Flip ? 2 : 0; Y <= 3; Y++)
                            {
                                for (int X = Flip ? 0 : 2; X <= 3; X++)
                                {
                                    int Image_Offset = ((Tile_X * 4) + X + (((Tile_Y * 4) + Y) * Img.Width)) * 4;
                                    int Luma = Convert.ToInt32(0.299f * Out[Image_Offset] + 0.587f * Out[Image_Offset + 1] + 0.114f * Out[Image_Offset + 2]);

                                    int Col_Diff = 255;
                                    int Pix_Table_Index = 0;
                                    for (int b = 0; b <= 3; b++)
                                    {
                                        int CR = Clip(Avg_R2 + Modulation_Table[Mod_Table_2, b]);
                                        int CG = Clip(Avg_G2 + Modulation_Table[Mod_Table_2, b]);
                                        int CB = Clip(Avg_B2 + Modulation_Table[Mod_Table_2, b]);

                                        int Test_Luma = Convert.ToInt32(0.299f * CR + 0.587f * CG + 0.114f * CB);
                                        int Diff = Math.Abs(Luma - Test_Luma);
                                        if (Diff < Col_Diff)
                                        {
                                            Col_Diff = Diff;
                                            Pix_Table_Index = b;
                                        }
                                    }

                                    int Index = X * 4 + Y;
                                    if (Index < 8)
                                    {
                                        Block_Bottom = Block_Bottom | (((Pix_Table_Index & 2) >> 1) << (Index + 8));
                                        Block_Bottom = Block_Bottom | ((Pix_Table_Index & 1) << (Index + 24));
                                    }
                                    else
                                    {
                                        Block_Bottom = Block_Bottom | (((Pix_Table_Index & 2) >> 1) << (Index - 8));
                                        Block_Bottom = Block_Bottom | ((Pix_Table_Index & 1) << (Index + 8));
                                    }
                                }
                            }

                            //Copia dados para a saída
                            byte[] Block = new byte[8];
                            Buffer.BlockCopy(BitConverter.GetBytes(Block_Top), 0, Block, 0, 4);
                            Buffer.BlockCopy(BitConverter.GetBytes(Block_Bottom), 0, Block, 4, 4);
                            byte[] New_Block = new byte[8];
                            for (int j = 0; j <= 7; j++)
                            {
                                New_Block[7 - j] = Block[j];
                            }
                            if (Format == 13)
                            {
                                byte[] Alphas = new byte[8];
                                int Alpha_Offset = 0;
                                for (int TX = 0; TX <= 3; TX++)
                                {
                                    for (int TY = 0; TY <= 3; TY += 2)
                                    {
                                        int Img_Offset_1 = (Tile_X * 4 + TX + ((Tile_Y * 4 + TY) * Img.Width)) * 4;
                                        int Img_Offset_2 = (Tile_X * 4 + TX + ((Tile_Y * 4 + TY + 1) * Img.Width)) * 4;

                                        byte Alpha_1 = (byte)(Out[Img_Offset_1 + 3] >> 4);
                                        byte Alpha_2 = (byte)(Out[Img_Offset_2 + 3] >> 4);

                                        Alphas[Alpha_Offset] = (byte)(Alpha_1 | (Alpha_2 << 4));

                                        Alpha_Offset += 1;
                                    }
                                }

                                Buffer.BlockCopy(Alphas, 0, Out_Data, Out_Data_Offset, 8);
                                Buffer.BlockCopy(New_Block, 0, Out_Data, Out_Data_Offset + 8, 8);
                                Out_Data_Offset += 16;
                            }
                            else if (Format == 12)
                            {
                                Buffer.BlockCopy(New_Block, 0, Out_Data, Out_Data_Offset, 8);
                                Out_Data_Offset += 8;
                            }

                            // Texture_Insertion_Percentage = Convert.ToSingle((Out_Data_Offset / Out_Data.Length) * 100);
                        }
                    }
                    break;
            }
            #endregion
            // Texture_Insertion_Percentage = 0;
            return Out_Data;
        }
        #region Misc Functions
        internal static sbyte Signed_Byte(byte Byte_To_Convert)
        {
            if ((Byte_To_Convert < 0x80))
                return Convert.ToSByte(Byte_To_Convert);
            return Convert.ToSByte(Byte_To_Convert - 0x100);
        }
        internal static int Signed_Short(int Short_To_Convert)
        {
            if ((Short_To_Convert < 0x8000))
                return Short_To_Convert;
            return Short_To_Convert - 0x10000;
        }
        internal static byte[] Mirror_Texture(byte[] Data, int Width, int Height)
        {
            byte[] Out = new byte[((Width * 2) * Height * 4)];
            for (int Y = 0; Y <= Height - 1; Y++)
            {
                for (int X = 0; X <= Width - 1; X++)
                {
                    int Offset = (X + (Y * Width)) * 4;
                    int Offset_2 = (X + (Y * (Width * 2))) * 4;
                    int Offset_3 = ((Width + (Width - X - 1)) + (Y * (Width * 2))) * 4;
                    Buffer.BlockCopy(Data, Offset, Out, Offset_2, 4);
                    Buffer.BlockCopy(Data, Offset, Out, Offset_3, 4);
                }
            }
            return Out;
        }
        internal static bool Check_Alpha(byte[] Img)
        {
            for (int Offset = 0; Offset <= Img.Length - 1; Offset += 4)
            {
                if (Img[Offset + 3] < 0xff)
                    return true;
            }
            return false;
        }
        internal static int[] Get_ETC1_Scramble(int Width, int Height)
        {
            int[] Tile_Scramble = new int[((Width / 4) * (Height / 4))];
            int Base_Accumulator = 0;
            int Line_Accumulator = 0;
            int Base_Number = 0;
            int Line_Number = 0;

            for (int Tile = 0; Tile <= Tile_Scramble.Length - 1; Tile++)
            {
                if ((Tile % (Width / 4) == 0) & Tile > 0)
                {
                    if (Line_Accumulator < 1)
                    {
                        Line_Accumulator += 1;
                        Line_Number += 2;
                        Base_Number = Line_Number;
                    }
                    else
                    {
                        Line_Accumulator = 0;
                        Base_Number -= 2;
                        Line_Number = Base_Number;
                    }
                }

                Tile_Scramble[Tile] = Base_Number;

                if (Base_Accumulator < 1)
                {
                    Base_Accumulator += 1;
                    Base_Number += 1;
                }
                else
                {
                    Base_Accumulator = 0;
                    Base_Number += 3;
                }
            }

            return Tile_Scramble;
        }
        internal static Color Modify_Pixel(int R, int G, int B, int X, int Y, int Mod_Block, int Mod_Table)
        {
            int Index = X * 4 + Y;
            int Pixel_Modulation = 0;
            int MSB = Mod_Block << 1;

            Pixel_Modulation = Index < 8 
                ? Modulation_Table[Mod_Table, ((Mod_Block >> (Index + 24)) & 1) + ((MSB >> (Index + 8)) & 2)] 
                : Modulation_Table[Mod_Table, ((Mod_Block >> (Index + 8)) & 1) + ((MSB >> (Index - 8)) & 2)];

            R = Clip(R + Pixel_Modulation);
            G = Clip(G + Pixel_Modulation);
            B = Clip(B + Pixel_Modulation);

            return Color.FromArgb(B, G, R);
        }
        internal static byte Clip(int Value)
        {
            if (Value > 0xff)
                return 0xff;
            else if (Value < 0)
                return 0;
            else
                return Convert.ToByte(Value & 0xff);
        }
        internal static int[,] Modulation_Table =
        {
            { 2, 8, 2, 8 },
            { 5, 17, 5, 17 },
            { 9, 29, 9, 29 },
            { 13, 42, 13, 42 },
            { 18, 60, 18, 60 },
            { 24, 80, 24, 80 },
            { 33, 106, 33, -106 },
            { 47, 183, 47, 183 }
        };
        #endregion
    }
}
