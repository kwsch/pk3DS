using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;
using pk3DS.Core.CTR;
using pk3DS.Core.CTR.Images;
using static pk3DS.Core.CTR.Images.XLIMUtil;

namespace pk3DS.Core
{
    /// <summary>
    /// Image Utility class using <see cref="System.Drawing"/>.
    /// </summary>
    public static class ImageUtil
    {
        /// <summary>
        /// Converts a <see cref="BFLIM"/> to <see cref="Bitmap"/> via the 32bit/pixel data.
        /// </summary>
        /// <param name="bflim">Image data</param>
        /// <param name="crop">Crop the image area to the actual dimensions</param>
        /// <returns>Human visible data</returns>
        public static Bitmap GetBitmap(this BXLIM bflim, bool crop = true)
        {
            if (bflim.Format == XLIMEncoding.ETC1 || bflim.Format == XLIMEncoding.ETC1A4)
                return GetBitmapETC(bflim, crop);
            var data = bflim.GetImageData(crop);
            return GetBitmap(data, bflim.Footer.Width, bflim.Footer.Height);
        }

        public static Bitmap GetBitmap(byte[] data, int width, int height, PixelFormat format = PixelFormat.Format32bppArgb)
        {
            var bmp = new Bitmap(width, height, format);
            var bmpData = bmp.LockBits(new Rectangle(0, 0, width, height), ImageLockMode.WriteOnly, format);
            var ptr = bmpData.Scan0;
            Marshal.Copy(data, 0, ptr, data.Length);
            bmp.UnlockBits(bmpData);
            return bmp;
        }

        public static byte[] GetPixelData(Bitmap bitmap)
        {
            var argbData = new byte[bitmap.Width * bitmap.Height * 4];
            var bd = bitmap.LockBits(new Rectangle(0, 0, bitmap.Width, bitmap.Height), ImageLockMode.ReadOnly, bitmap.PixelFormat);
            Marshal.Copy(bd.Scan0, argbData, 0, bitmap.Width * bitmap.Height * 4);
            bitmap.UnlockBits(bd);
            return argbData;
        }

        public static Bitmap GetBitmapETC(BXLIM bxlim, bool crop = true)
        {
            bool etc1a4 = bxlim.Footer.Format == XLIMEncoding.ETC1A4;
            byte[] data = bxlim.PixelData;
            ETC1.CheckETC1Lib();

            try
            {
                var width = Math.Max(NextLargestPow2(bxlim.Width), 16);
                var height = Math.Max(NextLargestPow2(bxlim.Height), 16);
                Bitmap img = new Bitmap(width, height);
                img = DecodeETC(bxlim, img, data, etc1a4);
                return crop ? CropBMP(bxlim, img) : img;
            }
            catch { return null; }
        }

        public static Bitmap CropBMP(IXLIMHeader bclim, Bitmap img)
        {
            Rectangle cropRect = new Rectangle(0, 0, bclim.Width, bclim.Height);
            Bitmap src = img;
            Bitmap target = new Bitmap(cropRect.Width, cropRect.Height);

            using (Graphics g = Graphics.FromImage(target))
            {
                g.DrawImage(src, new Rectangle(0, 0, target.Width, target.Height),
                    cropRect,
                    GraphicsUnit.Pixel);
            }
            Console.WriteLine($"Returning cropped {target.Width}");
            return target;
        }

        private static Bitmap DecodeETC(IXLIMHeader bclim, Bitmap img, byte[] textureData, bool etc1A4)
        {
            /* http://jul.rustedlogic.net/thread.php?id=17312
             * Much of this code is taken/modified from Tharsis. Thank you to Tharsis's creator, xdaniel.
             * https://github.com/xdanieldzd/Tharsis
             */

            /* Get compressed data & handle to it */

            //textureData = switchEndianness(textureData, 0x10);
            ushort[] input = new ushort[textureData.Length / sizeof(ushort)];
            Buffer.BlockCopy(textureData, 0, input, 0, textureData.Length);
            GCHandle pInput = GCHandle.Alloc(input, GCHandleType.Pinned);

            /* Marshal data around, invoke ETC1.dll for conversion, etc */
            uint size1 = 0;
            var w = (ushort)img.Width;
            var h = (ushort)img.Height;

            ETC1.ConvertETC1(IntPtr.Zero, ref size1, IntPtr.Zero, w, h, etc1A4); // true = etc1a4, false = etc1
            uint[] output = new uint[size1];
            GCHandle pOutput = GCHandle.Alloc(output, GCHandleType.Pinned);
            ETC1.ConvertETC1(pOutput.AddrOfPinnedObject(), ref size1, pInput.AddrOfPinnedObject(), w, h, etc1A4);
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
                    int k = (j + (i * img.Height)) * 4;
                    img.SetPixel(i, j, Color.FromArgb(imgData[k + 3], imgData[k], imgData[k + 1], imgData[k + 2]));
                }
            }
            if (bclim is BFLIM)
                return img;

            // Image is 13  instead of 12
            //          24             34
            img.RotateFlip(RotateFlipType.Rotate90FlipX);
            if (w > h)
            {
                // Image is now in appropriate order, but the shifting is messed up. Let's fix that.
                Bitmap img2 = new Bitmap(Math.Max(NextLargestPow2(bclim.Width), 16), Math.Max(NextLargestPow2(bclim.Height), 16));
                for (int y = 0; y < Math.Max(NextLargestPow2(bclim.Width), 16); y += 8)
                {
                    for (int x = 0; x < Math.Max(NextLargestPow2(bclim.Height), 16); x++)
                    {
                        for (int j = 0; j < 8; j++)
                        // Treat every 8 vertical pixels as 1 pixel for purposes of calculation, add to offset later.
                        {
                            int x1 = (x + (y / 8 * h)) % img2.Width; // Reshift x
                            int y1 = (x + (y / 8 * h)) / img2.Width * 8; // Reshift y
                            img2.SetPixel(x1, y1 + j, img.GetPixel(x, y + j)); // Reswizzle
                        }
                    }
                }
                return img2;
            }

            if (h > w)
            {
                Bitmap img2 = new Bitmap(Math.Max(NextLargestPow2(bclim.Width), 16), Math.Max(NextLargestPow2(bclim.Height), 16));
                for (int y = 0; y < Math.Max(NextLargestPow2(bclim.Width), 16); y += 8)
                {
                    for (int x = 0; x < Math.Max(NextLargestPow2(bclim.Height), 16); x++)
                    {
                        for (int j = 0; j < 8; j++)
                            // Treat every 8 vertical pixels as 1 pixel for purposes of calculation, add to offset later.
                        {
                            int x1 = x % img2.Width; // Reshift x
                            int y1 = (x + (y / 8 * h)) / img2.Width * 8; // Reshift y
                            img2.SetPixel(x1, y1 + j, img.GetPixel(x, y + j)); // Reswizzle
                        }
                    }
                }
                return img2;
            }

            return img;
        }
    }
}
