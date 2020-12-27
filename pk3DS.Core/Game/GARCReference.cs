using System.IO;
using System.Linq;

namespace pk3DS.Core
{
    public sealed class GARCReference
    {
        public readonly int FileNumber;
        public readonly string Name;
        private int A => FileNumber / 100 % 10;
        private int B => FileNumber / 10 % 10;
        private int C => FileNumber / 1 % 10;
        public readonly bool LanguageVariant;
        public string Reference => Path.Combine("a", A.ToString(), B.ToString(), C.ToString());

        private GARCReference(int file, string name, bool lv = false)
        {
            Name = name;
            FileNumber = file;
            LanguageVariant = lv;
        }

        public GARCReference GetRelativeGARC(int offset, string name = "")
        {
            return new(FileNumber + offset, name);
        }

        public static readonly GARCReference[] GARCReference_XY =
        {
            new(005, "movesprite"),
            new(012, "encdata"),
            new(038, "trdata"),
            new(039, "trclass"),
            new(040, "trpoke"),
            new(041, "mapGR"),
            new(042, "mapMatrix"),
            new(104, "wallpaper"),
            new(165, "titlescreen"),
            new(203, "maisonpkN"),
            new(204, "maisontrN"),
            new(205, "maisonpkS"),
            new(206, "maisontrS"),
            new(212, "move"),
            new(213, "eggmove"),
            new(214, "levelup"),
            new(215, "evolution"),
            new(216, "megaevo"),
            new(218, "personal"),
            new(220, "item"),

            // Varied
            new(072, "gametext", true),
            new(080, "storytext", true),
        };

        public static readonly GARCReference[] GARCReference_AO =
        {
            new(013, "encdata"),
            new(036, "trdata"),
            new(037, "trclass"),
            new(038, "trpoke"),
            new(039, "mapGR"),
            new(040, "mapMatrix"),
            new(103, "wallpaper"),
            new(152, "titlescreen"),
            new(182, "maisonpkN"),
            new(183, "maisontrN"),
            new(184, "maisonpkS"),
            new(185, "maisontrS"),
            new(189, "move"),
            new(190, "eggmove"),
            new(191, "levelup"),
            new(192, "evolution"),
            new(193, "megaevo"),
            new(195, "personal"),
            new(197, "item"),

            // Varied
            new(071, "gametext", true),
            new(079, "storytext", true),
        };

        public static readonly GARCReference[] GARCReference_SMDEMO =
        {
            new(011, "move"),
            new(012, "eggmove"),
            new(013, "levelup"),
            new(014, "evolution"),
            new(015, "megaevo"),
            new(017, "personal"),
            new(019, "item"),

            new(076, "zonedata"),
            new(081, "encdata"),

            new(101, "trclass"),
            new(102, "trdata"),
            new(103, "trpoke"),

            // Varied
            new(030, "gametext", true),
            new(040, "storytext", true),
        };

        private static readonly GARCReference[] GARCReference_SM =
        {
            new(011, "move"),
            new(012, "eggmove"),
            new(013, "levelup"),
            new(014, "evolution"),
            new(015, "megaevo"),
            new(017, "personal"),
            new(019, "item"),

            new(077, "zonedata"),
            new(091, "worlddata"),

            new(104, "trclass"),
            new(105, "trdata"),
            new(106, "trpoke"),

            new(155, "encounterstatic"),

            new(267, "pickup"),

            new(277, "maisonpkN"),
            new(278, "maisontrN"),
            new(279, "maisonpkS"),
            new(280, "maisontrS"),

            // Varied
            new(030, "gametext", true),
            new(040, "storytext", true),
        };

        private static readonly GARCReference[] GARCReference_USUM =
        {
            new(011, "move"),
            new(012, "eggmove"),
            new(013, "levelup"),
            new(014, "evolution"),
            new(015, "megaevo"),
            new(017, "personal"),
            new(019, "item"),

            new(077, "zonedata"),
            new(091, "worlddata"),

            new(105, "trclass"),
            new(106, "trdata"),
            new(107, "trpoke"),

            new(159, "encounterstatic"),

            new(271, "pickup"),

            new(281, "maisonpkN"),
            new(282, "maisontrN"),
            new(283, "maisonpkS"),
            new(284, "maisontrS"),

            // Varied
            new(030, "gametext", true),
            new(040, "storytext", true),
        };

        public static readonly GARCReference[] GARCReference_SN = GARCReference_SM.Concat(
            new[] {
              new GARCReference(082, "encdata"),
            }).ToArray();

        public static readonly GARCReference[] GARCReference_MN = GARCReference_SM.Concat(
            new[] {
              new GARCReference(083, "encdata"),
            }).ToArray();

        public static readonly GARCReference[] GARCReference_US = GARCReference_USUM.Concat(
            new[] {
                new GARCReference(082, "encdata"),
            }).ToArray();

        public static readonly GARCReference[] GARCReference_UM = GARCReference_USUM.Concat(
            new[] {
                new GARCReference(083, "encdata"),
            }).ToArray();
    }
}
