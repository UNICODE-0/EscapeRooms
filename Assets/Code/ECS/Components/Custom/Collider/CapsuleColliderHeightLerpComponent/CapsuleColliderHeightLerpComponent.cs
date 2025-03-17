using EscapeRooms.Data;
using Scellecs.Morpeh;
using Sirenix.OdinInspector;
using Unity.IL2CPP.CompilerServices;
using UnityEngine;

namespace EscapeRooms.Components
{
    [System.Serializable]
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    public struct CapsuleColliderHeightLerpComponent : IComponent
    {
        [DisableIf("@UseFloatProviderInputState")]
        public bool ChangeHeightInput;

        [PropertySpace] 
        
        [Required]
        public FloatLerpProvider FloatLerpProvider;

        public bool UseFloatProviderInputState;
        
        public CapsuleHeightState DefaultState;
        public CapsuleHeightState TargetState;
        
        [FoldoutGroup(Consts.COMPONENT_RUNTIME_FOLDOUT_NAME)] 
        [ReadOnly] public bool IsTargetState;
    }
    
    [System.Serializable]
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    public struct CapsuleHeightState
    {
        public float CapsuleHeight;
        public Vector3 CapsuleCenter;
    }
}