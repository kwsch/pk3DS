using System;
using System.Collections.Generic;

namespace pk3DS.Core.Structures
{
    public class TrainerData7
    {
        private readonly byte[] trdata;
        public readonly List<TrainerPoke7> Pokemon = new();

        public int ID { get; set; }
        public string Name { get; set; }

        public TrainerData7(byte[] tr = null, byte[] tp = null)
        {
            tr ??= new byte[0x14];
            tp ??= new byte[0x20];
            trdata = (byte[])tr.Clone();
            for (int i = 0; i < NumPokemon; i++)
            {
                byte[] poke = new byte[0x20];
                Array.Copy(tp, i * 0x20, poke, 0, 0x20);
                Pokemon.Add(new TrainerPoke7(poke));
            }
        }

        public int TrainerClass { get => BitConverter.ToUInt16(trdata, 0x00); set => BitConverter.GetBytes((ushort)value).CopyTo(trdata, 0x00); }
        public BattleMode Mode { get => (BattleMode)trdata[2]; set => trdata[2] = (byte)value; }
        public int NumPokemon { get => trdata[3]; set => trdata[3] = (byte)(value%7); }
        public int Item1 { get => BitConverter.ToUInt16(trdata, 0x04); set => BitConverter.GetBytes((ushort)value).CopyTo(trdata, 0x04); }
        public int Item2 { get => BitConverter.ToUInt16(trdata, 0x06); set => BitConverter.GetBytes((ushort)value).CopyTo(trdata, 0x06); }
        public int Item3 { get => BitConverter.ToUInt16(trdata, 0x08); set => BitConverter.GetBytes((ushort)value).CopyTo(trdata, 0x08); }
        public int Item4 { get => BitConverter.ToUInt16(trdata, 0x0A); set => BitConverter.GetBytes((ushort)value).CopyTo(trdata, 0x0A); }

        public int AI { get => trdata[0x0C]; set => trdata[0x0C] = (byte)value; }
        public bool Flag { get => trdata[0x0D] == 1; set => trdata[0x0D] = value ? (byte)1 : (byte)0; }
        public int Money { get => trdata[0x11]; set => trdata[0x11] = (byte)value; }

        public void Write(out byte[] tr, out byte[] pk)
        {
            tr = trdata;
            byte[] dat = new byte[TrainerPoke7.SIZE * NumPokemon];
            for (int i = 0; i < NumPokemon; i++)
                Pokemon[i].Write().CopyTo(dat, TrainerPoke7.SIZE*i);
            pk = dat;
        }
    }

    public enum BattleMode : byte
    {
        Singles,
        Doubles,
        Multi,
    }
}
