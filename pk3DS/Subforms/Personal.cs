using System;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Media;
using System.Text;
using System.Windows.Forms;
using pk3DS.Properties;

namespace pk3DS
{
    public partial class Personal : Form
    {
        public Personal()
        {
            InitializeComponent();
            helditem_boxes = new[] { CB_HeldItem1, CB_HeldItem2, CB_HeldItem3 };
            ability_boxes = new[] { CB_Ability1, CB_Ability2, CB_Ability3 };
            typing_boxes = new[] { CB_Type1, CB_Type2 };
            eggGroup_boxes = new[] { CB_EggGroup1, CB_EggGroup2 };
            byte_boxes = new[] { TB_BaseHP, TB_BaseATK, TB_BaseDEF, TB_BaseSPA, TB_BaseSPD, TB_BaseSPE, TB_Gender, TB_HatchCycles, TB_Friendship, TB_CatchRate };
            ev_boxes = new[] { TB_HPEVs, TB_ATKEVs, TB_DEFEVs, TB_SPEEVs, TB_SPAEVs, TB_SPDEVs };
            rstat_boxes = new[] { CHK_rHP, CHK_rATK, CHK_rDEF, CHK_rSPA, CHK_rSPD, CHK_rSPE };

            data = File.ReadAllBytes(paths.Last()); // Load last to data.
            Setup(); //Turn string resources into arrays
            CB_Species.SelectedIndex = 1;
        }
        #region Global Variables
        private readonly string[] paths = Directory.GetFiles("personal", "*.*", SearchOption.TopDirectoryOnly);
        private readonly string mode = Main.oras ? "ORAS" : "XY";

        private string[] items = { };
        private string[] moves = { };
        private string[] species = { };
        private string[] abilities = { };
        private string[] forms = { };

        private readonly byte[] data = { };

        private readonly ComboBox[] helditem_boxes;
        private readonly ComboBox[] ability_boxes;
        private readonly ComboBox[] typing_boxes;
        private readonly ComboBox[] eggGroup_boxes;

        private readonly MaskedTextBox[] byte_boxes;
        private readonly MaskedTextBox[] ev_boxes;
        private readonly CheckBox[] rstat_boxes;

        public string[] types = { };

        public string[] eggGroups = { "---", "Monster", "Water 1", "Bug", "Flying", "Field", "Fairy", "Grass", "Human-Like", "Water 3", "Mineral", "Amorphous", "Water 2", "Ditto", "Dragon", "Undiscovered" };
        public string[] EXPGroups = { "Medium-Fast", "Erratic", "Fluctuating", "Medium-Slow", "Fast", "Slow" };
        public string[] colors = { "Red", "Blue", "Yellow", "Green", "Black", "Brown", "Purple", "Gray", "White", "Pink" };

        /*
        public string[] tutormoves = { "Frenzy Plant", "Blast Burn", "Hydro Cannon", "Grass Pledge", "Fire Pledge", "Water Pledge", "Draco Meteor", "Dragon's Ascent" };
        public string[] tutor1 = { "Bug Bite", "Covet", "Super Fang", "Dual Chop", "Signal Beam", "Iron Head", "Seed Bomb", "Drill Run", "Bounce", "Low Kick", "Gunk Shot", "Uproar", "Thunder Punch", "Fire Punch", "Ice Punch" };
        public string[] tutor2 = { "Magic Coat", "Block", "Earth Power", "Foul Play", "Gravity", "Magnet Rise", "Iron Defense", "Last Resort", "Superpower", "Electroweb", "Icy Wind", "Aqua Tail", "Dark Pulse", "Zen Headbutt", "Dragon Pulse", "Hyper Voice", "Iron Tail" };
        public string[] tutor3 = { "Bind", "Snore", "Knock Off", "Synthesis", "Heat Wave", "Role Play", "Heal Bell", "Tailwind", "Sky Attack", "Pain Split", "Giga Drain", "Drain Punch", "Air Cutter", "Focus Punch", "Shock Wave", "Water Pulse" };
        public string[] tutor4 = { "Gastro Acid", "Worry Seed", "Spite", "After You", "Helping Hand", "Trick", "Magic Room", "Wonder Room", "Endeavor", "Outrage", "Recycle", "Snatch", "Stealth Rock", "Sleep Talk", "Skill Swap" };
        */
        public ushort[] tutormoves = { 338, 307, 308, 520, 519, 518, 434, 620 };
        public ushort[] tutor1 = { 450, 343, 162, 530, 324, 442, 402, 529, 340, 67, 441, 253, 9, 7, 8 };
        public ushort[] tutor2 = { 277, 335, 414, 492, 356, 393, 334, 387, 276, 527, 196, 401, 399, 428, 406, 304, 231 };
        public ushort[] tutor3 = { 20, 173, 282, 235, 257, 272, 215, 366, 143, 220, 202, 409, 314, 264, 351, 352 };
        public ushort[] tutor4 = { 380, 388, 180, 495, 270, 271, 478, 472, 283, 200, 278, 289, 446, 214, 285 };

        private string[][] AltForms;
        int entrysize = Main.oras ? 0x50 : 0x40;
        int entry = -1;
        #endregion
        private void Setup()
        {
            abilities = Main.getText(Main.oras ? 37 : 34);
            moves = Main.getText(Main.oras ? 14 : 13);
            items = Main.getText(Main.oras ? 114 : 96);
            species = Main.getText(Main.oras ? 98 : 80);
            types = Main.getText(Main.oras ? 18 : 17);
            forms = Main.getText(Main.oras ? 5 : 5);
            species[0] = "---";
            abilities[0] = items[0] = moves[0] = "";
            AltForms = getFormList(data, Main.oras, species, forms, types, items);
            species = getPersonalEntryList(data, Main.oras, AltForms, species);

            ushort[] TMs = new ushort[0];
            ushort[] HMs = new ushort[0];
            TMHM.getTMHMList(Main.oras, ref TMs, ref HMs);
            CLB_TMHM.Items.Clear();
            int hmcount = Main.oras ? 7 : 5;

            if (TMs.Length == 0) // No ExeFS to grab TMs from.
            {
                for (int i = 1; i <= 100; i++)
                    CLB_TMHM.Items.Add("TM" + i.ToString("00"));
                for (int i = 1; i <= hmcount; i++)
                    CLB_TMHM.Items.Add("HM" + i.ToString("00"));
            }
            else // Use TMHM moves.
            {
                for (int i = 1; i <= 100; i++)
                    CLB_TMHM.Items.Add($"TM{i.ToString("00")} {moves[TMs[i - 1]]}");
                for (int i = 1; i <= hmcount; i++)
                    CLB_TMHM.Items.Add($"HM{i.ToString("00")} {moves[HMs[i - 1]]}");
            }
            for (int i = 0; i < tutormoves.Length - 1; i++)
                CLB_MoveTutors.Items.Add(moves[tutormoves[i]]);

            if (mode == "XY")
            {
                string[] temp_items = new string[718]; // 719 items in XY
                Array.Copy(items, temp_items, temp_items.Length);
                items = temp_items;

                string[] temp_moves = new string[moves.Length - 4]; // 4 new moves added in ORAS
                Array.Copy(moves, temp_moves, temp_moves.Length);
                moves = temp_moves;

                string[] temp_species = new string[799]; // 799 species in XY
                Array.Copy(species, temp_species, temp_species.Length);
                species = temp_species;

                CLB_OrasTutors.Visible = 
                CLB_OrasTutors.Enabled =
                L_ORASTutors.Visible = false;
            }
            else if (mode == "ORAS")
            {
                CLB_MoveTutors.Items.Add(moves[tutormoves[tutormoves.Length - 1]]); //Dragon's Ascent
                foreach (ushort tm in tutor1)
                    CLB_OrasTutors.Items.Add(moves[tm]);
                foreach (ushort tm in tutor2)
                    CLB_OrasTutors.Items.Add(moves[tm]);
                foreach (ushort tm in tutor3)
                    CLB_OrasTutors.Items.Add(moves[tm]);
                foreach (ushort tm in tutor4)
                    CLB_OrasTutors.Items.Add(moves[tm]);

                CLB_OrasTutors.Visible = 
                CLB_OrasTutors.Enabled = 
                L_ORASTutors.Visible = true;
            }
            for (int i = 0; i < species.Length; i++)
                CB_Species.Items.Add($"{species[i]} - {i.ToString("000")}");

            foreach (ComboBox cb in helditem_boxes)
                foreach (string it in items)
                    cb.Items.Add(it);

            foreach (ComboBox cb in ability_boxes)
                foreach (string ab in abilities)
                    cb.Items.Add(ab);

            foreach (ComboBox cb in typing_boxes)
                foreach (string ty in types)
                    cb.Items.Add(ty);

            foreach (ComboBox cb in eggGroup_boxes)
                foreach (string eg in eggGroups)
                    cb.Items.Add(eg);

            foreach (string co in colors)
                CB_Color.Items.Add(co);

            foreach (string eg in EXPGroups)
                CB_EXPGroup.Items.Add(eg);
        }

        private void CB_Species_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (entry > -1 && !dumping) saveEntry();
            entry = CB_Species.SelectedIndex;
            readEntry();
        }
        private void ByteLimiter(object sender, EventArgs e)
        {
            MaskedTextBox mtb = sender as MaskedTextBox;
            int val;
            int.TryParse(mtb.Text, out val);
            if (Array.IndexOf(byte_boxes, mtb) > -1 && val > 255)
                mtb.Text = "255";
            else if (Array.IndexOf(ev_boxes, mtb) > -1 && val > 3)
                mtb.Text = "3";
        }

        private PersonalInfo pkm;
        private void readInfo()
        {
            byte[] array = new byte[Main.oras ? 0x50 : 0x40];
            Array.Copy(data, array.Length * entry, array, 0, array.Length);
            pkm = new PersonalInfo(array);

            TB_BaseHP.Text = pkm.HP.ToString("000");
            TB_BaseATK.Text = pkm.ATK.ToString("000");
            TB_BaseDEF.Text = pkm.DEF.ToString("000");
            TB_BaseSPE.Text = pkm.SPE.ToString("000");
            TB_BaseSPA.Text = pkm.SPA.ToString("000");
            TB_BaseSPD.Text = pkm.SPD.ToString("000");
            TB_HPEVs.Text = pkm.EV_HP.ToString("0");
            TB_ATKEVs.Text = pkm.EV_ATK.ToString("0");
            TB_DEFEVs.Text = pkm.EV_DEF.ToString("0");
            TB_SPEEVs.Text = pkm.EV_SPE.ToString("0");
            TB_SPAEVs.Text = pkm.EV_SPA.ToString("0");
            TB_SPDEVs.Text = pkm.EV_SPD.ToString("0");
            
            CB_Type1.SelectedIndex = pkm.Types[0];
            CB_Type2.SelectedIndex = pkm.Types[1];

            TB_CatchRate.Text = pkm.CatchRate.ToString("000");
            TB_Stage.Text = pkm.EvoStage.ToString("0");

            CB_HeldItem1.SelectedIndex = pkm.Items[0];
            CB_HeldItem2.SelectedIndex = pkm.Items[1];
            CB_HeldItem3.SelectedIndex = pkm.Items[2];

            TB_Gender.Text = pkm.Gender.ToString("000");
            TB_HatchCycles.Text = pkm.HatchCycles.ToString("000");
            TB_Friendship.Text = pkm.BaseFriendship.ToString("000");

            CB_EXPGroup.SelectedIndex = pkm.EXPGrowth;

            CB_EggGroup1.SelectedIndex = pkm.EggGroups[0];
            CB_EggGroup2.SelectedIndex = pkm.EggGroups[1];

            CB_Ability1.SelectedIndex = pkm.Abilities[0];
            CB_Ability2.SelectedIndex = pkm.Abilities[1];
            CB_Ability3.SelectedIndex = pkm.Abilities[2];

            TB_FormeCount.Text = pkm.FormeCount.ToString("000");
            TB_FormeSprite.Text = pkm.FormeSprite.ToString("000");

            TB_RawColor.Text = pkm.Color.ToString("000");
            CB_Color.SelectedIndex = pkm.Color & 0xF;

            TB_BaseExp.Text = pkm.BaseEXP.ToString("000");

            TB_Height.Text = (pkm.Height / 100).ToString("00.0");
            TB_Weight.Text = (pkm.Weight / 10).ToString("000.0");

            for (int i = 0; i < CLB_TMHM.Items.Count; i++)
                CLB_TMHM.SetItemChecked(i, pkm.TMHM[i]); // Bitflags for TMHM

            for (int i = 0; i < CLB_MoveTutors.Items.Count; i++)
                CLB_MoveTutors.SetItemChecked(i, pkm.Tutors[i]); // Bitflags for Tutors

            if (pkm.ORASTutors[0] != null)
            {
                int[] len = { tutor1.Length, tutor2.Length, tutor3.Length, tutor4.Length };
                int ctr = 0;
                for (int i = 0; i < len.Length; i++)
                    for (int b = 0; b < len[i]; b++)
                        CLB_OrasTutors.SetItemChecked(ctr++, pkm.ORASTutors[i][b]);
            }
        }
        private void readEntry()
        {
            readInfo();
            
            if (dumping) return;
            int[] specForm = getSpecies(data, Main.oras, CB_Species.SelectedIndex);
            string filename = "_" + specForm[0] + (CB_Species.SelectedIndex > 721 ? "_" + (specForm[1] + 1) : "");
            Bitmap rawImg = (Bitmap)Resources.ResourceManager.GetObject(filename);
            Bitmap bigImg = new Bitmap(rawImg.Width * 2, rawImg.Height * 2);
            for (int x = 0; x < rawImg.Width; x++)
            {
                for (int y = 0; y < rawImg.Height; y++)
                {
                    Color c = rawImg.GetPixel(x, y);
                    bigImg.SetPixel(2 * x, 2 * y, c);
                    bigImg.SetPixel(2 * x + 1, 2 * y, c);
                    bigImg.SetPixel(2 * x, 2 * y + 1, c);
                    bigImg.SetPixel(2 * x + 1, 2 * y + 1, c);
                }
            }
            PB_MonSprite.Image = bigImg;
        }
        private void savePersonal()
        {
            pkm.HP = Convert.ToByte(TB_BaseHP.Text);
            pkm.ATK = Convert.ToByte(TB_BaseATK.Text);
            pkm.DEF = Convert.ToByte(TB_BaseDEF.Text);
            pkm.SPE = Convert.ToByte(TB_BaseSPE.Text);
            pkm.SPA = Convert.ToByte(TB_BaseSPA.Text);
            pkm.SPD = Convert.ToByte(TB_BaseSPD.Text);

            pkm.EV_HP = Convert.ToByte(TB_HPEVs.Text);
            pkm.EV_ATK = Convert.ToByte(TB_ATKEVs.Text);
            pkm.EV_DEF = Convert.ToByte(TB_DEFEVs.Text);
            pkm.EV_SPE = Convert.ToByte(TB_SPEEVs.Text);
            pkm.EV_SPA = Convert.ToByte(TB_SPAEVs.Text);
            pkm.EV_SPD = Convert.ToByte(TB_SPDEVs.Text);

            pkm.CatchRate = Convert.ToByte(TB_CatchRate.Text);
            pkm.EvoStage = Convert.ToByte(TB_Stage.Text);

            pkm.Types[0] = (byte) CB_Type1.SelectedIndex;
            pkm.Types[1] = (byte) CB_Type2.SelectedIndex;
            pkm.Items[0] = (ushort) CB_HeldItem1.SelectedIndex;
            pkm.Items[1] = (ushort) CB_HeldItem2.SelectedIndex;
            pkm.Items[2] = (ushort) CB_HeldItem3.SelectedIndex;

            pkm.Gender = Convert.ToByte(TB_Gender.Text);
            pkm.HatchCycles = Convert.ToByte(TB_HatchCycles.Text);
            pkm.BaseFriendship = Convert.ToByte(TB_Friendship.Text);
            pkm.EXPGrowth = (byte) CB_EXPGroup.SelectedIndex;
            pkm.EggGroups[0] = (byte) CB_EggGroup1.SelectedIndex;
            pkm.EggGroups[1] = (byte) CB_EggGroup2.SelectedIndex;

            pkm.Abilities[0] = (byte) CB_Ability1.SelectedIndex;
            pkm.Abilities[1] = (byte) CB_Ability2.SelectedIndex;
            pkm.Abilities[2] = (byte) CB_Ability3.SelectedIndex;

            pkm.FormeSprite = Convert.ToUInt16(TB_FormeSprite.Text);
            pkm.FormeCount = Convert.ToByte(TB_FormeCount.Text);
            pkm.Color = (byte) (Convert.ToByte(CB_Color.SelectedIndex) | (Convert.ToByte(TB_RawColor.Text) & 0xF0));
            pkm.BaseEXP = Convert.ToUInt16(TB_BaseExp.Text);

            pkm.Height = (float) Convert.ToDouble(TB_Height.Text)*100;
            pkm.Weight = (float) Convert.ToDouble(TB_Weight.Text)*10;

            for (int i = 0; i < CLB_TMHM.Items.Count; i++)
                pkm.TMHM[i] = CLB_TMHM.GetItemChecked(i);

            for (int t = 0; t < CLB_MoveTutors.Items.Count; t++)
                pkm.Tutors[t] = CLB_MoveTutors.GetItemChecked(t);

            if (!Main.oras) return;

            int[] len = {tutor1.Length, tutor2.Length, tutor3.Length, tutor4.Length};
            int ctr = 0;
            for (int i = 0; i < 4; i++)
                for (int t = 0; t < len[i]; t++)
                    pkm.ORASTutors[i][t] = CLB_OrasTutors.GetItemChecked(ctr++);
        }
        private void saveEntry()
        {
            savePersonal();
            byte[] edits = pkm.Write();
            File.WriteAllBytes(paths[entry], edits);
            Array.Copy(edits, 0, data, entry * edits.Length, edits.Length);
            File.WriteAllBytes(paths[paths.Length - 1], data);
        }

        private void B_Randomize_Click(object sender, EventArgs e)
        {
            Random rnd = new Random();
            const int TMPercent = 35; // Average Learnable TMs is 35.260.
            const int TutorPercent = 2; //136 special tutor moves learnable by species in Untouched ORAS.
            const int OrasTutorPercent = 30; //10001 tutor moves learnable by 826 species in Untouched ORAS.
            ushort[] itemlist = Main.oras ? Legal.Pouch_Items_ORAS : Legal.Pouch_Items_XY;
            ushort[] berrylist = Legal.Pouch_Berry_XY;
            Array.Resize(ref itemlist, itemlist.Length + berrylist.Length);
            Array.Copy(berrylist, 0, itemlist, itemlist.Length - berrylist.Length, berrylist.Length);

            int itemlen = itemlist.Length;
            int abillen = CB_Ability1.Items.Count;
            int typelen = CB_Type1.Items.Count;

            for (int i = 1; i < CB_Species.Items.Count; i++)
            {
                CB_Species.SelectedIndex = i; // Get new Species

                // Fiddle with TM Learnsets
                if (CHK_TM.Checked)
                    for (int t = 0; t < 100; t++)
                        CLB_TMHM.SetItemCheckState(t, rnd.Next(0, 100) < TMPercent ? CheckState.Checked : CheckState.Unchecked);
                if (CHK_HM.Checked)
                    for (int t = 100; t < CLB_TMHM.Items.Count;t++)
                        CLB_TMHM.SetItemCheckState(t, rnd.Next(0, 100) < TMPercent ? CheckState.Checked : CheckState.Unchecked);
                if (CHK_Tutors.Checked)
                {
                    for (int t = 0; t < CLB_MoveTutors.Items.Count; t++)
                        CLB_MoveTutors.SetItemCheckState(t, rnd.Next(0, 100) < TutorPercent ? CheckState.Checked : CheckState.Unchecked);
                    if (Main.oras && (CB_Species.SelectedIndex == 384 || CB_Species.SelectedIndex == 814)) //Make sure Rayquaza can learn Dragon Ascent.
                        CLB_MoveTutors.SetItemCheckState(CLB_MoveTutors.Items.Count-1, CheckState.Checked); 
                }
                if (Main.oras && CHK_ORASTutors.Checked)
                    for (int t = 0; t < CLB_OrasTutors.Items.Count; t++)
                        CLB_OrasTutors.SetItemCheckState(t, rnd.Next(0, 100) < OrasTutorPercent ? CheckState.Checked : CheckState.Unchecked);

                // Abilities:
                if (CHK_Ability.Checked)
                {
                    ComboBox[] abils = {CB_Ability1, CB_Ability2, CB_Ability3};
                    for (int a = 0; a < 3; a++) // Set 3 New Abilities, none being Wonder Guard (25) unless CHK_WGuard is checked.
                    {
                        int newabil = rnd.Next(1, abillen);
                        while (newabil == 25 && !CHK_WGuard.Checked)
                            newabil = rnd.Next(1, abillen);
                        if (abils[a].SelectedIndex != 25 || CHK_WGuard.Checked) abils[a].SelectedIndex = newabil;
                    }
                }

                // Fiddle with Base Stats, don't muck with Shedinja.
                if (CHK_Stats.Checked)
                {
                    if (Convert.ToByte(byte_boxes[0].Text) != 1)
                        for (int z = 0; z < 6; z++)
                            if (rstat_boxes[z].Checked)
                                byte_boxes[z].Text =
                                    Math.Max(5,rnd.Next(
                                            Math.Min(255, (int) (Convert.ToByte(byte_boxes[z].Text)*(1 - NUD_StatDev.Value/100))),
                                            Math.Min(255, (int) (Convert.ToByte(byte_boxes[z].Text)*(1 + NUD_StatDev.Value/100)))
                                            )).ToString("000");
                }
                // EV yield stays the same...

                if (CHK_CatchRate.Checked)
                    TB_CatchRate.Text = rnd.Next(3, 251).ToString("000"); //Random Catch Rate between 3 and 250. Should I make this normally distributed?
                if (CHK_EggGroup.Checked)
                {
                    if (rnd.Next(0, 100) < NUD_Egg.Value) // 50% chance to have either One or Two Egg Groups
                        CB_EggGroup1.SelectedIndex = CB_EggGroup2.SelectedIndex = rnd.Next(1, CB_EggGroup1.Items.Count);
                    else
                    {
                        CB_EggGroup1.SelectedIndex = rnd.Next(1, CB_EggGroup1.Items.Count);
                        CB_EggGroup2.SelectedIndex = rnd.Next(1, CB_EggGroup1.Items.Count);
                    }
                }

                // Items
                if (CHK_Item.Checked)
                {
                    CB_HeldItem1.SelectedIndex = itemlist[rnd.Next(1, itemlen)];
                    CB_HeldItem2.SelectedIndex = itemlist[rnd.Next(1, itemlen)];
                    CB_HeldItem3.SelectedIndex = itemlist[rnd.Next(1, itemlen)];
                }

                // Type
                if (CHK_Type.Checked)
                {
                    if (rnd.Next(0, 100) < NUD_TypePercent.Value) // 50% chance to have either Single or Dual Typing
                        CB_Type1.SelectedIndex = CB_Type2.SelectedIndex = rnd.Next(0, typelen);
                    else
                    {
                        CB_Type1.SelectedIndex = rnd.Next(0, typelen);
                        CB_Type2.SelectedIndex = rnd.Next(0, typelen);
                    }
                }
            }
            saveEntry();
            Util.Alert("All relevant Pokemon Personal Entries have been randomized!");
        }
        private void B_ModifyAll(object sender, EventArgs e)
        {
            for (int i = 1; i < CB_Species.Items.Count; i++)
            {
                CB_Species.SelectedIndex = i; // Get new Species

                if (CHK_NoEV.Checked)
                    for (int z = 0; z < 6; z++)
                        ev_boxes[z].Text = 0.ToString();
                if (CHK_LowCatch.Checked)
                    TB_CatchRate.Text = 3.ToString("000");
                if (CHK_Growth.Checked)
                    CB_EXPGroup.SelectedIndex = 5;
                if (CHK_EXP.Checked)
                    TB_BaseExp.Text = ((float)NUD_EXP.Value*(Convert.ToUInt16(TB_BaseExp.Text)/100f)).ToString("000");

                if (CHK_QuickHatch.Checked)
                    TB_HatchCycles.Text = 1.ToString();
            }
            CB_Species.SelectedIndex = 1;
            Util.Alert("All species modified to specification!");
        }
        private bool dumping;
        private void B_Dump_Click(object sender, EventArgs e)
        {

            if (DialogResult.Yes != Util.Prompt(MessageBoxButtons.YesNo, "Dump all Personal Entries to Text File?"))
                return;

            dumping = true;
            string result = "";
            for (int i = 0; i < CB_Species.Items.Count; i++)
            {
                CB_Species.SelectedIndex = i; // Get new Species
                result += "======" + Environment.NewLine + entry + " " + CB_Species.Text + Environment.NewLine + "======" + Environment.NewLine;

                result +=
                    $"Base Stats: {TB_BaseHP.Text}.{TB_BaseATK.Text}.{TB_BaseDEF.Text}.{TB_BaseSPA.Text}.{TB_BaseSPD.Text}.{TB_BaseSPE.Text} (BST: {pkm.BST})" + Environment.NewLine;
                result +=
                    $"EV Yield: {TB_HPEVs.Text}.{TB_ATKEVs.Text}.{TB_DEFEVs.Text}.{TB_SPAEVs.Text}.{TB_SPDEVs.Text}.{TB_SPEEVs.Text}" + Environment.NewLine;
                result += $"Abilities: {CB_Ability1.Text} (1) | {CB_Ability2.Text} (2) | {CB_Ability3.Text} (H)" + Environment.NewLine;

                result += string.Format(CB_Type1.SelectedIndex != CB_Type2.SelectedIndex ? "Type: {0} / {1}" : "Type: {0}", CB_Type1.Text, CB_Type2.Text);

                result += $"Item 1 (50%): {CB_HeldItem1.Text}" + Environment.NewLine;
                result += $"Item 2 (5%): {CB_HeldItem2.Text}" + Environment.NewLine;
                result += $"Item 3 (1%): {CB_HeldItem3.Text}" + Environment.NewLine;
                // I don't want to add anything else. Should be pretty easy for anyone else to expand.

                result += Environment.NewLine;
            }

            SaveFileDialog sfd = new SaveFileDialog {FileName = "Personal Entries.txt", Filter = "Text File|*.txt"};

            SystemSounds.Asterisk.Play();
            if (sfd.ShowDialog() == DialogResult.OK)
            {
                string path = sfd.FileName;
                File.WriteAllText(path, result, Encoding.Unicode);
            }
            dumping = false;
        }
        private void CHK_Stats_CheckedChanged(object sender, EventArgs e)
        {
            L_StatDev.Visible = NUD_StatDev.Visible = CHK_Stats.Checked;
        }
        private void CHK_Ability_CheckedChanged(object sender, EventArgs e)
        {
            CHK_WGuard.Enabled = CHK_Ability.Checked;
            if (!CHK_WGuard.Enabled)
                CHK_WGuard.Checked = false;
        }

        private void formClosing(object sender, FormClosingEventArgs e)
        {
            if (entry > -1) saveEntry();
        }

        // Utility (Shared)
        internal static int[] getSpecies(byte[] data, bool oras, int PersonalEntry)
        {
            int entrysize = oras ? 0x50 : 0x40;
            if (PersonalEntry < 722) return new[] { PersonalEntry, 0 };

            for (int i = 0; i < 722; i++)
            {
                int FormCount = data[i * entrysize + 0x20] - 1; // Mons with no alt forms have a FormCount of 1.
                ushort altformpointer = BitConverter.ToUInt16(data, entrysize * i + 0x1C);
                if (altformpointer <= 0) continue;
                for (int j = 0; j < FormCount; j++)
                    if (altformpointer + j == PersonalEntry)
                        return new[] { i, j };
            }

            return new[] { -1, -1 };
        }
        internal static string[][] getFormList(byte[] data, bool oras, string[] species, string[] forms, string[] types, string[] items)
        {
            try
            {
                string[][] FormList = new string[722][];
                int entrysize = oras ? 0x50 : 0x40;
                int AltFormOfs = 723; //null + 721 species + 1 gap
                for (int i = 0; i < 722; i++) //Hardcode 721 species + null
                {
                    int FormCount = data[i * entrysize + 0x20]; // Mons with no alt forms have a FormCount of 1.
                    FormList[i] = new string[FormCount];
                    if (FormCount <= 0) continue;
                    FormList[i][0] = forms[i] == "" ? species[i] : forms[i];
                    for (int j = 1; j < FormCount; j++)
                        FormList[i][j] = forms[AltFormOfs++];
                }
                // Need to hardcode here: Unown, Arceus, Genesect, any others?
                // Unown
                for (int i = 0; i < 26; i++)
                    FormList[201][i] = ((char)(i + 0x41)).ToString();
                FormList[201][26] = "!";
                FormList[201][27] = "?";
                // Arceus
                for (int i = 0; i < types.Length; i++)
                    FormList[493][i] = types[i];
                // Genesect
                for (int i = 0; i < 4; i++)
                    FormList[649][1 + i] = items[i + 116];

                return FormList;
            }
            catch (Exception e) { Util.Error("Error while creating the Alternate Formes Listing.", "Please make sure that the Personal.dat & Forms text are as intended", "Error:" + Environment.NewLine + e); return null; }
        }
        internal static string[] getPersonalEntryList(byte[] data, bool oras, string[][] AltForms, string[] species)
        {
            int entrysize = oras ? 0x50 : 0x40;
            string[] result = new string[data.Length / entrysize];
            for (int i = 0; i < 722; i++)
            {
                result[i] = species[i];
                if (AltForms[i].Length == 0) continue;
                ushort altformpointer = BitConverter.ToUInt16(data, entrysize * i + 0x1C);
                if (altformpointer <= 0) continue;
                for (int j = 1; j < AltForms[i].Length; j++)
                    result[altformpointer + j - 1] = AltForms[i][j];
            }
            return result;
        }
        internal static ushort[] getPersonalIndexList(byte[] data, bool oras)
        {
            ushort[] result = new ushort[722];
            int entrysize = oras ? 0x50 : 0x40;
            for (int i = 0; i < result.Length; i++)
                result[i] = BitConverter.ToUInt16(data, entrysize * i + 0x1C);
            return result;
        }
        internal static void setForms(int species, ComboBox cb, string[][] AltForms)
        {
            cb.Items.Clear();
            string[] forms = AltForms[species];
            if (forms.Length < 2)
            {
                cb.Items.Add("");
                cb.Enabled = false;
            }
            else
            {
                foreach (string s in forms)
                    cb.Items.Add(s);
                cb.Enabled = true;
            }
            cb.SelectedIndex = 0;
        }

        internal static string[] getSpeciesIndexStrings(bool oras)
        {
            string[] items = Main.getText(Main.oras ? 114 : 96);
            string[] species = Main.getText(Main.oras ? 98 : 80);
            string[] types = Main.getText(Main.oras ? 18 : 17);
            string[] forms = Main.getText(Main.oras ? 5 : 5);
            species[0] = "---";
            byte[] data = File.ReadAllBytes(Directory.GetFiles("personal").Last());
            string[][] AltForms = getFormList(data, Main.oras, species, forms, types, items);
            species = getPersonalEntryList(data, Main.oras, AltForms, species);
            Array.Resize(ref species, oras ? species.Length : 799);
            return species;
        }

        internal static PersonalInfo[] getPersonalArray(string Master)
        {
            byte[] data = File.ReadAllBytes(Master);
            int EntryLength = Main.oras ? 0x50 : 0x40;
            PersonalInfo[] piA = new PersonalInfo[data.Length/EntryLength];
            for (int i = 0; i < piA.Length; i++)
                piA[i] = new PersonalInfo(data.Skip(EntryLength*i).Take(EntryLength).ToArray());
            return piA;
        }
    }
}