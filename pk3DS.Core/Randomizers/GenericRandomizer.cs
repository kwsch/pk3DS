namespace pk3DS.Core.Randomizers
{
    /// <summary> Cyclical Shuffled Randomizer </summary>
    /// <remarks>
    /// The shuffled list is iterated over, and reshuffled when exhausted.
    /// The list does not repeat values until the list is exhausted.
    /// </remarks>
    /// 
    using System.Diagnostics;
    public class GenericRandomizer
    {
        public GenericRandomizer(int[] randomValues)
        {
            RandomValues = randomValues;
        }

        private readonly int[] RandomValues;
        private int ctr;

        public void Reset()
        {
            ctr = 0;
            Util.Shuffle(RandomValues);
        }

        public int Next()
        {
            if (ctr == 0)
            {
                Util.Shuffle(RandomValues);
                Debug.Print("Shuffling list.  Length = " + RandomValues.Length);
            }

            int value = RandomValues[ctr++];
            ctr %= RandomValues.Length;
            return value;
        }
    }
}
