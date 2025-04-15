using Scellecs.Morpeh;
using Scellecs.Morpeh.Providers;

namespace EscapeRooms.Components
{
    public interface IOwnerProviderComponent : IComponent
    {
        public EntityProvider Owner { get; set; }
    }
}