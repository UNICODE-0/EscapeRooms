using Scellecs.Morpeh;
using Sirenix.OdinInspector;
using UnityEngine;
using Unity.IL2CPP.CompilerServices;
using UnityEngine.Serialization;

namespace EscapeRooms.Components
{
    [System.Serializable]
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    public struct GravityComponent : IComponent
    {
        [MaxValue(-0.001f)]
        public float GravitationalAttraction;
        
        [MaxValue(-0.001f)]
        public float GroundedAttraction;

        [ReadOnly] public Vector3 CurrentAttraction;
        [ReadOnly] public bool IgnoreAttraction;
    }
}