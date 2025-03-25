namespace EscapeRooms.Data
{
    public class FrameData : Singleton<FrameData>
    {
        public const int MAX_FRAME_ID = int.MaxValue;
        
        public int FrameId = 1;
    }
}