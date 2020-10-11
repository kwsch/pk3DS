using System.Collections.Generic;
using System.IO;
using System.Linq;
using pk3DS.Core.Structures.PersonalInfo;

namespace pk3DS.Core
{
    public static class Gen7SlotDumper
    {
        public static byte[][] GetRegularBinary(Area7[] areas)
        {
            var dict = DumpAreas(areas);
            return GetLocationDump(dict).ToArray();
        }
        public static byte[][] GetSOSBinary(Area7[] areas, PersonalTable personal)
        {
            var dict = DumpAreas(areas, personal);
            return GetLocationDump(dict).ToArray();
        }

        private static Dictionary<int, List<uint>> DumpAreas(Area7[] areas, PersonalTable personal)
        {
            var dict = new Dictionary<int, List<uint>>();
            for (var areaIndex = 0; areaIndex < areas.Length; areaIndex++)
            {
                var area = areas[areaIndex];
                for (var zoneIndex = 0; zoneIndex < area.Zones.Length; zoneIndex++)
                {
                    var z = area.Zones[zoneIndex];
                    int loc = z.ParentMap;
                    if (!dict.ContainsKey(loc))
                        dict.Add(loc, new List<uint>());

                    var table = dict[loc];
                    for (var x = 0; x < area.Tables.Count; x++)
                    {
                        var t = area.Tables[x];
                        var first = t.Encounter7s[0];
                        for (int i = 0; i < first.Length; i++)
                        {
                            // Only add the column SOS slots if the wild slot can SOS for help.
                            var wild = first[i];
                            if (personal[(int)wild.Species].EscapeRate == 0)
                                continue;

                            for (int j = 1; j < t.Encounter7s.Length - 1; j++)
                                table.Add(t.Encounter7s[j][i].Dump(t));
                        }

                        foreach (var s in t.AdditionalSOS)
                            table.Add(s.Dump(t));
                    }
                }
            }

            return dict;
        }

        private static Dictionary<int, List<uint>> DumpAreas(Area7[] areas)
        {
            var dict = new Dictionary<int, List<uint>>();
            for (var areaIndex = 0; areaIndex < areas.Length; areaIndex++)
            {
                var area = areas[areaIndex];
                for (var zoneIndex = 0; zoneIndex < area.Zones.Length; zoneIndex++)
                {
                    var z = area.Zones[zoneIndex];
                    int loc = z.ParentMap;
                    if (!dict.ContainsKey(loc))
                        dict.Add(loc, new List<uint>());

                    var table = dict[loc];
                    for (var tableIndex = 0; tableIndex < area.Tables.Count; tableIndex++)
                    {
                        var t = area.Tables[tableIndex];
                        var first = t.Encounter7s[0];
                        for (int i = 0; i < first.Length; i++)
                        {
                            // Only add the column SOS slots if the wild slot can SOS for help.
                            var wild = first[i];
                            table.Add(wild.Dump(t));
                        }
                    }
                }
            }

            return dict;
        }

        private static IEnumerable<byte[]> GetLocationDump(Dictionary<int, List<uint>> dict)
        {
            foreach (var z in dict.OrderBy(z => z.Key))
            {
                using (var ms = new MemoryStream())
                using (var bw = new BinaryWriter(ms))
                {
                    bw.Write((ushort)z.Key);
                    foreach (var s in z.Value.Distinct())
                        bw.Write(s);
                    yield return ms.ToArray();
                }
            }
        }
    }
}