using System.Collections.Generic;
using System.Linq;

namespace pk3DS.Core
{
    public static partial class Legal
    {
        public static readonly ushort[] Mega_XY =
        {
            003, 006, 009, 065, 080, 115, 127, 130, 142, 150,
            181, 212, 214, 229, 248,
            257, 282, 303, 306, 308, 310, 354, 359, 380, 381,
            445, 448, 460
        };

        public static readonly ushort[] Mega_ORAS = Mega_XY.Concat(new ushort[]
        {
            015, 018, 094,
            208,
            254, 260, 302, 319, 323, 334, 362, 373, 376, 384,
            428, 475,
            531,
            719
        }).ToArray();

        public static readonly int[] SpecialClasses_XY =
        {
            #region Classes
            000, // Pokémon Trainer
            001, // Pokémon Trainer
            004, // Leader
            018, // Team Flare
            019, // Team Flare
            020, // Team Flare
            021, // Team Flare
            022, // Team Flare
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
            081, // Team Flare
            102, // Pokémon Trainer
            103, // Pokémon Trainer
            104, // Pokémon Trainer
            105, // Pokémon Professor
            139, // Marchioness
            140, // Marquis
            141, // Marchioness
            142, // Marquis
            143, // Marquis
            144, // Marchioness
            145, // Marchioness
            146, // Marquis
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
            064, // Battle Chatelaine
            065, // Battle Chatelaine
            066, // Battle Chatelaine
            067, // Battle Chatelaine
            127, // Pokémon Trainer
            128, // Pokémon Trainer
            174, // Aqua Leader
            175, // Aqua Admin
            178, // Magma Leader
            180, // Magma Admin
            182, // Magma Admin
            186, // Aqua Admin
            187, // Magma Admin
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
            221, // Lorekeeper
            232, // Pokémon Trainer
            233, // Pokémon Trainer
            234, // Pokémon Trainer
            236, // Secret Base Expert
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
            #region Classes
            000, // Pokémon Trainer
            001, // Pokémon Trainer
            030, // Pokémon Trainer
            031, // Island Kahuna
            038, // Captain
            040, // Pokémon Trainer
            041, // Pokémon Trainer
            043, // Captain
            044, // Captain
            045, // Captain
            046, // Captain
            047, // Captain
            048, // Captain
            049, // Island Kahuna
            050, // Island Kahuna
            051, // Island Kahuna
            070, // Team Skull
            071, // Aether President
            072, // Aether Branch Chief
            076, // Team Skull Boss
            077, // Pokémon Trainer
            078, // Team Skull Admin
            079, // Pokémon Trainer
            080, // Elite Four
            081, // Pokémon Trainer
            082, // Aether President
            083, // Pokémon Trainer
            084, // Pokémon Trainer
            085, // Pokémon Trainer
            086, // Pokémon Trainer
            087, // Pokémon Trainer
            088, // Pokémon Trainer
            089, // Pokémon Trainer
            090, // Pokémon Trainer
            091, // Pokémon Trainer
            092, // Pro Wrestler
            093, // Pokémon Trainer
            097, // Pokémon Trainer
            098, // Pokémon Trainer
            099, // Pokémon Trainer
            100, // Pokémon Trainer
            101, // Pokémon Trainer
            102, // Pokémon Trainer
            103, // Pokémon Trainer
            104, // Pokémon Trainer
            105, // Pokémon Trainer
            106, // Pokémon Trainer
            107, // Elite Four
            108, // Pokémon Trainer
            109, // Elite Four
            110, // Elite Four
            111, // Pokémon Professor
            128, // Pokémon Trainer
            139, // GAME FREAK
            140, // Pokémon Trainer
            141, // Island Kahuna
            142, // Captain
            143, // Pokémon Trainer
            150, // Pokémon Trainer
            151, // Captain
            152, // Captain
            153, // Captain
            154, // Pokémon Professor
            164, // Island Kahuna
            166, // Pokémon Trainer
            167, // Pokémon Trainer
            168, // Pokémon Trainer
            169, // Pokémon Trainer
            170, // Pokémon Trainer
            171, // Pokémon Trainer
            165, // Pokémon Professor
            183, // Battle Legend
            184, // Battle Legend
            185, // Aether Foundation
            #endregion
        };

        public static readonly int[] SpecialClasses_USUM =
        {
            #region Classes
            000, // Pokémon Trainer
            001, // Pokémon Trainer
            030, // Pokémon Trainer
            031, // Island Kahuna
            038, // Captain
            040, // Pokémon Trainer
            041, // Pokémon Trainer
            043, // Captain
            044, // Captain
            045, // Captain
            046, // Captain
            047, // Captain
            048, // Captain
            049, // Island Kahuna
            050, // Island Kahuna
            051, // Island Kahuna
            070, // Team Skull
            071, // Aether President
            072, // Aether Branch Chief
            076, // Team Skull Boss
            077, // Pokémon Trainer
            078, // Team Skull Admin
            079, // Pokémon Trainer
            080, // Elite Four
            081, // Pokémon Trainer
            082, // Aether President
            083, // Pokémon Trainer
            084, // Pokémon Trainer
            085, // Pokémon Trainer
            086, // Pokémon Trainer
            087, // Pokémon Trainer
            088, // Pokémon Trainer
            089, // Pokémon Trainer
            090, // Pokémon Trainer
            091, // Pokémon Trainer
            092, // Pro Wrestler
            093, // Pokémon Trainer
            097, // Pokémon Trainer
            098, // Pokémon Trainer
            099, // Pokémon Trainer
            100, // Pokémon Trainer
            101, // Pokémon Trainer
            102, // Pokémon Trainer
            103, // Pokémon Trainer
            104, // Pokémon Trainer
            105, // Pokémon Trainer
            106, // Pokémon Trainer
            107, // Elite Four
            108, // Pokémon Trainer
            109, // Elite Four
            110, // Elite Four
            111, // Pokémon Professor
            128, // Pokémon Trainer
            139, // GAME FREAK
            140, // Pokémon Trainer
            141, // Island Kahuna
            142, // Captain
            143, // Pokémon Trainer
            150, // Pokémon Trainer
            151, // Captain
            152, // Captain
            153, // Captain
            154, // Pokémon Professor
            164, // Island Kahuna
            166, // Pokémon Trainer
            167, // Pokémon Trainer
            168, // Pokémon Trainer
            169, // Pokémon Trainer
            170, // Pokémon Trainer
            171, // Pokémon Trainer
            165, // Pokémon Professor
            183, // Battle Legend
            184, // Battle Legend
            185, // Aether Foundation
            186, // Pokémon Trainer
            187, // Pokémon Trainer
            188, // Pokémon Trainer
            189, // Pokémon Trainer
            190, // Pokémon Trainer
            191, // Elite Four
            192, // Ultra Recon Squad
            193, // Ultra Recon Squad
            194, // Pokémon Trainer
            198, // Team Aqua
            199, // Team Galactic
            200, // Team Magma
            201, // Team Plasma
            202, // Team Flare
            205, // GAME FREAK
            206, // Team Rainbow Rocket
            207, // Pokémon Trainer
            219, // Pokémon Trainer
            220, // Aether President
            221, // Pokémon Trainer
            222, // Pokémon Trainer
            #endregion
        };

        public static readonly int[] Model_XY =
        {
            #region Models
            018, // Aliana
            019, // Bryony
            020, // Celosia
            021, // Mable
            022, // Xerosic
            055, // Shauna
            056, // Tierno
            057, // Trevor
            081, // Lysandre
            102, // AZ
            103, // Calem
            104, // Serena
            105, // Sycamore
            175, // Lysandre (Mega Ring)
            #endregion
        };

        public static readonly int[] Model_AO =
        {
            #region Models
            127, // Brendan
            128, // May
            174, // Archie
            178, // Maxie
            192, // Wally
            198, // Steven
            219, // Steven (Multi Battle)
            221, // Zinnia (Lorekeeper)
            267, // Zinnia
            272, // Wally (Mega Pendant)
            277, // Steven (Rematch)
            278, // Brendan (Mega Bracelet)
            279, // May (Mega Bracelet)
            #endregion
        };

        public static readonly int[] Z_Moves =
        {
            622, 623, 624, 625, 626, 627, 628, 629, 630, 631, 632, 633, 634, 635, 636, 637, 638, 639, 640, 641, 642, 643, 644, 645, 646, 647, 648, 649, 650, 651, 652, 653, 654, 655, 656, 657, 658,
            695, 696, 697, 698, 699, 700, 701, 702, 703, 719, 723, 724, 725, 726, 727, 728
        };

        public static readonly int[] ImportantTrainers_SM =
        {
            012, 013, 014, 023, 052, 074, 075, 076, 077, 078, 079, 089, 090, 129, 131, 132, 138, 144, 146, 149, 152, 153, 154, 155, 156, 158, 159, 160, 164, 167, 215, 216, 217, 218, 219, 220, 221,
            222, 235, 236, 238, 239, 240, 241, 349, 350, 351, 352, 356, 357, 358, 359, 360, 392, 396, 398, 400, 401, 403, 405, 409, 410, 412, 413, 414, 415, 416, 417, 418, 419, 435, 438, 439, 440,
            441, 447, 448, 449, 450, 451, 452, 467, 477, 478, 479, 480, 481, 482, 483, 484
        };

        public static readonly int[] ImportantTrainers_USUM =
        {
            012, 013, 014, 023, 052, 074, 075, 076, 077, 078, 079, 089, 090, 131, 132, 138, 144, 146, 149, 153, 154, 156, 159, 160, 215, 216, 217, 218, 219, 220, 221, 222, 235, 236, 238, 239, 240,
            241, 350, 351, 352, 356, 358, 359, 396, 398, 401, 405, 409, 410, 412, 415, 416, 417, 418, 419, 438, 439, 440, 441, 447, 448, 449, 450, 451, 452, 477, 478, 479, 480, 489, 490, 494, 495,
            496, 497, 498, 499, 500, 501, 502, 503, 504, 505, 506, 507, 508, 541, 542, 543, 555, 556, 557, 558, 559, 560, 561, 562, 572, 573, 578, 580, 582, 583, 623, 630, 644, 645, 647, 648, 649,
            650, 651, 652
        };

        public static readonly int[][] BasicStarters = {
            new[] {001, 004, 007, 010, 013, 016, 029, 032, 041, 043, 060, 063, 066, 069, 074, 081, 092, 111, 116, 137, 147},
            new[] {152, 155, 158, 172, 173, 174, 175, 179, 187, 220, 239, 240, 246},
            new[] {252, 255, 258, 265, 270, 273, 280, 287, 293, 298, 304, 328, 355, 363, 371, 374},
            new[] {387, 390, 393, 396, 403, 406, 440, 443},
            new[] {495, 498, 501, 506, 519, 524, 532, 535, 540, 543, 551, 574, 577, 582, 599, 602, 607, 610, 633},
            new[] {650, 653, 656, 661, 664, 669, 679, 704},
            new[] {722, 725, 728, 731, 736, 761, 782, 789},
        };

        public static readonly int[] BasicStarters_6 = BasicStarters[0]
            .Concat(BasicStarters[1])
            .Concat(BasicStarters[2])
            .Concat(BasicStarters[3])
            .Concat(BasicStarters[4])
            .Concat(BasicStarters[5])
            .ToArray();

        public static readonly int[] BasicStarters_7 = BasicStarters_6.Concat(BasicStarters[6]).ToArray();

        public static readonly int[] FinalEvolutions_6 =
        {
            003, 006, 009, 012, 015, 018, 020, 022, 024, 026, 028, 031, 034, 036, 038, 040, 045, 047, 049, 051, 053, 055, 057, 059, 062, 065, 068, 071, 073, 076, 078, 080, 083, 085, 087, 089, 091,
            094, 097, 099, 101, 103, 105, 106, 107, 110, 115, 119, 121, 122, 124, 127, 128, 130, 131, 132, 134, 135, 136, 139, 141, 142, 143, 149, 154, 157, 160, 162, 164, 166, 168, 169, 171, 178,
            181, 182, 184, 185, 186, 189, 192, 195, 196, 197, 199, 201, 202, 203, 205, 206, 208, 210, 211, 212, 213, 214, 217, 219, 222, 224, 225, 226, 227, 229, 230, 232, 234, 235, 237, 241, 242,
            248, 254, 257, 260, 262, 264, 267, 269, 272, 275, 277, 279, 282, 284, 286, 289, 291, 292, 295, 297, 301, 302, 303, 306, 308, 310, 311, 312, 313, 314, 317, 319, 321, 323, 324, 326, 327,
            330, 332, 334, 335, 336, 337, 338, 340, 342, 344, 346, 348, 350, 351, 352, 354, 357, 358, 359, 362, 365, 367, 368, 369, 370, 373, 376, 389, 392, 395, 398, 400, 402, 405, 407, 409, 411,
            413, 414, 416, 417, 419, 421, 423, 424, 426, 428, 429, 430, 432, 435, 437, 441, 442, 445, 448, 450, 452, 454, 455, 457, 460, 461, 462, 463, 464, 465, 466, 467, 468, 469, 470, 471, 472,
            473, 474, 475, 476, 477, 478, 479, 497, 500, 503, 505, 508, 510, 512, 514, 516, 518, 521, 523, 526, 528, 530, 531, 534, 537, 538, 539, 542, 545, 547, 549, 550, 553, 555, 556, 558, 560,
            561, 563, 565, 567, 569, 571, 573, 576, 579, 581, 584, 586, 587, 589, 591, 593, 594, 596, 598, 601, 604, 606, 609, 612, 614, 615, 617, 618, 620, 621, 623, 625, 626, 628, 630, 631, 632,
            635, 637, 652, 655, 658, 660, 663, 666, 668, 671, 673, 675, 676, 678, 681, 683, 685, 687, 689, 691, 693, 695, 697, 699, 700, 701, 702, 703, 706, 707, 709, 711, 713, 715,
        };

        public static readonly int[] FinalEvolutions_7 = FinalEvolutions_6.Concat(new[]
        {
            724, 727, 730, 733, 735, 738, 740, 741, 743, 745, 746, 748, 750, 752, 754, 756, 758, 760, 763, 764, 765, 766, 768, 770, 771, 774, 775, 776, 777, 779, 780, 781, 784,
        }).ToArray();

        public static readonly int[] Legendary_6 =
        {
            #region Legendary
            144, // Articuno
            145, // Zapdos
            146, // Moltres
            150, // Mewtwo
            243, // Raikou
            244, // Entei
            245, // Suicune
            249, // Lugia
            250, // Ho-Oh
            377, // Regirock
            378, // Regice
            379, // Registeel
            380, // Latias
            381, // Latios
            382, // Kyogre
            383, // Groudon
            384, // Rayquaza
            480, // Uxie
            481, // Mesprit
            482, // Azelf
            483, // Dialga
            484, // Palkia
            485, // Heatran
            486, // Regigigas
            487, // Giratina
            488, // Cresselia
            638, // Cobalion
            639, // Terrakion
            640, // Virizion
            641, // Tornadus
            642, // Thundurus
            643, // Reshiram
            644, // Zekrom
            645, // Landorus
            646, // Kyurem
            716, // Xerneas
            717, // Yveltal
            718, // Zygarde
            #endregion
        };

        public static readonly int[] Legendary_SM = Legendary_6.Concat(new[]
        {
            #region Legendary
            773, // Silvally
            785, // Tapu Koko
            786, // Tapu Lele
            787, // Tapu Bulu
            788, // Tapu Fini
            791, // Solgaleo
            792, // Lunala
            793, // Nihilego
            794, // Buzzwole
            795, // Pheromosa
            796, // Xurkitree
            797, // Celesteela
            798, // Kartana
            799, // Guzzlord
            800, // Necrozma
            #endregion
        }).ToArray();

        public static readonly int[] Legendary_USUM = Legendary_SM.Concat(new[]
        {
            #region Legendary
            804, // Naganadel
            805, // Stakataka
            806, // Blacephalon
            #endregion
        }).ToArray();

        public static readonly int[] Mythical_6 =
        {
            #region Mythical
            151, // Mew
            251, // Celebi
            385, // Jirachi
            386, // Deoxys
            489, // Phione
            490, // Manaphy
            491, // Darkrai
            492, // Shaymin
            493, // Arceus
            494, // Victini
            647, // Keldeo
            648, // Meloetta
            649, // Genesect
            719, // Diancie
            720, // Hoopa
            721, // Volcanion
            #endregion
        };

        public static readonly int[] Mythical_SM = Mythical_6.Concat(new[]
        {
            #region Mythical
            801, // Magearna
            802, // Marshadow
            #endregion
        }).ToArray();

        public static readonly int[] Mythical_USUM = Mythical_SM.Concat(new[]
        {
            #region Mythical
            807, // Zeraora
            #endregion
        }).ToArray();

        public static readonly HashSet<int> BattleForms = new()
        {
            351, // Castform
            421, // Cherrim
            555, // Darmanitan
            648, // Meloetta
            681, // Aegislash
            716, // Xerneas
            746, // Wishiwashi
            778, // Mimikyu
        };

        public static readonly HashSet<int> BattleMegas = new()
        {
            // XY
            003, 006, 009, 065, 080, 115, 127, 130, 142, 150,
            181, 212, 214, 229, 248,
            257, 282, 303, 306, 308, 310, 354, 359, 380, 381,
            445, 448, 460,

            // AO
            015, 018, 094,
            208,
            254, 260, 302, 319, 323, 334, 362, 373, 376, 384,
            428, 475,
            531,
            719,

            // USUM
            800, // Ultra Necrozma
        };

        public static readonly HashSet<int> BattlePrimals = new() { 382, 383 };
        public static HashSet<int> BattleExclusiveForms = new(BattleForms.Concat(BattleMegas.Concat(BattlePrimals)));
    }
}
