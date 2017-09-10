namespace pk3DS.Core.Randomizers
{
    /// <summary> Cyclical Shuffled Randomizer </summary>
    /// <remarks> 
    /// The shuffled list is iterated over, and reshuffled when exhausted.
    /// The list does not repeat values until the list is exhausted.
    /// </remarks>
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
                Util.Shuffle(RandomValues);

            int value = RandomValues[ctr++];
            ctr %= RandomValues.Length;
            return value;
        }
    }
}
