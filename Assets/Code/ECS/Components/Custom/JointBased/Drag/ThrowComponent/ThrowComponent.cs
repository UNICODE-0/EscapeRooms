using Scellecs.Morpeh;
using Sirenix.OdinInspector;
using Unity.IL2CPP.CompilerServices;

namespace EscapeRooms.Components
{
    [System.Serializable]
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    public struct ThrowComponent : IComponent
    {
        public bool ThrowInput;

        [PropertySpace] 
        
        [MinValue(0.001f)]
        public float ThrowImpulse;

        [LabelText("Mass Impulse Bonus (%)")]
        [PropertyRange(0f, 1f)]
        public float MassImpulseBonus;
    }
}