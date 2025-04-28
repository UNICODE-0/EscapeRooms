using EscapeRooms.Providers;
using Sirenix.OdinInspector;
using Unity.IL2CPP.CompilerServices;
using UnityEngine;

namespace EscapeRooms.Components
{
    [System.Serializable]
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    public struct AnimationPlayNodeComponent : INodeComponent
    {
        [Required]
        public Animation Animation;
        
        [field: SerializeField]
        public NodeTagProvider NextNodeProvider { get; set; }
    }
}