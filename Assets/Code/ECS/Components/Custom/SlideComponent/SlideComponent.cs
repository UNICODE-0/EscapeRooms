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
    public struct SlideComponent : IComponent
    {
        public float SlideStartAngle;
        public float SlideStopAngle;
        public float SlideSpeed;
        
        [ReadOnly] public Vector3 CurrentVelocity;
    }
}