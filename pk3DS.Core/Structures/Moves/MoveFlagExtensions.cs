namespace pk3DS.Core.Structures
{
    public static class MoveFlagExtensions
    {
        public static bool HasFlagFast(this MoveFlag6 value, MoveFlag6 flag)
        {
            return (value & flag) != 0;
        }

        public static bool HasFlagFast(this MoveFlag7 value, MoveFlag7 flag)
        {
            return (value & flag) != 0;
        }
    }
}