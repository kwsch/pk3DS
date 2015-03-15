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
        internal static readonly int[] RandomSpeciesList = Enumerable.Range(1, 721).Select(i => i).ToArray();

        internal static int[] getSpeciesList(bool G1, bool G2, bool G3, bool G4, bool G5, bool G6, bool L, bool E)
        {
            int[] sL = new int[0];

            // Gen 1
            if (G1) sL = sL.Concat(Enumerable.Range(1, 143)).Concat(Enumerable.Range(147, 3)).ToArray(); // Bulbasaur - Snorlax & Dratini - Dragonite
            if (G1 && L) sL = sL.Concat(Enumerable.Range(144, 3)).Concat(Enumerable.Range(150, 1)).ToArray(); // Birds & Mewtwo
            if (G1 && E) sL = sL.Concat(Enumerable.Range(151, 1)).ToArray(); // Mew
            // Gen 2
            if (G2) sL = sL.Concat(Enumerable.Range(152, 91)).Concat(Enumerable.Range(248, 3)).ToArray(); // Chikorita - Blissey & Larvitar - Tyranitar
            if (G2 && L) sL = sL.Concat(Enumerable.Range(243, 3)).Concat(Enumerable.Range(249, 2)).ToArray(); // Dogs, Lugia & Ho-Oh
            if (G2 && E) sL = sL.Concat(Enumerable.Range(251, 1)).ToArray(); // Celebi
            // Gen 3
            if (G3) sL = sL.Concat(Enumerable.Range(252, 125)).ToArray();
            if (G3 && L) sL = sL.Concat(Enumerable.Range(337, 8)).ToArray(); // Regi, Lati, Mascot
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
    }
}
