using System.Drawing;
using System.Drawing.Imaging;
using pk3DS.Core.CTR;

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
        public static Bitmap GetBitmap(this BFLIM bflim, bool crop = true)
        {
            var data = bflim.GetImageData(crop);
            return GetBitmap(data, bflim.Footer.Width, bflim.Footer.Height);
        }
        public static Bitmap GetBitmap(byte[] data, int width, int height)
        {
            var bmp = new Bitmap(width, height);
            var rect = new Rectangle(0, 0, width, height);
            var bmpData = bmp.LockBits(rect, ImageLockMode.ReadWrite, PixelFormat.Format32bppArgb);

            var ptr = bmpData.Scan0;
            System.Runtime.InteropServices.Marshal.Copy(data, 0, ptr, data.Length);
            bmp.UnlockBits(bmpData);

            return bmp;
        }
    }
}
