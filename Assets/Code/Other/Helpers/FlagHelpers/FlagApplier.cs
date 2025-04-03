namespace EscapeRooms.Helpers
{
    public static class FlagApplier
    {
        public static void HandleFlagCondition(ref int target, int flag, bool condition)
        {
            if (condition)
                target.AddFlag(flag);
            else
                target.RemoveFlag(flag);
        }
    }
}