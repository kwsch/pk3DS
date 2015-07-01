using System.Linq;

namespace pk3DS
{
    class Legal
    {
        internal static readonly ushort[] Pouch_Items_XY = { 
                000,001,002,003,004,005,006,007,008,009,010,011,012,013,014,015,016,055,056,
                057,058,059,060,061,062,063,064,065,066,067,068,069,070,071,072,073,074,075,
                076,077,078,079,080,081,082,083,084,085,086,087,088,089,090,091,092,093,094,
                099,100,101,102,103,104,105,106,107,108,109,110,112,116,117,118,119,135,136,
                213,214,215,217,218,219,220,221,222,223,224,225,226,227,228,229,230,231,232,
                233,234,235,236,237,238,239,240,241,242,243,244,245,246,247,248,249,250,251,
                252,253,254,255,256,257,258,259,260,261,262,263,264,265,266,267,268,269,270,
                271,272,273,274,275,276,277,278,279,280,281,282,283,284,285,286,287,288,289,
                290,291,292,293,294,295,296,297,298,299,300,301,302,303,304,305,306,307,308,
                309,310,311,312,313,314,315,316,317,318,319,320,321,322,323,324,325,326,327,
                504,                                537,538,539,540,541,542,543,544,545,546,
                547,548,549,550,551,552,553,554,555,556,557,558,559,560,561,562,563,564,571,
                572,573,576,577,580,581,582,583,584,585,586,587,588,589,590,639,640,644,646,
                647,648,649,650,652,653,654,655,656,657,658,659,660,661,662,663,664,665,666,
                667,668,669,670,671,672,673,674,675,676,677,678,679,680,681,682,683,684,685,
                699,704,710,711,715,
        };
        internal static readonly ushort[] Pouch_Items_ORAS = Pouch_Items_XY.Concat(new ushort[] {
                534,535, 
                        752,753,754,755,756,757,758,759,
                760,761,762,763,764,        767,768,769,
                770,
        }).ToArray();
        internal static readonly ushort[] Pouch_Key_XY = {
                000,216,431,442,445,446,447,450,465,466,471,628,
                629,631,632,638,641,642,643,689,695,696,697,698,
                700,701,702,703,705,706,707,712,713,714,
                
                // Illegal
                716,717, // For the cheaters who want useless items... 
        };
        internal static readonly ushort[] Pouch_Key_ORAS = {
                000,216,        445,446,447,    465,466,471,628,
                629,631,632,638,                        697,

                // Illegal
                716,717,745,746,747,748,749,750, // For the cheaters who want useless items...

                // ORAS
                457,474,503,

                718,719,
                720,721,722,723,724,725,726,727,728,729,
                730,731,732,733,734,735,736,    738,739,
                740,741,742,743,744,
                751,765,766,771,772,774,775,
        };
        internal static readonly ushort[] Pouch_TMHM_XY = {
            0,
            328,329,330,331,332,333,334,335,336,337,338,339,340,341,342,343,344,345,
            346,347,348,349,350,351,352,353,354,355,356,357,358,359,360,361,362,363,
            364,365,366,367,368,369,370,371,372,373,374,375,376,377,378,379,380,381,
            382,383,384,385,386,387,388,389,390,391,392,393,394,395,396,397,398,399,
            400,401,402,403,404,405,406,407,408,409,410,411,412,413,414,415,416,417,
            418,419,618,619,620,690,691,692,693,694,
                
            420,421,422,423,424,
        };
        internal static readonly ushort[] Pouch_TMHM_ORAS = Pouch_TMHM_XY.Concat(new ushort[] {
            425,737,
        }).ToArray();
        internal static readonly ushort[] Pouch_Medicine_XY = {
            000,017,018,019,020,021,022,023,024,025,026,027,028,029,030,031,032,033,
            034,035,036,037,038,039,040,041,042,043,044,045,046,047,048,049,050,051,
            052,053,054,134,504,565,566,567,568,569,570,571,591,645,708,709,
        };
        internal static readonly ushort[] Pouch_Medicine_ORAS = Pouch_Medicine_XY.Concat(new ushort[] {
            065,066,067
        }).ToArray();
        internal static readonly ushort[] Pouch_Berry_XY = {
            0,149,150,151,152,153,154,155,156,157,158,159,160,161,162,
            163,164,165,166,167,168,169,170,171,172,173,174,175,176,177,
            178,179,180,181,182,183,184,185,186,187,188,189,190,191,192,
            193,194,195,196,197,198,199,200,201,202,203,204,205,206,207,
            208,209,210,211,212,686,687,688,
        };
        internal static readonly ushort[] Mega_XY =
        {
            3,6,9,65,80,
            115,127,130,142,150,181,
            212,214,229,248,282,
            303,306,308,310,354,359,380,381,
            445,448,460,
        };
        internal static readonly ushort[] Mega_ORAS = Mega_XY.Concat(new ushort[]
        {
            15,18,94,
            208,254,257,260,
            302,319,323,334,362,373,376,384,
            428,475,
            531,
            719
        }).ToArray();
        internal static readonly int[] SpecialClasses_XY =
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
        internal static readonly int[] SpecialClasses_ORAS =
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
    }
}
