using Scellecs.Morpeh;
using Unity.Collections;
using UnityEngine;
using Unity.IL2CPP.CompilerServices;

namespace EscapeRooms.Components
{
    [System.Serializable]
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]

    public struct CharacterControllerComponent : IComponent
    {
        public CharacterController CharacterController;
        public ControllerColliderHitHolder HitHolder;
    }
}