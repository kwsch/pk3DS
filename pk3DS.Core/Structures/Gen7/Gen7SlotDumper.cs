using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using pk3DS.Core.Structures.PersonalInfo;

namespace pk3DS.Core
{
    public static class Gen7SlotDumper
    {
        public static byte[][] GetRegularBinary(Area7[] areas, bool sm)
        {
            var dict = DumpAreas(areas, sm ? InaccessibleUnused_SM : InaccessibleUnused_USUM);
            return GetLocationDump(dict).ToArray();
        }
        public static byte[][] GetSOSBinary(Area7[] areas, PersonalTable personal, bool sm)
        {
            var dict = DumpAreas(areas, personal, sm ? InaccessibleUnused_SM : InaccessibleUnused_USUM);
            return GetLocationDump(dict).ToArray();
        }

        private static Dictionary<int, List<uint>> DumpAreas(Area7[] areas, PersonalTable personal, IReadOnlyDictionary<int, int[]> ignored)
        {
            var dict = new Dictionary<int, List<uint>>();
            for (var areaIndex = 0; areaIndex < areas.Length; areaIndex++)
            {
                var area = areas[areaIndex];
                for (var zoneIndex = 0; zoneIndex < area.Zones.Length; zoneIndex++)
                {
                    var z = area.Zones[zoneIndex];
                    int loc = z.ParentMap;

                    var ignore = ignored.TryGetValue(z.Index, out var skip) ? skip : Array.Empty<int>();
                    if (!dict.ContainsKey(loc))
                        dict.Add(loc, new List<uint>());

                    for (var index = 0; index < area.Tables.Count; index++)
                    {
                        var t = area.Tables[index];
                        if (ignore.Contains((index >> 1) + 1)) // not zero indexed; bias +1
                        {
                            Log(areaIndex, z.Index, index, z.Name);
                            continue;
                        }

                        if (!dict.ContainsKey(loc))
                            dict.Add(loc, new List<uint>());
                        var table = dict[loc];
                        var first = t.Encounter7s[0];
                        if (first.All(sz => sz.Species == 731))
                        {
                            Log(areaIndex, z.Index, index, z.Name, "Pikipek Table");
                            continue;
                        }

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

        private static Dictionary<int, List<uint>> DumpAreas(Area7[] areas, IReadOnlyDictionary<int, int[]> ignored)
        {
            var dict = new Dictionary<int, List<uint>>();
            for (var areaIndex = 0; areaIndex < areas.Length; areaIndex++)
            {
                var area = areas[areaIndex];
                for (var zoneIndex = 0; zoneIndex < area.Zones.Length; zoneIndex++)
                {
                    var z = area.Zones[zoneIndex];
                    int loc = z.ParentMap;

                    var ignore = ignored.TryGetValue(z.Index, out var skip) ? skip : Array.Empty<int>();
                    for (var index = 0; index < area.Tables.Count; index++)
                    {
                        if (ignore.Contains((index >> 1) + 1)) // not zero indexed; bias +1
                        {
                            Log(areaIndex, z.Index, index, z.Name);
                            continue;
                        }
                        if (!dict.ContainsKey(loc))
                            dict.Add(loc, new List<uint>());

                        var t = area.Tables[index];
                        var first = t.Encounter7s[0];
                        if (first.All(sz => sz.Species == 731))
                        {
                            Log(areaIndex, z.Index, index, z.Name, "Pikipek Table");
                            continue;
                        }
                        var table = dict[loc];
                        foreach (var wild in first)
                            table.Add(wild.Dump(t));
                    }
                }
            }

            return dict;
        }

        private static void Log(int area, int zi, int ti, string zn, string msg = "Dictionary")
        {
            Console.WriteLine($"Skipped [{area}] {zi:00},{ti:00} ({(ti >> 1) + 1:00} {(ti % 2 == 0 ? "D" : "N")}) @ {zn}: {msg}");
        }

        public static bool IsZoneAccessible(int areaIndex, int zoneIndex, bool sm)
        {
            var dict = sm ? InaccessibleUnused_SM : InaccessibleUnused_USUM;
            if (!dict.TryGetValue(areaIndex, out var zones))
                return false;

            return !zones.Contains((zoneIndex >> 1) + 1);
        }

        private static IEnumerable<byte[]> GetLocationDump(Dictionary<int, List<uint>> dict)
        {
            foreach (var z in dict.OrderBy(z => z.Key))
            {
                using var ms = new MemoryStream();
                using var bw = new BinaryWriter(ms);
                bw.Write((ushort)z.Key);
                foreach (var s in z.Value.Distinct())
                    bw.Write(s);
                yield return ms.ToArray();
            }
        }

        public static readonly Dictionary<int, int[]> InaccessibleUnused_USUM = new()
        {
            // Route 1 (Hau’oli Outskirts)
            {000, new[] {1, 2, 4, 5, 6, 7, 8, 9, 12, 13, 14, 15, 16, 17, 18}},

            // Route 1
            {003, new[] {3, 4, 5, 6, 7, 8, 10, 11, 18}},

            // Melemele Sea
            {010, new[] {1, 2, 3, 9, 10, 12, 13, 14, 15, 16, 17, 18}},

            // Route 1 (Hau’oli Outskirts)
            {001, new[] {1, 2, 3}},

            // Route 3
            {006, new[] {2, 5, 6, 7, 8, 9, 15}},

            // Kala’e Bay
            {008, new[] {1, 3, 4, 10, 11, 12, 13, 14}},

            // Hau’oli City (Beachfront)
            {012, new[] {2, 3, 4, 5, 6}},

            // Hau’oli City (Shopping District)
            {013, new[] {1, 2, 7, 8}},

            // Hau’oli City (Marina)
            {014, new[] {1, 2, 3, 4, 5, 6, 7, 8}},

            // Hano Grand Resort
            {086, new[] {1, 2, 3, 4}},

            // Memorial Hill
            {097, new[] {3, 4, 5, 6, 7, 8, 9}},

            // Akala Outskirts
            {098, new[] {1, 2}},

            // Tapu Village
            {168, new[] {2, 3, 4, 5, 6, 7, 8}},

            // Route 14
            {169, new[] {1}},

            // Route 15
            {170, new[] {5, 7, 8, 10}},

            // Route 16
            {171, new[] {1, 2, 3, 4, 6, 9, 10}},

            // Malie City
            {174, new[] {1, 2, 3}},

            // Ancient Poni Path
            {265, new[] {1, 2, 3, 4, 5, 6, 7, 8}},

            // Poni Breaker Coast
            {266, new[] {9, 10, 11}},
        };

        public static readonly Dictionary<int, int[]> InaccessibleUnused_SM = new()
        {
            // Route 1 (Hau’oli Outskirts)
            {000, new[] {1, 2, 3, 5, 6, 7, 8, 9, 10, 11, 12, 13, 15, 16}},

            // Route 1
            {001, new[] {4, 5, 6, 7, 8, 9, 14, 17}},

            // Melemele Sea
            {006, new[] {1, 2, 3, 4, 10, 11, 12, 13, 14, 15, 16, 17}},

            // Route 3
            {002, new[] {2, 5, 6, 7, 8, 9, 15}},

            // Kala’e Bay
            {004, new[] {1, 3, 4, 10, 11, 12, 13, 14}},

            // Hau’oli City (Beachfront)
            {007, new[] {2, 3, 4, 5, 6}},

            // Hau’oli City (Shopping District)
            {008, new[] {1, 2}},

            // Hau’oli City (Marina)
            {009, new[] {1, 2, 3, 4, 5, 6, 7, 8}},

            // Hano Grand Resort
            {074, new[] {1, 2, 3, 4}},

            // Memorial Hill
            {083, new[] {3, 4, 5, 6, 7, 8, 9}},

            // Akala Outskirts
            {084, new[] {1, 2}},

            // Secluded Shore
            {142, new[] {1, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15}},

            // Route 12
            {149, new[] {2, 3, 4, 5}},

            // Tapu Village
            {144, new[] {2, 3, 4, 5, 6, 7, 8}},

            // Route 14
            {145, new[] {1}},

            // Route 15
            {146, new[] {5, 6, 7, 8, 10}},

            // Route 16
            {147, new[] {2, 3, 4, 9, 10}},

            // Malie City
            {150, new[] {1, 2, 3}},

            // Ancient Poni Path
            {232, new[] {1, 2, 3, 4, 5}},

            // Poni Breaker Coast
            {233, new[] {6, 7, 8}},
        };
    }
}
