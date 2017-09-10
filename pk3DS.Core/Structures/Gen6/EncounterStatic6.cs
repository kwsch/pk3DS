using System;

namespace pk3DS.Core.Structures
{
    public class EncounterStatic6
    {
        // All
        public readonly byte[] Data;

        public ushort Species
        {
            get { return BitConverter.ToUInt16(Data, 0x0); }
            set { BitConverter.GetBytes(value).CopyTo(Data, 0x0); }
        }
        public byte Form { get { return Data[0x2]; } set { Data[0x2] = value; } }
        public byte Level { get { return Data[0x3]; } set { Data[0x3] = value; } }
        public int HeldItem
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

        public EncounterStatic6(byte[] data)
        {
            Data = (byte[])data.Clone();
        }
        public byte[] Write()
        {
            return (byte[])Data.Clone();
        }
    }
}
