using System;
using System.Drawing;
using System.IO;
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

            data = File.ReadAllBytes(paths[paths.Length - 1]); // Load last to data.
            L_Mode.Text = "Mode: " + mode;
            Setup(); //Turn string resources into arrays
            CB_Species.SelectedIndex = 1;
        }
        #region Global Variables
        private string[] paths = Directory.GetFiles("personal", "*.*", SearchOption.TopDirectoryOnly);
        private string mode = (Main.oras) ? "ORAS" : "XY";

        private string[] items = { };
        private string[] moves = { };
        private string[] species = { };
        private string[] abilities = { };
        private string[] forms = { };

        private byte[] data = { };

        private ComboBox[] helditem_boxes;
        private ComboBox[] ability_boxes;
        private ComboBox[] typing_boxes;
        private ComboBox[] eggGroup_boxes;

        private MaskedTextBox[] byte_boxes;
        private MaskedTextBox[] ev_boxes;

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
        int entrysize = (Main.oras) ? 0x50 : 0x40;
        int entry = -1;
        #endregion
        private void Setup()
        {
            abilities = Main.getText((Main.oras) ? 37 : 34);
            moves = Main.getText((Main.oras) ? 14 : 13);
            items = Main.getText((Main.oras) ? 114 : 96);
            species = Main.getText((Main.oras) ? 98 : 80);
            types = Main.getText((Main.oras) ? 18 : 17);
            forms = Main.getText((Main.oras) ? 5 : 5);
            species[0] = "---";
            abilities[0] = items[0] = moves[0] = "";
            AltForms = getFormList(data, Main.oras, species, forms, types, items);
            species = getPersonalEntryList(data, Main.oras, AltForms, species);
            for (int i = 1; i <= 100; i++)
                CLB_TMHM.Items.Add("TM" + i.ToString("00"));
            for (int i = 1; i <= 7; i++)
                CLB_TMHM.Items.Add("HM" + i.ToString("00"));
            for (int i = 0; i < tutormoves.Length - 1; i++)
                CLB_MoveTutors.Items.Add(moves[tutormoves[i]]);

            if (mode == "XY")
            {
                string[] temp_abilities = new string[abilities.Length - 3]; // 3 abilities added in ORAS
                Array.Copy(abilities, temp_abilities, temp_abilities.Length);
                abilities = temp_abilities;

                string[] temp_items = new string[718]; // 719 items in XY
                Array.Copy(items, temp_items, temp_items.Length);
                items = temp_items;

                string[] temp_moves = new string[moves.Length - 4]; // 4 new moves added in ORAS
                Array.Copy(moves, temp_moves, temp_moves.Length);
                moves = temp_moves;

                string[] temp_species = new string[799]; // 799 species in XY
                Array.Copy(species, temp_species, temp_species.Length);
                species = temp_species;
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

                CLB_OrasTutors.Visible = true;
                CLB_OrasTutors.Enabled = true;
                Lbl_OrasTutors.Visible = true;
            }
            for (int i = 0; i < species.Length; i++)
                CB_Species.Items.Add(String.Format("{0} - {1}", species[i], i.ToString("000")));

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
            int val = 0;
            Int32.TryParse(mtb.Text, out val);
            if (Array.IndexOf(byte_boxes, mtb) > -1 && val > 255)
                mtb.Text = "255";
            else if (Array.IndexOf(ev_boxes, mtb) > -1 && val > 3)
                mtb.Text = "3";
        }
        int bst;

        private void readEntry()
        {
            int len = 0;
            if (mode == "ORAS")
                len = 0x50;
            else if (mode == "XY")
                len = 0x40;

            byte[] file = new byte[len];
            Array.Copy(data, len * entry, file, 0, len);
            TB_BaseHP.Text = file[0].ToString("000");
            TB_BaseATK.Text = file[1].ToString("000");
            TB_BaseDEF.Text = file[2].ToString("000");
            TB_BaseSPE.Text = file[3].ToString("000");
            TB_BaseSPA.Text = file[4].ToString("000");
            TB_BaseSPD.Text = file[5].ToString("000");
            bst = file[0] + file[1] + file[2] + file[3] + file[4] + file[5];

            int EVs = BitConverter.ToInt16(file, 0xA);
            TB_HPEVs.Text = ((EVs >> 0) & 0x3).ToString("0");
            TB_ATKEVs.Text = ((EVs >> 2) & 0x3).ToString("0");
            TB_DEFEVs.Text = ((EVs >> 4) & 0x3).ToString("0");
            TB_SPEEVs.Text = ((EVs >> 6) & 0x3).ToString("0");
            TB_SPAEVs.Text = ((EVs >> 8) & 0x3).ToString("0");
            TB_SPDEVs.Text = ((EVs >> 10) & 0x3).ToString("0");

            CB_Type1.SelectedIndex = file[6];
            CB_Type2.SelectedIndex = file[7];

            TB_CatchRate.Text = file[8].ToString("000");
            TB_Stage.Text = file[9].ToString("0");

            CB_HeldItem1.SelectedIndex = BitConverter.ToUInt16(file, 0xC);
            CB_HeldItem2.SelectedIndex = BitConverter.ToUInt16(file, 0xE);
            CB_HeldItem3.SelectedIndex = BitConverter.ToUInt16(file, 0x10);

            TB_Gender.Text = file[0x12].ToString("000");
            TB_HatchCycles.Text = file[0x13].ToString("000");
            TB_Friendship.Text = file[0x14].ToString("000");

            CB_EXPGroup.SelectedIndex = file[0x15];

            CB_EggGroup1.SelectedIndex = file[0x16];
            CB_EggGroup2.SelectedIndex = file[0x17];

            CB_Ability1.SelectedIndex = file[0x18];
            CB_Ability2.SelectedIndex = file[0x19];
            CB_Ability3.SelectedIndex = file[0x1A];

            TB_FormeCount.Text = file[0x20].ToString("000");
            TB_FormeSprite.Text = BitConverter.ToUInt16(file, 0x1E).ToString("000");

            int color = file[0x21] & 0xF;
            TB_RawColor.Text = file[0x21].ToString("000");
            CB_Color.SelectedIndex = color;

            TB_BaseExp.Text = BitConverter.ToUInt16(file, 0x22).ToString("000");

            TB_Height.Text = ((float)BitConverter.ToUInt16(file, 0x24) / 100).ToString("00.0");
            TB_Weight.Text = ((float)BitConverter.ToUInt16(file, 0x26) / 10).ToString("000.0");

            for (int i = 0; i < 16; i++)
                for (int j = 0; j < 8; j++)
                    if (i * 8 + j < CLB_TMHM.Items.Count)
                        CLB_TMHM.SetItemChecked(i * 8 + j, ((file[0x28 + i] >> j) & 0x1) == 1); //Bitflags for TMHM

            uint tutors = BitConverter.ToUInt32(file, 0x38);
            for (int t = 0; t < 8; t++)
                if (t < CLB_MoveTutors.Items.Count)
                    CLB_MoveTutors.SetItemChecked(t, ((tutors >> t) & 1) == 1);

            if (mode == "ORAS")
            {
                uint[] tutorm = new uint[4];
                for (int j = 0x40; j < 0x50; j += 4)
                    tutorm[(j - 0x40) / 4] = BitConverter.ToUInt32(file, j);

                int ofs = 0;
                for (int j = 0; j < tutorm.Length; j++)
                {
                    for (int i = 0; i < 32; i++)
                        if (ofs + i < CLB_OrasTutors.Items.Count)
                            CLB_OrasTutors.SetItemChecked(ofs + i, ((tutorm[j] >> i) & 1) == 1);
                    // hardcode this, bad, I know
                    if (j == 0)
                        ofs += tutor1.Length;
                    else if (j == 1)
                        ofs += tutor2.Length;
                    else if (j == 2)
                        ofs += tutor3.Length;
                    else
                        ofs += tutor4.Length;
                }
            }
            if (!dumping)
            {
                int[] specForm = getSpecies(data, Main.oras, CB_Species.SelectedIndex);
                string filename = "_" + specForm[0] + ((CB_Species.SelectedIndex > 721) ? "_" + (specForm[1] + 1) : "");
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
        }
        private void saveEntry()
        {
            int len = 0;
            if (mode == "XY")
                len = 0x40;
            else if (mode == "ORAS")
                len = 0x50;

            byte[] edits = new byte[len];
            Array.Copy(data, len * entry, edits, 0, len); //edit raw data for easiness's sake
            edits[0] = Convert.ToByte(TB_BaseHP.Text);
            edits[1] = Convert.ToByte(TB_BaseATK.Text);
            edits[2] = Convert.ToByte(TB_BaseDEF.Text);
            edits[3] = Convert.ToByte(TB_BaseSPE.Text);
            edits[4] = Convert.ToByte(TB_BaseSPA.Text);
            edits[5] = Convert.ToByte(TB_BaseSPD.Text);

            ushort evs = 0;
            evs |= (ushort)((Convert.ToByte(TB_HPEVs.Text) & 3) << 0);
            evs |= (ushort)((Convert.ToByte(TB_ATKEVs.Text) & 3) << 2);
            evs |= (ushort)((Convert.ToByte(TB_DEFEVs.Text) & 3) << 4);
            evs |= (ushort)((Convert.ToByte(TB_SPEEVs.Text) & 3) << 6);
            evs |= (ushort)((Convert.ToByte(TB_SPAEVs.Text) & 3) << 8);
            evs |= (ushort)((Convert.ToByte(TB_SPDEVs.Text) & 3) << 10);
            Array.Copy(BitConverter.GetBytes(evs), 0, edits, 0xa, 2);

            edits[6] = (byte)CB_Type1.SelectedIndex;
            edits[7] = (byte)CB_Type2.SelectedIndex;

            edits[8] = Convert.ToByte(TB_CatchRate.Text);
            edits[9] = Convert.ToByte(TB_Stage.Text);

            Array.Copy(BitConverter.GetBytes((ushort)CB_HeldItem1.SelectedIndex), 0, edits, 0xC, 2);
            Array.Copy(BitConverter.GetBytes((ushort)CB_HeldItem2.SelectedIndex), 0, edits, 0xE, 2);
            Array.Copy(BitConverter.GetBytes((ushort)CB_HeldItem3.SelectedIndex), 0, edits, 0x10, 2);

            edits[0x12] = Convert.ToByte(TB_Gender.Text);
            edits[0x13] = Convert.ToByte(TB_HatchCycles.Text);
            edits[0x14] = Convert.ToByte(TB_Friendship.Text);

            edits[0x15] = (byte)CB_EXPGroup.SelectedIndex;

            edits[0x16] = (byte)CB_EggGroup1.SelectedIndex;
            edits[0x17] = (byte)CB_EggGroup2.SelectedIndex;

            edits[0x18] = (byte)CB_Ability1.SelectedIndex;
            edits[0x19] = (byte)CB_Ability2.SelectedIndex;
            edits[0x1A] = (byte)CB_Ability3.SelectedIndex;

            //edits[0x20] = Convert.ToByte(TB_FormeCount.Text);
            //Array.Copy(BitConverter.GetBytes(Convert.ToUInt16(TB_FormeSprite.Text)),0, edits, 0x1E,2);
            byte color = Convert.ToByte(CB_Color.SelectedIndex);
            color |= (byte)(Convert.ToByte(TB_RawColor.Text) & 0xF0);
            edits[0x21] = color;

            Array.Copy(BitConverter.GetBytes(Convert.ToUInt16(TB_BaseExp.Text)), 0, edits, 0x22, 2);

            float height = (float)Convert.ToDouble(TB_Height.Text) * 100;
            float weight = (float)Convert.ToDouble(TB_Weight.Text) * 10;
            Array.Copy(BitConverter.GetBytes(Convert.ToUInt16(height)), 0, edits, 0x24, 2);
            Array.Copy(BitConverter.GetBytes(Convert.ToUInt16(weight)), 0, edits, 0x26, 2);

            //TMHM
            for (int i = 0; i < 16; i++)
                for (int j = 0; j < 8; j++)
                    if (i * 8 + j < CLB_TMHM.Items.Count)
                        if (CLB_TMHM.GetItemChecked(i * 8 + j))
                            edits[0x28 + i] |= (byte)(1 << j);

            uint tutors = 0;
            for (int t = 0; t < 8; t++)
                if (t < CLB_MoveTutors.Items.Count && CLB_MoveTutors.GetItemChecked(t))
                    tutors |= (uint)(1 << t);

            Array.Copy(BitConverter.GetBytes(tutors), 0, edits, 0x38, 4);

            if (mode == "ORAS")
            {
                uint[] tutorm = new uint[4];
                int ofs = 0;
                for (int j = 0; j < tutor1.Length; j++)
                    if (CLB_OrasTutors.GetItemChecked(ofs++))
                        tutorm[0] |= (uint)(1 << j);

                for (int j = 0; j < tutor2.Length; j++)
                    if (CLB_OrasTutors.GetItemChecked(ofs++))
                        tutorm[1] |= (uint)(1 << j);

                for (int j = 0; j < tutor3.Length; j++)
                    if (CLB_OrasTutors.GetItemChecked(ofs++))
                        tutorm[2] |= (uint)(1 << j);

                for (int j = 0; j < tutor4.Length; j++)
                    if (CLB_OrasTutors.GetItemChecked(ofs++))
                        tutorm[3] |= (uint)(1 << j);

                for (int j = 0x40; j < 0x50; j += 4)
                    Array.Copy(BitConverter.GetBytes(tutorm[(j - 0x40) / 4]), 0, edits, j, 4);
            }

            File.WriteAllBytes(paths[entry], edits);
            Array.Copy(edits, 0, data, entry * len, len);
            File.WriteAllBytes(paths[paths.Length - 1], data);
        }


        private void B_Difficulty_Click(object sender, EventArgs e)
        {
            for (int i = 1; i < CB_Species.Items.Count; i++)
            {
                CB_Species.SelectedIndex = i; // Get new Species
                TB_BaseExp.Text = (Convert.ToUInt16(TB_BaseExp.Text) / 2).ToString("000");
                for (int z = 0; z < 6; z++)
                    ev_boxes[z].Text = 0.ToString();

                CB_EXPGroup.SelectedIndex = 5;
            }
            saveEntry();
            Util.Alert("EXP Yield reduced by 50%, Level Growth Type set to Slow, and EV yields set to 0. Good luck!");
        }
        private void B_Randomize_Click(object sender, EventArgs e)
        {
            Random rnd = new Random();
            const int TMPercent = 35; // Average Learnable TMs is 35.260.
            ushort[] itemlist = (Main.oras) ? Legal.Pouch_Items_ORAS : Legal.Pouch_Items_XY;
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
                for (int t = 0; t < CLB_TMHM.Items.Count; t++)
                    CLB_TMHM.SetItemChecked(t, rnd.Next(0, 100) < TMPercent);


                // Abilities:
                int[] abils = new int[3];
                for (int a = 0; a < 3; a++) // Get 3 New Abilities, none being Wonder Guard (25)
                { int newabil = rnd.Next(1, abillen); while (newabil == 25) newabil = rnd.Next(1, abillen); abils[a] = newabil; }
                CB_Ability1.SelectedIndex = rnd.Next(1, abillen);
                CB_Ability2.SelectedIndex = rnd.Next(1, abillen);
                CB_Ability3.SelectedIndex = rnd.Next(1, abillen);


                // Fiddle with Base Stats, don't muck with Shedinja.
                if (Convert.ToByte(byte_boxes[0].Text) == 1)
                    CB_Ability1.SelectedIndex = CB_Ability2.SelectedIndex = CB_Ability3.SelectedIndex = 25;
                else
                    for (int z = 0; z < 6; z++)
                        byte_boxes[z].Text =
                            Math.Max(
                                5,
                                rnd.Next
                                (
                                    Convert.ToByte(byte_boxes[z].Text) * 3 / 4,
                                    Convert.ToByte(byte_boxes[z].Text) * 5 / 4)
                                )
                            .ToString("000");

                // EV yield stays the same...

                // Items
                CB_HeldItem1.SelectedIndex = (CB_HeldItem1.SelectedIndex > 0) ? itemlist[rnd.Next(1, itemlen)] : 0;
                CB_HeldItem2.SelectedIndex = (CB_HeldItem2.SelectedIndex > 0) ? itemlist[rnd.Next(1, itemlen)] : 0;
                CB_HeldItem3.SelectedIndex = (CB_HeldItem3.SelectedIndex > 0) ? itemlist[rnd.Next(1, itemlen)] : 0;

                // Type
                if (rnd.Next(0, 100) < 50) // 50% chance to have either Single or Dual Typing
                    CB_Type1.SelectedIndex = CB_Type2.SelectedIndex = rnd.Next(0, typelen);
                else
                {
                    CB_Type1.SelectedIndex = rnd.Next(0, typelen);
                    CB_Type2.SelectedIndex = rnd.Next(0, typelen);
                }
            }
            saveEntry();
            Util.Alert("All relevant Pokemon Personal Entries have been randomized!");
        }
        bool dumping;
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

                result += String.Format("Base Stats: {0}.{1}.{2}.{3}.{4}.{5} (BST: {6})", TB_BaseHP.Text, TB_BaseATK.Text, TB_BaseDEF.Text, TB_BaseSPA.Text, TB_BaseSPD.Text, TB_BaseSPE.Text, bst) + Environment.NewLine;
                result += String.Format("EV Yield: {0}.{1}.{2}.{3}.{4}.{5}", TB_HPEVs.Text, TB_ATKEVs.Text, TB_DEFEVs.Text, TB_SPAEVs.Text, TB_SPDEVs.Text, TB_SPEEVs.Text) + Environment.NewLine;
                result += String.Format("Abilities: {0} (1) | {1} (2) | {2} (H)", CB_Ability1.Text, CB_Ability2.Text, CB_Ability3.Text) + Environment.NewLine;

                result += String.Format(((CB_Type1.SelectedIndex != CB_Type2.SelectedIndex) ? "Type: {0} / {1}" : "Type: {0}"), CB_Type1.Text, CB_Type2.Text);

                result += String.Format("Item 1 (50%): {0}", CB_HeldItem1.Text) + Environment.NewLine;
                result += String.Format("Item 2 (5%): {0}", CB_HeldItem2.Text) + Environment.NewLine;
                result += String.Format("Item 3 (1%): {0}", CB_HeldItem3.Text) + Environment.NewLine;
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

        private void formClosing(object sender, FormClosingEventArgs e)
        {
            if (entry > -1) saveEntry();
        }

        // Utility (Shared)
        internal static int[] getSpecies(byte[] data, bool oras, int PersonalEntry)
        {
            int entrysize = (oras) ? 0x50 : 0x40;
            if (PersonalEntry < 722) return new[] { PersonalEntry, 0 };

            for (int i = 0; i < 722; i++)
            {
                int FormCount = data[i * entrysize + 0x20] - 1; // Mons with no alt forms have a FormCount of 1.
                ushort altformpointer = BitConverter.ToUInt16(data, entrysize * i + 0x1C);
                if (altformpointer > 0)
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
                int entrysize = (oras) ? 0x50 : 0x40;
                int AltFormOfs = 723; //null + 721 species + 1 gap
                for (int i = 0; i < 722; i++) //Hardcode 721 species + null
                {
                    int FormCount = data[i * entrysize + 0x20]; // Mons with no alt forms have a FormCount of 1.
                    FormList[i] = new string[FormCount];
                    if (FormCount > 0)
                    {
                        FormList[i][0] = (forms[i] == "") ? species[i] : forms[i];
                        for (int j = 1; j < FormCount; j++)
                            FormList[i][j] = forms[AltFormOfs++];
                    }
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
            catch (Exception e) { Util.Error("Error while creating the Alternate Formes Listing.", "Please make sure that the Personal.dat & Forms text are as intended", "Error:" + Environment.NewLine + e.ToString()); return null; }
        }
        internal static string[] getPersonalEntryList(byte[] data, bool oras, string[][] AltForms, string[] species)
        {
            int entrysize = (oras) ? 0x50 : 0x40;
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
            int entrysize = (oras) ? 0x50 : 0x40;
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
    }
}