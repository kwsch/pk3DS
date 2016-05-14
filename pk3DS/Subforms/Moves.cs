using System;
using System.IO;
using System.Windows.Forms;

namespace pk3DS
{
    public partial class Moves : Form
    {
        public Moves()
        {
            if (Main.oras)
                CTR.mini.unpackMini(Directory.GetFiles("move")[0], "WD");

            movelist[0] = "";
            sortedmoves = (string[])movelist.Clone();
            Array.Sort(sortedmoves);

            InitializeComponent();
            Setup();
        }
        private string[] files = Directory.GetFiles("move");
        private readonly string[] types = Main.getText(Main.oras ? 18 : 17);
        private readonly string[] moveflavor = Main.getText(Main.oras ? 16 : 15);
        private readonly string[] movelist = Main.getText(Main.oras ? 14 : 13);
        private readonly string[] sortedmoves;
        private readonly string[] MoveCategories = { "Status", "Physical", "Special", };
        private readonly string[] StatCategories = { "None", "Attack", "Defense", "Special Attack", "Special Defense", "Speed", "Accuracy", "Evasion", "All", };

        private readonly string[] TargetingTypes =
        { "Single Adjacent Ally/Foe", 
            "Any Ally", "Any Adjacent Ally", "Single Adjacent Foe", "Everyone but User", "All Foes", 
            "All Allies", "Self", "All Pokemon on Field", "Single Adjacent Foe (2)", "Entire Field", 
            "Opponent's Field", "User's Field", "Self", 
        };

        private readonly string[] InflictionTypes =
        { "None", 
            "Paralyze", "Sleep", "Freeze", "Burn", "Poison", 
            "Confusion", "Attract", "Capture", "Nightmare", "Curse", 
            "Taunt", "Torment", "Disable", "Yawn", "Heal Block", 
            "?", "Detect", "Leech Seed", "Embargo", "Perish Song", 
            "Ingrain", 
        };

        private readonly string[] MoveQualities =
        { "Only DMG", 
            "No DMG -> Inflict Status", "No DMG -> -Target/+User Stat", "No DMG | Heal User", "DMG | Inflict Status", "No DMG | STATUS | +Target Stat", 
            "DMG | -Target Stat", "DMG | +User Stat", "DMG | Absorbs DMG", "One-Hit KO", "Affects Whole Field", 
            "Affect One Side of the Field", "Forces Target to Switch", "Unique Effect",  };
        private void Setup()
        {
            foreach (string s in sortedmoves) CB_Move.Items.Add(s);
            foreach (string s in types) CB_Type.Items.Add(s);
            foreach (string s in MoveCategories) CB_Category.Items.Add(s);
            foreach (string s in StatCategories) CB_Stat1.Items.Add(s);
            foreach (string s in StatCategories) CB_Stat2.Items.Add(s);
            foreach (string s in StatCategories) CB_Stat3.Items.Add(s);
            foreach (string s in TargetingTypes) CB_Targeting.Items.Add(s);
            foreach (string s in MoveQualities) CB_Quality.Items.Add(s);
            foreach (string s in InflictionTypes) CB_Inflict.Items.Add(s);
            CB_Inflict.Items.Add("Special");

            CB_Move.Items.RemoveAt(0);
            files = Directory.GetFiles("move");
            CB_Move.SelectedIndex = 0;
        }
        private int entry = -1;
        private void changeEntry(object sender, EventArgs e)
        {
            setEntry();
            entry = Array.IndexOf(movelist, CB_Move.Text);
            getEntry();
        }
        private void getEntry()
        {
            if (entry < 1) return;
            byte[] data = File.ReadAllBytes(files[entry]);
            {
                string flavor = moveflavor[entry].Replace("\\n", Environment.NewLine);
                RTB.Text = flavor;

                CB_Type.SelectedIndex = data[0x00];
                CB_Quality.SelectedIndex = data[0x01];
                CB_Category.SelectedIndex = data[0x02];
                NUD_Power.Value = data[0x3];
                NUD_Accuracy.Value = data[0x4];
                NUD_PP.Value = data[0x05];
                NUD_Priority.Value = (sbyte)data[0x06];
                NUD_HitMin.Value = data[0x7] & 0xF;
                NUD_HitMax.Value = data[0x7] >> 4;
                short inflictVal = BitConverter.ToInt16(data, 0x08);
                CB_Inflict.SelectedIndex = inflictVal < 0 ? CB_Inflict.Items.Count - 1 : inflictVal;
                NUD_Inflict.Value = data[0xA];
                NUD_0xB.Value = data[0xB]; // 0xB ~ Something to deal with skipImmunity
                NUD_TurnMin.Value = data[0xC];
                NUD_TurnMax.Value = data[0xD];
                NUD_CritStage.Value = data[0xE];
                NUD_Flinch.Value = data[0xF];
                NUD_Effect.Value = BitConverter.ToUInt16(data, 0x10);
                NUD_Recoil.Value = (sbyte)data[0x12];
                NUD_Heal.Value = data[0x13];

                CB_Targeting.SelectedIndex = data[0x14];
                CB_Stat1.SelectedIndex = data[0x15];
                CB_Stat2.SelectedIndex = data[0x16];
                CB_Stat3.SelectedIndex = data[0x17];
                NUD_Stat1.Value = (sbyte)data[0x18];
                NUD_Stat2.Value = (sbyte)data[0x19];
                NUD_Stat3.Value = (sbyte)data[0x1A];
                NUD_StatP1.Value = data[0x1B];
                NUD_StatP2.Value = data[0x1C];
                NUD_StatP3.Value = data[0x1D];

                // Unknown (Bitflag Related for stuff like Contact and Extra Move Effects)
                NUD_0x20.Value = data[0x20]; // 0x20
                NUD_0x21.Value = data[0x21]; // 0x21
                // end, the other bytes aren't used.

                //NUD_0x1E.Value = data[0x1E]; // 0x1E
                //NUD_0x1F.Value = data[0x1F]; // 0x1F
                //NUD_0x22.Value = data[0x22]; // 0x22
                //NUD_0x23.Value = data[0x23]; // 0x23
            }
        }
        private void setEntry()
        {
            if (entry < 1) return;
            byte[] data = File.ReadAllBytes(files[entry]);
            {
                data[0x00] = (byte)CB_Type.SelectedIndex;
                data[0x01] = (byte)CB_Quality.SelectedIndex;
                data[0x02] = (byte)CB_Category.SelectedIndex;
                data[0x03] = (byte)NUD_Power.Value;
                data[0x04] = (byte)NUD_Accuracy.Value;
                data[0x05] = (byte)NUD_PP.Value;
                data[0x06] = (byte)(int)NUD_Priority.Value;
                data[0x07] = (byte)((byte)NUD_HitMin.Value | ((byte)NUD_HitMax.Value << 4));
                int inflictval = CB_Inflict.SelectedIndex; if (inflictval == CB_Inflict.Items.Count) inflictval = -1;
                Array.Copy(BitConverter.GetBytes((short)inflictval), 0, data, 0x08, 2);
                data[0x0A] = (byte)NUD_Inflict.Value;
                data[0x0B] = (byte)NUD_0xB.Value;
                data[0x0C] = (byte)NUD_TurnMin.Value;
                data[0x0D] = (byte)NUD_TurnMax.Value;
                data[0x0E] = (byte)NUD_CritStage.Value;
                data[0x0F] = (byte)NUD_Flinch.Value;
                Array.Copy(BitConverter.GetBytes((ushort)NUD_Effect.Value), 0, data, 0x10, 2);
                data[0x12] = (byte)(int)NUD_Recoil.Value;
                data[0x13] = (byte)NUD_Heal.Value;
                data[0x14] = (byte)CB_Targeting.SelectedIndex;
                data[0x15] = (byte)CB_Stat1.SelectedIndex;
                data[0x16] = (byte)CB_Stat2.SelectedIndex;
                data[0x17] = (byte)CB_Stat3.SelectedIndex;
                data[0x18] = (byte)(int)NUD_Stat1.Value;
                data[0x19] = (byte)(int)NUD_Stat2.Value;
                data[0x1A] = (byte)(int)NUD_Stat3.Value;
                data[0x1B] = (byte)NUD_StatP1.Value;
                data[0x1C] = (byte)NUD_StatP2.Value;
                data[0x1D] = (byte)NUD_StatP3.Value;

                data[0x20] = (byte)NUD_0x20.Value;
                data[0x21] = (byte)NUD_0x21.Value;
                // end, the other bytes aren't used.
            }
            File.WriteAllBytes(files[entry], data);
        }
        private void formClosing(object sender, FormClosingEventArgs e)
        {
            setEntry();

            if (Main.oras)
                CTR.mini.packMini("move", "WD", "0", ".bin");
        }

        private void B_RandAll_Click(object sender, EventArgs e)
        {
            if (!CHK_Category.Checked && !CHK_Type.Checked) return;
            Random rnd = new Random();
            for (int i = 0; i < CB_Move.Items.Count; i++)
            {
                CB_Move.SelectedIndex = i; // Get new Move
                if (i == 165 || i == 174) continue; // Don't change Struggle or Curse

                // Change Damage Category if Not Status
                if (CB_Category.SelectedIndex > 0 && CHK_Category.Checked) // Not Status
                    CB_Category.SelectedIndex = rnd.Next(1, 3);

                // Change Move Type
                if (CHK_Type.Checked)
                    CB_Type.SelectedIndex = rnd.Next(0, 18);
            }
            Util.Alert("Moves have been randomized!");
        }

        internal static Move[] getMoves()
        {
            if (Main.oras)
                CTR.mini.unpackMini(Directory.GetFiles("move")[0], "WD");
            string[] f2 = Directory.GetFiles("move");
            Move[] moves = new Move[f2.Length];
            for (int i = 0; i < f2.Length; i++)
                moves[i] = new Move(File.ReadAllBytes(f2[i]));

            if (Main.oras)
                CTR.mini.packMini("move", "WD", "0", ".bin");

            return moves;
        }
    }
}