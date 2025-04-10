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
    public struct JointSlidableComponent : IComponent
    {
        [MinValue(0.01f)] 
        public float Spring;
            
        [MinValue(0f)]
        public float Damper;
        
        public Vector3 MinDistance;
        public Vector3 MaxDistance;
        
        public Vector3 SlideDirection;
        
        [Required]
        public Transform Origin;
    }
}