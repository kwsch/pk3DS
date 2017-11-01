using System;

namespace pk3DS.Core.Structures
{
    [Flags]
    public enum BattleStatusFlags : byte
    {
        /// <summary>
        /// Sleep
        /// </summary>
        SLP = 1 << 0,

        /// <summary>
        /// Poison
        /// </summary>
        PSN = 1 << 1,

        /// <summary>
        /// Burn
        /// </summary>
        BRN = 1 << 2,

        /// <summary>
        /// Freeze
        /// </summary>
        FRZ = 1 << 3,

        /// <summary>
        /// Paralysis
        /// </summary>
        PAR = 1 << 4,

        /// <summary>
        /// Confusion
        /// </summary>
        CFZ = 1 << 5,

        /// <summary>
        /// Infatuation
        /// </summary>
        INF = 1 << 6,

        /// <summary>
        /// Guard Spec.
        /// </summary>
        GSP = 1 << 7,
    }
}