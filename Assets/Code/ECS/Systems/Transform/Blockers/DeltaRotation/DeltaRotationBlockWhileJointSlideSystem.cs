using EscapeRooms.Components;
using EscapeRooms.Helpers;
using Scellecs.Morpeh;
using Unity.IL2CPP.CompilerServices;

namespace EscapeRooms.Systems
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    public sealed class DeltaRotationBlockWhileJointSlideSystem : ISystem
    {
        public World World { get; set; }

        private Filter _filter;
        private Stash<TransformDeltaRotationComponent> _deltaRotationStash;
        private Stash<DeltaRotationBlockWhileHingeRotationComponent> _blockerStash;
        private Stash<JointSlideComponent> _jointSlideStash;

        public void OnAwake()
        {
            _filter = World.Filter
                .With<TransformDeltaRotationComponent>()
                .With<DeltaRotationBlockWhileHingeRotationComponent>()
                .Build();

            _deltaRotationStash = World.GetStash<TransformDeltaRotationComponent>();
            _blockerStash = World.GetStash<DeltaRotationBlockWhileHingeRotationComponent>();
            _jointSlideStash = World.GetStash<JointSlideComponent>();
        }

        public void OnUpdate(float deltaTime)
        {
            foreach (var entity in _filter)
            {
                ref var deltaRotationComponent = ref _deltaRotationStash.Get(entity);
                ref var blockerComponent = ref _blockerStash.Get(entity);
                ref var jointSlideComponent = ref _jointSlideStash.Get(blockerComponent.DragRotation.Entity);

                FlagApplier.HandleFlagCondition(ref deltaRotationComponent.RotationBlockFlag, 
                    BodyRotationBlockers.JOINT_SLIDE, jointSlideComponent.IsSliding);
            }
        }

        public void Dispose()
        {
        }
    }
}