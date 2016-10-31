using System;

namespace pk3DS
{
    public class PersonalTable
    {
        private static byte[][] splitBytes(byte[] data, int size)
        {
            byte[][] r = new byte[data.Length / size][];
            for (int i = 0; i < data.Length; i += size)
            {
                r[i / size] = new byte[size];
                Array.Copy(data, i, r[i / size], 0, size);
            }
            return r;
        }
        public PersonalTable(byte[] data, GameVersion format)
        {
            int size = 0;
            switch (format)
            {
                case GameVersion.XY: size = PersonalInfoXY.SIZE; break;
                case GameVersion.ORASDEMO:
                case GameVersion.ORAS: size = PersonalInfoORAS.SIZE; break;
                case GameVersion.SMDEMO:
                case GameVersion.SM: size = PersonalInfoSM.SIZE; break;
            }

            if (size == 0)
            { Table = null; return; }

            byte[][] entries = splitBytes(data, size);
            PersonalInfo[] d = new PersonalInfo[data.Length / size];

            switch (format)
            {
                case GameVersion.XY:
                    for (int i = 0; i < d.Length; i++)
                        d[i] = new PersonalInfoXY(entries[i]);
                    break;
                case GameVersion.ORASDEMO:
                case GameVersion.ORAS:
                    for (int i = 0; i < d.Length; i++)
                        d[i] = new PersonalInfoORAS(entries[i]);
                    break;
                case GameVersion.SMDEMO:
                case GameVersion.SM:
                    for (int i = 0; i < d.Length; i++)
                        d[i] = new PersonalInfoSM(entries[i]);
                    break;
            }
            Table = d;
        }

        public readonly PersonalInfo[] Table;
        public PersonalInfo this[int index]
        {
            get
            {
                if (index < Table.Length)
                    return Table[index];
                return Table[0];
            }
            set
            {
                if (index < Table.Length)
                    return;
                Table[index] = value; 
            }
        }

        public int[] getAbilities(int species, int forme)
        {
            if (species >= Table.Length)
            { species = 0; Console.WriteLine("Requested out of bounds SpeciesID"); }
            return this[getFormeIndex(species, forme)].Abilities;
        }
        public int getFormeIndex(int species, int forme)
        {
            if (species >= Table.Length)
            { species = 0; Console.WriteLine("Requested out of bounds SpeciesID"); }
            return this[species].FormeIndex(species, forme);
        }
        public PersonalInfo getFormeEntry(int species, int forme)
        {
            return this[getFormeIndex(species, forme)];
        }
    }
}
