using EscapeRooms.Data;
using Scellecs.Morpeh;
using Sirenix.OdinInspector;
using Unity.IL2CPP.CompilerServices;
using UnityEngine;

namespace EscapeRooms.Components
{
    [System.Serializable]
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    public struct CharacterHeadbuttComponent : IComponent
    {
        [Required]
        public OverlapSphereProvider HeadOverlapCheckProvider;
        
        [MinValue(0.01f)]
        public float ReboundForcePercentage;
        
        [FoldoutGroup(Consts.COMPONENT_RUNTIME_FOLDOUT_NAME)] 
        [ReadOnly] public Vector3 CurrentForce;
        
        [FoldoutGroup(Consts.COMPONENT_RUNTIME_FOLDOUT_NAME)] 
        [ReadOnly] public bool IsHeadbuttForceApplied;
    }
}