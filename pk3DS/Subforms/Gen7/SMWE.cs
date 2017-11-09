using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using pk3DS.Core;
using pk3DS.Core.CTR;
using pk3DS.Core.Randomizers;

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
            var locationList = Main.Config.getText(TextName.metlist_000000);
            locationList = GetGoodLocationList(locationList);

            nup_spec = LoadFormeNUD();
            cb_spec = LoadSpeciesComboBoxes();
            rate_spec = LoadRateNUD();

            encdata = ed;
            var areas = Area7.GetArray(ed, zd, wd, locationList);
            Areas = areas.OrderBy(a => a.Zones[0].Name).ToArray();

            LoadData();

            // ExportEncounters("um", "uu");
        }


        private NumericUpDown[] LoadRateNUD()
        {
            var list = new[] {NUP_Rate1, NUP_Rate2, NUP_Rate3, NUP_Rate4, NUP_Rate5, NUP_Rate6, NUP_Rate7, NUP_Rate8, NUP_Rate9, NUP_Rate10};
            foreach (var nup in list)
                nup.ValueChanged += UpdateEncounterRate;
            return list;
        }
        private ComboBox[][] LoadSpeciesComboBoxes()
        {
            var list = new[] {
                new[] {CB_Enc01, CB_Enc02, CB_Enc03, CB_Enc04, CB_Enc05, CB_Enc06, CB_Enc07, CB_Enc08, CB_Enc09, CB_Enc10},
                new[] {CB_Enc11, CB_Enc12, CB_Enc13, CB_Enc14, CB_Enc15, CB_Enc16, CB_Enc17, CB_Enc18, CB_Enc19, CB_Enc20},
                new[] {CB_Enc21, CB_Enc22, CB_Enc23, CB_Enc24, CB_Enc25, CB_Enc26, CB_Enc27, CB_Enc28, CB_Enc29, CB_Enc30},
                new[] {CB_Enc31, CB_Enc32, CB_Enc33, CB_Enc34, CB_Enc35, CB_Enc36, CB_Enc37, CB_Enc38, CB_Enc39, CB_Enc40},
                new[] {CB_Enc41, CB_Enc42, CB_Enc43, CB_Enc44, CB_Enc45, CB_Enc46, CB_Enc47, CB_Enc48, CB_Enc49, CB_Enc50},
                new[] {CB_Enc51, CB_Enc52, CB_Enc53, CB_Enc54, CB_Enc55, CB_Enc56, CB_Enc57, CB_Enc58, CB_Enc59, CB_Enc60},
                new[] {CB_Enc61, CB_Enc62, CB_Enc63, CB_Enc64, CB_Enc65, CB_Enc66, CB_Enc67, CB_Enc68, CB_Enc69, CB_Enc70},
                new[] {CB_Enc71, CB_Enc72, CB_Enc73, CB_Enc74, CB_Enc75, CB_Enc76, CB_Enc77, CB_Enc78, CB_Enc79, CB_Enc80},
                new[] {CB_WeatherEnc1, CB_WeatherEnc2, CB_WeatherEnc3, CB_WeatherEnc4, CB_WeatherEnc5, CB_WeatherEnc6}
            };
            foreach (var cb_l in list)
            foreach (var cb in cb_l)
            {
                cb.Items.AddRange(speciesList);
                cb.SelectedIndex = 0;
                cb.SelectedIndexChanged += UpdateSpeciesForm;
            }
            return list;
        }
        private NumericUpDown[][] LoadFormeNUD()
        {
            var list = new[] {
                new [] { NUP_Forme01, NUP_Forme02, NUP_Forme03, NUP_Forme04, NUP_Forme05, NUP_Forme06, NUP_Forme07, NUP_Forme08, NUP_Forme09, NUP_Forme10 },
                new [] { NUP_Forme11, NUP_Forme12, NUP_Forme13, NUP_Forme14, NUP_Forme15, NUP_Forme16, NUP_Forme17, NUP_Forme18, NUP_Forme19, NUP_Forme20 },
                new [] { NUP_Forme21, NUP_Forme22, NUP_Forme23, NUP_Forme24, NUP_Forme25, NUP_Forme26, NUP_Forme27, NUP_Forme28, NUP_Forme29, NUP_Forme30 },
                new [] { NUP_Forme31, NUP_Forme32, NUP_Forme33, NUP_Forme34, NUP_Forme35, NUP_Forme36, NUP_Forme37, NUP_Forme38, NUP_Forme39, NUP_Forme40 },
                new [] { NUP_Forme41, NUP_Forme42, NUP_Forme43, NUP_Forme44, NUP_Forme45, NUP_Forme46, NUP_Forme47, NUP_Forme48, NUP_Forme49, NUP_Forme50 },
                new [] { NUP_Forme51, NUP_Forme52, NUP_Forme53, NUP_Forme54, NUP_Forme55, NUP_Forme56, NUP_Forme57, NUP_Forme58, NUP_Forme59, NUP_Forme60 },
                new [] { NUP_Forme61, NUP_Forme62, NUP_Forme63, NUP_Forme64, NUP_Forme65, NUP_Forme66, NUP_Forme67, NUP_Forme68, NUP_Forme69, NUP_Forme70 },
                new [] { NUP_Forme71, NUP_Forme72, NUP_Forme73, NUP_Forme74, NUP_Forme75, NUP_Forme76, NUP_Forme77, NUP_Forme78, NUP_Forme79, NUP_Forme80 },
                new [] { NUP_WeatherForme1, NUP_WeatherForme2, NUP_WeatherForme3, NUP_WeatherForme4, NUP_WeatherForme5, NUP_WeatherForme6 }
            };

            foreach (var nup_l in list)
            foreach (var nup in nup_l)
                nup.ValueChanged += UpdateSpeciesForm;

            return list;
        }

        private readonly Area7[] Areas;
        private readonly lzGARCFile encdata;
        private readonly string[] speciesList = Main.Config.getText(TextName.SpeciesNames);
        private readonly Font font;
        private readonly NumericUpDown[][] nup_spec;
        private readonly ComboBox[][] cb_spec;
        private readonly NumericUpDown[] rate_spec;

        private int TotalEncounterRate => rate_spec.Sum(nup => (int)nup.Value);
        private bool loadingdata;
        private EncounterTable CurrentTable;

        private void LoadData()
        {
            loadingdata = true;

            CB_LocationID.Items.Clear();
            CB_LocationID.Items.AddRange(Areas.Select(a => a.Name).ToArray());

            CB_SlotRand.SelectedIndex = 0;
            CB_LocationID.SelectedIndex = 0;

            loadingdata = false;
            ChangeMap(null, null);
        }

        private void ChangeMap(object sender, EventArgs e)
        {
            loadingdata = true;
            CB_TableID.Items.Clear();
            if (Areas[CB_LocationID.SelectedIndex].HasTables)
            {
                for (int i = 0; i < Areas[CB_LocationID.SelectedIndex].Tables.Count; i += 2)
                {
                    CB_TableID.Items.Add($"{i / 2 + 1} (Day)");
                    CB_TableID.Items.Add($"{i / 2 + 1} (Night)");
                }
            }
            else
                CB_TableID.Items.Add("(None)");
            CB_TableID.SelectedIndex = 0;
            loadingdata = false;
            UpdatePanel(sender, e);
        }
        private void UpdatePanel(object sender, EventArgs e)
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
            for (int slot = 0; slot < CurrentTable.Encounter7s.Length; slot++)
            for (int i = 0; i < CurrentTable.Encounter7s[slot].Length; i++)
            {
                var sl = CurrentTable.Encounter7s[slot];
                if (slot == 8)
                    sl = CurrentTable.AdditionalSOS;
                rate_spec[i].Value = CurrentTable.Rates[i];
                cb_spec[slot][i].SelectedIndex = (int)sl[i].Species;
                nup_spec[slot][i].Value = (int)sl[i].Forme;
            }
            loadingdata = false;

            int base_id = CB_TableID.SelectedIndex/2;
            base_id *= 2;
            PB_DayTable.Image = Map.Tables[base_id].GetTableImg(font);
            PB_NightTable.Image = Map.Tables[base_id + 1].GetTableImg(font);
        }
        private void UpdateMinMax(object sender, EventArgs e)
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
        private void UpdateSpeciesForm(object sender, EventArgs e)
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
            CurrentTable.Encounter7s[table][slot].Species = (uint)cb_l[slot].SelectedIndex;
            CurrentTable.Encounter7s[table][slot].Forme = (uint)nup_l[slot].Value;

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

                var enc = CurrentTable.Encounter7s[table][slot];
                g.DrawImage(enc.Species == 0 ? Properties.Resources.empty : WinFormsUtil.getSprite((int)enc.Species, (int)enc.Forme, 0, 0, Main.Config), pnt);
            }

            cur_pb.Image = cur_img;
        }
        private void UpdateEncounterRate(object sender, EventArgs e)
        {
            if (loadingdata)
                return;
            
            var cur_pb = CB_TableID.SelectedIndex%2 == 0 ? PB_DayTable : PB_NightTable;
            var cur_img = cur_pb.Image;
            
            int slot = Array.IndexOf(rate_spec, sender);
            int rate = (int) ((NumericUpDown) sender).Value;
            CurrentTable.Rates[slot] = rate;
            
            using (var g = Graphics.FromImage(cur_img))
            {
                var pnt = new PointF(40 * slot + 10, 10);
                g.SetClip(new Rectangle((int) pnt.X, (int) pnt.Y, 40, 14), CombineMode.Replace);
                g.Clear(Color.Transparent);
                g.DrawString($"{rate}%", font, Brushes.Black, pnt);
            }
            
            cur_pb.Image = cur_img;

            var sum = TotalEncounterRate;
            GB_Encounters.Text = $"Encounters ({sum}%)";
        }

        private void B_Save_Click(object sender, EventArgs e)
        {
            var sum = TotalEncounterRate;
            if (sum != 100 && sum != 0)
            {
                WinFormsUtil.Error("Encounter rates must add up to either 0% or 100%.");
                return;
            }
            
            CurrentTable.Write();
            var area = Areas[CB_LocationID.SelectedIndex];
            area.Tables[CB_TableID.SelectedIndex] = CurrentTable;

            // Set data back to GARC
            encdata[area.FileNumber] = Area7.GetDayNightTableBinary(area.Tables);
        }
        private void B_Export_Click(object sender, EventArgs e)
        {
            B_Save_Click(sender, e);

            Directory.CreateDirectory("encdata");
            foreach (var Map in Areas)
            {
                var packed = Area7.GetDayNightTableBinary(Map.Tables);
                File.WriteAllBytes(Path.Combine("encdata", Map.FileNumber.ToString()), packed);
            }
            WinFormsUtil.Alert("Exported all tables!");
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
                    sb.Append(Map.GetSummary(speciesList));
                File.WriteAllText(sfd.FileName, sb.ToString());
            }
        }
        
        // Randomization & Bulk Modification
        private void B_Randomize_Click(object sender, EventArgs e)
        {
            if (DialogResult.Yes != WinFormsUtil.Prompt(MessageBoxButtons.YesNo, 
                "Randomize all? Cannot undo.", "Double check Randomization settings at the bottom left."))
                return;

            Enabled = false;
            ExecuteRandomization();
            UpdatePanel(null, null);
            Enabled = true;

            WinFormsUtil.Alert("Randomized all Wild Encounters according to specification!", "Press the Dump Tables button to view the new Wild Encounter information!");
        }
        private void ExecuteRandomization()
        {
            var rnd = new SpeciesRandomizer(Main.Config)
            {
                G1 = CHK_G1.Checked,
                G2 = CHK_G2.Checked,
                G3 = CHK_G3.Checked,
                G4 = CHK_G4.Checked,
                G5 = CHK_G5.Checked,
                G6 = CHK_G6.Checked,
                G7 = CHK_G7.Checked,

                E = CHK_E.Checked,
                L = CHK_L.Checked,
                rBST = CHK_BST.Checked,
            };
            rnd.Initialize();
            var form = new FormRandomizer(Main.Config)
            {
                AllowMega = CHK_MegaForm.Checked,
                AllowAlolanForm = true,
            };
            var wild7 = new Wild7Randomizer
            {
                RandSpec = rnd,
                RandForm = form,
                TableRandomizationOption = CB_SlotRand.SelectedIndex,
                LevelAmplifier = NUD_LevelAmp.Value,
                ModifyLevel = CHK_Level.Checked,
            };
            wild7.Execute(Areas, encdata);
        }
        private void CopySOS_Click(object sender, EventArgs e)
        {
            if (WinFormsUtil.Prompt(MessageBoxButtons.YesNo, "Copy initial species to SOS slots?", "Cannot undo.") != DialogResult.Yes)
                return;

            // first table is copied to all other tables except weather (last)
            for (int i = 1; i < nup_spec.Length - 1; i++)
            {
                for (int s = 0; s < nup_spec[i].Length; s++) // slot copy
                {
                    nup_spec[i][s].Value = nup_spec[0][s].Value;
                    cb_spec[i][s].SelectedIndex = cb_spec[0][s].SelectedIndex;
                }
            }
            WinFormsUtil.Alert("All initial species copied to SOS slots!");
        }
        private void ModifyAllLevelRanges(object sender, EventArgs e)
        {
            if (DialogResult.Yes != WinFormsUtil.Prompt(MessageBoxButtons.YesNo,
                    "Modify all current Level ranges?", "Cannot undo."))
                return;

            // Disable Interface while modifying
            Enabled = false;

            // Cycle through each location to modify levels
            foreach (var Table in Areas.SelectMany(Map => Map.Tables))
            {
                Table.MinLevel = Randomizer.getModifiedLevel(Table.MinLevel, NUD_LevelAmp.Value);
                Table.MaxLevel = Randomizer.getModifiedLevel(Table.MaxLevel, NUD_LevelAmp.Value);
                Table.Write();
            }
            // Enable Interface... modification complete.
            Enabled = true;
            WinFormsUtil.Alert("Modified all Level ranges according to specification!", "Press the Dump Tables button to view the new Level ranges!");

            UpdatePanel(sender, e);
        }

        // Utility
        /// <summary>
        /// Moves the sub-location names into the location name string entry.
        /// </summary>
        /// <param name="list">Raw location list</param>
        /// <returns>Cleaned location list</returns>
        public static string[] GetGoodLocationList(string[] list)
        {
            var bad = list;
            var good = (string[])bad.Clone();
            for (int i = 0; i < bad.Length; i += 2)
            {
                var nextLoc = bad[i + 1];
                if (!string.IsNullOrWhiteSpace(nextLoc) && nextLoc[0] != '[')
                    good[i] += $" ({nextLoc})";
                if (i > 0 && !string.IsNullOrWhiteSpace(good[i]) && good.Take(i - 1).Contains(good[i]))
                    good[i] += $" ({good.Take(i - 1).Count(s => s == good[i]) + 1})";
            }
            return good;
        }

        private void ExportEncounters(string gameID, string ident)
        {
            var reg = dumpreg();
            var sos = dumpsos();

            File.WriteAllBytes($"encounter_{gameID}.pkl", mini.packMini(reg, ident));
            File.WriteAllBytes($"encounter_{gameID}_sos.pkl", mini.packMini(sos, ident));
        }
        private byte[][] dumpreg()
        {
            var dict = new Dictionary<int, List<uint>>();
            foreach (var area in Areas)
            {
                foreach (var z in area.Zones)
                {
                    int loc = z.ParentMap;
                    if (!dict.ContainsKey(loc))
                        dict.Add(loc, new List<uint>());

                    var table = dict[loc];
                    table.AddRange(from t in area.Tables
                        from s in t.Encounter7s.Take(1)
                        from e in s
                        select e.RawValue | (uint)(t.MinLevel << 16) | (uint)(t.MaxLevel << 24));
                }
            }

            return GetLocationDump(dict).ToArray();
        }
        private byte[][] dumpsos()
        {
            var dict = new Dictionary<int, List<uint>>();
            foreach (var area in Areas)
            {
                foreach (var z in area.Zones)
                {
                    int loc = z.ParentMap;
                    if (!dict.ContainsKey(loc))
                        dict.Add(loc, new List<uint>());

                    var table = dict[loc];
                    table.AddRange(from t in area.Tables from s in t.Encounter7s.Skip(1)
                                   from e in s
                                   select e.RawValue | (uint) (t.MinLevel << 16) | (uint) (t.MaxLevel << 24));

                    table.AddRange(from t in area.Tables
                                   from e in t.AdditionalSOS
                                   select e.RawValue | (uint) (t.MinLevel << 16) | (uint) (t.MaxLevel << 24));
                }
            }

            return GetLocationDump(dict).ToArray();
        }

        private IEnumerable<byte[]> GetLocationDump(Dictionary<int, List<uint>> dict)
        {
            foreach (var z in dict.OrderBy(z => z.Key))
            {
                using (var ms = new MemoryStream())
                using (var bw = new BinaryWriter(ms))
                {
                    bw.Write((ushort)z.Key);
                    foreach (var s in z.Value.Distinct())
                        bw.Write(s);
                    yield return ms.ToArray();
                }
            }
        }
    }

    public static partial class Extensions
    {
        public static Bitmap GetTableImg(this EncounterTable table, Font font)
        {
            var img = new Bitmap(10 * 40, 10 * 30);
            using (var g = Graphics.FromImage(img))
            {
                g.TextRenderingHint = System.Drawing.Text.TextRenderingHint.SingleBitPerPixelGridFit;
                for (int i = 0; i < table.Rates.Length; i++)
                    g.DrawString($"{table.Rates[i]}%", font, Brushes.Black, new PointF(40 * i + 10, 10));
                g.DrawString("Weather: ", font, Brushes.Black, new PointF(10, 280));

                // Draw Sprites
                for (int i = 0; i < table.Encounter7s.Length - 1; i++)
                for (int j = 0; j < table.Encounter7s[i].Length; j++)
                {
                    var slot = table.Encounter7s[i][j];
                    var sprite = GetSprite((int)slot.Species, (int)slot.Forme);
                    g.DrawImage(sprite, new Point(40 * j, 30 * (i + 1)));
                }
                for (int i = 0; i < table.AdditionalSOS.Length; i++)
                {
                    var slot = table.AdditionalSOS[i];
                    var sprite = GetSprite((int)slot.Species, (int)slot.Forme);
                    g.DrawImage(sprite, new Point(40 * i + 60, 270));
                }
            }

            Bitmap GetSprite(int species, int form)
            {
                return species == 0
                    ? Properties.Resources.empty
                    : WinFormsUtil.getSprite(species, form, 0, 0, Main.Config);
            }

            return img;
        }
    }
}
