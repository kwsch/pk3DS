namespace pk3DS.Core.Randomizers;

/// <summary> Cyclical Shuffled Randomizer </summary>
/// <remarks>
/// The shuffled list is iterated over, and reshuffled when exhausted.
/// The list does not repeat values until the list is exhausted.
/// </remarks>
public class GenericRandomizer(int[] randomValues)
{
    private int ctr;

    public void Reset()
    {
            ctr = 0;
            Util.Shuffle(randomValues);
        }

    public int Next()
    {
            if (ctr == 0)
                Util.Shuffle(randomValues);

            int value = randomValues[ctr++];
            ctr %= randomValues.Length;
            return value;
        }
}