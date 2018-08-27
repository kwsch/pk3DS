using System;

namespace pk3DS.Core.Structures
{
    [Flags]
    public enum MoveFlag6 : uint
    {
        None,

        MakesContact     = 1u << 00, // Makes contact.
        Charge           = 1u << 01, // The user is unable to make a move between turns.
        Recharge         = 1u << 02, // If this move is successful, the user must recharge on the following turn and cannot make a move.
        Protect          = 1u << 03, // Blocked by Detect, Protect, Spiky Shield, and if not a Status move, King's Shield.
        Reflectable      = 1u << 04, // Bounced back to the original user by Magic Coat or the Magic Bounce Ability.
        Snatch           = 1u << 05, // Can be stolen from the original user and instead used by another Pokemon using Snatch.
        Mirror           = 1u << 06, // Can be copied by Mirror Move.
        Punch            = 1u << 07, // Power is multiplied when used by a Pokemon with the Iron Fist Ability.

        Sound            = 1u << 08, // Has no effect on Pokemon with the Soundproof Ability.
        Gravity          = 1u << 09, // Prevented from being executed or selected during Gravity's effect.
        Defrost          = 1u << 10, // Thaws the user if executed successfully while the user is frozen.
        DistanceTriple   = 1u << 11, // Can target a Pokemon positioned anywhere in a Triple Battle.
        Heal             = 1u << 12, // Prevented from being executed or selected during Heal Block's effect.
        IgnoreSubstitute = 1u << 13, // Ignores a target's substitute.
        FailSkyBattle    = 1u << 14, // Prevented from being executed or selected in a Sky Battle.
        AnimateAlly      = 1u << 15, // Always animate the move when used on an ally.

        F17              = 1u << 16, // Dancer in future games
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