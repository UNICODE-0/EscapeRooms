using EscapeRooms.Data;
using Scellecs.Morpeh;
using Sirenix.OdinInspector;
using UnityEngine;
using Unity.IL2CPP.CompilerServices;

namespace EscapeRooms.Components
{
    [System.Serializable]
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    public struct CharacterSlideComponent : IComponent
    {
        [MinValue(0f)]
        public float SlideStartAngle;
        
        [MinValue(0f)]
        public float SlideStopAngle;
        
        [MinValue(0.001f)]
        public float SlideSpeed;
        
        [MinValue(0f)]
        public float SlideVelocityReduction;
        
        [MinValue(0f)]
        public float MaxSlideVelocityMagnitude;
        
        [MinValue(0f)]
        public float ZeroVelocityMagnitudePrecision;
        
        [MinMaxSlider(-1f, 1f, true)]
        public Vector2 MovementSimilarityNeutralizeThresholds;
        
        [FoldoutGroup(Consts.COMPONENT_RUNTIME_FOLDOUT_NAME)] 
        [ReadOnly] public Vector3 CurrentVelocity;
        
        [FoldoutGroup(Consts.COMPONENT_RUNTIME_FOLDOUT_NAME)] 
        [ReadOnly] public float SlopeAngle;
        
        [FoldoutGroup(Consts.COMPONENT_RUNTIME_FOLDOUT_NAME)] 
        [ReadOnly] public bool IsSliding;
    }
}