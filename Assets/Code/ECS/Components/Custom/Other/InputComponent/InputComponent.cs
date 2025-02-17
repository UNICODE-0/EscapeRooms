using System;
using EscapeRooms.Data;
using Scellecs.Morpeh;
using Sirenix.OdinInspector;
using Unity.IL2CPP.CompilerServices;
using UnityEngine;
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
        
        [FoldoutGroup(Consts.COMPONENT_RUNTIME_FOLDOUT_NAME)] 
        [ReadOnly] public Vector2 MoveActionValue;
        
        [FoldoutGroup(Consts.COMPONENT_RUNTIME_FOLDOUT_NAME)] 
        [ReadOnly] public Vector2 LookActionValue;
        
        [FoldoutGroup(Consts.COMPONENT_RUNTIME_FOLDOUT_NAME)]
        [InlineProperty]
        [ReadOnly] public DelayedInputTrigger JumpTriggerValue;

        [FoldoutGroup(Consts.COMPONENT_RUNTIME_FOLDOUT_NAME)]
        [InlineProperty]
        [ReadOnly] public DelayedInputTrigger CrouchTriggerValue;
    }
}