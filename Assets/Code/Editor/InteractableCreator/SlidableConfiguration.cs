using EscapeRooms.Components;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Serialization;

namespace EscapeRooms.Editor
{
    [CreateAssetMenu(fileName = "SlidableConfiguration", menuName = "EscapeRooms/SlidableConfiguration", order = 1)]
    public class SlidableConfiguration : ScriptableObject
    {
        [MinValue(0f)]
        public int SlidableLayer;
        
        [PropertySpace]
        
        public JointSlidableComponent JointSlidableSample;
    }
}