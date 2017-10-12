using pk3DS.Core;
using pk3DS.Core.Structures;
using System;
using System.IO;
using System.Windows.Forms;

namespace pk3DS
{
    public partial class TypeChart7 : Form
    {
        private readonly int offset = Main.Config.ORAS ? 0x000DB428 : 0x000D12A8;
        private readonly string codebin;
        private readonly byte[] chart = new byte[TypeCount * TypeCount];
        private readonly byte[] exefs;
        private readonly string[] types = Main.Config.getText(TextName.Types);
        private const int TypeCount = 18;
        private const int TypeWidth = 32;

        public TypeChart7()
        {
            if (Main.ExeFSPath == null)
            { WinFormsUtil.Alert("No exeFS code to load."); Close(); }

            string[] files = Directory.GetFiles(Main.ExeFSPath);
            if (!File.Exists(files[0]) || !Path.GetFileNameWithoutExtension(files[0]).Contains("code"))
            { WinFormsUtil.Alert("No .code.bin detected."); Close(); }

            InitializeComponent();

            codebin = files[0];
            exefs = File.ReadAllBytes(codebin);
            if (exefs.Length % 0x200 != 0) { WinFormsUtil.Alert(".code.bin not decompressed. Aborting."); Close(); }
            offset = Util.IndexOfBytes(exefs, Signature, 0x400000, 0) + Signature.Length;

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
            File.WriteAllBytes(codebin, exefs);
            Close();
        }
        private void B_Cancel_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void moveMouse(object sender, MouseEventArgs e)
        {
            TypeChart6.GetCoordinate((PictureBox)sender, e, out int X, out int Y);
            int index = Y*TypeCount + X;

            updateLabel(X, Y, chart[index]);
        }
        private void clickMouse(object sender, MouseEventArgs e)
        {
            TypeChart6.GetCoordinate((PictureBox)sender, e, out int X, out int Y);
            int index = Y * TypeCount + X;
            chart[index] = TypeChart6.ToggleEffectiveness(chart[index], e.Button == MouseButtons.Left);

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
    }
}