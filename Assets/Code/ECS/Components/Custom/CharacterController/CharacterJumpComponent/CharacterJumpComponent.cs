using System;
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
    public struct CharacterJumpComponent : IComponent
    {
        [NonSerialized] public int JumpBlockFlag;
        
        public bool JumpInput;
        
        [PropertySpace]
        
        [MinValue(0.001f)]
        public float JumpStrength;
        
        [MinValue(0f)]
        public float FrameTimeCorrection;
        
        [MinValue(0f)]
        public float ReferenceFrameTime;
        
        [FoldoutGroup(Consts.COMPONENT_RUNTIME_FOLDOUT_NAME)] 
        [ReadOnly] public bool IsJumpAllowed;
        
        [FoldoutGroup(Consts.COMPONENT_RUNTIME_FOLDOUT_NAME)] 
        [ReadOnly] public bool IsJumpForceApplied;
        
        [FoldoutGroup(Consts.COMPONENT_RUNTIME_FOLDOUT_NAME)] 
        [ReadOnly] public Vector3 CurrentForce;
    }
}