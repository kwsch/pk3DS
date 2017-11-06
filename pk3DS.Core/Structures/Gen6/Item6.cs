using System.ComponentModel;
using System.Runtime.InteropServices;

namespace pk3DS.Core.Structures
{
    [StructLayout(LayoutKind.Sequential)]
    public struct Item
    {
        public Item(byte[] data) => this = data.ToStructure<Item>();
        public byte[] Write() => this.ToBytes();
        private const string Battle = "Battle";
        private const string Field = "Field";
        private const string Mart = "Mart";
        private const string Heal = "Heal";

        #region Structure
        private ushort Price;
        [Category(Battle)]
        public byte HeldEffect { get; set; }
        public byte HeldArgument { get; set; }
        public byte NaturalGiftEffect { get; set; }
        public byte FlingEffect { get; set; }
        public byte FlingPower { get; set; }
        private byte NaturalGiftPower { get; set; }
        public byte NaturalGiftTypeFlags { get; set; }
        public byte KeyFlags { get; set; }
        [Category(Field), Description("Routine # to call when used; 0=unusable.")]
        public byte UseEffect { get; set; }
        public byte _0xB { get; set; } // Battle Type
        public byte _0xC { get; set; } // 0 or 1
        public byte _0xD { get; set; } // Classification (0-3 Battle, 4 Balls, 5 Mail)
        public byte Consumable { get; set; }
        public byte SortIndex { get; set; }
        public BattleStatusFlags CureInflict { get; set; } // Bitflags
        private byte Boost0; // Revive 1, Sacred Ash 3, Rare Candy 5, EvoStone 8, upper4 for BoostAtk
        private byte Boost1; // DEF, SPA
        private byte Boost2; // SPD, SPE
        private byte Boost3; // ACC, CRIT
        public byte FunctionFlags0 { get; set; }
        public byte FunctionFlags1{ get; set; }
        [Category(Field), Description("Adds EVs to the HP stat.")]
        public sbyte EVHP { get; set; }
        [Category(Field), Description("Adds EVs to the Attack stat.")]
        public sbyte EVATK { get; set; }
        [Category(Field), Description("Adds EVs to the Defense stat.")]
        public sbyte EVDEF { get; set; }
        [Category(Field), Description("Adds EVs to the Speed stat.")]
        public sbyte EVSPE { get; set; }
        [Category(Field), Description("Adds EVs to the Sp. Attack stat.")]
        public sbyte EVSPA { get; set; }
        [Category(Field), Description("Adds EVs to the Sp. Defense stat.")]
        public sbyte EVSPD { get; set; }
        [Category(Heal), Description("Determines the healing percent, or if a flat value is used."), RefreshProperties(RefreshProperties.All)]
        public HealValue HealAmount { get; set; }
        [Category(Field), Description("PP to be added to the move's current PP if used.")]
        public byte PPGain { get; set; }
        public sbyte Friendship1 { get; set; }
        public sbyte Friendship2 { get; set; }
        public sbyte Friendship3 { get; set; }
        public byte _0x23, _0x24;
        #endregion

        [Category(Mart), RefreshProperties(RefreshProperties.All)]
        public int BuyPrice { get => Price * 10; set => Price = (ushort)(value / 10); }
        [Category(Mart), ReadOnly(true)]
        public int SellPrice { get => Price * 5; set => Price = (ushort)(value / 5); }

        [Category(Battle)]
        public int NaturalGiftType { get => NaturalGiftTypeFlags & 0x1F; set => NaturalGiftTypeFlags = (byte)(NaturalGiftEffect & ~0x1F | value); }
        [Category(Battle)]
        public int U8Flags { get => NaturalGiftTypeFlags >> 5; set => NaturalGiftTypeFlags = (byte)(NaturalGiftEffect & 0x1F | (value << 5)); }

        [Category(Battle)]
        public int FieldEffect { get => Boost0 & 0xF; set => Boost0 = (byte)(Boost0 & ~0xF | (value & 0xF)); }
        [Category(Battle)]
        public int BoostATK { get => Boost0 >> 4; set => Boost0 = (byte)(Boost0 & 0xF | (value << 4)); }

        [Category(Battle)]
        public int BoostDEF { get => Boost1 & 0xF; set => Boost1 = (byte)(Boost1 & ~0xF | (value & 0xF)); }
        [Category(Battle)]
        public int BoostSPA { get => Boost1 >> 4; set => Boost1 = (byte)(Boost1 & 0xF | (value << 4)); }

        [Category(Battle)]
        public int BoostSPD { get => Boost2 & 0xF; set => Boost2 = (byte)(Boost2 & ~0xF | (value & 0xF)); }
        [Category(Battle)]
        public int BoostSPE { get => Boost2 >> 4; set => Boost2 = (byte)(Boost2 & 0xF | (value << 4)); }

        [Category(Battle)]
        public int BoostACC { get => Boost0 & 0xF; set => Boost0 = (byte)(Boost3 & ~0xF | (value & 0xF)); }
        [Category(Battle)]
        public int BoostCRIT { get => (Boost3 >> 4) & 3; set => Boost3 = (byte)(Boost3 & ~0x30 | ((value & 3) << 4)); }
        [Category(Battle)]
        public int BoostPP { get => (Boost3 >> 6) & 3; set => Boost3 = (byte)(Boost3 & 0x3F | ((value & 3) << 6)); }

        [Category(Heal), Description("Raw value of the Heal enum."), RefreshProperties(RefreshProperties.All)]
        public int HealValue
        {
            get => (int) HealAmount;
            set => HealAmount = (HealValue)value;
        }
    }
}
