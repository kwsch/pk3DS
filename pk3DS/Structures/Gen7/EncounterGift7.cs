using System;

namespace pk3DS
{
    public class EncounterGift7 : EncounterStatic
    {
        public const int SIZE = 0x14;
        public readonly byte[] Data;
        public EncounterGift7(byte[] data)
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
        public int Gender { get { return Data[0x4]; } set { Data[0x4] = (byte)value; } }
        public override int HeldItem { get { return BitConverter.ToUInt16(Data, 0x8); } set { BitConverter.GetBytes((ushort)value).CopyTo(Data, 0x8); } }
    }
}
