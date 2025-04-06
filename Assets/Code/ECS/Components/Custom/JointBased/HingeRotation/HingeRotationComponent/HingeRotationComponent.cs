using EscapeRooms.Data;
using JetBrains.Annotations;
using Scellecs.Morpeh;
using Sirenix.OdinInspector;
using Unity.IL2CPP.CompilerServices;

namespace EscapeRooms.Components
{
    [System.Serializable]
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    public struct HingeRotationComponent : IComponent
    {
        public float RotateDeltaInput;
        public bool RotateStartInput;
        public bool RotateStopInput;

        [PropertySpace] 
        
        [NotNull]
        public RaycastProvider DetectionRaycast;
        
        [MinValue(0.01f)]
        public float RotationSpeed;

        [FoldoutGroup(Consts.COMPONENT_RUNTIME_FOLDOUT_NAME)] 
        [ReadOnly] public bool IsRotating;
        
        [FoldoutGroup(Consts.COMPONENT_RUNTIME_FOLDOUT_NAME)] 
        [ReadOnly] public Entity RotatingEntity;
    }
}