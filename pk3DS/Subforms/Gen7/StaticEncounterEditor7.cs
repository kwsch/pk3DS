using System;
using System.Linq;
using System.Windows.Forms;

using pk3DS.Core;
using pk3DS.Core.Randomizers;
using pk3DS.Core.Structures;

namespace pk3DS
{
    public partial class StaticEncounterEditor7 : Form
    {
        private readonly byte[][] files;
        private readonly EncounterGift7[] Gifts;
        private readonly EncounterStatic7[] Encounters;
        private readonly EncounterTrade7[] Trades;
        private readonly string[] movelist = Main.Config.getText(TextName.MoveNames);
        private readonly string[] itemlist = Main.Config.getText(TextName.ItemNames);
        private readonly string[] specieslist = Main.Config.getText(TextName.SpeciesNames);
        private readonly string[] natures = Main.Config.getText(TextName.Natures);
        private readonly string[] types = Main.Config.getText(TextName.Types);
        private readonly int[] oldStarters;

        private readonly string[] aura =
        {
            "(None)",
            "Attack (+1)",
            "Attack (+2)",
            "Attack (+3)",
            "Defense (+1)",
            "Defense (+2)",
            "Defense (+3)",
            "Sp. Attack (+1)",
            "Sp. Attack (+2)",
            "Sp. Attack (+3)",
            "Sp. Defense (+1)",
            "Sp. Defense (+2)",
            "Sp. Defense (+3)",
            "Speed (+1)",
            "Speed (+2)",
            "Speed (+3)",
            "All Stats (+1)",
            "All Stats (+2)",
            "All Stats (+3)",
        };

        public StaticEncounterEditor7(byte[][] infiles)
        {
            InitializeComponent();
            files = infiles;

            // File 0: Gifts
            {
                byte[] data = files[0];
                Gifts = new EncounterGift7[data.Length / EncounterGift7.SIZE];
                for (int i = 0; i < data.Length; i += EncounterGift7.SIZE)
                {
                    byte[] entry = new byte[EncounterGift7.SIZE];
                    Array.Copy(data, i, entry, 0, entry.Length);
                    Gifts[i/EncounterGift7.SIZE] = new EncounterGift7(entry);
                }
            }
            oldStarters = Gifts.Take(3).Select(gift => gift.Species).ToArray();

            // File 1: Encounters
            {
                byte[] data = files[1];
                Encounters = new EncounterStatic7[data.Length / EncounterStatic7.SIZE];
                for (int i = 0; i < data.Length; i += EncounterStatic7.SIZE)
                {
                    byte[] entry = new byte[EncounterStatic7.SIZE];
                    Array.Copy(data, i, entry, 0, entry.Length);
                    Encounters[i/EncounterStatic7.SIZE] = new EncounterStatic7(entry);
                }
            }

            // File 4: Trades
            {
                byte[] data = files[4];
                Trades = new EncounterTrade7[data.Length / EncounterTrade7.SIZE];
                for (int i = 0; i < data.Length; i += EncounterTrade7.SIZE)
                {
                    byte[] entry = new byte[EncounterTrade7.SIZE];
                    Array.Copy(data, i, entry, 0, entry.Length);
                    Trades[i/EncounterTrade7.SIZE] = new EncounterTrade7(entry);
                }
            }

            movelist[0] = itemlist[0] = specieslist[0] = "(None)";
            foreach (var s in specieslist)
            {
                CB_GSpecies.Items.Add(s);
                CB_ESpecies.Items.Add(s);
                CB_TSpecies.Items.Add(s);
                CB_TRequest.Items.Add(s);
            }
            foreach (var s in itemlist)
            {
                CB_GHeldItem.Items.Add(s);
                CB_EHeldItem.Items.Add(s);
                CB_THeldItem.Items.Add(s);
            }
            foreach (var s in movelist)
            {
                CB_EMove0.Items.Add(s);
                CB_EMove1.Items.Add(s);
                CB_EMove2.Items.Add(s);
                CB_EMove3.Items.Add(s);
                CB_SpecialMove.Items.Add(s);
            }

            CB_GNature.Items.Add("Random");
            CB_GNature.Items.AddRange(natures.Take(25).ToArray());
            CB_ENature.Items.Add("Random");
            CB_ENature.Items.AddRange(natures.Take(25).ToArray());
            CB_TNature.Items.AddRange(natures.Take(25).ToArray());

            foreach (string s in aura) CB_Aura.Items.Add(s);

            getListBoxEntries();
            LB_Gift.SelectedIndex = 0;
            LB_Encounter.SelectedIndex = 0;
            LB_Trade.SelectedIndex = 0;

            // Select last tab (Randomization) by default in case info already randomized.
            TC_Tabs.SelectedIndex = TC_Tabs.TabCount - 1;

            RandSettings.GetFormSettings(this, Tab_Randomizer.Controls);
            // ExportEncounters();
        }
        private void getListBoxEntries()
        {
            loading = true;
            LB_Gift.Items.Clear();
            LB_Encounter.Items.Clear();
            LB_Trade.Items.Clear();

            for (int i = 0; i < Gifts.Length; i++)
                LB_Gift.Items.Add(getEntryText(Gifts[i], i));
            for (int i = 0; i < Encounters.Length; i++)
                LB_Encounter.Items.Add(getEntryText(Encounters[i], i));
            for (int i = 0; i < Trades.Length; i++)
                LB_Trade.Items.Add(getEntryText(Trades[i], i));
            loading = false;
        }

        private int gEntry = -1;
        private int eEntry = -1;
        private int tEntry = -1;

        private void B_Save_Click(object sender, EventArgs e)
        {
            setGift();
            setEncounter();
            setTrade();
            saveData();
            RandSettings.SetFormSettings(this, Tab_Randomizer.Controls);
            Close();
        }
        private void B_Cancel_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void saveData()
        {
            files[0] = Gifts.SelectMany(file => file.Data).ToArray();
            files[1] = Encounters.SelectMany(file => file.Data).ToArray();
            files[4] = Trades.SelectMany(file => file.Data).ToArray();

            if (Gifts.Take(3).Select(gift => gift.Species).SequenceEqual(oldStarters))
                return;

            var dr = WinFormsUtil.Prompt(MessageBoxButtons.YesNo, "Starters have been changed. Update text references?", "Note that this only updates text references for the current language set in pk3DS.", "This can be changed from Options -> Language on the main window.");
            if (dr == DialogResult.Yes)
                updateStarterText();
        }

        private string getEntryText(int species, int entry)
        {
            return $"{entry:00} - {specieslist[species]}";
        }
        private string getEntryText(EncounterStatic enc, int entry)
        {
            return getEntryText(enc.Species, entry);
        }

        private void LB_Gift_SelectedIndexChanged(object sender, EventArgs e)
        {
            setGift();
            gEntry = LB_Gift.SelectedIndex;
            getGift();
        }
        private void LB_Encounter_SelectedIndexChanged(object sender, EventArgs e)
        {
            setEncounter();
            eEntry = LB_Encounter.SelectedIndex;
            getEncounter();
        }
        private void LB_Trade_SelectedIndexChanged(object sender, EventArgs e)
        {
            setTrade();
            tEntry = LB_Trade.SelectedIndex;
            getTrade();
        }

        private bool loading;
        private void getGift()
        {
            if (gEntry < 0)
                return;

            loading = true;
            var entry = Gifts[gEntry];
            CB_GSpecies.SelectedIndex = entry.Species;
            CB_GHeldItem.SelectedIndex = entry.HeldItem;
            NUD_GLevel.Value = entry.Level;
            NUD_GForm.Value = entry.Form;
            NUD_GGender.Value = entry.Gender;
            CB_GNature.SelectedIndex = entry.Nature + 1;
            CB_SpecialMove.SelectedIndex = entry.SpecialMove;
            CHK_G_Lock.Checked = entry.ShinyLock;
            CHK_GIV3.Checked = entry.IV3;
            CHK_IsEgg.Checked = entry.IsEgg;

            loading = false;
        }
        private void setGift()
        {
            if (gEntry < 0)
                return;

            var entry = Gifts[gEntry];
            entry.Species = CB_GSpecies.SelectedIndex;
            entry.HeldItem = CB_GHeldItem.SelectedIndex;
            entry.Level = (int)NUD_GLevel.Value;
            entry.Form = (int)NUD_GForm.Value;
            entry.Gender = (int) NUD_GGender.Value;
            entry.Nature = (sbyte)(CB_GNature.SelectedIndex - 1);
            entry.SpecialMove = CB_SpecialMove.SelectedIndex;
            entry.ShinyLock = CHK_G_Lock.Checked;
            entry.IsEgg = CHK_IsEgg.Checked;
        }
        private void getEncounter()
        {
            if (eEntry < 0)
                return;

            loading = true;
            var entry = Encounters[eEntry];
            var iv = entry.IVs;
            var ev = entry.EVs;
            CB_ESpecies.SelectedIndex = entry.Species;
            CB_EHeldItem.SelectedIndex = entry.HeldItem;
            NUD_ELevel.Value = entry.Level;
            NUD_EForm.Value = entry.Form;

            int[] moves = entry.RelearnMoves;
            CB_EMove0.SelectedIndex = moves[0];
            CB_EMove1.SelectedIndex = moves[1];
            CB_EMove2.SelectedIndex = moves[2];
            CB_EMove3.SelectedIndex = moves[3];

            NUD_EIV0.Value = iv[0];
            NUD_EIV1.Value = iv[1];
            NUD_EIV2.Value = iv[2];
            NUD_EIV3.Value = iv[3];
            NUD_EIV4.Value = iv[4];
            NUD_EIV5.Value = iv[5];

            NUD_EV0.Value = ev[0];
            NUD_EV1.Value = ev[1];
            NUD_EV2.Value = ev[2];
            NUD_EV3.Value = ev[3];
            NUD_EV4.Value = ev[4];
            NUD_EV5.Value = ev[5];

            CHK_ShinyLock.Checked = entry.ShinyLock;
            CHK_EIV3.Checked = entry.IV3;
            CB_ENature.SelectedIndex = entry.Nature;
            CB_Aura.SelectedIndex = entry.Aura;

            loading = false;
        }
        private void setEncounter()
        {
            if (eEntry < 0)
                return;

            var entry = Encounters[eEntry];
            var iv = entry.IVs;
            var ev = entry.EVs;
            entry.Species = CB_ESpecies.SelectedIndex;
            entry.HeldItem = CB_EHeldItem.SelectedIndex;
            entry.Level = (int)NUD_ELevel.Value;
            entry.Form = (int)NUD_EForm.Value;
            entry.RelearnMoves = new[]
            {
                CB_EMove0.SelectedIndex,
                CB_EMove1.SelectedIndex,
                CB_EMove2.SelectedIndex,
                CB_EMove3.SelectedIndex,
            };

            iv[0] = (int)NUD_EIV0.Value;
            iv[1] = (int)NUD_EIV1.Value;
            iv[2] = (int)NUD_EIV2.Value;
            iv[3] = (int)NUD_EIV3.Value;
            iv[4] = (int)NUD_EIV4.Value;
            iv[5] = (int)NUD_EIV5.Value;
            entry.IVs = iv;

            ev[0] = (int)NUD_EV0.Value;
            ev[1] = (int)NUD_EV1.Value;
            ev[2] = (int)NUD_EV2.Value;
            ev[3] = (int)NUD_EV3.Value;
            ev[4] = (int)NUD_EV4.Value;
            ev[5] = (int)NUD_EV5.Value;
            entry.EVs = ev;

            entry.ShinyLock = CHK_ShinyLock.Checked;
            entry.Nature = CB_ENature.SelectedIndex;
            entry.Aura = CB_Aura.SelectedIndex;
        }
        private void getTrade()
        {
            if (tEntry < 0)
                return;

            loading = true;
            var entry = Trades[tEntry];
            var iv = entry.IVs;
            CB_TSpecies.SelectedIndex = entry.Species;
            CB_THeldItem.SelectedIndex = entry.HeldItem;
            NUD_TLevel.Value = entry.Level;
            NUD_TForm.Value = entry.Form;
            CB_TNature.SelectedIndex = entry.Nature;

            NUD_TID.Value = entry.ID;
            CB_TRequest.SelectedIndex = entry.TradeRequestSpecies;

            NUD_TIV0.Value = iv[0];
            NUD_TIV1.Value = iv[1];
            NUD_TIV2.Value = iv[2];
            NUD_TIV3.Value = iv[3];
            NUD_TIV4.Value = iv[4];
            NUD_TIV5.Value = iv[5];

            loading = false;
        }
        private void setTrade()
        {
            if (tEntry < 0)
                return;

            var entry = Trades[tEntry];
            var iv = entry.IVs;
            entry.Species = CB_TSpecies.SelectedIndex;
            entry.HeldItem = CB_THeldItem.SelectedIndex;
            entry.Level = (int)NUD_TLevel.Value;
            entry.Form = (int)NUD_TForm.Value;
            entry.Nature = CB_TNature.SelectedIndex;

            entry.TID = (int)NUD_TID.Value;
            entry.TradeRequestSpecies = CB_TRequest.SelectedIndex;

            iv[0] = (int)NUD_TIV0.Value;
            iv[1] = (int)NUD_TIV1.Value;
            iv[2] = (int)NUD_TIV2.Value;
            iv[3] = (int)NUD_TIV3.Value;
            iv[4] = (int)NUD_TIV4.Value;
            iv[5] = (int)NUD_TIV5.Value;
            entry.IVs = iv;
        }
        
        private void changeSpecies(object sender, EventArgs e)
        {
            if (loading)
                return;
            var cb = sender as ComboBox;
            if (cb == null)
                return;

            if (sender == CB_GSpecies)
            {
                var entry = Gifts[gEntry];
                entry.Species = cb.SelectedIndex;
                LB_Gift.Items[gEntry] = getEntryText(entry, gEntry);
            }
            else if (sender == CB_ESpecies)
            {
                var entry = Encounters[eEntry];
                entry.Species = cb.SelectedIndex;
                LB_Encounter.Items[eEntry] = getEntryText(entry, eEntry);
            }
            else if (sender == CB_TSpecies)
            {
                var entry = Trades[tEntry];
                entry.Species = cb.SelectedIndex;
                LB_Trade.Items[tEntry] = getEntryText(entry, tEntry);
            }
        }
        private void changeTID(object sender, EventArgs e)
        {
            L_TTID.Text = $"Gen 7 ID: {NUD_TID.Value % 100000:000000}";
        }

        // Randomization
        private SpeciesRandomizer getRandomizer()
        {
            var specrand = new SpeciesRandomizer(Main.Config)
            {
                G1 = CHK_G1.Checked,
                G2 = CHK_G2.Checked,
                G3 = CHK_G3.Checked,
                G4 = CHK_G4.Checked,
                G5 = CHK_G5.Checked,
                G6 = CHK_G6.Checked,
                G7 = false,

                E = CHK_E.Checked,
                L = CHK_L.Checked,

                rBST = CHK_BST.Checked,
            };
            specrand.Initialize();
            return specrand;
        }
        private void B_Starters_Click(object sender, EventArgs e)
        {
            if (WinFormsUtil.Prompt(MessageBoxButtons.YesNo, "Randomize Starters? Cannot undo.", "Double check Randomization settings before continuing.") != DialogResult.Yes)
                return;

            setGift();

            var specrand = getRandomizer();
            var formrand = new FormRandomizer(Main.Config) { AllowMega = false, AllowAlolanForm = true };
            var items = Randomizer.getRandomItemList();

            // Assign Species
            for (int i = 0; i < 3; i++)
            {
                var t = Gifts[i];
                t.Species = specrand.GetRandomSpecies(oldStarters[i]);
                t.Form = formrand.GetRandomForme(t.Species);

                if (CHK_AllowMega.Checked)
                    formrand.AllowMega = true;

                if (CHK_Item.Checked)
                    t.HeldItem = items[Util.rnd32() % items.Length];

                if (CHK_Level.Checked)
                    t.Level = Randomizer.getModifiedLevel(t.Level, NUD_LevelBoost.Value);

                if (CHK_RemoveShinyLock.Checked)
                    t.ShinyLock = false; // in case any user modifications locked the starters
            }

            getListBoxEntries();
            getGift();

            WinFormsUtil.Alert("Randomized Starters according to specification!");
        }
        private void B_RandAll_Click(object sender, EventArgs e)
        {
            if (WinFormsUtil.Prompt(MessageBoxButtons.YesNo, "Randomize Static Encounters? Cannot undo.", "Double check Randomization Settings before continuing.") != DialogResult.Yes)
                return;
            
            setGift();
            setEncounter();
            setTrade();

            var specrand = getRandomizer();
            var formrand = new FormRandomizer(Main.Config) { AllowMega = false, AllowAlolanForm = true };
            var move = new LearnsetRandomizer(Main.Config, Main.Config.Learnsets);
            var items = Randomizer.getRandomItemList();

            for (int i = 3; i < Gifts.Length; i++) // Skip Starters
            {
                var t = Gifts[i];
                t.Species = specrand.GetRandomSpecies(t.Species);
                t.Form = formrand.GetRandomForme(t.Species);

                if (CHK_AllowMega.Checked)
                    formrand.AllowMega = true;

                if (CHK_Item.Checked)
                    t.HeldItem = items[Util.rnd32() % items.Length];

                if (CHK_Level.Checked)
                    t.Level = Randomizer.getModifiedLevel(t.Level, NUD_LevelBoost.Value);

                if (CHK_RemoveShinyLock.Checked)
                    t.ShinyLock = false;
            }
            foreach (EncounterStatic7 t in Encounters)
            {
                t.Species = specrand.GetRandomSpecies(t.Species);
                t.Form = formrand.GetRandomForme(t.Species);
                t.RelearnMoves = move.GetCurrentMoves(t.Species, t.Form, t.Level, 4);

                if (CHK_AllowMega.Checked)
                    formrand.AllowMega = true;

                if (CHK_Item.Checked)
                    t.HeldItem = items[Util.rnd32() % items.Length];

                if (CHK_Level.Checked)
                    t.Level = Randomizer.getModifiedLevel(t.Level, NUD_LevelBoost.Value);

                if (CHK_RemoveShinyLock.Checked)
                    t.ShinyLock = false;

                if (CHK_RandomAura.Checked)
                {
                    if (t.Aura == 0)
                        continue; // don't apply aura to a pkm without it

                    CB_Aura.Items.AddRange(aura.Take(0).ToArray()); // don't allow (None) as an option; temporarily remove
                    t.Aura = (int)(Util.rnd32() % CB_Aura.Items.Count);
                }
            }
            foreach (EncounterTrade7 t in Trades)
            {
                t.Species = specrand.GetRandomSpecies(t.Species);
                t.Form = formrand.GetRandomForme(t.Species);
                t.TradeRequestSpecies = specrand.GetRandomSpecies(t.TradeRequestSpecies);

                if (CHK_AllowMega.Checked)
                    formrand.AllowMega = true;

                if (CHK_Item.Checked)
                    t.HeldItem = items[Util.rnd32() % items.Length];

                if (CHK_Level.Checked)
                    t.Level = Randomizer.getModifiedLevel(t.Level, NUD_LevelBoost.Value);
            }

            getListBoxEntries();
            getGift();
            getEncounter();
            getTrade();

            WinFormsUtil.Alert("Randomized Static Encounters according to specification!");
        }

        // Mirror Changes
        private void updateStarterText()
        {
            var gr = Main.Config.getGARCReference("storytext");
            int file = Main.Config.USUM ? 39 : 41;
            for (int i = 0; i < 10; i++)
            {
                // get Story Text
                var sr = gr.getRelativeGARC(i, gr.Name);
                var s = Main.Config.getGARCByReference(sr);
                byte[][] storytextdata = s.Files;

                string[] storyText = TextFile.getStrings(Main.Config, storytextdata[file]);

                for (int j = 0; j < 3; j++)
                {
                    int oldSpecies = oldStarters[j];
                    int species = Gifts[j].Species;
                    // Replace Story Text
                    string line = storyText[1 + j];
                    // Replace Species
                    line = line.Replace(specieslist[oldSpecies], specieslist[species]);

                    if (Main.Config.SM) // replace type text
                    {
                        int oldIndex = Main.Config.Personal.getFormeIndex(oldSpecies, Gifts[j].Form);
                        int oldtype0 = Main.Config.Personal[oldIndex].Types[0];
                        int newIndex = Main.Config.Personal.getFormeIndex(species, Gifts[j].Form);
                        int newtype0 = Main.Config.Personal[newIndex].Types[0];
                        line = line.Replace(types[oldtype0], types[newtype0]);
                    }
                    else if (Main.Config.USUM)
                    {
                        storyText[14 + j] = specieslist[species];
                    }

                    storyText[1 + j] = line;
                }
                storytextdata[file] = TextFile.getBytes(Main.Config, storyText);
                s.Files = storytextdata;
                s.Save();
            }
        }

        private void ExportEncounters()
        {
            System.IO.File.WriteAllBytes("0", files[0]);
            System.IO.File.WriteAllBytes("1", files[1]);
            var g = Gifts.Select(z => z.GetSummary() + $" // {specieslist[z.Species]} @ ???");
            var s = Encounters.Select(z => z.GetSummary() + $" // {specieslist[z.Species]} @ ???");
            Clipboard.SetText(string.Join(Environment.NewLine, g.Concat(s)));
        }

        private void ModifyLevels(object sender, EventArgs e)
        {
            if (WinFormsUtil.Prompt(MessageBoxButtons.YesNo, "Modify all current Levels?", "Cannot undo.") != DialogResult.Yes) return;

            // encounter
            for (int i = 0; i < LB_Encounter.Items.Count; i++)
            {
                LB_Encounter.SelectedIndex = i;
                NUD_ELevel.Value = Randomizer.getModifiedLevel((int)NUD_ELevel.Value, NUD_LevelBoost.Value);
            }
            // gift
            for (int i = 0; i < LB_Gift.Items.Count; i++)
            {
                LB_Gift.SelectedIndex = i;
                NUD_GLevel.Value = Randomizer.getModifiedLevel((int)NUD_GLevel.Value, NUD_LevelBoost.Value);
            }
            // trade
            for (int i = 0; i < LB_Trade.Items.Count; i++)
            {
                LB_Trade.SelectedIndex = i;
                NUD_TLevel.Value = Randomizer.getModifiedLevel((int)NUD_TLevel.Value, NUD_LevelBoost.Value);
            }
            WinFormsUtil.Alert("Modified all Levels according to specification!");
        }
    }
}