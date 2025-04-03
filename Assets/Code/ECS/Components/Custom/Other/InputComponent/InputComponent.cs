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
        [ReadOnly] public Vector2 MoveValue;
        
        [FoldoutGroup(Consts.COMPONENT_RUNTIME_FOLDOUT_NAME)] 
        [ReadOnly] public Vector2 LookValue;
        
        [FoldoutGroup(Consts.COMPONENT_RUNTIME_FOLDOUT_NAME)]
        [ReadOnly] public bool JumpTrigger;

        [FoldoutGroup(Consts.COMPONENT_RUNTIME_FOLDOUT_NAME)]
        [ReadOnly] public bool CrouchTrigger;
        
        [FoldoutGroup(Consts.COMPONENT_RUNTIME_FOLDOUT_NAME)]
        [ReadOnly] public bool InteractStartTrigger;
        
        [FoldoutGroup(Consts.COMPONENT_RUNTIME_FOLDOUT_NAME)]
        [ReadOnly] public bool InteractStopInProgress;
        
        [FoldoutGroup(Consts.COMPONENT_RUNTIME_FOLDOUT_NAME)]
        [ReadOnly] public bool ThrowTrigger;
        
        [FoldoutGroup(Consts.COMPONENT_RUNTIME_FOLDOUT_NAME)]
        [ReadOnly] public bool DragRotationInProgress;
        
        [FoldoutGroup(Consts.COMPONENT_RUNTIME_FOLDOUT_NAME)]
        [ReadOnly] public Vector2 DragRadiusChangeValue;
    }
    

    public enum InterruptibleInputTrigger
    {
        Jump,
        Crouch,
    }
}