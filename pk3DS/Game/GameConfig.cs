using System.Linq;

namespace pk3DS
{
    public class GameConfig
    {
        private const int FILECOUNT_XY = 271;
        private const int FILECOUNT_ORASDEMO = 301;
        private const int FILECOUNT_ORAS = 299;
        private const int FILECOUNT_SMDEMO = 238;
        private const int FILECOUNT_SM = 236; // only a guess for now
        public readonly GameVersion Version = GameVersion.Invalid;

        public GARCReference[] Files { get; private set; }
        public TextVariableCode[] Variables { get; private set; }
        public TextReference[] GameText { get; private set; }

        public GameConfig(int fileCount)
        {
            GameVersion game = GameVersion.Invalid;
            switch (fileCount)
            {
                case FILECOUNT_XY:
                    game = GameVersion.XY;
                    break;
                case FILECOUNT_ORAS:
                    game = GameVersion.ORASDEMO;
                    break;
                case FILECOUNT_ORASDEMO:
                    game = GameVersion.ORAS;
                    break;
                case FILECOUNT_SMDEMO:
                    game = GameVersion.SMDEMO;
                    break;
                case FILECOUNT_SM:
                    game = GameVersion.SM;
                    break;
            }
            if (game == GameVersion.Invalid)
                return;

            Version = game;
            getGameData(game);
        }
        public GameConfig(GameVersion game)
        {
            Version = game;
            getGameData(game);
        }

        private void getGameData(GameVersion game)
        {
            switch (game)
            {
                case GameVersion.XY:
                    Files = GARCReference.GARCReference_XY;
                    Variables = TextVariableCode.VariableCodes_XY;
                    GameText = TextReference.GameText_XY;
                    break;

                case GameVersion.ORASDEMO:
                case GameVersion.ORAS:
                    Files = GARCReference.GARCReference_AO;
                    Variables = TextVariableCode.VariableCodes_AO;
                    GameText = TextReference.GameText_AO;
                    break;

                case GameVersion.SMDEMO:
                case GameVersion.SM:
                    Files = GARCReference.GARCReference_SM;
                    Variables = TextVariableCode.VariableCodes_SM;
                    GameText = TextReference.GameText_SM;
                    break;
            }
        }

        public GARCReference getGARC(string name) { return Files.FirstOrDefault(f => f.Name == name); }
        public TextVariableCode getVariableCode(string name) { return Variables.FirstOrDefault(v => v.Name == name); }
        public TextVariableCode getVariableName(int value) { return Variables.FirstOrDefault(v => v.Code == value); }
        public TextReference getGameText(string name) { return GameText.FirstOrDefault(f => f.Name == name); }

        public bool XY => Version == GameVersion.XY;
        public bool ORAS => Version == GameVersion.ORAS || Version == GameVersion.ORASDEMO;
        public bool SM => Version == GameVersion.SM || Version == GameVersion.SMDEMO;
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
    }
}
