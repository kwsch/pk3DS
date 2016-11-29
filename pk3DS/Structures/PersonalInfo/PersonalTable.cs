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

        public string[][] getFormList(string[] species, int MaxSpecies)
        {
            string[][] FormList = new string[MaxSpecies + 1][];
            for (int i = 0; i <= MaxSpecies; i++)
            {
                int FormCount = this[i].FormeCount;
                FormList[i] = new string[FormCount];
                if (FormCount <= 0) continue;
                FormList[i][0] = species[i];
                for (int j = 1; j < FormCount; j++)
                {
                    FormList[i][j] = $"{species[i]} " + j;
                }
            }

            return FormList;
        }
        public string[] getPersonalEntryList(string[][] AltForms, string[] species, int MaxSpecies, out int[] baseForm, out int[] formVal)
        {
            string[] result = new string[Table.Length];
            baseForm = new int[result.Length];
            formVal = new int[result.Length];
            for (int i = 0; i <= MaxSpecies; i++)
            {
                result[i] = species[i];
                if (AltForms[i].Length == 0) continue;
                int altformpointer = this[i].FormStatsIndex;
                if (altformpointer <= 0) continue;
                for (int j = 1; j < AltForms[i].Length; j++)
                {
                    int ptr = altformpointer + j - 1;
                    baseForm[ptr] = i;
                    formVal[ptr] = j;
                    result[ptr] = AltForms[i][j];
                }
            }
            return result;
        }
        public int[] getSpeciesForm(int PersonalEntry)
        {
            if (PersonalEntry < Main.Config.MaxSpeciesID) return new[] { PersonalEntry, 0 };

            for (int i = 0; i < Main.Config.MaxSpeciesID; i++)
            {
                int FormCount = this[i].FormeCount - 1; // Mons with no alt forms have a FormCount of 1.
                var altformpointer = this[i].FormStatsIndex;
                if (altformpointer <= 0) continue;
                for (int j = 0; j < FormCount; j++)
                    if (altformpointer + j == PersonalEntry)
                        return new[] { i, j };
            }

            return new[] { -1, -1 };
        }
    }
}
