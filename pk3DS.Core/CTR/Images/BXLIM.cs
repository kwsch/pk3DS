using System;
using System.Linq;

namespace pk3DS.Core.CTR.Images
{
    public abstract class BXLIM : IXLIMHeader
    {
        public byte[] PixelData;
        public IXLIMHeader Footer { get; protected set; }

        public uint Magic { get => Footer.Magic; set => Footer.Magic = value; }
        public ushort Width { get => Footer.Width; set => Footer.Width = value; }
        public ushort Height { get => Footer.Height; set => Footer.Height = value; }
        public XLIMEncoding Format { get => Footer.Format; set => Footer.Format = value; }
        public XLIMOrientation Orientation { get => Footer.Orientation; set => Footer.Orientation = value; }
        public bool Valid => Footer.Valid && PixelData != null;

        public string FileName { get; set; }
        public string FilePath { get; set; }
        public string Extension { get; set; }
        public int BaseSize => Math.Max(XLIMUtil.NextLargestPow2(Width), XLIMUtil.NextLargestPow2(Height));

        /// <summary>
        /// ARGB 32bpp
        /// </summary>
        public byte[] GetImageData(bool crop = true)
        {
            var orienter = new XLIMOrienter(Footer.Width, Footer.Height, Footer.Orientation);
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
                uint o = 4 * (coord.X + (coord.Y * Footer.Width));
                array[o + 0] = (byte)(val & 0xFF);
                array[o + 1] = (byte)(val >> 8 & 0xFF);
                array[o + 2] = (byte)(val >> 16 & 0xFF);
                array[o + 3] = (byte)(val >> 24 & 0xFF);
            }
            return array;
        }

        public virtual uint[] GetPixels()
        {
            return PixelConverter.GetPixels(PixelData, Footer.Format).ToArray();
        }

        public byte[] GetPixelsRaw()
        {
            var pix = GetPixels();
            byte[] raw = new byte[pix.Length * 4];
            Buffer.BlockCopy(pix, 0, raw, 0, raw.Length);
            return raw;
        }
    }
}
