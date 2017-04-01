using System;
using System.Linq;
using System.Windows.Forms;
using CTR;

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

            locationList = Main.getText(TextName.metlist_000000);
            locationList = SMWE.getGoodLocationList(locationList);

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
            ZoneData7[] zd = new ZoneData7[zoneData.Length / ZoneData7.SIZE];
            for (int i = 0; i < zd.Length; i++)
            {
                byte[] buff = new byte[ZoneData7.SIZE];
                Buffer.BlockCopy(zoneData, i*ZoneData7.SIZE, buff, 0, ZoneData7.SIZE);
                zd[i] = new ZoneData7(buff);
            }

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

            byte[][] files = new byte[11][];
            files[07] = EncounterData[index + 7];
            files[08] = EncounterData[index + 8];

            if (files[07].Length > 0)
            {
                byte[][] _7 = mini.unpackMini(EncounterData[index + 7], "ZS");
                Console.WriteLine($"7: {_7.Length}");
                L_7_Count.Text = "Files: " + _7.Length;
                RTB_7_Raw.Lines = Scripts.getHexLines(_7[0], 16);
                var s_7 = new Script(_7[0]);
                RTB_7_Script.Lines = Scripts.getHexLines(s_7.DecompressedInstructions);
            }
            else
                Console.WriteLine("7: None.");
            RTB_7_Raw.Visible = RTB_7_Script.Visible = L_7_Count.Visible = files[07].Length > 0;

            if (files[08].Length > 0)
            {
                byte[][] _8 = mini.unpackMini(EncounterData[index + 8], "ZI");
                Console.WriteLine($"8: {_8.Length}");
                L_8_Count.Text = "Files: " + _8.Length;
                RTB_8_Raw.Lines = Scripts.getHexLines(_8[0], 16);
                var s_8 = new Script(_8[0]);
                RTB_8_Script.Lines = Scripts.getHexLines(s_8.DecompressedInstructions);
            }
            else
                Console.WriteLine("8: None.");
            RTB_8_Raw.Visible = RTB_8_Script.Visible = L_8_Count.Visible = files[08].Length > 0;
        }
    }
}
