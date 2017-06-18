using pk3DS.Core;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace pk3DS
{
    public partial class ShinyRate : Form
    {
        public ShinyRate()
        {
            InitializeComponent();
            if (Main.ExeFSPath == null) { WinFormsUtil.Alert("No exeFS code to load."); Close(); }
            string[] files = Directory.GetFiles(Main.ExeFSPath);
            if (!File.Exists(files[0]) || !Path.GetFileNameWithoutExtension(files[0]).Contains("code")) { WinFormsUtil.Alert("No .code.bin detected."); Close(); }
            codebin = files[0];
            exefsData = File.ReadAllBytes(codebin);
            if (exefsData.Length % 0x200 != 0) { WinFormsUtil.Alert(".code.bin not decompressed. Aborting."); Close(); }

            // Load instruction set
            byte[] raw = Core.Properties.Resources.asm_mov;
            for (int i = 0; i < raw.Length; i += 4)
            {
                byte[] data = new byte[2];
                Array.Copy(raw, i + 2, data, 0, 2);
                InstructionList.Add(new Instruction(BitConverter.ToUInt16(raw, i), data));
            }

            // Fetch Offset
            byte[] pattern = {0x01, 0x50, 0x85, 0xE2, 0x05, 0x00, 0x50, 0xE1, 0xDE, 0xFF, 0xFF, 0xCA};
            offset = Util.IndexOfBytes(exefsData, pattern, 0, 0) - 4;
            if (offset < 0)
            {
                WinFormsUtil.Alert("Unable to find PID Generation routine.", "Closing.");
                Close();
            }
            if (exefsData[offset] != 0x23) // already patched
            {
                uint val = BitConverter.ToUInt16(exefsData, offset);
                var instruction = InstructionList.FirstOrDefault(z => z.ArgVal == val);
                if (instruction == null)
                {
                    WinFormsUtil.Alert(".code.bin was modified externally.", "Existing value not loaded.");
                }
                else
                {
                    WinFormsUtil.Alert(".code.bin was already patched for shiny rate.", "Loaded existing value.");
                    NUD_Rerolls.Value = instruction.Value;
                }
                modified = true;
            }
            changeRerolls(null, null);
        }

        private readonly List<Instruction> InstructionList = new List<Instruction>();
        private readonly bool modified;
        private readonly string codebin;
        private readonly int offset;
        private readonly byte[] exefsData;

        private class Instruction
        {
            public readonly int Value;
            private readonly byte[] Argument;
            public readonly ushort ArgVal;

            public Instruction(int val, byte[] arg)
            {
                Value = val;
                Argument = arg;
                ArgVal = BitConverter.ToUInt16(Argument, 0);
            }
            public byte[] Bytes
            {
                get
                {
                    var bytes = new byte[] {0, 0, 0xA0, 0xE3};
                    Argument.CopyTo(bytes, 0);
                    return bytes;
                }
            }
        }

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
            // 23 00 D4 E5 is then replaced with the instruction MOV R0, $value
            // $value is the amount of PID rerolls to iterate for.

            int rerolls = (int)NUD_Rerolls.Value;
            if (rerolls > ushort.MaxValue)
                rerolls = ushort.MaxValue;
            // lazy precomputed table for MOV0 up to 9000, lol
            var instruction = InstructionList.FirstOrDefault(z => z.Value >= rerolls) ?? InstructionList.Last();
            byte[] data = instruction.Bytes;
            data.CopyTo(exefsData, offset);

            if (instruction.Value != rerolls)
                WinFormsUtil.Alert("Specified reroll count increased to the next highest supported value.",
                    $"{rerolls} -> {instruction.Value}");
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