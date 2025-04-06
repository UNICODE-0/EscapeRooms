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
    public struct DragRotationComponent : IComponent
    {
        public bool RotationActiveInput;
        public Vector2 RotationDeltaInput;

        [PropertySpace] 
        
        [MinValue(0.001f)] 
        public float RotationSpeed;

        [MinValue(0.001f)]
        public Vector2 MaxInputDelta;
        
        [MaxValue(0.001f)]
        public Vector2 MinInputDelta;
        
        [FoldoutGroup(Consts.COMPONENT_RUNTIME_FOLDOUT_NAME)] 
        [ReadOnly] public bool IsRotating;
    }
}