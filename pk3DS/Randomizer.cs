using System.Linq;

namespace pk3DS
{
    class Randomizer
    {
        internal static int getRandomSpecies(ref int[] list, ref int ctr)
        {
            if (ctr == 0) { Util.Shuffle(list); }
            int species = list[ctr++]; ctr %= list.Length;
            return species;
        }
        internal static readonly int[] RandomSpeciesList = Enumerable.Range(1, 721).Select(i => (int)i).ToArray();
    }
}
