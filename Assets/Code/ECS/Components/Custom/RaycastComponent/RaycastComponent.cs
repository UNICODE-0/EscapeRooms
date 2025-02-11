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
        #if UNITY_EDITOR
        public bool UseDebugDraw;
        public Color DebugColor;
        [PropertySpace]
        #endif
        
        [Required]
        public Transform RayStartPoint;
        
        public Vector3 Direction;
        
        [MinValue(0.01f)]
        public float RayLength;
        
        [MinValue(1)]
        public int HitsCount;
        
        public LayerMask LayerMask;

        [FoldoutGroup(Consts.COMPONENT_RUNTIME_FOLDOUT_NAME)] 
        [ReadOnly] public bool IsRayHit;
    
        [FoldoutGroup(Consts.COMPONENT_RUNTIME_FOLDOUT_NAME)] 
        [ReadOnly] public RaycastHit[] Hits;
    }
}