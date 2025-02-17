using System;
using Scellecs.Morpeh;
using UnityEngine;
using Unity.IL2CPP.CompilerServices;
using UnityEngine.InputSystem;

namespace EscapeRooms.Components
{
    [System.Serializable]
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    public struct InputComponent : IComponent
    {
        [NonSerialized] public InputAction MoveAction;
        [NonSerialized] public InputAction LookAction;
        [NonSerialized] public InputAction JumpAction;
        [NonSerialized] public InputAction CrouchAction;
    }
}