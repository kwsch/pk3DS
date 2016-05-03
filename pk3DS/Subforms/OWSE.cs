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

            // Script Drag&Drop
            AllowDrop = true;
            DragEnter += tabMain_DragEnter;
            DragDrop += tabMain_DragDrop;
            
            // Finished
            openQuick(Directory.GetFiles("encdata"));
            mapView.Show();
            tb_Zone.SelectedIndex = 1;  // Show Overworlds tab
        }
        private readonly string[] gameLocations = Main.getText(Main.oras ? 90 : 72);
        private string[] filepaths;
        private string[] encdatapaths;
        private byte[] masterZoneData;
        private bool debugToolDumping;

        // Generated Storage
        private string[] zdLocations;
        private string[] rawLocations;
        private byte[][] locationData;

        // Map Viewer References
        internal static Zone CurrentZone;
        internal static MapMatrix mm;
        private readonly MapPermView mapView = new MapPermView();

        private void openQuick(string[] encdata)
        {
            // Gather
            encdatapaths = encdata;
            Array.Sort(encdatapaths);
            filepaths = encdatapaths.Skip(Main.oras ? 2 : 1).Take(encdatapaths.Length - (Main.oras ? 2 : 1)).ToArray();
            masterZoneData = File.ReadAllBytes(encdatapaths[0]);
            zdLocations = new string[filepaths.Length];
            rawLocations = new string[filepaths.Length];

            tb_File5.Visible = Main.oras; // 5th File is only present with OR/AS.

            // Analyze
            for (int f = 0; f < filepaths.Length; f++)
            {
                string name = Path.GetFileNameWithoutExtension(filepaths[f]);

                int LocationNum = Convert.ToInt16(name.Substring(4, name.Length - 4));
                ZoneData zo = new ZoneData(masterZoneData.Skip(f * ZoneData.Size).Take(ZoneData.Size).ToArray());
                string LocationName = gameLocations[zo.ParentMap];
                zdLocations[f] = LocationNum.ToString("000") + " - " + LocationName;
                rawLocations[f] = LocationName;
            }
            
            // Assign
            CB_LocationID.DataSource = zdLocations;
            CB_LocationID.Enabled = true;
            CB_LocationID_SelectedIndexChanged(null, null);
            NUD_WMap.Maximum = zdLocations.Length; // Cap map warp destinations to the amount of maps.
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
            byte[] zd = masterZoneData.Skip(ZoneData.Size * entry).Take(ZoneData.Size).ToArray();
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
            if (entry < 0) return;
            if (debugToolDumping) return;
            
            // Set the data back into the class object
            // Currently only the first two files.
            setZoneData(); // File 0
            setOWSData();
            // setMSData();
            // setEncounterData();
            // if (Main.oras)
            //     setUnknown();

            // Reassemble files
            byte[][] data = CurrentZone.Write();

            // Debug Check (can stay, why not.)
            if (!locationData.Where((t, i) => !data[i].SequenceEqual(t)).Any())
                return;
            
            // Util.Alert("Zone has been edited!");
            System.Media.SystemSounds.Asterisk.Play();

            // Package the files into the permanent package file.
            byte[] raw = CTR.mini.packMini(data, "ZO");
            File.WriteAllBytes(filepaths[entry], raw);
        }

        // Loading of Data
        private void getZoneData()
        {
            L_ZDPreview.Text = "Text File: " + CurrentZone.ZD.TextFile
            + Environment.NewLine + "Map File: " + CurrentZone.ZD.MapMatrix;

            L_ZD.Text = string.Format("X: {0,5}{3}Y: {1,5}{3}Z:{2,6}{3}{3}X: {4,5}{3}Y: {5,5}{3}Z:{6,6}", CurrentZone.ZD.pX, CurrentZone.ZD.pY,
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
            RTB_F.Text = RTB_N.Text = RTB_W.Text = RTB_T1.Text = RTB_T2.Text = string.Empty;
            fEntry = nEntry = wEntry = tEntry = uEntry = -1;
            // Set Counters
            NUD_FurnCount.Value = CurrentZone.Entities.FurnitureCount; changeFurnitureCount(null, null);
            NUD_NPCCount.Value = CurrentZone.Entities.NPCCount; changeNPCCount(null, null);
            NUD_WarpCount.Value = CurrentZone.Entities.WarpCount; changeWarpCount(null, null);
            NUD_TrigCount.Value = CurrentZone.Entities.TriggerCount; changeTriggerCount(null, null);
            NUD_UnkCount.Value = CurrentZone.Entities.UnknownCount; changeUnkCount(null, null);

            // Collect/Load Data
            NUD_FE.Value = NUD_FE.Maximum < 0 ? -1 : 0; changeFurniture(null, null);
            NUD_NE.Value = NUD_NE.Maximum < 0 ? -1 : 0; changeNPC(null, null);
            NUD_WE.Value = NUD_WE.Maximum < 0 ? -1 : 0; changeWarp(null, null);
            NUD_TE.Value = NUD_TE.Maximum < 0 ? -1 : 0; changeTrigger1(null, null);
            NUD_UE.Value = NUD_UE.Maximum < 0 ? -1 : 0; changeTrigger2(null, null);

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
        private void setZoneData()
        {
            // Nothing, ZoneData is not currently researched enough.
        }
        private void setOWSData()
        {
            // Force all entities to be written back
            setFurniture();
            setNPC();
            setWarp();
            setTrigger1();
            setTrigger2();
        }

        // Overworld Viewing
        private int entry = -1;
        private int fEntry, nEntry, wEntry, tEntry, uEntry = -1;
        #region Enabling
        internal static void toggleEnable(NumericUpDown master, NumericUpDown slave, GroupBox display)
        {
            slave.Maximum = master.Value - 1;
            slave.Enabled = display.Visible = slave.Maximum > -1;
            slave.Minimum = slave.Enabled ? 0 : -1;
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

            toggleEnable(NUD_TrigCount, NUD_TE, GB_T1);
        }
        private void changeUnkCount(object sender, EventArgs e)
        {
            // Resize array
            int count = (int)NUD_UnkCount.Value;
            CurrentZone.Entities.UnknownCount = count;
            Array.Resize(ref CurrentZone.Entities.Triggers2, count);
            for (int i = 0; i < count; i++)
                CurrentZone.Entities.Triggers2[i] = CurrentZone.Entities.Triggers2[i] ?? new Zone.ZoneEntities.EntityTrigger2();

            toggleEnable(NUD_UnkCount, NUD_UE, GB_T2);
        }
        #endregion
        #region Updating
        private void changeFurniture(object sender, EventArgs e)
        {
            if (NUD_FE.Value < 0) return;
            setFurniture();
            fEntry = (int)NUD_FE.Value;
            getFurniture();
        }
        private void getFurniture()
        {
            if (NUD_FE.Value < 0) return;

            var Furniture = CurrentZone.Entities.Furniture[fEntry];
            NUD_FX.Value = Furniture.X;
            NUD_FY.Value = Furniture.Y;
            NUD_FWX.Value = Furniture.WX;
            NUD_FWY.Value = Furniture.WY;
            RTB_F.Text = Util.getHexString(Furniture.Raw);
        }
        private void setFurniture()
        {
            if (NUD_FE.Value < 0) return;
            if (fEntry < 0) return;

            var FUrniture = CurrentZone.Entities.Furniture[fEntry];
            FUrniture.X = (int)NUD_FX.Value;
            FUrniture.Y = (int)NUD_FY.Value;
            FUrniture.WX = (int)NUD_FWX.Value;
            FUrniture.WY = (int)NUD_FWY.Value;
        }

        private void changeNPC(object sender, EventArgs e)
        {
            if (NUD_NE.Value < 0) return;
            setNPC();
            nEntry = (int)NUD_NE.Value;
            getNPC();
        }
        private void getNPC()
        {
            if (NUD_NE.Value < 0) return;
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
            TB_Leash.Text = NPC.L1 == NPC.L2 && NPC.L2 == NPC.L3 && NPC.L3 == -1
                ? TB_Leash.Text = "No Leash!"
                : $"{NPC.L1}, {NPC.L2}, {NPC.L3} -- {NPC.LDir}";

            RTB_N.Text = Util.getHexString(NPC.Raw);
        }
        private void setNPC()
        {
            if (NUD_NE.Value < 0) return;
            if (nEntry < 0) return;

            var NPC = CurrentZone.Entities.NPCs[nEntry];
            NPC.ID = (int)NUD_NID.Value;
            NPC.Model = (int)NUD_NModel.Value;
            NPC.SpawnFlag = (int)NUD_NFlag.Value;
            NPC.Script = (int)NUD_NScript.Value;
            NPC.FaceDirection = (int)NUD_NFace.Value;
            NPC.SightRange = (int)NUD_NRange.Value;
            NPC.X = (int)NUD_NX.Value;
            NPC.Y = (int)NUD_NY.Value;

            NPC.MovePermissions = (int)NUD_NMove1.Value;
            NPC.MovePermissions2 = (int)NUD_NMove2.Value;
        }

        private void changeWarp(object sender, EventArgs e)
        {
            if (NUD_WE.Value < 0) return;
            setWarp();
            wEntry = (int)NUD_WE.Value;
            getWarp();
        }
        private void getWarp()
        {
            if (NUD_WE.Value < 0) return;

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
        private void setWarp()
        {
            if (NUD_WE.Value < 0) return;
            if (wEntry < 0) return;

            var Warp = CurrentZone.Entities.Warps[wEntry];
            Warp.DestinationMap = (int)NUD_WMap.Value;
            Warp.DestinationTileIndex = (int)NUD_WTile.Value;
            Warp.X = (int)NUD_WX.Value;
            Warp.Y = (int)NUD_WY.Value;
        }

        private void changeTrigger1(object sender, EventArgs e)
        {
            if (NUD_TE.Value < 0) return;
            setTrigger1();
            tEntry = (int)NUD_TE.Value;
            getTrigger1();
        }
        private void getTrigger1()
        {
            if (NUD_TE.Value < 0) return;

            var Trigger1 = CurrentZone.Entities.Triggers1[tEntry];
            NUD_T1X.Value = Trigger1.X;
            NUD_T1Y.Value = Trigger1.Y;
            RTB_T1.Text = Util.getHexString(Trigger1.Raw);
        }
        private void setTrigger1()
        {
            if (NUD_TE.Value < 0) return;
            if (tEntry < 0) return;

            var Trigger1 = CurrentZone.Entities.Triggers1[tEntry];
            Trigger1.X = (int)NUD_T1X.Value;
            Trigger1.Y = (int)NUD_T1Y.Value;
        }

        private void changeTrigger2(object sender, EventArgs e)
        {
            if (NUD_UE.Value < 0) return;
            setTrigger2();
            uEntry = (int)NUD_UE.Value;
            getTrigger2();
        }
        private void getTrigger2()
        {
            if (NUD_UE.Value < 0) return;

            // Load New Data
            var Trigger2 = CurrentZone.Entities.Triggers2[uEntry];
            NUD_T2X.Value = Trigger2.X;
            NUD_T2Y.Value = Trigger2.Y;
            RTB_T2.Text = Util.getHexString(Trigger2.Raw);
        }
        private void setTrigger2()
        {
            if (NUD_UE.Value < 0) return;
            if (uEntry < 0) return;

            var Trigger2 = CurrentZone.Entities.Triggers2[uEntry];
            Trigger2.X = (int)NUD_T2X.Value;
            Trigger2.Y = (int)NUD_T2Y.Value;
        }
        #endregion

        // Overworld User Enhancements
        private void changeNPC_ID(object sender, EventArgs e)
        {
            L_NID.ForeColor = NUD_NID.Value != NUD_NE.Value ? Color.Red : Color.Black;
        }
        private void changeNPC_Model(object sender, EventArgs e)
        {
            L_ModelAsHex.Text = "0x" + ((int)NUD_NModel.Value).ToString("X4");
        }
        private void dclickDestMap(object sender, EventArgs e)
        {
            var Tile = NUD_WTile.Value;
            CB_LocationID.SelectedIndex = (int)NUD_WMap.Value;
            try
            { NUD_WE.Value = Tile; }
            catch
            { try { NUD_WE.Value = 0; } catch { } }
        }
        private void changeWarp_X(object sender, EventArgs e)
        {
            L_WpX.Text = (NUD_WX.Value / 18).ToString();
        }
        private void changeWarp_Y(object sender, EventArgs e)
        {
            L_WpY.Text = (NUD_WY.Value / 18).ToString();
        }

        // Script Handling
        private void B_HLCMD_Click(object sender, EventArgs e)
        {
            int ctr = Util.highlightText(RTB_OSP, "**", Color.Red) + Util.highlightText(RTB_MSP, "**", Color.Red) / 2;
            Util.Alert($"{ctr} instance{(ctr > 1 ? "s" : "")} of \"*\" present.");
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

        // Dev Dumpers
        private void B_DumpFurniture_Click(object sender, EventArgs e)
        {
            if (Util.Prompt(MessageBoxButtons.YesNoCancel, "Export all Furniture?") != DialogResult.Yes)
                return;

            debugToolDumping = true;
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
            debugToolDumping = false;
        }
        private void B_DumpNPC_Click(object sender, EventArgs e)
        {
            if (Util.Prompt(MessageBoxButtons.YesNoCancel, "Export all NPCs?") != DialogResult.Yes)
                return;

            debugToolDumping = true;
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
            debugToolDumping = false;
        }
        private void B_DumpWarp_Click(object sender, EventArgs e)
        {
            if (Util.Prompt(MessageBoxButtons.YesNoCancel, "Export all Warps?") != DialogResult.Yes)
                return;

            debugToolDumping = true;
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
            debugToolDumping = false;
        }
        private void B_DumpTrigger_Click(object sender, EventArgs e)
        {
            if (Util.Prompt(MessageBoxButtons.YesNoCancel, "Export all Triggers?") != DialogResult.Yes)
                return;

            debugToolDumping = true;
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
            debugToolDumping = false;
        }
        private void B_DumpUnk_Click(object sender, EventArgs e)
        {
            if (Util.Prompt(MessageBoxButtons.YesNoCancel, "Export all Unks?") != DialogResult.Yes)
                return;

            debugToolDumping = true;
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
            debugToolDumping = false;
        }
        private void B_DumpMaps_Click(object sender, EventArgs e)
        {
            if (Util.Prompt(MessageBoxButtons.YesNoCancel, "Export all MapImages?") != DialogResult.Yes)
                return;

            debugToolDumping = true;
            const string folder = "MapImages";
            if (!Directory.Exists(folder))
                Directory.CreateDirectory(folder);

            string[] result = new string[CB_LocationID.Items.Count];
            for (int i = 0; i < CB_LocationID.Items.Count; i++)
            {
                mapView.DrawMap = i;
                Image img = mapView.getMapImage(crop: true);
                using (MemoryStream ms = new MemoryStream())
                {
                    //error will throw from here
                    img.Save(ms, System.Drawing.Imaging.ImageFormat.Png);
                    byte[] data = ms.ToArray();
                    File.WriteAllBytes(Path.Combine(folder, $"{zdLocations[i].Replace('?', '-')} ({i}).png"), data);
                }
                string l = mm.EntryList.Where(t => t != 0xFFFF).Aggregate("", (current, t) => current + t.ToString("000" + " "));
                result[i] = $"{i.ToString("000")}\t{CB_LocationID.Items[i]}\t{l}";
            }
            if (Util.Prompt(MessageBoxButtons.YesNoCancel, "Write Map parse output?") == DialogResult.Yes)
                File.WriteAllLines("MapLocations.txt", result);
            CB_LocationID.SelectedIndex = 0;
            Util.Alert("All Map images have been dumped to " + folder + ".");
            debugToolDumping = false;
        }
        private void B_DumpZD_Click(object sender, EventArgs e)
        {
            if (Util.Prompt(MessageBoxButtons.YesNoCancel, "Export all ZD?") != DialogResult.Yes)
                return;

            debugToolDumping = true;
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
            debugToolDumping = false;
        }

        // Raw file editing
        private void changeRAWCheck(object sender, EventArgs e)
        {
            bool chk = (sender as CheckBox).Checked;
            foreach (NumericUpDown nud in GB_F.Controls.OfType<NumericUpDown>())
                nud.Enabled = !chk;
            foreach (NumericUpDown nud in GB_N.Controls.OfType<NumericUpDown>())
                nud.Enabled = !chk;
            foreach (NumericUpDown nud in GB_W.Controls.OfType<NumericUpDown>())
                nud.Enabled = !chk;
            foreach (NumericUpDown nud in GB_T1.Controls.OfType<NumericUpDown>())
                nud.Enabled = !chk;
            foreach (NumericUpDown nud in GB_T2.Controls.OfType<NumericUpDown>())
                nud.Enabled = !chk;

            foreach (RichTextBox rtb in new[] {RTB_F, RTB_N, RTB_W, RTB_T1, RTB_T2})
                rtb.Visible = chk;
        }
        private void changeRAW_F(object sender, EventArgs e)
        {
            if (!(sender is RichTextBox) || !(sender as RichTextBox).Visible)
                return;

            try
            {
                byte[] data = Util.StringToByteArray((sender as RichTextBox).Text.Replace(Environment.NewLine, " ").Replace(" ", ""));
                if (data.Length != Zone.ZoneEntities.EntityFurniture.Size)
                    return;
                CurrentZone.Entities.Furniture[fEntry].Raw = data;
                getFurniture();
            }
            catch
            {
                (sender as RichTextBox).Text = Util.getHexString(CurrentZone.Entities.Furniture[fEntry].Raw);
            }
        }
        private void changeRAW_N(object sender, EventArgs e)
        {
            if (!(sender is RichTextBox) || !(sender as RichTextBox).Visible)
                return;

            try
            {
                byte[] data = Util.StringToByteArray((sender as RichTextBox).Text.Replace(Environment.NewLine, " ").Replace(" ", ""));
                if (data.Length != Zone.ZoneEntities.EntityNPC.Size)
                    return;
                CurrentZone.Entities.NPCs[nEntry].Raw = data;
                getNPC();
            }
            catch
            {
                (sender as RichTextBox).Text = Util.getHexString(CurrentZone.Entities.NPCs[nEntry].Raw);
            }
        }
        private void changeRAW_W(object sender, EventArgs e)
        {
            if (!(sender is RichTextBox) || !(sender as RichTextBox).Visible)
                return;

            try
            {
                byte[] data = Util.StringToByteArray((sender as RichTextBox).Text.Replace(Environment.NewLine, " ").Replace(" ", ""));
                if (data.Length != Zone.ZoneEntities.EntityWarp.Size)
                    return;
                CurrentZone.Entities.Warps[wEntry].Raw = data;
                getWarp();
            }
            catch
            {
                (sender as RichTextBox).Text = Util.getHexString(CurrentZone.Entities.Warps[wEntry].Raw);
            }
        }
        private void changeRAW_T1(object sender, EventArgs e)
        {
            if (!(sender is RichTextBox) || !(sender as RichTextBox).Visible)
                return;

            try
            {
                byte[] data = Util.StringToByteArray((sender as RichTextBox).Text.Replace(Environment.NewLine, " ").Replace(" ", ""));
                if (data.Length != Zone.ZoneEntities.EntityTrigger1.Size)
                    return;
                CurrentZone.Entities.Triggers1[tEntry].Raw = data;
                getTrigger1();
            }
            catch
            {
                (sender as RichTextBox).Text = Util.getHexString(CurrentZone.Entities.Triggers1[tEntry].Raw);
            }
        }
        private void changeRAW_T2(object sender, EventArgs e)
        {
            if (!(sender is RichTextBox) || !(sender as RichTextBox).Visible)
                return;

            try
            {
                byte[] data = Util.StringToByteArray((sender as RichTextBox).Text.Replace(Environment.NewLine, " ").Replace(" ",""));
                if (data.Length != Zone.ZoneEntities.EntityTrigger2.Size)
                    return;
                CurrentZone.Entities.Triggers2[uEntry].Raw = data;
                getTrigger2();
            }
            catch
            {
                (sender as RichTextBox).Text = Util.getHexString(CurrentZone.Entities.Triggers2[uEntry].Raw);
            }
        }

        // RAW Resets
        private void B_ResetOverworlds_Click(object sender, EventArgs e)
        {
            if (DialogResult.Yes != Util.Prompt(MessageBoxButtons.YesNo, "Reset all overworld entities?"))
                return;

            // since scripts are not editable, just reset the overworld file.
            CurrentZone.Entities = new Zone.ZoneEntities(locationData[1]);
            getOWSData();
        }
        private void B_ResetFurniture_Click(object sender, EventArgs e)
        {
            CurrentZone.Entities.Furniture[fEntry].OriginalData.CopyTo(CurrentZone.Entities.Furniture[fEntry].Raw, 0);
            getFurniture();
        }
        private void B_ResetNPC_Click(object sender, EventArgs e)
        {
            CurrentZone.Entities.NPCs[nEntry].OriginalData.CopyTo(CurrentZone.Entities.NPCs[nEntry].Raw, 0);
            getNPC();
        }
        private void B_ResetWarp_Click(object sender, EventArgs e)
        {
            CurrentZone.Entities.Warps[wEntry].OriginalData.CopyTo(CurrentZone.Entities.Warps[wEntry].Raw, 0);
            getWarp();
        }
        private void B_ResetTrigger1_Click(object sender, EventArgs e)
        {
            CurrentZone.Entities.Triggers1[tEntry].OriginalData.CopyTo(CurrentZone.Entities.Triggers1[tEntry].Raw, 0);
            getTrigger1();
        }
        private void B_ResetTrigger2_Click(object sender, EventArgs e)
        {
            CurrentZone.Entities.Triggers2[uEntry].OriginalData.CopyTo(CurrentZone.Entities.Triggers2[uEntry].Raw, 0);
            getTrigger2();
        }
    }
}