namespace pk3DS.Core.Structures
{
    public enum MoveTarget : byte
    {
        // Specific target
        AnyExceptSelf,
        AllyOrSelf,
        Ally,
        Opponent,
        AllAdjacent,
        AllAdjacentOpponents,
        AllAllies,
        Self,
        All,
        RandomOpponent,

        // No pkm target
        SideAll,
        SideOpponent,
        SideSelf,
        Counter,
    }
}