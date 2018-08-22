using System;

namespace pk3DS.Core.Structures
{
    [Flags]
    public enum MoveFlag6 : uint
    {
        None,

        F01 = 1u << 00,
        F02 = 1u << 01,
        F03 = 1u << 02,
        F04 = 1u << 03,
        F05 = 1u << 04,
        F06 = 1u << 05,
        F07 = 1u << 06,
        F08 = 1u << 07,

        F09 = 1u << 08,
        F10 = 1u << 09,
        F11 = 1u << 10,
        F12 = 1u << 11,
        F13 = 1u << 12,
        F14 = 1u << 13,
        F15 = 1u << 14,
        F16 = 1u << 15,

        F17 = 1u << 16,
        F18 = 1u << 17,
        F19 = 1u << 18,
        F20 = 1u << 19,
        F21 = 1u << 20,
        F22 = 1u << 21,
        F23 = 1u << 22,
        F24 = 1u << 23,

        F25 = 1u << 24,
        F26 = 1u << 25,
        F27 = 1u << 26,
        F28 = 1u << 27,
        F29 = 1u << 28,
        F30 = 1u << 29,
        F31 = 1u << 30,
        F32 = 1u << 31,
    }
}