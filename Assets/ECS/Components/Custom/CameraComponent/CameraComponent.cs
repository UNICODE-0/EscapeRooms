using Scellecs.Morpeh;
using Sirenix.OdinInspector;
using UnityEngine;
using Unity.IL2CPP.CompilerServices;
using UnityEngine.Serialization;

[System.Serializable]
[Il2CppSetOption(Option.NullChecks, false)]
[Il2CppSetOption(Option.ArrayBoundsChecks, false)]
[Il2CppSetOption(Option.DivideByZeroChecks, false)]
public struct CameraComponent : IComponent
{
    public float VerticalSensitivity;
    public float MinXRotation;
    public float MaxXRotation;

    [ReadOnly] public float CurrentXRotation;
}