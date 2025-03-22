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
    public struct DraggableComponent : IComponent
    {
        [Required]
        public Collider[] Colliders;
        
        public PhysicsMaterial MaterialOnDrag;

        [PropertySpace]
        
        [MinValue(0)]
        public float DragDriveSpring;
        
        [MinValue(0)]
        public float DragDriveDamper;
        
        [PropertySpace]
        
        [MinValue(0)]
        public float DragAngularDriveSpring;
        
        [MinValue(0)]
        public float DragAngularDriveDamper;

        [PropertySpace]
        
        [MinValue(0)]
        public float BodyLinearDamping;
        
        [MinValue(0)]
        public float BodyAngularDamping;
    }
}