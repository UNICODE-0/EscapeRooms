using EscapeRooms.Data;
using JetBrains.Annotations;
using Scellecs.Morpeh;
using Sirenix.OdinInspector;
using Unity.IL2CPP.CompilerServices;
using UnityEngine.Serialization;

namespace EscapeRooms.Components
{
    [System.Serializable]
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    public struct DragComponent : IComponent
    {
        public bool DragStartInput;
        public bool DragStopInput;

        [PropertySpace] 
        
        [NotNull]
        public RaycastProvider DragRaycast;

        [MinValue(0.001f)] 
        public float MinDragDistance;
        
        [MinValue(0.001f)] 
        public float MaxDragDistance;
        
        [MinValue(0.001f)] 
        public float MaxDragDeviation;
        
        [FoldoutGroup(Consts.COMPONENT_RUNTIME_FOLDOUT_NAME)] 
        [ReadOnly] public bool IsDragging;
        
        [FoldoutGroup(Consts.COMPONENT_RUNTIME_FOLDOUT_NAME)] 
        [ReadOnly] public Entity DraggableEntity;
    }
}