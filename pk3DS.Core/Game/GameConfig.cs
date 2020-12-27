using System.IO;
using System.Linq;
using pk3DS.Core.CTR;
using pk3DS.Core.Structures.PersonalInfo;
using pk3DS.Core.Structures;

namespace pk3DS.Core
{
    public class GameConfig
    {
        private const int FILECOUNT_XY = 271;
        private const int FILECOUNT_ORASDEMO = 301;
        private const int FILECOUNT_ORAS = 299;
        private const int FILECOUNT_SMDEMO = 239;
        private const int FILECOUNT_SM = 311;
        private const int FILECOUNT_USUM = 333;
        public readonly GameVersion Version = GameVersion.Invalid;

        public GARCReference[] Files { get; private set; }
        public TextVariableCode[] Variables { get; private set; }
        public TextReference[] GameText { get; private set; }
        public GameInfo Info { get; private set; }

        /// <summary>
        /// Whether or not to remap characters in text files to proper unicode. Defaults to false.
        /// </summary>
        /// <remarks>
        /// This will enable the display of ♂ and ♀, but may interfere with text editing. Set this to true if you're not going to change any text, otherwise set it to false.
        /// </remarks>
        public bool RemapCharacters { get; set; } = false;

        public GameConfig(int fileCount)
        {
            GameVersion game = GameVersion.Invalid;
            switch (fileCount)
            {
                case FILECOUNT_XY:
                    game = GameVersion.XY;
                    break;
                case FILECOUNT_ORASDEMO:
                    game = GameVersion.ORASDEMO;
                    break;
                case FILECOUNT_ORAS:
                    game = GameVersion.ORAS;
                    break;
                case FILECOUNT_SMDEMO:
                    game = GameVersion.SMDEMO;
                    break;
                case FILECOUNT_SM:
                    game = GameVersion.SM;
                    break;
                case FILECOUNT_USUM:
                    game = GameVersion.USUM;
                    break;
            }
            if (game == GameVersion.Invalid)
                return;

            Version = game;
        }

        public GameConfig(GameVersion game)
        {
            Version = game;
        }

        private void GetGameData(GameVersion game)
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
                case GameVersion.SN:
                case GameVersion.MN:
                case GameVersion.SM:
                    Files = GARCReference.GARCReference_SN;
                    if (new FileInfo(Path.Combine(RomFS, GetGARCFileName("encdata"))).Length == 0)
                        Files = GARCReference.GARCReference_MN;
                    Variables = TextVariableCode.VariableCodes_SM;
                    GameText = TextReference.GameText_SM;
                    break;
                case GameVersion.US:
                case GameVersion.UM:
                case GameVersion.USUM:
                    Files = GARCReference.GARCReference_US;
                    if (new FileInfo(Path.Combine(RomFS, GetGARCFileName("encdata"))).Length == 0)
                        Files = GARCReference.GARCReference_UM;
                    Variables = TextVariableCode.VariableCodes_SM;
                    GameText = TextReference.GameText_USUM;
                    break;
            }
        }

        public void Initialize(string romFSpath, string exeFSpath, int lang)
        {
            RomFS = romFSpath;
            ExeFS = exeFSpath;
            Language = lang;
            GetGameData(Version);
            InitializeAll();
        }

        public void InitializeAll()
        {
            InitializePersonal();
            InitializeLearnset();
            InitializeGameText();
            InitializeMoves();
            InitializeEvos();
            InitializeGameInfo();
        }

        public void InitializePersonal()
        {
            GARCPersonal = GetGARCData("personal");
            Personal = new PersonalTable(GARCPersonal.GetFile(GARCPersonal.FileCount - 1), Version);
        }

        public void InitializeLearnset()
        {
            GARCLearnsets = GetGARCData("levelup");
            switch (Generation)
            {
                case 6:
                    Learnsets = GARCLearnsets.Files.Select(file => new Learnset6(file)).ToArray();
                    break;
                case 7:
                    Learnsets = GARCLearnsets.Files.Select(file => new Learnset6(file)).ToArray();
                    break;
            }
        }

        public void InitializeGameText()
        {
            GARCGameText = GetGARCData("gametext");
            GameTextStrings = GARCGameText.Files.Select(file => new TextFile(this, file, RemapCharacters).Lines).ToArray();
        }

        public void InitializeMoves()
        {
            GARCMoves = GetGARCData("move");
            switch (Generation)
            {
                case 6:
                    if (XY)
                        Moves = GARCMoves.Files.Select(file => new Move6(file)).ToArray();
                    if (ORAS)
                        Moves = Mini.UnpackMini(GARCMoves.GetFile(0), "WD").Select(file => new Move6(file)).ToArray();
                    break;
                case 7:
                    Moves = Mini.UnpackMini(GARCMoves.GetFile(0), "WD").Select(file => new Move7(file)).ToArray();
                    break;
            }
        }
        public void InitializeEvos()
        {
            var g = GetGARCData("evolution");
            byte[][] d = g.Files;
            switch (Generation)
            {
                case 6:
                    Evolutions = d.Select(z => new EvolutionSet6(z)).ToArray();
                    break;
                case 7:
                    Evolutions = d.Select(z => new EvolutionSet7(z)).ToArray();
                    break;
            }
        }

        private void InitializeGameInfo()
        {
            Info = new GameInfo(this);
        }

        public LazyGARCFile GetlzGARCData(string file)
        {
            var gr = GetGARCReference(file);
            gr = gr.LanguageVariant ? gr.GetRelativeGARC(Language, gr.Name) : gr;
            return new LazyGARCFile(GetlzGARC(file), gr, GetGARCPath(file));
        }

        public GARCFile GetGARCData(string file, bool skipRelative = false)
        {
            var gr = GetGARCReference(file);
            if (gr.LanguageVariant && !skipRelative)
                gr = gr.GetRelativeGARC(Language, gr.Name);
            return GetGARCByReference(gr);
        }

        public GARCFile GetGARCByReference(GARCReference gr)
        {
            return new(GetMemGARC(gr.Name), gr, GetGARCPath(gr.Name));
        }

        private string GetGARCPath(string file)
        {
            var gr = GetGARCReference(file);
            gr = gr.LanguageVariant ? gr.GetRelativeGARC(Language, gr.Name) : gr;
            string subloc = gr.Reference;
            return Path.Combine(RomFS, subloc);
        }

        private GARC.MemGARC GetMemGARC(string file)
        {
            return new(File.ReadAllBytes(GetGARCPath(file)));
        }

        private GARC.LazyGARC GetlzGARC(string file)
        {
            return new(File.ReadAllBytes(GetGARCPath(file)));
        }

        public string RomFS, ExeFS;

        public GARCReference GetGARCReference(string name) { return Files?.FirstOrDefault(f => f.Name == name); }
        public TextVariableCode GetVariableCode(string name) { return Variables?.FirstOrDefault(v => v.Name == name); }
        public TextVariableCode GetVariableName(int value) { return Variables?.FirstOrDefault(v => v.Code == value); }

        private TextReference GetGameText(TextName name) { return GameText?.FirstOrDefault(f => f.Name == name); }
        public TextData GetTextData(TextName file) => new(GetText(file));

        public string[] GetText(TextName file)
        {
            return (string[])GameTextStrings[GetGameText(file).Index].Clone();
        }

        public bool SetText(TextName file, string[] strings)
        {
            GameTextStrings[GetGameText(file).Index] = strings;
            return true;
        }

        public string GetGARCFileName(string requestedGARC)
        {
            var garc = GetGARCReference(requestedGARC);
            if (garc.LanguageVariant)
                garc = garc.GetRelativeGARC(Language);

            return garc.Reference;
        }

        public int Language { get; set; }

        public GARCFile GARCPersonal, GARCLearnsets, GARCMoves, GARCGameText;
        public PersonalTable Personal { get; private set; }
        public Learnset[] Learnsets { get; private set; }
        public string[][] GameTextStrings { get; private set; }
        public Move[] Moves { get; private set; }
        public EvolutionSet[] Evolutions { get; private set; }

        public bool XY => Version == GameVersion.XY;
        public bool ORAS => Version == GameVersion.ORAS || Version == GameVersion.ORASDEMO;
        public bool SM => Version == GameVersion.SM || Version == GameVersion.SMDEMO;
        public bool USUM => Version == GameVersion.USUM;
        public int MaxSpeciesID => XY || ORAS ? Legal.MaxSpeciesID_6 : SM ? Legal.MaxSpeciesID_7_SM : Legal.MaxSpeciesID_7_USUM;
        public int GARCVersion => XY || ORAS ? GARC.VER_4 : GARC.VER_6;

        public int Generation
        {
            get
            {
                if (XY || ORAS)
                    return 6;
                if (SM || USUM)
                    return 7;
                return -1;
            }
        }

        public bool IsRebuildable(int fileCount)
        {
            return fileCount switch
            {
                FILECOUNT_XY => Version == GameVersion.XY,
                FILECOUNT_ORAS => Version == GameVersion.ORAS,
                FILECOUNT_ORASDEMO => Version == GameVersion.ORASDEMO,
                FILECOUNT_SMDEMO => Version == GameVersion.SMDEMO,
                FILECOUNT_SM => Version == GameVersion.SM,
                FILECOUNT_USUM => Version == GameVersion.USUM,
                _ => false
            };
        }
    }
}
