using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace pk3DS
{
    public class EncounterTable
    {
        public int MinLevel;
        public int MaxLevel;
        public readonly int[] Rates;
        public readonly Encounter7[][] Encounter7s;
        public readonly Encounter7[] AdditionalSOS;

        public readonly byte[] Data;

        public EncounterTable(byte[] t)
        {
            Rates = new int[10];
            Encounter7s = new Encounter7[9][];
            MinLevel = t[0];
            MaxLevel = t[1];

            for (int i = 0; i < Rates.Length; i++)
                Rates[i] = t[2 + i];

            for (int i = 0; i < Encounter7s.Length - 1; i++)
            {
                Encounter7s[i] = new Encounter7[10];
                var ofs = 0xC + i * 4 * Encounter7s[i].Length;
                for (int j = 0; j < Encounter7s[i].Length; j++)
                    Encounter7s[i][j] = new Encounter7(BitConverter.ToUInt32(t, ofs + 4 * j));
            }

            AdditionalSOS = new Encounter7[6];
            for (var i = 0; i < AdditionalSOS.Length; i++)
                AdditionalSOS[i] = new Encounter7(BitConverter.ToUInt32(t, 0x14C + 4 * i));

            Encounter7s[8] = AdditionalSOS;
            Data = (byte[])t.Clone();
        }

        /// <summary>
        /// Copies the first set of slots to all others, including weather if specified.
        /// </summary>
        /// <param name="PlusWeather">Copy first set to weather slots (only first few get copied as Weather has less slots)</param>
        public void CopySlotsToSOS(bool PlusWeather = false)
        {
            int length = Encounter7s.Length - 2; // except weather
            if (PlusWeather)
                length++; // reinclude weather slots
            var toBeCopiedTo = Encounter7s.Skip(1).Take(length);

            var first = Encounter7s[0];
            foreach (var slots in toBeCopiedTo)
            {
                for (int s = 0; s < slots.Length; s++)
                {
                    slots[s].Species = first[s].Species;
                    slots[s].Forme = first[s].Forme;
                }
            }
        }

        public void Write()
        {
            Data[0] = (byte)MinLevel;
            Data[1] = (byte)MaxLevel;
            for (int i = 0; i < Rates.Length; i++)
                Data[2 + i] = (byte)Rates[i];

            for (int i = 0; i < Encounter7s.Length - 1; i++)
            {
                var ofs = 0xC + i * 4 * Encounter7s[i].Length;
                for (int j = 0; j < Encounter7s[i].Length; j++)
                    BitConverter.GetBytes(Encounter7s[i][j].RawValue).CopyTo(Data, ofs + 4 * j);
            }

            for (int i = 0; i < AdditionalSOS.Length; i++)
                BitConverter.GetBytes(AdditionalSOS[i].RawValue).CopyTo(Data, 0x14C + 4 * i);
        }

        public string GetSummary(string[] speciesList)
        {
            var sb = new StringBuilder();
            for (int i = 0; i < Encounter7s.Length - 1; i++)
            {
                var tn = "Encounters";
                if (i != 0)
                    tn = "SOS Slot " + i;
                sb.Append($"{tn} (Levels {MinLevel}-{MaxLevel}): ");
                sb.AppendLine(GetSlotSetSummary(speciesList, i));
            }

            sb.Append("Additional SOS encounters: ");
            sb.AppendLine(string.Join(", ", AdditionalSOS
                .Select(e => e.RawValue).Distinct().Select(e => new Encounter7(e))
                .Select(e => e.GetSummary(speciesList))));

            return sb.ToString();
        }

        private string GetSlotSetSummary(string[] speciesList, int setNumber)
        {
            var specToRate = new Dictionary<uint, int>();
            var distincts = new List<Encounter7>();
            for (int j = 0; j < Encounter7s[setNumber].Length; j++)
            {
                var encounter = Encounter7s[setNumber][j];
                if (!specToRate.ContainsKey(encounter.RawValue))
                {
                    specToRate[encounter.RawValue] = 0;
                    distincts.Add(encounter);
                }
                specToRate[encounter.RawValue] += Rates[j];
            }
            var list = distincts.OrderBy(e => specToRate[e.RawValue]).Reverse();
            var summaries = list.Select(e => $"{e.GetSummary(speciesList)} ({specToRate[e.RawValue]}%)");
            return string.Join(", ", summaries);
        }
    }
}
