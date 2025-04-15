using Scellecs.Morpeh;
using Scellecs.Morpeh.Providers;
using UnityEngine;

namespace EscapeRooms.Components
{
    public interface INodeComponent : IComponent
    {
        public EntityProvider NextNodeProvider { get; set; }
    }
}