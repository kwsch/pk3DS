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
            { WinFormsUtil.Error("CRO does not exist! Closing.", CROPath); Close(); }

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
            GetCoordinate((PictureBox)sender, e, out int X, out int Y);
            int index = Y * TypeCount + X;

            updateLabel(X, Y, chart[index]);
        }
        private void clickMouse(object sender, MouseEventArgs e)
        {
            GetCoordinate((PictureBox)sender, e, out int X, out int Y);
            int index = Y * TypeCount + X;
            chart[index] = ToggleEffectiveness(chart[index], e.Button == MouseButtons.Left);

            updateLabel(X, Y, chart[index]);
            populateChart();
        }
        private void updateLabel(int X, int Y, int value)
        {
            L_Hover.Text = $"[{X:00}x{Y:00}: {value:00}] {types[Y]} attacking {types[X]} {effects[value]}";
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

        public static void GetCoordinate(Control sender, MouseEventArgs e, out int X, out int Y)
        {
            X = e.X / TypeWidth;
            Y = e.Y / TypeWidth;
            if (e.X == sender.Width - 1 - 2) // tweak because the furthest pixel is unused for transparent effect, and 2 px are used for border
                X -= 1;
            if (e.Y == sender.Height - 1 - 2)
                Y -= 1;
        }
        public static byte ToggleEffectiveness(byte currentValue, bool increase)
        {
            byte[] vals = { 0, 2, 4, 8 };
            int curIndex = Array.IndexOf(vals, currentValue);
            if (curIndex < 0)
                return currentValue;

            uint shift = (uint) (curIndex + (increase ? 1 : -1));
            var newIndex = shift % vals.Length;
            return vals[newIndex];
        }
    }
}