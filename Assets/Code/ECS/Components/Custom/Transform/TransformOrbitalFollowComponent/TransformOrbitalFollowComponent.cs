using EscapeRooms.Data;
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
    public struct TransformOrbitalFollowComponent : IComponent
    {
        [Required] 
        public Transform Target;
        
        [Required] 
        public Transform SphereCenter;

        [MinValue(0.001f)]
        public float FollowSpeed;
        
        [MinValue(0.001f)]
        public float RotationSpeed;

        [MinValue(0.001f)]
        public float SphereRadius;
        
        public Vector3 Offset;
        
        [FoldoutGroup(Consts.COMPONENT_RUNTIME_FOLDOUT_NAME)] 
        [ReadOnly] public float CurrentAzimuth;
        
        [FoldoutGroup(Consts.COMPONENT_RUNTIME_FOLDOUT_NAME)] 
        [ReadOnly] public float CurrentZenith;
        
        [FoldoutGroup(Consts.COMPONENT_RUNTIME_FOLDOUT_NAME)] 
        [ReadOnly] public bool OneFramePermanentCalculation;
    }
}