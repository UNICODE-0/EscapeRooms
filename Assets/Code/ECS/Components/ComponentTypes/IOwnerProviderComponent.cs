using Scellecs.Morpeh;
using Scellecs.Morpeh.Providers;
using UnityEngine;

namespace EscapeRooms.Components
{
    public interface IOwnerProviderComponent : IComponent
    {
        public EntityProvider Owner { get; set; }
    }
}