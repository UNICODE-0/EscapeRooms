using EscapeRooms.Providers;
using Scellecs.Morpeh;
using Scellecs.Morpeh.Providers;
using Sirenix.OdinInspector;

namespace EscapeRooms.Components
{
    public interface INodeComponent : IComponent
    {
        public NodeTagProvider NextNodeProvider { get; set; }
    }
}