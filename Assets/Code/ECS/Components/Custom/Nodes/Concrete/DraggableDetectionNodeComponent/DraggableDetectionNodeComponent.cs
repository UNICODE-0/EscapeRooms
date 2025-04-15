using EscapeRooms.Providers;
using Scellecs.Morpeh.Providers;
using Unity.IL2CPP.CompilerServices;
using UnityEngine;

namespace EscapeRooms.Components
{
    [System.Serializable]
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    public struct DraggableDetectionNodeComponent : IOutputNodeComponent<DraggableDetectionNodeOutputDataComponent>
    {
        [field: SerializeField]
        public NodeTagProvider NextNodeProvider { get; set; }
        
        [field: SerializeField]
        public NodeDataProvider<DraggableDetectionNodeOutputDataComponent> OutputDataProvider { get; set; }
    }
}