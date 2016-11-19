using System;
using System.Drawing;
using System.Drawing.Imaging;

namespace pk3DS
{
    public static class TypeChart
    {
        private static readonly uint[] Colors = { 0xFF000000,
                            0, // unused
                            0xFFFF0000,
                            0, // unused
                            0xFFFFFFFF,
                            0, 0, 0, // unused
                            0xFF008000 };

        public static Bitmap getGrid(int itemsize, int itemsPerRow, byte[] vals)
        {
            // set up image
            int width = itemsize * itemsPerRow,
                height = itemsize * vals.Length / itemsPerRow;
            byte[] bmpData = new byte[4 * width * height];

            // loop over area
            for (int i = 0; i < vals.Length; i++)
            {
                int X = i % itemsPerRow;
                int Y = i / itemsPerRow;
                
                // Plop into image
                byte[] itemColor = BitConverter.GetBytes(Colors[vals[i]]);
                for (int x = 0; x < itemsize * itemsize; x++)
                    Buffer.BlockCopy(itemColor, 0, bmpData,
                        (Y * itemsize + x % itemsize) * width * 4 + (X * itemsize + x / itemsize) * 4, 4);
            }
            // slap on a grid
            byte[] gridColor = BitConverter.GetBytes(0x17000000);
            for (int i = 0; i < width * height; i++)
                if (i % itemsize == 0 || i / (itemsize * itemsPerRow) % itemsize == 0)
                    Buffer.BlockCopy(gridColor, 0, bmpData,
                        i / (itemsize * itemsPerRow) * width * 4 + i % (itemsize * itemsPerRow) * 4, 4);

            // assemble image
            Bitmap b = new Bitmap(width, height, PixelFormat.Format32bppArgb);
            BitmapData bData = b.LockBits(new Rectangle(0, 0, width, height), ImageLockMode.WriteOnly, PixelFormat.Format32bppArgb);
            System.Runtime.InteropServices.Marshal.Copy(bmpData, 0, bData.Scan0, bmpData.Length);
            b.UnlockBits(bData);
            return b;
        }
    }
}
