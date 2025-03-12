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
        
        [PropertySpace]
        
        [MinValue(0)]
        public float SmoothDriveSpring;
        
        [MinValue(0)]
        public float SmoothDriveDamper;
        
        [PropertySpace]
        
        [MinValue(0)]
        public float SmoothAngularDriveSpring;
        
        [MinValue(0)]
        public float SmoothAngularDriveDamper;
        
        [FoldoutGroup(Consts.COMPONENT_RUNTIME_FOLDOUT_NAME)] 
        [ReadOnly] public bool IsSmoothed;
    }
}