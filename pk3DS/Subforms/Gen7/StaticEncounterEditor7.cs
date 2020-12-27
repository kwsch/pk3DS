using System;
using System.Collections.Generic;
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
        private readonly LearnsetRandomizer learn = new(Main.Config, Main.Config.Learnsets);
        private readonly string[] movelist = Main.Config.GetText(TextName.MoveNames);
        private readonly string[] itemlist = Main.Config.GetText(TextName.ItemNames);
        private readonly string[] specieslist = Main.Config.GetText(TextName.SpeciesNames);
        private readonly string[] natures = Main.Config.GetText(TextName.Natures);
        private readonly string[] types = Main.Config.GetText(TextName.Types);
        private readonly int[] oldStarters;
        private static int[] FinalEvo = Legal.FinalEvolutions_7;
        private static readonly int[] Legendary = Main.Config.USUM ? Legal.Legendary_USUM : Legal.Legendary_SM;
        private static readonly int[] Mythical = Main.Config.USUM ? Legal.Mythical_USUM : Legal.Mythical_SM;
        private static readonly int[] ReplaceLegend = Legendary.Concat(Mythical).ToArray();
        private static readonly int[] BasicStarter = Legal.BasicStarters_7;

        private readonly string[] gender =
        {
            "- / Genderless/Random",
            "♂ / Male",
            "♀ / Female",
        };

        private readonly string[] ability =
        {
            "Any (1 or 2)",
            "Ability 1",
            "Ability 2",
            "Hidden Ability",
        };

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

        private static readonly int[] Totem = { 020, 105, 735, 738, 743, 746, 752, 754, 758, 777, 778, 784 }; // Totem battles
        private static readonly int[] UnevolvedLegend = { 772, 789, 803 }; // Type: Null, Cosmog, Poipole gifts

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
                    Gifts[i / EncounterGift7.SIZE] = new EncounterGift7(entry);
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
                    Encounters[i / EncounterStatic7.SIZE] = new EncounterStatic7(entry);
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
                    Trades[i / EncounterTrade7.SIZE] = new EncounterTrade7(entry);
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
            foreach (var s in ability)
            {
                CB_GAbility.Items.Add(s);
                CB_EAbility.Items.Add(s);
                CB_TAbility.Items.Add(s);
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
            foreach (string s in gender)
            {
                CB_EGender.Items.Add(s);
                CB_TGender.Items.Add(s);
            }
            foreach (string s in aura) CB_Aura.Items.Add(s);

            CB_GNature.Items.Add("Random");
            CB_GNature.Items.AddRange(natures.Take(25).ToArray());
            CB_ENature.Items.Add("Random");
            CB_ENature.Items.AddRange(natures.Take(25).ToArray());
            CB_TNature.Items.AddRange(natures.Take(25).ToArray());

            NUD_Ally1.Maximum = NUD_Ally2.Maximum = Main.Config.USUM ? 251 : 136;

            GetListBoxEntries();
            LB_Gift.SelectedIndex = 0;
            LB_Encounter.SelectedIndex = 0;
            LB_Trade.SelectedIndex = 0;

            // Select last tab (Randomization) by default in case info already randomized.
            TC_Tabs.SelectedIndex = TC_Tabs.TabCount - 1;

            RandSettings.GetFormSettings(this, Tab_Randomizer.Controls);
            // ExportEncounters();
        }

        private void GetListBoxEntries()
        {
            loading = true;
            LB_Gift.Items.Clear();
            LB_Encounter.Items.Clear();
            LB_Trade.Items.Clear();

            for (int i = 0; i < Gifts.Length; i++)
                LB_Gift.Items.Add(GetEntryText(Gifts[i], i));
            for (int i = 0; i < Encounters.Length; i++)
                LB_Encounter.Items.Add(GetEntryText(Encounters[i], i));
            for (int i = 0; i < Trades.Length; i++)
                LB_Trade.Items.Add(GetEntryText(Trades[i], i));
            loading = false;
        }

        private int gEntry = -1;
        private int eEntry = -1;
        private int tEntry = -1;

        private void B_Save_Click(object sender, EventArgs e)
        {
            SetGift();
            SetEncounter();
            SetTrade();
            SaveData();
            RandSettings.SetFormSettings(this, Tab_Randomizer.Controls);
            Close();
        }

        private void B_Cancel_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void SaveData()
        {
            files[0] = Gifts.SelectMany(file => file.Data).ToArray();
            files[1] = Encounters.SelectMany(file => file.Data).ToArray();
            files[4] = Trades.SelectMany(file => file.Data).ToArray();

            if (Gifts.Take(3).Select(gift => gift.Species).SequenceEqual(oldStarters))
                return;

            var dr = WinFormsUtil.Prompt(MessageBoxButtons.YesNo, "Starters have been changed. Update text references?", "Note that this only updates text references for the current language set in pk3DS.", "This can be changed from Options -> Language on the main window.");
            if (dr == DialogResult.Yes)
                UpdateStarterText();
        }

        private string GetEntryText(int species, int entry)
        {
            return $"{entry:00} - {specieslist[species]}";
        }

        private string GetEntryText(EncounterStatic enc, int entry)
        {
            return GetEntryText(enc.Species, entry);
        }

        private void LB_Gift_SelectedIndexChanged(object sender, EventArgs e)
        {
            SetGift();
            gEntry = LB_Gift.SelectedIndex;
            GetGift();
        }

        private void LB_Encounter_SelectedIndexChanged(object sender, EventArgs e)
        {
            SetEncounter();
            eEntry = LB_Encounter.SelectedIndex;
            GetEncounter();
            GetAllies();
        }

        private void LB_Trade_SelectedIndexChanged(object sender, EventArgs e)
        {
            SetTrade();
            tEntry = LB_Trade.SelectedIndex;
            GetTrade();
        }

        private bool loading;

        private void GetAllies()
        {
            if (eEntry < 0)
                return;
            var entry = Encounters[eEntry];

            // USUM has slots with SOS allies beyond slot 100, accommodate by trimming an extra character
            int endTrim = eEntry < 100 ? 5 : 6;

            L_Ally1.Text = entry.Ally1 == 0 ? "No SOS Ally" : ((string)LB_Encounter.Items[entry.Ally1 - 1]).Remove(0, endTrim);
            L_Ally2.Text = entry.Ally2 == 0 ? "No SOS Ally" : ((string)LB_Encounter.Items[entry.Ally2 - 1]).Remove(0, endTrim);
        }

        private void GetGift()
        {
            if (gEntry < 0)
                return;

            loading = true;
            var entry = Gifts[gEntry];
            CB_GSpecies.SelectedIndex = entry.Species;
            CB_GHeldItem.SelectedIndex = entry.HeldItem;
            NUD_GLevel.Value = entry.Level;
            NUD_GForm.Value = entry.Form;
            CB_GAbility.SelectedIndex = entry.Ability + 1;
            CB_GNature.SelectedIndex = entry.Nature + 1;
            CB_SpecialMove.SelectedIndex = entry.SpecialMove;
            CHK_G_Lock.Checked = entry.ShinyLock;
            CHK_GIV3.Checked = entry.IV3;
            CHK_IsEgg.Checked = entry.IsEgg;

            loading = false;
        }

        private void SetGift()
        {
            if (gEntry < 0)
                return;

            var entry = Gifts[gEntry];
            entry.Species = CB_GSpecies.SelectedIndex;
            entry.HeldItem = CB_GHeldItem.SelectedIndex;
            entry.Level = (int)NUD_GLevel.Value;
            entry.Form = (int)NUD_GForm.Value;
            entry.Ability = (sbyte)(CB_GAbility.SelectedIndex - 1);
            entry.Nature = (sbyte)(CB_GNature.SelectedIndex - 1);
            entry.SpecialMove = CB_SpecialMove.SelectedIndex;
            entry.ShinyLock = CHK_G_Lock.Checked;
            entry.IsEgg = CHK_IsEgg.Checked;
        }

        private void GetEncounter()
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
            CB_EGender.SelectedIndex = entry.Gender;
            CB_EAbility.SelectedIndex = entry.Ability;

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
            NUD_Ally1.Value = entry.Ally1 - 1;
            NUD_Ally2.Value = entry.Ally2 - 1;

            loading = false;
        }

        private void SetEncounter()
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
            entry.Gender = CB_EGender.SelectedIndex;
            entry.Ability = CB_EAbility.SelectedIndex;
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
            entry.Ally1 = (int)NUD_Ally1.Value + 1;
            entry.Ally2 = (int)NUD_Ally2.Value + 1;
        }

        private void GetTrade()
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
            CB_TGender.SelectedIndex = entry.Gender + 1;
            CB_TAbility.SelectedIndex = entry.Ability + 1;
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

        private void SetTrade()
        {
            if (tEntry < 0)
                return;

            var entry = Trades[tEntry];
            var iv = entry.IVs;
            entry.Species = CB_TSpecies.SelectedIndex;
            entry.HeldItem = CB_THeldItem.SelectedIndex;
            entry.Level = (int)NUD_TLevel.Value;
            entry.Form = (int)NUD_TForm.Value;
            entry.Gender = CB_TGender.SelectedIndex - 1;
            entry.Ability = (CB_TAbility.SelectedIndex - 1);
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

        private void ChangeSpecies(object sender, EventArgs e)
        {
            if (loading)
                return;
            if (sender is not ComboBox cb)
                return;

            if (sender == CB_GSpecies)
            {
                var entry = Gifts[gEntry];
                entry.Species = cb.SelectedIndex;
                LB_Gift.Items[gEntry] = GetEntryText(entry, gEntry);
            }
            else if (sender == CB_ESpecies)
            {
                var entry = Encounters[eEntry];
                entry.Species = cb.SelectedIndex;
                LB_Encounter.Items[eEntry] = GetEntryText(entry, eEntry);
            }
            else if (sender == CB_TSpecies)
            {
                var entry = Trades[tEntry];
                entry.Species = cb.SelectedIndex;
                LB_Trade.Items[tEntry] = GetEntryText(entry, tEntry);
            }
        }

        private void ChangeTID(object sender, EventArgs e)
        {
            L_TTID.Text = $"Gen 7 ID: {NUD_TID.Value % 100000:000000}";
        }

        // Randomization
        private SpeciesRandomizer GetRandomizer()
        {
            var specrand = new SpeciesRandomizer(Main.Config)
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
            specrand.Initialize();

            // add Legendary/Mythical to final evolutions if checked
            if (CHK_L.Checked) FinalEvo = FinalEvo.Concat(Legendary).ToArray();
            if (CHK_E.Checked) FinalEvo = FinalEvo.Concat(Mythical).ToArray();

            return specrand;
        }

        private void B_Starters_Click(object sender, EventArgs e)
        {
            if (WinFormsUtil.Prompt(MessageBoxButtons.YesNo, "Randomize Starters? Cannot undo.", "Double check Randomization settings before continuing.") != DialogResult.Yes)
                return;

            SetGift();

            var specrand = GetRandomizer();
            var formrand = new FormRandomizer(Main.Config) { AllowMega = false, AllowAlolanForm = true };
            var items = Randomizer.GetRandomItemList();
            int[] banned = Legal.Z_Moves.Concat(new[] { 165, 464, 621 }).ToArray();

            // Assign Species
            for (int i = 0; i < 3; i++)
            {
                var t = Gifts[i];

                // Pokemon with 2 evolutions
                if (CHK_BasicStarter.Checked)
                {
                    static int basic() => (int)(Util.Random32() % BasicStarter.Length);
                    t.Species = BasicStarter[basic()];
                }
                else
                {
                    t.Species = specrand.GetRandomSpecies(oldStarters[i]);
                }

                if (CHK_AllowMega.Checked)
                    formrand.AllowMega = true;

                if (CHK_Item.Checked)
                    t.HeldItem = items[Util.Random32() % items.Length];

                if (CHK_Level.Checked)
                    t.Level = Randomizer.GetModifiedLevel(t.Level, NUD_LevelBoost.Value);

                if (CHK_RemoveShinyLock.Checked)
                    t.ShinyLock = false;

                if (CHK_SpecialMove.Checked && !CHK_Metronome.Checked)
                {
                    int rv;
                    do { rv = Util.Rand.Next(1, CB_SpecialMove.Items.Count); }
                    while (banned.Contains(rv));
                    t.SpecialMove = rv;
                }

                if (CHK_RandomAbility.Checked)
                    t.Ability = (sbyte)(Util.Rand.Next(0, 3)); // 1, 2, or H

                t.Form = Randomizer.GetRandomForme(t.Species, CHK_AllowMega.Checked, true, Main.SpeciesStat);
                t.Nature = -1; // random
            }

            GetListBoxEntries();
            GetGift();

            WinFormsUtil.Alert("Randomized Starters according to specification!");
        }

        private void B_RandAll_Click(object sender, EventArgs e)
        {
            if (WinFormsUtil.Prompt(MessageBoxButtons.YesNo, "Randomize Static Encounters? Cannot undo.", "Double check Randomization Settings before continuing.") != DialogResult.Yes)
                return;

            SetGift();
            SetEncounter();
            SetTrade();

            var specrand = GetRandomizer();
            var formrand = new FormRandomizer(Main.Config) { AllowMega = false, AllowAlolanForm = true };
            var move = new LearnsetRandomizer(Main.Config, Main.Config.Learnsets);
            var items = Randomizer.GetRandomItemList();
            int[] banned = Legal.Z_Moves.Concat(new[] { 165, 464, 621 }).ToArray();
            int randFinalEvo() => (int)(Util.Random32() % FinalEvo.Length);
            int randLegend() => (int)(Util.Random32() % ReplaceLegend.Length);

            for (int i = 3; i < Gifts.Length; i++) // Skip Starters
            {
                var t = Gifts[i];

                // Legendary-for-Legendary
                if ((CHK_ReplaceLegend.Checked && ReplaceLegend.Contains(t.Species)) || UnevolvedLegend.Contains(t.Species))
                    t.Species = ReplaceLegend[randLegend()];

                // every other entry
                else
                    t.Species = specrand.GetRandomSpecies(t.Species);

                if (CHK_AllowMega.Checked)
                    formrand.AllowMega = true;

                if (CHK_Item.Checked)
                    t.HeldItem = items[Util.Random32() % items.Length];

                if (CHK_Level.Checked)
                    t.Level = Randomizer.GetModifiedLevel(t.Level, NUD_LevelBoost.Value);

                if (CHK_RemoveShinyLock.Checked)
                    t.ShinyLock = false;

                if (CHK_SpecialMove.Checked)
                {
                    if (CHK_Metronome.Checked)
                    {
                        t.SpecialMove = 0; // remove Surf Pikachu's special move
                    }
                    else
                    {
                        int rv;
                        do { rv = Util.Rand.Next(1, CB_SpecialMove.Items.Count); }
                        while (banned.Contains(rv));
                        t.SpecialMove = rv;
                    }
                }

                if (CHK_RandomAbility.Checked)
                    t.Ability = (sbyte)(Util.Rand.Next(0, 3)); // 1, 2, or H

                if (CHK_ForceFullyEvolved.Checked && t.Level >= NUD_ForceFullyEvolved.Value && !FinalEvo.Contains(t.Species))
                    t.Species = FinalEvo[randFinalEvo()];

                t.Form = Randomizer.GetRandomForme(t.Species, CHK_AllowMega.Checked, true, Main.SpeciesStat);
                t.Nature = -1; // random
            }
            foreach (EncounterStatic7 t in Encounters)
            {
                // Legendary-for-Legendary
                if (CHK_ReplaceLegend.Checked && ReplaceLegend.Contains(t.Species))
                    t.Species = ReplaceLegend[randLegend()];

                // fully evolved Totems
                else if (CHK_ForceTotem.Checked && Totem.Contains(t.Species))
                    t.Species = FinalEvo[randFinalEvo()];

                // every other entry
                else
                    t.Species = specrand.GetRandomSpecies(t.Species);

                if (CHK_AllowMega.Checked)
                    formrand.AllowMega = true;

                if (CHK_Item.Checked)
                    t.HeldItem = items[Util.Random32() % items.Length];

                if (CHK_Level.Checked)
                    t.Level = Randomizer.GetModifiedLevel(t.Level, NUD_LevelBoost.Value);

                if (CHK_RemoveShinyLock.Checked)
                    t.ShinyLock = false;

                if (CHK_RandomAura.Checked && t.Aura != 0) // don't apply aura to a pkm without it
                    t.Aura = Util.Rand.Next(1, CB_Aura.Items.Count); // don't allow none

                if (CHK_RandomAbility.Checked)
                    t.Ability = (sbyte)(Util.Rand.Next(1, 4)); // 1, 2, or H

                if (CHK_ForceFullyEvolved.Checked && t.Level >= NUD_ForceFullyEvolved.Value && !FinalEvo.Contains(t.Species))
                    t.Species = FinalEvo[randFinalEvo()];

                t.IVs = t.IV3
                    ? new[] { -4, -1, -1, -1, -1, -1 }  // random with IV3 flag
                    : new[] { -1, -1, -1, -1, -1, -1 }; // random

                t.EVs = new[] { 0, 0, 0, 0, 0, 0 }; // reset EVs

                t.Form = Randomizer.GetRandomForme(t.Species, CHK_AllowMega.Checked, true, Main.SpeciesStat);
                t.Gender = 0; // random
                t.Nature = 0; // random

                t.RelearnMoves = CHK_Metronome.Checked
                    ? new[] {118, 0, 0, 0}
                    : move.GetCurrentMoves(t.Species, t.Form, t.Level, 4);
            }
            foreach (EncounterTrade7 t in Trades)
            {
                t.Species = specrand.GetRandomSpecies(t.Species);
                t.TradeRequestSpecies = specrand.GetRandomSpecies(t.TradeRequestSpecies);

                if (CHK_AllowMega.Checked)
                    formrand.AllowMega = true;

                if (CHK_Item.Checked)
                    t.HeldItem = items[Util.Random32() % items.Length];

                if (CHK_Level.Checked)
                    t.Level = Randomizer.GetModifiedLevel(t.Level, NUD_LevelBoost.Value);

                if (CHK_RandomAbility.Checked)
                    t.Ability = (sbyte)(Util.Rand.Next(0, 3)); // 1, 2, or H

                if (CHK_ForceFullyEvolved.Checked && t.Level >= NUD_ForceFullyEvolved.Value && !FinalEvo.Contains(t.Species))
                    t.Species = FinalEvo[randFinalEvo()]; // only do offered species to be fair

                t.Form = Randomizer.GetRandomForme(t.Species, CHK_AllowMega.Checked, true, Main.SpeciesStat);
                t.Nature = (int)(Util.Random32() % CB_TNature.Items.Count); // randomly selected
                t.IVs = new[] { -1, -1, -1, -1, -1, -1 }; // random
            }

            GetListBoxEntries();
            GetGift();
            GetEncounter();
            GetTrade();

            WinFormsUtil.Alert("Randomized Static Encounters according to specification!");
        }

        // Mirror Changes
        private void UpdateStarterText()
        {
            var gr = Main.Config.GetGARCReference("storytext");
            int file = Main.Config.USUM ? 39 : 41;
            for (int i = 0; i < 10; i++)
            {
                // get Story Text
                var sr = gr.GetRelativeGARC(i, gr.Name);
                var s = Main.Config.GetGARCByReference(sr);
                byte[][] storytextdata = s.Files;

                string[] storyText = TextFile.GetStrings(Main.Config, storytextdata[file]);

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
                        int oldIndex = Main.Config.Personal.GetFormIndex(oldSpecies, Gifts[j].Form);
                        int oldtype0 = Main.Config.Personal[oldIndex].Types[0];
                        int newIndex = Main.Config.Personal.GetFormIndex(species, Gifts[j].Form);
                        int newtype0 = Main.Config.Personal[newIndex].Types[0];
                        line = line.Replace(types[oldtype0], types[newtype0]);
                    }
                    else if (Main.Config.USUM)
                    {
                        storyText[14 + j] = specieslist[species];
                    }

                    storyText[1 + j] = line;
                }
                storytextdata[file] = TextFile.GetBytes(Main.Config, storyText);
                s.Files = storytextdata;
                s.Save();
            }
        }

        public void ExportEncounters()
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

            for (int i = 0; i < LB_Encounter.Items.Count; i++)
            {
                LB_Encounter.SelectedIndex = i;
                NUD_ELevel.Value = Randomizer.GetModifiedLevel((int)NUD_ELevel.Value, NUD_LevelBoost.Value);
            }
            for (int i = 0; i < LB_Gift.Items.Count; i++)
            {
                LB_Gift.SelectedIndex = i;
                NUD_GLevel.Value = Randomizer.GetModifiedLevel((int)NUD_GLevel.Value, NUD_LevelBoost.Value);
            }
            for (int i = 0; i < LB_Trade.Items.Count; i++)
            {
                LB_Trade.SelectedIndex = i;
                NUD_TLevel.Value = Randomizer.GetModifiedLevel((int)NUD_TLevel.Value, NUD_LevelBoost.Value);
            }
            WinFormsUtil.Alert("Modified all Levels according to specification!");
        }

        private void B_CurrentAttackSE_Click(object sender, EventArgs e)
        {
            int species = CB_ESpecies.SelectedIndex;
            int lvl = (int)NUD_ELevel.Value;
            int frm = (int)NUD_EForm.Value;
            int[] moves = learn.GetCurrentMoves(species, frm, lvl, 4);
            SetMoves(moves);
        }

        private void B_HighAttackSE_Click(object sender, EventArgs e)
        {
            int species = CB_ESpecies.SelectedIndex;
            int frm = (int)NUD_EForm.Value;
            int[] moves = learn.GetHighPoweredMoves(species, frm, 4);
            SetMoves(moves);
        }

        private void B_ClearSE_Click(object sender, EventArgs e) => SetMoves(new int[4]);

        private void SetMoves(IList<int> moves)
        {
            var mcb = new[] { CB_EMove0, CB_EMove1, CB_EMove2, CB_EMove3 };
            for (int i = 0; i < mcb.Length; i++)
                mcb[i].SelectedIndex = moves[i];
        }
    }
}