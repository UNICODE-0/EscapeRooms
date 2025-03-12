using Scellecs.Morpeh;

namespace EscapeRooms.Events
{
    public struct DragStopEvent : IEventData
    {
        public Entity Draggable;
        public Entity Owner;
    }
}