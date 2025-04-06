using EscapeRooms.Data;
using EscapeRooms.Helpers;
using Scellecs.Morpeh;
using Sirenix.OdinInspector;
using Unity.IL2CPP.CompilerServices;

namespace EscapeRooms.Components
{
    [System.Serializable]
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    public struct HingeRotatableComponent : IComponent
    {
        [MinValue(0.01f)] 
        public float Spring;
            
        [MinValue(0f)]
        public float Damper;

        [FoldoutGroup("MinMaxAngle")] 
        public QuaternionQuarter MinAngleQuarter;
        
        [FoldoutGroup("MinMaxAngle")] 
        [MinValue(0f), MaxValue(90f)]
        public float MinAngle;
        
        [FoldoutGroup("MinMaxAngle")] 
        public QuaternionQuarter MaxAngleQuarter;
        
        [FoldoutGroup("MinMaxAngle")] 
        [MinValue(0f), MaxValue(90f)]
        public float MaxAngle;
        
#if UNITY_EDITOR
        [FoldoutGroup(Consts.DEBUG_FOLDOUT_NAME)]
        public bool ShowQuarterAndAngle;
#endif
    }
}