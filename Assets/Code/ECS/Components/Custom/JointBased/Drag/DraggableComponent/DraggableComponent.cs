using EscapeRooms.Data;
using Scellecs.Morpeh;
using Sirenix.OdinInspector;
using Unity.IL2CPP.CompilerServices;
using UnityEngine;
using UnityEngine.Serialization;

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

        [FoldoutGroup("DragDrive")]
        [MinValue(0)]
        public float DragDriveSpring;
        
        [FoldoutGroup("DragDrive")]
        [MinValue(0)]
        public float DragDriveDamper;
        
        [FoldoutGroup("DragAngularDrive")]
        [MinValue(0)]
        public float DragAngularDriveSpring;
        
        [FoldoutGroup("DragAngularDrive")]
        [MinValue(0)]
        public float DragAngularDriveDamper;

        [FoldoutGroup("Rigidbody")]
        [MinValue(0)]
        public float BodyLinearDamping;
        
        [FoldoutGroup("Rigidbody")]
        [MinValue(0)]
        public float BodyAngularDamping;
        
        [FoldoutGroup("Rigidbody")]
        [MinValue(0.001f)]
        public float MassWhileDrag;

        [FoldoutGroup("Rigidbody")]
        [MinValue(0.001f)]
        public float MaxVelocity;
        
        [FoldoutGroup("Rigidbody")]
        [MinValue(0.001f)]
        public float MaxAngularVelocity;
        
        [FoldoutGroup(Consts.COMPONENT_RUNTIME_FOLDOUT_NAME)] 
        [ReadOnly] public float MassBeforeDrag;
    }
}