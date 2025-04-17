using Scellecs.Morpeh;

namespace EscapeRooms.Components
{
    public interface INodeComponent : IComponent
    {
        public NodeTagProvider NextNodeProvider { get; set; }
    }
}