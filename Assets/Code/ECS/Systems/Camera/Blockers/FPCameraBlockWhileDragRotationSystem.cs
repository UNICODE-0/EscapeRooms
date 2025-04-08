using EscapeRooms.Components;
using EscapeRooms.Helpers;
using Scellecs.Morpeh;
using Unity.IL2CPP.CompilerServices;

namespace EscapeRooms.Systems
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    public sealed class FPCameraBlockWhileDragRotationSystem : ISystem
    {
        public World World { get; set; }

        private Filter _filter;
        private Stash<DragRotationComponent> _dragRotationStash;
        private Stash<FPCameraComponent> _fpCameraStash;
        private Stash<FPCameraBlockWhileDragRotationComponent> _blockWhileRotationStash;

        public void OnAwake()
        {
            _filter = World.Filter
                .With<FPCameraComponent>()
                .With<FPCameraBlockWhileDragRotationComponent>()
                .Build();

            _dragRotationStash = World.GetStash<DragRotationComponent>();
            _fpCameraStash = World.GetStash<FPCameraComponent>();
            _blockWhileRotationStash = World.GetStash<FPCameraBlockWhileDragRotationComponent>();
        }

        public void OnUpdate(float deltaTime)
        {
            foreach (var entity in _filter)
            {
                ref var fpCameraComponent = ref _fpCameraStash.Get(entity);
                ref var blockerComponent = ref _blockWhileRotationStash.Get(entity);
                ref var dragRotationComponent = ref _dragRotationStash.Get(blockerComponent.DragRotation.Entity);
                
                FlagApplier.HandleFlagCondition(ref fpCameraComponent.RotationBlockFlag, 
                    FPCameraBlockers.DRAG_ROTATION, dragRotationComponent.IsRotating);
            }
        }

        public void Dispose()
        {
        }
    }
}