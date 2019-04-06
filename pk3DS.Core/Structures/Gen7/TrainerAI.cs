using System;

namespace pk3DS.Core.Structures
{
    [Flags]
    public enum TrainerAI : byte
    {
        Basic = 1 << 0,
        Strong = 1 << 1,
        Expert = 1 << 2,

        Doubles = 1 << 3,
        NoWhiteout = 1 << 4,
        BattleRoyal = 1 << 5,
        PokeChange = 1 << 6,
        UseItem = 1 << 7,
    }
}