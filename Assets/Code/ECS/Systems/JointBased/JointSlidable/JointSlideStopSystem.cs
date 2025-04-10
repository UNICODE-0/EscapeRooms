using EscapeRooms.Components;
using EscapeRooms.Events;
using EscapeRooms.Helpers;
using Scellecs.Morpeh;
using Scellecs.Morpeh.Collections;
using Scellecs.Morpeh.Providers;
using Unity.IL2CPP.CompilerServices;
using UnityEngine;

namespace EscapeRooms.Systems
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    public sealed class JointSlideStopSystem : ISystem
    {
        public World World { get; set; }

        private Filter _filter;
        private Stash<JointSlideComponent> _slideStash;
        private Stash<OnJointSlideFlag> _onSlideStash;
        private Stash<ConfigurableJointComponent> _jointStash;

        public void OnAwake()
        {
            _filter = World.Filter
                .With<JointSlideComponent>()
                .Build();

            _slideStash = World.GetStash<JointSlideComponent>();
            _onSlideStash = World.GetStash<OnJointSlideFlag>();
            _jointStash = World.GetStash<ConfigurableJointComponent>();
        }

        public void OnUpdate(float deltaTime)
        {
            foreach (var entity in _filter)
            {
                ref var slideComponent = ref _slideStash.Get(entity);

                if (slideComponent.SlideStopInput && slideComponent.IsSliding)
                {
                    ref var jointComponent = ref _jointStash.Get(slideComponent.SlidableEntity);
                    ref var onSlideFlag = ref _onSlideStash.Get(slideComponent.SlidableEntity);

                    jointComponent.ConfigurableJoint.targetPosition = default;
                    jointComponent.ConfigurableJoint.xDrive = new JointDrive()
                    {
                        positionSpring = default,
                        positionDamper = default,
                        maximumForce = float.MaxValue
                    };
                    
                    Entity slidableEntity = slideComponent.SlidableEntity;
                    FlagDisposeSystem.ScheduleFlagDispose(ref onSlideFlag, () =>
                    {
                        _onSlideStash.Remove(slidableEntity);
                    });

                    slideComponent.IsSliding = false;
                }
            }
        }

        public void Dispose()
        {
        }
    }
}