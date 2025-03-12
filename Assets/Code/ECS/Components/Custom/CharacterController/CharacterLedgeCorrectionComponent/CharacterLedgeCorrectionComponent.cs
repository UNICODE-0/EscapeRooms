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
    public struct CharacterLedgeCorrectionComponent : IComponent
    {
        [MinValue(0f)]
        public float OutOfLedgeStepOffset;
        
        [MinValue(0f)]
        public float LedgeStepOffset;
        
        [MinMaxSlider(0f, 90f, true)]
        public Vector2 LedgeDetectionInterval;
        
        [MinMaxSlider(0f, 0.5f, true)]
        public Vector2 PositionDifferenceDetectionInterval;
        
        [FoldoutGroup(Consts.COMPONENT_RUNTIME_FOLDOUT_NAME)] 
        [ReadOnly] public Vector3 CurrentAttraction;
        
        [FoldoutGroup(Consts.COMPONENT_RUNTIME_FOLDOUT_NAME)] 
        [ReadOnly] public bool IgnoreAttraction;
    }
}