using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Media;
using System.Text;
using System.Windows.Forms;
using pk3DS.Core.Structures.PersonalInfo;
using pk3DS.Core;
using pk3DS.Core.Randomizers;

namespace pk3DS
{
    public partial class PersonalEditor6 : Form
    {
        public PersonalEditor6(byte[][] infiles)
        {
            InitializeComponent();
            helditem_boxes = new[] { CB_HeldItem1, CB_HeldItem2, CB_HeldItem3 };
            ability_boxes = new[] { CB_Ability1, CB_Ability2, CB_Ability3 };
            typing_boxes = new[] { CB_Type1, CB_Type2 };
            eggGroup_boxes = new[] { CB_EggGroup1, CB_EggGroup2 };
            byte_boxes = new[] { TB_BaseHP, TB_BaseATK, TB_BaseDEF, TB_BaseSPA, TB_BaseSPD, TB_BaseSPE, TB_Gender, TB_HatchCycles, TB_Friendship, TB_CatchRate };
            ev_boxes = new[] { TB_HPEVs, TB_ATKEVs, TB_DEFEVs, TB_SPEEVs, TB_SPAEVs, TB_SPDEVs };
            rstat_boxes = new[] { CHK_rHP, CHK_rATK, CHK_rDEF, CHK_rSPA, CHK_rSPD, CHK_rSPE };
            files = infiles;

            abilities = Main.Config.getText(TextName.AbilityNames);
            moves = Main.Config.getText(TextName.MoveNames);
            items = Main.Config.getText(TextName.ItemNames);
            species = Main.Config.getText(TextName.SpeciesNames);
            types = Main.Config.getText(TextName.Types);
            species[0] = "---";
            abilities[0] = items[0] = moves[0] = "";
            string[][] AltForms = Main.Config.Personal.getFormList(species, Main.Config.MaxSpeciesID);
            species = Main.Config.Personal.getPersonalEntryList(AltForms, species, Main.Config.MaxSpeciesID, out baseForms, out formVal);

            Setup(); //Turn string resources into arrays
            CB_Species.SelectedIndex = 1;
        }
        #region Global Variables
        private readonly string mode = Main.Config.ORAS ? "ORAS" : "XY";
        private readonly byte[][] files;

        private string[] items = { };
        private string[] moves = { };
        private string[] species = { };
        private readonly string[] abilities = { };

        private readonly ComboBox[] helditem_boxes;
        private readonly ComboBox[] ability_boxes;
        private readonly ComboBox[] typing_boxes;
        private readonly ComboBox[] eggGroup_boxes;

        private readonly MaskedTextBox[] byte_boxes;
        private readonly MaskedTextBox[] ev_boxes;
        private readonly CheckBox[] rstat_boxes;

        private readonly string[] types = { };

        private readonly string[] eggGroups = { "---", "Monster", "Water 1", "Bug", "Flying", "Field", "Fairy", "Grass", "Human-Like", "Water 3", "Mineral", "Amorphous", "Water 2", "Ditto", "Dragon", "Undiscovered" };
        private readonly string[] EXPGroups = { "Medium-Fast", "Erratic", "Fluctuating", "Medium-Slow", "Fast", "Slow" };
        private readonly string[] colors = { "Red", "Blue", "Yellow", "Green", "Black", "Brown", "Purple", "Gray", "White", "Pink" };

        /*
        public string[] tutormoves = { "Frenzy Plant", "Blast Burn", "Hydro Cannon", "Grass Pledge", "Fire Pledge", "Water Pledge", "Draco Meteor", "Dragon's Ascent" };
        public string[] tutor1 = { "Bug Bite", "Covet", "Super Fang", "Dual Chop", "Signal Beam", "Iron Head", "Seed Bomb", "Drill Run", "Bounce", "Low Kick", "Gunk Shot", "Uproar", "Thunder Punch", "Fire Punch", "Ice Punch" };
        public string[] tutor2 = { "Magic Coat", "Block", "Earth Power", "Foul Play", "Gravity", "Magnet Rise", "Iron Defense", "Last Resort", "Superpower", "Electroweb", "Icy Wind", "Aqua Tail", "Dark Pulse", "Zen Headbutt", "Dragon Pulse", "Hyper Voice", "Iron Tail" };
        public string[] tutor3 = { "Bind", "Snore", "Knock Off", "Synthesis", "Heat Wave", "Role Play", "Heal Bell", "Tailwind", "Sky Attack", "Pain Split", "Giga Drain", "Drain Punch", "Air Cutter", "Focus Punch", "Shock Wave", "Water Pulse" };
        public string[] tutor4 = { "Gastro Acid", "Worry Seed", "Spite", "After You", "Helping Hand", "Trick", "Magic Room", "Wonder Room", "Endeavor", "Outrage", "Recycle", "Snatch", "Stealth Rock", "Sleep Talk", "Skill Swap" };
        */
        private readonly ushort[] tutormoves = { 520, 519, 518, 338, 307, 308, 434, 620 };
        private readonly ushort[] tutor1 = { 450, 343, 162, 530, 324, 442, 402, 529, 340, 067, 441, 253, 009, 007, 008 };
        private readonly ushort[] tutor2 = { 277, 335, 414, 492, 356, 393, 334, 387, 276, 527, 196, 401, 399, 428, 406, 304, 231 };
        private readonly ushort[] tutor3 = { 020, 173, 282, 235, 257, 272, 215, 366, 143, 220, 202, 409, 355, 264, 351, 352 };
        private readonly ushort[] tutor4 = { 380, 388, 180, 495, 270, 271, 478, 472, 283, 200, 278, 289, 446, 214, 285 };

        private readonly int[] baseForms, formVal;
        int entry = -1;
        #endregion
        private void Setup()
        {
            ushort[] TMs = new ushort[0];
            ushort[] HMs = new ushort[0];
            TMHMEditor6.getTMHMList(Main.Config.ORAS, ref TMs, ref HMs);
            CLB_TMHM.Items.Clear();
            int hmcount = Main.Config.ORAS ? 7 : 5;

            if (TMs.Length == 0) // No ExeFS to grab TMs from.
            {
                for (int i = 1; i <= 100; i++)
                    CLB_TMHM.Items.Add($"TM{i:00}");
                for (int i = 1; i <= hmcount; i++)
                    CLB_TMHM.Items.Add($"HM{i:00}");
            }
            else // Use TMHM moves.
            {
                for (int i = 1; i <= 100; i++)
                    CLB_TMHM.Items.Add($"TM{i:00} {moves[TMs[i - 1]]}");
                for (int i = 1; i <= hmcount; i++)
                    CLB_TMHM.Items.Add($"HM{i:00} {moves[HMs[i - 1]]}");
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
                CHK_ORASTutors.Visible = false;
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
                CB_Species.Items.Add($"{species[i]} - {i:000}");

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
            if (!(sender is MaskedTextBox mtb))
                return;
            int.TryParse(mtb.Text, out int val);
            if (Array.IndexOf(byte_boxes, mtb) > -1 && val > 255)
                mtb.Text = "255";
            else if (Array.IndexOf(ev_boxes, mtb) > -1 && val > 3)
                mtb.Text = "3";
        }

        private PersonalInfo pkm;
        private void readInfo()
        {
            pkm = Main.SpeciesStat[entry];

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
            TB_BST.Text = pkm.BST.ToString("000");

            TB_Height.Text = ((decimal)pkm.Height / 100).ToString("00.00");
            TB_Weight.Text = ((decimal)pkm.Weight / 10).ToString("000.0");

            for (int i = 0; i < CLB_TMHM.Items.Count; i++)
                CLB_TMHM.SetItemChecked(i, pkm.TMHM[i]); // Bitflags for TMHM

            for (int i = 0; i < CLB_MoveTutors.Items.Count; i++)
                CLB_MoveTutors.SetItemChecked(i, pkm.TypeTutors[i]); // Bitflags for Tutors

            if (pkm.SpecialTutors.Length > 0)
            {
                int[] len = { tutor1.Length, tutor2.Length, tutor3.Length, tutor4.Length };
                int ctr = 0;
                for (int i = 0; i < len.Length; i++)
                    for (int b = 0; b < len[i]; b++)
                        CLB_OrasTutors.SetItemChecked(ctr++, pkm.SpecialTutors[i][b]);
            }
        }
        private void readEntry()
        {
            readInfo();
            
            if (dumping) return;
            int s = baseForms[entry];
            int f = formVal[entry];
            if (entry <= Main.Config.MaxSpeciesID)
                s = entry;
            Bitmap rawImg = WinFormsUtil.getSprite(s, f, 0, 0, Main.Config);
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

            pkm.Types = new[] { CB_Type1.SelectedIndex, CB_Type2.SelectedIndex };
            pkm.Items = new[] { CB_HeldItem1.SelectedIndex, CB_HeldItem2.SelectedIndex, CB_HeldItem3.SelectedIndex };

            pkm.Gender = Convert.ToByte(TB_Gender.Text);
            pkm.HatchCycles = Convert.ToByte(TB_HatchCycles.Text);
            pkm.BaseFriendship = Convert.ToByte(TB_Friendship.Text);
            pkm.EXPGrowth = (byte) CB_EXPGroup.SelectedIndex;
            pkm.EggGroups = new[] { CB_EggGroup1.SelectedIndex, CB_EggGroup2.SelectedIndex };
            pkm.Abilities = new[] { CB_Ability1.SelectedIndex, CB_Ability2.SelectedIndex, CB_Ability3.SelectedIndex };

            pkm.FormeSprite = Convert.ToUInt16(TB_FormeSprite.Text);
            pkm.FormeCount = Convert.ToByte(TB_FormeCount.Text);
            pkm.Color = (byte) (Convert.ToByte(CB_Color.SelectedIndex) | (Convert.ToByte(TB_RawColor.Text) & 0xF0));
            pkm.BaseEXP = Convert.ToUInt16(TB_BaseExp.Text);

            decimal h; decimal.TryParse(TB_Height.Text, out h);
            decimal w; decimal.TryParse(TB_Weight.Text, out w);
            pkm.Height = (int)(h * 100);
            pkm.Weight = (int)(w * 10);
            
            for (int i = 0; i < CLB_TMHM.Items.Count; i++)
                pkm.TMHM[i] = CLB_TMHM.GetItemChecked(i);

            for (int t = 0; t < CLB_MoveTutors.Items.Count; t++)
                pkm.TypeTutors[t] = CLB_MoveTutors.GetItemChecked(t);

            if (!Main.Config.ORAS) return;

            int[] len = {tutor1.Length, tutor2.Length, tutor3.Length, tutor4.Length};
            int ctr = 0;
            for (int i = 0; i < 4; i++)
                for (int t = 0; t < len[i]; t++)
                    pkm.SpecialTutors[i][t] = CLB_OrasTutors.GetItemChecked(ctr++);
        }
        private void saveEntry()
        {
            savePersonal();
            byte[] edits = pkm.Write();
            files[entry] = edits;
        }

        private void B_Randomize_Click(object sender, EventArgs e)
        {
            saveEntry();

            // input settings
            var rnd = new PersonalRandomizer(Main.SpeciesStat, Main.Config)
            {
                TypeCount = CB_Type1.Items.Count,
                ModifyCatchRate = CHK_CatchRate.Checked,
                ModifyEggGroup = CHK_EggGroup.Checked,
                ModifyStats = CHK_Stats.Checked,
                StatsToRandomize = rstat_boxes.Select(g => g.Checked).ToArray(),
                ModifyAbilities = CHK_Ability.Checked,
                ModifyLearnsetTM = CHK_TM.Checked,
                ModifyLearnsetHM = CHK_HM.Checked,
                ModifyLearnsetTypeTutors = CHK_Tutors.Checked,
                ModifyLearnsetMoveTutors = Main.Config.ORAS && CHK_ORASTutors.Checked,
                ModifyTypes = CHK_Type.Checked,
                ModifyHeldItems = CHK_Item.Checked,
                SameTypeChance = NUD_TypePercent.Value,
                SameEggGroupChance = NUD_Egg.Value,
                StatDeviation = NUD_StatDev.Value,
                AllowWonderGuard = CHK_WGuard.Checked
            };
            rnd.Execute();
            Main.SpeciesStat.Select(z => z.Write()).ToArray().CopyTo(files, 0);

            readEntry();
            WinFormsUtil.Alert("All relevant Pokémon Personal Entries have been randomized!");
        }
        private void B_ModifyAll(object sender, EventArgs e)
        {
            for (int i = 1; i < CB_Species.Items.Count; i++)
            {
                CB_Species.SelectedIndex = i; // Get new Species

                if (CHK_NoEV.Checked)
                    for (int z = 0; z < 6; z++)
                        ev_boxes[z].Text = 0.ToString();
                if (CHK_Growth.Checked)
                    CB_EXPGroup.SelectedIndex = 5;
                if (CHK_EXP.Checked)
                    TB_BaseExp.Text = ((float)NUD_EXP.Value*(Convert.ToUInt16(TB_BaseExp.Text)/100f)).ToString("000");

                if (CHK_QuickHatch.Checked)
                    TB_HatchCycles.Text = 1.ToString();
                if(CHK_CatchRateMod.Checked)
                    TB_CatchRate.Text = ((int)NUD_CatchRateMod.Value).ToString();
            }
            CB_Species.SelectedIndex = 1;
            WinFormsUtil.Alert("All species modified according to specification!");
        }
        private bool dumping;
        private void B_Dump_Click(object sender, EventArgs e)
        {
            if (DialogResult.Yes != WinFormsUtil.Prompt(MessageBoxButtons.YesNo, "Dump all Personal Entries to Text File?"))
                return;
            SaveFileDialog sfd = new SaveFileDialog { FileName = "Personal Entries.txt", Filter = "Text File|*.txt" };
            SystemSounds.Asterisk.Play();
            if (sfd.ShowDialog() != DialogResult.OK)
                return;

            dumping = true;
            List<string> lines = new List<string>();
            for (int i = 0; i < CB_Species.Items.Count; i++)
            {
                CB_Species.SelectedIndex = i; // Get new Species
                lines.Add("======");
                lines.Add($"{entry} - {CB_Species.Text} (Stage: {TB_Stage.Text})");
                lines.Add("======");
                lines.Add($"Base Stats: {TB_BaseHP.Text}.{TB_BaseATK.Text}.{TB_BaseDEF.Text}.{TB_BaseSPA.Text}.{TB_BaseSPD.Text}.{TB_BaseSPE.Text} (BST: {pkm.BST})");
                lines.Add($"EV Yield: {TB_HPEVs.Text}.{TB_ATKEVs.Text}.{TB_DEFEVs.Text}.{TB_SPAEVs.Text}.{TB_SPDEVs.Text}.{TB_SPEEVs.Text}");
                lines.Add($"Abilities: {CB_Ability1.Text} (1) | {CB_Ability2.Text} (2) | {CB_Ability3.Text} (H)");
                lines.Add(string.Format(CB_Type1.SelectedIndex != CB_Type2.SelectedIndex 
                    ? "Type: {0} / {1}" 
                    : "Type: {0}", CB_Type1.Text, CB_Type2.Text));

                lines.Add($"Item 1 (50%): {CB_HeldItem1.Text}");
                lines.Add($"Item 2 (5%): {CB_HeldItem2.Text}");
                lines.Add($"Item 3 (1%): {CB_HeldItem3.Text}");

                lines.Add($"EXP Group: {CB_EXPGroup.Text}");
                lines.Add(string.Format(CB_EggGroup1.SelectedIndex != CB_EggGroup2.SelectedIndex
                        ? "Egg Group: {0} / {1}"
                        : "Egg Group: {0}", CB_EggGroup1.Text, CB_EggGroup2.Text));
                lines.Add($"Hatch Cycles: {TB_HatchCycles.Text}");
                lines.Add($"Height: {TB_Height.Text} m, Weight: {TB_Weight.Text} kg, Color: {CB_Color.Text}");
                lines.Add("");
            }
            string path = sfd.FileName;
            File.WriteAllLines(path, lines, Encoding.Unicode);
        }
        private void CHK_Stats_CheckedChanged(object sender, EventArgs e)
        {
            L_StatDev.Visible = NUD_StatDev.Visible = CHK_Stats.Checked;
            CHK_rHP.Enabled = CHK_rATK.Enabled = CHK_rDEF.Enabled = CHK_rSPA.Enabled = CHK_rSPD.Enabled = CHK_rSPE.Enabled = CHK_Stats.Checked;
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
    }
}