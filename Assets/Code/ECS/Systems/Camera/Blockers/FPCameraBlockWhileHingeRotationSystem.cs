using EscapeRooms.Components;
using EscapeRooms.Helpers;
using Scellecs.Morpeh;
using Unity.IL2CPP.CompilerServices;

namespace EscapeRooms.Systems
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    public sealed class FPCameraBlockWhileHingeRotationSystem : ISystem
    {
        public World World { get; set; }

        private Filter _filter;
        private Stash<HingeRotationComponent> _hingeRotationStash;
        private Stash<FPCameraComponent> _fpCameraStash;
        private Stash<FPCameraBlockWhileHingeRotationComponent> _blockWhileRotationStash;

        public void OnAwake()
        {
            _filter = World.Filter
                .With<FPCameraComponent>()
                .With<FPCameraBlockWhileHingeRotationComponent>()
                .Build();

            _hingeRotationStash = World.GetStash<HingeRotationComponent>();
            _fpCameraStash = World.GetStash<FPCameraComponent>();
            _blockWhileRotationStash = World.GetStash<FPCameraBlockWhileHingeRotationComponent>();
        }

        public void OnUpdate(float deltaTime)
        {
            foreach (var entity in _filter)
            {
                ref var fpCameraComponent = ref _fpCameraStash.Get(entity);
                ref var blockerComponent = ref _blockWhileRotationStash.Get(entity);
                ref var hingeRotationComponent = ref _hingeRotationStash.Get(blockerComponent.HingeRotation.Entity);
                
                FlagApplier.HandleFlagCondition(ref fpCameraComponent.RotationBlockFlag, 
                    FPCameraBlockers.HINGE_ROTATION, hingeRotationComponent.IsRotating);
            }
        }

        public void Dispose()
        {
        }
    }
}