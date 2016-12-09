using System;

namespace pk3DS
{
    public class EncounterStatic7 : EncounterStatic
    {
        public const int SIZE = 0x38;
        public readonly byte[] Data;
        public EncounterStatic7(byte[] data)
        {
            Data = data;
        }
        public override int Species
        {
            get { return BitConverter.ToUInt16(Data, 0x0); }
            set { BitConverter.GetBytes((ushort)value).CopyTo(Data, 0x0); }
        }
        public int Form { get { return Data[0x2]; } set { Data[0x2] = (byte)value; } }
        public int Level { get { return Data[0x3]; } set { Data[0x3] = (byte)value; } }
        public override int HeldItem
        {
            get
            {
                int item = BitConverter.ToInt16(Data, 0x4);
                if (item < 0) item = 0;
                return item;
            }
            set
            {
                if (value == 0) value = -1;
                BitConverter.GetBytes((short)value).CopyTo(Data, 0x4);
            }
        }

        public bool ShinyLock { get { return (Data[0x6] & 2) >> 1 == 1; } set { Data[0x6] = (byte)(Data[0x6] & ~2 | (value ? 2 : 0)); } }
        public int Gender { get { return (Data[0x6] & 0xC) >> 2; } set { Data[0x6] = (byte)(Data[0x6] & ~0xC | ((value & 3) << 2)); } }
        public int Ability { get { return (Data[0x6] & 0x70) >> 4; } set { Data[0x6] = (byte)(Data[0x6] & ~0x70 | ((value & 7) << 4)); } }

        public bool IV3 { get { return (Data[0x7] & 1) >> 0 == 1; } set { Data[0x7] = (byte)(Data[0x7] & ~1 | (value ? 1 : 0)); } }
        public bool IV3_1 { get { return (Data[0x7] & 2) >> 1 == 1; } set { Data[0x7] = (byte)(Data[0x7] & ~2 | (value ? 2 : 0)); } }

        public int[] RelearnMoves
        {
            get
            {
                return new int[]
                {
                    BitConverter.ToUInt16(Data, 0xC),
                    BitConverter.ToUInt16(Data, 0xE),
                    BitConverter.ToUInt16(Data, 0x10),
                    BitConverter.ToUInt16(Data, 0x12),
                };
            }
            set
            {
                if (value.Length != 4)
                    return;
                for (int i = 0; i < 4; i++)
                    BitConverter.GetBytes((ushort)value[i]).CopyTo(Data, 0xC + i * 2);
            }
        }

        public int[] IVs
        {
            get
            {
                return new int[]
                {
                        (sbyte) Data[0x15], (sbyte) Data[0x16], (sbyte) Data[0x17], (sbyte) Data[0x18], (sbyte) Data[0x19], (sbyte) Data[0x1A]
                };
            }
            set
            {
                if (value.Length != 6)
                    return;
                for (int i = 0; i < 6; i++)
                    Data[i + 0x15] = (byte)Convert.ToSByte(value[i]);
            }
        }
    }
}
