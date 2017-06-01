namespace pk3DS.Core
{
    public abstract class LCRNG32
    {
        public static uint Advance(uint seed, int ctr)
        {
            for (int i = 0; i < ctr; i++)
            {
                seed *= 0x41C64E6D;
                seed += 0x00006073;
            }
            return seed;
        }
        public static uint Reverse(uint seed, int ctr)
        {
            for (int i = 0; i < ctr; i++)
            {
                seed *= 0xEEB9EB65;
                seed += 0x0A3561A1;
            }
            return seed;
        }
    }
}
