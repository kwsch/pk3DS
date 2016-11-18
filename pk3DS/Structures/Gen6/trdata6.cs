using System;
using System.IO;
using System.Linq;

namespace pk3DS
{
    public class trdata6
    {
        public bool isORAS;
        public int Format, Class;
        public bool Item, Moves;
        public byte BattleType, NumPokemon, AI;
        public ushort[] Items = new ushort[4];
        public byte u1, u2, u3;
        public ushort uORAS;
        public bool Healer;
        public byte Money;
        public ushort Prize;
        public Pokemon[] Team;
        public trdata6(byte[] trData, byte[] trPoke, bool ORAS)
        {
            using (BinaryReader br = new BinaryReader(new MemoryStream(trData)))
            {
                isORAS = ORAS;
                Format = ORAS ? br.ReadUInt16() : br.ReadByte();
                Class = ORAS ? br.ReadUInt16() : br.ReadByte();
                if (ORAS) uORAS = br.ReadUInt16();
                Item = ((Format >> 1) & 1) == 1;
                Moves = (Format & 1) == 1;
                BattleType = br.ReadByte();
                NumPokemon = br.ReadByte();
                for (int i = 0; i < 4; i++)
                    Items[i] = br.ReadUInt16();
                AI = br.ReadByte();
                u1 = br.ReadByte();
                u2 = br.ReadByte();
                u3 = br.ReadByte();
                Healer = br.ReadByte() != 0;
                Money = br.ReadByte();
                Prize = br.ReadUInt16();

                // Fetch Team
                Team = new Pokemon[NumPokemon];
                byte[][] TeamData = new byte[NumPokemon][];
                int dataLen = trPoke.Length / NumPokemon;
                for (int i = 0; i < TeamData.Length; i++)
                    TeamData[i] = trPoke.Skip(i * dataLen).Take(dataLen).ToArray();
                for (int i = 0; i < NumPokemon; i++)
                    Team[i] = new Pokemon(TeamData[i], Item, Moves);
            }
        }
        public byte[] Write()
        {
            using (MemoryStream ms = new MemoryStream())
            using (BinaryWriter bw = new BinaryWriter(ms))
            {
                Format = Convert.ToByte(Moves) + (Convert.ToByte(Item) << 1);
                if (isORAS)
                { bw.Write((ushort)Format); bw.Write((ushort)Class); bw.Write((ushort)0); }
                else
                { bw.Write((byte)Format); bw.Write((byte)Class); }

                bw.Write(BattleType);
                bw.Write(NumPokemon);
                bw.Write(Items[0]);
                bw.Write(Items[1]);
                bw.Write(Items[2]);
                bw.Write(Items[3]);

                bw.Write(AI);
                bw.Write(u1);
                bw.Write(u2);
                bw.Write(u3);
                bw.Write(Convert.ToByte(Healer));
                bw.Write(Money);
                bw.Write(Prize);

                return ms.ToArray();
            }
        }
        public byte[] WriteTeam()
        {
            return Team.Aggregate(new byte[0], (i, pkm) => i.Concat(pkm.Write(Item, Moves)).ToArray());
        }

        public class Pokemon
        {
            public byte IVs;
            public byte PID;
            public ushort Level;
            public ushort Species;
            public ushort Form;
            public int Ability;
            public int Gender;
            public int uBit;
            public ushort Item;
            public ushort[] Moves = new ushort[4];

            public Pokemon(byte[] data, bool HasItem, bool HasMoves)
            {
                using (BinaryReader br = new BinaryReader(new MemoryStream(data)))
                {
                    IVs = br.ReadByte();
                    PID = br.ReadByte();
                    Level = br.ReadUInt16();
                    Species = br.ReadUInt16();
                    Form = br.ReadUInt16();

                    Ability = PID >> 4;
                    Gender = PID & 3;
                    uBit = (PID >> 3) & 1;

                    if (HasItem)
                        Item = br.ReadUInt16();
                    if (HasMoves)
                        for (int i = 0; i < 4; i++)
                            Moves[i] = br.ReadUInt16();
                }
            }
            public byte[] Write(bool HasItem, bool HasMoves)
            {
                using (MemoryStream ms = new MemoryStream())
                using (BinaryWriter bw = new BinaryWriter(ms))
                {
                    bw.Write(IVs);
                    PID = (byte)(((Ability & 0xF) << 4) | ((uBit & 1) << 3) | (Gender & 0x7));
                    bw.Write(PID);
                    bw.Write(Level);
                    bw.Write(Species);
                    bw.Write(Form);

                    if (HasItem)
                        bw.Write(Item);
                    if (HasMoves)
                        foreach (ushort Move in Moves)
                            bw.Write(Move);
                    return ms.ToArray();
                }
            }
        }
    }
}
