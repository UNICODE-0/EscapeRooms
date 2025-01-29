using System;
using Scellecs.Morpeh;
using UnityEngine;
using Unity.IL2CPP.CompilerServices;
using UnityEngine.InputSystem;

[System.Serializable]
[Il2CppSetOption(Option.NullChecks, false)]
[Il2CppSetOption(Option.ArrayBoundsChecks, false)]
[Il2CppSetOption(Option.DivideByZeroChecks, false)]
public struct InputComponent : IComponent, IDisposable
{
    [NonSerialized] public InputAction MoveAction;

    public void Dispose()
    {
        MoveAction.Dispose();
    }
}