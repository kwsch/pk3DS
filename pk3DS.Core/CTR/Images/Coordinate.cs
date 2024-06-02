namespace pk3DS.Core.CTR;

public struct Coordinate(uint x, uint y)
{
    public uint X { get; private set; } = x;
    public uint Y { get; private set; } = y;

    public void Transpose() => (X, Y) = (Y, X);

    public void Rotate90(uint height)
    {
        var tmp = X;
        X = Y;
        Y = height - 1 - tmp;
    }
}