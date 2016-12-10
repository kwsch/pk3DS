using System.Linq;

namespace pk3DS
{
    public class Randomizer
    {
        public PersonalInfo[] Stats { private get; set; }
        public bool BST;
        private int[] SpeciesList;
        private int ctr;
        public Randomizer(bool G1, bool G2, bool G3, bool G4, bool G5, bool G6, bool G7, bool L, bool E, bool Shedinja = true)
        {
            SpeciesList = getSpeciesList(G1, G2, G3, G4, G5, G6, G7, L, E, Shedinja);
        }
        public int getRandomSpecies(int oldSpecies, int Type = -1)
        {
            return getRandomSpecies(ref SpeciesList, ref ctr, oldSpecies, BST, Stats, Type);
        }

        internal static int getRandomSpecies(ref int[] list, ref int ctr)
        {
            if (ctr == 0) { Util.Shuffle(list); }
            int species = list[ctr++]; ctr %= list.Length;
            return species;
        }
        internal static int MaxSpeciesID = 721;
        internal static int[] RandomSpeciesList => Enumerable.Range(1, MaxSpeciesID).ToArray();
        internal static int getRandomSpecies(ref int[] sL, ref int ctr, int oldSpecies, bool BST, PersonalInfo[] stats = null, int Type = -1)
        {
            int species = getRandomSpecies(ref sL, ref ctr);
            if (!BST || stats == null)
                return species;

            PersonalInfo oldpkm = stats[oldSpecies];
            PersonalInfo pkm = stats[species];

            // Stat Deviation: increasing 10% increments if no suitable match found in entire list
            int a = 11;
            const int c = 10;
            
            int iter = 1;
            bool valid = pkm.BST*c/a < oldpkm.BST && pkm.BST*a/c > oldpkm.BST;
            if (Type > -1) valid &= pkm.Types.Any(type => type == Type);

            while (!valid)
            {
                species = getRandomSpecies(ref sL, ref ctr);
                pkm = Main.SpeciesStat[species];
                if (++iter % sL.Length == 0)
                    a++;

                // Check Satisfaction
                valid = pkm.BST*c/a < oldpkm.BST && pkm.BST*a/c > oldpkm.BST;
                if (Type > -1) valid &= pkm.Types.Any(type => type == Type);
            }
            return species;
        }

        internal static int[] getSpeciesList(bool G1, bool G2, bool G3, bool G4, bool G5, bool G6, bool G7, bool L, bool E, bool Shedinja = true)
        {
            int[] sL = new int[0];

            // Gen 1
            if (G1) sL = sL.Concat(Enumerable.Range(1, 143)).Concat(Enumerable.Range(147, 3)).ToArray(); // Bulbasaur - Snorlax & Dratini - Dragonite
            if (G1 && L) sL = sL.Concat(Enumerable.Range(144, 3)).Concat(Enumerable.Range(150, 1)).ToArray(); // Birds & Mewtwo
            if (G1 && E) sL = sL.Concat(Enumerable.Range(151, 1)).ToArray(); // Mew
            // Gen 2
            if (G2) sL = sL.Concat(Enumerable.Range(152, 91)).Concat(Enumerable.Range(246, 3)).ToArray(); // Chikorita - Blissey & Larvitar - Tyranitar
            if (G2 && L) sL = sL.Concat(Enumerable.Range(243, 3)).Concat(Enumerable.Range(249, 2)).ToArray(); // Dogs, Lugia & Ho-Oh
            if (G2 && E) sL = sL.Concat(Enumerable.Range(251, 1)).ToArray(); // Celebi
            // Gen 3
            if (G3) sL = sL.Concat(Enumerable.Range(252, 40)).Concat(Enumerable.Range(293, 84)).ToArray();
            if (G3 && Shedinja) sL = sL.Concat(Enumerable.Range(292, 1)).ToArray(); // Shedinja
            if (G3 && L) sL = sL.Concat(Enumerable.Range(377, 8)).ToArray(); // Regi, Lati, Mascot
            if (G3 && E) sL = sL.Concat(Enumerable.Range(385, 2)).ToArray(); // Jirachi/Deoxys
            // Gen 4
            if (G4) sL = sL.Concat(Enumerable.Range(387, 93)).ToArray();
            if (G4 && L) sL = sL.Concat(Enumerable.Range(480, 9)).ToArray(); //
            if (G4 && E) sL = sL.Concat(Enumerable.Range(489, 5)).ToArray(); //
            // Gen 5
            if (G5) sL = sL.Concat(Enumerable.Range(495, 143)).ToArray();
            if (G5 && L) sL = sL.Concat(Enumerable.Range(638, 9)).Concat(Enumerable.Range(494, 1)).ToArray(); // 
            if (G5 && E) sL = sL.Concat(Enumerable.Range(647, 3)).ToArray(); // 
            // Gen 6
            if (G6) sL = sL.Concat(Enumerable.Range(650, 66)).ToArray();
            if (G6 && L) sL = sL.Concat(Enumerable.Range(716, 3)).ToArray(); // 
            if (G6 && E) sL = sL.Concat(Enumerable.Range(719, 3)).ToArray(); // 
            // Gen 7
            if (G7) sL = sL.Concat(Enumerable.Range(722, 67)).ToArray();
            if (G7 && L) sL = sL.Concat(Enumerable.Range(785, 16)).ToArray(); // Tapus, Legends, UBs
            if (G7 && E) sL = sL.Concat(Enumerable.Range(801, 2)).ToArray(); // Magearna, Marshadow

            return sL.Length == 0 ? RandomSpeciesList : sL;
        }
        internal static int GetRandomForme(int species, bool mega, bool alola, PersonalInfo[] stats = null)
        {
            if (stats == null)
                return 0;
            if (stats[species].FormeCount <= 1)
                return 0;
            if (alola && Legal.EvolveToAlolanForms.Contains(species))
                return (int)(Util.rnd32()%2);
            if (!Legal.Mega_ORAS.Contains((ushort)species) || mega)
                return (int)(Util.rnd32() % stats[species].FormeCount); // Slot-Random
            return 0;
        }

        internal static int[] getRandomItemList()
        {
            if (Main.Config.ORAS)
                return Items_HeldAO.Concat(Items_Ball).Where(i => i != 0).ToArray();
            if (Main.Config.XY)
                return Items_HeldXY.Concat(Items_Ball).Where(i => i != 0).ToArray();
            if (Main.Config.SM)
                return HeldItemsBuy_SM.Select(i => (int)i).Concat(Items_Ball).Where(i => i != 0).ToArray();
            return new int[0];
        }
        #region Random Item List
        private static readonly int[] Items_HeldXY =
        {
            /* 000, */ 001, 002, 003, 004, 005, 006, 007, 008, 009, 010, 011, 012, 013, 014, 015, 017, 018, 019, 020, 021, 022,
            023, 024, 025, 026, 027, 028, 029, 030, 031, 032, 033, 034, 035,
            036, 037, 038, 039, 040, 041, 042, 043, 044, 045, 046, 047, 048, 049, 050, 051, 052, 053, 054, 055, 056, 057,
            058, 059, 060, 061, 062, 063, 064, 065, 066, 067, 068, 069, 070,
            071, 072, 073, 074, 075, 076, 077, 078, 079, 080, 081, 082, 083, 084, 085, 086, 087, 088, 089, 090, 091, 092,
            093, 094, 099, 100, 101, 102, 103, 104, 105, 106, 107, 108, 109,
            110, 112, 116, 117, 118, 119, 134, 135, 136, 149, 150, 151, 152, 153, 154, 155, 156, 157, 158, 159, 160, 161,
            162, 163, 164, 165, 166, 167, 168, 169, 170, 171, 172, 173, 174,
            175, 176, 177, 178, 179, 180, 181, 182, 183, 184, 185, 186, 187, 188, 189, 190, 191, 192, 193, 194, 195, 196,
            197, 198, 199, 200, 201, 202, 203, 204, 205, 206, 207, 208, 209,
            210, 211, 212, 213, 214, 215, 217, 218, 219, 220, 221, 222, 223, 224, 225, 226, 227, 228, 229, 230, 231, 232,
            233, 234, 235, 236, 237, 238, 239, 240, 241, 242, 243, 244,
            245, 246, 247, 248, 249, 250, 251, 252, 253, 254, 255, 256, 257, 258, 259, 260, 261, 262, 263, 264, 265, 266,
            267, 268, 269, 270, 271, 272, 273, 274, 275, 276, 277, 278, 279,
            280, 281, 282, 283, 284, 285, 286, 287, 288, 289, 290, 291, 292, 293, 294, 295, 296, 297, 298, 299, 300, 301,
            302, 303, 304, 305, 306, 307, 308, 309, 310, 311, 312, 313, 314,
            315, 316, 317, 318, 319, 320, 321, 322, 323, 324, 325, 326, 327, 504, 537, 538, 539, 540, 541, 542, 543, 544,
            545, 546, 547, 548, 549, 550, 551, 552, 553, 554, 555, 556, 557,
            558, 559, 560, 561, 562, 563, 564, 565, 566, 567, 568, 569, 570, 571, 572, 573, 577, 580, 581, 582, 583, 584,
            585, 586, 587, 588, 589, 590, 591, 639, 640, 644, 645, 646, 647,
            648, 649, 650, 652, 653, 654, 655, 656, 657, 658, 659, 660, 661, 662, 663, 664, 665, 666, 667, 668, 669, 670,
            671, 672, 673, 674, 675, 676, 677, 678, 679, 680, 681, 682, 683,
            684, 685, 686, 687, 688, 699, 704, 708, 709, 710, 711, 715,
        };
        private static readonly int[] Items_HeldAO = Items_HeldXY.Concat(new[]
        {
            534, 535,
            752, 753, 754, 755, 756, 757, 758, 759, 760, 761, 762, 763, 764, 767, 768, 769, 770,
        }).ToArray();
        private static readonly int[] Items_Ball =
        {
            000, 001, 002, 003, 004, 005, 006, 007, 008, 009, 010, 011, 012,
            013, 014, 015, 016, 492, 493, 494, 495, 496, 497, 498, 499, 576,
        };
        #endregion

        #region Gen7

        internal static readonly ushort[] Pouch_Regular_SM = // 00
        {
            068, 069, 070, 071, 072, 073, 074, 075, 076, 077, 078, 079, 080, 081, 082, 083, 084, 085, 086, 087,
            088, 089, 090, 091, 092, 093, 094, 099, 100, 101, 102, 103, 104, 105, 106, 107, 108, 109, 110, 111,
            112, 116, 117, 118, 119, 135, 136, 137, 213, 214, 215, 217, 218, 219, 220, 221, 222, 223, 224, 225,
            226, 227, 228, 229, 230, 231, 232, 233, 234, 235, 236, 237, 238, 239, 240, 241, 242, 243, 244, 245,
            246, 247, 248, 249, 250, 251, 252, 253, 254, 255, 256, 257, 258, 259, 260, 261, 262, 263, 264, 265,
            266, 267, 268, 269, 270, 271, 272, 273, 274, 275, 276, 277, 278, 279, 280, 281, 282, 283, 284, 285,
            286, 287, 288, 289, 290, 291, 292, 293, 294, 295, 296, 297, 298, 299, 300, 301, 302, 303, 304, 305,
            306, 307, 308, 309, 310, 311, 312, 313, 314, 315, 316, 317, 318, 319, 320, 321, 322, 323, 324, 325,
            326, 327, 499, 534, 535, 537, 538, 539, 540, 541, 542, 543, 544, 545, 546, 547, 548, 549, 550, 551,
            552, 553, 554, 555, 556, 557, 558, 559, 560, 561, 562, 563, 564, 571, 572, 573, 580, 581, 582, 583,
            584, 585, 586, 587, 588, 589, 590, 639, 640, 644, 645, 646, 647, 648, 649, 650, 656, 657, 658, 659,
            660, 661, 662, 663, 664, 665, 666, 667, 668, 669, 670, 671, 672, 673, 674, 675, 676, 677, 678, 679,
            680, 681, 682, 683, 684, 685, 699, 704, 710, 711, 715, 752, 753, 754, 755, 756, 757, 758, 759, 760,
            761, 762, 763, 764, 767, 768, 769, 770, 795, 796, 844, 849, 853, 854, 855, 856, 879, 880, 881, 882,
            883, 884, 904, 905, 906, 907, 908, 909, 910, 911, 912, 913, 914, 915, 916, 917, 918, 919, 920,
        };
        internal static readonly ushort[] Pouch_Ball_SM = { // 08
            1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 492, 493, 494, 495, 496, 497, 498, 576,
            851
        };
        internal static readonly ushort[] Pouch_Battle_SM = { // 16
            55, 56, 57, 58, 59, 60, 61, 62, 63, 64, 577,
            846,
        };
        internal static readonly ushort[] Pouch_Items_SM = Pouch_Regular_SM.Concat(Pouch_Ball_SM).Concat(Pouch_Battle_SM).ToArray();

        internal static readonly ushort[] Pouch_Key_SM = {
            216, 465, 466, 628, 629, 631, 632, 633, 638, 696,
            705, 706, 765, 773, 797,
            841, 842, 843, 845, 847, 850, 857, 858, 860,
        };
        internal static readonly ushort[] Pouch_TMHM_SM = { // 02
            328, 329, 330, 331, 332, 333, 334, 335, 336, 337, 338, 339, 340, 341, 342, 343, 344, 345,
            346, 347, 348, 349, 350, 351, 352, 353, 354, 355, 356, 357, 358, 359, 360, 361, 362, 363,
            364, 365, 366, 367, 368, 369, 370, 371, 372, 373, 374, 375, 376, 377, 378, 379, 380, 381,
            382, 383, 384, 385, 386, 387, 388, 389, 390, 391, 392, 393, 394, 395, 396, 397, 398, 399,
            400, 401, 402, 403, 404, 405, 406, 407, 408, 409, 410, 411, 412, 413, 414, 415, 416, 417,
            418, 419, 618, 619, 620, 690, 691, 692, 693, 694,
        };
        internal static readonly ushort[] Pouch_Medicine_SM = { // 32
            17, 18, 19, 20, 21, 22, 23, 24, 25, 26, 27, 28, 29, 30, 31, 32, 33, 34, 35, 36, 37, 38, 39, 40, 41, 42, 43, 44, 45, 46, 47, 48, 49, 50, 51, 52, 53, 54, 65, 66, 67, 134,
            504, 565, 566, 567, 568, 569, 570, 591, 708, 709,
            852,
        };
        internal static readonly ushort[] Pouch_Berries_SM = {
            149, 150, 151, 152, 153, 154, 155, 156, 157, 158, 159, 160, 161, 162, 163, 164, 165, 166, 167, 168, 169, 170, 171, 172, 173, 174, 175, 176, 177, 178, 179, 180, 181, 182, 183, 184, 185, 186, 187, 188, 189, 190, 191, 192, 193, 194, 195, 196, 197, 198, 199, 200, 201, 202, 203, 204, 205, 206, 207, 208, 209, 210, 211, 212,
            686, 687, 688,
        };
        internal static readonly ushort[] Pouch_ZCrystal_SM = { // Bead
            807, 808, 809, 810, 811, 812, 813, 814, 815, 816, 817, 818, 819, 820, 821, 822, 823, 824, 825, 826, 827, 828, 829, 830, 831, 832, 833, 834, 835,
        };
        internal static readonly ushort[] Pouch_ZCrystalHeld_SM = { // Piece
            776, 777, 778, 779, 780, 781, 782, 783, 784, 785, 786, 787, 788, 789, 790, 791, 792, 793, 794, 798, 799, 800, 801, 802, 803, 804, 805, 806, 836
        };
        internal static readonly ushort[] HeldItems_SM = new ushort[1].Concat(Pouch_Items_SM).Concat(Pouch_Berries_SM).Concat(Pouch_Medicine_SM).Concat(Pouch_ZCrystalHeld_SM).ToArray();
        internal static readonly ushort[] HeldItemsBuy_SM = new ushort[1].Concat(Pouch_Items_SM).Concat(Pouch_Medicine_SM).ToArray();
        #endregion

        internal static int[] getRandomMoves(int[] Types, Move[] moveData, bool rDMG, int rDMGCount, bool rSTAB, int rSTABCount)
        {
            int maxmove = Main.Config.XY ? 617 
                : Main.Config.ORAS ? 620 
                : 718; // SM
            maxmove += 1;
            int[] moves = new int[4];
            int loopctr = 0;
            const int maxLoop = 666;

            getMoves:
            switch (Main.Config.Generation)
            {
                case 6:
                    for (int i = 0; i < 4; i++)
                        moves[i] = (int)(Util.rnd32() % maxmove);
                    break;
                    
                case 7:
                    int m = 0;
                    while (m != 4)
                    {
                        moves[m] = (int)(Util.rnd32() % maxmove);
                        if (!Legal.Z_Moves.Contains(moves[m]))
                            m++; // Valid
                    }
                    break;

                default:
                    return moves;
            }
            if (loopctr++ < maxLoop || ScreenMoves(moves, Types, moveData, rDMG, rDMGCount, rSTAB, rSTABCount))
                return moves;
            goto getMoves;
        }

        private static bool ScreenMoves(int[] moves, int[] Types, Move[] moveData, bool rDMG, int rDMGCount, bool rSTAB, int rSTABCount)
        {
            if (rDMG && rDMGCount > moves.Count(move => moveData[move].Category != 0))
                return false;
            if (rSTAB && rSTABCount > moves.Count(move => Types.Contains(moveData[move].Type)))
                return false;

            return true;
        }
    }
}
