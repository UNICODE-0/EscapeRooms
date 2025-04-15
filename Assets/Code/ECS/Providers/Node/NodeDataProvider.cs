using EscapeRooms.Components;
using Scellecs.Morpeh.Providers;

namespace EscapeRooms.Providers
{
    public class NodeDataProvider<T> : MonoProvider<T> where T: struct, INodeDataComponent
    {
        
    }
}