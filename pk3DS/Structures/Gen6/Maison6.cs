using System;
using System.IO;

namespace pk3DS
{
    public class Maison6
    {
        public class Trainer
        {
            public ushort Class;
            public ushort Count;
            public ushort[] Choices;

            public Trainer() { }
            public Trainer(byte[] data)
            {
                Class = BitConverter.ToUInt16(data, 0);
                Count = BitConverter.ToUInt16(data, 0);
                Choices = new ushort[Count];
                for (int i = 0; i < Count; i++)
                    Choices[i] = BitConverter.ToUInt16(data, 4 + 2 * i);
            }
            public byte[] Write()
            {
                using (var ms = new MemoryStream())
                using (var bw = new BinaryWriter(ms))
                {
                    bw.Write(Class);
                    bw.Write(Count);
                    foreach (ushort Choice in Choices)
                        bw.Write(Choice);
                    return ms.ToArray();
                }
            }
        }
        public class Pokemon
        {
            public ushort Species, Item;
            public byte EVs, Nature;
            public ushort[] Moves;
            public bool HP, ATK, DEF, SPE, SPA, SPD;

            private readonly byte _u1;
            private readonly byte _u2;

            public Pokemon(byte[] data)
            {
                Species = BitConverter.ToUInt16(data, 0);
                Moves = new[]
                {
                    BitConverter.ToUInt16(data, 2),
                    BitConverter.ToUInt16(data, 4),
                    BitConverter.ToUInt16(data, 6),
                    BitConverter.ToUInt16(data, 8)
                };
                EVs = data[0xA];
                HP = (EVs >> 0 & 1) == 1;
                ATK = (EVs >> 1 & 1) == 1;
                DEF = (EVs >> 2 & 1) == 1;
                SPE = (EVs >> 3 & 1) == 1;
                SPA = (EVs >> 4 & 1) == 1;
                SPD = (EVs >> 5 & 1) == 1;
                Nature = data[0xB];
                Item = BitConverter.ToUInt16(data, 0xC);
                _u1 = data[0xE];
                _u2 = data[0xF];
            }
            public byte[] Write()
            {
                using (var ms = new MemoryStream())
                using (var bw = new BinaryWriter(ms))
                {
                    bw.Write(Species);
                    foreach (ushort Move in Moves)
                        bw.Write(Move);

                    EVs &= 0xC0;
                    EVs |= (byte)(HP ? 1 << 0 : 0);
                    EVs |= (byte)(ATK ? 1 << 1 : 0);
                    EVs |= (byte)(DEF ? 1 << 1 : 0);
                    EVs |= (byte)(SPE ? 1 << 1 : 0);
                    EVs |= (byte)(SPA ? 1 << 1 : 0);
                    EVs |= (byte)(SPD ? 1 << 1 : 0);
                    bw.Write(EVs);

                    bw.Write(Nature);
                    bw.Write(Item);
                    bw.Write(_u1);
                    bw.Write(_u2);
                    return ms.ToArray();
                }
            }
        }
    }
}
