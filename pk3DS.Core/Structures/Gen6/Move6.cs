using System;

namespace pk3DS.Core.Structures
{
    public class Move
    {
        private const int SIZE = 0x22;
        private readonly byte[] Data;
        public Move() => Data = new byte[SIZE];
        public Move(byte[] data) => Data = data ?? new byte[SIZE];
        public byte[] Write() => Data;

        public int Type { get => Data[0x00]; set => Data[0x00] = (byte)value; }
        public int Quality { get => Data[0x01]; set => Data[0x01] = (byte)value; }
        public int Category { get => Data[0x02]; set => Data[0x02] = (byte)value; }
        public int Power { get => Data[0x03]; set => Data[0x03] = (byte)value; }
        public int Accuracy { get => Data[0x04]; set => Data[0x04] = (byte)value; }
        public int PP { get => Data[0x05]; set => Data[0x05] = (byte)value; }
        public int Priority { get => Data[0x06]; set => Data[0x06] = (byte)value; }
        public int HitMin { get => Data[0x07] & 0xF; set => Data[0x07] = (byte)(HitMax << 4 | value); }
        public int HitMax { get => Data[0x07] >> 4; set => Data[0x07] = (byte)(value << 4 | HitMin); }
        public int Inflict { get => BitConverter.ToUInt16(Data, 0x08); set => BitConverter.GetBytes((ushort)value).CopyTo(Data, 0x08); }
        public int InflictPercent { get => Data[0x0A]; set => Data[0x0A] = (byte)value; }
        public MoveInflictDuration InflictCount { get => (MoveInflictDuration)Data[0x0B]; set => Data[0x0B] = (byte)value; }
        public int TurnMin { get => Data[0x0C]; set => Data[0x0C] = (byte)value; }
        public int TurnMax { get => Data[0x0D]; set => Data[0x0D] = (byte)value; }
        public int CritStage { get => Data[0x0E]; set => Data[0x0E] = (byte)value; }
        public int Flinch { get => Data[0x0F]; set => Data[0x0F] = (byte)value; }
        public int EffectSequence { get => BitConverter.ToUInt16(Data, 0x10); set => BitConverter.GetBytes((ushort)value).CopyTo(Data, 0x10); }
        public int Recoil { get => Data[0x12]; set => Data[0x12] = (byte)value; }
        public Heal Healing { get => (Heal)Data[0x13]; set => Data[0x13] = (byte)value; }
        public MoveTarget Target { get => (MoveTarget)Data[0x14]; set => Data[0x14] = (byte)value; }
        public int Stat1 { get => Data[0x15]; set => Data[0x15] = (byte)value; }
        public int Stat2 { get => Data[0x16]; set => Data[0x16] = (byte)value; }
        public int Stat3 { get => Data[0x17]; set => Data[0x17] = (byte)value; }
        public int Stat1Stage { get => Data[0x18]; set => Data[0x18] = (byte)value; }
        public int Stat2Stage { get => Data[0x19]; set => Data[0x19] = (byte)value; }
        public int Stat3Stage { get => Data[0x1A]; set => Data[0x1A] = (byte)value; }
        public int Stat1Percent { get => Data[0x1B]; set => Data[0x1B] = (byte)value; }
        public int Stat2Percent { get => Data[0x1C]; set => Data[0x1C] = (byte)value; }
        public int Stat3Percent { get => Data[0x1D]; set => Data[0x1D] = (byte)value; }

        public MoveFlag Flags { get => (MoveFlag)BitConverter.ToUInt32(Data, 0x1E); set => BitConverter.GetBytes((uint)value).CopyTo(Data, 0x1E); }

        public enum Heal : byte
        {
            None = 0,
            Full = 255,
            Half = 254,
            Quarter = 253,
        }
    }

    public enum MoveInflictDuration
    {
        None = 0,
        Permanent,
        TurnCount,
        Unused,
        Switch,
    };

    public enum MoveTarget : byte
    {
        // Specific target
        AnyExceptSelf,
        AllyOrSelf,
        Ally,
        Opponent,
        AllAdjacent,
        AllAdjacentOpponents,
        AllAllies,
        Self,
        All,
        RandomOpponent,

        // No pkm target
        SideAll,
        SideOpponent,
        SideSelf,
        Counter,
    }

    [Flags]
    public enum MoveFlag : uint
    {
        None,
        // TBD
    }

    public static class MoveFlagExtensions
    {
        public static bool HasFlagFast(this MoveFlag value, MoveFlag flag)
        {
            return (value & flag) != 0;
        }
    }
}
