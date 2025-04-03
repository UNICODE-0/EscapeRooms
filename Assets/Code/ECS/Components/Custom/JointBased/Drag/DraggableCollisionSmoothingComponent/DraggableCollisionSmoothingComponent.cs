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
    public struct DraggableCollisionSmoothingComponent : IComponent
    {
        [Required]
        public Collider SmoothingTrigger;
        
        [FoldoutGroup("SmoothDrive")]
        [MinValue(0)]
        public float SmoothDriveSpring;
        
        [FoldoutGroup("SmoothDrive")]
        [MinValue(0)]
        public float SmoothDriveDamper;
        
        [FoldoutGroup("SmoothAngularDrive")]
        [MinValue(0)]
        public float SmoothAngularDriveSpring;
        
        [FoldoutGroup("SmoothAngularDrive")]
        [MinValue(0)]
        public float SmoothAngularDriveDamper;
        
        [FoldoutGroup(Consts.COMPONENT_RUNTIME_FOLDOUT_NAME)] 
        [ReadOnly] public bool IsSmoothed;
    }
}