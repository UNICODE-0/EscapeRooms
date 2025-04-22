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
    public struct OneHitRaycastComponent : IComponent
    {
        [Required]
        public Transform RayStartPoint;
        
        public Vector3 Direction;
        
        [MinValue(0.01f)]
        public float RayLength;
        
        public LayerMask LayerMask;

        [FoldoutGroup(Consts.COMPONENT_RUNTIME_FOLDOUT_NAME)] 
        [ReadOnly] public bool IsRayHit;
        
        [NonSerialized]
        [ShowInInspector]
        [FoldoutGroup(Consts.COMPONENT_RUNTIME_FOLDOUT_NAME)] 
        [ReadOnly] public RaycastHit Hit;
        
#if UNITY_EDITOR
        [FoldoutGroup(Consts.DEBUG_FOLDOUT_NAME)]
        public bool DrawLineToBoundsClosestPoint;
#endif
    }
}