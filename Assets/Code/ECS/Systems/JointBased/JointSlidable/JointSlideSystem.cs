using EscapeRooms.Components;
using Scellecs.Morpeh;
using Sirenix.Utilities;
using Unity.IL2CPP.CompilerServices;
using UnityEngine;

namespace EscapeRooms.Systems
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    public sealed class JointSlideSystem : ISystem
    {
        public World World { get; set; }

        private Filter _filter;
        private Stash<JointSlideComponent> _slideStash;
        private Stash<JointSlidableComponent> _slidableStash;
        private Stash<ConfigurableJointComponent> _jointStash;
        private Stash<OnJointSlideFlag> _onSlideStash;

        public void OnAwake()
        {
            _filter = World.Filter
                .With<JointSlidableComponent>()
                .With<OnJointSlideFlag>()
                .Build();

            _slideStash = World.GetStash<JointSlideComponent>();
            _slidableStash = World.GetStash<JointSlidableComponent>();
            _jointStash = World.GetStash<ConfigurableJointComponent>();
            _onSlideStash = World.GetStash<OnJointSlideFlag>();
        }

        public void OnUpdate(float deltaTime)
        {
            foreach (var entity in _filter)
            {
                ref var slidableComponent = ref _slidableStash.Get(entity);
                ref var onSlideComponent = ref _onSlideStash.Get(entity);
                ref var slideComponent = ref _slideStash.Get(onSlideComponent.Owner);
                ref var jointComponent = ref _jointStash.Get(entity);

                float input =
                    -Mathf.Clamp(slideComponent.SlideDeltaInput, slideComponent.DeltaRange.x,
                        slideComponent.DeltaRange.y) * slideComponent.SlideSpeed;

                Vector3 positionDelta = slidableComponent.SlideDirection *
                                        (slideComponent.InverseInput ? -input : input);
                Vector3 result = jointComponent.ConfigurableJoint.targetPosition + positionDelta;

                result = result.Clamp(slidableComponent.MinDistance, slidableComponent.MaxDistance);
                
                jointComponent.ConfigurableJoint.targetPosition = result;
            }
        }
        
        public void Dispose()
        {
        }
    }
}