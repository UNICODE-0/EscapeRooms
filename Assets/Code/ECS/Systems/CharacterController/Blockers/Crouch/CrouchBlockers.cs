namespace EscapeRooms.Systems
{
    public static class CrouchBlockers
    {
        public const int JUMPING = 1 << 0;
        public const int FALLING = 1 << 1;
        public const int STAND = 1 << 2;
        public const int STANDING = 1 << 3;
        public const int STATIC_COLLISION = 1 << 4;
    }
}