﻿using System;

namespace pk3DS
{
    public class EncounterTrade7 : EncounterStatic
    {
        public const int SIZE = 0x34;

        public readonly byte[] Data;
        public EncounterTrade7(byte[] data)
        {
            Data = data;
        }
        public override int Species
        {
            get { return BitConverter.ToUInt16(Data, 0x0); }
            set { BitConverter.GetBytes((ushort)value).CopyTo(Data, 0x0); }
        }
        public int Form { get { return Data[0x4]; } set { Data[0x4] = (byte)value; } }
        public int Level { get { return Data[0x5]; } set { Data[0x5] = (byte)value; } }
        public int[] IVs
        {
            get
            {
                return new int[]
                {
                        (sbyte) Data[0x6], (sbyte) Data[0x7], (sbyte) Data[0x8], (sbyte) Data[0x9], (sbyte) Data[0xA], (sbyte) Data[0xB]
                };
            }
            set
            {
                if (value.Length != 6)
                    return;
                for (int i = 0; i < 6; i++)
                    Data[i + 0x6] = (byte)Convert.ToSByte(value[i]);
            }
        }
        public int Ability { get { return Data[0xC]; } set { Data[0xC] = (byte)value; } }
        public int Nature { get { return Data[0xD]; } set { Data[0xD] = (byte)value; } }
        public int Gender { get { return Data[0xE]; } set { Data[0xE] = (byte)value; } }
        public int TID { get { return BitConverter.ToUInt16(Data, 0x10); } set { BitConverter.GetBytes((ushort)value).CopyTo(Data, 0x10); } }
        public int SID { get { return BitConverter.ToUInt16(Data, 0x12); } set { BitConverter.GetBytes((ushort)value).CopyTo(Data, 0x12); } }
        public override int HeldItem
        {
            get
            {
                int item = BitConverter.ToInt16(Data, 0x14);
                if (item < 0) item = 0;
                return item;
            }
            set
            {
                if (value == 0) value = -1;
                BitConverter.GetBytes((short)value).CopyTo(Data, 0x14);
            }
        }
        public int trGender { get { return Data[0x1A]; } set { Data[0x1A] = (byte)value; } }
    }
}
