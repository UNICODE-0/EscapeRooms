using EscapeRooms.Providers;
using Scellecs.Morpeh;
using Scellecs.Morpeh.Providers;
using UnityEngine;

namespace EscapeRooms.Components
{
    public interface IInputNodeComponent<T> : INodeComponent where T: struct, INodeDataComponent 
    {
        public NodeDataProvider<T> InputDataProvider { get; set; }
    }
}