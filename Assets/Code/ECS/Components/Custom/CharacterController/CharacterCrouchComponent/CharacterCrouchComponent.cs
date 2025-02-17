using System.Collections.Generic;
using EscapeRooms.Data;
using Scellecs.Morpeh;
using Sirenix.OdinInspector;
using UnityEngine;
using Unity.IL2CPP.CompilerServices;
using UnityEngine.Serialization;

namespace EscapeRooms.Components
{
    [System.Serializable]
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    public struct CharacterCrouchComponent : IComponent
    {
        public bool CrouchInput;
        
        [PropertySpace]
        
        [MinValue(0f)]
        public float StandCapsuleHeight;
        
        public Vector3 StandCapsuleCenter;

        public AnimationCurve StandAnimationCurve;
        
        [MinValue(0f)]
        public float CrouchCapsuleHeight;
        
        public Vector3 CrouchCapsuleCenter;
        
        public AnimationCurve CrouchAnimationCurve;

        [MinValue(0.01f)]
        public float SquatSpeed;

        [FoldoutGroup(Consts.COMPONENT_RUNTIME_FOLDOUT_NAME)] 
        [ReadOnly] public bool IsStandPosition;
        
        [FoldoutGroup(Consts.COMPONENT_RUNTIME_FOLDOUT_NAME)] 
        [ReadOnly] public bool IsCrouchPosition;
    }
}