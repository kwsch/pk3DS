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

            itemlist[0] = specieslist[0] = "(None)";
            foreach (var s in specieslist)
            {
                CB_GSpecies.Items.Add(s);
                CB_ESpecies.Items.Add(s);
                CB_TSpecies.Items.Add(s);
            }
            foreach (var s in itemlist)
            {
                CB_GHeldItem.Items.Add(s);
                CB_EHeldItem.Items.Add(s);
                CB_THeldItem.Items.Add(s);
            }

            LB_Gift.Items.Clear();
            LB_Encounter.Items.Clear();
            LB_Trade.Items.Clear();

            for (int i = 0; i < Gifts.Length; i++)
                LB_Gift.Items.Add(getEntryText(Gifts[i], i));
            for (int i = 0; i < Encounters.Length; i++)
                LB_Encounter.Items.Add(getEntryText(Encounters[i], i));
            for (int i = 0; i < Trades.Length; i++)
                LB_Trade.Items.Add(getEntryText(Trades[i], i));

            LB_Gift.SelectedIndex = 0;
            LB_Encounter.SelectedIndex = 0;
            LB_Trade.SelectedIndex = 0;
        }

        private int gEntry = -1;
        private int eEntry = -1;
        private int tEntry = -1;

        private void B_Save_Click(object sender, EventArgs e)
        {
            setGift();
            setEncounters();
            setTrade();
            saveData();
            Close();
        }

        private void saveData()
        {
            files[0] = Gifts.SelectMany(file => file.Data).ToArray();
            files[1] = Encounters.SelectMany(file => file.Data).ToArray();
            files[4] = Trades.SelectMany(file => file.Data).ToArray();
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
            setEncounters();
            eEntry = LB_Encounter.SelectedIndex;
            getEncounters();
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
        private void getEncounters()
        {
            if (eEntry < 0)
                return;

            loading = true;
            var entry = Encounters[eEntry];
            CB_ESpecies.SelectedIndex = entry.Species;
            CB_EHeldItem.SelectedIndex = entry.HeldItem;
            NUD_ELevel.Value = entry.Level;
            NUD_EForm.Value = entry.Form;
            loading = false;
        }
        private void setEncounters()
        {
            if (eEntry < 0)
                return;

            var entry = Encounters[eEntry];
            entry.Species = CB_ESpecies.SelectedIndex;
            entry.HeldItem = CB_EHeldItem.SelectedIndex;
            entry.Level = (int)NUD_ELevel.Value;
            entry.Form = (int)NUD_EForm.Value;
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
            if (sender == CB_ESpecies)
            {
                var entry = Encounters[eEntry];
                entry.Species = cb.SelectedIndex;
                LB_Encounter.Items[eEntry] = getEntryText(entry, eEntry);
            }
            if (sender == CB_TSpecies)
            {
                var entry = Trades[tEntry];
                entry.Species = cb.SelectedIndex;
                LB_Trade.Items[tEntry] = getEntryText(entry, tEntry);
            }
        }

        private void B_Starters_Click(object sender, EventArgs e)
        {
            bool blind = DialogResult.Yes ==
                         Util.Prompt(MessageBoxButtons.YesNo, "Hide randomization, save, and close?",
                             "If you want the Starters to be a surprise :)");
            if (blind)
                Hide();
            
            setGift();

            int[] sL = Randomizer.getSpeciesList(true, true, true, true, true, true, false, false, false);
            int ctr = 0;
            // Assign Species
            for (int j = 0; j < 3; j++)
            {
                int species = Randomizer.getRandomSpecies(ref sL, ref ctr);

                if (true) // Enforce BST
                {
                    int oldSpecies = Gifts[j].Species;
                    PersonalInfo oldpkm = Main.SpeciesStat[oldSpecies]; // Use original species cuz why not.
                    PersonalInfo pkm = Main.SpeciesStat[species];

                    while (!(pkm.BST * 5 / 6 < oldpkm.BST && pkm.BST * 6 / 5 > oldpkm.BST))
                    { species = Randomizer.getRandomSpecies(ref sL, ref ctr); pkm = Main.SpeciesStat[species]; }
                }

                Gifts[j].Species = species;
            }

            if (blind)
            {
                saveData();
                Close();
            }
        }
    }
}
