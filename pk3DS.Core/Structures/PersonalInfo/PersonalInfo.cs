namespace pk3DS.Core.Structures.PersonalInfo
{
    public abstract class PersonalInfo
    {
        protected byte[] Data;
        public abstract byte[] Write();
        public abstract int HP { get; set; }
        public abstract int ATK { get; set; }
        public abstract int DEF { get; set; }
        public abstract int SPE { get; set; }
        public abstract int SPA { get; set; }
        public abstract int SPD { get; set; }

        public int[] Stats
        {
            get => new[] {HP, ATK, DEF, SPE, SPA, SPD};
            set
            {
                HP = value[0];
                ATK = value[1];
                DEF = value[2];
                SPE = value[3];
                SPA = value[4];
                SPD = value[5];
            }
        }

        public abstract int EV_HP { get; set; }
        public abstract int EV_ATK { get; set; }
        public abstract int EV_DEF { get; set; }
        public abstract int EV_SPE { get; set; }
        public abstract int EV_SPA { get; set; }
        public abstract int EV_SPD { get; set; }

        public abstract int[] Types { get; set; }
        public abstract int CatchRate { get; set; }
        public virtual int EvoStage { get; set; }
        public abstract int[] Items { get; set; }
        public abstract int Gender { get; set; }
        public abstract int HatchCycles { get; set; }
        public abstract int BaseFriendship { get; set; }
        public abstract int EXPGrowth { get; set; }
        public abstract int[] EggGroups { get; set; }
        public abstract int [] Abilities { get; set; }
        public abstract int EscapeRate { get; set; }
        public virtual int FormeCount { get; set; }
        public virtual int FormStatsIndex { get; protected internal set; }
        public virtual int FormeSprite { get; set; }
        public abstract int BaseEXP { get; set; }
        public abstract int Color { get; set; }

        public virtual int Height { get; set; } = 0;
        public virtual int Weight { get; set; } = 0;

        public bool[] TMHM { get; set; }
        public bool[] TypeTutors { get; set; }
        public bool[][] SpecialTutors { get; set; } = System.Array.Empty<bool[]>();

        protected static bool[] GetBits(byte[] data)
        {
            bool[] r = new bool[8 * data.Length];
            for (int i = 0; i < r.Length; i++)
                r[i] = (data[i/8] >> (i&7) & 0x1) == 1;
            return r;
        }

        protected static byte[] SetBits(bool[] bits)
        {
            byte[] data = new byte[bits.Length/8];
            for (int i = 0; i < bits.Length; i++)
                data[i / 8] |= (byte)(bits[i] ? 1 << (i&0x7) : 0);
            return data;
        }

        // Data Manipulation
        public int FormeIndex(int species, int forme)
        {
            if (forme <= 0) // no forme requested
                return species;
            if (FormStatsIndex <= 0) // no formes present
                return species;
            if (forme > FormeCount) // beyond range of species' formes
                return species;

            return FormStatsIndex + forme - 1;
        }

        public int RandomGender
        {
            get
            {
                return Gender switch
                {
                    255 => 2, // Genderless
                    254 => 1, // Female
                    0 => 0, // Male
                    _ => (int)(Util.Random32() % 2),
                };
            }
        }

        public bool HasFormes => FormeCount > 1;
        public int BST => HP + ATK + DEF + SPE + SPA + SPD;
    }
}
