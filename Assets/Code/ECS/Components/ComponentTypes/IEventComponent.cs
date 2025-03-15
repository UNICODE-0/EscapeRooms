namespace EscapeRooms.Components
{
    public interface IEventComponent : IOwnerComponent
    {
        public bool IsLastFrameOfLife { get; set; }
    }
}