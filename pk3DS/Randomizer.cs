using System.Linq;

namespace pk3DS
{
    public class Randomizer
    {
        internal static int getRandomSpecies(ref int[] list, ref int ctr)
        {
            if (ctr == 0) { Util.Shuffle(list); }
            int species = list[ctr++]; ctr %= list.Length;
            return species;
        }
        internal static readonly int[] RandomSpeciesList = Enumerable.Range(1, 721).ToArray();

        internal static int[] getSpeciesList(bool G1, bool G2, bool G3, bool G4, bool G5, bool G6, bool L, bool E, bool Shedinja = true)
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

            return sL.Length == 0 ? RandomSpeciesList : sL;
        }

        internal static int[] getRandomItemList(bool oras)
        {
            return (Main.oras ? Items_HeldAO : Items_HeldXY).Concat(Items_Ball).ToArray();
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
    }
}
