using EscapeRooms.Components;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Serialization;

namespace EscapeRooms.Editor
{
    [CreateAssetMenu(fileName = "DraggableConfiguration", menuName = "EscapeRooms/DraggableConfiguration", order = 1)]
    public class DraggableConfiguration : ScriptableObject
    {
        [MinValue(0f)]
        public int DraggableLayer;
        
        [MinValue(1f)]
        public float SmoothingTriggerSizeScale;
        
        public LayerMask SmoothingExcludeLayerMask;
        
        [PropertySpace]
        
        public DraggableComponent DraggableComponentSample;
        public DraggableCollisionSmoothingComponent DraggableCollisionSmoothingComponentSample;
    }
}