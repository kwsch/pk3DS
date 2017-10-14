using System.Collections.Generic;
using System.Linq;

using pk3DS.Core;
using pk3DS.Core.Randomizers;

namespace pk3DS
{
    public class Wild7Randomizer
    {
        public SpeciesRandomizer RandSpec { private get; set; }
        public FormRandomizer RandForm { private get; set; }

        public int TableRandomizationOption { private get; set; }
        public decimal LevelAmplifier { private get; set; }
        public bool ModifyLevel { private get; set; }

        private void RandomizeTable7(EncounterTable Table, int slotStart, int slotStop)
        {
            int end = slotStop < 0 ? Table.Encounter7s.Length : slotStop;
            for (int s = slotStart; s < end; s++)
            {
                var EncounterSet = Table.Encounter7s[s];
                foreach (var enc in EncounterSet.Where(enc => enc.Species != 0))
                {
                    enc.Species = (uint)RandSpec.GetRandomSpecies((int)enc.Species);
                    enc.Forme = (uint)RandForm.GetRandomForme((int)enc.Species);
                }
            }
        }

        public void Execute(IEnumerable<Area7> Areas, lzGARCFile encdata)
        {
            GetTableRandSettings((RandOption)TableRandomizationOption, out int slotStart, out int slotStop, out bool copy);

            foreach (var Map in Areas)
            {
                foreach (var Table in Map.Tables)
                {
                    if (ModifyLevel)
                    {
                        Table.MinLevel = Randomizer.getModifiedLevel(Table.MinLevel, LevelAmplifier);
                        Table.MaxLevel = Randomizer.getModifiedLevel(Table.MaxLevel, LevelAmplifier);
                    }

                    RandomizeTable7(Table, slotStart, slotStop);
                    if (copy) // copy row 0 to rest
                        Table.CopySlotsToSOS();

                    Table.Write();
                }
                encdata[Map.FileNumber] = Area7.GetDayNightTableBinary(Map.Tables);
            }
        }

        private static void GetTableRandSettings(RandOption option, out int slotStart, out int slotStop, out bool copy)
        {
            copy = false;
            switch (option)
            {
                default: // All
                    slotStart = 0;
                    slotStop = -1;
                    break;
                case RandOption.Regular_Only:
                    slotStart = 0;
                    slotStop = 1;
                    break;
                case RandOption.SOS_Only:
                    slotStart = 1;
                    slotStop = -1;
                    break;
                case RandOption.Regular_CopySOS:
                    slotStart = 0;
                    slotStop = 1;
                    copy = true;
                    break;
            }
        }

        private enum RandOption
        {
            All = 0,
            Regular_Only = 1,
            SOS_Only = 2,
            Regular_CopySOS = 3,
        }
    }
}
