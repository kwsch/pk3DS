using System;
using System.Linq;
using System.Windows.Forms;

namespace pk3DS
{
    public partial class StaticEncounterEditor7 : Form
    {
        private readonly byte[][] files;
        private readonly EncounterGift7[] Gifts;
        private readonly EncounterStatic7[] Encounters;
        private readonly EncounterTrade7[] Trades;
        private readonly string[] movelist = Main.getText(TextName.MoveNames);
        private readonly string[] itemlist = Main.getText(TextName.ItemNames);
        private readonly string[] specieslist = Main.getText(TextName.SpeciesNames);
        private readonly string[] natures = Main.getText(TextName.Natures);
        private readonly string[] types = Main.getText(TextName.Types);
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

            var dr = Util.Prompt(MessageBoxButtons.YesNo, "Starters have been changed. Update text references?");
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
        private int[] getRandomSpeciesList()
        {
            return Randomizer.getSpeciesList(CHK_G1.Checked, CHK_G2.Checked, CHK_G3.Checked, CHK_G4.Checked, CHK_G5.Checked, CHK_G6.Checked, CHK_G7.Checked,
                CHK_L.Checked, CHK_E.Checked);
        }
        private void B_Starters_Click(object sender, EventArgs e)
        {
            int[] sL = getRandomSpeciesList();
            int ctr = 0;

            setGift();

            // Assign Species
            for (int i = 0; i < 3; i++)
            {
                var t = Gifts[i];
                t.Species = Randomizer.getRandomSpecies(ref sL, ref ctr, oldStarters[i], CHK_BST.Checked, Main.SpeciesStat);
                t.Form = Randomizer.GetRandomForme(t.Species, false, true);
            }

            getListBoxEntries();
            getGift();

            System.Media.SystemSounds.Asterisk.Play();
        }
        private void B_RandAll_Click(object sender, EventArgs e)
        {
            int[] sL = getRandomSpeciesList();
            int ctr = 0;

            setGift();
            setEncounter();
            setTrade();

            for (int i = 3; i < Gifts.Length; i++) // Skip Starters
            {
                var t = Gifts[i];
                t.Species = Randomizer.getRandomSpecies(ref sL, ref ctr, t.Species, CHK_BST.Checked, Main.SpeciesStat);
                t.Form = Randomizer.GetRandomForme(t.Species, false, true);
            }
            foreach (EncounterStatic7 t in Encounters)
            {
                t.Species = Randomizer.getRandomSpecies(ref sL, ref ctr, t.Species, CHK_BST.Checked, Main.SpeciesStat);
                t.Form = Randomizer.GetRandomForme(t.Species, false, true);
                int[] moves = Main.Config.Learnsets[t.Species].getMoves(t.Level); Array.Resize(ref moves, 4);
                t.RelearnMoves = moves;
            }
            foreach (EncounterTrade7 t in Trades)
            {
                t.Species = Randomizer.getRandomSpecies(ref sL, ref ctr, t.Species, CHK_BST.Checked, Main.SpeciesStat);
                t.Form = Randomizer.GetRandomForme(t.Species, false, true);
                t.TradeRequestSpecies = Randomizer.getRandomSpecies(ref sL, ref ctr, t.TradeRequestSpecies, CHK_BST.Checked, Main.SpeciesStat);
            }

            getListBoxEntries();
            getGift();
            getEncounter();
            getTrade();

            System.Media.SystemSounds.Asterisk.Play();
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

                string[] storyText = TextFile.getStrings(storytextdata[41]);

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
                storytextdata[41] = TextFile.getBytes(storyText);
                s.Files = storytextdata;
                s.Save();
            }
        }
    }
}
