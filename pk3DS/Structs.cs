using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
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

    #region Pokémon Related Classes
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
                EV_HP = (EVs >> 0) & 0x3;
                EV_ATK = (EVs >> 2) & 0x3;
                EV_DEF = (EVs >> 4) & 0x3;
                EV_SPE = (EVs >> 6) & 0x3;
                EV_SPA = (EVs >> 8) & 0x3;
                EV_SPD = (EVs >> 10) & 0x3;

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
                    TMHM[j] = ((TMHMData[j / 8] >> (j % 8)) & 0x1) == 1; //Bitflags for TMHM

                byte[] TutorData = br.ReadBytes(8);
                Tutors = new bool[8 * TutorData.Length];
                for (int j = 0; j < Tutors.Length; j++)
                    Tutors[j] = ((TutorData[j / 8] >> (j % 8)) & 0x1) == 1; //Bitflags for Tutors

                if (br.BaseStream.Length - br.BaseStream.Position == 0x10) // ORAS
                {
                    byte[][] ORASTutorData =
                        {
                            br.ReadBytes(4), // 15
                            br.ReadBytes(4), // 17
                            br.ReadBytes(4), // 16
                            br.ReadBytes(4), // 15
                        };
                    for (int i = 0; i < 4; i++)
                    {
                        ORASTutors[i] = new bool[8 * ORASTutorData[i].Length];
                        for (int b = 0; b < 8 * ORASTutorData[i].Length; b++)
                            ORASTutors[i][b] = ((ORASTutorData[i][b / 8] >> (b % 8)) & 0x1) == 1;
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
                bw.Write(SPA);
                bw.Write(SPD);
                foreach (byte Type in Types) bw.Write(Type);
                bw.Write(CatchRate);
                bw.Write(EvoStage);
                EVs = (ushort)(EVs & 0x80 | ((EV_HP & 3) << 0) | ((EV_ATK & 3) << 2) | ((EV_DEF & 3) << 4) | ((EV_SPE & 3) << 6) | ((EV_SPA & 3) << 8) | ((EV_SPD & 3) << 10));
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
                    TMHMData[i / 8] |= (byte)(TMHM[i] ? 1 << (i % 8) : 0);
                bw.Write(TMHMData);

                byte[] TutorData = new byte[8];
                for (int i = 0; i < Tutors.Length; i++)
                    TutorData[i / 8] |= (byte)(Tutors[i] ? 1 << (i % 8) : 0);
                bw.Write(TutorData);

                while (bw.BaseStream.Length != 0x40) bw.Write((byte)0);

                if (ORASTutors[0] != null) // ORAS Data
                {
                    byte[][] ORASTutorData =
                        {
                            new byte[4], // 15
                            new byte[4], // 17
                            new byte[4], // 16
                            new byte[4], // 15
                        };
                    for (int i = 0; i < 4; i++)
                        for (int b = 0; b < ORASTutors[i].Length; b++)
                            ORASTutorData[i][b / 8] |= (byte)(ORASTutors[i][b] ? 1 << b % 8 : 0);

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
                Value = Val < 0xFD;
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
        public ushort Price;
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
        public ushort FunctionFlags;

        public ushort FieldBoost; // 0x15-0x16
        public byte EVHP, EVATK, EVDEF, EVSPE, EVSPA, EVSPD;
        public Heal Healing;
        public byte PPGain, Friendship1, Friendship2, Friendship3;
        public byte _0x23, _0x24;

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
                BoostPP = BoostACC >> 6;
                BoostACC &= 0xF;

                FunctionFlags = br.ReadUInt16();

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
                _0x23 = br.ReadByte();
                _0x24 = br.ReadByte();
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

                bw.Write(FunctionFlags);

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

                bw.Write(_0x23);
                bw.Write(_0x24);

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
                Value = Val < 0xFD;
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
    public class Learnset
    {
        public int Count;
        public short[] Moves;
        public short[] Levels;
        public Learnset(byte[] data)
        {
            if (data.Length < 4 || data.Length % 4 != 0) return; // Detect invalid files, weakly.
            Count = data.Length / 4 - 1;
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
            Count = (ushort)Moves.Length;
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
    public class Gift
    {
        // All
        public byte[] Data;
        public bool ORAS;

        public ushort Species;
        public ushort u2;
        public byte Form;
        public byte Level;
        public sbyte Ability;
        public sbyte Nature;
        public byte Shiny, u9, uA, uB;
        public int HeldItem;
        public sbyte Gender;
        // ORAS
        public byte u11;
        public short MetLocation;
        public ushort Move;
        // All
        public sbyte[] IVs = new sbyte[6];
        // ORAS
        public byte[] ContestStats;
        public byte u22;
        // All
        public byte uLast;

        public Gift(byte[] data, bool oras)
        {
            Data = data;
            ORAS = oras;
            using (BinaryReader br = new BinaryReader(new MemoryStream(Data)))
            {
                Species = br.ReadUInt16();
                u2 = br.ReadUInt16();
                Form = br.ReadByte();
                Level = br.ReadByte();
                Shiny = br.ReadByte();
                Ability = br.ReadSByte();
                Nature = br.ReadSByte();
                u9 = br.ReadByte();
                uA = br.ReadByte();
                uB = br.ReadByte();
                HeldItem = br.ReadInt32();
                Gender = br.ReadSByte();

                if (ORAS)
                {
                    u11 = br.ReadByte();
                    MetLocation = br.ReadInt16();
                    Move = br.ReadUInt16();
                }

                for (int i = 0; i < 6; i++)
                    IVs[i] = br.ReadSByte();

                if (ORAS)
                {
                    ContestStats = br.ReadBytes(6);
                    u22 = br.ReadByte();
                }

                uLast = br.ReadByte();
            }
        }
        public byte[] Write()
        {
            using (MemoryStream ms = new MemoryStream())
            using (BinaryWriter bw = new BinaryWriter(ms))
            {
                bw.Write(Species);
                bw.Write(u2);
                bw.Write(Form);
                bw.Write(Level);
                bw.Write(Shiny);
                bw.Write(Ability);
                bw.Write(Nature);
                bw.Write(u9);
                bw.Write(uA);
                bw.Write(uB);
                bw.Write(HeldItem);
                bw.Write(Gender);

                if (ORAS)
                {
                    bw.Write(u11);
                    bw.Write(MetLocation);
                    bw.Write(Move);
                }

                for (int i = 0; i < 6; i++)
                    bw.Write(IVs[i]);

                if (ORAS)
                {
                    bw.Write(ContestStats);
                    bw.Write(u22);
                }

                bw.Write(uLast);

                return ms.ToArray();
            }
        }
    }
    public class EncounterStatic
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

        public EncounterStatic(byte[] data)
        {
            Data = (byte[])data.Clone();
        }
        public byte[] Write()
        {
            return (byte[])Data.Clone();
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
    #endregion

    #region Game Related Classes
    public class MapMatrix
    {
        public uint u0;
        public ushort uL;
        public ushort Width, Height;
        private readonly int Area;
        public ushort[] EntryList;
        public Entry[] Entries;
        public Unknown[] Unknowns;

        public byte[] UnkData;
        public MapMatrix(byte[][] data)
        {
            using (BinaryReader br = new BinaryReader(new MemoryStream(data[0])))
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
            if (data.Length > 1)
                parseUnk(UnkData = data[1]);
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

        public Bitmap Preview(int Scale, int ColorShift)
        {
            // Require the entries to be defined in order to continue.
            if (Entries.Any(entry => entry == null))
            {
                // Do nothing; images are instead created with the standard dimensions and returned.
            }

            // Fetch Singular Images first
            Bitmap[] EntryImages = new Bitmap[Area];
            for (int i = 0; i < Area; i++)
                EntryImages[i] = Entries[i] == null
                    ? new Bitmap(40 * Scale, 40 * Scale)
                    : Entries[i].Preview(Scale, ColorShift);

            // Combine all images into one.
            Bitmap img = new Bitmap(EntryImages[0].Width * Width, EntryImages[0].Height * Height);

            using (Graphics g = Graphics.FromImage(img))
            for (int i = 0; i < Area; i++)
            {
                g.DrawImage(EntryImages[i], new Point(i * EntryImages[0].Width % img.Width, EntryImages[0].Height * (i / Width)));
            }
            return img;
        }

        public class Entry
        {
            public Collision coll;

            public ushort Width, Height;
            private readonly int Area;
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
            
            public Bitmap Preview(int s, int ColorShift)
            {
                byte[] bmpData = BytePreview(s, ColorShift);
                Bitmap b = new Bitmap(Width * s, Height * s, PixelFormat.Format32bppArgb);
                BitmapData bData = b.LockBits(new Rectangle(0, 0, Width * s, Height * s), ImageLockMode.WriteOnly, PixelFormat.Format32bppArgb);
                System.Runtime.InteropServices.Marshal.Copy(bmpData, 0, bData.Scan0, bmpData.Length);
                b.UnlockBits(bData);

                return b;
            }
            public byte[] BytePreview(int s, int ColorShift)
            {
                byte[] bmpData = new byte[4 * Width * Height * s * s];
                for (int i = 0; i < Area; i++)
                {
                    int X = i % 40;
                    int Y = i / 40;
                    uint colorValue = Tiles[i] == 0x01000021
                        ? 0xFF000000
                        : RNG.Forward32(Tiles[i], ColorShift) | 0xFF000000;

                    byte[] pixel = BitConverter.GetBytes(colorValue);
                    for (int x = 0; x < s * s; x++)
                        pixel.CopyTo(bmpData, 4 * ((Y * s + x / s) * Width * s + X * s + x % s));
                }
                return bmpData;
            }
        }
        public class Collision
        {
            public string Magic;
            public int termOffset;
            public int U5D8;
            public byte[] UnknownBytes;
            public CollisionObject[] Map40;
            public int[] MapInts;
            public CollisionObject[] MapMisc;
            public string termMagic;
            public byte[] termData;
            public Collision(byte[] data)
            {
                using (BinaryReader br = new BinaryReader(new MemoryStream(data)))
                {
                    Magic = new string(br.ReadChars(4)); // Magic
                    if (Magic != "coll")
                        return; // all other properties are null

                    termOffset = br.ReadInt32();
                    U5D8 = br.ReadInt32();
                    UnknownBytes = br.ReadBytes(0x14);

                    // Read 40 collision rectangles
                    Map40 = new CollisionObject[40];
                    for (int i = 0; i < Map40.Length; i++)
                        Map40[i] = new CollisionObject(br.ReadBytes(0x10));

                    // Read 32 Int32s
                    MapInts = new int[0x20];
                    for (int i = 0; i < MapInts.Length; i++)
                        MapInts[i] = br.ReadInt32();

                    // Read misc collision rectangles
                    int ct = termOffset - (int)br.BaseStream.Position + 0x10;
                    MapMisc = new CollisionObject[ct/0x10];
                    for (int i = 0; i < MapMisc.Length; i++)
                        MapMisc[i] = new CollisionObject(br.ReadBytes(0x10));

                    // Read Term
                    termMagic = new string(br.ReadChars(4));
                    if (termMagic != "term")
                        return; // all other properties are null

                    // Read the rest of the data....
                    termData = br.ReadBytes((int)(br.BaseStream.Length - br.BaseStream.Position));
                }
            }
            public class CollisionObject
            {
                private readonly float _0;
                private readonly float _1;
                private readonly float _2;
                private readonly float _3; // rarely used

                // I don't even know...
                public float F1 => _0 / 2;
                public float F2 => _1 * 80;
                public float F3 => _2 / 2;
                public float F4 => _3;

                public CollisionObject(byte[] data)
                {
                    _0 = BitConverter.ToSingle(data, 0x0);
                    _1 = BitConverter.ToSingle(data, 0x4);
                    _2 = BitConverter.ToSingle(data, 0x8);
                    _3 = BitConverter.ToSingle(data, 0xC);
                }
                public override string ToString()
                {
                    return string.Join(", ", F1.ToString(), F2.ToString(), F3.ToString(), F4.ToString());
                }
            }
        }

        public string Unk2String()
        {
            return Unknowns.Aggregate("", (current, l) => current + $"{l.Direction}: {l.p1,3} {l.p2,3} {l.p3,3} {l.p4,3}{Environment.NewLine,3}");
        }
        private void parseUnk(byte[] data)
        {
            List<Unknown> unk = new List<Unknown>();
            using (var br = new BinaryReader(new MemoryStream(data)))
            do
            {
                unk.Add(new Unknown {
                    Direction = br.ReadUInt32(),
                    _1 = br.ReadSingle(),
                    _2 = br.ReadSingle(),
                    _3 = br.ReadSingle(),
                    _4 = br.ReadSingle(),
                });
            } while (unk.Last().Direction != 0);
            unk.RemoveAt(unk.Count-1);
            Unknowns = unk.ToArray();
        }

        public class Unknown
        {
            public uint Direction;
            public float _1;
            public float _2;
            public float _3;
            public float _4;

            public int p1 => (int)_1 / 18;
            public int p2 => (int)_2 / 18;
            public int p3 => (int)_3 / 18;
            public int p4 => (int)_4 / 18;
        }
    }
    public class ZoneData
    {
        internal static int Size = 0x38;
        public byte[] Data;
        public int MapMatrix { get { return BitConverter.ToUInt16(Data, 0x04); } set { BitConverter.GetBytes((ushort)value).CopyTo(Data, 0x04);} }
        public int TextFile { get { return BitConverter.ToUInt16(Data, 0x06); } set { BitConverter.GetBytes((ushort)value).CopyTo(Data, 0x06); } }
        public int ParentMap // 0x1C - low 7 bits
        {
            get { return BitConverter.ToUInt16(Data, 0x1C) & 0x1FF; }
            set { BitConverter.GetBytes((ushort)(value | (BitConverter.ToUInt16(Data, 0x1C) & ~0x1FF))).CopyTo(Data, 0x1C); }
        }
        public int OLFlags // 0x1C - high 9(?) bits
        {
            get { return BitConverter.ToUInt16(Data, 0x1C) >> 9; }
            set { BitConverter.GetBytes((ushort)((value << 9) | (BitConverter.ToUInt16(Data, 0x1C) & 0x1FF))).CopyTo(Data, 0x1C); }
        }

        private int X { get { return BitConverter.ToUInt16(Data, 0x2C); } set { BitConverter.GetBytes((ushort)value).CopyTo(Data, 0x2C); } }
        public int Z { get { return BitConverter.ToInt16(Data, 0x2E); } set { BitConverter.GetBytes((short)value).CopyTo(Data, 0x2E); } }
        private int Y { get { return BitConverter.ToUInt16(Data, 0x30); } set { BitConverter.GetBytes((ushort)value).CopyTo(Data, 0x30); } }
        private int X2 { get { return BitConverter.ToUInt16(Data, 0x32); } set { BitConverter.GetBytes((ushort)value).CopyTo(Data, 0x32); } }
        public int Z2 { get { return BitConverter.ToInt16(Data, 0x34); } set { BitConverter.GetBytes((short)value).CopyTo(Data, 0x34); } }
        private int Y2 { get { return BitConverter.ToUInt16(Data, 0x36); } set { BitConverter.GetBytes((ushort)value).CopyTo(Data, 0x36); } }

        public float pX { get { return (float)X / 18; } set { X = (int)(18 * value); } }
        public float pY { get { return (float)Y / 18; } set { Y = (int)(18 * value); } }
        public float pX2 { get { return (float)X2 / 18; } set { X2 = (int)(18 * value); } }
        public float pY2 { get { return (float)Y2 / 18; } set { Y2 = (int)(18 * value); } }

        public ZoneData(byte[] data)
        {
            if (data.Length != Size) 
                return;
            Data = data;
        }

        public byte[] Write()
        {
            return Data;
        }
    }
    public class Zone
    {
        public ZoneData ZD;
        public ZoneEntities Entities;
        public ZoneScript MapScript;
        public ZoneEncounters Encounters;
        public ZoneUnknown File5;

        public Zone(byte[][] Zone)
        {
            // A ZO is comprised of 4-5 files.

            // Array 0 is [Map Info]
            ZD = new ZoneData(Zone[0]);
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
            byte[][] Zone = new byte[File5 != null ? 5 : 4][];
            Zone[0] = ZD.Data;
            Zone[1] = Entities.Write();
            Zone[2] = MapScript.Write();
            Zone[3] = Encounters.Write();
            if (Zone.Length <= 4) 
                return Zone;

            Zone[4] = File5.Write();
            return Zone;
        }

        public class ZoneEntities
        {
            public byte[] Data;

            public int Length;
            public int FurnitureCount, NPCCount, WarpCount, TriggerCount, UnknownCount;
            public EntityFurniture[] Furniture;
            public EntityNPC[] NPCs;
            public EntityWarp[] Warps;
            public EntityTrigger1[] Triggers1;
            public EntityTrigger2[] Triggers2;

            public Script Script;

            public ZoneEntities(byte[] data)
            {
                Data = data;

                using (BinaryReader br = new BinaryReader(new MemoryStream(data)))
                {
                    // Load Header
                    Length = br.ReadInt32();
                    Furniture = new EntityFurniture[FurnitureCount = br.ReadByte()];
                    NPCs = new EntityNPC[NPCCount = br.ReadByte()];
                    Warps = new EntityWarp[WarpCount = br.ReadByte()];
                    Triggers1 = new EntityTrigger1[TriggerCount = br.ReadByte()];
                    Triggers2 = new EntityTrigger2[UnknownCount = br.ReadInt32()]; // not sure if there's other types or if the remaining 3 bytes are padding.

                    // Load Entitites
                    for (int i = 0; i < FurnitureCount; i++)
                        Furniture[i] = new EntityFurniture(br.ReadBytes(EntityFurniture.Size));
                    for (int i = 0; i < NPCCount; i++)
                        NPCs[i] = new EntityNPC(br.ReadBytes(EntityNPC.Size));
                    for (int i = 0; i < WarpCount; i++)
                        Warps[i] = new EntityWarp(br.ReadBytes(EntityWarp.Size));
                    for (int i = 0; i < TriggerCount; i++)
                        Triggers1[i] = new EntityTrigger1(br.ReadBytes(EntityTrigger1.Size));
                    for (int i = 0; i < UnknownCount; i++)
                        Triggers2[i] = new EntityTrigger2(br.ReadBytes(EntityTrigger2.Size));

                    // Load Script Data
                    int len = br.ReadInt32();
                    br.BaseStream.Position -= 4;
                    Script = new Script(br.ReadBytes(len));
                }
            }
            public byte[] Write()
            {
                byte[] F = new byte[Furniture.Length * EntityFurniture.Size];
                for (int i = 0; i < Furniture.Length; i++)
                    Furniture[i].Write().CopyTo(F, i * EntityFurniture.Size);

                byte[] N = new byte[NPCs.Length * EntityNPC.Size];
                for (int i = 0; i < NPCs.Length; i++)
                    NPCs[i].Write().CopyTo(N, i * EntityNPC.Size);

                byte[] W = new byte[Warps.Length * EntityWarp.Size];
                for (int i = 0; i < Warps.Length; i++)
                    Warps[i].Write().CopyTo(W, i * EntityWarp.Size);

                byte[] T = new byte[Triggers1.Length * EntityTrigger1.Size];
                for (int i = 0; i < Triggers1.Length; i++)
                    Triggers1[i].Write().CopyTo(T, i * EntityTrigger1.Size);

                byte[] U = new byte[Triggers2.Length * EntityTrigger2.Size];
                for (int i = 0; i < Triggers2.Length; i++)
                    Triggers2[i].Write().CopyTo(U, i * EntityTrigger2.Size);

                // Assemble entity information
                byte[] OWEntities = F.Concat(N).Concat(W).Concat(T).Concat(U).ToArray();
                byte[] EntityLength = BitConverter.GetBytes(8 + OWEntities.Length);
                byte[] EntityCounts = {(byte)Furniture.Length, (byte)NPCs.Length, (byte)Warps.Length, (byte)Triggers1.Length, (byte)Triggers2.Length, 0, 0, 0 };
                
                // Reassemble NPC portion
                byte[] OWEntityData = EntityLength.Concat(EntityCounts).Concat(OWEntities).ToArray();

                // Reassemble Script portion
                byte[] OWScriptData = Script.Write();
                
                byte[] finalData = OWEntityData.Concat(OWScriptData).ToArray();

                // Add padding zeroes if required (yield size % 4 == 0)
                if (finalData.Length % 4 != 0)
                    Array.Resize(ref finalData, finalData.Length + 4 - finalData.Length % 4);

                return finalData;
            }

            // Entity Classes
            public class EntityFurniture
            {
                // Usable Attributes
                public int Script { get { return BitConverter.ToUInt16(Raw, 0x00); } set { BitConverter.GetBytes((ushort)value).CopyTo(Raw, 0x00); } }

                public int U2 { get { return BitConverter.ToUInt16(Raw, 0x02); } set { BitConverter.GetBytes((ushort)value).CopyTo(Raw, 0x02); } }
                public int U4 { get { return BitConverter.ToUInt16(Raw, 0x04); } set { BitConverter.GetBytes((ushort)value).CopyTo(Raw, 0x04); } }
                public int U6 { get { return BitConverter.ToUInt16(Raw, 0x06); } set { BitConverter.GetBytes((ushort)value).CopyTo(Raw, 0x06); } }

                // Coordinates have some upper-bit usage it seems...
                public int X { get { return BitConverter.ToUInt16(Raw, 0x08); } set { BitConverter.GetBytes((ushort)value).CopyTo(Raw, 0x08); } }
                public int Y { get { return BitConverter.ToUInt16(Raw, 0x0A); } set { BitConverter.GetBytes((ushort)value).CopyTo(Raw, 0x0A); } }
                // Next two bytes should be dealing with furniture width?
                public int WX { get { return BitConverter.ToInt16(Raw, 0x0C); } set { BitConverter.GetBytes((short)value).CopyTo(Raw, 0x0C); } }
                public int WY { get { return BitConverter.ToInt16(Raw, 0x0E); } set { BitConverter.GetBytes((short)value).CopyTo(Raw, 0x0E); } }

                public int U10 { get { return BitConverter.ToInt32(Raw, 0x10); } set { BitConverter.GetBytes(value).CopyTo(Raw, 0x10); } }

                public byte[] Raw;
                public byte[] OriginalData;
                internal static readonly byte Size = 0x14;
                public EntityFurniture(byte[] data = null)
                {
                    Raw = data ?? new byte[Size];
                    OriginalData = (byte[])Raw.Clone();
                }
                public byte[] Write()
                {
                    return Raw;
                }
            }
            public class EntityNPC
            {
                // Usable Attributes
                public int ID { get { return BitConverter.ToUInt16(Raw, 0x00); } set { BitConverter.GetBytes((ushort)value).CopyTo(Raw, 0x00); } }
                public int Model { get { return BitConverter.ToUInt16(Raw, 0x02); } set { BitConverter.GetBytes((ushort)value).CopyTo(Raw, 0x02); } }
                public int MovePermissions { get { return BitConverter.ToUInt16(Raw, 0x04); } set { BitConverter.GetBytes((ushort)value).CopyTo(Raw, 0x04); } }
                public int MovePermissions2 { get { return BitConverter.ToUInt16(Raw, 0x06); } set { BitConverter.GetBytes((ushort)value).CopyTo(Raw, 0x06); } }
                public int SpawnFlag { get { return BitConverter.ToUInt16(Raw, 0x08); } set { BitConverter.GetBytes((ushort)value).CopyTo(Raw, 0x08); } }
                public int Script { get { return BitConverter.ToUInt16(Raw, 0x0A); } set { BitConverter.GetBytes((ushort)value).CopyTo(Raw, 0x0A); } }
                public int FaceDirection { get { return BitConverter.ToUInt16(Raw, 0x0C); } set { BitConverter.GetBytes((ushort)value).CopyTo(Raw, 0x0C); } }
                public int SightRange { get { return BitConverter.ToUInt16(Raw, 0x0E); } set { BitConverter.GetBytes((ushort)value).CopyTo(Raw, 0x0E); } }

                // XY Only
                public int U10 { get { return BitConverter.ToUInt16(Raw, 0x10); } set { BitConverter.GetBytes((ushort)value).CopyTo(Raw, 0x10); } }
                public int U12 { get { return BitConverter.ToUInt16(Raw, 0x12); } set { BitConverter.GetBytes((ushort)value).CopyTo(Raw, 0x12); } }

                // Shorts
                public int U14 { get { return BitConverter.ToInt16(Raw, 0x14); } set { BitConverter.GetBytes((short)value).CopyTo(Raw, 0x14); } }
                public int U16 { get { return BitConverter.ToInt16(Raw, 0x16); } set { BitConverter.GetBytes((short)value).CopyTo(Raw, 0x16); } }
                // Negative only in X/Y... seeing behind them? Might be projection of an interaction area.
                public int U18 { get { return BitConverter.ToInt16(Raw, 0x18); } set { BitConverter.GetBytes((short)value).CopyTo(Raw, 0x18); } }
                public int U1A { get { return BitConverter.ToInt16(Raw, 0x1A); } set { BitConverter.GetBytes((short)value).CopyTo(Raw, 0x1A); } }

                // WalkArea Leashes (?): If these are for NPCs that walk in an area, I'm not sure if there's a direction specified.
                // Set L# to -1 to turn off.
                public int L1 { get { return BitConverter.ToInt16(Raw, 0x1C); } set { BitConverter.GetBytes((short)value).CopyTo(Raw, 0x1C); } }
                public int L2 { get { return BitConverter.ToInt16(Raw, 0x1E); } set { BitConverter.GetBytes((short)value).CopyTo(Raw, 0x1E); } }
                public int L3 { get { return BitConverter.ToInt16(Raw, 0x20); } set { BitConverter.GetBytes((short)value).CopyTo(Raw, 0x20); } }
                // Leash Direction? Only used when an area is specified.
                public int LDir { get { return BitConverter.ToUInt16(Raw, 0x22); } set { BitConverter.GetBytes((ushort)value).CopyTo(Raw, 0x22); } }

                // 0x24-0x25 is Unused in OR/AS, rarely 1 in XY
                public int U24 { get { return BitConverter.ToUInt16(Raw, 0x24); } set { BitConverter.GetBytes((ushort)value).CopyTo(Raw, 0x24); } }
                // 0x26-0x27 is Unused in OR/AS

                // Highest bits for X/Y seem to be fractions of a coordinate?
                public int X { get { return BitConverter.ToUInt16(Raw, 0x28); } set { BitConverter.GetBytes((ushort)value).CopyTo(Raw, 0x28); } }
                public int Y { get { return BitConverter.ToUInt16(Raw, 0x2A); } set { BitConverter.GetBytes((ushort)value).CopyTo(Raw, 0x2A); } }

                // -360, 360 ????
                public float Degrees { get { return BitConverter.ToSingle(Raw, 0x2C); } set { BitConverter.GetBytes(value).CopyTo(Raw, 0x2C); } }
                public float Deg18 => Degrees/18;

                public byte[] Raw;
                public byte[] OriginalData;
                internal static readonly byte Size = 0x30;
                public EntityNPC(byte[] data = null)
                {
                    Raw = data ?? new byte[Size];
                    OriginalData = (byte[])Raw.Clone();
                }
                public byte[] Write()
                {
                    return Raw;
                }
            }
            public class EntityWarp
            {
                // Usable Attributes
                public int DestinationMap { get { return BitConverter.ToUInt16(Raw, 0x00); } set { BitConverter.GetBytes((ushort)value).CopyTo(Raw, 0x00);} }
                public int DestinationTileIndex { get { return BitConverter.ToUInt16(Raw, 0x02); } set { BitConverter.GetBytes((ushort)value).CopyTo(Raw, 0x02);} }

                // Not sure if these are widths or face direction
                public int WX { get { return Raw[0x04]; } set { Raw[0x4] = (byte)value; } }
                public int WY { get { return Raw[0x05]; } set { Raw[0x5] = (byte)value; } }

                // Either 0 or 1, only in X/Y
                public int U06 { get { return BitConverter.ToUInt16(Raw, 0x06); } set { BitConverter.GetBytes((ushort)value).CopyTo(Raw, 0x06); } }
                // Coordinates have some upper-bit usage it seems...
                public int X { get { return BitConverter.ToUInt16(Raw, 0x08); } set { BitConverter.GetBytes((ushort)value).CopyTo(Raw, 0x08); } }
                public int Z { get { return BitConverter.ToInt16(Raw, 0x0A); } set { BitConverter.GetBytes((short)value).CopyTo(Raw, 0x0A); } }
                public int Y { get { return BitConverter.ToUInt16(Raw, 0x0C); } set { BitConverter.GetBytes((ushort)value).CopyTo(Raw, 0x0C); } }

                public decimal pX => (decimal)X / 18;
                public decimal pY => (decimal)Y / 18;

                // Stretches RIGHT
                public int Width { get { return BitConverter.ToInt16(Raw, 0x0E); } set { BitConverter.GetBytes((short)value).CopyTo(Raw, 0x0E); } }
                // Stretches DOWN
                public int Height { get { return BitConverter.ToInt16(Raw, 0x10); } set { BitConverter.GetBytes((short)value).CopyTo(Raw, 0x10); } }
                // Not sure.
                public int U12 { get { return BitConverter.ToInt16(Raw, 0x12); } set { BitConverter.GetBytes((short)value).CopyTo(Raw, 0x12); } }

                // 0x14-0x15 Unused
                // 0x16-0x17 Unused

                public byte[] Raw;
                public byte[] OriginalData;
                internal static readonly byte Size = 0x18;
                public EntityWarp(byte[] data = null)
                {
                    Raw = data ?? new byte[Size];
                    OriginalData = (byte[])Raw.Clone();
                }
                public byte[] Write()
                {
                    return Raw;
                }
            }
            public class EntityTrigger1
            {
                // Usable Attributes
                public int Script { get { return BitConverter.ToUInt16(Raw, 0x00); } set { BitConverter.GetBytes((ushort)value).CopyTo(Raw, 0x00); } }
                public int U2 { get { return BitConverter.ToUInt16(Raw, 0x02); } set { BitConverter.GetBytes((ushort)value).CopyTo(Raw, 0x02); } }
                public int Constant { get { return BitConverter.ToUInt16(Raw, 0x04); } set { BitConverter.GetBytes((ushort)value).CopyTo(Raw, 0x04); } }

                // 0 or 1 for type2, 0/5-8 for type1
                public int U6 { get { return BitConverter.ToUInt16(Raw, 0x06); } set { BitConverter.GetBytes((ushort)value).CopyTo(Raw, 0x06); } }
                // 0 or 1, always 0 in ORAS
                public int U8 { get { return BitConverter.ToUInt16(Raw, 0x08); } set { BitConverter.GetBytes((ushort)value).CopyTo(Raw, 0x08); } }
                
                // 0x0A-0x0B unused

                public int X { get { return BitConverter.ToUInt16(Raw, 0x0C); } set { BitConverter.GetBytes((ushort)value).CopyTo(Raw, 0x0C); } }
                public int Y { get { return BitConverter.ToUInt16(Raw, 0x0E); } set { BitConverter.GetBytes((ushort)value).CopyTo(Raw, 0x0E); } }

                public int Width { get { return BitConverter.ToInt16(Raw, 0x10); } set { BitConverter.GetBytes((short)value).CopyTo(Raw, 0x10); } }
                public int Height { get { return BitConverter.ToInt16(Raw, 0x12); } set { BitConverter.GetBytes((short)value).CopyTo(Raw, 0x12); } }
                public int U14 { get { return BitConverter.ToInt16(Raw, 0x14); } set { BitConverter.GetBytes((short)value).CopyTo(Raw, 0x14); } }
                public int U16 { get { return BitConverter.ToInt16(Raw, 0x16); } set { BitConverter.GetBytes((short)value).CopyTo(Raw, 0x16); } }

                public byte[] Raw;
                public byte[] OriginalData;
                internal static readonly byte Size = 0x18;
                public EntityTrigger1(byte[] data = null)
                {
                    Raw = data ?? new byte[Size];
                    OriginalData = (byte[])Raw.Clone();
                }
                public byte[] Write()
                {
                    return Raw;
                }
            }
            public class EntityTrigger2
            {
                // Usable Attributes
                public int Script { get { return BitConverter.ToUInt16(Raw, 0x00); } set { BitConverter.GetBytes((ushort)value).CopyTo(Raw, 0x00); } }
                public int U2 { get { return BitConverter.ToUInt16(Raw, 0x02); } set { BitConverter.GetBytes((ushort)value).CopyTo(Raw, 0x02); } }
                public int Constant { get { return BitConverter.ToUInt16(Raw, 0x04); } set { BitConverter.GetBytes((ushort)value).CopyTo(Raw, 0x04); } }

                // 0 or 1 for type2, 0/5-8 for type1
                public int U6 { get { return BitConverter.ToUInt16(Raw, 0x06); } set { BitConverter.GetBytes((ushort)value).CopyTo(Raw, 0x06); } }
                // 0 or 1, always 0 in ORAS
                public int U8 { get { return BitConverter.ToUInt16(Raw, 0x08); } set { BitConverter.GetBytes((ushort)value).CopyTo(Raw, 0x08); } }

                // 0x0A-0x0B unused

                public int X { get { return BitConverter.ToUInt16(Raw, 0x0C); } set { BitConverter.GetBytes((ushort)value).CopyTo(Raw, 0x0C); } }
                public int Y { get { return BitConverter.ToUInt16(Raw, 0x0E); } set { BitConverter.GetBytes((ushort)value).CopyTo(Raw, 0x0E); } }

                public int Width { get { return BitConverter.ToInt16(Raw, 0x10); } set { BitConverter.GetBytes((short)value).CopyTo(Raw, 0x10); } }
                public int Height { get { return BitConverter.ToInt16(Raw, 0x12); } set { BitConverter.GetBytes((short)value).CopyTo(Raw, 0x12); } }
                public int U14 { get { return BitConverter.ToInt16(Raw, 0x14); } set { BitConverter.GetBytes((short)value).CopyTo(Raw, 0x14); } }
                public int U16 { get { return BitConverter.ToInt16(Raw, 0x16); } set { BitConverter.GetBytes((short)value).CopyTo(Raw, 0x16); } }

                public byte[] Raw;
                public byte[] OriginalData;
                internal static readonly byte Size = 0x18;
                public EntityTrigger2(byte[] data = null)
                {
                    Raw = data ?? new byte[Size];
                    OriginalData = (byte[])Raw.Clone();
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
            public Script Script;
            public ZoneScript(byte[] data)
            {
                Data = data;
                Script = new Script(data);
            }
            public byte[] Write()
            {
                Data = Script.Write();
                return Data;
            }
        }
        public class ZoneEncounters
        {
            public byte[] Data; // File details unknown.
            public byte[] Header;
            public EncounterSet[] Encounters;
            public ZoneEncounters(byte[] data)
            {
                Data = data;

                using (BinaryReader br = new BinaryReader(new MemoryStream(data)))
                {
                    Header = br.ReadBytes(0x10);
                    Encounters = new EncounterSet[(int)(br.BaseStream.Length - br.BaseStream.Position)/4];
                    for (int i = 0; i < Encounters.Length; i++)
                        Encounters[i] = new EncounterSet(br.ReadBytes(4));
                }
            }
            public byte[] Write()
            {
                byte[] data = Header; // Start with the header data, then concat every encounter in afterwards.
                return Encounters.Aggregate(data, (current, t) => current.Concat(t.Write()).ToArray());
            }

            public class EncounterSet
            {
                public int Species;
                public int Form;
                public byte LevelMin, LevelMax;

                public EncounterSet(byte[] data)
                {
                    using (BinaryReader br = new BinaryReader(new MemoryStream(data)))
                    {
                        ushort SpecForm = br.ReadUInt16();
                        Species = SpecForm & 0x7FF;
                        Form = SpecForm >> 11;
                        LevelMin = br.ReadByte();
                        LevelMax = br.ReadByte();
                    }
                }
                public byte[] Write()
                {
                    using (MemoryStream ms = new MemoryStream())
                    using (BinaryWriter bw = new BinaryWriter(ms))
                    {
                        bw.Write((ushort)(Species | (Form << 11)));
                        bw.Write(LevelMin);
                        bw.Write(LevelMax);
                        return ms.ToArray();
                    }
                }
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
    public class Script
    {
        public int Length => BitConverter.ToInt32(Raw, 0x00);
        public uint Magic => BitConverter.ToUInt32(Raw, 0x04);
        // case 0x0A0AF1E0: code = read_code_block(f); break;
        // case 0x0A0AF1EF: debug = read_debug_block(f); break;
        public bool Debug => Magic  == 0x0A0AF1EF;

        public ushort PtrOffset => BitConverter.ToUInt16(Raw, 0x08);
        public ushort PtrCount => BitConverter.ToUInt16(Raw, 0x0A);

        public int ScriptInstructionStart => BitConverter.ToInt32(Raw, 0x0C);
        public int ScriptMovementStart => BitConverter.ToInt32(Raw, 0x10);
        public int FinalOffset => BitConverter.ToInt32(Raw, 0x14);
        public int AllocatedMemory => BitConverter.ToInt32(Raw, 0x18);

        // Generated Attributes
        public int CompressedLength => Length - ScriptInstructionStart;
        public byte[] CompressedBytes => Raw.Skip(ScriptInstructionStart).ToArray();
        public int DecompressedLength => FinalOffset - ScriptInstructionStart;
        public uint[] DecompressedInstructions => Scripts.quickDecompress(CompressedBytes, DecompressedLength/4);

        public uint[] ScriptCommands => DecompressedInstructions.Take((ScriptMovementStart - ScriptInstructionStart) / 4).ToArray();
        public uint[] MoveCommands => DecompressedInstructions.Skip((ScriptMovementStart - ScriptInstructionStart) / 4).ToArray();
        public string[] ParseScript => Scripts.parseScript(ScriptCommands);
        public string[] ParseMoves => Scripts.parseMovement(MoveCommands);

        public string Info => "Data Start: 0x" + ScriptInstructionStart.ToString("X4")
                              + Environment.NewLine + "Movement Offset: 0x" + ScriptMovementStart.ToString("X4")
                              + Environment.NewLine + "Total Used Size: 0x" + FinalOffset.ToString("X4")
                              + Environment.NewLine + "Reserved Size: 0x" + AllocatedMemory.ToString("X4")
                              + Environment.NewLine + "Compressed Len: 0x" + CompressedLength.ToString("X4")
                              + Environment.NewLine + "Decompressed Len: 0x" + DecompressedLength.ToString("X4")
                              + Environment.NewLine + "Compression Ratio: " +
                              ((DecompressedLength - CompressedLength)/(decimal)DecompressedLength).ToString("p1");

        public byte[] Raw;
        public Script(byte[] data = null)
        {
            Raw = data ?? new byte[0];
        }
        public byte[] Write()
        {
            return Raw;
        }
    }
    #endregion
}
