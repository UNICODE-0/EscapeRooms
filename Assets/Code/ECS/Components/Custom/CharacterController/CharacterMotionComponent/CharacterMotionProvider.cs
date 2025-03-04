using Scellecs.Morpeh.Providers;
using Unity.IL2CPP.CompilerServices;
using UnityEngine;

namespace EscapeRooms.Components
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    
    [RequireComponent(typeof(CharacterControllerProvider))]
    
    public sealed class CharacterMotionProvider : MonoProvider<CharacterMotionComponent>
    {
    }
}