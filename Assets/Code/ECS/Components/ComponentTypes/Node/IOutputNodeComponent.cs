using EscapeRooms.Providers;

namespace EscapeRooms.Components
{
    public interface IOutputNodeComponent<T> : INodeComponent where T: struct, INodeDataComponent 
    {
        public NodeDataProvider<T> OutputDataProvider { get; set; }
    }
}