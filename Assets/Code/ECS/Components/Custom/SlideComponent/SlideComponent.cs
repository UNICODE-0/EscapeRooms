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
        [MinValue(0f)]
        public float SlideStartAngle;
        
        [MinValue(0f)]
        public float SlideStopAngle;
        
        [MinValue(0.001f)]
        public float SlideSpeed;
        
        [FormerlySerializedAs("SpeedReduction")] [MinValue(0f)]
        public float SlideSpeedReduction;
        
        [ReadOnly] public Vector3 CurrentVelocity;
    }
}