using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Windows.Forms;

namespace pk3DS
{
    public partial class TypeChart : Form
    {
        private readonly string CROPath = Path.Combine(Main.RomFSPath, "DllBattle.cro");
        private readonly string[] types = Main.getText(Main.oras ? 18 : 17);
        private readonly int offset = Main.oras ? 0x000DB428 : 0x000D12A8;
        private readonly byte[] chart = new byte[0x144];
        private readonly byte[] CROData;

        private readonly uint[] Colors = { 0xFF000000, 
                            0, // unused
                            0xFFFF0000,
                            0, // unused
                            0xFFFFFFFF, 
                            0, 0, 0, // unused
                            0xFF008000 };
        public TypeChart()
        {
            if (!File.Exists(CROPath))
            {
                Util.Error("CRO does not exist! Closing.", CROPath);
                Close();
            }
            InitializeComponent();

            CROData = File.ReadAllBytes(CROPath);
            Array.Copy(CROData, offset, chart, 0, chart.Length);

            populateChart();
        }

        private void populateChart()
        {
            PB_Chart.Image = getGrid(32, 18, chart);
        }
        private Bitmap getGrid(int itemsize, int itemsPerRow, byte[] vals)
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

                uint itemColor = Colors[vals[i]];

                // Plop into image
                for (int x = 0; x < itemsize * itemsize; x++)
                    Buffer.BlockCopy(BitConverter.GetBytes(itemColor), 0, bmpData,
                        (Y * itemsize + x % itemsize) * width * 4 + (X * itemsize + x / itemsize) * 4, 4);
            }
            // slap on a grid
            for (int i = 0; i < width * height; i++)
                if (i % itemsize == 0 || i / (itemsize * itemsPerRow) % itemsize == 0)
                    Buffer.BlockCopy(BitConverter.GetBytes(0x17000000), 0, bmpData,
                        i / (itemsize * itemsPerRow) * width * 4 + i % (itemsize * itemsPerRow) * 4, 4);

            // assemble image
            Bitmap b = new Bitmap(width, height, PixelFormat.Format32bppArgb);
            BitmapData bData = b.LockBits(new Rectangle(0, 0, width, height), ImageLockMode.WriteOnly, PixelFormat.Format32bppArgb);
            System.Runtime.InteropServices.Marshal.Copy(bmpData, 0, bData.Scan0, bmpData.Length);
            b.UnlockBits(bData);
            return b;
        }
        private void B_Save_Click(object sender, EventArgs e)
        {
            Array.Copy(chart, 0, CROData, offset, chart.Length);
            File.WriteAllBytes(CROPath, CROData);
            Close();
        }
        private void B_Cancel_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void moveMouse(object sender, MouseEventArgs e)
        {
            int X = e.X / 32;
            int Y = e.Y / 32;
            if (e.X == (sender as PictureBox).Width - 1 - 2) // tweak because the furthest pixel is unused for transparent effect, and 2 px are used for border
                X -= 1;
            if (e.Y == (sender as PictureBox).Height - 1 - 2)
                Y -= 1;

            int index = Y*18 + X;
            updateLabel(X, Y, chart[index]);
        }
        private void clickMouse(object sender, MouseEventArgs e)
        {
            int X = e.X / 32;
            int Y = e.Y / 32;
            if (e.X == (sender as PictureBox).Width - 1 - 2) // tweak because the furthest pixel is unused for transparent effect, and 2 px are used for border
                X -= 1;
            if (e.Y == (sender as PictureBox).Height - 1 - 2)
                Y -= 1;

            int index = Y * 18 + X;
            if (e.Button == MouseButtons.Left) // Increase
            switch (chart[index])
            {
                case 08:
                    chart[index] = 4;
                    break;
                case 04:
                    chart[index] = 2;
                    break;
                case 02:
                    chart[index] = 0;
                    break;
                case 00:
                    chart[index] = 8;
                    break;
            }
            else // Decrease
            switch (chart[index])
            {
                case 08:
                    chart[index] = 0;
                    break;
                case 04:
                    chart[index] = 8;
                    break;
                case 02:
                    chart[index] = 4;
                    break;
                case 00:
                    chart[index] = 2;
                    break;
            }
            updateLabel(X, Y, chart[index]);
            populateChart();
        }
        private void updateLabel(int X, int Y, int value)
        {
            L_Hover.Text = string.Format("[{0}x{1}: {2}] {4} attacking {3} {5}", X.ToString("00"), Y.ToString("00"),
                value.ToString("00"), types[X], types[Y], effects[value]);
        }
        private readonly string[] effects =
        {
            "has no effect!",
            "",
            "is not very effective.",
            "",
            "does regular damage.",
            "", "", "",
            "is super effective!"
        };
    }
}