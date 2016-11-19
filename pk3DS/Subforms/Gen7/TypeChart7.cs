using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Windows.Forms;

namespace pk3DS
{
    public partial class TypeChart7 : Form
    {
        private readonly int offset = Main.Config.ORAS ? 0x000DB428 : 0x000D12A8;
        private readonly byte[] chart = new byte[TypeCount * TypeCount];
        private readonly byte[] exefs;
        private readonly string[] types = Main.getText(TextName.Types);
        private const int TypeCount = 18;
        private const int TypeWidth = 32;

        public TypeChart7()
        {
            InitializeComponent();
            if (Main.ExeFSPath == null) { Util.Alert("No exeFS code to load."); Close(); }
            string[] files = Directory.GetFiles(Main.ExeFSPath);
            if (!File.Exists(files[0]) || !Path.GetFileNameWithoutExtension(files[0]).Contains("code")) { Util.Alert("No .code.bin detected."); Close(); }
            exefs = File.ReadAllBytes(files[0]);
            if (exefs.Length % 0x200 != 0) { Util.Alert(".code.bin not decompressed. Aborting."); Close(); }
            offset = Util.IndexOfBytes(exefs, Signature, 0x500000, 0) + Signature.Length;

            Array.Copy(exefs, offset, chart, 0, chart.Length);
            populateChart();
        }
        private readonly byte[] Signature =
        {
            0x02, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x01, 0x00, 0x00, 0x00, 0x02, 0x00, 0x00, 0x00,
            0xC3, 0x00, 0x00, 0x00, 0xCB, 0x00, 0x00, 0x00, 0xD3, 0x00, 0x00, 0x00, 0xDB, 0x00, 0x00, 0x00,
            0xF3, 0x00, 0x00, 0x00, 0xFB, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x01, 0x00, 0x00, 0x00,
            0x02, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x01, 0x00, 0x00, 0x00, 0x02, 0x00, 0x00, 0x00,
        };

        private void populateChart()
        {
            PB_Chart.Image = TypeChart.getGrid(TypeWidth, TypeCount, chart);
        }
        private void B_Save_Click(object sender, EventArgs e)
        {
            chart.CopyTo(exefs, offset);
            File.WriteAllBytes(Main.ExeFSPath, exefs);
            Close();
        }
        private void B_Cancel_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void moveMouse(object sender, MouseEventArgs e)
        {
            int X = e.X / TypeWidth;
            int Y = e.Y / TypeWidth;
            if (e.X == (sender as PictureBox).Width - 1 - 2) // tweak because the furthest pixel is unused for transparent effect, and 2 px are used for border
                X -= 1;
            if (e.Y == (sender as PictureBox).Height - 1 - 2)
                Y -= 1;

            int index = Y*TypeCount + X;
            updateLabel(X, Y, chart[index]);
        }
        private void clickMouse(object sender, MouseEventArgs e)
        {
            int X = e.X / TypeWidth;
            int Y = e.Y / TypeWidth;
            if (e.X == (sender as PictureBox).Width - 1 - 2) // tweak because the furthest pixel is unused for transparent effect, and 2 px are used for border
                X -= 1;
            if (e.Y == (sender as PictureBox).Height - 1 - 2)
                Y -= 1;

            int index = Y * TypeCount + X;
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