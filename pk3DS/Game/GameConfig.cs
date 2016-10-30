using System.Collections.Generic;
using System.Linq;

namespace pk3DS
{
    public class GameConfig
    {
        private const int FILECOUNT_XY = 271;
        private const int FILECOUNT_ORASDEMO = 301;
        private const int FILECOUNT_ORAS = 299;
        private const int FILECOUNT_SMDEMO = 238;
        private const int FILECOUNT_SM = 1;
        public readonly List<GARCReference> Files = new List<GARCReference>();
        public GARCReference getGARC(string name) { return Files.FirstOrDefault(file => file.Name == name); }
        public bool XY => Version == GameVersion.XY;
        public bool ORAS => Version == GameVersion.ORAS || Version == GameVersion.ORASDEMO;
        public bool SM => Version == GameVersion.SM || Version == GameVersion.SMDEMO;

        private readonly GameVersion Version = GameVersion.Invalid;
        public bool IsRebuildable(int fileCount)
        {
            switch (fileCount)
            {
                case FILECOUNT_XY:
                    return Version == GameVersion.XY;
                case FILECOUNT_ORAS:
                    return Version == GameVersion.ORAS;
                case FILECOUNT_ORASDEMO:
                    return Version == GameVersion.ORASDEMO;
                case FILECOUNT_SMDEMO:
                    return Version == GameVersion.SMDEMO;
                case FILECOUNT_SM:
                    return Version == GameVersion.SM;
            }
            return false;
        }

        public GameConfig(int fileCount)
        {
            GameVersion game = GameVersion.Invalid;
            switch (fileCount)
            {
                case FILECOUNT_XY:
                    game = GameVersion.XY;
                    break;
                case FILECOUNT_ORAS:
                case FILECOUNT_ORASDEMO:
                    game = GameVersion.ORAS;
                    break;
                case FILECOUNT_SMDEMO:
                case FILECOUNT_SM:
                    game = GameVersion.SM;
                    break;
            }
            if (game == GameVersion.Invalid)
                return;

            Files.AddRange(getGameFiles(game));
            Version = game;
        }
        public GameConfig(GameVersion game)
        {
            Files.AddRange(getGameFiles(game));
            Version = game;
        }

        private static IEnumerable<GARCReference> getGameFiles(GameVersion game)
        {
            switch (game)
            {
                case GameVersion.XY:
                    return setupXY();
                case GameVersion.ORASDEMO:
                case GameVersion.ORAS:
                    return setupORAS();
                case GameVersion.SMDEMO:
                case GameVersion.SM:
                    return setupSM();
                default:
                    return null;
            }
        }
        private static IEnumerable<GARCReference> setupXY()
        {
            return new[]
            {
                new GARCReference(005,"movesprite"),
                new GARCReference(012,"encdata"),
                new GARCReference(038, "trdata"),
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
        }
        private static IEnumerable<GARCReference> setupORAS()
        {
            return new[]
            {
                new GARCReference(013, "encdata"),
                new GARCReference(036, "trdata"),
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
        }
        private static IEnumerable<GARCReference> setupSM()
        {
            return new[]
            {
                new GARCReference(011, "move"),
                new GARCReference(012, "eggmove"),
                new GARCReference(013, "levelup"),
                new GARCReference(014, "evolution"),
                new GARCReference(015, "megaevo"),
                new GARCReference(017, "personal"),
                new GARCReference(019, "item"),

                new GARCReference(081, "encdata"),

                new GARCReference(101, "trclass"),
                new GARCReference(102, "trdata"),
                new GARCReference(013, "trpoke"),
                
                // Varied
                new GARCReference(030, "gametext", true),
                new GARCReference(040, "storytext", true),
            };
        }
    }
}
