using EscapeRooms.Mono;
using Scellecs.Morpeh;
using Sirenix.OdinInspector;
using Unity.IL2CPP.CompilerServices;

namespace EscapeRooms.Components
{
    [System.Serializable]
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    public struct ColliderTriggerEventsHolderComponent : IComponent
    {
        [Required]
        public ColliderTriggerEventsHolder EventsHolder;
    }
}