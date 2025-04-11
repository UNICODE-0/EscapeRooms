using EscapeRooms.Components;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Serialization;

namespace EscapeRooms.Editor
{
    [CreateAssetMenu(fileName = "RotatableConfiguration", menuName = "EscapeRooms/RotatableConfiguration", order = 1)]
    public class RotatableConfiguration : ScriptableObject
    {
        [MinValue(0f)]
        public int RotatableLayer;
        
        [PropertySpace]
        
        public HingeRotatableComponent RotatableSample;
    }
}