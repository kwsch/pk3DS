﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Media;
using System.Text;
using System.Windows.Forms;

namespace pk3DS
{
    public partial class RSTE : Form
    {
        public RSTE()
        {
            Array.Resize(ref specieslist, 722);

            InitializeComponent();
            // String Fetching
            #region Combo Box Arrays
            trpk_pkm = new[] 
            {
                CB_Pokemon_1_Pokemon,
                CB_Pokemon_2_Pokemon,
                CB_Pokemon_3_Pokemon,
                CB_Pokemon_4_Pokemon,
                CB_Pokemon_5_Pokemon,
                CB_Pokemon_6_Pokemon,
            };
            trpk_lvl = new[] 
            { 
                CB_Pokemon_1_Level,
                CB_Pokemon_2_Level,
                CB_Pokemon_3_Level,
                CB_Pokemon_4_Level,
                CB_Pokemon_5_Level,
                CB_Pokemon_6_Level,
            };
            trpk_item = new[] 
            { 
                CB_Pokemon_1_Item,
                CB_Pokemon_2_Item,
                CB_Pokemon_3_Item,
                CB_Pokemon_4_Item,
                CB_Pokemon_5_Item,
                CB_Pokemon_6_Item,
            };
            trpk_abil = new[] 
            { 
                CB_Pokemon_1_Ability,
                CB_Pokemon_2_Ability,
                CB_Pokemon_3_Ability,
                CB_Pokemon_4_Ability,
                CB_Pokemon_5_Ability,
                CB_Pokemon_6_Ability,
            };
            trpk_m1 = new[]
            {
                CB_Pokemon_1_Move_1,
                CB_Pokemon_2_Move_1,
                CB_Pokemon_3_Move_1,
                CB_Pokemon_4_Move_1,
                CB_Pokemon_5_Move_1,
                CB_Pokemon_6_Move_1,
            };
            trpk_m2 = new[]
            {
                CB_Pokemon_1_Move_2,
                CB_Pokemon_2_Move_2,
                CB_Pokemon_3_Move_2,
                CB_Pokemon_4_Move_2,
                CB_Pokemon_5_Move_2,
                CB_Pokemon_6_Move_2,
            };
            trpk_m3 = new[]
            {
                CB_Pokemon_1_Move_3,
                CB_Pokemon_2_Move_3,
                CB_Pokemon_3_Move_3,
                CB_Pokemon_4_Move_3,
                CB_Pokemon_5_Move_3,
                CB_Pokemon_6_Move_3,
            };
            trpk_m4 = new[]
            {
                CB_Pokemon_1_Move_4,
                CB_Pokemon_2_Move_4,
                CB_Pokemon_3_Move_4,
                CB_Pokemon_4_Move_4,
                CB_Pokemon_5_Move_4,
                CB_Pokemon_6_Move_4,
            };
            trpk_IV = new[]
            {
                CB_Pokemon_1_IVs,
                CB_Pokemon_2_IVs,
                CB_Pokemon_3_IVs,
                CB_Pokemon_4_IVs,
                CB_Pokemon_5_IVs,
                CB_Pokemon_6_IVs,
            };
            trpk_form = new[]
            {
                CB_Pokemon_1_Form,
                CB_Pokemon_2_Form,
                CB_Pokemon_3_Form,
                CB_Pokemon_4_Form,
                CB_Pokemon_5_Form,
                CB_Pokemon_6_Form,
            };
            trpk_gender = new[]
            {
                CB_Pokemon_1_Gender,
                CB_Pokemon_2_Gender,
                CB_Pokemon_3_Gender,
                CB_Pokemon_4_Gender,
                CB_Pokemon_5_Gender,
                CB_Pokemon_6_Gender,
            };
            #endregion
            Setup();
        }
        private string[][] AltForms;
        internal static Random rand = new Random();
        internal static uint rnd32()
        {
            return (uint)(rand.Next(1 << 30)) << 2 | (uint)(rand.Next(1 << 2));
        }
        bool start = true;
        bool loading = true;
        byte[][] personal;
        int index = -1;
        #region Global Variables
        private ComboBox[] trpk_pkm, trpk_lvl, trpk_item, trpk_abil,
            trpk_m1, trpk_m2, trpk_m3, trpk_m4, trpk_IV, trpk_form, trpk_gender;

        // Top Level Functions
        private string[] trdatapaths = Directory.GetFiles("trdata");
        private string[] trpokepaths = Directory.GetFiles("trpoke");
        private string[] abilitylist = Main.getText((Main.oras) ? 37 : 34);
        private string[] movelist = Main.getText((Main.oras) ? 14 : 13);
        private string[] itemlist = Main.getText((Main.oras) ? 114 : 96);
        private string[] specieslist = Main.getText((Main.oras) ? 98 : 80);
        private string[] types = Main.getText((Main.oras) ? 18 : 17);
        private string[] forms = Main.getText((Main.oras) ? 5 : 5);
        private string[] trName = Main.getText((Main.oras) ? 22 : 21);
        private string[] trClass = Main.getText((Main.oras) ? 21 : 20);
        #endregion

        // Ability Loading
        private void refreshFormAbility(object sender, EventArgs e)
        {
            int i = Array.IndexOf(trpk_form, sender as ComboBox);
            refreshPKMSlotAbility(i);
        }
        private void refreshSpeciesAbility(object sender, EventArgs e)
        {
            int i = Array.IndexOf(trpk_pkm, sender as ComboBox);
            Personal.setForms(trpk_pkm[i].SelectedIndex, trpk_form[i], AltForms);
            refreshPKMSlotAbility(i);
        }
        private void refreshPKMSlotAbility(int slot)
        {
            int previousAbility = trpk_abil[slot].SelectedIndex;

            int species = trpk_pkm[slot].SelectedIndex;
            int formnum = trpk_form[slot].SelectedIndex;
            species = (formnum > 0) 
                ? ((indexList[species] > 0) 
                    ? indexList[species] + formnum - 1 
                    : species) 
                : species;

            byte[] abilities = new byte[3];
            Array.Copy(personalData, ((Main.oras) ? 0x50 : 0x40) * species + 0x18, abilities, 0, 3);
            trpk_abil[slot].Items.Clear();
            trpk_abil[slot].Items.Add("Any (1 or 2)");
            trpk_abil[slot].Items.Add(abilitylist[abilities[0]] + " (1)");
            trpk_abil[slot].Items.Add(abilitylist[abilities[1]] + " (2)");
            trpk_abil[slot].Items.Add(abilitylist[abilities[2]] + " (H)");

            trpk_abil[slot].SelectedIndex = previousAbility;
        }
        // Set Loading
        private void changeTrainerType(object sender, EventArgs e)
        {
            if (start || loading) return;
            int pkm = CB_numPokemon.SelectedIndex;
            {
                for (int i = 0; i < 6; i++) // enable all if the pkm exists
                {
                    trpk_pkm[i].Enabled =
                    trpk_gender[i].Enabled =
                    trpk_abil[i].Enabled =
                    trpk_IV[i].Enabled =
                    trpk_lvl[i].Enabled = (i < pkm);

                    trpk_item[i].Enabled = (i < pkm) && (checkBox_Item.Checked);

                    trpk_m1[i].Enabled =
                    trpk_m2[i].Enabled =
                    trpk_m3[i].Enabled =
                    trpk_m4[i].Enabled = (i < pkm) && (checkBox_Moves.Checked);

                    if (!trpk_pkm[i].Enabled)
                    {
                        trpk_pkm[i].SelectedIndex =
                        trpk_gender[i].SelectedIndex =
                        trpk_form[i].SelectedIndex =
                        trpk_abil[i].SelectedIndex =
                        trpk_IV[i].SelectedIndex =
                        trpk_lvl[i].SelectedIndex = 0;
                    }
                    if (!trpk_item[i].Enabled)
                    {
                        trpk_item[i].SelectedIndex = 0;
                    }
                    if (!trpk_m1[i].Enabled)
                    {
                        trpk_m1[i].SelectedIndex =
                        trpk_m2[i].SelectedIndex =
                        trpk_m3[i].SelectedIndex =
                        trpk_m4[i].SelectedIndex = 0;
                    }
                }
                for (int i = pkm; i < 6; i++)
                    trpk_form[i].Enabled = false;
            }
        }

        // Dumping
        private string getTRSummary()
        {
            string toret = "======" + Environment.NewLine;

            toret += CB_TrainerID.SelectedIndex + " - " + CB_Trainer_Class.Text.Substring(0, CB_Trainer_Class.Text.Length - 6) + " " + CB_TrainerID.Text.Substring(0, CB_TrainerID.Text.Length - 6) + Environment.NewLine;
            toret += "======" + Environment.NewLine;
            int pkm = CB_numPokemon.SelectedIndex;
            toret += "Pokemon: " + pkm + Environment.NewLine;
            for (int i = 0; i < pkm; i++)
            {
                toret += trpk_pkm[i].Text + " (Lv. " + trpk_lvl[i].SelectedIndex + ") ";
                if (trpk_item[i].SelectedIndex > 0)
                    toret += "@" + trpk_item[i].Text;
                if (trpk_abil[i].SelectedIndex != 0)
                {
                    string abil = trpk_abil[i].Text;
                    abil = abil.Substring(0, abil.Length - 4);
                    toret += " (Ability: " + abil + ")";
                }
                if (checkBox_Moves.Checked)
                {
                    toret += " (Moves: ";
                    if (trpk_m1[i].SelectedIndex > 0) toret += trpk_m1[i].Text;
                    if (trpk_m2[i].SelectedIndex > 0) toret += " / " + trpk_m2[i].Text;
                    if (trpk_m3[i].SelectedIndex > 0) toret += " / " + trpk_m3[i].Text;
                    if (trpk_m4[i].SelectedIndex > 0) toret += " / " + trpk_m4[i].Text;
                    toret += ")";
                }
                toret += " IVs: All " + (Convert.ToInt32(trpk_IV[i].SelectedIndex) / 8);
                toret += Environment.NewLine;
            }
            toret += Environment.NewLine;
            return toret;
        }
        bool dumping;
        private void B_Dump_Click(object sender, EventArgs e)
        {
            string toret = ""; dumping = true;
            for (int i = 1; i < CB_TrainerID.Items.Count; i++)
            {
                CB_TrainerID.SelectedIndex = i;
                string tdata = getTRSummary();
                toret += tdata;
            }
            SaveFileDialog sfd = new SaveFileDialog {FileName = "Battles.txt", Filter = "Text File|*.txt"};

            SystemSounds.Asterisk.Play();
            if (sfd.ShowDialog() == DialogResult.OK)
            {
                string path = sfd.FileName;
                File.WriteAllText(path, toret, Encoding.Unicode);
            }
            dumping = false;
        }

        // Change Read
        private void changeTrainerIndex(object sender, EventArgs e)
        {
            if (!dumping && index > -1) writeFile();
            readFile(); // Load the new file.
        }
        private void readFile()
        {
            if (start) return;
            index = CB_TrainerID.SelectedIndex;
            loading = true;
            int format;

            if (index == 0) return;

            // Load Trainer Data
            using (BinaryReader br = new BinaryReader(File.OpenRead(trdatapaths[index])))
            {
                // load trainer data
                tabControl1.Enabled = true;
                {
                    format = (Main.oras) ? br.ReadUInt16() : br.ReadByte();
                    CB_Trainer_Class.SelectedIndex = (Main.oras) ? br.ReadUInt16() : br.ReadByte();
                    if (Main.oras) br.ReadUInt16();

                    checkBox_Item.Checked = ((format >> 1) & 1) == 1;
                    checkBox_Moves.Checked = ((format) & 1) == 1;

                    CB_Battle_Type.SelectedIndex = br.ReadByte();
                    CB_numPokemon.SelectedIndex = br.ReadByte();

                    CB_Item_1.SelectedIndex = br.ReadUInt16();
                    CB_Item_2.SelectedIndex = br.ReadUInt16();
                    CB_Item_3.SelectedIndex = br.ReadUInt16();
                    CB_Item_4.SelectedIndex = br.ReadUInt16();
                    CB_AI.SelectedIndex = br.ReadByte();
                    br.ReadByte();
                    br.ReadByte();
                    br.ReadByte();

                    checkBox_Healer.Checked = Convert.ToBoolean(br.ReadByte());
                    CB_Money.SelectedIndex = br.ReadByte();

                    CB_Prize.SelectedIndex = br.ReadUInt16();
                }
            }
            // Load Pokemon Data
            using (BinaryReader br = new BinaryReader(File.OpenRead(trpokepaths[index])))
            {
                for (int i = 0; i < CB_numPokemon.SelectedIndex; i++)
                {
                    trpk_IV[i].SelectedIndex = br.ReadByte();
                    byte PID = br.ReadByte();
                    trpk_lvl[i].SelectedIndex = br.ReadUInt16();
                    trpk_pkm[i].SelectedIndex = br.ReadUInt16();
                    Personal.setForms(trpk_pkm[i].SelectedIndex, trpk_form[i], AltForms);
                    trpk_form[i].SelectedIndex = br.ReadUInt16() % trpk_form[i].Items.Count; // stupid X/Y bug edge cases (220 / 222)
                    refreshPKMSlotAbility(i); // Repopulate Abilities

                    trpk_abil[i].SelectedIndex = PID >> 4;
                    trpk_gender[i].SelectedIndex = PID & 3;

                    if (((format >> 1) & 1) == 1) // Items Exist in Data
                        trpk_item[i].SelectedIndex = br.ReadUInt16();
                    if (((format) & 1) == 1) // Moves Exist in Data
                    {
                        trpk_m1[i].SelectedIndex = br.ReadUInt16();
                        trpk_m2[i].SelectedIndex = br.ReadUInt16();
                        trpk_m3[i].SelectedIndex = br.ReadUInt16();
                        trpk_m4[i].SelectedIndex = br.ReadUInt16();
                    }
                }
            }
            loading = false;
            changeTrainerType(null, null); // Prompt cleaning update of PKM fields
        }
        // Change Write
        private void writeFile()
        {
            // fetch trainer format we're saving as
            // index = index;
            int format = Convert.ToByte(checkBox_Moves.Checked) + (Convert.ToByte(checkBox_Item.Checked) << 1);

            // Write Trainer Data
            using (MemoryStream ms = new MemoryStream())
            using (BinaryWriter bw = new BinaryWriter(ms))
            {
                if (Main.oras)
                { bw.Write((ushort)format); bw.Write((ushort)CB_Trainer_Class.SelectedIndex); bw.Write((ushort)0); }
                else
                { bw.Write((byte)format); bw.Write((byte)CB_Trainer_Class.SelectedIndex); }

                bw.Write((byte)CB_Battle_Type.SelectedIndex);
                bw.Write((byte)CB_numPokemon.SelectedIndex);
                bw.Write((ushort)CB_Item_1.SelectedIndex);
                bw.Write((ushort)CB_Item_2.SelectedIndex);
                bw.Write((ushort)CB_Item_3.SelectedIndex);
                bw.Write((ushort)CB_Item_4.SelectedIndex);

                bw.Write((byte)CB_AI.SelectedIndex);
                bw.Write((byte)0);
                bw.Write((byte)0);
                bw.Write((byte)0);
                bw.Write((byte)Convert.ToInt32(checkBox_Healer.Checked));
                bw.Write((byte)CB_Money.SelectedIndex);
                bw.Write((ushort)CB_Prize.SelectedIndex);

                File.WriteAllBytes(trdatapaths[index], ms.ToArray());
            }
            // Load Pokemon Data
            using (MemoryStream ms = new MemoryStream())
            using (BinaryWriter bw = new BinaryWriter(ms))
            {
                for (int i = 0; i < CB_numPokemon.SelectedIndex; i++)
                {
                    bw.Write((byte)trpk_IV[i].SelectedIndex);
                    int PID = (byte)((trpk_abil[i].SelectedIndex << 4) + trpk_gender[i].SelectedIndex);
                    bw.Write((byte)PID);
                    bw.Write((ushort)trpk_lvl[i].SelectedIndex);

                    bw.Write((ushort)trpk_pkm[i].SelectedIndex);
                    bw.Write((ushort)trpk_form[i].SelectedIndex);

                    if (((format >> 1) & 1) == 1) // Items Exist in Data
                        bw.Write((ushort)trpk_item[i].SelectedIndex);
                    if (((format) & 1) == 1) // Moves Exist in Data
                    {
                        bw.Write((ushort)trpk_m1[i].SelectedIndex);
                        bw.Write((ushort)trpk_m2[i].SelectedIndex);
                        bw.Write((ushort)trpk_m3[i].SelectedIndex);
                        bw.Write((ushort)trpk_m4[i].SelectedIndex);
                    }
                }
                File.WriteAllBytes(trpokepaths[index], ms.ToArray());
            }
        }

        byte[] personalData;
        ushort[] indexList;
        private void Setup()
        {
            start = true;
            string[] personalList = Directory.GetFiles("personal");
            personalData = File.ReadAllBytes(personalList[personalList.Length - 1]);
            indexList = Personal.getPersonalIndexList(personalData, Main.oras);
            personal = new byte[personalList.Length][];
            for (int i = 0; i < personalList.Length; i++)
                personal[i] = File.ReadAllBytes("personal" + Path.DirectorySeparatorChar + i.ToString("000") + ".bin");
            AltForms = Personal.getFormList(personalData, Main.oras, specieslist, forms, types, itemlist);

            Array.Resize(ref trName, trdatapaths.Length);
            CB_TrainerID.Items.Clear();
            for (int i = 0; i < trdatapaths.Length; i++)
                CB_TrainerID.Items.Add(String.Format("{1} - {0}", i.ToString("000"), trName[i] ?? "UNKNOWN"));

            CB_Trainer_Class.Items.Clear();
            for (int i = 0; i < trClass.Length; i++)
                CB_Trainer_Class.Items.Add(String.Format("{1} - {0}", i.ToString("000"), trClass[i]));

            specieslist[0] = "---";
            abilitylist[0] = itemlist[0] = movelist[0] = "";

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

                trpk_item[i].Items.Clear();
                foreach (string s in itemlist)
                    trpk_item[i].Items.Add(s);

                trpk_lvl[i].Items.Clear();
                for (int z = 0; z <= 100; z++)
                    trpk_lvl[i].Items.Add((z).ToString());

                trpk_IV[i].Items.Clear();
                for (int z = 0; z < 256; z++)
                    trpk_IV[i].Items.Add(z.ToString());

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

            megaEvos = (Main.oras) ? new int[] { 15, 18, 80, 208, 254, 260, 302, 319, 323, 334, 362, 373, 376, 380, 381, 428, 475, 531, 719, 3, 6, 9, 65, 94, 115, 127, 130, 142, 150, 181, 212, 214, 229, 248, 257, 282, 303, 306, 308, 310, 354, 359, 445, 448, 460 } : new int[] { 3, 6, 9, 65, 94, 115, 127, 130, 142, 150, 181, 212, 214, 229, 248, 257, 282, 303, 306, 308, 310, 354, 359, 445, 448, 460 };
            
            CB_TrainerID.SelectedIndex = 1;
            start = false;
            readFile();
            SystemSounds.Asterisk.Play();
        }

        public static bool rPKM, rSmart, rLevel, rMove, rAbility, rDiffAI, rDiffIV, rClass, rGift, rItem, rDoRand, rTypeTheme, rTypeGymTrainers;
        public static bool[] rThemedClasses = new bool[] {};
        public static string[] rTags;
        public static int[] megaEvos;
        public static int[] rEnsureMEvo;
        private int[] mEvoTypes;
        private List<string> Tags = new List<string>();
        private Dictionary<string, int> TagTypes = new Dictionary<string, int>();
        public static int[] sL; // Random Species List
        public static decimal rGiftPercent, rLevelPercent;
        private void B_Randomize_Click(object sender, EventArgs e)
        {
            rPKM = rMove = rAbility = rDiffAI = rDiffIV = rClass = rGift = rItem = rDoRand = false; // init to false
            rGiftPercent = 0; // 0
            (new TrainerRand()).ShowDialog(); // Open Randomizer Config to get config vals
            if (rDoRand)
                Randomize();
        }
        private void Randomize()
        {
            rTags = (Main.oras) ? GetTagsORAS() : GetTagsXY();
            mEvoTypes = GetMegaEvolvableTypes();
            List<int> GymE4Types = new List<int>();
            if (rEnsureMEvo.Length > 0)
            {
                if (mEvoTypes.Length < 13 && rTypeTheme)
                {
                    Util.Alert("There are insufficient types with at least one mega evolution to Guarantee story Mega Evos while keeping Type theming.", "Re-Randomize Personal or don't choose both options.");
                    return;
                }
                GymE4Types.AddRange(mEvoTypes);
            }
            else
            {
                GymE4Types.AddRange(Enumerable.Range(0, types.Length).ToArray());
            }
            for (int i = 0; i < rEnsureMEvo.Length; i++) // Ensure Story MEvo Trainers have MEvoable Mons
            {
                if (rTags[rEnsureMEvo[i]] != "" && !TagTypes.Keys.Contains(rTags[rEnsureMEvo[i]]))
                {
                    int t;
                    if (rTags[rEnsureMEvo[i]].Contains("GYM") || rTags[rEnsureMEvo[i]].Contains("ELITE") || rTags[rEnsureMEvo[i]].Contains("CHAMPION"))
                    {
                        int roll = (int)(rnd32() % GymE4Types.Count);
                        t = GymE4Types[roll];
                        GymE4Types.Remove(t);
                    }
                    else
                    {
                        t = mEvoTypes[rnd32() % mEvoTypes.Length];
                    }
                    TagTypes[rTags[rEnsureMEvo[i]]] = t;
                    
                }
            }
            for (int i = 0; i < Tags.Count; i++)
            {
                if (!TagTypes.Keys.Contains(Tags[i]) && Tags[i] != "")
                {
                    int t;
                    if (Tags[i].Contains("GYM") || Tags[i].Contains("ELITE") || Tags[i].Contains("CHAMPION"))
                    {
                        int roll = (int)(rnd32() % GymE4Types.Count);
                        t = GymE4Types[roll];
                        GymE4Types.Remove(t);
                    }
                    else
                    {
                        t = (int)(rnd32() % types.Length);
                    }
                    TagTypes[Tags[i]] = t;
                }
                Console.WriteLine(Tags[i] + ": "+types[TagTypes[Tags[i]]]);
            }
            for (int i = 1; i < CB_TrainerID.Items.Count; i++)
            {
                CB_TrainerID.SelectedIndex = i; // data is loaded

                // Setup
                checkBox_Moves.Checked = rMove;
                checkBox_Item.Checked = rItem;

                // Randomize Trainer Stats
                if (rDiffAI)
                {
                    if (CB_Battle_Type.SelectedIndex == 0)
                        CB_AI.SelectedIndex = 7; // Max Single
                    else if (CB_Battle_Type.SelectedIndex == 1)
                        CB_AI.SelectedIndex = 135; // Max Double
                }
                if (rClass)
                    CB_Trainer_Class.SelectedIndex = CB_Battle_Type.SelectedIndex > 0 ? (int)(rnd32() % (CB_Trainer_Class.Items.Count)) : 0; // Change only Single Battles

                if (rGift && rnd32() % 100 < rGiftPercent)
                #region Random Prize Logic
                {
                    ushort[] items;
                    uint rnd = rnd32() % 10;
                    if (rnd < 2) // held item
                        items = (Main.oras) ? Legal.Pouch_Items_ORAS : Legal.Pouch_Items_XY;
                    else if (rnd < 5) // medicine
                        items = (Main.oras) ? Legal.Pouch_Medicine_ORAS : Legal.Pouch_Medicine_XY;
                    else // berry
                        items = Legal.Pouch_Berry_XY;
                    CB_Prize.SelectedIndex = items[(rnd32() % items.Length)];
                }
                #endregion
                else if (rGift)
                    CB_Prize.SelectedIndex = 0;

                ushort[] itemvals = (Main.oras) ? Legal.Pouch_Items_ORAS : Legal.Pouch_Items_XY;
                itemvals = itemvals.Concat(Legal.Pouch_Berry_XY).ToArray();
                int moves = trpk_m1[0].Items.Count;
                int itemC = itemvals.Length;
                int ctr = 0;

                int type = GetRandomType(i);
                bool mevo = rEnsureMEvo.Contains(i);

                // Randomize Pokemon
                for (int p = 0; p < CB_numPokemon.SelectedIndex; p++)
                {
                    if (rPKM)
                    {
                        // randomize pokemon
                        int species = Randomizer.getRandomSpecies(ref sL, ref ctr);
                        if (rTypeTheme)
                        {
                            while (personal[species][6] != type && personal[species][7] != type || (mevo && p == CB_numPokemon.SelectedIndex-1 && !megaEvos.Contains(species)))
                            {
                                if (p == CB_numPokemon.SelectedIndex - 1 && mevo)
                                {
                                    species = GetRandomMegaEvolvablePokemon(type);
                                }
                                else if (rSmart) // Get a new Pokemon with a close BST
                                {
                                    int oldBST = personal[trpk_pkm[p].SelectedIndex].Take(6).Sum(b => (ushort)b);
                                    int newBST = personal[species].Take(6).Sum(b => (ushort)b);
                                    while (!(newBST * 5 / 6 < oldBST && newBST * 6 / 5 > oldBST))
                                    { species = sL[rand.Next(1, sL.Length)]; newBST = personal[species].Take(6).Sum(b => (ushort)b); }
                                }
                                else
                                {
                                    species = Randomizer.getRandomSpecies(ref sL, ref ctr);
                                }
                            }
                        }
                        else if (p == CB_numPokemon.SelectedIndex - 1 && mevo)
                        {
                            species = megaEvos[rnd32() % megaEvos.Length];
                        }
                        else if (rSmart) // Get a new Pokemon with a close BST
                        {
                            int oldBST = personal[trpk_pkm[p].SelectedIndex].Take(6).Sum(b => (ushort)b);
                            int newBST = personal[species].Take(6).Sum(b => (ushort)b);
                            while (!(newBST * 5 / 6 < oldBST && newBST * 6 / 5 > oldBST))
                            { species = sL[rand.Next(1, sL.Length)]; newBST = personal[species].Take(6).Sum(b => (ushort)b); }
                        }

                        trpk_pkm[p].SelectedIndex = species;
                        // Set Gender to Random
                        trpk_gender[p].SelectedIndex = 0;

                        if (trpk_form[p].Items.Count > 0)
                            trpk_form[p].SelectedIndex = 0;
                        // Randomize form
                        if (trpk_form[p].Items.Count > 0 && !megaEvos.Contains(species))
                            trpk_form[p].SelectedIndex = (int)(rnd32() % trpk_form[p].Items.Count);
                    }
                    if (rLevel)
                        trpk_lvl[p].SelectedIndex = Math.Max(1, Math.Min((int)(trpk_lvl[p].SelectedIndex * ((100 + rLevelPercent) / 100)), 100));
                    if (rAbility)
                        trpk_abil[p].SelectedIndex = (int)(1 + rnd32() % 3);
                    if (rDiffIV)
                        trpk_IV[p].SelectedIndex = 255;
                    if (mevo && p == CB_numPokemon.SelectedIndex - 1)
                    {
                        int[] megastones = GetMegaStones(trpk_pkm[p].SelectedIndex);
                        trpk_item[p].SelectedIndex = megastones[rnd32() % megastones.Length];
                    }
                    else if (rItem)
                        #region RandomItem
                        trpk_item[p].SelectedIndex = itemvals[(rnd32() % itemC)];
                        #endregion
                    
                    if (rMove)
                    {
                        trpk_m1[p].SelectedIndex = (int)(rnd32() % (moves));
                        trpk_m2[p].SelectedIndex = (int)(rnd32() % (moves));
                        trpk_m3[p].SelectedIndex = (int)(rnd32() % (moves));
                        trpk_m4[p].SelectedIndex = (int)(rnd32() % (moves));
                    }
                }
            }
            CB_TrainerID.SelectedIndex = 1;
            Util.Alert("Randomized all trainers according to specification!", "Press the Dump to TXT to view the new trainer information!");
        }

        private string[] GetTagsORAS()
        {
            string[] tags = Enumerable.Repeat("", trdatapaths.Length).ToArray();
            
            //Rival Battles
            TagTrainer(tags, "RIVAL1", 1, 4, 289, 292, 295, 298, 527, 530, 674, 677, 699, 906); // Rival w/ Grass Starter
            TagTrainer(tags, "RIVAL2", 2, 5, 290, 293, 296, 299, 528, 531, 675, 678, 700, 907); // Rival w/ Fire Starter
            TagTrainer(tags, "RIVAL3", 3, 6, 291, 294, 297, 300, 529, 532, 676, 679, 701, 908); // Rival w/ Water Starter

            //Aqua Admins
            TagTrainer(tags, "AQUA1", 178, 231, 266);              // Archie
            TagTrainer(tags, "AQUA2", 683, 684, 685, 686, 687);    // Matt
            TagTrainer(tags, "AQUA3", 688, 689, 690);              // Shelly

            //Magma Admins
            TagTrainer(tags, "MAGMA1", 235, 236, 271);             // Maxie
            TagTrainer(tags, "MAGMA2", 694, 695, 696, 697, 698);   // Courney
            TagTrainer(tags, "MAGMA3", 691, 692, 693);             // Tabitha

            //Gym Leaders
            TagTrainer(tags, "GYM1", 561);         // Roxanne
            TagTrainer(tags, "GYM2", 563);         // Brawly
            TagTrainer(tags, "GYM3", 567);         // Wattson
            TagTrainer(tags, "GYM4", 569);         // Flannery
            TagTrainer(tags, "GYM5", 570);         // Norman
            TagTrainer(tags, "GYM6", 571);         // Winona
            TagTrainer(tags, "GYM7", 552);         // Liza & Tate
            TagTrainer(tags, "GYM8", 572, 943);    // Wallace

            //Elite 4
            TagTrainer(tags, "ELITE1", 553, 909);  // Sidney
            TagTrainer(tags, "ELITE2", 554, 910);  // Phoebe
            TagTrainer(tags, "ELITE3", 555, 911);  // Glacia
            TagTrainer(tags, "ELITE4", 556, 912);  // Drake
            TagTrainer(tags, "CHAMPION", 557, 680, 913, 942); // Champion Steven

            //Wally
            TagTrainer(tags, "WALLY", 518, 583, 944, 945, 946, 947);

            //Zinnia
            TagTrainer(tags, "LOREKEEPER", 713, 898);

            //Gym Trainers (Tagged in order of appearance on Bulbapedia's lists)
            if (rTypeGymTrainers)
            {
                TagTrainer(tags, "GYM1", 562, 22, 667);
                TagTrainer(tags, "GYM2", 60, 56, 59);
                TagTrainer(tags, "GYM3", 34, 568, 614, 35);
                TagTrainer(tags, "GYM4", 81, 824, 83, 615, 823, 613, 85);
                TagTrainer(tags, "GYM5", 63, 67, 64, 68, 65, 69, 66);
                TagTrainer(tags, "GYM6", 115, 517, 516, 118, 730);
                TagTrainer(tags, "GYM7", 157, 226, 320, 159, 225, 158);
                TagTrainer(tags, "GYM8", 647, 342, 594, 646, 338, 339, 340, 341);
            }
            return tags;
        }
        private string[] GetTagsXY()
        {
            string[] tags = Enumerable.Repeat("", trdatapaths.Length).ToArray();
            
            //Rival Battles
            TagTrainer(tags, "RIVAL1", 130, 184, 329, 332, 335, 338, 341, 435, 519, 604, 575, 578, 581, 584, 587, 590, 593, 596, 599, 607); // Rival w/ Fire Starter
            TagTrainer(tags, "RIVAL2", 131, 185, 330, 333, 336, 339, 342, 436, 520, 605, 576, 579, 582, 585, 588, 591, 594, 597, 600, 608); // Rival w/ Water Starter
            TagTrainer(tags, "RIVAL3", 132, 186, 331, 334, 337, 340, 343, 437, 521, 606, 577, 580, 583, 586, 589, 592, 595, 598, 601, 609); // Rival w/ Grass Starter

            //Important Flare Members
            TagTrainer(tags, "FLAREBOSS", 303, 525, 526);   // Lysandre
            TagTrainer(tags, "FLARE1", 175, 344);           // Aliana
            TagTrainer(tags, "FLARE2", 350, 351);           // Bryony
            TagTrainer(tags, "FLARE3", 348, 349);           // Celosia
            TagTrainer(tags, "FLARE4", 346, 347);           // Mable
            TagTrainer(tags, "FLARE5", 345);                // Xerosic

            //Gym Leaders
            TagTrainer(tags, "GYM1", 6, 254, 262);              // Viola
            TagTrainer(tags, "GYM2", 76, 261, 279);             // Grant
            TagTrainer(tags, "GYM3", 21, 188, 255, 263, 613);   // Korrina
            TagTrainer(tags, "GYM4", 22, 256, 264);             // Ramos
            TagTrainer(tags, "GYM5", 23, 257, 265);             // Clemont
            TagTrainer(tags, "GYM6", 24, 258, 266);             // Valerie
            TagTrainer(tags, "GYM7", 25, 259, 267);             // Olympia
            TagTrainer(tags, "GYM8", 26, 260, 268);             // Wulfric

            //Elite 4
            TagTrainer(tags, "ELITE1", 269, 273, 507);  // Malva
            TagTrainer(tags, "ELITE2", 271, 275);       // Siebold
            TagTrainer(tags, "ELITE3", 187, 272);       // Wikstrom
            TagTrainer(tags, "ELITE4", 270, 274);       // Drasna
            TagTrainer(tags, "CHAMPION", 276, 277);     // Champion Diantha

            //"Friends"
            TagTrainer(tags, "SHAUNA", 137, 138, 139, 321, 322, 323);
            TagTrainer(tags, "TREVOR", 325, 439);
            TagTrainer(tags, "TIERNO", 324, 438, 573);

            //Prof
            TagTrainer(tags, "PROFESSOR", 327, 328);

            //Suspicious Trainer ???
            TagTrainer(tags, "ESSENTIA", 503, 504, 505, 511, 512, 513, 514, 515); // Emma

            //Gym Trainers (Tagged in order of appearance on Bulbapedia's lists)
            if (rTypeGymTrainers)
            {
                TagTrainer(tags, "GYM1", 39, 40, 48);
                TagTrainer(tags, "GYM2", 64, 63, 106, 105);
                TagTrainer(tags, "GYM3", 83, 147, 84, 146);
                TagTrainer(tags, "GYM4", 123, 121, 124, 122);
                TagTrainer(tags, "GYM5", 461, 462, 463, 464, 465, 466, 28, 29, 30, 467, 468, 469);
                TagTrainer(tags, "GYM6", 245, 250, 248, 243);
                TagTrainer(tags, "GYM7", 170, 171, 172, 365, 366);
                TagTrainer(tags, "GYM8", 169, 32, 168, 31);
            }
            return tags;
        }

        private void TagTrainer(string[] rTags, string tag, params int[] ids)
        {
            foreach (int id in ids)
            {
                if (id < rTags.Length)
                    rTags[id] = tag;
            }
            if (!Tags.Contains(tag))
                Tags.Add(tag);
        }

        private int[] GetMegaStones(int species) // This is horrible.
        {
            switch (species)
            {
                case 3:
                    return new int[] { 659 };
                case 6:
                    return new int[] { 660, 678 };
                case 9:
                    return new int[] { 661 };
                case 15:
                    return new int[] { 770 };
                case 18:
                    return new int[] { 762 };
                case 65:
                    return new int[] { 679 };
                case 80:
                    return new int[] { 760 };
                case 94:
                    return new int[] { 656 };
                case 115:
                    return new int[] { 675 };
                case 127:
                    return new int[] { 671 };
                case 130:
                    return new int[] { 676 };
                case 142:
                    return new int[] { 672 };
                case 150:
                    return new int[] { 662, 663 };
                case 181:
                    return new int[] { 658 };
                case 208:
                    return new int[] { 761 };
                case 212:
                    return new int[] { 670 };
                case 214:
                    return new int[] { 680 };
                case 229:
                    return new int[] { 666 };
                case 248:
                    return new int[] { 669 };
                case 254:
                    return new int[] { 753 };
                case 257:
                    return new int[] { 664 };
                case 260:
                    return new int[] { 752 };
                case 282:
                    return new int[] { 657 };
                case 302:
                    return new int[] { 754 };
                case 303:
                    return new int[] { 681 };
                case 306:
                    return new int[] { 667 };
                case 308:
                    return new int[] { 665 };
                case 310:
                    return new int[] { 682 };
                case 319:
                    return new int[] { 759 };
                case 323:
                    return new int[] { 767 };
                case 334:
                    return new int[] { 755 };
                case 354:
                    return new int[] { 668 };
                case 359:
                    return new int[] { 677 };
                case 362:
                    return new int[] { 763 };
                case 373:
                    return new int[] { 769 };
                case 376:
                    return new int[] { 758 };
                case 380:
                    return new int[] { 684 };
                case 381:
                    return new int[] { 685 };
                case 428:
                    return new int[] { 768 };
                case 445:
                    return new int[] { 683 };
                case 448:
                    return new int[] { 673 };
                case 460:
                    return new int[] { 674 };
                case 475:
                    return new int[] { 756 };
                case 531:
                    return new int[] { 757 };
                case 719:
                    return new int[] { 764 };
                default:
                    return new int[] { };
            }
        }

        private int GetRandomType(int trainer)
        {
            if (rTags[trainer] == "")
            {
                if (rEnsureMEvo.Contains(trainer))
                {
                    int t = mEvoTypes[rnd32() % mEvoTypes.Length];
                    return t;
                }
                else
                    return (int)(rnd32() % types.Length);
            }
            else
            {
                return TagTypes[rTags[trainer]];
            }
        }

        private int[] GetMegaEvolvableTypes()
        {
            List<int> MEvoTypes = new List<int>();
            foreach (int spec in megaEvos)
            {
                if (!MEvoTypes.Contains((int)personal[spec][6]))
                    MEvoTypes.Add((int)personal[spec][6]);
                if (!MEvoTypes.Contains((int)personal[spec][7]))
                    MEvoTypes.Add((int)personal[spec][7]);
            }
            MEvoTypes.Sort();
            Console.WriteLine("There are " + MEvoTypes.Count + " types capable of mega evolution.");
            return MEvoTypes.ToArray();
        }

        private int GetRandomMegaEvolvablePokemon(int type)
        {
            List<int> valids = new List<int>();
            foreach (int spec in megaEvos)
            {
                if ((int)personal[spec][6] == type || (int)personal[spec][7] == type)
                    valids.Add(spec);
            }
            return valids[(int)(rnd32() % valids.Count)];
        }
    }
}