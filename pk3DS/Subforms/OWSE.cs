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
                ZoneData zo = new ZoneData(zonedata.Skip(f * ZoneData.Size).Take(ZoneData.Size).ToArray());
                string LocationName = gameLocations[zo.ParentMap];
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
            byte[] raw = File.ReadAllBytes(filepaths[entry]);
            locationData = CTR.mini.unpackMini(raw, "ZO");
            if (locationData == null) return;

            // Read master ZD table
            byte[] zd = zonedata.Skip(ZoneData.Size * entry).Take(ZoneData.Size).ToArray();
            RTB_ZDMaster.Lines = Scripts.getHexLines(zd, 0x10);

            // Load from location Data.
            CurrentZone = new Zone(locationData);
            // File 0 - ZoneData
            RTB_ZD.Lines = Scripts.getHexLines(locationData[0], 0x10);
            getZoneData();

            // File 1 - Overworld Setup & Script
            RTB_OWSC.Lines = Scripts.getHexLines(locationData[1], 0x10);
            getOWSData();

            // File 2 - Map Script
            RTB_MapSC.Lines = Scripts.getHexLines(locationData[2], 0x10);
            getScriptData();

            // File 3 - Encounters
            RTB_Encounter.Lines = Scripts.getHexLines(locationData[3], 0x10);

            // File 4 - ?? (ORAS Only?)
            RTB_File5.Lines = Scripts.getHexLines(locationData.Length <= 4 ? null : locationData[4], 0x10);
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

        private byte[] OWScriptData;
        private Zone CurrentZone;

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

        private void getZoneData()
        {
            L_ZDPreview.Text = "Text File: " + CurrentZone.ZD.TextFile
            + Environment.NewLine + "Map File: " + CurrentZone.ZD.MapMatrix;

            // Fetch Map Image
            DrawMap = CurrentZone.ZD.MapMatrix;
            PB_Map.Image = (CHK_AutoDraw.Checked) ? getMapImage() : null;
        }
        private void getOWSData()
        {
            File.WriteAllBytes("zd.bin", CurrentZone.Entities.Data);
            RTB_F.Text = RTB_N.Text = RTB_W.Text = RTB_T.Text = string.Empty;
            // Set Counters
            NUD_FurnCount.Value = CurrentZone.Entities.FurnitureCount; changeFurnitureCount(null, null);
            NUD_NPCCount.Value = CurrentZone.Entities.NPCCount; changeNPCCount(null, null);
            NUD_WarpCount.Value = CurrentZone.Entities.WarpCount; changeWarpCount(null, null);
            NUD_TrigCount.Value = CurrentZone.Entities.TriggerCount; changeTriggerCount(null, null);

            // Collect/Load Data
            NUD_FE.Value = (NUD_FE.Maximum < 0) ? -1 : 0; changeFurniture(null, null);
            NUD_NE.Value = (NUD_NE.Maximum < 0) ? -1 : 0; changeOverworld(null, null);
            NUD_WE.Value = (NUD_WE.Maximum < 0) ? -1 : 0; changeWarp(null, null);
            NUD_TE.Value = (NUD_TE.Maximum < 0) ? -1 : 0; changeTrigger(null, null);

            // Process Scripts
            OWScriptData = CurrentZone.Entities.ScriptData;
            if (OWScriptData.Length > 4)
            {
                byte[] ScriptData = OWScriptData;
                int length = CurrentZone.Entities.ScriptLength;

                RTB_OS.Lines = Scripts.getHexLines(ScriptData);

                int start = BitConverter.ToInt32(ScriptData, 0x8) - 4;
                int moves = BitConverter.ToInt32(ScriptData, 0xC) - 4;
                int finaloffset = BitConverter.ToInt32(ScriptData, 0x10) - 4;
                int reserved = BitConverter.ToInt32(ScriptData, 0x14) - 4;
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

        // Overworld Functions
        #region Enabling
        internal static void toggleEnable(NumericUpDown master, NumericUpDown slave)
        {
            slave.Maximum = master.Value - 1;
            slave.Enabled = slave.Maximum > -1;
            slave.Minimum = (slave.Enabled) ? 0 : -1;
        }
        private void changeFurnitureCount(object sender, EventArgs e)
        {
            // Resize array
            int count = (int)NUD_FurnCount.Value;
            CurrentZone.Entities.FurnitureCount = count;
            Array.Resize(ref CurrentZone.Entities.Furniture, count);
            for (int i = 0; i < count; i++)
                CurrentZone.Entities.Furniture[i] = CurrentZone.Entities.Furniture[i] ?? new Zone.ZoneEntities.EntityFurniture();

            toggleEnable(NUD_FurnCount, NUD_FE);
        }
        private void changeNPCCount(object sender, EventArgs e)
        {
            // Resize array
            int count = (int)NUD_NPCCount.Value;
            CurrentZone.Entities.NPCCount = count;
            Array.Resize(ref CurrentZone.Entities.NPCs, count);
            for (int i = 0; i < count; i++)
                CurrentZone.Entities.NPCs[i] = CurrentZone.Entities.NPCs[i] ?? new Zone.ZoneEntities.EntityNPC();

            toggleEnable(NUD_NPCCount, NUD_NE);
        }
        private void changeWarpCount(object sender, EventArgs e)
        {
            // Resize array
            int count = (int)NUD_WarpCount.Value;
            CurrentZone.Entities.WarpCount = count;
            Array.Resize(ref CurrentZone.Entities.Warps, count);
            for (int i = 0; i < count; i++)
                CurrentZone.Entities.Warps[i] = CurrentZone.Entities.Warps[i] ?? new Zone.ZoneEntities.EntityWarp();

            toggleEnable(NUD_WarpCount, NUD_WE);
        }
        private void changeTriggerCount(object sender, EventArgs e)
        {
            // Resize array
            int count = (int)NUD_TrigCount.Value;
            CurrentZone.Entities.TriggerCount = count;
            Array.Resize(ref CurrentZone.Entities.Triggers, count);
            for (int i = 0; i < count; i++)
                CurrentZone.Entities.Triggers[i] = CurrentZone.Entities.Triggers[i] ?? new Zone.ZoneEntities.EntityTrigger();

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
                // No attributes editable atm
            }
            fEntry = (int)NUD_FE.Value;

            // Load New Data
            var Furniture = CurrentZone.Entities.Furniture[fEntry];
            RTB_F.Text = Util.getHexString(Furniture.Raw);
        }
        private void changeOverworld(object sender, EventArgs e)
        {
            if (NUD_NE.Value < 0) return;

            // Set Old Data
            if (nEntry > 0)
            {
                var n = CurrentZone.Entities.NPCs[nEntry];
                n.ID = (int)NUD_NID.Value;
                n.Model = (int)NUD_NModel.Value;
                n.SpawnFlag = (int)NUD_NFlag.Value;
                n.Script = (int)NUD_NScript.Value;
                
                n.FaceDirection = (int)NUD_NFace.Value;
                n.X = (int)NUD_NX.Value;
                n.Y = (int)NUD_NY.Value;
            }
            nEntry = (int)NUD_NE.Value;

            // Load New Data
            var NPC = CurrentZone.Entities.NPCs[nEntry];

            // Load new Attributes
            NUD_NID.Value = NPC.ID;
            NUD_NModel.Value = NPC.Model;
            NUD_NFlag.Value = NPC.SpawnFlag;
            NUD_NScript.Value = NPC.Script;

            NUD_NFace.Value = NPC.FaceDirection;
            NUD_NX.Value = NPC.X;
            NUD_NY.Value = NPC.Y;

            RTB_N.Text = Util.getHexString(NPC.Raw);
        }
        private void changeWarp(object sender, EventArgs e)
        {
            if (NUD_WE.Value < 0) return;

            // Set Old Data
            if (wEntry > 0)
            {
                CurrentZone.Entities.Warps[wEntry].DestinationMap = (int)NUD_WMap.Value;
                CurrentZone.Entities.Warps[wEntry].DestinationTileIndex = (int)NUD_WTile.Value;
            }
            wEntry = (int)NUD_WE.Value;

            // Load New Data
            var Warp = CurrentZone.Entities.Warps[wEntry];
            RTB_W.Text = Util.getHexString(Warp.Raw);

            // Load new Attributes
            NUD_WMap.Value = Warp.DestinationMap;
            NUD_WTile.Value = Warp.DestinationTileIndex;

            // Flavor Mods
            L_WarpDest.Text = zdLocations[Warp.DestinationMap];
        }
        private void changeTrigger(object sender, EventArgs e)
        {
            if (NUD_TE.Value < 0) return;

            // Set Old Data
            if (tEntry > 0)
            {
                // No attributes editable atm
            }
            tEntry = (int)NUD_TE.Value;

            // Load New Data
            var Trigger = CurrentZone.Entities.Triggers[tEntry];
            RTB_T.Text = Util.getHexString(Trigger.Raw);
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