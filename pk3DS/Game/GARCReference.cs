using System.IO;
using System.Linq;

namespace pk3DS
{
    public class GARCReference
    {
        public readonly int FileNumber;
        public readonly string Name;
        private int A => (FileNumber / 100) % 10;
        private int B => (FileNumber / 10) % 10;
        private int C => (FileNumber / 1) % 10;
        public readonly bool LanguageVariant;
        public string Reference => Path.Combine("a", A.ToString(), B.ToString(), C.ToString());

        private GARCReference(int file, string name, bool lv = false)
        {
            Name = name;
            FileNumber = file;
            LanguageVariant = lv;
        }
        public GARCReference getRelativeGARC(int offset, string name = "")
        {
            return new GARCReference(FileNumber + offset, name);
        }

        public static readonly GARCReference[] GARCReference_XY =
        {
            new GARCReference(005, "movesprite"),
            new GARCReference(012, "encdata"),
            new GARCReference(038, "trdata"),
            new GARCReference(039, "trclass"),
            new GARCReference(040, "trpoke"),
            new GARCReference(041, "mapGR"),
            new GARCReference(042, "mapMatrix"),
            new GARCReference(104, "wallpaper"),
            new GARCReference(165, "titlescreen"),
            new GARCReference(203, "maisonpkN"),
            new GARCReference(204, "maisontrN"),
            new GARCReference(205, "maisonpkS"),
            new GARCReference(206, "maisontrS"),
            new GARCReference(212, "move"),
            new GARCReference(213, "eggmove"),
            new GARCReference(214, "levelup"),
            new GARCReference(215, "evolution"),
            new GARCReference(216, "megaevo"),
            new GARCReference(218, "personal"),
            new GARCReference(220, "item"),

            // Varied
            new GARCReference(072, "gametext", true),
            new GARCReference(080, "storytext", true),
        };
        public static readonly GARCReference[] GARCReference_AO =
        {
            new GARCReference(013, "encdata"),
            new GARCReference(036, "trdata"),
            new GARCReference(037, "trclass"),
            new GARCReference(038, "trpoke"),
            new GARCReference(039, "mapGR"),
            new GARCReference(040, "mapMatrix"),
            new GARCReference(103, "wallpaper"),
            new GARCReference(152, "titlescreen"),
            new GARCReference(182, "maisonpkN"),
            new GARCReference(183, "maisontrN"),
            new GARCReference(184, "maisonpkS"),
            new GARCReference(185, "maisontrS"),
            new GARCReference(189, "move"),
            new GARCReference(190, "eggmove"),
            new GARCReference(191, "levelup"),
            new GARCReference(192, "evolution"),
            new GARCReference(193, "megaevo"),
            new GARCReference(195, "personal"),
            new GARCReference(197, "item"),
                
            // Varied
            new GARCReference(071, "gametext", true),
            new GARCReference(079, "storytext", true),
        };
        public static readonly GARCReference[] GARCReference_SMDEMO =
        {
            new GARCReference(011, "move"),
            new GARCReference(012, "eggmove"),
            new GARCReference(013, "levelup"),
            new GARCReference(014, "evolution"),
            new GARCReference(015, "megaevo"),
            new GARCReference(017, "personal"),
            new GARCReference(019, "item"),

            new GARCReference(076, "zonedata"),
            new GARCReference(081, "encdata"),

            new GARCReference(101, "trclass"),
            new GARCReference(102, "trdata"),
            new GARCReference(103, "trpoke"),
                
            // Varied
            new GARCReference(030, "gametext", true),
            new GARCReference(040, "storytext", true),
        };
        private static readonly GARCReference[] GARCReference_SM =
        {
            new GARCReference(011, "move"),
            new GARCReference(012, "eggmove"),
            new GARCReference(013, "levelup"),
            new GARCReference(014, "evolution"),
            new GARCReference(015, "megaevo"),
            new GARCReference(017, "personal"),
            new GARCReference(019, "item"),

            new GARCReference(077, "zonedata"),
            new GARCReference(091, "worlddata"),

            new GARCReference(104, "trclass"),
            new GARCReference(105, "trdata"),
            new GARCReference(106, "trpoke"),
                
            // Varied
            new GARCReference(030, "gametext", true),
            new GARCReference(040, "storytext", true),
        };

        public static readonly GARCReference[] GARCReference_SN = GARCReference_SM.Concat(
            new[] {
              new GARCReference(082, "encdata"),
            }).ToArray();
        public static readonly GARCReference[] GARCReference_MN = GARCReference_SM.Concat(
            new[] {
              new GARCReference(083, "encdata"),
            }).ToArray();
    }
}
