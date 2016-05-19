using System;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace pk3DS
{
    public partial class XYWE : Form
    {
        public XYWE()
        {
            InitializeComponent();
            spec = new[] 
            {
                CB_Grass1, CB_Grass2, CB_Grass3, CB_Grass4, CB_Grass5, CB_Grass6, CB_Grass7, CB_Grass8, CB_Grass9, CB_Grass10, CB_Grass11, CB_Grass12,
                CB_Yellow1, CB_Yellow2, CB_Yellow3, CB_Yellow4, CB_Yellow5, CB_Yellow6, CB_Yellow7, CB_Yellow8, CB_Yellow9, CB_Yellow10, CB_Yellow11, CB_Yellow12,
                CB_Purple1, CB_Purple2, CB_Purple3, CB_Purple4, CB_Purple5, CB_Purple6, CB_Purple7, CB_Purple8, CB_Purple9, CB_Purple10, CB_Purple11, CB_Purple12,
                CB_Red1, CB_Red2, CB_Red3, CB_Red4, CB_Red5, CB_Red6, CB_Red7, CB_Red8, CB_Red9, CB_Red10, CB_Red11, CB_Red12,
                CB_RT1, CB_RT2, CB_RT3, CB_RT4, CB_RT5, CB_RT6, CB_RT7, CB_RT8, CB_RT9, CB_RT10, CB_RT11, CB_RT12,
                CB_Surf1, CB_Surf2, CB_Surf3, CB_Surf4, CB_Surf5, 
                CB_RockSmash1, CB_RockSmash2, CB_RockSmash3, CB_RockSmash4, CB_RockSmash5, 
                CB_Old1, CB_Old2, CB_Old3, 
                CB_Good1, CB_Good2, CB_Good3,
                CB_Super1, CB_Super2, CB_Super3,
                CB_HordeA1, CB_HordeA2, CB_HordeA3, CB_HordeA4, CB_HordeA5,
                CB_HordeB1, CB_HordeB2, CB_HordeB3, CB_HordeB4, CB_HordeB5,
                CB_HordeC1, CB_HordeC2, CB_HordeC3, CB_HordeC4, CB_HordeC5,
            };
            min = new[]
            {
                NUP_GrassMin1, NUP_GrassMin2, NUP_GrassMin3, NUP_GrassMin4, NUP_GrassMin5, NUP_GrassMin6, NUP_GrassMin7, NUP_GrassMin8, NUP_GrassMin9, NUP_GrassMin10, NUP_GrassMin11, NUP_GrassMin12,
                NUP_YellowMin1, NUP_YellowMin2, NUP_YellowMin3, NUP_YellowMin4, NUP_YellowMin5, NUP_YellowMin6, NUP_YellowMin7, NUP_YellowMin8, NUP_YellowMin9, NUP_YellowMin10, NUP_YellowMin11, NUP_YellowMin12,
                NUP_PurpleMin1, NUP_PurpleMin2, NUP_PurpleMin3, NUP_PurpleMin4, NUP_PurpleMin5, NUP_PurpleMin6, NUP_PurpleMin7, NUP_PurpleMin8, NUP_PurpleMin9, NUP_PurpleMin10, NUP_PurpleMin11, NUP_PurpleMin12,
                NUP_RedMin1, NUP_RedMin2, NUP_RedMin3, NUP_RedMin4, NUP_RedMin5, NUP_RedMin6, NUP_RedMin7, NUP_RedMin8, NUP_RedMin9, NUP_RedMin10, NUP_RedMin11, NUP_RedMin12,
                NUP_RTMin1, NUP_RTMin2, NUP_RTMin3, NUP_RTMin4, NUP_RTMin5, NUP_RTMin6, NUP_RTMin7, NUP_RTMin8, NUP_RTMin9, NUP_RTMin10, NUP_RTMin11, NUP_RTMin12,
                NUP_SurfMin1, NUP_SurfMin2, NUP_SurfMin3, NUP_SurfMin4, NUP_SurfMin5, 
                NUP_RockSmashMin1, NUP_RockSmashMin2, NUP_RockSmashMin3, NUP_RockSmashMin4, NUP_RockSmashMin5, 
                NUP_OldMin1, NUP_OldMin2, NUP_OldMin3, 
                NUP_GoodMin1, NUP_GoodMin2, NUP_GoodMin3,
                NUP_SuperMin1, NUP_SuperMin2, NUP_SuperMin3,
                NUP_HordeAMin1, NUP_HordeAMin2, NUP_HordeAMin3, NUP_HordeAMin4, NUP_HordeAMin5,
                NUP_HordeBMin1, NUP_HordeBMin2, NUP_HordeBMin3, NUP_HordeBMin4, NUP_HordeBMin5,
                NUP_HordeCMin1, NUP_HordeCMin2, NUP_HordeCMin3, NUP_HordeCMin4, NUP_HordeCMin5,
            };
            max = new[]
            {
                NUP_GrassMax1, NUP_GrassMax2, NUP_GrassMax3, NUP_GrassMax4, NUP_GrassMax5, NUP_GrassMax6, NUP_GrassMax7, NUP_GrassMax8, NUP_GrassMax9, NUP_GrassMax10, NUP_GrassMax11, NUP_GrassMax12,
                NUP_YellowMax1, NUP_YellowMax2, NUP_YellowMax3, NUP_YellowMax4, NUP_YellowMax5, NUP_YellowMax6, NUP_YellowMax7, NUP_YellowMax8, NUP_YellowMax9, NUP_YellowMax10, NUP_YellowMax11, NUP_YellowMax12,
                NUP_PurpleMax1, NUP_PurpleMax2, NUP_PurpleMax3, NUP_PurpleMax4, NUP_PurpleMax5, NUP_PurpleMax6, NUP_PurpleMax7, NUP_PurpleMax8, NUP_PurpleMax9, NUP_PurpleMax10, NUP_PurpleMax11, NUP_PurpleMax12,
                NUP_RedMax1, NUP_RedMax2, NUP_RedMax3, NUP_RedMax4, NUP_RedMax5, NUP_RedMax6, NUP_RedMax7, NUP_RedMax8, NUP_RedMax9, NUP_RedMax10, NUP_RedMax11, NUP_RedMax12,
                NUP_RTMax1, NUP_RTMax2, NUP_RTMax3, NUP_RTMax4, NUP_RTMax5, NUP_RTMax6, NUP_RTMax7, NUP_RTMax8, NUP_RTMax9, NUP_RTMax10, NUP_RTMax11, NUP_RTMax12, 
                NUP_SurfMax1, NUP_SurfMax2, NUP_SurfMax3, NUP_SurfMax4, NUP_SurfMax5, 
                NUP_RockSmashMax1, NUP_RockSmashMax2, NUP_RockSmashMax3, NUP_RockSmashMax4, NUP_RockSmashMax5, 
                NUP_OldMax1, NUP_OldMax2, NUP_OldMax3, 
                NUP_GoodMax1, NUP_GoodMax2, NUP_GoodMax3,
                NUP_SuperMax1, NUP_SuperMax2, NUP_SuperMax3,
                NUP_HordeAMax1, NUP_HordeAMax2, NUP_HordeAMax3, NUP_HordeAMax4, NUP_HordeAMax5,
                NUP_HordeBMax1, NUP_HordeBMax2, NUP_HordeBMax3, NUP_HordeBMax4, NUP_HordeBMax5,
                NUP_HordeCMax1, NUP_HordeCMax2, NUP_HordeCMax3, NUP_HordeCMax4, NUP_HordeCMax5,
            };
            form = new[]
            {
                NUP_GrassForme1, NUP_GrassForme2, NUP_GrassForme3, NUP_GrassForme4, NUP_GrassForme5, NUP_GrassForme6, NUP_GrassForme7, NUP_GrassForme8, NUP_GrassForme9, NUP_GrassForme10, NUP_GrassForme11, NUP_GrassForme12,
                NUP_YellowForme1, NUP_YellowForme2, NUP_YellowForme3, NUP_YellowForme4, NUP_YellowForme5, NUP_YellowForme6, NUP_YellowForme7, NUP_YellowForme8, NUP_YellowForme9, NUP_YellowForme10, NUP_YellowForme11, NUP_YellowForme12,
                NUP_PurpleForme1, NUP_PurpleForme2, NUP_PurpleForme3, NUP_PurpleForme4, NUP_PurpleForme5, NUP_PurpleForme6, NUP_PurpleForme7, NUP_PurpleForme8, NUP_PurpleForme9, NUP_PurpleForme10, NUP_PurpleForme11, NUP_PurpleForme12,
                NUP_RedForme1, NUP_RedForme2, NUP_RedForme3, NUP_RedForme4, NUP_RedForme5, NUP_RedForme6, NUP_RedForme7, NUP_RedForme8, NUP_RedForme9, NUP_RedForme10, NUP_RedForme11, NUP_RedForme12,
                NUP_RTForme1, NUP_RTForme2, NUP_RTForme3, NUP_RTForme4, NUP_RTForme5, NUP_RTForme6, NUP_RTForme7, NUP_RTForme8, NUP_RTForme9, NUP_RTForme10, NUP_RTForme11, NUP_RTForme12,
                NUP_SurfForme1, NUP_SurfForme2, NUP_SurfForme3, NUP_SurfForme4, NUP_SurfForme5, 
                NUP_RockSmashForme1, NUP_RockSmashForme2, NUP_RockSmashForme3, NUP_RockSmashForme4, NUP_RockSmashForme5, 
                NUP_OldForme1, NUP_OldForme2, NUP_OldForme3, 
                NUP_GoodForme1, NUP_GoodForme2, NUP_GoodForme3,
                NUP_SuperForme1, NUP_SuperForme2, NUP_SuperForme3,
                NUP_HordeAForme1, NUP_HordeAForme2, NUP_HordeAForme3, NUP_HordeAForme4, NUP_HordeAForme5,
                NUP_HordeBForme1, NUP_HordeBForme2, NUP_HordeBForme3, NUP_HordeBForme4, NUP_HordeBForme5,
                NUP_HordeCForme1, NUP_HordeCForme2, NUP_HordeCForme3, NUP_HordeCForme4, NUP_HordeCForme5,
            };

            formlist = new[] { "Unown-A - 0",
            "Unown-B - 1",
            "Unown-C - 2",
            "Unown-D - 3",
            "Unown-E - 4",
            "Unown-F - 5",
            "Unown-G - 6",
            "Unown-H - 7",
            "Unown-I - 8",
            "Unown-J - 9",
            "Unown-K - 10",
            "Unown-L - 11",
            "Unown-M - 12",
            "Unown-N - 13",
            "Unown-O - 14",
            "Unown-P - 15",
            "Unown-Q - 16",
            "Unown-R - 17",
            "Unown-S - 18",
            "Unown-T - 19",
            "Unown-U - 20",
            "Unown-V - 21",
            "Unown-W - 22",
            "Unown-X - 23",
            "Unown-Y - 24",
            "Unown-Z - 25",
            "Unown-! - 26",
            "Unown-? - 27",
            "",
            "Castform-Normal - 0",
            "Castform-Sunny - 1",
            "Castform-Rainy - 2",
            "Castform-Snowy - 3",
            "",
            "Deoxys-Normal - 0",
            "Deoxys-Attack - 1",
            "Deoxys-Defense - 2",
            "Deoxys-Speed - 3",
            "",
            "Burmy-Plant Cloak - 0",
            "Burmy-Sandy Cloak - 1",
            "Burmy-Trash Cloak - 2",
            "",
            "Wormadam-Plant Cloak - 0",
            "Wormadam-Sandy Cloak - 1",
            "Wormadam-Trash Cloak - 2",
            "",
            "Cherrim-Overcast - 0",
            "Cherrim-Sunshine - 1",
            "",
            "Shellos-West Sea - 0",
            "Shellos-East Sea - 1",
            "",
            "Gastrodon-West Sea - 0",
            "Gastrodon-East Sea - 1",
            "",
            "Rotom-Normal - 0",
            "Rotom-Heat - 1",
            "Rotom-Wash - 2",
            "Rotom-Frost - 3",
            "Rotom-Fan - 4",
            "Rotom-Mow - 5",
            "",
            "Giratina-Altered - 0",
            "Giratina-Origin - 1",
            "",
            "Shaymin-Land - 0",
            "Shaymin-Sky - 1",
            "",
            "Arceus-Normal - 0",
            "Arceus-Fighting - 1",
            "Arceus-Flying - 2",
            "Arceus-Poison - 3",
            "Arceus-Ground - 4",
            "Arceus-Rock - 5",
            "Arceus-Bug - 6",
            "Arceus-Ghost - 7",
            "Arceus-Steel - 8",
            "Arceus-Fire - 9",
            "Arceus-Water - 10",
            "Arceus-Grass - 11",
            "Arceus-Electric - 12",
            "Arceus-Psychic - 13",
            "Arceus-Ice - 14",
            "Arceus-Dragon - 15",
            "Arceus-Dark - 16",
            "Arceus-Fairy - 17",
            "",
            "Basculin-Red-Striped - 0",
            "Basculin-Blue-Striped - 1",
            "",
            "Darmanitan-Standard Mode - 0",
            "Darmanitan-Zen Mode - 1",
            "",
            "Deerling-Spring - 0",
            "Deerling-Summer - 1",
            "Deerling-Autumn - 2",
            "Deerling-Winter - 3",
            "",
            "Sawsbuck-Spring - 0",
            "Sawsbuck-Summer - 1",
            "Sawsbuck-Autumn - 2",
            "Sawsbuck-Winter - 3",
            "",
            "Tornadus-Incarnate - 0",
            "Tornadus-Therian - 1",
            "",
            "Thundurus-Incarnate - 0",
            "Thundurus-Therian - 1",
            "",
            "Landorus-Incarnate - 0",
            "Landorus-Therian - 1",
            "",
            "Kyurem-Normal - 0",
            "Kyurem-White - 1",
            "Kyurem-Black - 2",
            "",
            "Keldeo-Usual - 0",
            "Keldeo-Resolution - 1",
            "",
            "Meloetta-Aria - 0",
            "Meloetta-Pirouette - 1",
            "",
            "Genesect-Normal - 0",
            "Genesect-Water - 1",
            "Genesect-Electric - 2",
            "Genesect-Fire - 3",
            "Genesect-Ice - 4",
            "",
            "Flabebe-Red - 0",
            "Flabebe-Yellow - 1",
            "Flabebe-Orange - 2",
            "Flabebe-Blue - 3",
            "Flabebe-White - 4",
            "",
            "Floette-Red - 0",
            "Floette-Yellow - 1",
            "Floette-Orange - 2",
            "Floette-Blue - 3",
            "Floette-Wite - 4",
            "Floette-Eternal - 5",
            "",
            "Florges-Red - 0",
            "Florges-Yellow - 1",
            "Florges-Orange - 2",
            "Florges-Blue - 3",
            "Florges-White - 4",
            "",
            "Furfrou- Natural - 0",
            "Furfrou- Heart - 1",
            "Furfrou- Star - 2",
            "Furfrou- Diamond - 3",
            "Furfrou- Deputante - 4",
            "Furfrou- Matron - 5",
            "Furfrou- Dandy - 6",
            "Furfrou- La Reine- 7",
            "Furfrou- Kabuki - 8",
            "Furfrou- Pharaoh - 9",
            "",
            "Aegislash- Shield - 0",
            "Aegislash- Blade - 0",
            "",
            "Vivillon-Icy Snow - 0",
            "Vivillon-Polar - 1",
            "Vivillon-Tundra - 2",
            "Vivillon-Continental  - 3",
            "Vivillon-Garden - 4",
            "Vivillon-Elegant - 5",
            "Vivillon-Meadow - 6",
            "Vivillon-Modern  - 7",
            "Vivillon-Marine - 8",
            "Vivillon-Archipelago - 9",
            "Vivillon-High-Plains - 10",
            "Vivillon-Sandstorm - 11",
            "Vivillon-River - 12",
            "Vivillon-Monsoon - 13",
            "Vivillon-Savannah  - 14",
            "Vivillon-Sun - 15",
            "Vivillon-Ocean - 16",
            "Vivillon-Jungle - 17",
            "Vivillon-Fancy - 18",
            "Vivillon-Poké Ball - 19",
            "",
            "Pumpkaboo/Gourgeist-Small - 0",
            "Pumpkaboo/Gourgeist-Average - 1",
            "Pumpkaboo/Gourgeist-Large - 2",
            "Pumpkaboo/Gourgeist-Super - 3",
            "",
            "Megas-Normal - 0",
            "Megas-Mega (X) - 1",
            "Megas-Mega (Y) - 2",
            };
            Load_XYWE();
            openQuick(Directory.GetFiles("encdata"));

            string[] personalList = Directory.GetFiles("personal");
            personal = new byte[personalList.Length][];
            for (int i = 0; i < personalList.Length; i++)
                personal[i] = File.ReadAllBytes("personal" + Path.DirectorySeparatorChar + i.ToString("000") + ".bin");
        }
        private readonly ComboBox[] spec;
        private readonly NumericUpDown[] min;
        private readonly NumericUpDown[] max;
        private readonly NumericUpDown[] form;
        string[] specieslist = { };
        readonly string[] formlist = { };
        string[] metXY_00000 = { };
        byte[] zonedata = { };
        string[] LocationNames = { };
        private string[] encdatapaths;
        private string[] filepaths;

        readonly byte[][] personal;

        private void Load_XYWE()
        {
            specieslist = Main.getText(Main.oras ? 98 : 80);
            specieslist[0] = "---";

            CB_FormeList.Items.AddRange(formlist);

            // Clear & Reset Data
            for (int i = 0; i < max.Length; i++)
            {
                spec[i].Items.Clear();
                foreach (string s in specieslist)
                    spec[i].Items.Add(s);
                spec[i].SelectedIndex = 0;
            }
        }

        private void openQuick(string[] encdata)
        {
            encdatapaths = encdata;
            Array.Sort(encdatapaths);
            filepaths = new string[encdatapaths.Length - 1];
            Array.Copy(encdatapaths, 1, filepaths, 0, filepaths.Length);
            metXY_00000 = Main.getText(Main.oras ? 90 : 72);
            zonedata = File.ReadAllBytes(encdatapaths[0]);
            LocationNames = new string[filepaths.Length];
            for (int f = 0; f < filepaths.Length; f++)
            {
                string name = Path.GetFileNameWithoutExtension(filepaths[f]);

                int LocationNum = Convert.ToInt16(name.Substring(4, name.Length - 4));
                int indNum = LocationNum * 56 + 0x1C;
                string LocationName = metXY_00000[zonedata[indNum] + 0x100 * (zonedata[indNum + 1] & 1)];
                LocationNames[f] = LocationNum.ToString("000") + " - " + LocationName;
            }
            CB_LocationID.DataSource = LocationNames;
            B_Save.Enabled = B_Dump.Enabled = B_Randomize.Enabled = true;
            CB_LocationID.Enabled = true;
            CB_LocationID_SelectedIndexChanged(null, null);
        }

        private bool hasData()
        {
            for (int i = 0; i < max.Length; i++)
            {
                if (spec[i].SelectedIndex > 0) { return true; }
                if (form[i].Value > 0) { return true; }
                if (min[i].Value > 0) { return true; }
                if (max[i].Value > 0) { return true; }
            }
            return false;
        }
        private void parse(byte[] ed)
        {
            // 12,12,12,12,12
            // 5,5
            // 3,3,3
            // 5,5,5,
            byte[] slot = new Byte[4];
            const int offset = 0x0;

            // read data into form
            for (int i = 0; i < max.Length; i++)
            {
                // Fetch Data
                Array.Copy(ed, offset + i * 4, slot, 0, 4);
                int[] data = pslot(slot);

                // Load Data
                spec[i].SelectedIndex = data[0];
                form[i].Value = data[1];
                min[i].Value = data[2];
                max[i].Value = data[3];
            }

            #if DUMPER
            int r = CB_LocationID.SelectedIndex * 56 + 0x1C;
            int loc = zonedata[r] + 0x100 * (zonedata[r + 1] & 1);
            byte[] edata = BitConverter.GetBytes((ushort)loc).Concat(ed).ToArray();

            if (!Directory.Exists("encounter_xy"))
                Directory.CreateDirectory("encounter_xy");
            File.WriteAllBytes(Path.Combine("encounter_xy", loc.ToString("000") + CB_LocationID.SelectedIndex.ToString("000") + ".bin"), edata);
            #endif
        }
        private int[] pslot(byte[] slot)
        {
            int index = BitConverter.ToUInt16(slot, 0) & 0x7FF;
            int form = BitConverter.ToUInt16(slot, 0) >> 11;
            int min = slot[2];
            int max = slot[3];
            int[] data = new int[4];
            data[0] = index;
            data[1] = form;
            data[2] = min;
            data[3] = max;
            return data;
        }
        private string parseslot(byte[] slot)
        {
            int index = BitConverter.ToUInt16(slot, 0) & 0x7FF;
            if (index == 0) return "";
            int form = BitConverter.ToUInt16(slot, 0) >> 11;
            int min = slot[2];
            int max = slot[3];
            string species = specieslist[index];
            if (form > 0) species += "-" + form;
            return species + ',' + min + ',' + max + ',';
        }

        private void CB_LocationID_SelectedIndexChanged(object sender, EventArgs e)
        {
            int f = CB_LocationID.SelectedIndex;
            FileStream InStream = File.OpenRead(filepaths[f]);
            BinaryReader br = new BinaryReader(InStream);
            br.BaseStream.Seek(0x10, SeekOrigin.Begin);
            int offset = br.ReadInt32() + 0x10;
            int length = (int)br.BaseStream.Length - offset;
            if (length < 0x178) //no encounters in this map
            {
                ClearData();
                return;
            }
            br.Close();

            byte[] filedata = File.ReadAllBytes(filepaths[f]);

            byte[] encounterdata = new Byte[0x178];
            Array.Copy(filedata, offset, encounterdata, 0, 0x178);
            parse(encounterdata);
        }
        private void ClearData()
        {
            for (int i = 0; i < max.Length; i++)
            {
                // Load Data
                spec[i].SelectedIndex = 0;
                form[i].Value = 0;
                min[i].Value = 0;
                max[i].Value = 0;
            }
        }
        private byte[] MakeSlotData(int species, int f, int lo, int hi)
        {
            byte[] data = new byte[4];
            Array.Copy(BitConverter.GetBytes(Convert.ToUInt16((Convert.ToUInt16(f) << 11) + Convert.ToUInt16(species))), 0, data, 0, 2);
            data[2] = (byte)lo;
            data[3] = (byte)hi;
            return data;
        }
        private byte[] MakeEncounterData()
        {
            byte[] ed = new byte[0x178];
            const int offset = 0x0;
            for (int i = 0; i < max.Length; i++)
            {
                byte[] data = MakeSlotData(spec[i].SelectedIndex, (int)form[i].Value, (int)min[i].Value, (int)max[i].Value);
                Array.Copy(data, 0, ed, offset + i * 4, 4);
            }
            return ed;
        }
        private byte[] ConcatArrays(byte[] b1, byte[] b2)
        {
            byte[] concat = new byte[b1.Length + b2.Length];
            Array.Copy(b1, 0, concat, 0, b1.Length);
            Array.Copy(b2, 0, concat, b1.Length, b2.Length);
            return concat;
        }

        private void B_Save_Click(object sender, EventArgs e)
        {
            int f = CB_LocationID.SelectedIndex;
            string filepath = filepaths[f];
            FileStream InStream = File.OpenRead(filepaths[f]);
            BinaryReader br = new BinaryReader(InStream);
            br.BaseStream.Seek(0x10, SeekOrigin.Begin);
            int offset = br.ReadInt32() + 0x10;
            br.Close();
            byte[] filedata = File.ReadAllBytes(filepaths[f]);
            byte[] preoffset;
            if (offset < filedata.Length)
            {
                preoffset = new byte[offset];
                Array.Copy(filedata, preoffset, offset);
            }
            else
            {
                preoffset = new byte[filedata.Length];
                Array.Copy(filedata, preoffset, filedata.Length);
                //overwrite offset so the game actually looks at the data
                Array.Copy(BitConverter.GetBytes(Convert.ToUInt32(filedata.Length)), 0, preoffset, 0x10, 4);
            }
            byte[] encdata = { };
            if (hasData()) { encdata = MakeEncounterData(); }
            byte[] newdata = ConcatArrays(preoffset, encdata);
            File.WriteAllBytes(filepath, newdata);
        }

        private void PreloadTabs(object sender, EventArgs e)
        {
            for (int i = 0; i < TabControl_EncounterData.TabPages.Count; i++)
            {
                TabControl_EncounterData.TabPages[i].Show();
                Console.WriteLine("Loading Tab " + (i + 1).ToString("0") + ".");
            }
            TabControl_EncounterData.TabPages[0].Show();
        }
        internal static Random rand = new Random();
        internal static uint rnd32()
        {
            return (uint)rand.Next(1 << 30) << 2 | (uint)rand.Next(1 << 2);
        }

        private void B_Randomize_Click(object sender, EventArgs e)
        {
            if (Util.Prompt(MessageBoxButtons.YesNo, "Randomize all?", "Cannot undo.") != DialogResult.Yes) return;

            Enabled = false;

            // Calculate % diff we will apply to each level
            decimal leveldiff = (100 + NUD_LevelAmp.Value) / 100;

            // Nonrepeating List Start
            int[] sL = Randomizer.getSpeciesList(CHK_G1.Checked, CHK_G2.Checked, CHK_G3.Checked,
                CHK_G4.Checked, CHK_G5.Checked, CHK_G6.Checked, CHK_L.Checked, CHK_E.Checked);

            int ctr = 0;

            for (int i = 0; i < CB_LocationID.Items.Count; i++) // for every location
            {
                CB_LocationID.SelectedIndex = i;
                if (!hasData()) continue; // Don't randomize if doesn't have data.

                // Assign Levels
                if (CHK_Level.Checked)
                    for (int l = 0; l < max.Length; l++)
                        min[l].Value = max[l].Value = max[l].Value <= 1 ? max[l].Value : Math.Max(1, Math.Min(100, (int)(leveldiff * max[l].Value)));

                for (int slot = 0; slot < max.Length; slot++)
                {
                    if (spec[slot].SelectedIndex == 0) continue;

                    int species = Randomizer.getRandomSpecies(ref sL, ref ctr);

                    if (CHK_BST.Checked)
                    {
                        int oldBST = personal[spec[slot].SelectedIndex].Take(6).Sum(b => (ushort)b);
                        int newBST = personal[species].Take(6).Sum(b => (ushort)b);
                        while (!(newBST * 4 / 5 < oldBST && newBST * 6 / 5 > oldBST))
                        { species = sL[rand.Next(1, sL.Length)]; newBST = personal[species].Take(6).Sum(b => (ushort)b); }
                    }

                    spec[slot].SelectedIndex = species;
                    setRandomForm(slot, spec[slot].SelectedIndex);
                }
                B_Save_Click(sender, e);
            }
            Enabled = true;
            Util.Alert("Randomized!");
        }
        private void setRandomForm(int slot, int species)
        {
            if (CHK_MegaForm.Checked && Main.SpeciesStat[species].FormeCount > 1 && Legal.Mega_XY.Contains((ushort)species))
                form[slot].Value = rnd32() % Main.SpeciesStat[species].FormeCount; // Slot-Random
            else if (species == 666 || species == 665 || species == 664) // Vivillon
                form[slot].Value = rnd32() % 20;
            else if (species == 386) // Deoxys
                form[slot].Value = rnd32() % 4;
            else if (species == 201) // Unown
                form[slot].Value = 31;
            else if (species == 550) // Basculin
                form[slot].Value = rnd32() % 2;
            else if (species == 412 || species == 413) // Wormadam
                form[slot].Value = rnd32() % 3;
            else if (species == 422 || species == 423) // Gastrodon
                form[slot].Value = rnd32() % 2;
            else if (species == 585 || species == 586) // Sawsbuck
                form[slot].Value = rnd32() % 4;
            else if (species == 669 || species == 671) // Flabebe/Florges
                form[slot].Value = rnd32() % 5;
            else if (species == 670) // Floette
                form[slot].Value = rnd32() % 6;
            else if (species == 710 || species == 711) // Pumpkaboo
                form[slot].Value = rnd32() % 4;
            else
                form[slot].Value = 0;
        }

        private void B_Dump_Click(object sender, EventArgs e)
        {
            string toret = "";
            for (int i = 0; i < 360; i++) //hardcoded map count. Yes, it's bad. No, I don't really care.
            {
                CB_LocationID.SelectedIndex = i;
                string tdata = GetEncDataString();
                toret += tdata;
            }
            SaveFileDialog savetxt = new SaveFileDialog {FileName = "Encounter Slots", Filter = "Text File|*.txt"};
            if (savetxt.ShowDialog() != DialogResult.OK) return;

            string path = savetxt.FileName;
            File.WriteAllText(path, toret);
        }

        private string GetEncDataString()
        {
            string toret = "======" + Environment.NewLine;
            toret += "Map " + CB_LocationID.Text + "" + Environment.NewLine;
            toret += "======" + Environment.NewLine;
            if (hasData())
            {
                toret += "Grass: " + CB_Grass1.Text + "(Level " + NUP_GrassMin1.Text + "), " + CB_Grass2.Text + "(Level " + NUP_GrassMin2.Text + "), " + CB_Grass3.Text + "(Level " + NUP_GrassMin3.Text + "), " + CB_Grass4.Text + "(Level " + NUP_GrassMin4.Text + "), " + CB_Grass5.Text + "(Level " + NUP_GrassMin5.Text + "), " + CB_Grass6.Text + "(Level " + NUP_GrassMin6.Text + "), " + CB_Grass7.Text + "(Level " + NUP_GrassMin7.Text + "), " + CB_Grass8.Text + "(Level " + NUP_GrassMin8.Text + "), " + CB_Grass9.Text + "(Level " + NUP_GrassMin9.Text + "), " + CB_Grass10.Text + "(Level " + NUP_GrassMin10.Text + "), " + CB_Grass11.Text + "(Level " + NUP_GrassMin11.Text + "), " + CB_Grass12.Text + "(Level " + NUP_GrassMin12.Text + ")" + Environment.NewLine;
                toret += "Yellow: " + CB_Yellow1.Text + "(Level " + NUP_YellowMin1.Text + "), " + CB_Yellow2.Text + "(Level " + NUP_YellowMin2.Text + "), " + CB_Yellow3.Text + "(Level " + NUP_YellowMin3.Text + "), " + CB_Yellow4.Text + "(Level " + NUP_YellowMin4.Text + "), " + CB_Yellow5.Text + "(Level " + NUP_YellowMin5.Text + "), " + CB_Yellow6.Text + "(Level " + NUP_YellowMin6.Text + "), " + CB_Yellow7.Text + "(Level " + NUP_YellowMin7.Text + "), " + CB_Yellow8.Text + "(Level " + NUP_YellowMin8.Text + "), " + CB_Yellow9.Text + "(Level " + NUP_YellowMin9.Text + "), " + CB_Yellow10.Text + "(Level " + NUP_YellowMin10.Text + "), " + CB_Yellow11.Text + "(Level " + NUP_YellowMin11.Text + "), " + CB_Yellow12.Text + "(Level " + NUP_YellowMin12.Text + ")" + Environment.NewLine;
                toret += "Purple: " + CB_Purple1.Text + "(Level " + NUP_PurpleMin1.Text + "), " + CB_Purple2.Text + "(Level " + NUP_PurpleMin2.Text + "), " + CB_Purple3.Text + "(Level " + NUP_PurpleMin3.Text + "), " + CB_Purple4.Text + "(Level " + NUP_PurpleMin4.Text + "), " + CB_Purple5.Text + "(Level " + NUP_PurpleMin5.Text + "), " + CB_Purple6.Text + "(Level " + NUP_PurpleMin6.Text + "), " + CB_Purple7.Text + "(Level " + NUP_PurpleMin7.Text + "), " + CB_Purple8.Text + "(Level " + NUP_PurpleMin8.Text + "), " + CB_Purple9.Text + "(Level " + NUP_PurpleMin9.Text + "), " + CB_Purple10.Text + "(Level " + NUP_PurpleMin10.Text + "), " + CB_Purple11.Text + "(Level " + NUP_PurpleMin11.Text + "), " + CB_Purple12.Text + "(Level " + NUP_PurpleMin12.Text + ")" + Environment.NewLine;
                toret += "Red: " + CB_Red1.Text + "(Level " + NUP_RedMin1.Text + "), " + CB_Red2.Text + "(Level " + NUP_RedMin2.Text + "), " + CB_Red3.Text + "(Level " + NUP_RedMin3.Text + "), " + CB_Red4.Text + "(Level " + NUP_RedMin4.Text + "), " + CB_Red5.Text + "(Level " + NUP_RedMin5.Text + "), " + CB_Red6.Text + "(Level " + NUP_RedMin6.Text + "), " + CB_Red7.Text + "(Level " + NUP_RedMin7.Text + "), " + CB_Red8.Text + "(Level " + NUP_RedMin8.Text + "), " + CB_Red9.Text + "(Level " + NUP_RedMin9.Text + "), " + CB_Red10.Text + "(Level " + NUP_RedMin10.Text + "), " + CB_Red11.Text + "(Level " + NUP_RedMin11.Text + "), " + CB_Red12.Text + "(Level " + NUP_RedMin12.Text + ")" + Environment.NewLine;
                toret += "RoughTerrain: " + CB_RT1.Text + "(Level " + NUP_RTMin1.Text + "), " + CB_RT2.Text + "(Level " + NUP_RTMin2.Text + "), " + CB_RT3.Text + "(Level " + NUP_RTMin3.Text + "), " + CB_RT4.Text + "(Level " + NUP_RTMin4.Text + "), " + CB_RT5.Text + "(Level " + NUP_RTMin5.Text + "), " + CB_RT6.Text + "(Level " + NUP_RTMin6.Text + "), " + CB_RT7.Text + "(Level " + NUP_RTMin7.Text + "), " + CB_RT8.Text + "(Level " + NUP_RTMin8.Text + "), " + CB_RT9.Text + "(Level " + NUP_RTMin9.Text + "), " + CB_RT10.Text + "(Level " + NUP_RTMin10.Text + "), " + CB_RT11.Text + "(Level " + NUP_RTMin11.Text + "), " + CB_RT12.Text + "(Level " + NUP_RTMin12.Text + ")" + Environment.NewLine;
                toret += "RockSmash: " + CB_RockSmash1.Text + "(Level " + NUP_RockSmashMin1.Text + "), " + CB_RockSmash2.Text + "(Level " + NUP_RockSmashMin2.Text + "), " + CB_RockSmash3.Text + "(Level " + NUP_RockSmashMin3.Text + "), " + CB_RockSmash4.Text + "(Level " + NUP_RockSmashMin4.Text + "), " + CB_RockSmash5.Text + "(Level " + NUP_RockSmashMin5.Text + ")" + Environment.NewLine;
                toret += "Old: " + CB_Old1.Text + "(Level " + NUP_OldMin1.Text + "), " + CB_Old2.Text + "(Level " + NUP_OldMin2.Text + "), " + CB_Old3.Text + "(Level " + NUP_OldMin3.Text + ")" + Environment.NewLine;
                toret += "Good: " + CB_Good1.Text + "(Level " + NUP_GoodMin1.Text + "), " + CB_Good2.Text + "(Level " + NUP_GoodMin2.Text + "), " + CB_Good3.Text + "(Level " + NUP_GoodMin3.Text + ")" + Environment.NewLine;
                toret += "Super: " + CB_Super1.Text + "(Level " + NUP_SuperMin1.Text + "), " + CB_Super2.Text + "(Level " + NUP_SuperMin2.Text + "), " + CB_Super3.Text + "(Level " + NUP_SuperMin3.Text + ")" + Environment.NewLine;
                toret += "Surf: " + CB_Surf1.Text + "(Level " + NUP_SurfMin1.Text + "), " + CB_Surf2.Text + "(Level " + NUP_SurfMin2.Text + "), " + CB_Surf3.Text + "(Level " + NUP_SurfMin3.Text + "), " + CB_Surf4.Text + "(Level " + NUP_SurfMin4.Text + "), " + CB_Surf5.Text + "(Level " + NUP_SurfMin5.Text + ")" + Environment.NewLine;
                toret += "HordeA: " + CB_HordeA1.Text + "(Level " + NUP_HordeAMin1.Text + "), " + CB_HordeA2.Text + "(Level " + NUP_HordeAMin2.Text + "), " + CB_HordeA3.Text + "(Level " + NUP_HordeAMin3.Text + "), " + CB_HordeA4.Text + "(Level " + NUP_HordeAMin4.Text + "), " + CB_HordeA5.Text + "(Level " + NUP_HordeAMin5.Text + ")" + Environment.NewLine;
                toret += "HordeB: " + CB_HordeB1.Text + "(Level " + NUP_HordeBMin1.Text + "), " + CB_HordeB2.Text + "(Level " + NUP_HordeBMin2.Text + "), " + CB_HordeB3.Text + "(Level " + NUP_HordeBMin3.Text + "), " + CB_HordeB4.Text + "(Level " + NUP_HordeBMin4.Text + "), " + CB_HordeB5.Text + "(Level " + NUP_HordeBMin5.Text + ")" + Environment.NewLine;
                toret += "HordeC: " + CB_HordeC1.Text + "(Level " + NUP_HordeCMin1.Text + "), " + CB_HordeC2.Text + "(Level " + NUP_HordeCMin2.Text + "), " + CB_HordeC3.Text + "(Level " + NUP_HordeCMin3.Text + "), " + CB_HordeC4.Text + "(Level " + NUP_HordeCMin4.Text + "), " + CB_HordeC5.Text + "(Level " + NUP_HordeCMin5.Text + ")" + Environment.NewLine;
                toret = toret.Replace("---(Level 1)", "None");
            }
            else
                toret += "No encounters found." + Environment.NewLine + Environment.NewLine;
            return toret;
        }

        private void modifyLevels(object sender, EventArgs e)
        {
            // Disable Interface while modifying
            Enabled = false;

            // Calculate % diff we will apply to each level
            decimal leveldiff = (100 + NUD_LevelAmp.Value) / 100;

            // Cycle through each location to modify levels
            for (int i = 0; i < CB_LocationID.Items.Count; i++) // for every location
            {
                // Load location
                CB_LocationID.SelectedIndex = i;

                // Amp Levels
                for (int l = 0; l < max.Length; l++)
                    min[l].Value = max[l].Value = max[l].Value <= 1 ? max[l].Value : Math.Max(1, Math.Min(100, (int)(leveldiff * max[l].Value)));

                // Save Changes
                B_Save_Click(sender, e);
            }
            // Enable Interface... modification complete.
            Enabled = true;
        }
    }
}