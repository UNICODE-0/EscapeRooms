using EscapeRooms.Data;
using Sirenix.OdinInspector;
using Unity.IL2CPP.CompilerServices;

namespace EscapeRooms.Components
{
    [System.Serializable]
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    public struct LerpDataComponent<State> where State: struct
    {
        [DisableIf("@UseLerpProviderInputState")]
        public bool ChangeStateInput;

        [PropertySpace] 
        
        [Required] 
        public FloatLerpProvider LerpProvider;
        public bool UseLerpProviderInputState;

        public State DefaultState;
        public State TargetState;

        [FoldoutGroup(Consts.COMPONENT_RUNTIME_FOLDOUT_NAME)] 
        [ReadOnly] public bool IsTargetState;
    }
}