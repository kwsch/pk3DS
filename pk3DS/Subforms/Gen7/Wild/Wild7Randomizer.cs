using pk3DS.Core;
using pk3DS.Core.Randomizers;
using System.Collections.Generic;
using System.Linq;
using System.Diagnostics;

namespace pk3DS
{
    public class Wild7Randomizer
    {
        public SpeciesRandomizer RandSpec { private get; set; }
        public FormRandomizer RandForm { private get; set; }

        public int TableRandomizationOption { private get; set; }
        public decimal LevelAmplifier { private get; set; }
        public bool ModifyLevel { private get; set; }
        public bool BalanceRates { private get; set; }
        public bool IgnoreDayNight { private get; set; }
        public bool RndBySlot { private get; set; }

        private void RandomizeTable7(EncounterTable Table, int slotStart, int slotStop)
        {
            int end = slotStop < 0 ? Table.Encounter7s.Length : slotStop;
            int SlotCounter = 0;
            int TotalRates = 0;

            for (int s = slotStart; s < end; s++)
            {
                var EncounterSet = Table.Encounter7s[s];
                foreach (var enc in EncounterSet.Where(enc => enc.Species != 0))
                {
                    enc.Species = (uint)RandSpec.GetRandomSpecies((int)enc.Species);
                    enc.Forme = (uint)RandForm.GetRandomForme((int)enc.Species);
                    
                    //Necessary because Weather SOS slots can be greater than initial SOS slots (see USUM Map set 181 for examples)
                    if(s < 1) 
                    SlotCounter++;
                }

                //Get number of encounters, divide by 100, distribute remainder to reach 100 total 
                if (SlotCounter > 0)
                    TotalRates = 100 % SlotCounter;
                if (BalanceRates == true)
                {
                    for (int r = 0; r < SlotCounter; r++)
                    {
                        int NewRate = 100 / SlotCounter;
                        if (TotalRates > 0)
                        {
                            NewRate++;
                            TotalRates--;
                        }
                        Table.Rates[r] = NewRate;
                    }
                }
                TotalRates = 0;
                SlotCounter = 0;
            }
        }

        //Copy Species, Form, and rate from slot to slot.  Day Table is randomized first so Copy it to Night table
        public void CopyTableToTable(EncounterTable TableD, EncounterTable TableN)
        {
            for (int s = 0; s < TableD.Encounter7s.Length; s++)
            {
                var EncounterSetD = TableD.Encounter7s[s];
                var EncounterSetN = TableN.Encounter7s[s];
                for (int e = 0; e < EncounterSetD.Length; e++)
                {
                    EncounterSetN[e].Species = EncounterSetD[e].Species;
                    EncounterSetN[e].Forme = EncounterSetD[e].Forme;
                }

                for (int r = 0; r < TableD.Rates.Length; r++)
                {
                    TableN.Rates[r] = TableD.Rates[r];
                }
            }
        }

        public void Execute(IEnumerable<Area7> Areas, LazyGARCFile encdata)
        {
            GetTableRandSettings((RandOption)TableRandomizationOption, out int slotStart, out int slotStop, out bool copy);

            if ((RandOption)TableRandomizationOption == RandOption.Regular_Then_SOS)
                Randomize(Areas, encdata, 0, 1, copy);
            if (RndBySlot)
            {
                for (int sl = 1; sl < 8; sl++)
                {
                    Debug.Print("Randomizing slot" + sl);
                    Randomize(Areas, encdata, sl, sl + 1, copy);
                }
                Debug.Print("Randomizing other");
                Randomize(Areas, encdata, 8, slotStop, copy);
            }
            else
            {
                Randomize(Areas, encdata, slotStart, slotStop, copy);
            }
        }

        private void Randomize(IEnumerable<Area7> Areas, LazyGARCFile encdata, int slotStart, int slotStop, bool copy)
        {
            RandSpec.Shuffle();
            foreach (var Map in Areas)
            {
                //foreach (var Table in Map.Tables)  //Replaced so that tables can be skipped via IgnoreDayNight option
                for (int t = 0; t < Map.Tables.Count; t++)
                {
                    Debug.Print("Day table = " + t);
                    EncounterTable Table = Map.Tables[t];
                    if (ModifyLevel)
                    {
                        Table.MinLevel = Randomizer.GetModifiedLevel(Table.MinLevel, LevelAmplifier);
                        Table.MaxLevel = Randomizer.GetModifiedLevel(Table.MaxLevel, LevelAmplifier);
                    }
                    RandomizeTable7(Table, slotStart, slotStop);
                    if (copy) // copy row 0 to rest
                        Table.CopySlotsToSOS();
                    Table.Write();
                    t++;
                }
                encdata[Map.FileNumber] = Area7.GetDayNightTableBinary(Map.Tables);
            }
            RandSpec.Shuffle();
            foreach (var Map in Areas)
            { 
                for (int t = 1; t < Map.Tables.Count; t++)
                {
                    Debug.Print("Night table = " + t);
                    EncounterTable Table = Map.Tables[t];
                    if (IgnoreDayNight) // Copy Table 1 to table 2 and skip to table 3.  All encounter tables have a Day and Night version  
                    {
                        CopyTableToTable(Map.Tables[t - 1], Table);
                        Map.Tables[t - 1].Write();
                        t++;
                    }
                    else
                    {
                        if (ModifyLevel)
                        {
                            Table.MinLevel = Randomizer.GetModifiedLevel(Table.MinLevel, LevelAmplifier);
                            Table.MaxLevel = Randomizer.GetModifiedLevel(Table.MaxLevel, LevelAmplifier);
                        }
                        RandomizeTable7(Table, slotStart, slotStop);
                        if (copy) // copy row 0 to rest
                            Table.CopySlotsToSOS();

                        Table.Write();
                        t++;
                    }
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

                case RandOption.Regular_Then_SOS: //Guarantees all species have an encounter before randomizing SOS slots
                    slotStart = 1;
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
            Regular_Then_SOS = 4,
        }
    }
}