using System;
using System.Collections.Generic;

namespace pk3DS
{
    public class trdata7
    {
        private readonly byte[] trdata;
        public readonly List<trpoke7> Pokemon = new List<trpoke7>();

        public int ID { get; set; }
        public string Name { get; set; }

        public trdata7(byte[] tr = null, byte[] tp = null)
        {
            tr = tr ?? new byte[0x14];
            tp = tp ?? new byte[0x20];
            trdata = (byte[])tr.Clone();
            for (int i = 0; i < NumPokemon; i++)
            {
                byte[] poke = new byte[0x20];
                Array.Copy(tp, i * 0x20, poke, 0, 0x20);
                Pokemon.Add(new trpoke7(poke));
            }
        }

        public byte TrainerClass { get { return trdata[0]; } set { trdata[0] = value; } }

        public int NumPokemon { get { return trdata[3]; } set { trdata[3] = (byte)(value%7); } }

        public void Write(out byte[] tr, out byte[] pk)
        {
            tr = trdata;
            byte[] dat = new byte[trpoke7.SIZE * NumPokemon];
            for (int i = 0; i < NumPokemon; i++)
                Pokemon[i].Write().CopyTo(dat, trpoke7.SIZE*i);
            pk = dat;
        }
    }
}
