using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Media;
using System.Text;
using System.Windows.Forms;

namespace pk3DS
{
    public partial class SMTE : Form
    {
        public SMTE()
        {
            InitializeComponent();
            Console.WriteLine("Starting SMTE...");
            #region Combo Box Arrays
            trpk_pkm = new ComboBox[6];
            trpk_lvl = new ComboBox[6];
            trpk_item = new ComboBox[6];
            trpk_abil = new ComboBox[6];
            trpk_m1 = new ComboBox[6];
            trpk_m2 = new ComboBox[6];
            trpk_m3 = new ComboBox[6];
            trpk_m4 = new ComboBox[6];
            trpk_form = new ComboBox[6];
            trpk_gender = new ComboBox[6];
            trpk_nature = new ComboBox[6];

            trpk_IVs = new MaskedTextBox[6][];
            trpk_EVs = new MaskedTextBox[6][];

            trpk_shiny = new CheckBox[6];

            trpk_m = new[]{ trpk_m1, trpk_m2, trpk_m3, trpk_m4 };
            #endregion
            #region Tab Initialization
            for (int i = 1; i <= 6; i++)
            {
                trpk_IVs[i - 1] = new MaskedTextBox[6];
                trpk_EVs[i - 1] = new MaskedTextBox[6];

                var tp = new TabPage();
                tp.Text = $"Pokemon {i}";
                TC_Main.TabPages.Add(tp);
                TC_Main.SelectTab(tp);
                var L_Shiny = new Label();
                L_Shiny.Text = "Shiny:";
                L_Shiny.Location = new Point(385, 36);
                L_Shiny.Size = new Size(36, 13);
                tp.Controls.Add(L_Shiny);
                var CHK_Shiny = new CheckBox();
                CHK_Shiny.Text = "";
                CHK_Shiny.Location = new Point(430, 36);
                CHK_Shiny.Size = new Size(15, 14);
                tp.Controls.Add(CHK_Shiny);
                var L_Nature = new Label();
                L_Nature.Text = "Nature:";
                L_Nature.Location = new Point(379, 9);
                L_Nature.Size = new Size(42, 13);
                tp.Controls.Add(L_Nature);
                var CB_Nature = new ComboBox();
                CB_Nature.Text = "";
                CB_Nature.Location = new Point(430, 6);
                CB_Nature.Size = new Size(121, 21);
                tp.Controls.Add(CB_Nature);
                var L_Ability = new Label();
                L_Ability.Text = "Ability:";
                L_Ability.Location = new Point(29, 90);
                L_Ability.Size = new Size(37, 13);
                tp.Controls.Add(L_Ability);
                var CB_Ability = new ComboBox();
                CB_Ability.Text = "";
                CB_Ability.Location = new Point(72, 87);
                CB_Ability.Size = new Size(121, 21);
                tp.Controls.Add(CB_Ability);
                var L_Gender = new Label();
                L_Gender.Text = "Gender:";
                L_Gender.Location = new Point(21, 117);
                L_Gender.Size = new Size(45, 13);
                tp.Controls.Add(L_Gender);
                var CB_Gender = new ComboBox();
                CB_Gender.Text = "";
                CB_Gender.Location = new Point(72, 114);
                CB_Gender.Size = new Size(121, 21);
                tp.Controls.Add(CB_Gender);
                var L_Forme = new Label();
                L_Forme.Text = "Form:";
                L_Forme.Location = new Point(33, 36);
                L_Forme.Size = new Size(33, 13);
                tp.Controls.Add(L_Forme);
                var CB_Forme = new ComboBox();
                CB_Forme.Text = "";
                CB_Forme.Location = new Point(72, 33);
                CB_Forme.Size = new Size(121, 21);
                tp.Controls.Add(CB_Forme);
                var L_Item = new Label();
                L_Item.Text = "Item:";
                L_Item.Location = new Point(217, 9);
                L_Item.Size = new Size(30, 13);
                tp.Controls.Add(L_Item);
                var CB_Item = new ComboBox();
                CB_Item.Text = "";
                CB_Item.Location = new Point(253, 6);
                CB_Item.Size = new Size(121, 21);
                tp.Controls.Add(CB_Item);
                var L_Level = new Label();
                L_Level.Text = "Level:";
                L_Level.Location = new Point(30, 63);
                L_Level.Size = new Size(36, 13);
                tp.Controls.Add(L_Level);
                var CB_Level = new ComboBox();
                CB_Level.Text = "";
                CB_Level.Location = new Point(72, 60);
                CB_Level.Size = new Size(121, 21);
                tp.Controls.Add(CB_Level);
                var L_Pokemon = new Label();
                L_Pokemon.Text = "Pokemon:";
                L_Pokemon.Location = new Point(11, 9);
                L_Pokemon.Size = new Size(55, 13);
                tp.Controls.Add(L_Pokemon);
                var CB_Pokemon = new ComboBox();
                CB_Pokemon.Text = "";
                CB_Pokemon.Location = new Point(72, 6);
                CB_Pokemon.Size = new Size(121, 21);
                tp.Controls.Add(CB_Pokemon);

                // Moves
                for (var j = 1; j <= 4; j++)
                {
                    var L_Move = new Label();
                    L_Move.Text = $"Move {j}:";
                    L_Move.Location = new Point(201, 9 + 27*j);
                    L_Move.Size = new Size(46, 13);
                    tp.Controls.Add(L_Move);
                    var CB_Move = new ComboBox();
                    CB_Move.Location = new Point(253, 6 + 27*j);
                    CB_Move.Size = new Size(121, 21);
                    tp.Controls.Add(CB_Move);
                    trpk_m[j - 1][i - 1] = CB_Move;
                }

                // Stat labels
                var L_HP = new Label();
                L_HP.Text = "HP";
                L_HP.Location = new Point(413, 68);
                L_HP.Size = new Size(22, 13);
                tp.Controls.Add(L_HP);
                var stats = new[] {"ATK", "DEF", "SPA", "SPD", "SPE"};
                for (var j = 0; j < 5; j++)
                {
                    var L_Stat = new Label();
                    L_Stat.Text = stats[j];
                    L_Stat.Location = new Point(432 + 24 * j, 68);
                    L_Stat.Size = new Size(28, 13);
                    if (stats[j] == "SPD")
                        L_Stat.Size = new Size(29, 13);
                    if (stats[j] == "SPE")
                        L_Stat.Location = new Point(433 + 24 * j, 68);
                    tp.Controls.Add(L_Stat);
                }

                var L_EVs = new Label();
                var L_IVs = new Label();
                L_IVs.Text = "IVs:";
                L_EVs.Text = "EVs:";
                L_IVs.Location = new Point(384, 91);
                L_EVs.Location = new Point(379, 118);
                L_IVs.Size = new Size(29, 13);
                L_EVs.Size = new Size(29, 13);
                tp.Controls.Add(L_IVs);
                tp.Controls.Add(L_EVs);

                for (var j = 0; j < 6; j++)
                {
                    var MTB_IV = new MaskedTextBox();
                    MTB_IV.Mask = "00";
                    MTB_IV.Location = new Point(413 + 24*j, 86);
                    MTB_IV.Size = new Size(22, 20);
                    var MTB_EV = new MaskedTextBox();
                    MTB_EV.Mask = "00";
                    MTB_EV.Location = new Point(413 + 24 * j, 113);
                    MTB_EV.Size = new Size(22, 20);
                    MTB_IV.TextAlign = MTB_EV.TextAlign = HorizontalAlignment.Center;
                    tp.Controls.Add(MTB_IV);
                    tp.Controls.Add(MTB_EV);
                    trpk_IVs[i - 1][j] = MTB_IV;
                    trpk_EVs[i - 1][j] = MTB_EV;
                }

                trpk_pkm[i-1] = CB_Pokemon;
                trpk_lvl[i-1] = CB_Level;
                trpk_item[i-1] = CB_Item;
                trpk_abil[i-1] = CB_Ability;
                trpk_form[i - 1] = CB_Forme;
                trpk_gender[i - 1] = CB_Gender;
                trpk_shiny[i - 1] = CHK_Shiny;
                trpk_nature[i - 1] = CB_Nature;

                CB_Pokemon.SelectedIndexChanged += refreshSpeciesAbility;
                CB_Forme.SelectedIndexChanged += refreshFormAbility;
            }
            #endregion

            Trainers = new Trainer7[trdatapaths.Length];
            Setup();
            for (int i = 0; i < TC_Main.TabPages.Count; i++)
                TC_Main.SelectTab(i);
            TC_Main.SelectTab(0);

            changeTrainerIndex(null, null);
        }


        private Trainer7[] Trainers;

        private string[][] AltForms;
        internal static readonly Random rand = new Random();
        internal static uint rnd32()
        {
            return (uint)rand.Next(1 << 30) << 2 | (uint)rand.Next(1 << 2);
        }
        private bool start = true;
        private bool loading = true;
        private int index = -1;
        #region Global Variables
        private readonly ComboBox[] trpk_pkm;

        private readonly ComboBox[] trpk_lvl;

        private readonly ComboBox[] trpk_item;

        private readonly ComboBox[] trpk_abil;

        private readonly ComboBox[] trpk_nature;


        private readonly ComboBox[] trpk_m1;
        private readonly ComboBox[] trpk_m2;
        private readonly ComboBox[] trpk_m3;
        private readonly ComboBox[] trpk_m4;
        private readonly ComboBox[][] trpk_m;

        private readonly MaskedTextBox[][] trpk_IVs;
        private readonly MaskedTextBox[][] trpk_EVs;
        private readonly ComboBox[] trpk_form;
        private readonly ComboBox[] trpk_gender;

        private readonly CheckBox[] trpk_shiny;

        private PictureBox[] pba;

        // Top Level Functions
        private readonly string[] trdatapaths = Directory.GetFiles("trdata");
        private readonly string[] trpokepaths = Directory.GetFiles("trpoke");
        public static readonly string[] abilitylist = Main.getText(TextName.AbilityNames);
        public static readonly string[] movelist = Main.getText(TextName.MoveNames);
        public static readonly string[] itemlist = Main.getText(TextName.ItemNames);
        public static readonly string[] specieslist = Main.getText(TextName.SpeciesNames);
        public static readonly string[] types = Main.getText(TextName.Types);
        public static readonly string[] natures = Main.getText(TextName.Natures);
        public static readonly string[] forms = Enumerable.Range(0, 1000).Select(i => i.ToString("000")).ToArray();
        private string[] trName = Main.getText(TextName.TrainerNames);
        public static readonly string[] trClass = Main.getText(TextName.TrainerClasses);
        public static readonly string[] trText = Main.getText(TextName.TrainerText);
        #endregion

        private void refreshFormAbility(object sender, EventArgs e)
        {
            if (index < 0)
                return;
            int i = Array.IndexOf(trpk_form, sender as ComboBox);
            Trainers[index][i].Form = trpk_form[i].SelectedIndex;
            refreshPKMSlotAbility(i);
        }
        private void refreshSpeciesAbility(object sender, EventArgs e)
        {
            if (index < 0)
                return;
            int i = Array.IndexOf(trpk_pkm, sender as ComboBox);
            Trainers[index][i].Species = (ushort)trpk_pkm[i].SelectedIndex;
            Personal.setForms(trpk_pkm[i].SelectedIndex, trpk_form[i], AltForms);
            refreshPKMSlotAbility(i);
        }
        private void refreshPKMSlotAbility(int slot)
        {
            int previousAbility = trpk_abil[slot].SelectedIndex;

            int species = trpk_pkm[slot].SelectedIndex;
            int formnum = trpk_form[slot].SelectedIndex;
            species = formnum > 0
                ? (indexList[species] > 0
                    ? indexList[species] + formnum - 1
                    : species)
                : species;

            trpk_abil[slot].Items.Clear();
            trpk_abil[slot].Items.Add("Any (1 or 2)");
            trpk_abil[slot].Items.Add(abilitylist[Main.SpeciesStat[species].Abilities[0]] + " (1)");
            trpk_abil[slot].Items.Add(abilitylist[Main.SpeciesStat[species].Abilities[1]] + " (2)");
            trpk_abil[slot].Items.Add(abilitylist[Main.SpeciesStat[species].Abilities[2]] + " (H)");

            trpk_abil[slot].SelectedIndex = previousAbility;

            showTeams(slot);
        }

        private ushort[] indexList;
        private void Setup()
        {
            start = true;
            byte[] personalData = File.ReadAllBytes(Directory.GetFiles("personal").Last());
            indexList = Personal.getPersonalIndexList(personalData, Main.Config.ORAS);
            AltForms = forms.Select(f => Enumerable.Range(0, 100).Select(i => i.ToString()).ToArray()).ToArray();

            Array.Resize(ref trName, trdatapaths.Length);
            CB_TrainerID.Items.Clear();
            for (int i = 0; i < trdatapaths.Length; i++)
                CB_TrainerID.Items.Add(string.Format("{1} - {0}", i.ToString("000"), trName[i] ?? "UNKNOWN"));

            CB_Trainer_Class.Items.Clear();
            for (int i = 0; i < trClass.Length; i++)
                CB_Trainer_Class.Items.Add(string.Format("{1} - {0}", i.ToString("000"), trClass[i]));

            Trainers[0] = new Trainer7();

            for (int i = 1; i < trdatapaths.Length; i++)
            {
                Trainers[i] = new Trainer7(File.ReadAllBytes(trdatapaths[i]), File.ReadAllBytes(trpokepaths[i]));
                Trainers[i].Name = trName[i];
                Trainers[i].trdatapath = trdatapaths[i];
                Trainers[i].trpokepath = trpokepaths[i];
                Trainers[i].ID = i;
            }

            specieslist[0] = "---";
            abilitylist[0] = itemlist[0] = movelist[0] = "";
            pba = new[] { PB_Team1, PB_Team2, PB_Team3, PB_Team4, PB_Team5, PB_Team6 };

            for (int i = 0; i < 6; i++)
            {
                trpk_pkm[i].Items.Clear();
                foreach (string s in specieslist)
                    trpk_pkm[i].Items.Add(s);

                trpk_m1[i].Items.Clear();
                trpk_m2[i].Items.Clear();
                trpk_m3[i].Items.Clear();
                trpk_m4[i].Items.Clear();
                foreach (string s in movelist)
                {
                    trpk_m1[i].Items.Add(s);
                    trpk_m2[i].Items.Add(s);
                    trpk_m3[i].Items.Add(s);
                    trpk_m4[i].Items.Add(s);
                }

                trpk_nature[i].Items.Clear();
                foreach (string s in natures)
                    trpk_nature[i].Items.Add(s);

                trpk_item[i].Items.Clear();
                foreach (string s in itemlist)
                    trpk_item[i].Items.Add(s);

                trpk_lvl[i].Items.Clear();
                for (int z = 0; z <= 100; z++)
                    trpk_lvl[i].Items.Add(z.ToString());

                trpk_gender[i].Items.Clear();
                trpk_gender[i].Items.Add("- / G/Random");
                trpk_gender[i].Items.Add("♂ / M");
                trpk_gender[i].Items.Add("♀ / F");

                trpk_form[i].Items.Add("");

                trpk_pkm[i].SelectedIndex = 0;
            }
            CB_Item_1.Items.Clear();
            CB_Item_2.Items.Clear();
            CB_Item_3.Items.Clear();
            CB_Item_4.Items.Clear();
            CB_Prize.Items.Clear();
            foreach (string s in itemlist)
            {
                CB_Item_1.Items.Add(s);
                CB_Item_2.Items.Add(s);
                CB_Item_3.Items.Add(s);
                CB_Item_4.Items.Add(s);
                CB_Prize.Items.Add(s);
            }

            CB_AI.Items.Clear(); CB_Money.Items.Clear();
            for (int i = 0; i < 256; i++)
            { CB_AI.Items.Add(i.ToString()); CB_Money.Items.Add(i.ToString()); }

            CB_Battle_Type.Items.Clear();
            CB_Battle_Type.Items.Add("Single");
            CB_Battle_Type.Items.Add("Double");
            CB_Battle_Type.Items.Add("Triple");
            CB_Battle_Type.Items.Add("Rotation");
            CB_Battle_Type.Items.Add("Horde");

            CB_TrainerID.SelectedIndex = 1;
            start = false;
            index = 0;

            SystemSounds.Asterisk.Play();
        }

        private void showTeams(int i)
        {
            if (index < 0 || Trainers[index] == null) return;
            if (i >= Trainers[index].NumPokemon) { pba[i].Image = null; return; }
            Bitmap rawImg = Util.getSprite(Trainers[index][i].Species, Trainers[index][i].Form, Trainers[index][i].Gender, Trainers[index][i].Item);
            pba[i].Image = Util.scaleImage(rawImg, 2);
        }

        private void changeTrainerIndex(object sender, EventArgs e)
        {
            populate();
        }

        private void populate(bool write = true)
        {
            if (start) return;
            if (index != 0 && write)
                SaveChanges();
            index = CB_TrainerID.SelectedIndex;
            loading = true;

            if (index == 0) return;

            var tr = Trainers[index];

            // Load Trainer Data
            CB_Trainer_Class.SelectedIndex = tr.TrainerClass;
            CB_numPokemon.SelectedIndex = tr.NumPokemon;

            // Load Pokemon Data
            for (int i = 0; i < tr.NumPokemon; i++)
            {
                for (int j = 0; j < 6; j++)
                {
                    trpk_IVs[i][j].Text = tr[i].IVs[j].ToString();
                    trpk_EVs[i][j].Text = tr[i].EVs[j].ToString();
                }

                trpk_lvl[i].SelectedIndex = tr[i].Level;
                trpk_pkm[i].SelectedIndex = tr[i].Species;
                Personal.setForms(tr[i].Species, trpk_form[i], AltForms);
                trpk_form[i].SelectedIndex = tr[i].Form % trpk_form[i].Items.Count; // stupid X/Y buggy edge cases (220 / 222)
                refreshPKMSlotAbility(i); // Repopulate Abilities

                trpk_abil[i].SelectedIndex = tr[i].Ability;
                trpk_gender[i].SelectedIndex = tr[i].Gender;

                trpk_item[i].SelectedIndex = tr[i].Item;
                trpk_m1[i].SelectedIndex = tr[i].Moves[0];
                trpk_m2[i].SelectedIndex = tr[i].Moves[1];
                trpk_m3[i].SelectedIndex = tr[i].Moves[2];
                trpk_m4[i].SelectedIndex = tr[i].Moves[3];
                trpk_nature[i].SelectedIndex = tr[i].Nature;
            }
            loading = false;

            for (int i = 0; i < 6; i++)
                TC_Main.TabPages[1 + i].Enabled = i < tr.NumPokemon;

            for (int i = 0; i < 6; i++) showTeams(i);
        }

        private void SaveChanges()
        {

            var tr = Trainers[index];
            // Set Trainer Data
            tr.TrainerClass = (byte)CB_Trainer_Class.SelectedIndex;
            tr.NumPokemon = (byte)CB_numPokemon.SelectedIndex;

            for (int i = 0; i < tr.NumPokemon; i++)
            {
                tr[i].IVs = trpk_IVs[i].Select(mtb => Util.ToInt32(mtb)).ToArray();
                tr[i].EVs = trpk_EVs[i].Select(mtb => Util.ToInt32(mtb)).ToArray();
                tr[i].Ability = trpk_abil[i].SelectedIndex;
                tr[i].Gender = trpk_gender[i].SelectedIndex;
                tr[i].Level = (ushort)trpk_lvl[i].SelectedIndex;
                tr[i].Species = (ushort)trpk_pkm[i].SelectedIndex;
                tr[i].Form = (ushort)trpk_form[i].SelectedIndex;
                tr[i].Item = (ushort)trpk_item[i].SelectedIndex;

                tr[i].Nature = trpk_nature[i].SelectedIndex;
                tr[i].Shiny = trpk_shiny[i].Checked;

                tr[i].Move1 = (ushort)trpk_m1[i].SelectedIndex;
                tr[i].Move2 = (ushort)trpk_m2[i].SelectedIndex;
                tr[i].Move3 = (ushort)trpk_m3[i].SelectedIndex;
                tr[i].Move4 = (ushort)trpk_m4[i].SelectedIndex;
            }
            tr.Write();
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            base.OnFormClosing(e);

            SaveChanges();
        }

        private void DumpTxt(object sender, EventArgs e)
        {
            using (var sfd = new SaveFileDialog())
            {
                sfd.FileName = "Trainers.txt";
                if (sfd.ShowDialog() != DialogResult.OK)
                    return;
                var sb = new StringBuilder();
                foreach (var Trainer in Trainers)
                    sb.Append(Trainer.ToString());
                File.WriteAllText(sfd.FileName, sb.ToString());
            }
        }
    }

    internal class Trainer7
    {
        public string Name;
        public string trdatapath;
        public string trpokepath;
        public int ID;

        private byte[] trdata;
        public trpoke7[] Pokemon;

        public Trainer7(byte[] tr = null, byte[] tp = null)
        {
            tr = tr ?? new byte[0x14];
            tp = tp ?? new byte[0x20];
            trdata = (byte[]) tr.Clone();
            Pokemon = new trpoke7[6];
            for (int i = 0; i < Pokemon.Length; i++)
            {
                byte[] poke = new byte[0x20];
                if (i < NumPokemon)
                    Array.Copy(tp, i*0x20, poke, 0, 0x20);
                Pokemon[i] = new trpoke7(poke);
            }
        }


        public byte TrainerClass
        {
            get { return trdata[0]; }
            set { trdata[0] = value; }
        }
        public int NumPokemon
        {
            get { return trdata[3]; }
            set
            {
                var val = (byte)value;
                if (val > 6)
                    val = 6;
                if (val < 1)
                    val = 1;
                trdata[3] = val;
            }
        }

        public trpoke7 this[int i]
        {
            get { return Pokemon[i]; }
            set { Pokemon[i] = value; }
        }

        internal class trpoke7
        {
            private byte[] Data;

            public trpoke7(byte[] d)
            {
                if (d.Length != 0x20)
                    throw new ArgumentException("Invalid trpoke7!");
                Data = (byte[]) d.Clone();
            }

            public int Gender
            {
                get { return Data[0] & 0x3; }
                set { Data[0] = (byte)((Data[0] & 0xFC) | value & 0x3); }
            }
            public int Ability
            {
                get { return (Data[0] >> 4) & 0x3; }
                set { Data[0] = (byte)((Data[0] & 0xCF) | ((value & 0x3) << 4)); }
            }

            public int Nature
            {
                get { return Data[1]; }
                set { Data[1] = (byte) value; }
            }

            public int Level
            {
                get { return Data[0xE]; }
                set { Data[0xE] = (byte) value; }
            }

            public ushort Species
            {
                get { return BitConverter.ToUInt16(Data, 0x10); }
                set { BitConverter.GetBytes(value).CopyTo(Data, 0x10); }
            }

            public int Form
            {
                get { return Data[0x12]; }
                set { Data[0x12] = (byte) value; }
            }

            public ushort Item
            {
                get { return BitConverter.ToUInt16(Data, 0x14); }
                set { BitConverter.GetBytes(value).CopyTo(Data, 0x14); }
            }

            public ushort Move1
            {
                get { return BitConverter.ToUInt16(Data, 0x18); }
                set { BitConverter.GetBytes(value).CopyTo(Data, 0x18); }
            }

            public ushort Move2
            {
                get { return BitConverter.ToUInt16(Data, 0x1A); }
                set { BitConverter.GetBytes(value).CopyTo(Data, 0x1A); }
            }

            public ushort Move3
            {
                get { return BitConverter.ToUInt16(Data, 0x1C); }
                set { BitConverter.GetBytes(value).CopyTo(Data, 0x1C); }
            }

            public ushort Move4
            {
                get { return BitConverter.ToUInt16(Data, 0x1E); }
                set { BitConverter.GetBytes(value).CopyTo(Data, 0x1E); }
            }

            public ushort[] Moves
            {
                get { return new[] { Move1, Move2, Move3, Move4 }; }
                set { if (value?.Length != 4) return; Move1 = value[0]; Move2 = value[1]; Move3 = value[2]; Move4 = value[3]; }
            }

            public int EV_HP { get { return Data[0x2]; } set { Data[0x2] = (byte)value; } }
            public int EV_ATK { get { return Data[0x3]; } set { Data[0x3] = (byte)value; } }
            public int EV_DEF { get { return Data[0x4]; } set { Data[0x4] = (byte)value; } }
            public int EV_SPE { get { return Data[0x7]; } set { Data[0x7] = (byte)value; } }
            public int EV_SPA { get { return Data[0x5]; } set { Data[0x5] = (byte)value; } }
            public int EV_SPD { get { return Data[0x6]; } set { Data[0x6] = (byte)value; } }

            private uint IV32 { get { return BitConverter.ToUInt32(Data, 0x8); } set { BitConverter.GetBytes(value).CopyTo(Data, 0x8); } }

            public int IV_HP { get { return (int)(IV32 >> 00) & 0x1F; } set { IV32 = (uint)((IV32 & ~(0x1F << 00)) | (uint)((value > 31 ? 31 : value) << 00)); } }
            public int IV_ATK { get { return (int)(IV32 >> 05) & 0x1F; } set { IV32 = (uint)((IV32 & ~(0x1F << 05)) | (uint)((value > 31 ? 31 : value) << 05)); } }
            public int IV_DEF { get { return (int)(IV32 >> 10) & 0x1F; } set { IV32 = (uint)((IV32 & ~(0x1F << 10)) | (uint)((value > 31 ? 31 : value) << 10)); } }
            public int IV_SPE { get { return (int)(IV32 >> 15) & 0x1F; } set { IV32 = (uint)((IV32 & ~(0x1F << 15)) | (uint)((value > 31 ? 31 : value) << 15)); } }
            public int IV_SPA { get { return (int)(IV32 >> 20) & 0x1F; } set { IV32 = (uint)((IV32 & ~(0x1F << 20)) | (uint)((value > 31 ? 31 : value) << 20)); } }
            public int IV_SPD { get { return (int)(IV32 >> 25) & 0x1F; } set { IV32 = (uint)((IV32 & ~(0x1F << 25)) | (uint)((value > 31 ? 31 : value) << 25)); } }
            public bool Shiny { get { return ((IV32 >> 30) & 1) == 1; } set { IV32 = (uint)((IV32 & ~0x40000000) | (uint)(value ? 0x40000000 : 0)); } }

            public int[] IVs
            {
                get { return new[] { IV_HP, IV_ATK, IV_DEF, IV_SPA, IV_SPD, IV_SPE }; }
                set
                {
                    if (value?.Length != 6) return;
                    IV_HP = value[0]; IV_ATK = value[1]; IV_DEF = value[2];
                    IV_SPA = value[3]; IV_SPD = value[4]; IV_SPE = value[5];
                }
            }

            public int[] EVs
            {
                get { return new[] { EV_HP, EV_ATK, EV_DEF, EV_SPA, EV_SPD, EV_SPE }; }
                set
                {
                    if (value?.Length != 6) return;
                    EV_HP = value[0]; EV_ATK = value[1]; EV_DEF = value[2];
                    EV_SPA = value[3]; EV_SPD = value[4]; EV_SPE = value[5];
                }
            }



            public byte[] Write() => (byte[]) Data.Clone();
        }

        public void Write()
        {
            File.WriteAllBytes(trdatapath, trdata);
            byte[] dat = new byte[0x20*NumPokemon];
            for (int i = 0; i < NumPokemon; i++)
            {
                Pokemon[i].Write().CopyTo(dat, 0x20*i);
            }
            File.WriteAllBytes(trpokepath, dat);
        }

        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.AppendLine("======");
            sb.AppendLine($"{ID} - {SMTE.trClass[TrainerClass]} {Name}");
            sb.AppendLine("======");
            sb.AppendLine($"Pokemon: {NumPokemon}");
            for (int i = 0; i < NumPokemon; i++)
            {
                if (Pokemon[i].Shiny)
                    sb.Append("Shiny ");
                sb.Append(SMTE.specieslist[Pokemon[i].Species]);
                sb.Append($" (Lv. {Pokemon[i].Level}) ");
                if (Pokemon[i].Item > 0)
                    sb.Append($"@{SMTE.itemlist[Pokemon[i].Item]}");

                if (Pokemon[i].Nature != 0)
                    sb.Append($" (Nature: {SMTE.natures[Pokemon[i].Nature]})");

                sb.Append($" (Moves: {string.Join("/", Pokemon[i].Moves.Select(m => m == 0 ? "(None)" : SMTE.movelist[m]))})");

                sb.Append($" IVs: {string.Join("/", Pokemon[i].IVs)}");

                sb.Append($" EVs: {string.Join("/", Pokemon[i].EVs)}");
                sb.AppendLine();
            }
            return sb.ToString();
        }
    }
}
