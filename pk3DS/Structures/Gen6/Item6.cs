using System.IO;

namespace pk3DS
{
    public class Item
    {
        public ushort Price;
        public int BuyPrice;
        public int SellPrice;
        public byte HeldEffect, HeldArgument, NaturalGiftEffect, FlingEffect, FlingPower, NaturalGiftPower;
        public byte NaturalGiftType, u8Flags;
        public byte KeyFlags;
        public byte UseEffect;
        public byte _0xB; // Battle Type
        public byte _0xC; // 0 or 1
        public byte _0xD; // Classification (0-3 Battle, 4 Balls, 5 Mail)
        public byte Consumable;
        public byte SortIndex;
        public byte CureInflict; // Bitflags
        public byte FieldEffect; // Revive 1, Sacred Ash 3, Rare Candy 5, EvoStone 8
        public int BoostATK,
            BoostDEF, BoostSPA,
            BoostSPD, BoostSPE,
            BoostACC, BoostCRIT, BoostPP;
        public ushort FunctionFlags;

        public ushort FieldBoost; // 0x15-0x16
        public byte EVHP, EVATK, EVDEF, EVSPE, EVSPA, EVSPD;
        public Heal Healing;
        public byte PPGain, Friendship1, Friendship2, Friendship3;
        public byte _0x23, _0x24;

        public Item(byte[] data)
        {
            using (BinaryReader br = new BinaryReader(new MemoryStream(data)))
            {
                Price = br.ReadUInt16();
                BuyPrice = Price * 10;
                SellPrice = Price * 5;

                HeldEffect = br.ReadByte();
                HeldArgument = br.ReadByte();
                NaturalGiftEffect = br.ReadByte();
                FlingEffect = br.ReadByte();
                FlingPower = br.ReadByte();
                NaturalGiftPower = br.ReadByte();
                NaturalGiftType = br.ReadByte();
                u8Flags = (byte)(NaturalGiftType >> 5);
                NaturalGiftType &= 0x1F;
                KeyFlags = br.ReadByte();
                UseEffect = br.ReadByte();
                _0xB = br.ReadByte();
                _0xC = br.ReadByte();
                _0xD = br.ReadByte();
                Consumable = br.ReadByte();
                SortIndex = br.ReadByte();
                CureInflict = br.ReadByte();

                FieldEffect = br.ReadByte();
                BoostATK = FieldEffect >> 4;
                FieldEffect &= 0xF;
                BoostDEF = br.ReadByte();
                BoostSPA = BoostDEF >> 4;
                BoostDEF &= 0xF;
                BoostSPD = br.ReadByte();
                BoostSPE = BoostSPD >> 4;
                BoostSPD &= 0xF;
                BoostACC = br.ReadByte();
                BoostCRIT = (BoostACC >> 4) & 0x3;
                BoostPP = BoostACC >> 6;
                BoostACC &= 0xF;

                FunctionFlags = br.ReadUInt16();

                EVHP = br.ReadByte();
                EVATK = br.ReadByte();
                EVDEF = br.ReadByte();
                EVSPE = br.ReadByte();
                EVSPA = br.ReadByte();
                EVSPD = br.ReadByte();

                Healing = new Heal(br.ReadByte());
                PPGain = br.ReadByte();
                Friendship1 = br.ReadByte();
                Friendship2 = br.ReadByte();
                Friendship3 = br.ReadByte();
                _0x23 = br.ReadByte();
                _0x24 = br.ReadByte();
            }
        }
        public byte[] Write()
        {
            using (MemoryStream ms = new MemoryStream())
            using (BinaryWriter bw = new BinaryWriter(ms))
            {
                bw.Write(Price);
                bw.Write(HeldEffect);
                bw.Write(HeldArgument);
                bw.Write(NaturalGiftEffect);
                bw.Write(FlingEffect);
                bw.Write(FlingPower);
                bw.Write(NaturalGiftPower);
                bw.Write((byte)(NaturalGiftType | (u8Flags << 5)));
                bw.Write(KeyFlags);
                bw.Write(UseEffect);
                bw.Write(_0xB);
                bw.Write(_0xC);
                bw.Write(_0xD);
                bw.Write(Consumable);
                bw.Write(SortIndex);
                bw.Write(CureInflict);

                bw.Write((byte)(FieldEffect | (BoostATK << 4)));
                bw.Write((byte)(BoostDEF | (BoostSPA << 4)));
                bw.Write((byte)(BoostSPD | (BoostSPE << 4)));
                bw.Write((byte)(BoostACC | (BoostCRIT << 4) | (BoostPP << 6)));

                bw.Write(FunctionFlags);

                bw.Write(EVHP);
                bw.Write(EVATK);
                bw.Write(EVDEF);
                bw.Write(EVSPE);
                bw.Write(EVSPA);
                bw.Write(EVSPD);
                bw.Write(Healing.Write());
                bw.Write(PPGain);
                bw.Write(Friendship1);
                bw.Write(Friendship2);
                bw.Write(Friendship3);

                bw.Write(_0x23);
                bw.Write(_0x24);

                return ms.ToArray();
            }
        }
        public class Heal
        {
            public byte Val;
            public bool Full, Half, Quarter, Value;
            public Heal(byte val)
            {
                Val = val;
                Full = Val == 0xFF;
                Half = Val == 0xFE;
                Quarter = Val == 0xFD;
                Value = Val < 0xFD;
            }
            public byte Write()
            {
                if (Value)
                    return Val;
                if (Full)
                    return 0xFF;
                if (Half)
                    return 0xFE;
                if (Quarter)
                    return 0xFD;
                return Val;
            }
        }
    }
}
