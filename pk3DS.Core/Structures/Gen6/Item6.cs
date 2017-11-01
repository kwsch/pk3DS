using System.Runtime.InteropServices;

namespace pk3DS.Core.Structures
{
    [StructLayout(LayoutKind.Sequential)]
    public struct Item
    {
        public Item(byte[] data) => this = data.ToStructure<Item>();
        public byte[] Write() => this.ToBytes();

        #region Structure
        public ushort Price;
        public byte HeldEffect;
        public byte HeldArgument;
        public byte NaturalGiftEffect;
        public byte FlingEffect;
        public byte FlingPower;
        public byte NaturalGiftPower;
        public byte NaturalGiftTypeFlags;
        public byte KeyFlags;
        public byte UseEffect;
        public byte _0xB; // Battle Type
        public byte _0xC; // 0 or 1
        public byte _0xD; // Classification (0-3 Battle, 4 Balls, 5 Mail)
        public byte Consumable;
        public byte SortIndex;
        public BattleStatusFlags CureInflict; // Bitflags
        private byte Boost0; // Revive 1, Sacred Ash 3, Rare Candy 5, EvoStone 8, upper4 for BoostAtk
        private byte Boost1; // DEF, SPA
        private byte Boost2; // SPD, SPE
        private byte Boost3; // ACC, CRIT
        public byte FunctionFlags0;
        public byte FunctionFlags1;
        public byte EVHP;
        public byte EVATK;
        public byte EVDEF;
        public byte EVSPE;
        public byte EVSPA;
        public byte EVSPD;
        public HealValue HealAmount;
        public byte PPGain;
        public byte Friendship1;
        public byte Friendship2;
        public byte Friendship3;
        public byte _0x23, _0x24;
        #endregion

        // Exposing more info
        public int BuyPrice { get => Price * 10; set => Price = (ushort)(value / 10); }
        public int SellPrice { get => Price * 5; set => Price = (ushort)(value / 5); }
        public int NaturalGiftType { get => NaturalGiftTypeFlags & 0x1F; set => NaturalGiftTypeFlags = (byte)(NaturalGiftEffect & ~0x1F | value); }
        public int U8Flags { get => NaturalGiftTypeFlags >> 5; set => NaturalGiftTypeFlags = (byte)(NaturalGiftEffect & 0x1F | (value << 5)); }

        public int FieldEffect { get => Boost0 & 0xF; set => Boost0 = (byte)(Boost0 & ~0xF | (value & 0xF)); }
        public int BoostATK { get => Boost0 >> 4; set => Boost0 = (byte)(Boost0 & 0xF | (value << 4)); }

        public int BoostDEF { get => Boost1 & 0xF; set => Boost1 = (byte)(Boost1 & ~0xF | (value & 0xF)); }
        public int BoostSPA { get => Boost1 >> 4; set => Boost1 = (byte)(Boost1 & 0xF | (value << 4)); }

        public int BoostSPD { get => Boost2 & 0xF; set => Boost2 = (byte)(Boost2 & ~0xF | (value & 0xF)); }
        public int BoostSPE { get => Boost2 >> 4; set => Boost2 = (byte)(Boost2 & 0xF | (value << 4)); }

        public int BoostACC { get => Boost0 & 0xF; set => Boost0 = (byte)(Boost3 & ~0xF | (value & 0xF)); }
        public int BoostCRIT { get => (Boost3 >> 4) & 3; set => Boost3 = (byte)(Boost3 & ~0x30 | ((value & 3) << 4)); }
        public int BoostPP { get => (Boost3 >> 6) & 3; set => Boost3 = (byte)(Boost3 & 0x3F | ((value & 3) << 6)); }
    }
}
