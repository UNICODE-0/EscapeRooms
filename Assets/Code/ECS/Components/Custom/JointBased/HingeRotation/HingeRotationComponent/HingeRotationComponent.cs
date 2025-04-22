using EscapeRooms.Data;
using JetBrains.Annotations;
using Scellecs.Morpeh;
using Sirenix.OdinInspector;
using Unity.IL2CPP.CompilerServices;
using UnityEngine;
using UnityEngine.Serialization;

namespace EscapeRooms.Components
{
    [System.Serializable]
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    public struct HingeRotationComponent : IComponent
    {
        public float RotateDeltaInput;
        public bool RotateStartInput;
        public bool RotateStopInput;

        [PropertySpace] 
        
        [NotNull]
        public OneHitRaycastProvider DetectionRaycast;

        [MinMaxSlider(-5f, 5f, true)] 
        public Vector2 DeltaRange;
        
        [MinValue(0.01f)]
        public float RotationSpeed;
        
        [MinValue(0.01f)]
        public float MaxDeviation;

        [FoldoutGroup(Consts.COMPONENT_RUNTIME_FOLDOUT_NAME)] 
        [ReadOnly] public bool IsRotating;
        
        [FoldoutGroup(Consts.COMPONENT_RUNTIME_FOLDOUT_NAME)] 
        [ReadOnly] public Entity RotatableEntity;
    }
}