using pk3DS.Core;
using pk3DS.Core.Structures;
using System;
using System.IO;
using System.Windows.Forms;

namespace pk3DS
{
    public partial class TypeChart6 : Form
    {
        private readonly string CROPath = Path.Combine(Main.RomFSPath, "DllBattle.cro");
        private readonly string[] types = Main.Config.getText(TextName.Types);
        private readonly int offset = Main.Config.ORAS ? 0x000DB428 : 0x000D12A8;
        private readonly byte[] chart = new byte[TypeCount * TypeCount];
        private readonly byte[] CROData;
        private const int TypeCount = 18;
        private const int TypeWidth = 32;

        public TypeChart6()
        {
            if (!File.Exists(CROPath))
            {
                WinFormsUtil.Error("CRO does not exist! Closing.", CROPath);
                Close();
            }
            InitializeComponent();

            CROData = File.ReadAllBytes(CROPath);
            Array.Copy(CROData, offset, chart, 0, chart.Length);

            populateChart();
        }

        private void populateChart()
        {
            PB_Chart.Image = TypeChart.getGrid(TypeWidth, TypeCount, chart);
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
            int X = e.X / TypeWidth;
            int Y = e.Y / TypeWidth;
            if (e.X == (sender as PictureBox).Width - 1 - 2) // tweak because the furthest pixel is unused for transparent effect, and 2 px are used for border
                X -= 1;
            if (e.Y == (sender as PictureBox).Height - 1 - 2)
                Y -= 1;

            int index = Y * TypeCount + X;
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