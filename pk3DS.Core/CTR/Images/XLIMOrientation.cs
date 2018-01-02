using System;

namespace pk3DS.Core.CTR
{
    [Flags]
    public enum XLIMOrientation : byte
    {
        None = 0,
        Rotate90 = 4,
        Transpose = 8,
    }
}