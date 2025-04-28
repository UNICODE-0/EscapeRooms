using EscapeRooms.Components;
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
    public sealed class JointSlideStartSystem : ISystem
    {
        public World World { get; set; }

        private Filter _filter;
        private Stash<JointSlideComponent> _slideStash;
        private Stash<OneHitRaycastComponent> _raycastStash;
        private Stash<JointSlidableComponent> _slidableStash;
        private Stash<OnJointSlideFlag> _onSlideStash;
        private Stash<ConfigurableJointComponent> _jointStash;
        private Stash<TransformComponent> _transformStash;

        public void OnAwake()
        {
            _filter = World.Filter
                .With<JointSlideComponent>()
                .Build();

            _slideStash = World.GetStash<JointSlideComponent>();
            _raycastStash = World.GetStash<OneHitRaycastComponent>();
            _slidableStash = World.GetStash<JointSlidableComponent>();
            _onSlideStash = World.GetStash<OnJointSlideFlag>();
            _jointStash = World.GetStash<ConfigurableJointComponent>();
            _transformStash = World.GetStash<TransformComponent>();
        }

        public void OnUpdate(float deltaTime)
        {
            foreach (var entity in _filter)
            {
                ref var slideComponent = ref _slideStash.Get(entity);

                if (slideComponent.SlideStartInput && !slideComponent.IsSliding)
                {
                    ref var raycastComponent = ref _raycastStash.Get(slideComponent.DetectionRaycast.Entity);
                    
                    if(!RaycastExtension.GetHitEntity(ref raycastComponent, out Entity hitEntity))
                        continue;
                    
                    ref var slidableComponent = ref _slidableStash.Get(hitEntity, out bool slidableExist);
                    if (!slidableExist)
                        continue;
                        
                    ref var transformComponent = ref _transformStash.Get(hitEntity);
                    ref var jointComponent = ref _jointStash.Get(hitEntity);

                    float distanceToOrigin =
                        Vector3.Distance(
                            transformComponent.Transform.localPosition + jointComponent.ConfigurableJoint.anchor,
                            slidableComponent.Origin.localPosition);

                    jointComponent.ConfigurableJoint.targetPosition =
                        slidableComponent.SlideDirection * distanceToOrigin;
                    
                    jointComponent.ConfigurableJoint.xDrive = new JointDrive()
                    {
                        positionSpring = slidableComponent.Spring,
                        positionDamper = slidableComponent.Damper,
                        maximumForce = float.MaxValue
                    };
                    
                    _onSlideStash.Add(hitEntity, new OnJointSlideFlag()
                    {
                        Owner = entity
                    });
                    
                    slideComponent.SlidableEntity = hitEntity;
                    slideComponent.IsSliding = true;
                }
            }
        }

        public void Dispose()
        {
        }
    }
}