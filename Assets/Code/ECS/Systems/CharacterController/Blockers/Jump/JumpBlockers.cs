namespace EscapeRooms.Systems
{
    public static class JumpBlockers
    {
        public const int JUMP_BLOCK_WHILE_CROUCH_FLAG = 1 << 0;
        public const int JUMP_BLOCK_WHILE_STATIC_COLLISION_FLAG = 1 << 1;
    }
}