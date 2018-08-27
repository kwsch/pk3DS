using System;

namespace pk3DS.Core.Structures
{
    [Flags]
    public enum MoveFlag7 : uint
    {
        None,

        MakesContact     = 1u << 00,
        Charge           = 1u << 01,
        Recharge         = 1u << 02,
        Protect          = 1u << 03,
        Reflectable      = 1u << 04,
        Snatch           = 1u << 05,
        Mirror           = 1u << 06,
        Punch            = 1u << 07,

        Sound            = 1u << 08,
        Gravity          = 1u << 09,
        Defrost          = 1u << 10,
        Distance         = 1u << 11,
        Heal             = 1u << 12,
        IgnoreSubstitute = 1u << 13,
        NonSky           = 1u << 14,
        Unknown          = 1u << 15,

        Dance            = 1u << 16,
        F18              = 1u << 17,
        F19              = 1u << 18,
        F20              = 1u << 19,
        F21              = 1u << 20,
        F22              = 1u << 21,
        F23              = 1u << 22,
        F24              = 1u << 23,

        F25              = 1u << 24,
        F26              = 1u << 25,
        F27              = 1u << 26,
        F28              = 1u << 27,
        F29              = 1u << 28,
        F30              = 1u << 29,
        F31              = 1u << 30,
        F32              = 1u << 31,
    }
}