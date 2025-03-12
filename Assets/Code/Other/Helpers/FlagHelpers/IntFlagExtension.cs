namespace EscapeRooms.Helpers
{
    public static class IntFlagExtension
    {
        public static void AddFlag(this ref int target, int flag)
        {
            target |= flag;
        }
        
        public static bool CheckFlag(this ref int target, int flag)
        {
            return (target & flag) != 0;
        }
        
        public static void RemoveFlag(this ref int target, int flag)
        {
            target &= ~flag;
        }
        
        public static bool IsFlagClear(this ref int target)
        {
            return target == 0;
        }
        
        public static bool IsFlagNotClear(this ref int target)
        {
            return target != 0;
        }
    }
}