using Scellecs.Morpeh.Providers;
using Unity.IL2CPP.CompilerServices;
using UnityEngine;

namespace EscapeRooms.Components
{
    [System.Serializable]
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    
    [RequireComponent(typeof(GameObjectProvider))]
    [RequireComponent(typeof(NodeTagProvider))]
    
    public sealed class AnimationPlayNodeProvider : MonoProvider<AnimationPlayNodeComponent>
    {
    }
}