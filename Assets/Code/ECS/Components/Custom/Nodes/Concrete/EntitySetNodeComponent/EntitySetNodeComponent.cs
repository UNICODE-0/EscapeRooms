using EscapeRooms.Providers;
using Scellecs.Morpeh.Providers;
using Sirenix.OdinInspector;
using Unity.IL2CPP.CompilerServices;
using UnityEngine;

namespace EscapeRooms.Components
{
    [System.Serializable]
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    public struct EntitySetNodeComponent : IOutputNodeComponent<EntityNodeIOComponent>
    {
        [Required]
        public EntityProvider EntityProvider;
        
        [field: SerializeField]
        public NodeTagProvider NextNodeProvider { get; set; }
        
        [field: SerializeField]
        public NodeDataProvider<EntityNodeIOComponent> OutputDataProvider { get; set; }
    }
}