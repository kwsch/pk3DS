using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Forms;

using pk3DS.Core;
using pk3DS.Core.Randomizers;
using pk3DS.Core.Structures;

namespace pk3DS
{
    public partial class GiftEditor6 : Form
    {
        public GiftEditor6()
        {
            specieslist[0] = "---";
            Array.Resize(ref specieslist, Main.Config.MaxSpeciesID + 1);
            if (!File.Exists(FieldPath))
            {
                WinFormsUtil.Error("CRO does not exist! Closing.", FieldPath);
                Close();
            }
            InitializeComponent();
            Dictionary<int, int[]> megaDictionary = GetMegaDictionary(Main.Config);
            MegaDictionary = megaDictionary;

            specieslist[0] = "---";
            itemlist[0] = "(None)"; // blank == -1

            CB_Species.Items.Clear();
            foreach (string s in specieslist)
                CB_Species.Items.Add(s);
            CB_HeldItem.Items.Clear();
            foreach (string s in itemlist)
                CB_HeldItem.Items.Add(s);
            CB_Nature.Items.Add("Random");
            CB_Nature.Items.AddRange(natureslist.Take(25).ToArray());
            RandSettings.GetFormSettings(this, tabPage2.Controls);

            LoadData();
        }

        public static Dictionary<int, int[]> GetMegaDictionary(GameConfig config)
        {
            return config.XY ? MegaDictionaryXY : MegaDictionaryAO.Concat(MegaDictionaryXY)
                            .ToDictionary(pair => pair.Key, pair => pair.Value);
        }

        private readonly string FieldPath = Path.Combine(Main.RomFSPath, "DllField.cro");
        private byte[] FieldData;
        private readonly int fieldOffset = Main.Config.ORAS ? 0xF906C : 0xF805C;
        private readonly int fieldSize = Main.Config.ORAS ? 0x24 : 0x18;
        private readonly int count = Main.Config.ORAS ? 0x25 : 0x13;
        private EncounterGift6[] GiftData;
        //private readonly string[] abilitylist = Main.Config.GetText(TextName.AbilityNames);
        //private readonly string[] movelist = Main.Config.GetText(TextName.MoveNames);
        private readonly string[] itemlist = Main.Config.GetText(TextName.ItemNames);
        private readonly string[] specieslist = Main.Config.GetText(TextName.SpeciesNames);
        private readonly string[] natureslist = Main.Config.GetText(TextName.Natures);
        private readonly Dictionary<int, int[]> MegaDictionary;
        private static int[] FinalEvo = Legal.FinalEvolutions_6;
        private static readonly int[] Legendary = Legal.Legendary_6;
        private static readonly int[] Mythical = Legal.Mythical_6;

        private readonly string[] ability =
        {
            "Any (1 or 2)",
            "Ability 1",
            "Ability 2",
            "Hidden Ability",
        };

        private void B_Save_Click(object sender, EventArgs e)
        {
            SaveEntry();
            SaveData();
            RandSettings.SetFormSettings(this, tabPage2.Controls);
            Close();
        }

        private void B_Cancel_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void LoadData()
        {
            FieldData = File.ReadAllBytes(FieldPath);
            GiftData = new EncounterGift6[count];
            LB_Gifts.Items.Clear();
            for (int i = 0; i < GiftData.Length; i++)
            {
                GiftData[i] = new EncounterGift6(FieldData.Skip(fieldOffset + (i * fieldSize)).Take(fieldSize).ToArray(), Main.Config.ORAS);
                LB_Gifts.Items.Add($"{i:00} - {specieslist[GiftData[i].Species]}");
            }
            foreach (var s in ability) CB_Ability.Items.Add(s);

            CB_Gender.Items.Clear();
            CB_Gender.Items.Add("- / Genderless/Random");
            CB_Gender.Items.Add("♂ / Male");
            CB_Gender.Items.Add("♀ / Female");

            loaded = true;
            LB_Gifts.SelectedIndex = 0;
        }

        private void SaveData()
        {
            // Check to see if a starter has been modified right before we write data.
            bool starters = false;
            int[] entries = Main.Config.ORAS
                ? new[]
                {
                    0, 1, 2, // Gen 3
                    28, 29, 30, // Gen 2
                    31, 32, 33, // Gen 4
                    34, 35, 36 // Gen 5
                }
                : new[]
                {
                    0, 1, 2, // Gen 6
                    3, 4, 5, // Gen 1
                };

            for (int i = 0; i < GiftData.Length; i++)
            {
                int offset = fieldOffset + (i * fieldSize);

                // Check too see if starters got modified
                if (Array.IndexOf(entries, i) > - 1 && BitConverter.ToUInt16(FieldData, offset) != GiftData[i].Species)
                    starters = true;

                // Write new data
                Array.Copy(GiftData[i].Write(), 0, FieldData, offset, fieldSize);
            }

            if (starters) // are modified
                WinFormsUtil.Alert("Starters have been modified.", "Be sure to update the Starters in DllPoke3Select.cro by updating via the Starter Editor.");

            File.WriteAllBytes(FieldPath, FieldData);
        }

        private int entry = -1;
        private bool loaded;

        private void ChangeIndex(object sender, EventArgs e)
        {
            if (LB_Gifts.SelectedIndex < 0)
                return;
            if (!loaded)
                return;
            if (entry != -1)
                SaveEntry();
            entry = LB_Gifts.SelectedIndex;
            LoadEntry();
        }

        private void LoadEntry()
        {
            bool oldloaded = loaded;
            loaded = false;

            CB_Species.SelectedIndex = GiftData[entry].Species;
            CB_HeldItem.SelectedIndex = GiftData[entry].HeldItem;
            NUD_Level.Value = GiftData[entry].Level;
            NUD_Form.Value = GiftData[entry].Form;
            CB_Nature.SelectedIndex = GiftData[entry].Nature + 1;
            CB_Ability.SelectedIndex = GiftData[entry].Ability + 1;
            CB_Gender.SelectedIndex = GiftData[entry].Gender;
            CHK_ShinyLock.Checked = GiftData[entry].ShinyLock;

            NUD_IV0.Value = GiftData[entry].IVs[0];
            NUD_IV1.Value = GiftData[entry].IVs[1];
            NUD_IV2.Value = GiftData[entry].IVs[2];
            NUD_IV3.Value = GiftData[entry].IVs[3];
            NUD_IV4.Value = GiftData[entry].IVs[4];
            NUD_IV5.Value = GiftData[entry].IVs[5];

            if (GiftData[entry].HeldItem < 0)
                CB_HeldItem.SelectedIndex = 0; // no item = 0xFFFF, set to 0 (None)

            loaded |= oldloaded;
        }

        private void SaveEntry()
        {
            GiftData[entry].Species = (ushort)CB_Species.SelectedIndex;
            GiftData[entry].HeldItem = CB_HeldItem.SelectedIndex;
            GiftData[entry].Level = (byte)NUD_Level.Value;
            GiftData[entry].Form = (byte)NUD_Form.Value;
            GiftData[entry].Nature = (sbyte)(CB_Nature.SelectedIndex - 1);
            GiftData[entry].Ability = (sbyte)(CB_Ability.SelectedIndex - 1);
            GiftData[entry].Gender = (sbyte)CB_Gender.SelectedIndex;
            GiftData[entry].ShinyLock = CHK_ShinyLock.Checked;

            GiftData[entry].IVs[0] = (sbyte)NUD_IV0.Value;
            GiftData[entry].IVs[1] = (sbyte)NUD_IV1.Value;
            GiftData[entry].IVs[2] = (sbyte)NUD_IV2.Value;
            GiftData[entry].IVs[3] = (sbyte)NUD_IV3.Value;
            GiftData[entry].IVs[4] = (sbyte)NUD_IV4.Value;
            GiftData[entry].IVs[5] = (sbyte)NUD_IV5.Value;
        }

        private void B_RandAll_Click(object sender, EventArgs e)
        {
            if (WinFormsUtil.Prompt(MessageBoxButtons.YesNo, "Randomize all? Cannot undo.", "Double check Randomization settings in the Randomizer Options tab.") != DialogResult.Yes) return;

            var formrand = new FormRandomizer(Main.Config) { AllowMega = false, AllowAlolanForm = false };
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

            // add Legendary/Mythical to final evolutions if checked
            if (CHK_L.Checked) FinalEvo = FinalEvo.Concat(Legendary).ToArray();
            if (CHK_E.Checked) FinalEvo = FinalEvo.Concat(Mythical).ToArray();

            var helditems = Randomizer.GetRandomItemList();
            for (int i = 0; i < LB_Gifts.Items.Count; i++)
            {
                LB_Gifts.SelectedIndex = i;
                int species = CB_Species.SelectedIndex;

                if (MegaDictionary.Values.Any(z => z.Contains(CB_HeldItem.SelectedIndex))) // Mega Stone Gifts (Lucario, Latias/Latios)
                {
                    if (!CHK_ReplaceMega.Checked)
                        continue; // skip Lucario, battle needs to Mega Evolve

                    int[] items = GetRandomMega(out species);
                    CB_HeldItem.SelectedIndex = items[Util.Rand.Next(0, items.Length)];
                }
                else
                {
                    species = specrand.GetRandomSpecies(species);
                    if (CHK_Item.Checked)
                        CB_HeldItem.SelectedIndex = helditems[Util.Random32() % helditems.Length];
                }

                if (CHK_AllowMega.Checked)
                    formrand.AllowMega = true;

                if (CHK_RemoveShinyLock.Checked)
                    CHK_ShinyLock.Checked = false;

                if (CHK_Level.Checked)
                    NUD_Level.Value = Randomizer.GetModifiedLevel((int)NUD_Level.Value, NUD_LevelBoost.Value);

                if (CHK_RandomAbility.Checked)
                    CB_Ability.SelectedIndex = (Util.Rand.Next(1, 4)); // 1, 2 , or H

                if (CHK_ForceFullyEvolved.Checked && NUD_Level.Value >= NUD_ForceFullyEvolved.Value && !FinalEvo.Contains(species))
                {
                    static int randFinalEvo() => (int)(Util.Random32() % FinalEvo.Length);
                    species = FinalEvo[randFinalEvo()];
                }

                CB_Species.SelectedIndex = species;
                NUD_Form.Value = formrand.GetRandomForme(species);
                CB_Gender.SelectedIndex = 0; // random
                CB_Nature.SelectedIndex = 0; // random
                NUD_IV0.Value = NUD_IV1.Value = NUD_IV2.Value = NUD_IV3.Value = NUD_IV4.Value = NUD_IV5.Value = -1; // random

                if (MegaDictionary.Values.Any(z => z.Contains(CB_HeldItem.SelectedIndex)) && NUD_Form.Value != 0)
                    NUD_Form.Value = 0; // don't allow mega gifts to be form 1
            }
            WinFormsUtil.Alert("Randomized all Gift Pokémon according to specification!");
        }

        private int[] GetRandomMega(out int species)
        {
            int rnd = Util.Rand.Next(0, MegaDictionary.Count - 1);
            species = MegaDictionary.Keys.ElementAt(rnd);
            return MegaDictionary.Values.ElementAt(rnd);
        }

        private static readonly Dictionary<int, int[]> MegaDictionaryXY = new()
        {
            {003, new[] {659}}, // Venusaur @ Venusaurite
            {006, new[] {660, 678}}, // Charizard @ Charizardite X/Y
            {009, new[] {661}}, // Blastoise @ Blastoisinite
            {065, new[] {679}}, // Alakazam @ Alakazite
            {094, new[] {656}}, // Gengar @ Gengarite
            {115, new[] {675}}, // Kangaskhan @ Kangaskhanite
            {127, new[] {671}}, // Pinsir @ Pinsirite
            {130, new[] {676}}, // Gyarados @ Gyaradosite
            {142, new[] {672}}, // Aerodactyl @ Aerodactylite
            {150, new[] {662, 663}}, // Mewtwo @ Mewtwonite X/Y
            {181, new[] {658}}, // Ampharos @ Ampharosite
            {212, new[] {670}}, // Scizor @ Scizorite
            {214, new[] {680}}, // Heracross @ Heracronite
            {229, new[] {666}}, // Houndoom @ Houndoominite
            {248, new[] {669}}, // Tyranitar @ Tyranitarite
            {257, new[] {664}}, // Blaziken @ Blazikenite
            {282, new[] {657}}, // Gardevoir @ Gardevoirite
            {303, new[] {681}}, // Mawile @ Mawilite
            {306, new[] {667}}, // Aggron @ Aggronite
            {308, new[] {665}}, // Medicham @ Medichamite
            {310, new[] {682}}, // Manectric @ Manectite
            {354, new[] {668}}, // Banette @ Banettite
            {359, new[] {677}}, // Absol @ Absolite
            {380, new[] {684}}, // Latias @ Latiasite
            {381, new[] {685}}, // Latios @ Latiosite
            {445, new[] {683}}, // Garchomp @ Garchompite
            {448, new[] {673}}, // Lucario @ Lucarionite
            {460, new[] {674}}, // Abomasnow @ Abomasite
        };

        private static readonly Dictionary<int, int[]> MegaDictionaryAO = new()
        {
            {015, new[] {770}}, // Beedrill @ Beedrillite
            {018, new[] {762}}, // Pidgeot @ Pidgeotite
            {080, new[] {760}}, // Slowbro @ Slowbronite
            {208, new[] {761}}, // Steelix @ Steelixite
            {254, new[] {753}}, // Sceptile @ Sceptilite
            {260, new[] {752}}, // Swampert @ Swampertite
            {302, new[] {754}}, // Sableye @ Sablenite
            {319, new[] {759}}, // Sharpedo @ Sharpedonite
            {323, new[] {767}}, // Camerupt @ Cameruptite
            {334, new[] {755}}, // Altaria @ Altarianite
            {362, new[] {763}}, // Glalie @ Glalitite
            {373, new[] {769}}, // Salamence @ Salamencite
            {376, new[] {758}}, // Metagross @ Metagrossite
            {428, new[] {768}}, // Lopunny @ Lopunnite
            {475, new[] {756}}, // Gallade @ Galladite
            {531, new[] {757}}, // Audino @ Audinite
            {719, new[] {764}}, // Diancie @ Diancite
        };

        private void ChangeSpecies(object sender, EventArgs e)
        {
            int index = LB_Gifts.SelectedIndex;
            LB_Gifts.Items[index] = index.ToString("00") + " - " + CB_Species.Text;
        }

        private void ModifyLevels(object sender, EventArgs e)
        {
            if (WinFormsUtil.Prompt(MessageBoxButtons.YesNo, "Modify all current Levels?", "Cannot undo.") != DialogResult.Yes) return;

            for (int i = 0; i < LB_Gifts.Items.Count; i++)
            {
                LB_Gifts.SelectedIndex = i;
                NUD_Level.Value = Randomizer.GetModifiedLevel((int)NUD_Level.Value, NUD_LevelBoost.Value);
            }
            WinFormsUtil.Alert("Modified all Levels according to specification!");
        }
    }
}