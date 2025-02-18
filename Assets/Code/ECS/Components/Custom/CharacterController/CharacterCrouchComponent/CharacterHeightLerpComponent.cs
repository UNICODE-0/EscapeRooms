using System.Collections.Generic;
using EscapeRooms.Data;
using Scellecs.Morpeh;
using Sirenix.OdinInspector;
using Sirenix.OdinInspector.Modules.UnityMathematics.Editor;
using UnityEngine;
using Unity.IL2CPP.CompilerServices;
using UnityEngine.Serialization;

namespace EscapeRooms.Components
{
    [System.Serializable]
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    public struct CharacterHeightLerpComponent : IComponent
    {
        public bool ChangeStateInput;

        [PropertySpace] 
        
        public CharacterHeightLerpState DefaultState;
        
        public CharacterHeightLerpState TargetState;
        
        [FoldoutGroup(Consts.COMPONENT_RUNTIME_FOLDOUT_NAME)] 
        [ReadOnly] public float LerpStartTime;

        [FoldoutGroup(Consts.COMPONENT_RUNTIME_FOLDOUT_NAME)] 
        [ReadOnly] public bool IsTargetState;
        
        [FoldoutGroup(Consts.COMPONENT_RUNTIME_FOLDOUT_NAME)] 
        [ReadOnly] public bool IsChangeColliderInProgress;
    }
    
    [System.Serializable]
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    public struct CharacterHeightLerpState
    {
        public float CapsuleHeight;
        public Vector3 CapsuleCenter;
        public AnimationCurve OutCurve;
        
        [MinValue(0f)]
        public float OutTime;
    }
}