using System;
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
            species = (indexList[species] > 0) ? indexList[species] + formnum - 1 : species;

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

            CB_TrainerID.SelectedIndex = 1;
            start = false;
            readFile();
            SystemSounds.Asterisk.Play();
        }

        public static bool rPKM, rSmart, rLevel, rMove, rAbility, rDiffAI, rDiffIV, rClass, rGift, rItem, rDoRand;
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
                    CB_Trainer_Class.SelectedIndex = (int)(rnd32() % (CB_Trainer_Class.Items.Count));

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
                // Randomize Pokemon
                for (int p = 0; p < CB_numPokemon.SelectedIndex; p++)
                {
                    if (rPKM)
                    {
                        // randomize pokemon
                        int species = rand.Next(1, 722);
                        if (rSmart) // Get a new Pokemon with a close BST
                        {
                            int oldBST = personal[trpk_pkm[p].SelectedIndex].Take(6).Sum(b => (ushort)b);
                            int newBST = personal[species].Take(6).Sum(b => (ushort)b);
                            while (!(newBST * 5 / 6 < oldBST && newBST * 6 / 5 > oldBST))
                            { species = rand.Next(1, 722); newBST = personal[species].Take(6).Sum(b => (ushort)b); }
                        }
                        trpk_pkm[p].SelectedIndex = species;
                        // Set Gender to Random
                        trpk_gender[p].SelectedIndex = 0;

                        // Randomize form
                        trpk_form[p].SelectedIndex = (int)(rnd32() % trpk_form[p].Items.Count);
                    }
                    if (rLevel)
                        trpk_lvl[p].SelectedIndex = Math.Min((int)(trpk_lvl[p].SelectedIndex * ((100 + rLevelPercent) / 100)), 100);
                    if (rAbility)
                        trpk_abil[p].SelectedIndex = (int)(1 + rnd32() % 3);
                    if (rDiffIV)
                        trpk_IV[p].SelectedIndex = 255;
                    if (rItem)
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
    }
}