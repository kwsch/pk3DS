using System.Collections.Generic;
using System.Linq;
using System.Text;
using pk3DS.Core;
using pk3DS.Core.CTR;
using pk3DS.Core.Structures;

namespace pk3DS
{
    public class Area7
    {
        public string Name => string.Join(" / ", Zones.Select(z => z.Name));
        public int FileNumber;
        public bool HasTables;
        public readonly List<EncounterTable> Tables;
        public ZoneData7[] Zones;

        public Area7()
        {
            Tables = new List<EncounterTable>();
        }

        public string GetSummary(string[] speciesList)
        {
            var sb = new StringBuilder();
            sb.AppendLine("==========");
            sb.AppendLine($"Map: {Name}");
            sb.AppendLine($"Tables: {Tables.Count / 2}");
            for (int i = 0; i < Tables.Count / 2; i++)
            {
                sb.AppendLine($"Table {i+1} (Day):");
                sb.AppendLine(Tables[i*2].GetSummary(speciesList));
                sb.AppendLine($"Table {i+1} (Night):");
                sb.AppendLine(Tables[i*2 + 1].GetSummary(speciesList));
            }
            sb.AppendLine("==========");
            return sb.ToString();
        }

        private const string PackIdentifier = "EA";
        public static byte[] GetDayNightTableBinary(IList<EncounterTable> tables)
        {
            byte[][] tabs = new byte[tables.Count / 2][];
            for (int i = 0; i < tables.Count; i += 2)
            {
                var table0 = tables[i + 0].Data; // day
                var table1 = tables[i + 1].Data; // night
                var arr = new byte[4 + table0.Length + table1.Length];
                table0.CopyTo(arr, 4);
                table1.CopyTo(arr, 4 + table0.Length);
                tabs[i / 2] = arr;
            }
            return mini.packMini(tabs, PackIdentifier);
        }

        public static Area7[] GetArray(lzGARCFile ed, ZoneData7[] zd)
        {
            int fileCount = ed.FileCount;
            var numAreas = fileCount / 11;
            var areas = new Area7[numAreas];
            for (int i = 0; i < numAreas; i++)
            {
                areas[i] = new Area7
                {
                    FileNumber = 9 + 11 * i,
                    Zones = zd.Where(z => z.AreaIndex == i).ToArray()
                };
                var md = ed[areas[i].FileNumber];
                if (md.Length <= 0)
                {
                    areas[i].HasTables = false;
                    continue;
                }

                byte[][] Tables = mini.unpackMini(md, PackIdentifier);
                areas[i].HasTables = Tables.Any(t => t.Length > 0);
                if (!areas[i].HasTables)
                    continue;

                foreach (var Table in Tables)
                {
                    var DayTable = Table.Skip(4).Take(0x164).ToArray();
                    var NightTable = Table.Skip(0x168).ToArray();
                    areas[i].Tables.Add(new EncounterTable(DayTable));
                    areas[i].Tables.Add(new EncounterTable(NightTable));
                }
            }
            return areas;
        }

        /// <summary>
        /// Gets an annotated Area array
        /// </summary>
        /// <param name="ed">Encounter Data GARC</param>
        /// <param name="zd">ZoneData GARC</param>
        /// <param name="wd">WorldData GARC</param>
        /// <param name="locationList">Location strings</param>
        /// <returns>Annotated Area Array</returns>
        public static Area7[] GetArray(lzGARCFile ed, lzGARCFile zd, lzGARCFile wd, string[] locationList)
        {
            var Worlds = wd.Files.Select(f => mini.unpackMini(f, "WD")[0]).ToArray();

            byte[][] zdfiles = zd.Files;
            var worldData = zdfiles[1];
            var zoneData = zdfiles[0];
            var zones = ZoneData7.GetZoneData7Array(zoneData, worldData, locationList, Worlds);
            var areas = GetArray(ed, zones);
            return areas;
        }
    }
}
