using EscapeRooms.Components;
using EscapeRooms.Helpers;
using Scellecs.Morpeh;
using Unity.IL2CPP.CompilerServices;

namespace EscapeRooms.Systems
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    public sealed class FPCameraBlockWhileJointSlidingSystem : ISystem
    {
        public World World { get; set; }

        private Filter _filter;
        private Stash<JointSlideComponent> _jointSlideStash;
        private Stash<FPCameraComponent> _fpCameraStash;
        private Stash<FPCameraBlockWhileJointSlidingComponent> _blockWhileSlidingStash;

        public void OnAwake()
        {
            _filter = World.Filter
                .With<FPCameraComponent>()
                .With<FPCameraBlockWhileJointSlidingComponent>()
                .Build();

            _jointSlideStash = World.GetStash<JointSlideComponent>();
            _fpCameraStash = World.GetStash<FPCameraComponent>();
            _blockWhileSlidingStash = World.GetStash<FPCameraBlockWhileJointSlidingComponent>();
        }

        public void OnUpdate(float deltaTime)
        {
            foreach (var entity in _filter)
            {
                ref var fpCameraComponent = ref _fpCameraStash.Get(entity);
                ref var blockerComponent = ref _blockWhileSlidingStash.Get(entity);
                ref var jointSlideComponent = ref _jointSlideStash.Get(blockerComponent.JointSlide.Entity);
                
                FlagApplier.HandleFlagCondition(ref fpCameraComponent.RotationBlockFlag, 
                    FPCameraBlockers.JOINT_SLIDE, jointSlideComponent.IsSliding);
            }
        }

        public void Dispose()
        {
        }
    }
}