namespace pk3DS.Core.CTR
{
    public struct Coordinate
    {
        public uint X { get; private set; }
        public uint Y { get; private set; }

        public Coordinate(uint x, uint y)
        {
            X = x; Y = y;
        }

        public void Transpose()
        {
            var tmp = X;
            X = Y;
            Y = tmp;
        }

        public void Rotate90(uint height)
        {
            var tmp = X;
            X = Y;
            Y = height - 1 - tmp;
        }
    }
}