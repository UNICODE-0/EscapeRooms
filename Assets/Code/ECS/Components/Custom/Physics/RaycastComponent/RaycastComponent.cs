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
    public struct RaycastComponent : IComponent
    {
        [Required]
        public Transform RayStartPoint;
        
        public Vector3 Direction;
        
        [MinValue(0.01f)]
        public float RayLength;
        
        [MinValue(1)]
        public int MaxHitsCount;
        
        public LayerMask LayerMask;

        [FoldoutGroup(Consts.COMPONENT_RUNTIME_FOLDOUT_NAME)] 
        [ReadOnly] public bool IsRayHit;
        
        [FoldoutGroup(Consts.COMPONENT_RUNTIME_FOLDOUT_NAME)] 
        [ReadOnly] public int HitsCount;
        
        [NonSerialized]
        [ShowInInspector]
        [FoldoutGroup(Consts.COMPONENT_RUNTIME_FOLDOUT_NAME)] 
        [ReadOnly] public RaycastHit[] Hits;
        
#if UNITY_EDITOR
        [FoldoutGroup(Consts.DEBUG_FOLDOUT_NAME)]
        public bool DrawLineToBoundsClosestPoint;
#endif
    }
}