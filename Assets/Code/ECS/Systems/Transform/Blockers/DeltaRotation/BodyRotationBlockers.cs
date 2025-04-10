namespace EscapeRooms.Systems
{
    public static class BodyRotationBlockers
    {
        public const int DRAG_ROTATION = 1 << 0;
        public const int HINGE_ROTATION = 1 << 1;
        public const int JOINT_SLIDE = 1 << 2;
    }
}