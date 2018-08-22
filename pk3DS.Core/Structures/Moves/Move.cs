namespace pk3DS.Core.Structures
{
    public abstract class Move
    {
        protected readonly byte[] Data;
        protected abstract int SIZE { get; }
        protected Move(byte[] data = null) => Data = data ?? new byte[SIZE];

        public byte[] Write() => Data;

        public abstract int Type { get; set; }
        public abstract int Quality { get; set; }
        public abstract int Category { get; set; }
        public abstract int Power { get; set; }
        public abstract int Accuracy { get; set; }
        public abstract int PP { get; set; }
        public abstract int Priority { get; set; }
        public abstract int HitMin { get; set; }
        public abstract int HitMax { get; set; }
        public abstract int Inflict { get; set; }
        public abstract int InflictPercent { get; set; }
        public abstract MoveInflictDuration InflictCount { get; set; }
        public abstract int TurnMin { get; set; }
        public abstract int TurnMax { get; set; }
        public abstract int CritStage { get; set; }
        public abstract int Flinch { get; set; }
        public abstract int EffectSequence { get; set; }
        public abstract int Recoil { get; set; }
        public abstract Heal Healing { get; set; }
        public abstract MoveTarget Target { get; set; }
        public abstract int Stat1 { get; set; }
        public abstract int Stat2 { get; set; }
        public abstract int Stat3 { get; set; }
        public abstract int Stat1Stage { get; set; }
        public abstract int Stat2Stage { get; set; }
        public abstract int Stat3Stage { get; set; }
        public abstract int Stat1Percent { get; set; }
        public abstract int Stat2Percent { get; set; }
        public abstract int Stat3Percent { get; set; }
    }
}