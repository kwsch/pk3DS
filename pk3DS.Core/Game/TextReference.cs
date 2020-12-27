namespace pk3DS.Core
{
    public enum TextName
    {
        AbilityNames,
        MoveNames,
        MoveFlavor,

        ItemNames,
        ItemFlavor,

        SpeciesNames,
        Types,
        Natures,
        Forms,

        TrainerNames,
        TrainerClasses,
        TrainerText,
        metlist_000000,
        OPowerFlavor,
        MaisonTrainerNames,
        SuperTrainerNames,
        BattleRoyalNames,
        BattleTreeNames,

        SpeciesClassifications,
        PokedexEntry1,
        PokedexEntry2
    }

    public class TextData
    {
        public readonly string[] Lines;
        public bool Modified { get; private set; }
        public TextData(string[] lines) => Lines = lines;

        public string this[int line]
        {
            get => Lines[line];
            set
            {
                if (Lines[line] == value)
                    return;
                Modified = true;
                Lines[line] = value;
            }
        }
    }

    public sealed class TextReference
    {
        public readonly int Index;
        public readonly TextName Name;

        private TextReference(int index, TextName name)
        {
            Index = index;
            Name = name;
        }

        public static readonly TextReference[] GameText_XY =
        {
            new(005, TextName.Forms),
            new(013, TextName.MoveNames),
            new(015, TextName.MoveFlavor),
            new(017, TextName.Types),
            new(020, TextName.TrainerClasses),
            new(021, TextName.TrainerNames),
            new(022, TextName.TrainerText),
            new(034, TextName.AbilityNames),
            new(047, TextName.Natures),
            new(072, TextName.metlist_000000),
            new(080, TextName.SpeciesNames),
            new(096, TextName.ItemNames),
            new(099, TextName.ItemFlavor),
            new(130, TextName.MaisonTrainerNames),
            new(131, TextName.SuperTrainerNames),
            new(141, TextName.OPowerFlavor),
        };

        public static readonly TextReference[] GameText_AO =
        {
            new(005, TextName.Forms),
            new(014, TextName.MoveNames),
            new(016, TextName.MoveFlavor),
            new(018, TextName.Types),
            new(021, TextName.TrainerClasses),
            new(022, TextName.TrainerNames),
            new(023, TextName.TrainerText),
            new(037, TextName.AbilityNames),
            new(051, TextName.Natures),
            new(090, TextName.metlist_000000),
            new(098, TextName.SpeciesNames),
            new(114, TextName.ItemNames),
            new(117, TextName.ItemFlavor),
            new(153, TextName.MaisonTrainerNames),
            new(154, TextName.SuperTrainerNames),
            new(165, TextName.OPowerFlavor),
        };

        public static readonly TextReference[] GameText_SMDEMO =
        {
            new(020, TextName.ItemFlavor),
            new(021, TextName.ItemNames),
            new(026, TextName.SpeciesNames),
            new(030, TextName.metlist_000000),
            new(044, TextName.Forms),
            new(044, TextName.Natures),
            new(046, TextName.AbilityNames),
            new(049, TextName.TrainerText),
            new(050, TextName.TrainerNames),
            new(051, TextName.TrainerClasses),
            new(052, TextName.Types),
            new(054, TextName.MoveFlavor),
            new(055, TextName.MoveNames),
        };

        public static readonly TextReference[] GameText_SM =
        {
            new(035, TextName.ItemFlavor),
            new(036, TextName.ItemNames),
            new(055, TextName.SpeciesNames),
            new(067, TextName.metlist_000000),
            new(086, TextName.BattleRoyalNames),
            new(087, TextName.Natures),
            new(096, TextName.AbilityNames),
            new(099, TextName.BattleTreeNames),
            new(104, TextName.TrainerText),
            new(105, TextName.TrainerNames),
            new(106, TextName.TrainerClasses),
            new(107, TextName.Types),
            new(112, TextName.MoveFlavor),
            new(113, TextName.MoveNames),
            new(114, TextName.Forms),
            new(116, TextName.SpeciesClassifications),
            new(119, TextName.PokedexEntry1),
            new(120, TextName.PokedexEntry2)
        };

        public static readonly TextReference[] GameText_USUM =
        {
            new(039, TextName.ItemFlavor),
            new(040, TextName.ItemNames),
            new(060, TextName.SpeciesNames),
            new(072, TextName.metlist_000000),
            new(091, TextName.BattleRoyalNames),
            new(092, TextName.Natures),
            new(101, TextName.AbilityNames),
            new(104, TextName.BattleTreeNames),
            new(109, TextName.TrainerText),
            new(110, TextName.TrainerNames),
            new(111, TextName.TrainerClasses),
            new(112, TextName.Types),
            new(117, TextName.MoveFlavor),
            new(118, TextName.MoveNames),
            new(119, TextName.Forms),
            new(121, TextName.SpeciesClassifications),
            new(124, TextName.PokedexEntry1),
            new(125, TextName.PokedexEntry2)
        };
    }
}
