using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using CTR;

namespace pk3DS
{
    public partial class SMWE : Form
    {
        public SMWE()
        {
            InitializeComponent();
            for (int i = 0; i < 8; i++)
            {
                CB_SlotType.Items.Add($"Slot Type {i+1}");
            }

            speciesList[0] = "(None)";
            for (int i = 0; i < locationList.Length; i++)
                locationList[i] = locationList[i].Replace("\r", "");

            nup_spec = new[]
            {NUP_Forme1, NUP_Forme2, NUP_Forme3, NUP_Forme4, NUP_Forme5, NUP_Forme6, NUP_Forme7, NUP_Forme8, NUP_Forme9, NUP_Forme10};
            cb_spec = new[]
            {CB_Enc1, CB_Enc2, CB_Enc3, CB_Enc4, CB_Enc5, CB_Enc6, CB_Enc7, CB_Enc8, CB_Enc9, CB_Enc10};
            rate_spec = new[]
            {L_Rate1, L_Rate2, L_Rate3, L_Rate4, L_Rate5, L_Rate6, L_Rate7, L_Rate8, L_Rate9, L_Rate10};

            foreach (var cb in cb_spec) { cb.Items.AddRange(speciesList); cb.SelectedIndex = 0; cb.SelectedIndexChanged += updateSpeciesForm; }
            foreach (var nup in nup_spec) { nup.ValueChanged += updateSpeciesForm; }
            foreach (var l in rate_spec)
                l.Text = "0%";

            zoneData = File.ReadAllBytes(zdpaths[1]); // dec_0.bin
            LoadData("encdata");
        }

        private Map[] Maps;

        public static readonly string[] speciesList = Main.getText(TextName.SpeciesNames);
        public static readonly string[] locationList = Main.getText(TextName.metlist_000000);
        public static readonly string[] zdpaths = Directory.GetFiles("zonedata");
        public static byte[] zoneData;

        private readonly NumericUpDown[] nup_spec;
        private readonly ComboBox[] cb_spec;
        private readonly Label[] rate_spec;

        private bool loadingdata;

        private EncounterTable CurrentTable;

        public void LoadData(string dir)
        {
            loadingdata = true;
            var files = (new DirectoryInfo(dir).GetFiles());
            var NumMaps = files.Length/11;
            Maps = new Map[NumMaps];
            var numNames = new Dictionary<string, int>();
            for (int i = 0; i < NumMaps; i++)
            {
                Maps[i] = new Map();
                Maps[i].Name = locationList[BitConverter.ToUInt32(zoneData, 0x1C+i*0x54)];
                if (numNames.ContainsKey(Maps[i].Name))
                {
                    numNames[Maps[i].Name] += 1;
                    Maps[i].Name += $" ({numNames[Maps[i].Name]})";
                }
                else
                    numNames[Maps[i].Name] = 1;
                Maps[i].Name = $"{i.ToString("000")} - {Maps[i].Name}";
                Maps[i].FullPath = files[9 + 11*i].FullName;
                byte[][] Tables = mini.unpackMini(File.ReadAllBytes(Maps[i].FullPath), "EA");
                Maps[i].HasTables = Tables.Any(t => t.Length > 0);
                if (Maps[i].HasTables)
                {
                    foreach (var Table in Tables)
                    {
                        Maps[i].Tables.Add(new EncounterTable(Table));
                    }
                }
            }

            CB_LocationID.Items.Clear();
            CB_LocationID.Items.AddRange(Maps.Select(m => m.Name).ToArray());

            foreach (Control ctrl in Controls)
            {
                ctrl.Enabled = true;
            }
            B_Randomize.Enabled = false; // TODO: Randomization

            CB_LocationID.SelectedIndex = 0;
            loadingdata = false;
            updateMap(null, null);
        }

        private void DumpTables(object sender, EventArgs e)
        {
            using (var sfd = new SaveFileDialog())
            {
                sfd.FileName = "EncounterTables.txt";
                if (sfd.ShowDialog() != DialogResult.OK)
                    return;
                var sb = new StringBuilder();
                foreach (var Map in Maps)
                    sb.Append(Map.ToString());
                File.WriteAllText(sfd.FileName, sb.ToString());
            }
        }

        private void updateMap(object sender, EventArgs e)
        {
            loadingdata = true;
            CB_TableID.Items.Clear();
            CB_TableID.Items.AddRange(Enumerable.Range(1, Math.Max(Maps[CB_LocationID.SelectedIndex].Tables.Count, 1)).Select(i => i.ToString()).ToArray());
            CB_TableID.SelectedIndex = 0;
            CB_SlotType.SelectedIndex = 0;
            loadingdata = false;
            updatePanel(sender, e);
        }

        private void updatePanel(object sender, EventArgs e)
        {
            if (loadingdata)
                return;
            loadingdata = true;
            var Map = Maps[CB_LocationID.SelectedIndex];
            GB_Encounters.Enabled = Map.HasTables;
            if (!Map.HasTables)
            {
                loadingdata = false;
                return;
            }
            if (sender != CB_SlotType)
                CurrentTable = new EncounterTable(Map.Tables[CB_TableID.SelectedIndex].Data);
            NUP_Min.Value = CurrentTable.MinLevel;
            NUP_Max.Minimum = CurrentTable.MinLevel;
            NUP_Max.Value = CurrentTable.MaxLevel;
            var Slots = CurrentTable.Encounters[CB_SlotType.SelectedIndex];
            for (int i = 0; i < Slots.Length; i++)
            {
                rate_spec[i].Text = $"{CurrentTable.Rates[i]}%";
                cb_spec[i].SelectedIndex = (int)Slots[i].Species;
                nup_spec[i].Value = (int) Slots[i].Forme;
            }
            loadingdata = false;
        }

        private void updateMinMax(object sender, EventArgs e)
        {
            if (loadingdata)
                return;
            loadingdata = true;
            int min = (int) NUP_Min.Value;
            int max = (int) NUP_Max.Value;
            if (max < min)
            {
                max = min;
                NUP_Max.Value = max;
                NUP_Max.Minimum = min;
            }
            CurrentTable.MinLevel = min;
            CurrentTable.MaxLevel = max;
            loadingdata = false;
        }

        private void updateSpeciesForm(object sender, EventArgs e)
        {
            if (loadingdata)
                return;
                
            if (sender is ComboBox)
            {
                int slotid = Array.FindIndex(cb_spec, cb => cb == (ComboBox)sender);
                CurrentTable.Encounters[CB_SlotType.SelectedIndex][slotid].Species = (uint)cb_spec[slotid].SelectedIndex;
            }
                
            if (sender is NumericUpDown)
            {
                int slotid = Array.FindIndex(nup_spec, nup => nup == (NumericUpDown)sender);
                CurrentTable.Encounters[CB_SlotType.SelectedIndex][slotid].Forme = (uint)nup_spec[slotid].Value;
            }
        }

        private void B_Save_Click(object sender, EventArgs e)
        {
            CurrentTable.Write();
            Maps[CB_LocationID.SelectedIndex].Tables[CB_TableID.SelectedIndex] = CurrentTable;
        }

        private void B_Export_Click(object sender, EventArgs e)
        {
            B_Save_Click(sender, e);

            foreach (var Map in Maps)
            {
                var packed = mini.packMini(Map.Tables.Select(t => t.Data).ToArray(), "EA");
                File.WriteAllBytes(Map.FullPath, packed);
            }
            MessageBox.Show("Exported all tables!");
        }
    }

    internal class Map
    {
        public string Name;
        public string FullPath;
        public bool HasTables;
        public List<EncounterTable> Tables;

        public Map()
        {
            Tables = new List<EncounterTable>();
        }

        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.AppendLine("==========");
            sb.AppendLine($"Map: {Name}");
            sb.AppendLine($"Tables: {Tables.Count}");
            for (int i = 0; i < Tables.Count; i++)
            {
                sb.AppendLine($"Table {i+1}:");
                sb.AppendLine(Tables[i].ToString());
            }
            sb.AppendLine("==========");
            return sb.ToString();
        }
    }

    internal class EncounterTable
    {
        public int MinLevel;
        public int MaxLevel;
        public int[] Rates;
        public Encounter[][] Encounters;

        public byte[] Data;

        public EncounterTable(byte[] t)
        {
            // Tables are stored twice. Validate. Throw error if I need more research
            for (var ofs = 0; ofs < 0x150; ofs += 4)
            {
                if (BitConverter.ToUInt32(t, ofs) != BitConverter.ToUInt32(t, 0x164+ofs))
                    throw new ArgumentException("Mismatched duplicate table. More research is needed. Contact SciresM at ProjectPokemon.org.");
            }
            Rates = new int[10];
            Encounters = new Encounter[8][];
            MinLevel = t[4];
            MaxLevel = t[5];
            for (int i = 0; i < Rates.Length; i++)
                Rates[i] = t[6 + i];
            for (int i = 0; i < Encounters.Length; i++)
            {
                Encounters[i] = new Encounter[10];
                var ofs = 0x10 + i*4*Encounters[i].Length;
                for (int j = 0; j < Encounters[i].Length; j++)
                {
                    Encounters[i][j] = new Encounter(BitConverter.ToUInt32(t, ofs + 4 * j));
                }
            }
            Data = (byte[])t.Clone();
        }

        public void Write()
        {
            Data[4] = (byte) MinLevel;
            Data[5] = (byte) MaxLevel;
            // TODO: Rate Editing?
            for (int i = 0; i < Encounters.Length; i++)
            {
                var ofs = 0x10 + i*4*Encounters[i].Length;
                for (int j = 0; j < Encounters[i].Length; j++)
                {
                    BitConverter.GetBytes(Encounters[i][j].RawValue).CopyTo(Data, ofs + 4*j);
                }
            }
            // Duplicate table.
            Array.Copy(Data, 0, Data, 0x164, 0x150);
        }

        public override string ToString()
        {
            var sb = new StringBuilder();
            for (int i = 0; i < Encounters.Length; i++)
            {
                sb.Append($"Slot Type {i + 1} (Levels {MinLevel}-{MaxLevel}): ");
                var specToRate = new Dictionary<uint, int>();
                var distincts = new List<Encounter>();
                for(int j = 0; j < Encounters[i].Length; j++)
                {
                    var encounter = Encounters[i][j];
                    if (!specToRate.ContainsKey(encounter.RawValue))
                    {
                        specToRate[encounter.RawValue] = 0;
                        distincts.Add(encounter);
                    }
                    specToRate[encounter.RawValue] += Rates[j];
                }
                distincts = distincts.OrderBy(e => specToRate[e.RawValue]).Reverse().ToList();
                sb.AppendLine(string.Join(", ", distincts.Select(e => $"{e.ToString()} ({specToRate[e.RawValue]}%)")));
            }

            return sb.ToString();
        }
    }

    internal class Encounter
    {
        public uint Species;
        public uint Forme;

        public uint RawValue => Species | (Forme << 11);

        public Encounter()
        {
            Species = 0;
            Forme = 0;
        }

        public Encounter(uint val)
        {
            Species = val & 0x7FF;
            Forme = (val >> 11) & 0x1F;
        }

        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.Append(SMWE.speciesList[Species]);
            if (Forme != 0)
                sb.Append($" (Forme {Forme})");
            return sb.ToString();
        }
    }
}
