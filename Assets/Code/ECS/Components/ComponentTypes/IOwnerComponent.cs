using Scellecs.Morpeh;

namespace EscapeRooms.Components
{
    public interface IOwnerComponent : IComponent
    {
        public Entity Owner { get; set; }
    }
}