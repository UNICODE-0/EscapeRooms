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
    public struct DragRadiusChangeComponent : IComponent
    {
        public Vector2 RadiusChangeDeltaInput;

        [PropertySpace] 
        
        [MinValue(0.001f)] 
        public float Speed;
    }
}