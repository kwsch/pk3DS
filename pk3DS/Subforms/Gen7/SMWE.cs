using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using CTR;

namespace pk3DS
{
    public partial class SMWE : Form
    {
        public SMWE(lzGARCFile ed, lzGARCFile zd, lzGARCFile wd)
        {
            InitializeComponent();

            PB_DayIcon.Image = Properties.Resources.sun;
            PB_NightIcon.Image = Properties.Resources.moon;
            PB_DayIcon.SizeMode = PictureBoxSizeMode.CenterImage;
            PB_NightIcon.SizeMode = PictureBoxSizeMode.CenterImage;

            font = L_Location.Font;

            speciesList[0] = "(None)";
            for (int i = 0; i < locationList.Length; i++)
                locationList[i] = locationList[i].Replace("\r", "");

            var metSM_00000 = locationList;

            var metSM_00000_good = (string[])metSM_00000.Clone();
            for (int i = 0; i < metSM_00000.Length; i += 2)
            {
                var nextLoc = metSM_00000[i + 1];
                if (!string.IsNullOrWhiteSpace(nextLoc) && nextLoc[0] != '[')
                    metSM_00000_good[i] += $" ({nextLoc})";
                if (i > 0 && !string.IsNullOrWhiteSpace(metSM_00000_good[i]) && metSM_00000_good.Take(i - 1).Contains(metSM_00000_good[i]))
                    metSM_00000_good[i] += $" ({metSM_00000_good.Take(i - 1).Count(s => s == metSM_00000_good[i]) + 1})";
            }
            metSM_00000_good.CopyTo(metSM_00000, 0);

            metSM_00000.CopyTo(locationList, 0);

            nup_spec = new[]
            {
                new [] { NUP_Forme1, NUP_Forme2, NUP_Forme3, NUP_Forme4, NUP_Forme5, NUP_Forme6, NUP_Forme7, NUP_Forme8, NUP_Forme9, NUP_Forme10 },
                new [] { NUP_Forme11, NUP_Forme12, NUP_Forme13, NUP_Forme14, NUP_Forme15, NUP_Forme16, NUP_Forme17, NUP_Forme18, NUP_Forme19, NUP_Forme20 },
                new [] { NUP_Forme21, NUP_Forme22, NUP_Forme23, NUP_Forme24, NUP_Forme25, NUP_Forme26, NUP_Forme27, NUP_Forme28, NUP_Forme29, NUP_Forme30 },
                new [] { NUP_Forme31, NUP_Forme32, NUP_Forme33, NUP_Forme34, NUP_Forme35, NUP_Forme36, NUP_Forme37, NUP_Forme38, NUP_Forme39, NUP_Forme40 },
                new [] { NUP_Forme41, NUP_Forme42, NUP_Forme43, NUP_Forme44, NUP_Forme45, NUP_Forme46, NUP_Forme47, NUP_Forme48, NUP_Forme49, NUP_Forme50 },
                new [] { NUP_Forme51, NUP_Forme52, NUP_Forme53, NUP_Forme54, NUP_Forme55, NUP_Forme56, NUP_Forme57, NUP_Forme58, NUP_Forme59, NUP_Forme60 },
                new [] { NUP_Forme61, NUP_Forme62, NUP_Forme63, NUP_Forme64, NUP_Forme65, NUP_Forme66, NUP_Forme67, NUP_Forme68, NUP_Forme69, NUP_Forme70 },
                new [] { NUP_Forme71, NUP_Forme72, NUP_Forme73, NUP_Forme74, NUP_Forme75, NUP_Forme76, NUP_Forme77, NUP_Forme78, NUP_Forme79, NUP_Forme80 },
                new [] { NUP_WeatherForme1, NUP_WeatherForme2, NUP_WeatherForme3, NUP_WeatherForme4, NUP_WeatherForme5, NUP_WeatherForme6 }
            };
            cb_spec = new[]
            {
                new[] {CB_Enc1, CB_Enc2, CB_Enc3, CB_Enc4, CB_Enc5, CB_Enc6, CB_Enc7, CB_Enc8, CB_Enc9, CB_Enc10},
                new[] {CB_Enc11, CB_Enc12, CB_Enc13, CB_Enc14, CB_Enc15, CB_Enc16, CB_Enc17, CB_Enc18, CB_Enc19, CB_Enc20},
                new[] {CB_Enc21, CB_Enc22, CB_Enc23, CB_Enc24, CB_Enc25, CB_Enc26, CB_Enc27, CB_Enc28, CB_Enc29, CB_Enc30},
                new[] {CB_Enc31, CB_Enc32, CB_Enc33, CB_Enc34, CB_Enc35, CB_Enc36, CB_Enc37, CB_Enc38, CB_Enc39, CB_Enc40},
                new[] {CB_Enc41, CB_Enc42, CB_Enc43, CB_Enc44, CB_Enc45, CB_Enc46, CB_Enc47, CB_Enc48, CB_Enc49, CB_Enc50},
                new[] {CB_Enc51, CB_Enc52, CB_Enc53, CB_Enc54, CB_Enc55, CB_Enc56, CB_Enc57, CB_Enc58, CB_Enc59, CB_Enc60},
                new[] {CB_Enc61, CB_Enc62, CB_Enc63, CB_Enc64, CB_Enc65, CB_Enc66, CB_Enc67, CB_Enc68, CB_Enc69, CB_Enc70},
                new[] {CB_Enc71, CB_Enc72, CB_Enc73, CB_Enc74, CB_Enc75, CB_Enc76, CB_Enc77, CB_Enc78, CB_Enc79, CB_Enc80},
                new[] {CB_WeatherEnc1, CB_WeatherEnc2, CB_WeatherEnc3, CB_WeatherEnc4, CB_WeatherEnc5, CB_WeatherEnc6}
            };
            rate_spec = new[]
            {L_Rate1, L_Rate2, L_Rate3, L_Rate4, L_Rate5, L_Rate6, L_Rate7, L_Rate8, L_Rate9, L_Rate10};

            foreach (var cb_l in cb_spec) foreach (var cb in cb_l) { cb.Items.AddRange(speciesList); cb.SelectedIndex = 0; cb.SelectedIndexChanged += updateSpeciesForm; }
            foreach (var nup_l in nup_spec) foreach (var nup in nup_l) { nup.ValueChanged += updateSpeciesForm; }
            foreach (var l in rate_spec)
                l.Text = "0%";

            byte[][] zdfiles = zd.Files;
            worldData = zdfiles[1]; // 1.bin
            zoneData = zdfiles[0]; // dec_0.bin
            Zones = new Zone[zoneData.Length / 0x54];

            var Worlds = wd.Files.Select(f => mini.unpackMini(f, "WD")[0]).ToArray();
            for (int i = 0; i < Zones.Length; i++)
            {
                Zones[i] = new Zone(i) {WorldIndex = BitConverter.ToUInt16(worldData, i*0x2)};
                var World = Worlds[Zones[i].WorldIndex];
                var mappingOffset = BitConverter.ToInt32(World, 0x8);
                for (var ofs = mappingOffset; ofs < World.Length; ofs += 4)
                {
                    if (BitConverter.ToUInt16(World, ofs) != i)
                        continue;
                    Zones[i].AreaIndex = BitConverter.ToUInt16(World, ofs + 2);
                    break;
                }
            }

            encdata = ed;
            LoadData();
        }

        private Area[] Areas;
        private readonly Zone[] Zones;
        private readonly lzGARCFile encdata;

        private static readonly string[] speciesList = Main.getText(TextName.SpeciesNames);
        private static readonly string[] locationList = Main.getText(TextName.metlist_000000);
        private static byte[] zoneData;
        private static byte[] worldData;

        private static Font font;

        private readonly NumericUpDown[][] nup_spec;
        private readonly ComboBox[][] cb_spec;
        private readonly Label[] rate_spec;

        private bool loadingdata;

        private EncounterTable CurrentTable;

        private void LoadData()
        {
            loadingdata = true;
            int fileCount = encdata.FileCount;
            var numAreas = fileCount / 11;
            Areas = new Area[numAreas];
            for (int i = 0; i < numAreas; i++)
            {
                Areas[i] = new Area
                {
                    FileNumber = 9 + 11*i,
                    Zones = Zones.Where(z => z.AreaIndex == i).ToArray()
                };
                var md = encdata[Areas[i].FileNumber];
                if (md.Length > 0)
                {
                    byte[][] Tables = mini.unpackMini(md, "EA");
                    Areas[i].HasTables = Tables.Any(t => t.Length > 0);
                    if (Areas[i].HasTables)
                    {
                        foreach (var Table in Tables)
                        {
                            var DayTable = Table.Skip(4).Take(0x164).ToArray();
                            var NightTable = Table.Skip(0x168).ToArray();
                            Areas[i].Tables.Add(new EncounterTable(DayTable));
                            Areas[i].Tables.Add(new EncounterTable(NightTable));
                        }
                    }
                }
                else
                {
                    Areas[i].HasTables = false;
                }
            }
            Areas = Areas.OrderBy(a => a.Zones[0].Name).ToArray();

            CB_LocationID.Items.Clear();
            CB_LocationID.Items.AddRange(Areas.Select(a => a.Name).ToArray());

            foreach (Control ctrl in Controls)
                ctrl.Enabled = true;
            B_Randomize.Enabled = true; // Randomization: complete


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
                foreach (var Map in Areas)
                    sb.Append(Map);
                File.WriteAllText(sfd.FileName, sb.ToString());
            }
        }

        private void updateMap(object sender, EventArgs e)
        {
            loadingdata = true;
            CB_TableID.Items.Clear();
            if (Areas[CB_LocationID.SelectedIndex].HasTables)
            {
                for (int i = 0; i < Areas[CB_LocationID.SelectedIndex].Tables.Count; i += 2)
                {
                    CB_TableID.Items.Add($"{(i / 2) + 1} (Day)");
                    CB_TableID.Items.Add($"{(i / 2) + 1} (Night)");
                }
            }
            else
                CB_TableID.Items.Add("(None)");
            CB_TableID.SelectedIndex = 0;
            loadingdata = false;
            updatePanel(sender, e);
        }

        private void updatePanel(object sender, EventArgs e)
        {
            if (loadingdata)
                return;
            loadingdata = true;
            var Map = Areas[CB_LocationID.SelectedIndex];
            GB_Encounters.Enabled = Map.HasTables;
            if (!Map.HasTables)
            {
                loadingdata = false;
                return;
            }
            CurrentTable = new EncounterTable(Map.Tables[CB_TableID.SelectedIndex].Data);
            NUP_Min.Value = CurrentTable.MinLevel;
            NUP_Max.Minimum = CurrentTable.MinLevel;
            NUP_Max.Value = CurrentTable.MaxLevel;
            for (int slot = 0; slot < CurrentTable.Encounters.Length; slot++)
            for (int i = 0; i < CurrentTable.Encounters[slot].Length; i++)
            {
                var sl = CurrentTable.Encounters[slot];
                if (slot == 8)
                    sl = CurrentTable.AdditionalSOS;
                rate_spec[i].Text = $"{CurrentTable.Rates[i]}%";
                cb_spec[slot][i].SelectedIndex = (int)sl[i].Species;
                nup_spec[slot][i].Value = (int)sl[i].Forme;
            }
            loadingdata = false;

            int base_id = CB_TableID.SelectedIndex/2;
            base_id *= 2;
            PB_DayTable.Image = Map.Tables[base_id].GetTableImg();
            PB_NightTable.Image = Map.Tables[base_id + 1].GetTableImg();
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

            var cur_pb = CB_TableID.SelectedIndex%2 == 0 ? PB_DayTable : PB_NightTable;
            var cur_img = cur_pb.Image;
            
            object[][] source = sender is NumericUpDown ? (object[][])nup_spec : cb_spec;
            int table = Array.FindIndex(source, t => t.Contains(sender));
            int slot = Array.IndexOf(source[table], sender);

            var cb_l = cb_spec[table];
            var nup_l = nup_spec[table];
            if (table == 8)
            {
                CurrentTable.AdditionalSOS[slot].Species = (uint)cb_l[slot].SelectedIndex;
                CurrentTable.AdditionalSOS[slot].Forme = (uint)nup_l[slot].Value;
            }
            CurrentTable.Encounters[table][slot].Species = (uint)cb_l[slot].SelectedIndex;
            CurrentTable.Encounters[table][slot].Forme = (uint)nup_l[slot].Value;

            using (var g = Graphics.FromImage(cur_img))
            {
                int x = 40*slot;
                int y = 30*(table + 1);
                if (table == 8)
                {
                    x = 40*slot + 60;
                    y = 270;
                }
                var pnt = new Point(x, y);
                g.SetClip(new Rectangle(pnt.X, pnt.Y, 40, 30), CombineMode.Replace);
                g.Clear(Color.Transparent);

                var enc = CurrentTable.Encounters[table][slot];
                g.DrawImage(enc.Species == 0 ? Properties.Resources.empty : Util.getSprite((int)enc.Species, (int)enc.Forme, 0, 0), pnt);
            }

            cur_pb.Image = cur_img;
        }

        private void B_Save_Click(object sender, EventArgs e)
        {
            CurrentTable.Write();
            var area = Areas[CB_LocationID.SelectedIndex];
            area.Tables[CB_TableID.SelectedIndex] = CurrentTable;

            // Set data back to GARC
            encdata[area.FileNumber] = getMapData(area.Tables);
        }

        private void B_Export_Click(object sender, EventArgs e)
        {
            B_Save_Click(sender, e);

            Directory.CreateDirectory("encdata");
            foreach (var Map in Areas)
            {
                var packed = getMapData(Map.Tables);
                File.WriteAllBytes(Path.Combine("encdata", Map.FileNumber.ToString()), packed);
            }
            Util.Alert("Exported all tables!");
        }
        private byte[] getMapData(List<EncounterTable> tables)
        {
            byte[][] tabs = new byte[tables.Count/2][];
            for (int i = 0; i < tables.Count; i += 2)
            {
                tabs[i / 2] = new byte[4].Concat(tables[i].Data).Concat(tables[i + 1].Data).ToArray();
            }
            return mini.packMini(tabs, "EA");
        }

        private class Area
        {
            public string Name => string.Join(" / ", Zones.Select(z => z.Name));
            public int FileNumber;
            public bool HasTables;
            public readonly List<EncounterTable> Tables;
            public Zone[] Zones;

            public Area()
            {
                Tables = new List<EncounterTable>();
            }

            public override string ToString()
            {
                var sb = new StringBuilder();
                sb.AppendLine("==========");
                sb.AppendLine($"Map: {Name}");
                sb.AppendLine($"Tables: {Tables.Count / 2}");
                for (int i = 0; i < Tables.Count / 2; i++)
                {
                    sb.AppendLine($"Table {i+1} (Day):");
                    sb.AppendLine(Tables[i*2].ToString());
                    sb.AppendLine($"Table {i+1} (Night):");
                    sb.AppendLine(Tables[i*2 + 1].ToString());
                }
                sb.AppendLine("==========");
                return sb.ToString();
            }
        }

        internal class Zone
        {
            private readonly byte[] Data;
            private readonly int Index;

            public int WorldIndex;
            public int AreaIndex;
            public string Name => $"{Index.ToString("000")} - {locationList[BitConverter.ToUInt32(Data, 0x1C)]}";

            public Zone(int i)
            {
                Data = new byte[0x54];
                Array.Copy(zoneData, i * 0x54, Data, 0, 0x54);
                Index = i;
            }
        }

        internal class EncounterTable
        {
            public int MinLevel;
            public int MaxLevel;
            public readonly int[] Rates;
            public readonly Encounter[][] Encounters;
            public readonly Encounter[] AdditionalSOS;

            public readonly byte[] Data;

            public EncounterTable(byte[] t)
            {
                Rates = new int[10];
                Encounters = new Encounter[9][];
                MinLevel = t[0];
                MaxLevel = t[1];
                for (int i = 0; i < Rates.Length; i++)
                    Rates[i] = t[2 + i];
                for (int i = 0; i < Encounters.Length - 1; i++)
                {
                    Encounters[i] = new Encounter[10];
                    var ofs = 0xC + i * 4 * Encounters[i].Length;
                    for (int j = 0; j < Encounters[i].Length; j++)
                    {
                        Encounters[i][j] = new Encounter(BitConverter.ToUInt32(t, ofs + 4 * j));
                    }
                }
                AdditionalSOS = new Encounter[6];
                for (var i = 0; i < AdditionalSOS.Length; i++)
                {
                    AdditionalSOS[i] = new Encounter(BitConverter.ToUInt32(t, 0x14C + 4 * i));
                }
                Encounters[8] = AdditionalSOS;
                Data = (byte[])t.Clone();
            }

            public void Write()
            {
                Data[0] = (byte)MinLevel;
                Data[1] = (byte)MaxLevel;
                // TODO: Rate Editing?
                for (int i = 0; i < Encounters.Length - 1; i++)
                {
                    var ofs = 0xC + i * 4 * Encounters[i].Length;
                    for (int j = 0; j < Encounters[i].Length; j++)
                    {
                        BitConverter.GetBytes(Encounters[i][j].RawValue).CopyTo(Data, ofs + 4 * j);
                    }
                }
                for (int i = 0; i < AdditionalSOS.Length; i++)
                    BitConverter.GetBytes(AdditionalSOS[i].RawValue).CopyTo(Data, 0x14C + 4 * i);
            }

            public override string ToString()
            {
                var sb = new StringBuilder();
                for (int i = 0; i < Encounters.Length - 1; i++)
                {
                    var tn = "Encounters";
                    if (i != 0)
                        tn = "SOS Slot " + (i);
                    sb.Append($"{tn} (Levels {MinLevel}-{MaxLevel}): ");
                    var specToRate = new Dictionary<uint, int>();
                    var distincts = new List<Encounter>();
                    for (int j = 0; j < Encounters[i].Length; j++)
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
                sb.Append("Additional SOS encounters: ");
                sb.AppendLine(string.Join(", ", AdditionalSOS.Select(e => e.RawValue).Distinct().Select(e => new Encounter(e)).Select(e => e.ToString())));

                return sb.ToString();
            }

            public Bitmap GetTableImg()
            {
                var img = new Bitmap(10*40, 10*30);
                using (var g = Graphics.FromImage(img))
                {
                    g.TextRenderingHint = System.Drawing.Text.TextRenderingHint.SingleBitPerPixelGridFit;
                    for (int i = 0; i < Encounters.Length - 1; i++)
                        for (int j = 0; j < Encounters[i].Length; j++)
                            g.DrawImage((Encounters[i][j].Species == 0 ? Properties.Resources.empty : Util.getSprite((int)Encounters[i][j].Species, (int)Encounters[i][j].Forme, 0, 0)), new Point(40 * j, 30 * (i+1)));
                    for (int i = 0; i < Rates.Length; i++)
                        g.DrawString($"{Rates[i]}%", font, Brushes.Black, new PointF(40 * i + 10, 10));
                    g.DrawString("Weather: ", font, Brushes.Black, new PointF(10, 280));
                    for (int i = 0; i < AdditionalSOS.Length; i++)
                        g.DrawImage((AdditionalSOS[i].Species == 0 ? Properties.Resources.empty : Util.getSprite((int)AdditionalSOS[i].Species, (int)AdditionalSOS[i].Forme, 0, 0)), new Point(40*i + 60, 270));
                }
                return img;
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
                sb.Append(speciesList[Species]);
                if (Forme != 0)
                    sb.Append($" (Forme {Forme})");
                return sb.ToString();
            }
        }

        private void modifyLevels(object sender, EventArgs e)
        {
            // Disable Interface while modifying
            Enabled = false;

            // Calculate % diff we will apply to each level
            decimal leveldiff = (100 + NUD_LevelAmp.Value) / 100;

            // Cycle through each location to modify levels
            foreach (var Table in Areas.SelectMany(Map => Map.Tables))
            {
                Table.MinLevel = Math.Max(1, Math.Min(100, (int)(leveldiff * Table.MinLevel)));
                Table.MaxLevel = Math.Max(1, Math.Min(100, (int)(leveldiff * Table.MaxLevel)));
                Table.Write();
            }
            // Enable Interface... modification complete.
            Enabled = true;

            updatePanel(sender, e);
        }
        
        // Randomization
        private void B_Randomize_Click(object sender, EventArgs e)
        {
            if (Util.Prompt(MessageBoxButtons.YesNo, "Randomize all? Cannot undo.", "Double check Randomization settings.") != DialogResult.Yes) return;

            Enabled = false;

            // Calculate % diff we will apply to each level
            decimal leveldiff = (100 + NUD_LevelAmp.Value) / 100;

            // Nonrepeating List Start
            int[] sL = Randomizer.getSpeciesList(CHK_G1.Checked, CHK_G2.Checked, CHK_G3.Checked,
                CHK_G4.Checked, CHK_G5.Checked, CHK_G6.Checked, CHK_G7.Checked, CHK_L.Checked, CHK_E.Checked);

            int ctr = 0;

            foreach (var Map in Areas)
            {
                foreach (var Table in Map.Tables)
                {
                    if (CHK_Level.Checked)
                    {
                        Table.MinLevel = Math.Max(1, Math.Min(100, (int)(leveldiff * Table.MinLevel)));
                        Table.MaxLevel = Math.Max(1, Math.Min(100, (int)(leveldiff * Table.MaxLevel)));
                    }

                    foreach (var EncounterSet in Table.Encounters)
                    {
                        foreach (var encounter in EncounterSet)
                        {
                            // Only modify slots that're used.
                            if (encounter.Species == 0)
                                continue;

                            if (!CHK_BST.Checked)
                                encounter.Species = (uint) Randomizer.getRandomSpecies(ref sL, ref ctr);
                            else
                            {
                                var old_ind = Main.SpeciesStat[encounter.Species].FormeIndex((int)encounter.Species, (int)encounter.Forme);
                                int oldBST = Main.SpeciesStat[old_ind].BST;

                                int species = Randomizer.getRandomSpecies(ref sL, ref ctr);
                                int newBST = Main.SpeciesStat[species].BST;
                                while (!(newBST*4/5 < oldBST && newBST*6/5 > oldBST))
                                {
                                    species = Randomizer.getRandomSpecies(ref sL, ref ctr);
                                    newBST = Main.SpeciesStat[species].BST;
                                }
                                encounter.Species = (uint)species;
                            }
                            encounter.Forme = GetRandomForme((int)encounter.Species);
                        }
                    }

                    Table.Write();
                }
                encdata[Map.FileNumber] = getMapData(Map.Tables);
            }
            updatePanel(sender, e);
            Enabled = true;
            Util.Alert("Randomized!");
        }

        private uint GetRandomForme(int species)
        {
            if (Main.SpeciesStat[species].FormeCount <= 1)
                return 0;
            if (!Legal.Mega_ORAS.Contains((ushort) species) || CHK_MegaForm.Checked)
                return (uint) (Util.rnd32()%Main.SpeciesStat[species].FormeCount); // Slot-Random
            return 0;
        }

        private void CopySOS_Click(object sender, EventArgs e)
        {
            // first table is copied to all other tables except weather (last)
            for (int i = 1; i < nup_spec.Length - 1; i++)
            {
                for (int s = 0; s < nup_spec[i].Length; s++) // slot copy
                {
                    nup_spec[i][s].Value = nup_spec[0][s].Value;
                    cb_spec[i][s].SelectedIndex = cb_spec[0][s].SelectedIndex;
                }
            }

            System.Media.SystemSounds.Asterisk.Play();
        }
    }
}
