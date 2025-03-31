using EscapeRooms.Data;
using Scellecs.Morpeh;
using Sirenix.OdinInspector;
using Unity.IL2CPP.CompilerServices;
using UnityEngine;
using UnityEngine.Serialization;

namespace EscapeRooms.Components
{
    [System.Serializable]
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    public struct InputComponent : IComponent
    {
        [FoldoutGroup(Consts.COMPONENT_RUNTIME_FOLDOUT_NAME)] 
        [ReadOnly] public Vector2 MoveActionValue;
        
        [FoldoutGroup(Consts.COMPONENT_RUNTIME_FOLDOUT_NAME)] 
        [ReadOnly] public Vector2 LookActionValue;
        
        [FoldoutGroup(Consts.COMPONENT_RUNTIME_FOLDOUT_NAME)]
        [ReadOnly] public bool JumpTrigger;

        [FoldoutGroup(Consts.COMPONENT_RUNTIME_FOLDOUT_NAME)]
        [ReadOnly] public bool CrouchTrigger;
        
        [FoldoutGroup(Consts.COMPONENT_RUNTIME_FOLDOUT_NAME)]
        [ReadOnly] public bool DragStartTrigger;
        
        [FoldoutGroup(Consts.COMPONENT_RUNTIME_FOLDOUT_NAME)]
        [ReadOnly] public bool DragStopInProgress;
        
        [FoldoutGroup(Consts.COMPONENT_RUNTIME_FOLDOUT_NAME)]
        [ReadOnly] public bool ThrowTrigger;
    }

    public enum InterruptibleInputTrigger
    {
        Jump,
        Crouch,
    }
}