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

    public static class XlimOrientationExtensions
    {
        public static bool HasFlagFast(this XLIMOrientation value, XLIMOrientation flag)
        {
            return (value & flag) != 0;
        }
    }
}