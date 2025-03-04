namespace EscapeRooms.Systems
{
    public static class CrouchBlockers
    {
        public const int CROUCH_BLOCK_WHILE_JUMP_FLAG = 1 << 0;
        public const int CROUCH_BLOCK_WHILE_FALLING_FLAG = 1 << 1;
        public const int CROUCH_STAND_BLOCK_FLAG = 1 << 2;
        public const int CROUCH_STANDING_BLOCK_FLAG = 1 << 3;
    }
}