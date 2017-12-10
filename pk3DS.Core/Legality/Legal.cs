using System.Linq;

namespace pk3DS.Core
{
    public static partial class Legal
    {
        public static readonly ushort[] Mega_XY =
        {
            3,6,9,65,80,
            115,127,130,142,150,181,
            212,214,229,248,282,
            303,306,308,310,354,359,380,381,
            445,448,460,
        };
        public static readonly ushort[] Mega_ORAS = Mega_XY.Concat(new ushort[]
        {
            15,18,94,
            208,254,257,260,
            302,319,323,334,362,373,376,384,
            428,475,
            531,
            719
        }).ToArray();
        public static readonly int[] SpecialClasses_XY =
        {
            #region Classes
            000, // Pokémon Trainer
            001, // Pokémon Trainer
            004, // Leader
            035, // Elite Four
            036, // Elite Four
            037, // Elite Four
            038, // Elite Four
            039, // Leader
            040, // Leader
            041, // Leader
            042, // Leader
            043, // Leader
            044, // Leader
            045, // Leader
            053, // Champion
            055, // Pokémon Trainer
            056, // Pokémon Trainer
            057, // Pokémon Trainer
            064, // Battle Chatelaine
            065, // Battle Chatelaine
            066, // Battle Chatelaine
            067, // Battle Chatelaine
            102, // Pokémon Trainer
            103, // Pokémon Trainer
            104, // Pokémon Trainer
            105, // Pokémon Professor
            151, // Grand Duchess
            160, // Pokémon Trainer
            161, // Pokémon Trainer
            170, // Pokémon Trainer
            171, // Pokémon Trainer
            172, // Pokémon Trainer
            173, // Team Flare
            174, // Team Flare
            175, // Team Flare Boss
            176, // Successor
            177, // Leader
            #endregion
        };
        public static readonly int[] SpecialClasses_ORAS =
        {
            #region Classes
            000, // Pokémon Trainer
            001, // Pokémon Trainer
            004, // Leader
            035, // Elite Four
            036, // Elite Four
            037, // Elite Four
            038, // Elite Four
            039, // Leader
            040, // Leader
            041, // Leader
            042, // Leader
            043, // Leader
            044, // Leader
            045, // Leader
            053, // Champion
            055, // Pokémon Trainer
            056, // Pokémon Trainer
            057, // Pokémon Trainer
            064, // Battle Chatelaine
            065, // Battle Chatelaine
            066, // Battle Chatelaine
            067, // Battle Chatelaine
            081, // Team Flare Boss
            102, // Pokémon Trainer
            103, // Pokémon Trainer
            104, // Pokémon Trainer
            105, // Pokémon Professor
            109, // Pokémon Trainer
            110, // Pokémon Trainer
            119, // Pokémon Trainer
            120, // Pokémon Trainer
            121, // Pokémon Trainer
            124, // Team Flare Boss
            125, // Successor
            126, // Leader
            127, // Pokémon Trainer
            128, // Pokémon Trainer
            174, // Aqua Leader
            178, // Magma Leader
            192, // Pokémon Trainer
            194, // Elite Four
            195, // Elite Four
            196, // Elite Four
            197, // Elite Four
            198, // Champion
            200, // Leader
            201, // Leader
            202, // Leader
            203, // Leader
            204, // Leader
            205, // Leader
            206, // Leaders
            207, // Leader
            219, // Pokémon Trainer
            232, // Pokémon Trainer
            233, // Pokémon Trainer
            234, // Pokémon Trainer
            267, // Pokémon Trainer
            268, // Sootopolitan
            270, // Pokémon Trainer
            271, // Pokémon Trainer
            272, // Pokémon Trainer
            273, // Elite Four
            274, // Elite Four
            275, // Elite Four
            276, // Elite Four
            277, // Champion
            278, // Pokémon Trainer
            279, // Pokémon Trainer
            #endregion
        };

        public static readonly int[] SpecialClasses_SM =
        {
            030, // Pokémon Trainer: Hau
            031, // Island Kahuna: Hala
            038, // Captain: Ilima
            042, // Trial Guide: Ben
            044, // Captain: Lana
            045, // Captain: Mallow
            049, // Island Kahuna: Olivia
            051, // Island Kahuna: Hapu
            071, // Aether President: Lusamine
            072, // Aether Branch Chief: Faba
            076, // Team Skull Boss: Guzma
            078, // Team Skull Admin: Plumeria
            079, // Pokémon Trainer: Plumeria
            080, // Elite Four: Kahili
            081, // Pokémon Trainer: [~ 157]
            082, // Aether President: Lusamine
            083, // Pokémon Trainer: Red
            084, // Pokémon Trainer: Blue
            085, // Pokémon Trainer: Sina
            086, // Pokémon Trainer: Dexio
            088, // Pokémon Trainer: Anabel
            092, // Pro Wrestler: The Royal
            093, // Pokémon Trainer: Molayne
            099, // Pokémon Trainer: Molayne
            100, // Pokémon Trainer: Hau
            101, // Pokémon Trainer: Hau
            102, // Pokémon Trainer: Gladion
            103, // Pokémon Trainer: Gladion
            107, // Elite Four: Acerola
            109, // Elite Four: Hala
            110, // Elite Four: Olivia
            111, // Pokémon Professor: Kukui
            139, // GAME FREAK: Morimoto
            141, // Island Kahuna: Nanu
            142, // Captain: Sophocles
            143, // Pokémon Trainer: Ryuki
            153, // Captain: Mina
            162, // Aether Foundation: Faba
            164, // Island Kahuna: Hapu
            165, // Pokémon Professor: Kukui
            185, // Aether Foundation: Faba
        };

        public static readonly int[] SpecialClasses_USUM =
{
            030, // Pokémon Trainer: Hau
            031, // Island Kahuna: Hala
            038, // Captain: Ilima
            042, // Trial Guide: Ben
            044, // Captain: Lana
            045, // Captain: Mallow
            049, // Island Kahuna: Olivia
            051, // Island Kahuna: Hapu
            071, // Aether President: Lusamine
            072, // Aether Branch Chief: Faba
            076, // Team Skull Boss: Guzma
            078, // Team Skull Admin: Plumeria
            079, // Pokémon Trainer: Plumeria
            080, // Elite Four: Kahili
            081, // Pokémon Trainer: [~ 157]
            082, // Aether President: Lusamine
            083, // Pokémon Trainer: Red
            084, // Pokémon Trainer: Blue
            085, // Pokémon Trainer: Sina
            086, // Pokémon Trainer: Dexio
            088, // Pokémon Trainer: Anabel
            092, // Pro Wrestler: The Royal
            093, // Pokémon Trainer: Molayne
            099, // Pokémon Trainer: Molayne
            100, // Pokémon Trainer: Hau
            101, // Pokémon Trainer: Hau
            102, // Pokémon Trainer: Gladion
            103, // Pokémon Trainer: Gladion
            107, // Elite Four: Acerola
            109, // Elite Four: Hala
            110, // Elite Four: Olivia
            111, // Pokémon Professor: Kukui
            139, // GAME FREAK: Morimoto
            140, // Pokémon Trainer: Guzma
            141, // Island Kahuna: Nanu
            142, // Captain: Sophocles
            143, // Pokémon Trainer: Ryuki
            153, // Captain: Mina
            162, // Aether Foundation: Faba
            164, // Island Kahuna: Hapu
            165, // Pokémon Professor: Kukui
            185, // Aether Foundation: Faba
            186, // Pokémon Trainer: Sophocles
            187, // Pokémon Trainer: Giovanni
            188, // Pokémon Trainer: Kukui
            189, // Pokémon Trainer: Lillie
            190, // Pokémon Trainer: Giovanni
            191, // Elite Four: Molayne
            192, // Ultra Recon Squad: Soliera
            193, // Ultra Recon Squad: Dulse
            194, // Pokémon Trainer: Hau
            198, // Team Aqua: Archie
            199, // Team Galactic: Cyrus
            200, // Team Magma: Maxie
            201, // Team Plasma: Ghetsis
            202, // Team Flare: Lysandre
            204, // Kantonian Gym: Leader
            205, // GAME FREAK: Iwao
            206, // Team Rainbow Rocket: Giovanni
            207, // Pokémon Trainer: Lillie
            219, // Pokémon Trainer: Guzma
            220, // Aether President: Lusamine
            221, // Pokémon Trainer: Hau
            222, // Pokémon Trainer: Hau
        };

        public static readonly int[] Model_XY =
        {
            018, // Team Flare (Aliana)
            019, // Team Flare (Bryony)
            020, // Team Flare (Celosia)
            021, // Team Flare (Mable)
            022, // Team Flare (Xerosic)
            055, // Pokémon Trainer (Shauna)
            056, // Pokémon Trainer (Tierno)
            057, // Pokémon Trainer (Trevor)
            077, // Team Flare (Admin)
            078, // Team Flare (Admin)
            079, // Team Flare (Grunt)
            080, // Team Flare (Grunt)
            081, // Team Flare (Lysandre)
            102, // Pokémon Trainer (AZ)
            103, // Pokémon Trainer (Calem)
            104, // Pokémon Trainer (Serena)
            105, // Pokémon Profoessor (Sycamore)
            175, // Team Flare Boss (Lysandre)
        };
        public static readonly int[] Model_AO =
        {
            127, // Pokémon Trainer (Brendan)
            128, // Pokémon Trainer (May)
            174, // Aqua Leader (Archie)
            178, // Magma Leader (Maxie)
            192, // Pokémon Trainer (Wally)
            219, // Pokémon Trainer (Steven)
            221, // Lorekeeper (Zinnia)
            267, // Pokémon Trainer (Zinnia)
            272, // Pokémon Trainer (Wally)
        };
        public static readonly int[] Ignore_AO =
        {
            001, 002, 003, 004, 005, 006, 007, 008, 009, 010, 011, 012, 013, 014, 015, 016, 017, 018, 019, 020, 021, 022, 023, 024, 025, 026, 027, 028, 029, 030, 031, 032, 033, 034, 035, 036, 037,
            038, 039, 040, 041, 042, 043, 044, 045, 046, 047, 048, 049, 050, 051, 052, 053, 054, 055, 056, 057, 058, 059, 060, 061, 062, 063, 064, 065, 066, 067, 068, 069, 070, 071, 072, 073, 074,
            075, 076, 077, 078, 079, 080, 081, 082, 083, 084, 085, 086, 087, 088, 089, 090, 091, 092, 093, 094, 095, 096, 097, 098, 099, 100, 101, 102, 103, 104, 105, 106, 107, 108, 109, 110, 111,
            112, 113, 114, 115, 116, 117, 118, 119, 120, 121, 122, 123, 124, 125, 126
        };
        public static readonly int[] Z_Moves =
        {
            622, 623, 624, 625, 626, 627, 628, 629, 630, 631, 632, 633, 634, 635, 636, 637, 638, 639, 640, 641, 642, 643, 644, 645, 646, 647, 648, 649, 650, 651, 652, 653, 654, 655, 656, 657, 658,
            695, 696, 697, 698, 699, 700, 701, 702, 703,
            719,
            723, 724, 725, 726, 727, 728
        };
    }
}
