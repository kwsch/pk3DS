using System;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace pk3DS
{
    public partial class OPower : Form
    {
        public OPower()
        {
            InitializeComponent();
            if (Main.ExeFS == null) { Util.Alert("No exeFS code to load."); Close(); }
            string[] files = Directory.GetFiles(Main.ExeFS);
            if (!File.Exists(files[0]) || !Path.GetFileNameWithoutExtension(files[0]).Contains("code")) { Util.Alert("No .code.bin detected."); Close(); }
            codebin = files[0];
            exefsData = File.ReadAllBytes(codebin);
            if (exefsData.Length % 0x200 != 0) { Util.Alert(".code.bin not decompressed. Aborting."); Close(); }

            // Fetch Offset
            offset = Util.IndexOfBytes(exefsData, new byte[] { 0x34, 0x39, 0x34, 0x36, 0x31, 0x38, 0x34, 0x35, 0x00 }, 0x400000, 0) + 9;
            while (exefsData[offset] == 0xFF) offset++;

            // Gather Data
            for (int i = 0; i < powerData.Length; i++)
                powerData[i] = exefsData.Skip(offset + 22*i).Take(22).ToArray();

            for (int i = 1; i < powerData.Length; i++) CB_Item.Items.Add(i);
            CB_Item.SelectedIndex = 0;
        }

        private string codebin;
        private int offset;
        private byte[] exefsData;
        private byte[][] powerData = new byte[65][];
        private string[] powerFlavor = Main.getText((Main.oras) ? 165 : 141);
        
        int entry = -1;
        private void changeEntry(object sender, EventArgs e)
        {
            setEntry();
            entry = CB_Item.SelectedIndex + 1;
            getEntry();
        }
        private void getEntry()
        {
            if (entry < 1) return;

            // Fetch Data
            byte _00 = powerData[entry][0];
            byte _01 = powerData[entry][1];
            byte _02 = powerData[entry][2];
            byte playerCost = powerData[entry][0x3];
            byte otherCost = powerData[entry][0x4];
            // 0x5 unused?
            int type = BitConverter.ToUInt16(powerData[entry], 0x6);
            int mini = BitConverter.ToUInt16(powerData[entry], 0x8);
            int desc = BitConverter.ToUInt16(powerData[entry], 0xA);
            int name = BitConverter.ToUInt16(powerData[entry], 0xC);
            int stage = powerData[entry][0xE];
            int lvlup = powerData[entry][0xF];
            int quality = BitConverter.ToUInt16(powerData[entry], 0x10);
            int efficacy = BitConverter.ToUInt16(powerData[entry], 0x12);
            byte duration = powerData[entry][0x14]; // sbyte FF = -1 (no duration?)
            // 0x15 unused?

            // Apply Fields
            NUD_PlayerCost.Value = playerCost;
            NUD_OtherCost.Value = otherCost;
            NUD_Stage.Value = stage;
            NUD_LevelUp.Value = lvlup;
            TB_Type.Text = powerFlavor[type];
            TB_Mini.Text = powerFlavor[mini];
            TB_Name.Text = powerFlavor[name];
            TB_Quality.Text = quality.ToString(); // powerFlavor[quality];
            RTB.Text = powerFlavor[desc].Replace("\\n", Environment.NewLine);

            NUD_Efficacy.Value = efficacy;
            NUD_Duration.Value = duration;

            NUD_0.Value = _00;
            NUD_1.Value = _01;
            NUD_2.Value = _02;
        }
        private void setEntry()
        {
            if (entry < 1) return;

            for (int i = 0; i < powerData.Length; i++)
                Array.Copy(powerData[i], 0, exefsData, offset + i * powerData[i].Length, powerData[i].Length);
        }

        private void formClosing(object sender, FormClosingEventArgs e)
        {
            setEntry();
            File.WriteAllBytes(codebin, exefsData);
        }
    }
}