using System;

namespace pk3DS.Core.Structures
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
            get => BitConverter.ToUInt16(Data, 0x0);
            set => BitConverter.GetBytes((ushort)value).CopyTo(Data, 0x0);
        }
        public int Form
        {
            get => Data[0x2];
            set => Data[0x2] = (byte)value;
        }
        public int Level
        {
            get => Data[0x3];
            set => Data[0x3] = (byte)value;
        }
        public int Gender
        {
            get => Data[0x4] & 1;
            set => Data[0x4] = (byte)(value & 1);
        }
        public bool ShinyLock
        {
            get => (Data[0x4] & 2) != 0;
            set => Data[0x4] = (byte)((Data[0x4] & ~2) | (value ? 2 : 0));
        }
        public sbyte _6
        {
            get => (sbyte)Data[0x6];
            set => Data[0x6] = (byte)value;
        }
        public sbyte _7
        {
            get => (sbyte)Data[0x6];
            set => Data[0x6] = (byte)value;
        }
        public override int HeldItem
        {
            get => BitConverter.ToUInt16(Data, 0x8);
            set => BitConverter.GetBytes((ushort)value).CopyTo(Data, 0x8);
        }
        public byte _A
        {
            get => Data[0xA];
            set => Data[0xA] = value;
        }
        public int _C
        {
            get => BitConverter.ToUInt16(Data, 0xC);
            set => BitConverter.GetBytes((ushort)value).CopyTo(Data, 0xC);
        }

        public string GetSummary()
        {
            var str = $"new EncounterStatic {{ Gift = true, Species = {Species:000}, Level = {Level:00}, Location = -01, ";
            if (Form != 0)
                str += $"Form = {Form}, ";
            if (ShinyLock)
                str += "Shiny = false, ";
            if (HeldItem != 0)
                str += $"HeldItem = {HeldItem}, ";

            str = str.Trim() + " },";
            return str;
        }
    }
}
