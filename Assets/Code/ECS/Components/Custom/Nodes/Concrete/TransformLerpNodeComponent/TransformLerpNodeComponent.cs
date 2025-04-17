using EscapeRooms.Data;
using EscapeRooms.Providers;
using Sirenix.OdinInspector;
using Unity.IL2CPP.CompilerServices;
using UnityEngine;

namespace EscapeRooms.Components
{
    [System.Serializable]
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    public struct TransformLerpNodeComponent : IInputNodeComponent<EntityNodeIOComponent>
    {
        [Required]
        public Transform Target;

        [Required]
        public FloatLerpProvider LerpProvider;
        
        [PropertySpace]
        
        [field: SerializeField]
        public NodeTagProvider NextNodeProvider { get; set; }

        [field: SerializeField]
        public NodeDataProvider<EntityNodeIOComponent> InputDataProvider { get; set; }
        
        [FoldoutGroup(Consts.COMPONENT_RUNTIME_FOLDOUT_NAME)] 
        [ReadOnly] public Vector3 StartPosition;
        
        [FoldoutGroup(Consts.COMPONENT_RUNTIME_FOLDOUT_NAME)] 
        [ReadOnly] public Quaternion StartRotation;
    }
}