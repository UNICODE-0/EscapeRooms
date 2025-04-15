using EscapeRooms.Providers;
using Scellecs.Morpeh;
using Scellecs.Morpeh.Providers;
using UnityEngine;

namespace EscapeRooms.Components
{
    public interface IOutputNodeComponent<T> : INodeComponent where T: struct, INodeDataComponent 
    {
        public NodeDataProvider<T> OutputDataProvider { get; set; }
    }
}