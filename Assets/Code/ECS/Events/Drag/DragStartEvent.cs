using Scellecs.Morpeh;

namespace EscapeRooms.Events
{
    public struct DragStartEvent : IEventData
    {
        public Entity Draggable;
        public Entity Owner;
    }
}