using Scellecs.Morpeh;
using Scellecs.Morpeh.Providers;
using Unity.IL2CPP.CompilerServices;

namespace EscapeRooms.Components
{
    [System.Serializable]
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    public struct HeadbuttComponent : IComponent
    {
        public EntityProvider HeadOverlapCheckEntity;

        // [FoldoutGroup(Consts.COMPONENT_RUNTIME_FOLDOUT_NAME)] 
        // [ReadOnly] public bool IsRayHit;
    }
}