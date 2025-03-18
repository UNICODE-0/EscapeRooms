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
    public struct CharacterCrouchComponent : IComponent
    {
        [NonSerialized] public int CrouchBlockFlag;
        
        public bool CrouchInput;

        [PropertySpace] 
        
        [Required]
        public FloatLerpProvider HeightLerpProvider;

        [Required]
        public FloatLerpProvider HeadLerpProvider;
        
        [FoldoutGroup(Consts.COMPONENT_RUNTIME_FOLDOUT_NAME)] 
        [ReadOnly] public bool IsCrouching;
        
        [FoldoutGroup(Consts.COMPONENT_RUNTIME_FOLDOUT_NAME)] 
        [ReadOnly] public bool IsSquatInProgress;
    }
}