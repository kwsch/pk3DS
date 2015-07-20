using System;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace pk3DS
{
    public partial class OWSE : Form
    {
        public OWSE()
        {
            InitializeComponent();
            AllowDrop = true;
            DragEnter += tabMain_DragEnter;
            DragDrop += tabMain_DragDrop;
            MapMatrixes = Directory.GetFiles("mapMatrix");
            MapGRs = Directory.GetFiles("mapGR");
            openQuick(Directory.GetFiles("encdata"));
            tabControl1.SelectedIndex = 2;  // Map Script Tab
        }
        private string[] MapMatrixes;
        private string[] MapGRs;
        private string[] encdatapaths;
        private string[] filepaths;
        private string[] gameLocations = Main.getText((Main.oras) ? 90 : 72);
        private string[] zdLocations;
        
        internal static Random rand = new Random();
        internal static uint rnd32()
        {
            return (uint)(rand.Next(1 << 30)) << 2 | (uint)(rand.Next(1 << 2));
        }

        private byte[] zonedata;
        private void openQuick(string[] encdata)
        {
            // Gather
            encdatapaths = encdata;
            Array.Sort(encdatapaths);
            filepaths = encdatapaths.Skip(Main.oras ? 2 : 1).Take(encdatapaths.Length - (Main.oras ? 2 : 1)).ToArray();
            zonedata = File.ReadAllBytes(encdatapaths[0]);
            zdLocations = new string[filepaths.Length];
            rawLocations = new string[filepaths.Length];

            tb_File5.Visible = Main.oras; // 5th File is only present with OR/AS.

            // Analyze
            for (int f = 0; f < filepaths.Length; f++)
            {
                string name = Path.GetFileNameWithoutExtension(filepaths[f]);

                int LocationNum = Convert.ToInt16(name.Substring(4, name.Length - 4));
                int indNum = LocationNum * 56 + 0x1C;
                string LocationName = gameLocations[BitConverter.ToUInt16(zonedata, indNum) & 0x1FF];
                zdLocations[f] = (LocationNum.ToString("000") + " - " + LocationName);
                rawLocations[f] = LocationName;
            }
            
            // Assign
            CB_LocationID.DataSource = zdLocations;
            CB_LocationID.Enabled = true;
            CB_LocationID_SelectedIndexChanged(null, null);
            NUD_WMap.Maximum = zdLocations.Length; // Cap map warp destinations to the amount of maps.
        }

        private int entry = -1;
        private byte[][] locationData;
        private string[] rawLocations;
        private void CB_LocationID_SelectedIndexChanged(object sender, EventArgs e)
        {
            setEntry();
            entry = CB_LocationID.SelectedIndex;
            getEntry();
        }
        private void getEntry()
        {
            if (entry < 0) return;
            RTB_F.Text = RTB_O.Text = RTB_W.Text = RTB_T.Text = string.Empty;
            byte[] raw = File.ReadAllBytes(filepaths[entry]);
            locationData = CTR.mini.unpackMini(raw, "ZO");
            if (locationData == null) return;

            RichTextBox[] rtba = {RTB_MapInfo, RTB_OWSC, RTB_MapSC, RTB_Encounter, RTB_File5};
            for (int i = 0; i < locationData.Length; i++)
                rtba[i].Lines = Scripts.getHexLines(locationData[i], 0x10);

            // File 0 - ??

            // File 1 - Overworld Setup & Script
            getOWSData();

            // File 2 - Map Script
            getScriptData();

            // File 3 - Encounters

            // File 4 - ??
        }
        private void setEntry()
        {
            //if (entry < 0) return;
            //
            //Force writeback of each overworld type
            //changeFurniture(null, null);
            //changeOverworld(null, null);
            //changeWarp(null, null);
            //changeTrigger(null, null);
            //
            //TODO: Reassemble the 5 files
            //locationData[0] = locationData[0];
            //locationData[1] = setOWSData();
            //locationData[2] = locationData[2];
            //locationData[3] = locationData[3];
            //if (Main.oras)
            //  locationData[4] = locationData[4];
            //
            //Package the files into the permanent package file.
            //byte[] raw = Util.packMini(locationData, "ZO");
            //File.WriteAllBytes(filepaths[entry], raw);
        }

        private byte[][] fData;
        private byte[][] nData;
        private byte[][] wData;
        private byte[][] tData;
        private byte[] OWScriptData;

        private void getScriptData()
        {
            byte[] data = locationData[2];
            if (data.Length > 4)
            {
                byte[] ScriptData = data;
                int length = BitConverter.ToInt32(ScriptData, 0);
                Array.Resize(ref ScriptData, length); // Cap Size

                RTB_MS.Lines = Scripts.getHexLines(ScriptData);

                int start = BitConverter.ToInt32(ScriptData, 0xC);
                int moves = BitConverter.ToInt32(ScriptData, 0x10);
                int finaloffset = BitConverter.ToInt32(ScriptData, 0x14);
                int reserved = BitConverter.ToInt32(ScriptData, 0x18);
                int compressedLength = length - start;
                int decompressedLength = finaloffset - start;

                L_MSSCDesc.Text = "Data Start: 0x" + start.ToString("X4")
                + Environment.NewLine + "Movement Offset: 0x" + moves.ToString("X4")
                + Environment.NewLine + "Total Used Size: 0x" + finaloffset.ToString("X4")
                + Environment.NewLine + "Reserved Size: 0x" + reserved.ToString("X4")
                + Environment.NewLine + "Compressed Len: 0x" + compressedLength.ToString("X4")
                + Environment.NewLine + "Decompressed Len: 0x" + decompressedLength.ToString("X4");

                byte[] compressed = ScriptData.Skip(start).ToArray();
                // string c = Util.getHexString(compressed);
                uint[] decompressed = Scripts.quickDecompress(compressed, decompressedLength/4) ?? new uint[0];
                // string d = Util.getHexString(decompressed);

                RTB_MSCMD.Lines = Scripts.getHexLines(decompressed);

                if (decompressedLength/4 != decompressed.Length)
                    RTB_MSCMD.Text = RTB_MSP.Text = "DCMP FAIL";
                else
                {
                    uint[] rawCMD = decompressed.Take((moves - start)/4).ToArray();
                    string[] instructions = Scripts.parseScript(rawCMD);
                    uint[] moveCMD = decompressed.Skip((moves - start)/4).ToArray();
                    string[] movements = Scripts.parseMovement(moveCMD);
                    RTB_MSP.Lines = instructions.Concat(movements).ToArray();
                }
                    
            }
            else
                RTB_MSCMD.Lines = RTB_MS.Lines = new[] {"No Data"};
        }
        private void getOWSData()
        {
            byte[] data = locationData[1];
            int len = BitConverter.ToInt32(data, 0);

            byte[] zd = zonedata.Skip(56*entry).Take(56).ToArray();
            ushort text = BitConverter.ToUInt16(zonedata, 56*entry + 6);
            ushort map = BitConverter.ToUInt16(zonedata, 56*entry + 4);
            RTB_zonedata.Lines = Scripts.getHexLines(zd, 0x10);
            L_ZDPreview.Text = "Text File: " + text
            + Environment.NewLine + "Map File: " + map;

            // Fetch Map Image
            DrawMap = map;
            PB_Map.Image = getMapImage();

            byte[] owData = data.Skip(4).Take(len).ToArray();
            // Process owData Header
            using (var s = new MemoryStream(owData))
            using (var br = new BinaryReader(s))
            {
                // Prepare Data
                byte F = br.ReadByte(); fData = new byte[255][]; for (int i = 0; i < 255; i++) fData[i] = new byte[flen];
                byte N = br.ReadByte(); nData = new byte[255][]; for (int i = 0; i < 255; i++) nData[i] = new byte[nlen];
                byte W = br.ReadByte(); wData = new byte[255][]; for (int i = 0; i < 255; i++) wData[i] = new byte[wlen];
                byte T = br.ReadByte(); tData = new byte[255][]; for (int i = 0; i < 255; i++) tData[i] = new byte[tlen];

                // Set Counters
                NUD_FurnCount.Value = F; changeFurnitureCount(null, null);
                NUD_NPCCount.Value = N; changeNPCCount(null, null);
                NUD_WarpCount.Value = W; changeWarpCount(null, null);
                NUD_TrigCount.Value = T; changeTriggerCount(null, null);

                // Collect/Load Data
                for (int i = 0; i < F; i++) fData[i] = br.ReadBytes(flen); 
                for (int i = 0; i < N; i++) nData[i] = br.ReadBytes(nlen); 
                for (int i = 0; i < W; i++) wData[i] = br.ReadBytes(wlen); 
                for (int i = 0; i < T; i++) tData[i] = br.ReadBytes(tlen);
                NUD_FE.Value = (NUD_FE.Maximum < 0) ? -1 : 0; changeFurniture(null, null);
                NUD_NE.Value = (NUD_NE.Maximum < 0) ? -1 : 0; changeOverworld(null, null);
                NUD_WE.Value = (NUD_WE.Maximum < 0) ? -1 : 0; changeWarp(null, null);
                NUD_TE.Value = (NUD_TE.Maximum < 0) ? -1 : 0; changeTrigger(null, null);
            }

            // Process Scripts
            OWScriptData = data.Skip(4 + owData.Length).Take(data.Length - 4 - owData.Length).ToArray();
            if (OWScriptData.Length > 4)
            {
                byte[] ScriptData = OWScriptData;
                int length = BitConverter.ToInt32(ScriptData, 0);
                Array.Resize(ref ScriptData, length); // Cap Size

                RTB_OS.Lines = Scripts.getHexLines(ScriptData);

                int start = BitConverter.ToInt32(ScriptData, 0xC);
                int moves = BitConverter.ToInt32(ScriptData, 0x10);
                int finaloffset = BitConverter.ToInt32(ScriptData, 0x14);
                int reserved = BitConverter.ToInt32(ScriptData, 0x18);
                int compressedLength = length - start;
                int decompressedLength = finaloffset - start;

                L_OWSCDesc.Text = "Data Start: 0x" + start.ToString("X4")
                + Environment.NewLine + "Movement Offset: 0x" + moves.ToString("X4")
                + Environment.NewLine + "Total Used Size: 0x" + finaloffset.ToString("X4")
                + Environment.NewLine + "Reserved Size: 0x" + reserved.ToString("X4")
                + Environment.NewLine + "Compressed Len: 0x" + compressedLength.ToString("X4")
                + Environment.NewLine + "Decompressed Len: 0x" + decompressedLength.ToString("X4");

                byte[] compressed = ScriptData.Skip(start).ToArray();
                // string c = Util.getHexString(compressed);
                uint[] decompressed = Scripts.quickDecompress(compressed, decompressedLength/4) ?? new uint[0];
                // byte[] decompressed = Scripts.decompressScript(compressed) ?? new byte[0]; -- DEPRECATED
                // string d = Util.getHexString(decompressed);

                RTB_OWSCMD.Lines = Scripts.getHexLines(decompressed);

                if (decompressedLength/4 != decompressed.Length)
                    RTB_OWSCMD.Text = RTB_OSP.Text = "DCMP FAIL";
                else
                {
                    uint[] rawCMD = decompressed.Take((moves - start) / 4).ToArray();
                    string[] instructions = Scripts.parseScript(rawCMD);
                    uint[] moveCMD = decompressed.Skip((moves - start) / 4).ToArray();
                    string[] movements = Scripts.parseMovement(moveCMD);
                    RTB_OSP.Lines = instructions.Concat(movements).ToArray();
                }
            }
            else
                RTB_OWSCMD.Lines = RTB_OS.Lines = new[] {"No Data"};
        }

        private byte[] setOWSData()
        {
            using (var s = new MemoryStream())
            using (var bw = new BinaryWriter(s))
            {
                // Overworld Payload
                bw.Write(0); // 4 Byte Length of 0 (Temporary)
                bw.Write((byte)NUD_FurnCount.Value);
                bw.Write((byte)NUD_NPCCount.Value);
                bw.Write((byte)NUD_WarpCount.Value);
                bw.Write((byte)NUD_TrigCount.Value);
                for (int i = 0; i < NUD_FurnCount.Value; i++) bw.Write(fData[i]);
                for (int i = 0; i < NUD_NPCCount.Value; i++) bw.Write(nData[i]);
                for (int i = 0; i < NUD_WarpCount.Value; i++) bw.Write(wData[i]);
                for (int i = 0; i < NUD_TrigCount.Value; i++) bw.Write(tData[i]);
                // have to check for 00 padding
                while (s.Length % 4 != 0) bw.Write((byte)0);
                s.Position = 0; bw.Write((int)s.Length);
                s.Position = s.Length - 1;
                // Script Payload
                byte[] scriptData = Util.StringToByteArray(RTB_OWSCMD.Text);
                bw.Write(scriptData.Length);
                bw.Write(scriptData); // ScriptData

                return s.ToArray();
            }
        }

        // Overworld Functions
        private const int flen = 0x14;
        private const int nlen = 0x30;
        private const int wlen = 0x18;
        private const int tlen = 0x18;
        #region Enabling
        internal static void toggleEnable(NumericUpDown master, NumericUpDown slave)
        {
            slave.Maximum = master.Value - 1;
            slave.Enabled = slave.Maximum > -1;
            slave.Minimum = (slave.Enabled) ? 0 : -1;
        }
        private void changeFurnitureCount(object sender, EventArgs e)
        {
            toggleEnable(NUD_FurnCount, NUD_FE);
        }
        private void changeNPCCount(object sender, EventArgs e)
        {
            toggleEnable(NUD_NPCCount, NUD_NE);
        }
        private void changeWarpCount(object sender, EventArgs e)
        {
            toggleEnable(NUD_WarpCount, NUD_WE);
        }
        private void changeTriggerCount(object sender, EventArgs e)
        {
            toggleEnable(NUD_TrigCount, NUD_TE);
        }
        #endregion
        private int fEntry, nEntry, wEntry, tEntry = -1;
        private void changeFurniture(object sender, EventArgs e)
        {
            if (NUD_FE.Value < 0) return;

            // Set Old Data
            if (fEntry > 0)
            {
                byte[] oldData = fData[fEntry];
                fData[fEntry] = oldData;
            }
            fEntry = (int)NUD_FE.Value;

            // Load New Data
            byte[] data = fData[fEntry];
            RTB_F.Text = Util.getHexString(data);
        }
        private void changeOverworld(object sender, EventArgs e)
        {
            if (NUD_NE.Value < 0) return;

            // Set Old Data
            if (nEntry > 0)
            {
                byte[] oldData = nData[nEntry];
                nData[nEntry] = oldData;
            }
            nEntry = (int)NUD_NE.Value;

            // Load New Data
            byte[] data = nData[nEntry];
            RTB_O.Text = Util.getHexString(data);

            ushort ID = BitConverter.ToUInt16(data, 0x4);

            NUD_OID.Value = ID;
        }
        private void changeWarp(object sender, EventArgs e)
        {
            if (NUD_WE.Value < 0) return;

            // Set Old Data
            if (wEntry > 0)
            {
                byte[] oldData = wData[wEntry];
                wData[wEntry] = oldData;
            }
            wEntry = (int)NUD_WE.Value;

            // Load New Data
            byte[] data = wData[wEntry];
            RTB_W.Text = Util.getHexString(data);

            ushort Map = BitConverter.ToUInt16(data, 0x4);
            ushort Dest = BitConverter.ToUInt16(data, 0x6);

            // Flavor Mods
            string MapName = zdLocations[Map];
            L_WarpDest.Text = MapName;

            NUD_WMap.Value = Map;
            NUD_WTile.Value = Dest;
        }
        private void changeTrigger(object sender, EventArgs e)
        {
            if (NUD_TE.Value < 0) return;

            // Set Old Data
            if (tEntry > 0)
            {
                byte[] oldData = tData[tEntry];
                tData[tEntry] = oldData;
            }
            tEntry = (int)NUD_TE.Value;

            // Load New Data
            byte[] data = tData[tEntry];
            RTB_T.Text = Util.getHexString(data);
        }

        private void B_HLCMD_Click(object sender, EventArgs e)
        {
            int ctr = Util.highlightText(RTB_OSP, "**", Color.Red) + Util.highlightText(RTB_MSP, "**", Color.Red) / 2;
            Util.Alert(String.Format("{0} instance{1} of \"*\" present.", ctr, ctr > 1 ? "s" : ""));
        }


        private void tabMain_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop)) e.Effect = DragDropEffects.Copy;
        }
        private void tabMain_DragDrop(object sender, DragEventArgs e)
        {
            string[] files = (string[])e.Data.GetData(DataFormats.FileDrop);
            string path = files[0]; // open first D&D
            if (new FileInfo(path).Length < 10000000)
                parseScriptInput(File.ReadAllBytes(path));
        }
        internal static void parseScriptInput(byte[] OWScriptData)
        {
            string[] ret, raw, raw2;
            if (OWScriptData.Length > 4)
            {
                byte[] ScriptData = OWScriptData;
                int length = BitConverter.ToInt32(ScriptData, 0);
                Array.Resize(ref ScriptData, length); // Cap Size

                raw = Scripts.getHexLines(ScriptData);

                int start = BitConverter.ToInt32(ScriptData, 0xC);
                int moves = BitConverter.ToInt32(ScriptData, 0x10);
                int finaloffset = BitConverter.ToInt32(ScriptData, 0x14);
                // int reserved = BitConverter.ToInt32(ScriptData, 0x18);
                // int compressedLength = length - start;
                int decompressedLength = finaloffset - start;

                byte[] compressed = ScriptData.Skip(start).ToArray();
                // string c = Util.getHexString(compressed);
                uint[] decompressed = Scripts.quickDecompress(compressed, decompressedLength / 4) ?? new uint[0];

                raw2 = Scripts.getHexLines(decompressed);

                if (decompressedLength / 4 != decompressed.Length)
                    raw2 = ret = new [] {"DCMP FAIL"};
                else
                {
                    uint[] rawCMD = decompressed.Take((moves - start) / 4).ToArray();
                    string[] instructions = Scripts.parseScript(rawCMD);
                    uint[] moveCMD = decompressed.Skip((moves - start) / 4).ToArray();
                    string[] movements = Scripts.parseMovement(moveCMD);
                    ret = instructions.Concat(movements).ToArray();
                }
            }
            else
                raw = raw2 = ret = new[] { "No Data" };

            if (ModifierKeys == Keys.Control)
            {
                Clipboard.SetText(String.Join(Environment.NewLine, raw2));
                //Util.Alert("Set cmd to clipboard.");
            }
            else if (ModifierKeys == Keys.Alt)
            {
                Clipboard.SetText(String.Join(Environment.NewLine, raw));
                //Util.Alert("Set raw to clipboard.");
            }
            else
            {
                Clipboard.SetText(String.Join(Environment.NewLine, ret));
                //Util.Alert("Set parse to clipboard.");
            }
            System.Media.SystemSounds.Asterisk.Play();
        }

        private int DrawMap = -1;
        private MapMatrix mm;
        private Image getMapImage()
        {
            // Load MM
            byte[][] MM = CTR.mini.unpackMini(File.ReadAllBytes(MapMatrixes[DrawMap]), "MM");
            mm = new MapMatrix(MM[0]);

            // Load GR TileMaps
            for (int i = 0; i < mm.EntryList.Length; i++)
            {
                if (mm.EntryList[i] == 0xFFFF) // Mystery Zone
                    continue;
                byte[][] GR = CTR.mini.unpackMini(File.ReadAllBytes(MapGRs[mm.EntryList[i]]), "GR");
                mm.Entries[i] = new MapMatrix.Entry(GR[0]);
            }
            Image img = mm.Preview((int)NUD_Scale.Value, (int)NUD_Flavor.Value);
            img = Util.TrimBitmap((Bitmap)img);
            PB_Map.Size = new Size(img.Width + 2, img.Height + 2);
            return img;
        }
        private void B_Redraw_Click(object sender, EventArgs e)
        {
            if (DrawMap != -1)
                PB_Map.Image = getMapImage();

            if (ModifierKeys != Keys.Control ||
                Util.Prompt(MessageBoxButtons.YesNoCancel, "Export all?") != DialogResult.Yes)
                return;
            
            const string folder = "MapImages";
            if (!Directory.Exists(folder))
                Directory.CreateDirectory(folder);

            string[] result = new string[CB_LocationID.Items.Count];
            for (int i = 0; i < CB_LocationID.Items.Count; i++)
            {
                DrawMap = BitConverter.ToUInt16(zonedata, 56 * i + 4);
                Image img = getMapImage();
                using (MemoryStream ms = new MemoryStream())
                {
                    //error will throw from here
                    img.Save(ms, System.Drawing.Imaging.ImageFormat.Png);
                    byte[] data = ms.ToArray();
                    File.WriteAllBytes(Path.Combine(folder, String.Format("{0} ({1}).png", zdLocations[i].Replace('?', '-'), DrawMap)), data);
                }
                string l = mm.EntryList.Where(t => t != 0xFFFF).Aggregate("", (current, t) => current + t.ToString("000" + " "));
                result[i] = String.Format("{0}\t{1}\t{2}", DrawMap.ToString("000"), CB_LocationID.Items[i], l);
            }
            if (Util.Prompt(MessageBoxButtons.YesNoCancel, "Write parse output?") == DialogResult.Yes)
                File.WriteAllLines("MapLocations.txt", result);
            CB_LocationID.SelectedIndex = 0;
            Util.Alert("All images have been dumped to " + folder + ".");
        }
    }
}