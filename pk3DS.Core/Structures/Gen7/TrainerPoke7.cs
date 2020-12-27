using System;

namespace pk3DS.Core.Structures
{
    public class TrainerPoke7
    {
        public const int SIZE = 0x20;
        private readonly byte[] Data;

        public TrainerPoke7(byte[] d = null)
        {
            Data = (byte[])(d ?? new byte[SIZE]).Clone();
            if (Data.Length != 0x20)
                throw new ArgumentException("Invalid trpoke7!");
        }

        public TrainerPoke7 Clone()
        {
            return new(Write());
        }

        public int Gender
        {
            get => Data[0] & 0x3;
            set => Data[0] = (byte)((Data[0] & 0xFC) | (value & 0x3));
        }

        public int Ability
        {
            get => (Data[0] >> 4) & 0x3;
            set => Data[0] = (byte)((Data[0] & 0xCF) | ((value & 0x3) << 4));
        }

        public int Nature { get => Data[1]; set => Data[1] = (byte)value; }

        public int EV_HP { get => Data[0x2]; set => Data[0x2] = (byte)value; }
        public int EV_ATK { get => Data[0x3]; set => Data[0x3] = (byte)value; }
        public int EV_DEF { get => Data[0x4]; set => Data[0x4] = (byte)value; }
        public int EV_SPA { get => Data[0x5]; set => Data[0x5] = (byte)value; }
        public int EV_SPD { get => Data[0x6]; set => Data[0x6] = (byte)value; }
        public int EV_SPE { get => Data[0x7]; set => Data[0x7] = (byte)value; }

        private uint IV32 { get => BitConverter.ToUInt32(Data, 0x8); set => BitConverter.GetBytes(value).CopyTo(Data, 0x8); }

        public int IV_HP { get => (int)(IV32 >> 00) & 0x1F; set => IV32 = (uint)((IV32 & ~(0x1F << 00)) | (uint)((value > 31 ? 31 : value) << 00)); }
        public int IV_ATK { get => (int)(IV32 >> 05) & 0x1F; set => IV32 = (uint)((IV32 & ~(0x1F << 05)) | (uint)((value > 31 ? 31 : value) << 05)); }
        public int IV_DEF { get => (int)(IV32 >> 10) & 0x1F; set => IV32 = (uint)((IV32 & ~(0x1F << 10)) | (uint)((value > 31 ? 31 : value) << 10)); }
        public int IV_SPA { get => (int)(IV32 >> 15) & 0x1F; set => IV32 = (uint)((IV32 & ~(0x1F << 15)) | (uint)((value > 31 ? 31 : value) << 15)); }
        public int IV_SPD { get => (int)(IV32 >> 20) & 0x1F; set => IV32 = (uint)((IV32 & ~(0x1F << 20)) | (uint)((value > 31 ? 31 : value) << 20)); }
        public int IV_SPE { get => (int)(IV32 >> 25) & 0x1F; set => IV32 = (uint)((IV32 & ~(0x1F << 25)) | (uint)((value > 31 ? 31 : value) << 25)); }
        public bool Shiny { get => ((IV32 >> 30) & 1) == 1; set => IV32 = ((IV32 & ~0x40000000u) | (value ? 0x40000000u : 0)); }

        public int Level { get => Data[0xE]; set => Data[0xE] = (byte)value; }
        public int Species { get => BitConverter.ToUInt16(Data, 0x10); set => BitConverter.GetBytes((ushort)value).CopyTo(Data, 0x10); }
        public int Form { get => Data[0x12]; set => Data[0x12] = (byte)value; }

        public int Item { get => BitConverter.ToUInt16(Data, 0x14); set => BitConverter.GetBytes((ushort)value).CopyTo(Data, 0x14); }
        public int Move1 { get => BitConverter.ToUInt16(Data, 0x18); set => BitConverter.GetBytes((ushort)value).CopyTo(Data, 0x18); }
        public int Move2 { get => BitConverter.ToUInt16(Data, 0x1A); set => BitConverter.GetBytes((ushort)value).CopyTo(Data, 0x1A); }
        public int Move3 { get => BitConverter.ToUInt16(Data, 0x1C); set => BitConverter.GetBytes((ushort)value).CopyTo(Data, 0x1C); }
        public int Move4 { get => BitConverter.ToUInt16(Data, 0x1E); set => BitConverter.GetBytes((ushort)value).CopyTo(Data, 0x1E); }

        public int[] IVs
        {
            get => new[] { IV_HP, IV_ATK, IV_DEF, IV_SPA, IV_SPD, IV_SPE };
            set
            {
                if (value?.Length != 6) return;
                IV_HP = value[0]; IV_ATK = value[1]; IV_DEF = value[2];
                IV_SPA = value[3]; IV_SPD = value[4]; IV_SPE = value[5];
            }
        }

        public int[] EVs
        {
            get => new[] { EV_HP, EV_ATK, EV_DEF, EV_SPA, EV_SPD, EV_SPE };
            set
            {
                if (value?.Length != 6) return;
                EV_HP = value[0]; EV_ATK = value[1]; EV_DEF = value[2];
                EV_SPA = value[3]; EV_SPD = value[4]; EV_SPE = value[5];
            }
        }

        public int[] Moves
        {
            get => new[] { Move1, Move2, Move3, Move4 };
            set { if (value?.Length != 4) return; Move1 = value[0]; Move2 = value[1]; Move3 = value[2]; Move4 = value[3]; }
        }

        public byte[] Write() => (byte[])Data.Clone();
    }
}
