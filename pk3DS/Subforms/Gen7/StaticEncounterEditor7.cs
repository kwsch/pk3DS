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
            }

            getListBoxEntries();
            LB_Gift.SelectedIndex = 0;
            LB_Encounter.SelectedIndex = 0;
            LB_Trade.SelectedIndex = 0;

            // Select last tab (Randomization) by default in case info already randomized.
            TC_Tabs.SelectedIndex = TC_Tabs.TabCount - 1;

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
            return $"{entry.ToString("00")} - {specieslist[species]}";
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
            CHK_G_Lock.Checked = entry.ShinyLock;
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
            entry.ShinyLock = CHK_G_Lock.Checked;
        }
        private void getEncounter()
        {
            if (eEntry < 0)
                return;

            loading = true;
            var entry = Encounters[eEntry];
            CB_ESpecies.SelectedIndex = entry.Species;
            CB_EHeldItem.SelectedIndex = entry.HeldItem;
            NUD_ELevel.Value = entry.Level;
            NUD_EForm.Value = entry.Form;

            int[] moves = entry.RelearnMoves;
            CB_EMove0.SelectedIndex = moves[0];
            CB_EMove1.SelectedIndex = moves[1];
            CB_EMove2.SelectedIndex = moves[2];
            CB_EMove3.SelectedIndex = moves[3];
            CHK_ShinyLock.Checked = entry.ShinyLock;

            loading = false;
        }
        private void setEncounter()
        {
            if (eEntry < 0)
                return;

            var entry = Encounters[eEntry];
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
            entry.ShinyLock = CHK_ShinyLock.Checked;
        }
        private void getTrade()
        {
            if (tEntry < 0)
                return;

            loading = true;
            var entry = Trades[tEntry];
            CB_TSpecies.SelectedIndex = entry.Species;
            CB_THeldItem.SelectedIndex = entry.HeldItem;
            NUD_TLevel.Value = entry.Level;
            NUD_TForm.Value = entry.Form;
            
            NUD_TID.Value = entry.ID;
            CB_TRequest.SelectedIndex = entry.TradeRequestSpecies;

            loading = false;
        }
        private void setTrade()
        {
            if (tEntry < 0)
                return;

            var entry = Trades[tEntry];
            entry.Species = CB_TSpecies.SelectedIndex;
            entry.HeldItem = CB_THeldItem.SelectedIndex;
            entry.Level = (int)NUD_TLevel.Value;
            entry.Form = (int)NUD_TForm.Value;

            entry.ID = (uint)NUD_TID.Value;
            entry.TradeRequestSpecies = CB_TRequest.SelectedIndex;
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
            else if(sender == CB_TSpecies)
            {
                var entry = Trades[tEntry];
                entry.Species = cb.SelectedIndex;
                LB_Trade.Items[tEntry] = getEntryText(entry, tEntry);
            }
        }
        private void changeTID(object sender, EventArgs e)
        {
            L_TTID.Text = $"TID: {NUD_TID.Value % 100000:000000}";
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

            // Assign Species
            for (int i = 0; i < 3; i++)
            {
                var t = Gifts[i];
                t.Species = specrand.GetRandomSpecies(oldStarters[i]);
                t.Form = formrand.GetRandomForme(t.Species);

                // no level boosting
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

            for (int i = 3; i < Gifts.Length; i++) // Skip Starters
            {
                var t = Gifts[i];
                t.Species = specrand.GetRandomSpecies(t.Species);
                t.Form = formrand.GetRandomForme(t.Species);

                if (CHK_Level.Checked)
                    t.Level = Randomizer.getModifiedLevel(t.Level, NUD_LevelBoost.Value);
            }
            foreach (EncounterStatic7 t in Encounters)
            {
                t.Species = specrand.GetRandomSpecies(t.Species);
                t.Form = formrand.GetRandomForme(t.Species);
                t.RelearnMoves = move.GetCurrentMoves(t.Species, t.Form, t.Level, 4);

                if (CHK_Level.Checked)
                    t.Level = Randomizer.getModifiedLevel(t.Level, NUD_LevelBoost.Value);
            }
            foreach (EncounterTrade7 t in Trades)
            {
                t.Species = specrand.GetRandomSpecies(t.Species);
                t.Form = formrand.GetRandomForme(t.Species);
                t.TradeRequestSpecies = specrand.GetRandomSpecies(t.TradeRequestSpecies);

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
            for (int i = 0; i < 10; i++)
            {
                // get Story Text
                var sr = gr.getRelativeGARC(i, gr.Name);
                var s = Main.Config.getGARCByReference(sr);
                byte[][] storytextdata = s.Files;

                string[] storyText = TextFile.getStrings(Main.Config, storytextdata[41]);

                for (int j = 0; j < 3; j++)
                {
                    int oldSpecies = oldStarters[j];
                    int species = Gifts[j].Species;
                    // Replace Story Text
                    string line = storyText[1 + j];
                    // Replace Species
                    line = line.Replace(specieslist[oldSpecies], specieslist[species]);

                    int oldIndex = Main.Config.Personal.getFormeIndex(oldSpecies, Gifts[j].Form);
                    int oldtype0 = Main.Config.Personal[oldIndex].Types[0];

                    int newIndex = Main.Config.Personal.getFormeIndex(species, Gifts[j].Form);
                    int newtype0 = Main.Config.Personal[newIndex].Types[0];
                    line = line.Replace(types[oldtype0], types[newtype0]);

                    storyText[1 + j] = line;
                }
                storytextdata[41] = TextFile.getBytes(Main.Config, storyText);
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
    }
}
