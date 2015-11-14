using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using pk3DS.Subforms;

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
            mapView.Show();
            tabControl1.SelectedIndex = 1;  // Overworlds
        }
        internal static string[] MapMatrixes;
        internal static string[] MapGRs;
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

        internal static Zone CurrentZone;
        internal static MapMatrix mm;
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

        // Loading of Data
        private MapPermView mapView = new MapPermView();
        private void getZoneData()
        {
            L_ZDPreview.Text = "Text File: " + CurrentZone.ZD.TextFile
            + Environment.NewLine + "Map File: " + CurrentZone.ZD.MapMatrix;

            L_ZD.Text = String.Format("X: {0,5}{3}Y: {1,5}{3}Z:{2,6}{3}{3}X: {4,5}{3}Y: {5,5}{3}Z:{6,6}", CurrentZone.ZD.pX, CurrentZone.ZD.pY,
                CurrentZone.ZD.Z, Environment.NewLine, CurrentZone.ZD.pX2, CurrentZone.ZD.pY2,
                CurrentZone.ZD.Z2);

            if (Math.Abs(CurrentZone.ZD.pX - CurrentZone.ZD.pX2) > 0.01
                || Math.Abs(CurrentZone.ZD.pY - CurrentZone.ZD.pY2) > 0.01
                || CurrentZone.ZD.Z != CurrentZone.ZD.Z2)
            {
                L_ZD.Text += Environment.NewLine + "COORDINATE MISMATCH";
            }

            // Fetch Map Image
            mapView.drawMap(CurrentZone.ZD.MapMatrix);
        }
        private void getOWSData()
        {
            // Reset Fields a little.
            RTB_F.Text = RTB_N.Text = RTB_W.Text = RTB_T.Text = RTB_U.Text = string.Empty;
            fEntry = nEntry = wEntry = tEntry = uEntry = -1;
            // Set Counters
            NUD_FurnCount.Value = CurrentZone.Entities.FurnitureCount; changeFurnitureCount(null, null);
            NUD_NPCCount.Value = CurrentZone.Entities.NPCCount; changeNPCCount(null, null);
            NUD_WarpCount.Value = CurrentZone.Entities.WarpCount; changeWarpCount(null, null);
            NUD_TrigCount.Value = CurrentZone.Entities.TriggerCount; changeTriggerCount(null, null);
            NUD_UnkCount.Value = CurrentZone.Entities.UnknownCount; changeUnkCount(null, null);

            // Collect/Load Data
            NUD_FE.Value = (NUD_FE.Maximum < 0) ? -1 : 0; changeFurniture(null, null);
            NUD_NE.Value = (NUD_NE.Maximum < 0) ? -1 : 0; changeOverworld(null, null);
            NUD_WE.Value = (NUD_WE.Maximum < 0) ? -1 : 0; changeWarp(null, null);
            NUD_TE.Value = (NUD_TE.Maximum < 0) ? -1 : 0; changeTrigger(null, null);
            NUD_UE.Value = (NUD_UE.Maximum < 0) ? -1 : 0; changeUnk(null, null);

            // Process Scripts
            var script = CurrentZone.Entities.Script;
            if (script.Raw.Length > 4)
            {
                RTB_OS.Lines = Scripts.getHexLines(script.Raw);
                L_OWSCDesc.Text = script.Info;

                uint[] Instructions = script.DecompressedInstructions;
                RTB_OWSCMD.Lines = Scripts.getHexLines(Instructions);

                if (script.DecompressedLength / 4 != Instructions.Length)
                    RTB_OWSCMD.Text = RTB_OSP.Text = "DCMP FAIL";
                else
                    RTB_OSP.Lines = script.ParseScript.Concat(script.ParseMoves).ToArray();
            }
            else
                RTB_OWSCMD.Lines = RTB_OS.Lines = new[] {"No Data"};
        }
        private void getScriptData()
        {
            var script = CurrentZone.MapScript.Script;
            if (script.Raw.Length > 4)
            {
                RTB_MS.Lines = Scripts.getHexLines(script.Raw);
                L_MSSCDesc.Text = script.Info;

                uint[] Instructions = script.DecompressedInstructions;
                RTB_MSCMD.Lines = Scripts.getHexLines(Instructions);

                if (script.DecompressedLength / 4 != Instructions.Length)
                    RTB_MSCMD.Text = RTB_OSP.Text = "DCMP FAIL";
                else
                    RTB_MSP.Lines = script.ParseScript.Concat(script.ParseMoves).ToArray();
            }
            else
                RTB_MSCMD.Lines = RTB_OS.Lines = new[] { "No Data" };
        }

        // Overworld Functions
        #region Enabling
        internal static void toggleEnable(NumericUpDown master, NumericUpDown slave, GroupBox display)
        {
            slave.Maximum = master.Value - 1;
            slave.Enabled = display.Visible = slave.Maximum > -1;
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

            toggleEnable(NUD_FurnCount, NUD_FE, GB_F);
        }
        private void changeNPCCount(object sender, EventArgs e)
        {
            // Resize array
            int count = (int)NUD_NPCCount.Value;
            CurrentZone.Entities.NPCCount = count;
            Array.Resize(ref CurrentZone.Entities.NPCs, count);
            for (int i = 0; i < count; i++)
                CurrentZone.Entities.NPCs[i] = CurrentZone.Entities.NPCs[i] ?? new Zone.ZoneEntities.EntityNPC();

            toggleEnable(NUD_NPCCount, NUD_NE, GB_N);
        }
        private void changeWarpCount(object sender, EventArgs e)
        {
            // Resize array
            int count = (int)NUD_WarpCount.Value;
            CurrentZone.Entities.WarpCount = count;
            Array.Resize(ref CurrentZone.Entities.Warps, count);
            for (int i = 0; i < count; i++)
                CurrentZone.Entities.Warps[i] = CurrentZone.Entities.Warps[i] ?? new Zone.ZoneEntities.EntityWarp();

            toggleEnable(NUD_WarpCount, NUD_WE, GB_W);
        }
        private void changeTriggerCount(object sender, EventArgs e)
        {
            // Resize array
            int count = (int)NUD_TrigCount.Value;
            CurrentZone.Entities.TriggerCount = count;
            Array.Resize(ref CurrentZone.Entities.Triggers1, count);
            for (int i = 0; i < count; i++)
                CurrentZone.Entities.Triggers1[i] = CurrentZone.Entities.Triggers1[i] ?? new Zone.ZoneEntities.EntityTrigger1();

            toggleEnable(NUD_TrigCount, NUD_TE, GB_T);
        }
        private void changeUnkCount(object sender, EventArgs e)
        {
            // Resize array
            int count = (int)NUD_UnkCount.Value;
            CurrentZone.Entities.UnknownCount = count;
            Array.Resize(ref CurrentZone.Entities.Triggers2, count);
            for (int i = 0; i < count; i++)
                CurrentZone.Entities.Triggers2[i] = CurrentZone.Entities.Triggers2[i] ?? new Zone.ZoneEntities.EntityTrigger2();

            toggleEnable(NUD_UnkCount, NUD_UE, GB_U);
        }
        #endregion
        private int fEntry, nEntry, wEntry, tEntry, uEntry = -1;
        private void changeFurniture(object sender, EventArgs e)
        {
            if (NUD_FE.Value < 0) return;

            // Set Old Data
            if (fEntry > 0)
            {
                var f = CurrentZone.Entities.Furniture[fEntry];
                f.X = (int)NUD_FX.Value;
                f.Y = (int)NUD_FY.Value;
                f.WX = (int)NUD_FWX.Value;
                f.WY = (int)NUD_FWY.Value;
            }
            fEntry = (int)NUD_FE.Value;

            // Load New Data
            var Furniture = CurrentZone.Entities.Furniture[fEntry];
            NUD_FX.Value = Furniture.X;
            NUD_FY.Value = Furniture.Y;
            NUD_FWX.Value = Furniture.WX;
            NUD_FWY.Value = Furniture.WY;
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
                n.SightRange = (int)NUD_NRange.Value;
                n.X = (int)NUD_NX.Value;
                n.Y = (int)NUD_NY.Value;

                n.MovePermissions = (int)NUD_NMove1.Value;
                n.MovePermissions2 = (int)NUD_NMove2.Value;
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
            NUD_NRange.Value = NPC.SightRange;
            NUD_NX.Value = NPC.X;
            NUD_NY.Value = NPC.Y;
            NUD_NMove1.Value = NPC.MovePermissions;
            NUD_NMove2.Value = NPC.MovePermissions2;

            // Uneditables
            TB_NDeg.Text = NPC.Deg18.ToString();
            TB_Leash.Text = (NPC.L1 == NPC.L2 && NPC.L2 == NPC.L3 && NPC.L3 == -1)
                ? TB_Leash.Text = "No Leash!"
                : String.Format("{0}, {1}, {2} -- {3}", NPC.L1, NPC.L2, NPC.L3, NPC.LDir);

            RTB_N.Text = Util.getHexString(NPC.Raw);
        }
        private void changeWarp(object sender, EventArgs e)
        {
            if (NUD_WE.Value < 0) return;

            // Set Old Data
            if (wEntry > 0)
            {
                var w = CurrentZone.Entities.Warps[wEntry];
                w.DestinationMap = (int)NUD_WMap.Value;
                w.DestinationTileIndex = (int)NUD_WTile.Value;
                w.X = (int)NUD_WX.Value;
                w.Y = (int)NUD_WY.Value;
            }
            wEntry = (int)NUD_WE.Value;

            // Load New Data
            var Warp = CurrentZone.Entities.Warps[wEntry];
            RTB_W.Text = Util.getHexString(Warp.Raw);

            // Load new Attributes
            NUD_WMap.Value = Warp.DestinationMap;
            NUD_WTile.Value = Warp.DestinationTileIndex;

            NUD_WX.Value = Warp.X;
            NUD_WY.Value = Warp.Y;

            // Flavor Mods
            L_WarpDest.Text = zdLocations[Warp.DestinationMap];
        }
        private void changeTrigger(object sender, EventArgs e)
        {
            if (NUD_TE.Value < 0) return;

            // Set Old Data
            if (tEntry > 0)
            {
                var t1 = CurrentZone.Entities.Triggers1[tEntry];
                t1.X = (int)NUD_T1X.Value;
                t1.Y = (int)NUD_T1Y.Value;
            }
            tEntry = (int)NUD_TE.Value;

            // Load New Data
            var Trigger = CurrentZone.Entities.Triggers1[tEntry];
            NUD_T1X.Value = Trigger.X;
            NUD_T1Y.Value = Trigger.Y;
            RTB_T.Text = Util.getHexString(Trigger.Raw);
        }
        private void changeUnk(object sender, EventArgs e)
        {
            if (NUD_UE.Value < 0) return;

            // Set Old Data
            if (uEntry > 0)
            {
                var t2 = CurrentZone.Entities.Triggers1[uEntry];
                t2.X = (int)NUD_T2X.Value;
                t2.Y = (int)NUD_T2Y.Value;
            }
            uEntry = (int)NUD_UE.Value;

            // Load New Data
            var Unk = CurrentZone.Entities.Triggers2[uEntry];
            NUD_T2X.Value = Unk.X;
            NUD_T2Y.Value = Unk.Y;
            RTB_U.Text = Util.getHexString(Unk.Raw);
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
        private void parseScriptInput(byte[] data)
        {
            Script scr = new Script(data);
            RTB_CompressedScript.Lines = Scripts.getHexLines(scr.CompressedBytes);
            System.Media.SystemSounds.Asterisk.Play();
        }
        
        // User Enhancements
        private void changeNModel(object sender, EventArgs e)
        {
            L_ModelAsHex.Text = "0x"+((int)NUD_NModel.Value).ToString("X4");
        }
        private void dclickDestMap(object sender, EventArgs e)
        {
            var Tile = NUD_WTile.Value;
            CB_LocationID.SelectedIndex = (int)NUD_WMap.Value;
            try
            { NUD_WE.Value = Tile; }
            catch
            { try { NUD_WE.Value = 0; } catch {}}
        }

        // Dev Dumpers
        private void B_DumpFurniture_Click(object sender, EventArgs e)
        {
            if (Util.Prompt(MessageBoxButtons.YesNoCancel, "Export all Furniture?") != DialogResult.Yes)
                return;

            List<string> result = new List<string>();
            List<byte[]> data = new List<byte[]>();
            for (int i = 0; i < CB_LocationID.Items.Count; i++)
            {
                CB_LocationID.SelectedIndex = i;
                for (int j = 0; j < CurrentZone.Entities.FurnitureCount; j++)
                {
                    result.Add(Util.getHexString(CurrentZone.Entities.Furniture[j].Raw));
                    data.Add(CurrentZone.Entities.Furniture[j].Raw);
                }
            }
            if (Util.Prompt(MessageBoxButtons.YesNoCancel, "Write Furniture to file?") == DialogResult.Yes)
                File.WriteAllBytes("Furniture.bin", data.SelectMany(z => z).ToArray());

            if (Util.Prompt(MessageBoxButtons.YesNoCancel, "Copy Furniture to Clipboard?") == DialogResult.Yes)
                Clipboard.SetText(string.Join(Environment.NewLine, result));

            CB_LocationID.SelectedIndex = 0;
        }
        private void B_DumpNPC_Click(object sender, EventArgs e)
        {
            if (Util.Prompt(MessageBoxButtons.YesNoCancel, "Export all NPCs?") != DialogResult.Yes)
                return;

            List<string> result = new List<string>();
            List<byte[]> data = new List<byte[]>();
            for (int i = 0; i < CB_LocationID.Items.Count; i++)
            {
                CB_LocationID.SelectedIndex = i;
                for (int j = 0; j < CurrentZone.Entities.NPCCount; j++)
                {
                    result.Add(Util.getHexString(CurrentZone.Entities.NPCs[j].Raw));
                    data.Add(CurrentZone.Entities.NPCs[j].Raw);
                }
            }
            if (Util.Prompt(MessageBoxButtons.YesNoCancel, "Write NPCs to file?") == DialogResult.Yes)
                File.WriteAllBytes("NPCs.bin", data.SelectMany(z => z).ToArray());

            if (Util.Prompt(MessageBoxButtons.YesNoCancel, "Copy NPCs to Clipboard?") == DialogResult.Yes)
                Clipboard.SetText(string.Join(Environment.NewLine, result));

            CB_LocationID.SelectedIndex = 0;
        }
        private void B_DumpWarp_Click(object sender, EventArgs e)
        {
            if (Util.Prompt(MessageBoxButtons.YesNoCancel, "Export all Warps?") != DialogResult.Yes)
                return;

            List<string> result = new List<string>();
            List<byte[]> data = new List<byte[]>();
            for (int i = 0; i < CB_LocationID.Items.Count; i++)
            {
                CB_LocationID.SelectedIndex = i;
                for (int j = 0; j < CurrentZone.Entities.WarpCount; j++)
                {
                    result.Add(Util.getHexString(CurrentZone.Entities.Warps[j].Raw));
                    data.Add(CurrentZone.Entities.Warps[j].Raw);
                }
            }
            if (Util.Prompt(MessageBoxButtons.YesNoCancel, "Write Warps to file?") == DialogResult.Yes)
                File.WriteAllBytes("Warps.bin", data.SelectMany(z => z).ToArray());

            if (Util.Prompt(MessageBoxButtons.YesNoCancel, "Copy Warps to Clipboard?") == DialogResult.Yes)
                Clipboard.SetText(string.Join(Environment.NewLine, result));

            CB_LocationID.SelectedIndex = 0;
        }
        private void B_DumpTrigger_Click(object sender, EventArgs e)
        {
            if (Util.Prompt(MessageBoxButtons.YesNoCancel, "Export all Triggers?") != DialogResult.Yes)
                return;

            List<string> result = new List<string>();
            List<byte[]> data = new List<byte[]>();
            for (int i = 0; i < CB_LocationID.Items.Count; i++)
            {
                CB_LocationID.SelectedIndex = i;
                for (int j = 0; j < CurrentZone.Entities.TriggerCount; j++)
                {
                    result.Add(Util.getHexString(CurrentZone.Entities.Triggers1[j].Raw));
                    data.Add(CurrentZone.Entities.Triggers1[j].Raw);
                }
            }
            if (Util.Prompt(MessageBoxButtons.YesNoCancel, "Write Triggers to file?") == DialogResult.Yes)
                File.WriteAllBytes("Triggers.bin", data.SelectMany(z => z).ToArray());

            if (Util.Prompt(MessageBoxButtons.YesNoCancel, "Copy Triggers to Clipboard?") == DialogResult.Yes)
                Clipboard.SetText(string.Join(Environment.NewLine, result));

            CB_LocationID.SelectedIndex = 0;
        }
        private void B_DumpUnk_Click(object sender, EventArgs e)
        {
            if (Util.Prompt(MessageBoxButtons.YesNoCancel, "Export all Unks?") != DialogResult.Yes)
                return;

            List<string> result = new List<string>();
            List<byte[]> data = new List<byte[]>();
            for (int i = 0; i < CB_LocationID.Items.Count; i++)
            {
                CB_LocationID.SelectedIndex = i;
                for (int j = 0; j < CurrentZone.Entities.UnknownCount; j++)
                {
                    result.Add(Util.getHexString(CurrentZone.Entities.Triggers2[j].Raw));
                    data.Add(CurrentZone.Entities.Triggers2[j].Raw);
                }
            }
            if (Util.Prompt(MessageBoxButtons.YesNoCancel, "Write Unks to file?") == DialogResult.Yes)
                File.WriteAllBytes("Unks.bin", data.SelectMany(z => z).ToArray());

            if (Util.Prompt(MessageBoxButtons.YesNoCancel, "Copy Unks to Clipboard?") == DialogResult.Yes)
                Clipboard.SetText(string.Join(Environment.NewLine, result));

            CB_LocationID.SelectedIndex = 0;
        }

        private void B_DumpMaps_Click(object sender, EventArgs e)
        {
            if (Util.Prompt(MessageBoxButtons.YesNoCancel, "Export all MapImages?") != DialogResult.Yes)
                return;

            const string folder = "MapImages";
            if (!Directory.Exists(folder))
                Directory.CreateDirectory(folder);

            string[] result = new string[CB_LocationID.Items.Count];
            for (int i = 0; i < CB_LocationID.Items.Count; i++)
            {
                int DrawMap = BitConverter.ToUInt16(zonedata, 56 * i + 4);
                Image img = mapView.getMapImage(crop: true);
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
            if (Util.Prompt(MessageBoxButtons.YesNoCancel, "Write Map parse output?") == DialogResult.Yes)
                File.WriteAllLines("MapLocations.txt", result);
            CB_LocationID.SelectedIndex = 0;
            Util.Alert("All Map images have been dumped to " + folder + ".");
        }
        private void B_DumpZD_Click(object sender, EventArgs e)
        {
            if (Util.Prompt(MessageBoxButtons.YesNoCancel, "Export all ZD?") != DialogResult.Yes)
                return;

            List<string> result = new List<string>();
            List<byte[]> data = new List<byte[]>();
            for (int i = 0; i < CB_LocationID.Items.Count; i++)
            {
                CB_LocationID.SelectedIndex = i;
                result.Add(Util.getHexString(CurrentZone.ZD.Data));
                data.Add(CurrentZone.ZD.Data);
            }
            if (Util.Prompt(MessageBoxButtons.YesNoCancel, "Write ZDs to file?") == DialogResult.Yes)
                File.WriteAllBytes("ZDs.bin", data.SelectMany(z => z).ToArray());

            if (Util.Prompt(MessageBoxButtons.YesNoCancel, "Copy ZDs to Clipboard?") == DialogResult.Yes)
                Clipboard.SetText(string.Join(Environment.NewLine, result));

            CB_LocationID.SelectedIndex = 0;
        }

        private void pasteScript(object sender, EventArgs e)
        {
            // import data as bytes
            try
            {
                string text = RTB_CompressedScript.Text.Replace(Environment.NewLine, "").Replace("\n", "").Replace(" ", "");
                byte[] data = Util.StringToByteArray(text);

                byte[] dec = Scripts.decompressScript(data);

                RTB_DecompressedScript.Lines = Scripts.getHexLines(dec);
            }
            catch
            {
                RTB_DecompressedScript.Text = "DECMP ERROR";
            }
        }

        // Coordinate Simplifiers
        private void changeWarp_X(object sender, EventArgs e)
        {
            L_WpX.Text = (NUD_WX.Value / 18).ToString();
        }
        private void changeWarp_Y(object sender, EventArgs e)
        {
            L_WpY.Text = (NUD_WY.Value / 18).ToString();
        }

        private void B_Map_Click(object sender, EventArgs e)
        {
            if (!mapView.Visible)
                mapView.Show();
        }

        private void closingForm(object sender, FormClosingEventArgs e)
        {
            // Close map view
            mapView.Close();
            mapView.Dispose();
        }

    }
}