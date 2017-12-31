using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;

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
        public byte[] GetImageData(bool crop = true)
        {
            var orienter = new BFLIMOrienter(Footer.Width, Footer.Height, Footer.Orientation);
            uint[] pixels = GetPixels();

            if (!crop)
            {
                Footer.Width = (ushort)orienter.Width;
                Footer.Height = (ushort)orienter.Height;
            }

            // uint[] -> byte[]
            byte[] array = new byte[Footer.Width * Footer.Height * 4];
            for (uint i = 0; i < pixels.Length; i++)
            {
                var coord = orienter.Get(i);
                if (coord.X >= Footer.Width || coord.Y >= Footer.Height)
                    continue;

                var val = pixels[i];
                Debug.WriteLine($"Writing {val:X8} for coord: X:{coord.X} | Y{coord.Y}");
                uint o = 4 * (coord.X + coord.Y * Footer.Width);
                array[o + 0] = (byte)(val & 0xFF);
                array[o + 1] = (byte)(val >> 8 & 0xFF);
                array[o + 2] = (byte)(val >> 16 & 0xFF);
                array[o + 3] = (byte)(val >> 24 & 0xFF);
            }
            return array;
        }
        public uint[] GetPixels()
        {
            return PixelConverter.GetPixels(PixelData, Footer.Format).ToArray();
        }
    }
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
    public struct FLIM
    {
        public const int SIZE = 40;
        public const string Identifier = "FLIM";
        public bool Valid => LittleEndian && Magic == 0x4D_49_4C_46; // FLIM
        public bool LittleEndian => BOM == 0xFEFF;
        public bool BigEndian => BOM == 0xFFFE;

        public uint Magic;          // FLIM
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
        public BFLIMEncoding Format;
        public BFLIMOrientation Orientation;
        public uint DataSize;
    }

    public enum BFLIMEncoding : byte
    {
        L8        = 0x00, // 8    Luminance
        A8        = 0x01, // 8    Alpha
        LA4       = 0x02, // 8    Luminance + Alpha
        LA8       = 0x03, // 16   Luminance + Alpha
        HILO8     = 0x04, // 16   ?
        RGB565    = 0x05, // 16   Color
        RGBX8     = 0x06, // 24   Color
        RGB5A1    = 0x07, // 16   Color + Alpha
        RGBA4     = 0x08, // 16   Color + Alpha
        RGBA8     = 0x09, // 32   Color + Alpha
        ETC1      = 0x0A, // 4    Color
        ETC1A4    = 0x0B, // 8    Color + Alpha
        L4        = 0x0C, // 4    Luminance
        A4        = 0x0D, // 4    Alpha
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
                case BFLIMEncoding.L4:
                case BFLIMEncoding.L8:
                {
                    r = g = b = (byte)val;
                    break;
                }
                case BFLIMEncoding.A4:
                case BFLIMEncoding.A8:
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
                case BFLIMEncoding.LA4:
                {
                    r = g = b = (byte)(val >> 4);
                    a = (byte)(val & 0x0F);
                    break;
                }
                case BFLIMEncoding.LA8:
                {
                    r = g = b = (byte)(val >> 8);
                    a = (byte)val;
                    break;
                }
                case BFLIMEncoding.RGBX8:
                {
                    return val | 0xFF000000;
                }
                case BFLIMEncoding.RGBA8:
                {
                    return val;
                }
                case BFLIMEncoding.RGBA4:
                {
                    a = (byte)(0x11 * (val & 0xf));
                    r = (byte)(0x11 * ((val >> 12) & 0xf));
                    g = (byte)(0x11 * ((val >> 8) & 0xf));
                    b = (byte)(0x11 * ((val >> 4) & 0xf));
                    break;
                }
                case BFLIMEncoding.RGB565:
                {
                    r = Convert5To8[(val >> 11) & 0x1F];
                    g = (byte)(((val >> 5) & 0x3F) * 4);
                    b = Convert5To8[val & 0x1F];
                    break;
                }
                case BFLIMEncoding.RGB5A1:
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
            BFLIMEncoding.RGBA8,
        };
        private static readonly HashSet<BFLIMEncoding> _24 = new HashSet<BFLIMEncoding>
        {
            BFLIMEncoding.RGBX8,
        };
        private static readonly HashSet<BFLIMEncoding> _16 = new HashSet<BFLIMEncoding>
        {
            BFLIMEncoding.LA8,
            BFLIMEncoding.HILO8,
            BFLIMEncoding.RGB565,
            BFLIMEncoding.RGB5A1,
            BFLIMEncoding.RGBA4,
        };
        private static readonly HashSet<BFLIMEncoding> _8 = new HashSet<BFLIMEncoding>
        {
            BFLIMEncoding.L8,
            BFLIMEncoding.A8,
            BFLIMEncoding.LA4,
            BFLIMEncoding.ETC1A4,
        };
    }

    [Flags]
    public enum BFLIMOrientation : byte
    {
        None = 0,
        Rotate90 = 4,
        Transpose = 8,
    }

    public class BFLIMOrienter
    {
        readonly BFLIMOrientation _orientation;

        public uint Width { get; }
        public uint Height { get; }
        public uint PanelsPerWidth { get; }
        
        public BFLIMOrienter(int width, int height, BFLIMOrientation orientation)
        {
            Width = (uint)BCLIM.nlpo2(BCLIM.gcm(width, 8));
            Height = (uint)BCLIM.nlpo2(BCLIM.gcm(height, 8));
            Debug.WriteLine($"Base Size = Width: {Width}, Height = {Height}");

            PanelsPerWidth = (uint)BCLIM.gcm(orientation == BFLIMOrientation.None ? width : height, 8) / 8;
            Debug.WriteLine($"PPW: {PanelsPerWidth}");

            _orientation = orientation;
        }

        public Coordinate Get(uint i)
        {
            BCLIM.d2xy(i & 0x3F, out uint x, out uint y);

            // Shift Tile Coordinate into Tilemap
            var tile = i >> 6;
            x |= (tile % PanelsPerWidth) << 3;
            y |= (tile / PanelsPerWidth) << 3;

            var coord = new Coordinate(x, y);
            if (_orientation.HasFlag(BFLIMOrientation.Rotate90))
                coord.Rotate90(Height);
            if (_orientation.HasFlag(BFLIMOrientation.Transpose))
                coord.Transpose();
            return coord;
        }
    }

    public class Coordinate
    {
        public uint X { get; private set; }
        public uint Y { get; private set; }

        public Coordinate(uint x, uint y)
        {
            X = x; Y = y;
        }
        public void Transpose()
        {
            var tmp = X;
            X = Y;
            Y = tmp;
        }
        public void Rotate90(uint height)
        {
            var tmp = X;
            X = Y;
            Y = height - 1 - tmp;
        }
    }
}
