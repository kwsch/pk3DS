using System;
using System.Collections.Generic;

namespace pk3DS.Core.CTR
{
    public static class PixelConverter
    {
        private const int BPP_32 = 32;
        private const int BPP_24 = 24;
        private const int BPP_16 = 16;
        private const int BPP_8 = 8;
        private const int BPP_4 = 4;

        internal static readonly byte[] Convert5To8 = {
            0x00,0x08,0x10,0x18,0x20,0x29,0x31,0x39,
            0x41,0x4A,0x52,0x5A,0x62,0x6A,0x73,0x7B,
            0x83,0x8B,0x94,0x9C,0xA4,0xAC,0xB4,0xBD,
            0xC5,0xCD,0xD5,0xDE,0xE6,0xEE,0xF6,0xFF
        };

        public static IEnumerable<uint> GetPixels(byte[] raw, XLIMEncoding e)
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

        internal static uint GetDecodedPixelValue(uint val, XLIMEncoding e)
        {
            byte a = byte.MaxValue, r, g, b;
            switch (e)
            {
                case XLIMEncoding.L4:
                case XLIMEncoding.L8:
                {
                    r = g = b = (byte)val;
                    break;
                }
                case XLIMEncoding.A4:
                case XLIMEncoding.A8:
                {
                    r = g = b = 0xFF;
                    a = (byte)val;
                    break;
                }
                case XLIMEncoding.HILO8:
                {
                    r = (byte)(val >> 8);
                    g = (byte)(val & 0xFF);
                    b = byte.MaxValue;
                    break;
                }
                case XLIMEncoding.LA4:
                {
                    r = g = b = (byte)(val >> 4);
                    a = (byte)(val & 0x0F);
                    break;
                }
                case XLIMEncoding.LA8:
                {
                    r = g = b = (byte)(val >> 8);
                    a = (byte)val;
                    break;
                }
                case XLIMEncoding.RGBX8:
                {
                    r = (byte)(val >> 16);
                    g = (byte)(val >> 8);
                    b = (byte)(val >> 0);
                    break;
                }
                case XLIMEncoding.RGBA8:
                {
                    r = (byte)(val >> 24);
                    g = (byte)(val >> 16);
                    b = (byte)(val >> 8);
                    a = (byte) val;
                    break;
                }
                case XLIMEncoding.RGBA4:
                {
                    a = (byte)(0x11 * (val & 0xf));
                    r = (byte)(0x11 * ((val >> 12) & 0xf));
                    g = (byte)(0x11 * ((val >> 8) & 0xf));
                    b = (byte)(0x11 * ((val >> 4) & 0xf));
                    break;
                }
                case XLIMEncoding.RGB565:
                {
                    r = Convert5To8[(val >> 11) & 0x1F];
                    g = (byte)(((val >> 5) & 0x3F) * 4);
                    b = Convert5To8[val & 0x1F];
                    break;
                }
                case XLIMEncoding.RGB5A1:
                {
                    r = Convert5To8[(val >> 11) & 0x1F];
                    g = Convert5To8[(val >> 6) & 0x1F];
                    b = Convert5To8[(val >> 1) & 0x1F];
                    a = (val & 1) == 1 ? byte.MaxValue : byte.MinValue;
                    break;
                }
                default:
                    throw new FormatException($"Unsupported {nameof(XLIMEncoding)} value = {e}");
            }
            return (uint)((a << 24) | (r << 16) | (g << 8) | b);
        }

        public static uint GetEncodedPixelValue(byte[] raw, int offset, int size)
        {
            return size switch
            {
                BPP_32 => BitConverter.ToUInt32(raw, offset),
                BPP_24 => BitConverter.ToUInt32(raw, offset) & 0x00FFFFFF,
                BPP_16 => BitConverter.ToUInt16(raw, offset),
                _ => raw[offset],
            };
        }

        public static int GetBitsPerPixel(this XLIMEncoding e)
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

        private static readonly HashSet<XLIMEncoding> _32 = new()
        {
            XLIMEncoding.RGBA8,
        };

        private static readonly HashSet<XLIMEncoding> _24 = new()
        {
            XLIMEncoding.RGBX8,
        };

        private static readonly HashSet<XLIMEncoding> _16 = new()
        {
            XLIMEncoding.LA8,
            XLIMEncoding.HILO8,
            XLIMEncoding.RGB565,
            XLIMEncoding.RGB5A1,
            XLIMEncoding.RGBA4,
        };

        private static readonly HashSet<XLIMEncoding> _8 = new()
        {
            XLIMEncoding.L8,
            XLIMEncoding.A8,
            XLIMEncoding.LA4,
            XLIMEncoding.ETC1A4,
        };
    }
}