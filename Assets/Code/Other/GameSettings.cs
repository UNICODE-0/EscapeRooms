namespace EscapeRooms.Data
{
    public class GameSettings : Singleton<GameSettings>
    {
        public int TargetFrameRate;
        public float Sensitivity;
        public float JumpInputTriggerDelay;
        public float CrouchInputTriggerDelay;
    }
}