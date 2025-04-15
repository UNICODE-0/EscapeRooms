using EscapeRooms.Providers;

namespace EscapeRooms.Components
{
    public interface IInputNodeComponent<T> : INodeComponent where T: struct, INodeDataComponent 
    {
        public NodeDataProvider<T> InputDataProvider { get; set; }
    }
}