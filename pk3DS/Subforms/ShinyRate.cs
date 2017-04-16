using System;
using System.IO;
using System.Windows.Forms;

namespace pk3DS
{
    public partial class ShinyRate : Form
    {
        public ShinyRate()
        {
            InitializeComponent();
            if (Main.ExeFSPath == null) { Util.Alert("No exeFS code to load."); Close(); }
            string[] files = Directory.GetFiles(Main.ExeFSPath);
            if (!File.Exists(files[0]) || !Path.GetFileNameWithoutExtension(files[0]).Contains("code")) { Util.Alert("No .code.bin detected."); Close(); }
            codebin = files[0];
            exefsData = File.ReadAllBytes(codebin);
            if (exefsData.Length % 0x200 != 0) { Util.Alert(".code.bin not decompressed. Aborting."); Close(); }

            // Fetch Offset
            byte[] pattern = {0x01, 0x50, 0x85, 0xE2, 0x05, 0x00, 0x50, 0xE1, 0xDE, 0xFF, 0xFF, 0xCA};
            offset = Util.IndexOfBytes(exefsData, pattern, 0, 0) - 4;
            if (offset < 0)
            {
                Util.Alert("Unable to find PID Generation routine.", "Closing.");
                Close();
            }
            if (exefsData[offset] != 0x23) // already patched
            {
                uint val = BitConverter.ToUInt32(exefsData, offset);
                val &= 0x00FFFFFF;
                val = (val & 0xFFF) | ((val & 0x00FF0000) >> 4);
                Util.Alert(".code.bin was already patched for shiny rate.", "Loaded existing value.");
                NUD_Rerolls.Value = Math.Max(NUD_Rerolls.Minimum, Math.Min(NUD_Rerolls.Maximum, val));
                modified = true;
            }
            changeRerolls(null, null);
        }

        private readonly bool modified;
        private readonly string codebin;
        private readonly int offset;
        private readonly byte[] exefsData;

        private void B_Cancel_Click(object sender, EventArgs e) => Close();
        private void B_Save_Click(object sender, EventArgs e)
        {
            writeCodePatch();
            File.WriteAllBytes(codebin, exefsData);
            Close();
        }
        private void changeRerolls(object sender, EventArgs e)
        {
            int count = (int)NUD_Rerolls.Value;
            const int bc = 4096;
            var pct = 1 - Math.Pow((float)(bc - 1)/bc, count);
            L_Overall.Text = $"~{pct:P}";
        }
        private void writeCodePatch()
        {
            // Overwrite the "load input argument value for reroll count" so that it loads a constant value.
            // 23 00 D4 E5 is then replaced with the instruction MOVW R0, $value
            // $value is the amount of PID rerolls to iterate for.

            int rerolls = (int)NUD_Rerolls.Value;
            if (rerolls > ushort.MaxValue)
                rerolls = ushort.MaxValue;
            byte[] data = {00, 00, 00, 0xE3}; // MOVW R0, xxx
            BitConverter.GetBytes((ushort)rerolls).CopyTo(data, 0x00); // = xxx
            data[2] = (byte)(data[1] >> 4);
            data[1] &= 0xF;
            data.CopyTo(exefsData, offset);
        }

        private void B_RestoreOriginal_Click(object sender, EventArgs e)
        {
            if (modified)
            {
                new byte[] {0x23, 0x00, 0xD4, 0xE5}.CopyTo(exefsData, offset);
                File.WriteAllBytes(codebin, exefsData);
            }
            Close();
        }
        private void changePercent(object sender, EventArgs e)
        {
            var pct = NUD_Rate.Value;
            const int bc = 4096;

            var inv = (int)Math.Log(1 - (float)pct/100, (float) (bc - 1)/bc);
            if (pct == 0)
                pct = 0.00001m; // arbitrary nonzero
            L_RerollCount.Text = $"Count: {inv.ToString("0")} = 1:{(int)(1/(pct/100))}";
        }
    }
}