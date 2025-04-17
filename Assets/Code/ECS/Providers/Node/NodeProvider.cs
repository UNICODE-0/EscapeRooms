using EscapeRooms.Components;
using Scellecs.Morpeh.Providers;

namespace EscapeRooms.Providers
{
    public class NodeProvider<T> : MonoProvider<T> where T: struct, INodeComponent
    {
        
    }
}