using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Runtime.InteropServices;
using pk3DS.Properties; // For other projects, replace this line with the namespace that contains your ETC1Lib.dll resource.

namespace CTR
{
    class BCLIM
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

        internal static byte[] IMGToBCLIM(Image img, char fc)
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
        internal static byte[] getBCLIM(string path, char fc)
        {
            byte[] byteArray = File.ReadAllBytes(path);
            using (Stream BitmapStream = new MemoryStream(byteArray)) // Open the file, even if it is in use.
            {
                Image img = Image.FromStream(BitmapStream);
                return IMGToBCLIM(img, fc);
            }
        }
        internal static Image makeBCLIM(string path, char fc)
        {
            byte[] bclim = getBCLIM(path, fc);
            string fp = Path.GetFileNameWithoutExtension(path);
            fp = "new_" + fp.Substring(fp.IndexOf('_') + 1);
            string pp = Path.GetDirectoryName(path);
            string newPath = Path.Combine(pp, fp + ".bclim");
            File.WriteAllBytes(newPath, bclim);

            return makeBMP(newPath);
        }
        internal static Image makeBMP(string path, bool autosave = false, bool crop = true)
        {
            CLIM bclim = analyze(path);
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

            if (img == null) return null;
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
        internal static Bitmap getIMG(int width, int height, byte[] bytes, int f)
        {
            Bitmap img = new Bitmap(width, height);
            int area = img.Width * img.Height;
            // Tiles Per Width
            int p = gcm(img.Width, 8) / 8;
            if (p == 0) p = 1;
            using (Stream BitmapStream = new MemoryStream(bytes))
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
        internal static Bitmap getIMG(CLIM bclim)
        {
            if (bclim.FileFormat == 7 && BitConverter.ToUInt16(bclim.Data, 0) == 2) // XY7
                return getIMG_XY7(bclim);
            if (bclim.FileFormat == 10 || bclim.FileFormat == 11) // Use ETC1 to get image instead.
                return getIMG_ETC(bclim);
            // New Image
            int w = nlpo2(gcm(bclim.Width, 8));
            int h = nlpo2(gcm(bclim.Height, 8));
            int f = bclim.FileFormat;
            int area = w * h;
            if (f == 9 && area > bclim.Data.Length / 4)
            {
                w = gcm(bclim.Width, 8);
                h = gcm(bclim.Height, 8);
            }
            // Build Image
            return getIMG(w, h, bclim.Data, f);
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
            Bitmap img = new Bitmap(Math.Max(nlpo2(bclim.Width), 16), Math.Max(nlpo2(bclim.Height), 16));
            string dllpath = Path.GetDirectoryName(System.Reflection.Assembly.GetEntryAssembly().Location) + "\\ETC1Lib.dll";
            if (!File.Exists(dllpath)) File.WriteAllBytes(dllpath, Resources.ETC1Lib);

            try
            {
                /* http://jul.rustedlogic.net/thread.php?id=17312
                 * Much of this code is taken/modified from Tharsis. Thank you to Tharsis's creator, xdaniel.
                 * https://github.com/xdanieldzd/Tharsis
                 */

                /* Get compressed data & handle to it */
                byte[] textureData = bclim.Data;
                //textureData = switchEndianness(textureData, 0x10);
                ushort[] input = new ushort[textureData.Length/sizeof (ushort)];
                Buffer.BlockCopy(textureData, 0, input, 0, textureData.Length);
                GCHandle pInput = GCHandle.Alloc(input, GCHandleType.Pinned);

                /* Marshal data around, invoke ETC1.dll for conversion, etc */
                uint size1 = 0;
                UInt16 w = (ushort)img.Width, h = (ushort)img.Height;

                ETC1.ConvertETC1(IntPtr.Zero, ref size1, IntPtr.Zero, w, h, bclim.FileFormat == 0xB); // true = etc1a4, false = etc1
                // System.Diagnostics.Debug.WriteLine(size1);
                uint[] output = new uint[size1];
                GCHandle pOutput = GCHandle.Alloc(output, GCHandleType.Pinned);
                ETC1.ConvertETC1(pOutput.AddrOfPinnedObject(), ref size1, pInput.AddrOfPinnedObject(), w, h, bclim.FileFormat == 0xB);
                pOutput.Free();
                pInput.Free();


                /* Unscramble if needed // could probably be done in ETC1Lib.dll, it's probably pretty ugly, but whatever... */
                /* Non-square code blocks could need some cleanup, verification, etc. as well... */
                uint[] finalized = new uint[output.Length];

                // Act if it's square because BCLIM swizzling is stupid
                Buffer.BlockCopy(output, 0, finalized, 0, finalized.Length);

                byte[] tmp = new byte[finalized.Length];
                Buffer.BlockCopy(finalized, 0, tmp, 0, tmp.Length);
                byte[] imgData = tmp;

                for (int i = 0; i < w; i++)
                {
                    for (int j = 0; j < h; j++)
                    {
                        int k = (j + i*img.Height)*4;
                        img.SetPixel(i, j, Color.FromArgb(imgData[k + 3], imgData[k], imgData[k + 1], imgData[k + 2]));
                    }
                }
                // Image is 13  instead of 12
                //          24             34
                img.RotateFlip(RotateFlipType.Rotate90FlipX);
                if (w > h)
                {
                    // Image is now in appropriate order, but the shifting is messed up. Let's fix that.
                    Bitmap img2 = new Bitmap(Math.Max(nlpo2(bclim.Width), 16), Math.Max(nlpo2(bclim.Height), 16));
                    for (int y = 0; y < Math.Max(nlpo2(bclim.Width), 16); y += 8)
                    {
                        for (int x = 0; x < Math.Max(nlpo2(bclim.Height), 16); x++)
                        {
                            for (int j = 0; j < 8; j++)
                                // Treat every 8 vertical pixels as 1 pixel for purposes of calculation, add to offset later.
                            {
                                int x1 = (x + y/8*h)%img2.Width; // Reshift x
                                int y1 = (x + y/8*h)/img2.Width*8; // Reshift y
                                img2.SetPixel(x1, y1 + j, img.GetPixel(x, y + j)); // Reswizzle
                            }
                        }
                    }
                    img = img2;
                }
                else if (h > w)
                {
                    Bitmap img2 = new Bitmap(Math.Max(nlpo2(bclim.Width), 16), Math.Max(nlpo2(bclim.Height), 16));
                    for (int y = 0; y < Math.Max(nlpo2(bclim.Width), 16); y += 8)
                    {
                        for (int x = 0; x < Math.Max(nlpo2(bclim.Height), 16); x++)
                        {
                            for (int j = 0; j < 8; j++)
                                // Treat every 8 vertical pixels as 1 pixel for purposes of calculation, add to offset later.
                            {
                                int x1 = x%img2.Width; // Reshift x
                                int y1 = (x + y/8*h)/img2.Width*8; // Reshift y
                                img2.SetPixel(x1, y1 + j, img.GetPixel(x, y + j)); // Reswizzle
                            }
                        }
                    }
                    img = img2;
                }
            }
            catch { }
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
                    pixelarray[i / div] |= (byte)index;
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
        internal static void writeGeneric(int format, Bitmap img, ref MemoryStream ms, bool rectangle = true)
        {
            BinaryWriter bz = new BinaryWriter(ms);
            bz.Write(getPixelData(img, format, rectangle));
            bz.Flush();
        }

        internal static byte[] getPixelData(Bitmap img, int format, bool rectangle = true)
        {
            int w = img.Width;
            int h = img.Height;

            bool perfect = w == h && (w != 0) && ((w & (w - 1)) == 0);
            if (!perfect) // Check if square power of two, else resize
            {
                // Square Format Checks
                if (rectangle && Math.Min(img.Width, img.Height) < 32)
                {
                    w = nlpo2(img.Width);
                    h = nlpo2(img.Height);
                }
                else
                {
                    w = h = Math.Max(nlpo2(w), nlpo2(h)); // else resize
                }
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
                if (!perfect)
                    while (mz.Length < nlpo2((int)mz.Length)) // pad
                        bz.Write((byte)0);
                return mz.ToArray();
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
                    red = (byte)(val >> 8 & 0xFF);
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
            return (byte)(c.A / 0x11 + c.R / 0x11 << 4);
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
        internal static uint DM2X(uint code)
        {
            return C11(code >> 0);
        }
        internal static uint DM2Y(uint code)
        {
            return C11(code >> 1);
        }
        internal static uint C11(uint x)
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
            return (n + m - 1) / m * m;
        }
        /// <summary>
        /// Next Largest Power of 2
        /// </summary>
        /// <param name="x">Input to round up to next 2^n</param>
        /// <returns>2^n > x && x > 2^(n-1) </returns>
        internal static int nlpo2(int x)
        {
            x--; // comment out to always take the next biggest power of two, even if x is already a power of two
            x |= x >> 1;
            x |= x >> 2;
            x |= x >> 4;
            x |= x >> 8;
            x |= x >> 16;
            return x+1;
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
	        x |= x << 8;
	        y |= y << 8;
	        x &= 0x00ff00ff;
	        y &= 0x00ff00ff;
	        x |= x << 4;
	        y |= y << 4;
	        x &= 0x0f0f0f0f;
	        y &= 0x0f0f0f0f;
	        x |= x << 2;
	        y |= y << 2;
	        x &= 0x33333333;
	        y &= 0x33333333;
	        x |= x << 1;
	        y |= y << 1;
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
	        y = x >> 1;
	        x &= 0x55555555;
	        y &= 0x55555555;
	        x |= x >> 1;
	        y |= y >> 1;
	        x &= 0x33333333;
	        y &= 0x33333333;
	        x |= x >> 2;
	        y |= y >> 2;
	        x &= 0x0f0f0f0f;
	        y &= 0x0f0f0f0f;
	        x |= x >> 4;
	        y |= y >> 4;
	        x &= 0x00ff00ff;
	        y &= 0x00ff00ff;
	        x |= x >> 8;
	        y |= y >> 8;
	        x &= 0x0000ffff;
	        y &= 0x0000ffff;
        }

        public static CLIM analyze(byte[] data, string shortPath)
        {
            CLIM bclim = new CLIM
            {
                FileName = Path.GetFileNameWithoutExtension(shortPath),
                FilePath = Path.GetDirectoryName(shortPath),
                Extension = Path.GetExtension(shortPath)
            };
            byte[] byteArray = data;
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
        public struct CLIM
        {
            public uint Magic;        // CLIM = 0x4D494C43
            public UInt16 BOM;          // 0xFFFE
            public uint CLIMLength;   // HeaderLength - 14
            public int TileWidth;       // 1<<[[n]]
            public int TileHeight;      // 1<<[[n]]
            public uint totalLength;  // Total Length of file
            public uint Count;        // "1" , guessing it's just Count.

            public char[] imag;         // imag = 0x67616D69
            public uint imagLength;   // HeaderLength - 10
            public UInt16 Width;        // Final Dimensions
            public UInt16 Height;       // Final Dimensions
            public int FileFormat;    // ??
            public uint dataLength;   // Pixel Data Region Length

            public byte[] Data;

            public int BaseSize;

            //// Contained Data
            //public int ColorFormat;
            //public int ColorCount;
            //public Color[] Colors;

            //public int[] Pixels;

            public string FileName;
            public string FilePath;
            public string Extension;
        }
    }
}