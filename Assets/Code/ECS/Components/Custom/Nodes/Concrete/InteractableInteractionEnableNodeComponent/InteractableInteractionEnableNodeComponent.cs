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
    public struct InteractableInteractionEnableNodeComponent : IInputNodeComponent<EntityNodeIOComponent>
    {
        [MinValue(0)]
        public int InteractableLayer;

        public bool DisableKinematic;
        
        [field: SerializeField]
        public NodeTagProvider NextNodeProvider { get; set; }

        [field: SerializeField]
        public NodeDataProvider<EntityNodeIOComponent> InputDataProvider { get; set; }
    }
}