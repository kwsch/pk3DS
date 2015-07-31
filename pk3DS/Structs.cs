using System;
using System.Drawing;
using System.IO;
using System.Linq;

namespace pk3DS
{
    public class RNG
    {
        public static uint Forward32(uint seed, int ctr)
        {
            for (int i = 0; i < ctr; i++)
            {
                seed *= 0x41C64E6D;
                seed += 0x00006073;
            }
            return seed;
        }
    }
    public class PersonalInfo
    {
        public byte HP, ATK, DEF, SPE, SPA, SPD;
        public int BST;
        private ushort EVs;
        public int EV_HP, EV_ATK, EV_DEF, EV_SPE, EV_SPA, EV_SPD;
        public byte[] Types = new byte[2];
        public byte CatchRate, EvoStage;
        public ushort[] Items = new ushort[3];
        public byte Gender, HatchCycles, BaseFriendship, EXPGrowth;
        public byte[] EggGroups = new byte[2];
        public byte[] Abilities = new byte[3];
        public ushort FormStats, FormeSprite, BaseEXP;
        public byte FormeCount, Color;
        public float Height, Weight;
        public bool[] TMHM;
        public bool[] Tutors;
        public bool[][] ORASTutors = new bool[4][];
        public byte EscapeRate;
        public PersonalInfo(byte[] data)
        {
            using (BinaryReader br = new BinaryReader(new MemoryStream(data)))
            {
                HP = br.ReadByte(); ATK = br.ReadByte(); DEF = br.ReadByte();
                SPE = br.ReadByte(); SPA = br.ReadByte(); SPD = br.ReadByte();
                BST = HP + ATK + DEF + SPE + SPA + SPD;

                Types = new[] { br.ReadByte(), br.ReadByte() };
                CatchRate = br.ReadByte();
                EvoStage = br.ReadByte();

                EVs = br.ReadUInt16();
                EV_HP = ((EVs >> 0) & 0x3);
                EV_ATK = ((EVs >> 2) & 0x3);
                EV_DEF = ((EVs >> 4) & 0x3);
                EV_SPE = ((EVs >> 6) & 0x3);
                EV_SPA = ((EVs >> 8) & 0x3);
                EV_SPD = ((EVs >> 10) & 0x3);

                Items = new[] { br.ReadUInt16(), br.ReadUInt16(), br.ReadUInt16() };
                Gender = br.ReadByte();
                HatchCycles = br.ReadByte();
                BaseFriendship = br.ReadByte();

                EXPGrowth = br.ReadByte();
                EggGroups = new[] { br.ReadByte(), br.ReadByte() };
                Abilities = new[] { br.ReadByte(), br.ReadByte(), br.ReadByte() };
                EscapeRate = br.ReadByte();
                FormStats = br.ReadUInt16();

                FormeSprite = br.ReadUInt16();
                FormeCount = br.ReadByte();
                Color = br.ReadByte();
                BaseEXP = br.ReadUInt16();

                Height = br.ReadUInt16();
                Weight = br.ReadUInt16();

                byte[] TMHMData = br.ReadBytes(0x10);
                TMHM = new bool[8 * TMHMData.Length];
                for (int j = 0; j < TMHM.Length; j++)
                    TMHM[j / 8 + j % 8] = ((TMHMData[j / 8] >> j % 8) & 0x1) == 1; //Bitflags for TMHM

                byte[] TutorData = br.ReadBytes(8);
                Tutors = new bool[8 * TutorData.Length];
                for (int j = 0; j < Tutors.Length; j++)
                    Tutors[j / 8 + j % 8] = ((TutorData[j / 8] >> j % 8) & 0x1) == 1; //Bitflags for Tutors

                if (br.BaseStream.Length - br.BaseStream.Position == 0x10) // ORAS
                {
                    byte[][] ORASTutorData =
                        {
                            br.ReadBytes(2), // 15
                            br.ReadBytes(3), // 17
                            br.ReadBytes(2), // 16
                            br.ReadBytes(2), // 15
                        };
                    for (int i = 0; i < 4; i++)
                    {
                        ORASTutors[i] = new bool[8 * ORASTutorData[i].Length];
                        for (int b = 0; b < 8 * ORASTutorData[i].Length; b++)
                            ORASTutors[i][b] = ((ORASTutorData[i][b / 8] >> b % 8) & 0x1) == 1;
                    }
                }
            }
        }
        public byte[] Write()
        {
            using (MemoryStream ms = new MemoryStream())
            using (BinaryWriter bw = new BinaryWriter(ms))
            {
                bw.Write(HP);
                bw.Write(ATK);
                bw.Write(DEF);
                bw.Write(SPE);
                bw.Write(SPD);
                bw.Write(SPE);
                foreach (byte Type in Types) bw.Write(Type);
                bw.Write(CatchRate);
                bw.Write(EvoStage);
                EVs = (ushort)(EVs & 0x80 | (HP >> 0 & 3) | (ATK >> 2 & 3) | (DEF >> 4 & 3) | (SPE >> 6 & 3) | (SPA >> 8 & 3) | (SPD >> 10 & 3));
                bw.Write(EVs);
                foreach (ushort Item in Items) bw.Write(Item);
                bw.Write(Gender);
                bw.Write(HatchCycles);
                bw.Write(BaseFriendship);
                bw.Write(EXPGrowth);
                foreach (byte EggGroup in EggGroups) bw.Write(EggGroup);
                foreach (byte Ability in Abilities) bw.Write(Ability);
                bw.Write(EscapeRate);
                bw.Write(FormStats);
                bw.Write(FormeSprite);
                bw.Write(FormeCount);
                bw.Write(Color);
                bw.Write(BaseEXP);
                bw.Write(BitConverter.GetBytes(Convert.ToUInt16(Height)));
                bw.Write(BitConverter.GetBytes(Convert.ToUInt16(Weight)));

                byte[] TMHMData = new byte[0x10];
                for (int i = 0; i < TMHM.Length; i++)
                    TMHMData[i % 8] |= (byte)(TMHM[i] ? (1 << i % 8) : 0);
                bw.Write(TMHMData);

                byte[] TutorData = new byte[8];
                for (int i = 0; i < Tutors.Length; i++)
                    TutorData[i % 8] |= (byte)(Tutors[i] ? (1 << i % 8) : 0);
                bw.Write(TutorData);

                while (bw.BaseStream.Length != 0x40) bw.Write((byte)0);

                if (ORASTutors[0] != null) // ORAS Data
                {
                    byte[][] ORASTutorData =
                        {
                            new byte[2], // 15
                            new byte[3], // 17
                            new byte[2], // 16
                            new byte[2], // 15
                        };
                    for (int i = 0; i < 4; i++)
                        for (int b = 0; b < ORASTutors[i].Length; b++)
                            ORASTutorData[i][b / 8] = (byte)(ORASTutors[i][b] ? (1 << b % 8) : 0);

                    foreach (byte[] ORASTutor in ORASTutorData) bw.Write(ORASTutor);

                    while (bw.BaseStream.Length != 0x50) bw.Write((byte)0);
                }
                return ms.ToArray();
            }
        }
    }
    public class Trainer
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
        public Trainer(byte[] trData, byte[] trPoke, bool ORAS)
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
    public class Move
    {
        public byte Type, Quality, Category, Power, Accuracy, PP, Priority, InflictPercent,
            HitMin, HitMax, TurnMin, TurnMax, CritStage, Flinch, Recoil, Targeting,
            Stat1, Stat2, Stat3,
            Stat1Stage, Stat2Stage, Stat3Stage,
            Stat1Percent, Stat2Percent, Stat3Percent;

        public Heal Healing;
        public ushort Inflict, Effect;
        public byte _0xB, _0x1E, _0x1F, _0x20, _0x21;
        public Move(byte[] data)
        {
            Type = data[0];
            Quality = data[1];
            Category = data[2];
            Power = data[3];
            Accuracy = data[4];
            PP = data[5];
            Priority = data[6];
            HitMin = (byte)(data[7] & 0xF);
            HitMax = (byte)(data[7] >> 4);
            Inflict = BitConverter.ToUInt16(data, 0x8);
            InflictPercent = data[0xA];
            _0xB = data[0xB];
            TurnMin = data[0xC];
            TurnMax = data[0xD];
            CritStage = data[0xE];
            Flinch = data[0xF];
            Effect = BitConverter.ToUInt16(data, 0x10);
            Recoil = data[0x12];
            Healing = new Heal(data[0x13]);
            Targeting = data[0x14];
            Stat1 = data[0x15];
            Stat2 = data[0x16];
            Stat3 = data[0x17];
            Stat1Stage = data[0x18];
            Stat2Stage = data[0x19];
            Stat3Stage = data[0x1A];
            Stat1Percent = data[0x1B];
            Stat2Percent = data[0x1C];
            Stat3Percent = data[0x1D];
            _0x1E = data[0x1E];
            _0x1F = data[0x1F];
            _0x20 = data[0x20];
            _0x21 = data[0x21];
        }
        public byte[] Write()
        {
            using (MemoryStream ms = new MemoryStream())
            using (BinaryWriter bw = new BinaryWriter(ms))
            {
                bw.Write(Type);
                bw.Write(Quality);
                bw.Write(Category);
                bw.Write(Power);
                bw.Write(Accuracy);
                bw.Write(PP);
                bw.Write(Priority);
                bw.Write((byte)(HitMin | (HitMax << 4)));
                bw.Write(Inflict);
                bw.Write(InflictPercent);
                bw.Write(_0xB);
                bw.Write(TurnMin);
                bw.Write(TurnMax);
                bw.Write(CritStage);
                bw.Write(Flinch);
                bw.Write(Effect);
                bw.Write(Recoil);
                bw.Write(Healing.Write());
                bw.Write(Targeting);
                bw.Write(Stat1);
                bw.Write(Stat2);
                bw.Write(Stat3);
                bw.Write(Stat1Stage);
                bw.Write(Stat2Stage);
                bw.Write(Stat3Stage);
                bw.Write(Stat1Percent);
                bw.Write(Stat2Percent);
                bw.Write(Stat3Percent);
                bw.Write(_0x1E);
                bw.Write(_0x1F);
                bw.Write(_0x20);
                bw.Write(_0x21);
                return ms.ToArray();
            }
        }
        public class Heal
        {
            public byte Val;
            public bool Full, Half, Quarter, Value;
            public Heal(byte val)
            {
                Val = val;
                Full = Val == 0xFF;
                Half = Val == 0xFE;
                Quarter = Val == 0xFD;
                Value = (Val < 0xFD);
            }
            public byte Write()
            {
                if (Value)
                    return Val;
                if (Full)
                    return 0xFF;
                if (Half)
                    return 0xFE;
                if (Quarter)
                    return 0xFD;
                return Val;
            }
        }
    }
    public class Item
    {
        private ushort Price;
        public int BuyPrice;
        public int SellPrice;
        public byte HeldEffect, HeldArgument, NaturalGiftEffect, FlingEffect, FlingPower, NaturalGiftPower;
        public byte NaturalGiftType, u8Flags;
        public byte KeyFlags;
        public byte UseEffect;
        public byte _0xB; // Battle Type
        public byte _0xC; // 0 or 1
        public byte _0xD; // Classification (0-3 Battle, 4 Balls, 5 Mail)
        public byte Consumable;
        public byte SortIndex;
        public byte CureInflict; // Bitflags
        public byte FieldEffect; // Revive 1, Sacred Ash 3, Rare Candy 5, EvoStone 8
        public int BoostATK, 
            BoostDEF, BoostSPA, 
            BoostSPD, BoostSPE, 
            BoostACC, BoostCRIT, BoostPP;

        public ushort FieldBoost; // 0x15-0x16
        public byte EVHP, EVATK, EVDEF, EVSPE, EVSPA, EVSPD;
        public Heal Healing;
        public byte PPGain, Friendship1, Friendship2, Friendship3;

        public Item(byte[] data)
        {
            using (BinaryReader br = new BinaryReader(new MemoryStream(data)))
            {
                Price = br.ReadUInt16();
                BuyPrice = Price*10;
                SellPrice = Price*5;

                HeldEffect = br.ReadByte();
                HeldArgument = br.ReadByte();
                NaturalGiftEffect = br.ReadByte();
                FlingEffect = br.ReadByte();
                FlingPower = br.ReadByte();
                NaturalGiftPower = br.ReadByte();
                NaturalGiftType = br.ReadByte();
                u8Flags = (byte)(NaturalGiftType >> 5);
                NaturalGiftType &= 0x1F;
                KeyFlags = br.ReadByte();
                UseEffect = br.ReadByte();
                _0xB = br.ReadByte();
                _0xC = br.ReadByte();
                _0xD = br.ReadByte();
                Consumable = br.ReadByte();
                SortIndex = br.ReadByte();
                CureInflict = br.ReadByte();

                FieldEffect = br.ReadByte();
                BoostATK = FieldEffect >> 4;
                FieldEffect &= 0xF;
                BoostDEF = br.ReadByte();
                BoostSPA = BoostDEF >> 4;
                BoostDEF &= 0xF;
                BoostSPD = br.ReadByte();
                BoostSPE = BoostSPD >> 4;
                BoostSPD &= 0xF;
                BoostACC = br.ReadByte();
                BoostCRIT = (BoostACC >> 4) & 0x3;
                BoostPP = (BoostACC >> 6);
                BoostACC &= 0xF;

                EVHP = br.ReadByte();
                EVATK = br.ReadByte();
                EVDEF = br.ReadByte();
                EVSPE = br.ReadByte();
                EVSPA = br.ReadByte();
                EVSPD = br.ReadByte();

                Healing = new Heal(br.ReadByte());
                PPGain = br.ReadByte();
                Friendship1 = br.ReadByte();
                Friendship2 = br.ReadByte();
                Friendship3 = br.ReadByte();
            }
        }
        public byte[] Write()
        {
            using (MemoryStream ms = new MemoryStream())
            using (BinaryWriter bw = new BinaryWriter(ms))
            {
                bw.Write(Price);
                bw.Write(HeldEffect);
                bw.Write(HeldArgument);
                bw.Write(NaturalGiftEffect);
                bw.Write(FlingEffect);
                bw.Write(FlingPower);
                bw.Write(NaturalGiftPower);
                bw.Write((byte)(NaturalGiftType | (u8Flags << 5)));
                bw.Write(KeyFlags);
                bw.Write(UseEffect);
                bw.Write(_0xB);
                bw.Write(_0xC);
                bw.Write(_0xD);
                bw.Write(Consumable);
                bw.Write(SortIndex);
                bw.Write(CureInflict);

                bw.Write((byte)(FieldEffect | (BoostATK << 4)));
                bw.Write((byte)(BoostDEF | (BoostSPA << 4)));
                bw.Write((byte)(BoostSPD | (BoostSPE << 4)));
                bw.Write((byte)(BoostACC | (BoostCRIT << 4) | (BoostPP << 6)));

                bw.Write(EVHP);
                bw.Write(EVATK);
                bw.Write(EVDEF);
                bw.Write(EVSPE);
                bw.Write(EVSPA);
                bw.Write(EVSPD);
                bw.Write(Healing.Write());
                bw.Write(PPGain);
                bw.Write(Friendship1);
                bw.Write(Friendship2);
                bw.Write(Friendship3);

                return ms.ToArray();
            }
        }
        public class Heal
        {
            public byte Val;
            public bool Full, Half, Quarter, Value;
            public Heal(byte val)
            {
                Val = val;
                Full = Val == 0xFF;
                Half = Val == 0xFE;
                Quarter = Val == 0xFD;
                Value = (Val < 0xFD);
            }
            public byte Write()
            {
                if (Value)
                    return Val;
                if (Full)
                    return 0xFF;
                if (Half)
                    return 0xFE;
                if (Quarter)
                    return 0xFD;
                return Val;
            }
        }
    }
    public class Maison
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

            private byte _u1, _u2;

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
    public class Learnset
    {
        public int Count;
        public short[] Moves;
        public short[] Levels;
        public Learnset(byte[] data)
        {
            if (data.Length < 4 || data.Length % 4 != 0) return; // Detect invalid files, weakly.
            Count = (data.Length / 4) - 1;
            Moves = new short[Count];
            Levels = new short[Count];
            using (BinaryReader br = new BinaryReader(new MemoryStream(data)))
            {
                for (int i = 0; i < Count; i++)
                {
                    Moves[i] = br.ReadInt16();
                    Levels[i] = br.ReadInt16();
                }
            }
        }
        public byte[] Write()
        {
            using (MemoryStream ms = new MemoryStream())
            using (BinaryWriter bw = new BinaryWriter(ms))
            {
                for (int i = 0; i < Count; i++)
                {
                    bw.Write(Moves[i]);
                    bw.Write(Levels[i]);
                }
                bw.Write(-1);
                return ms.ToArray();
            }
        }
    }
    public class EggMoves
    {
        public ushort Count;
        public ushort[] Moves;

        public EggMoves(byte[] data)
        {
            if (data.Length < 2 || data.Length % 2 != 0) return; // Detect invalid files, weakly.
            using (BinaryReader br = new BinaryReader(new MemoryStream(data)))
            {
                Count = br.ReadUInt16();
                Moves = new ushort[Count];
                for (int i = 0; i < Count; i++)
                    Moves[i] = br.ReadUInt16();
            }
        }
        public byte[] Write()
        {
            Count = (ushort)Moves.Length;
            if (Count == 0) return new byte[0];
            using (MemoryStream ms = new MemoryStream())
            using (BinaryWriter bw = new BinaryWriter(ms))
            {
                bw.Write(Count);
                for (int i = 0; i < Count; i++)
                    bw.Write(Moves[i]);

                return ms.ToArray();
            }
        }
    }
    public class Evolutions
    {
        public ushort[] Method, Criteria, Species;
        public Evolutions(byte[] data)
        {
            if (data.Length < 0x30 || data.Length % 6 != 0) return;
            Method = new ushort[data.Length / 6];
            Criteria = new ushort[data.Length / 6];
            Species = new ushort[data.Length / 6];
            using (BinaryReader br = new BinaryReader(new MemoryStream(data)))
            for (int i = 0; i < Species.Length; i++)
            {
                Method[i] = br.ReadUInt16();
                Criteria[i] = br.ReadUInt16();
                Species[i] = br.ReadUInt16();
            }
        }
        public byte[] Write()
        {
            using (MemoryStream ms = new MemoryStream())
            using (BinaryWriter bw = new BinaryWriter(ms))
            {
                for (int i = 0; i < Species.Length; i++)
                {
                    bw.Write(Method[i]);
                    bw.Write(Criteria[i]);
                    bw.Write(Species[i]);
                }
                return ms.ToArray();
            }
        }
    }
    public class MegaEvolutions
    {
        public ushort[] Form, Method, Argument, u6;
        public MegaEvolutions(byte[] data)
        {
            if (data.Length < 0x10 || data.Length % 8 != 0) return;
            Form = new ushort[data.Length / 8];
            Method = new ushort[data.Length / 8];
            Argument = new ushort[data.Length / 8];
            u6 = new ushort[data.Length / 8];
            using (BinaryReader br = new BinaryReader(new MemoryStream(data)))
            for (int i = 0; i < Form.Length; i++)
            {
                Form[i] = br.ReadUInt16();
                Method[i] = br.ReadUInt16();
                Argument[i] = br.ReadUInt16();
                u6[i] = br.ReadUInt16();
            }
        }
        public byte[] Write()
        {
            using (MemoryStream ms = new MemoryStream())
            using (BinaryWriter bw = new BinaryWriter(ms))
            {
                for (int i = 0; i < Form.Length; i++)
                {
                    if (Method[i] == 0)
                    { Form[i] = Argument[i] = 0; } // No method to evolve, clear information.
                    bw.Write(Form[i]);
                    bw.Write(Method[i]);
                    bw.Write(Argument[i]);
                    bw.Write(u6[i]);
                }
                return ms.ToArray();
            }
        }
    }
    public class MapMatrix
    {
        public uint u0;
        public ushort uL;
        public ushort Width, Height;
        private int Area;
        public ushort[] EntryList;
        public Entry[] Entries;
        public MapMatrix(byte[] data)
        {
            using (BinaryReader br = new BinaryReader(new MemoryStream(data)))
            {
                u0 = br.ReadUInt32();
                Width = br.ReadUInt16();
                Height = br.ReadUInt16();
                Area = Width*Height;
                Entries = new Entry[Area];
                EntryList = new ushort[Area];
                for (int i = 0; i < Area; i++)
                    EntryList[i] = br.ReadUInt16();

                if (br.BaseStream.Position != br.BaseStream.Length)
                    uL = br.ReadUInt16();
            }
        }
        public byte[] Write()
        {
            using (MemoryStream ms = new MemoryStream())
            using (BinaryWriter bw = new BinaryWriter(ms))
            {
                bw.Write(u0);
                bw.Write(Width);
                bw.Write(Height);
                foreach (ushort Entry in EntryList) bw.Write(Entry);
                bw.Write(uL);
                return ms.ToArray();
            }
        }

        public Image Preview(int Scale, int ColorShift)
        {
            // Require the entries to be defined in order to continue.
            if (Entries.Any(entry => entry == null))
            {
                // Do nothing; images are instead created with the standard dimensions and returned.
            }

            // Fetch Singular Images first
            Image[] EntryImages = new Image[Area];
            for (int i = 0; i < Area; i++)
                EntryImages[i] = (Entries[i] == null)
                    ? new Bitmap(40 * Scale, 40 * Scale)
                    : Entries[i].Preview(Scale, ColorShift);

            // Combine all images into one.
            Bitmap img = new Bitmap(EntryImages[0].Width * Width, EntryImages[0].Height * Height);

            using (Graphics g = Graphics.FromImage(img))
            for (int i = 0; i < Area; i++)
            {
                g.DrawImage(EntryImages[i], new Point((i * EntryImages[0].Width) % (img.Width), EntryImages[0].Height * ((i / Width))));
            }
            return img;
        }

        public class Entry
        {
            public ushort Width, Height;
            private int Area;
            public uint[] Tiles; // Certain bits?
            public Entry(byte[] data)
            {
                using (BinaryReader br = new BinaryReader(new MemoryStream(data)))
                {
                    Width = br.ReadUInt16();
                    Height = br.ReadUInt16();
                    Area = Width*Height;
                    Tiles = new uint[Area];
                    for (int i = 0; i < Area; i++)
                        Tiles[i] = br.ReadUInt32();
                }
            }
            public byte[] Write()
            {
                using (MemoryStream ms = new MemoryStream())
                using (BinaryWriter bw = new BinaryWriter(ms))
                {
                    bw.Write(Width);
                    bw.Write(Height);
                    foreach (uint Tile in Tiles) bw.Write(Tile);
                    return ms.ToArray();
                }
            }
            public Image Preview(int s, int ColorShift)
            {
                Bitmap img = new Bitmap(Width*s, Height*s);
                for (int i = 0; i < Area; i++)
                {
                    Color c = Tiles[i] == 0x01000021
                        ? Color.Black
                        : Color.FromArgb((int) (RNG.Forward32(Tiles[i], ColorShift) | 0xFF000000));
                    try
                    {
                        for (int x = 0; x < s; x++)
                            for (int y = 0; y < s; y++)
                            {
                                img.SetPixel(
                                    x + (i * s) % (img.Width),
                                    y + ((i/Width)*s),
                                    c);
                            }
                    }
                    catch { }
                }
                return img;
            }
        }
    }
    public class ZoneData
    {
        public ushort u00, u02, MapMatrix, TextFile, u08; // Changing
        public ushort u0A, u0C, u0E, u10, u12, u14, u16; // Constant

        public ushort u18, u1A, OverworldLocation, u1E;
        public byte OLFlags; // Part of OverworldLocation
        public byte u20, u21; // uses 3 bits, uses 7 bits
        public ushort u22, u24; // Zero
        public short u26; // -1 thru ~25

        public ushort u28, u2A; // 2 bytes float?
        public short u2C, u2E, u30, u32, u34, u36;

        public ZoneData(byte[] data)
        {
            if (data.Length != 0x36) 
                return;
            using (BinaryReader br = new BinaryReader(new MemoryStream(data)))
            {
                u00 = br.ReadUInt16();
                u02 = br.ReadUInt16();
                MapMatrix = br.ReadUInt16();
                TextFile = br.ReadUInt16();
                u08 = br.ReadUInt16();

                u0A = br.ReadUInt16();
                u0C = br.ReadUInt16();
                u0E = br.ReadUInt16();
                u10 = br.ReadUInt16();
                u12 = br.ReadUInt16();
                u14 = br.ReadUInt16();

                u18 = br.ReadUInt16();
                u1A = br.ReadUInt16();
                OverworldLocation = br.ReadUInt16();
                OLFlags = (byte)(OverworldLocation >> 9);
                OverworldLocation &= 0x1FF;

                u20 = br.ReadByte();
                u21 = br.ReadByte();

                u22 = br.ReadUInt16();
                u24 = br.ReadUInt16();
                u26 = br.ReadInt16();
                u28 = br.ReadUInt16();
                u2A = br.ReadUInt16();

                u2C = br.ReadInt16();
                u2E = br.ReadInt16();
                u30 = br.ReadInt16();
                u32 = br.ReadInt16();
                u34 = br.ReadInt16();
                u36 = br.ReadInt16();
            }
        }
        public byte[] Write()
        {
            using (MemoryStream ms = new MemoryStream())
            using (BinaryWriter bw = new BinaryWriter(ms))
            {
                bw.Write(u00); bw.Write(u02); bw.Write(MapMatrix); bw.Write(TextFile);
                bw.Write(u08); bw.Write(u0A); bw.Write(u0C); bw.Write(u0E);
                bw.Write(u10); bw.Write(u12); bw.Write(u14); bw.Write(u16);

                bw.Write(u18); bw.Write(u1A);
                bw.Write((ushort)(OverworldLocation << (OLFlags << 9)));
                bw.Write(u1E); bw.Write(u20); bw.Write(u21);
                bw.Write(u22); bw.Write(u22); bw.Write(u24);
                bw.Write(u26); bw.Write(u28); bw.Write(u2A);

                bw.Write(u2C); bw.Write(u2E); bw.Write(u30);
                bw.Write(u32); bw.Write(u34); bw.Write(u36);
                return ms.ToArray();
            }
        }
    }
    public class Zone
    {
        public ZoneInfo Info;
        public ZoneEntities Entities;
        public ZoneScript MapScript;
        public ZoneEncounters Encounters;
        public ZoneUnknown File5;

        public Zone(byte[][] Zone)
        {
            // A ZO is comprised of 4-5 files.

            // Array 0 is [Map Info]
            Info = new ZoneInfo(Zone[0]);
            // Array 1 is [Overworld Entities & their Scripts]
            Entities = new ZoneEntities(Zone[1]);
            // Array 2 is [Map Script]
            MapScript = new ZoneScript(Zone[2]);
            // Array 3 is [Wild Encounters]
            Encounters = new ZoneEncounters(Zone[3]);
            // Array 4 is [???] - May not be present in all.
            if (Zone.Length <= 4) 
                return;
            File5 = new ZoneUnknown(Zone[4]);
        }

        public byte[][] Write()
        {
            byte[][] Zone = new byte[(File5 != null) ? 5 : 4][];
            Zone[0] = Info.Write();
            Zone[1] = Entities.Write();
            Zone[2] = MapScript.Write();
            Zone[3] = Encounters.Write();
            if (Zone.Length <= 4) 
                return Zone;

            Zone[4] = File5.Write();
            return Zone;
        }

        public class ZoneInfo
        {
            public byte[] InfoData; // File details unknown.
            public ZoneInfo(byte[] data)
            {
                InfoData = data;
            }
            public byte[] Write()
            {
                return InfoData;
            }
        }
        public class ZoneEntities
        {
            public byte[] Data; // File details unknown.

            public EntityFurniture[] Furniture;
            public EntityNPC[] NPCs;
            public EntityWarp[] Warps;
            public EntityTrigger[] Triggers;
            public ZoneEntities(byte[] data)
            {
                Data = data;
            }
            public byte[] Write()
            {
                return Data;
            }

            // Entity Classes
            public class EntityFurniture
            {
                // 0x14 Bytes Long
                public byte[] Raw;
                public EntityFurniture(byte[] data)
                {
                    Raw = data;
                }
                public byte[] Write()
                {
                    return Raw;
                }
            }
            public class EntityNPC
            {
                // 0x30 Bytes Long
                public byte[] Raw;
                public EntityNPC(byte[] data)
                {
                    Raw = data;
                }
                public byte[] Write()
                {
                    return Raw;
                }
            }
            public class EntityWarp
            {
                // 0x18 Bytes Long
                public byte[] Raw;
                public EntityWarp(byte[] data)
                {
                    Raw = data;
                }
                public byte[] Write()
                {
                    return Raw;
                }
            }
            public class EntityTrigger
            {
                // 0x18 Bytes Long
                public byte[] Raw;
                public EntityTrigger(byte[] data)
                {
                    Raw = data;
                }
                public byte[] Write()
                {
                    return Raw;
                }
            }
        }
        public class ZoneScript
        {
            public byte[] Data; // File details unknown.
            public ZoneScript(byte[] data)
            {
                Data = data;
            }
            public byte[] Write()
            {
                return Data;
            }
        }
        public class ZoneEncounters
        {
            public byte[] Data; // File details unknown.
            public ZoneEncounters(byte[] data)
            {
                Data = data;
            }
            public byte[] Write()
            {
                return Data;
            }
        }
        public class ZoneUnknown
        {
            public byte[] FileData; // File details unknown.
            public ZoneUnknown(byte[] data)
            {
                FileData = data;
            }

            public byte[] Write()
            {
                return FileData;
            }
        }
    }
}
