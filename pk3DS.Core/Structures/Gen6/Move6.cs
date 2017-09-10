using System;
using System.IO;

namespace pk3DS.Core.Structures
{
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
}
