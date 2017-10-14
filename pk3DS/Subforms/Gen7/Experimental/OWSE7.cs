using System;
using System.Linq;
using System.Windows.Forms;

using pk3DS.Core;
using pk3DS.Core.CTR;
using pk3DS.Core.Structures;

namespace pk3DS
{
    public partial class OWSE7 : Form
    {
        private readonly lzGARCFile EncounterData;
        private readonly lzGARCFile WorldData;
        private readonly lzGARCFile ZoneData;
        public OWSE7(lzGARCFile ed, lzGARCFile zd, lzGARCFile wd)
        {
            EncounterData = ed;
            ZoneData = zd;
            WorldData = wd;

            locationList = Main.Config.getText(TextName.metlist_000000);
            locationList = SMWE.GetGoodLocationList(locationList);

            InitializeComponent();

            var zdFiles = ZoneData.Files;
            zoneData = zdFiles[0];
            worldData = zdFiles[1];
            loadData();
        }

        private readonly byte[] zoneData;
        private byte[] worldData;
        private readonly string[] locationList;
        
        private void loadData()
        {
            // get zonedata array
            var zd = ZoneData7.GetArray(zoneData);

            string[] locations = zd.Select((z, i) => $"{i:000} - {locationList[z.ParentMap]}").ToArray();
            CB_LocationID.Items.AddRange(locations);
            CB_LocationID.SelectedIndex = 0;
        }

        private void CB_LocationID_SelectedIndexChanged(object sender, EventArgs e)
        {
            setEntry();
            entry = CB_LocationID.SelectedIndex;
            getEntry();
        }

        private int entry = -1;
        private void setEntry()
        {
            if (entry < 0)
                return;

            // research only, no set
        }

        private bool loading = false;
        private World Map;
        private void getEntry()
        {
            Console.WriteLine($"Loading {CB_LocationID.Text}");
            int index = entry*11;
            // 00 - ED (???)
            // 01 - BG (???)
            // 02 - TR (???)
            // 03 - AC (???)
            // 04 - AS (???)
            // 05 - ???
            // 06 - AE (Area Environment)
            // 07 - ZS (Zone Script)
            // 08 - ZI (Zone Info)
            // 09 - EA (Encounter Area)
            // 10 - BG (???)

            if (index > EncounterData.FileCount)
            {
                Console.WriteLine("Out of range.");
                tabControl1.Visible = false;
                return;
            }
            tabControl1.Visible = true;

            Map = new World(EncounterData, entry);
            loading = true;

            NUD_7_Count.Maximum = Map.ZoneScripts.Length;
            NUD_7_Count.Value = Math.Min(Map.ZoneScripts.Length, 1);

            NUD_8_Count.Maximum = Map.ZoneInfoScripts.Length;
            NUD_8_Count.Value = Math.Min(Map.ZoneInfoScripts.Length, 1);
            loading = false;

            NUD_7_Count_ValueChanged(NUD_7_Count, null);
            NUD_8_Count_ValueChanged(NUD_8_Count, null);
        }

        private class World
        {
            private byte[][] _7;
            private byte[][] _8;

            private bool HasZS => _7 != null;
            private bool HasZI => _8 != null;
            public Script[] ZoneScripts; 
            public Script[] ZoneInfoScripts;

            public World(lzGARCFile garc, int worldID)
            {
                int index = worldID*11;
                _7 = mini.unpackMini(garc[index + 7], "ZS");
                _8 = mini.unpackMini(garc[index + 8], "ZI");

                ZoneScripts = HasZS ? _7.Select(arr => new Script(arr)).ToArray() : new Script[0];
                ZoneInfoScripts = HasZI ? _8.Select(arr => new Script(arr)).ToArray() : new Script[0];
            }
        }

        private void NUD_7_Count_ValueChanged(object sender, EventArgs e)
        {
            if (loading)
                return;

            bool vis = ((sender as NumericUpDown)?.Value ?? 0) != 0;
            RTB_7_Raw.Visible = RTB_7_Script.Visible = L_7_Info.Visible = RTB_7_Parse.Visible = vis;
            if (!vis)
                return;

            var script = Map.ZoneScripts[(int)NUD_7_Count.Value - 1];
            L_7_Count.Text = $"Files: {Map.ZoneScripts.Length}";
            RTB_7_Raw.Lines = Scripts.getHexLines(script.Raw, 16);
            RTB_7_Script.Lines = Scripts.getHexLines(script.DecompressedInstructions);
            RTB_7_Parse.Lines = script.ParseScript;

            string[] lines =
            {
                "Commands:" + Environment.NewLine + RTB_7_Script.Lines.Length,
                "CBytes:" + Environment.NewLine + script.CompressedBytes.Length,
            };
            L_7_Info.Text = string.Join(Environment.NewLine, lines);
        }
        private void NUD_8_Count_ValueChanged(object sender, EventArgs e)
        {
            if (loading)
                return;

            bool vis = ((sender as NumericUpDown)?.Value ?? 0) != 0;
            RTB_8_Raw.Visible = RTB_8_Script.Visible = L_8_Info.Visible = RTB_8_Parse.Visible = vis;
            if (!vis)
                return;

            var script = Map.ZoneInfoScripts[(int)NUD_8_Count.Value - 1];
            L_8_Count.Text = $"Files: {Map.ZoneInfoScripts.Length}";
            RTB_8_Raw.Lines = Scripts.getHexLines(script.Raw, 16);
            RTB_8_Script.Lines = Scripts.getHexLines(script.DecompressedInstructions);
            RTB_8_Parse.Lines = script.ParseScript;

            string[] lines =
            {
                "Commands:" + Environment.NewLine + RTB_8_Script.Lines.Length,
                "CBytes:" + Environment.NewLine + script.CompressedBytes.Length,
            };
            L_8_Info.Text = string.Join(Environment.NewLine, lines);
        }
    }
}
