using System.IO;
using System.Linq;
using CTR;

namespace pk3DS
{
    public class GameConfig
    {
        private const int FILECOUNT_XY = 271;
        private const int FILECOUNT_ORASDEMO = 301;
        private const int FILECOUNT_ORAS = 299;
        private const int FILECOUNT_SMDEMO = 239;
        private const int FILECOUNT_SM = 311; // only a guess for now
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
                    Files = GARCReference.GARCReference_SMDEMO;
                    Variables = TextVariableCode.VariableCodes_SM;
                    GameText = TextReference.GameText_SMDEMO;
                    break;
                case GameVersion.SM:
                    Files = GARCReference.GARCReference_SM;
                    Variables = TextVariableCode.VariableCodes_SM;
                    GameText = TextReference.GameText_SM;
                    break;
            }
        }
        public void Initialize(string romFSpath, string exeFSpath, int lang)
        {
            RomFS = romFSpath;
            ExeFS = exeFSpath;
            Language = lang;
        }

        public void InitializePersonal()
        {
            var pG = getROMFSFile("personal");
            Personal = new PersonalTable(pG.getFile(pG.FileCount - 1), Version);
        }

        private GARC.MemGARC getROMFSFile(string file)
        {
            string path = Path.Combine(RomFS, getGARC(file).Reference);
            return new GARC.MemGARC(File.ReadAllBytes(path));
        }

        private string RomFS, ExeFS;

        public GARCReference getGARC(string name) { return Files.FirstOrDefault(f => f.Name == name); }
        public TextVariableCode getVariableCode(string name) { return Variables.FirstOrDefault(v => v.Name == name); }
        public TextVariableCode getVariableName(int value) { return Variables.FirstOrDefault(v => v.Code == value); }
        public TextReference getGameText(TextName name) { return GameText.FirstOrDefault(f => f.Name == name); }

        public string getGARCFileName(string requestedGARC)
        {
            var garc = getGARC(requestedGARC);
            if (garc.LanguageVariant)
                garc = garc.getRelativeGARC(Language);

            return garc.Reference;
        }

        public int Language { get; set; }
        public PersonalTable Personal;
        public bool XY => Version == GameVersion.XY;
        public bool ORAS => Version == GameVersion.ORAS || Version == GameVersion.ORASDEMO;
        public bool SM => Version == GameVersion.SM || Version == GameVersion.SMDEMO;
        public int MaxSpeciesID => XY || ORAS ? 722 : 802;
        public int GARCVersion => XY || ORAS ? GARC.VER_4 : GARC.VER_6;
        public int Generation
        {
            get
            {
                if (XY || ORAS)
                    return 6;
                if (SM)
                    return 7;
                return -1;
            }
        }

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
