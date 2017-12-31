using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace pk3DS.Core.CTR
{
    public class BFLIM
    {
        public byte[] PixelData;
        public FLIM Footer;

        public BFLIM(Stream data) => ReadBFLIM(data);
        public BFLIM(byte[] data)
        {
            using (var ms = new MemoryStream(data))
                ReadBFLIM(ms);
        }
        public BFLIM(string path)
        {
            var data = File.ReadAllBytes(path);
            using (var ms = new MemoryStream(data))
                ReadBFLIM(ms);
        }

        private void ReadBFLIM(Stream ms)
        {
            PixelData = new byte[ms.Length - FLIM.SIZE];
            ms.Read(PixelData, 0, PixelData.Length);
            var footer = new byte[FLIM.SIZE];
            ms.Read(footer, 0, footer.Length);
            Footer = footer.ToStructure<FLIM>();
        }

        /// <summary>
        /// ARGB 32bpp
        /// </summary>
        public byte[] GetImageData()
        {
            uint[] pixels = GetPixels();

            int p = gcm(Footer.Width, 8) / 8;
            if (p == 0) p = 1;
            // uint[] -> byte[]
            byte[] array = new byte[Footer.Width * Footer.Height * 4];
            for (int i = 0; i < array.Length; i += 4)
            {
                int o = GetOffset((uint)i, Footer.Width, p);
                if (o + 3 >= array.Length)
                    continue;
                var val = pixels[i / 4];
                array[o + 0] = (byte)(val & 0xFF);
                array[o + 1] = (byte)(val >> 8 & 0xFF);
                array[o + 2] = (byte)(val >> 16 & 0xFF);
                array[o + 3] = (byte)(val >> 24 & 0xFF);
            }
            return array;
        }
        public uint[] GetPixels()
        {
            return PixelConverter.GetPixels(PixelData, (BFLIMEncoding)Footer.Format).ToArray();
        }

        internal static int gcm(int n, int m)
        {
            return (n + m - 1) / m * m;
        }
        private int GetOffset(uint i, ushort footerWidth, int p)
        {
            BCLIM.d2xy(i % 64, out uint x, out uint y);
            var tile = i / 64;

            // Shift Tile Coordinate into Tilemap
            x += (uint)(tile % p) * 8;
            y += (uint)(tile / p) * 8;
            if (x > footerWidth)
                return int.MaxValue;

            return 4 * (int)(x + y * footerWidth);
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
    }

    public struct FLIM
    {
        public const int SIZE = 40;
        public bool Valid => LittleEndian && Magic == 0x4D_49_4C_46; // FLIM
        public bool LittleEndian => BOM == 0xFEFF;
        public bool BigEndian => BOM == 0xFFFE;

        public uint Magic;        // FLIM
        public ushort BOM;          // 0xFFFE
        public ushort HeaderLength; // always 0x14
        public int Version;
        public uint TotalLength;
        public uint Count;

        public uint imag; // imag = 67616D69
        public uint imagLength; // always 0x10
        public ushort Width;
        public ushort Height;
        public short Alignment;
        public byte Format;
        public byte Orientation;
        public uint DataSize;
    }

    public enum BFLIMEncoding
    {
        L8_UNORM              = 0x00, // 8    Luminance
        A8_UNORM              = 0x01, // 8    Alpha
        LA4_UNORM             = 0x02, // 8    Luminance + Alpha
        LA8_UNORM             = 0x03, // 16   Luminance + Alpha
        HILO8                 = 0x04, // 16   ?
        RGB565_UNORM          = 0x05, // 16   Color
        RGBX8_UNORM           = 0x06, // 24   Color
        RGB5A1_UNORM          = 0x07, // 16   Color + Alpha
        RGBA4_UNORM           = 0x08, // 16   Color + Alpha
        RGBA8_UNORM           = 0x09, // 32   Color + Alpha
        ETC1_UNORM            = 0x0A, // 4    Color
        ETC1A4_UNORM          = 0x0B, // 8    Color + Alpha
        L4_UNORM              = 0x0C, // 4    Luminance
        A4_UNORM              = 0x0D, // 4    Alpha
    }

    public static class PixelConverter
    {
        public static IEnumerable<uint> GetPixels(byte[] raw, BFLIMEncoding e)
        {
            int bpp = e.GetBitsPerPixel();
            if (bpp == BPP_4)
            {
                foreach (byte b in raw)
                {
                    byte _0 = (byte)(b & 0xF);
                    byte _1 = (byte)(b >> 4);
                    yield return GetDecodedPixelValue(_0, e);
                    yield return GetDecodedPixelValue(_1, e);
                }
                yield break;
            }

            for (int i = 0; i < raw.Length; i += bpp / 8)
            {
                uint val = GetEncodedPixelValue(raw, i, bpp);
                yield return GetDecodedPixelValue(val, e);
            }
        }

        internal static readonly byte[] Convert5To8 = {
            0x00,0x08,0x10,0x18,0x20,0x29,0x31,0x39,
            0x41,0x4A,0x52,0x5A,0x62,0x6A,0x73,0x7B,
            0x83,0x8B,0x94,0x9C,0xA4,0xAC,0xB4,0xBD,
            0xC5,0xCD,0xD5,0xDE,0xE6,0xEE,0xF6,0xFF
        };

        private static uint GetDecodedPixelValue(uint val, BFLIMEncoding e)
        {
            byte a = byte.MaxValue, r = 0, g = 0, b = 0;
            switch (e)
            {
                case BFLIMEncoding.L4_UNORM:
                case BFLIMEncoding.L8_UNORM:
                {
                    r = g = b = (byte)val;
                    break;
                }
                case BFLIMEncoding.A4_UNORM:
                case BFLIMEncoding.A8_UNORM:
                {
                    r = g = b = 0xFF;
                    a = (byte)val;
                    break;
                }
                case BFLIMEncoding.HILO8:
                {
                    r = (byte)(val >> 8);
                    g = (byte)(val & 0xFF);
                    b = byte.MaxValue;
                    break;
                }
                case BFLIMEncoding.LA4_UNORM:
                {
                    r = g = b = (byte)(val >> 4);
                    a = (byte)(val & 0x0F);
                    break;
                }
                case BFLIMEncoding.LA8_UNORM:
                {
                    r = g = b = (byte)(val >> 8);
                    a = (byte)val;
                    break;
                }
                case BFLIMEncoding.RGBX8_UNORM:
                {
                    return val | 0xFF000000;
                }
                case BFLIMEncoding.RGBA8_UNORM:
                {
                    return val;
                }
                case BFLIMEncoding.RGBA4_UNORM:
                {
                    a = (byte)(0x11 * (val & 0xf));
                    r = (byte)(0x11 * ((val >> 12) & 0xf));
                    g = (byte)(0x11 * ((val >> 8) & 0xf));
                    b = (byte)(0x11 * ((val >> 4) & 0xf));
                    break;
                }
                case BFLIMEncoding.RGB565_UNORM:
                {
                    r = Convert5To8[(val >> 11) & 0x1F];
                    g = (byte)(((val >> 5) & 0x3F) * 4);
                    b = Convert5To8[val & 0x1F];
                    break;
                }
                case BFLIMEncoding.RGB5A1_UNORM:
                {
                    r = Convert5To8[(val >> 11) & 0x1F];
                    g = Convert5To8[(val >> 6) & 0x1F];
                    b = Convert5To8[(val >> 1) & 0x1F];
                    a = (val & 1) == 1 ? byte.MaxValue : byte.MinValue;
                    break;
                }
                default:
                    throw new FormatException($"Unsupported {nameof(BFLIMEncoding)} value = {e}");
            }
            return (uint)((a << 24) | (r << 16) | (g << 8) | b);
        }

        public static uint GetEncodedPixelValue(byte[] raw, int offset, int size)
        {
            switch (size)
            {
                case BPP_32:
                    return BitConverter.ToUInt32(raw, offset);
                case BPP_24:
                    return BitConverter.ToUInt32(raw, offset) & 0x00FFFFFF;
                case BPP_16:
                    return BitConverter.ToUInt16(raw, offset);
                default:
                    return raw[offset];
            }
        }
        public static int GetBitsPerPixel(this BFLIMEncoding e)
        {
            if (_32.Contains(e))
                return BPP_32;
            if (_24.Contains(e))
                return BPP_24;
            if (_16.Contains(e))
                return BPP_16;
            if (_8.Contains(e))
                return BPP_8;
            return BPP_4;
        }
        private const int BPP_32 = 32;
        private const int BPP_24 = 24;
        private const int BPP_16 = 16;
        private const int BPP_8 = 8;
        private const int BPP_4 = 4;
        private static readonly HashSet<BFLIMEncoding> _32 = new HashSet<BFLIMEncoding>
        {
            BFLIMEncoding.RGBA8_UNORM,
        };
        private static readonly HashSet<BFLIMEncoding> _24 = new HashSet<BFLIMEncoding>
        {
            BFLIMEncoding.RGBX8_UNORM,
        };
        private static readonly HashSet<BFLIMEncoding> _16 = new HashSet<BFLIMEncoding>
        {
            BFLIMEncoding.LA8_UNORM,
            BFLIMEncoding.HILO8,
            BFLIMEncoding.RGB565_UNORM,
            BFLIMEncoding.RGB5A1_UNORM,
            BFLIMEncoding.RGBA4_UNORM,
        };
        private static readonly HashSet<BFLIMEncoding> _8 = new HashSet<BFLIMEncoding>
        {
            BFLIMEncoding.L8_UNORM,
            BFLIMEncoding.A8_UNORM,
            BFLIMEncoding.LA4_UNORM,
            BFLIMEncoding.ETC1A4_UNORM,
        };
    }
}
