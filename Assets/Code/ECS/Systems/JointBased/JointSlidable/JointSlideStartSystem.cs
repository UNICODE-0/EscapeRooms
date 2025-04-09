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
    public sealed class JointSlideStartSystem : ISystem
    {
        public World World { get; set; }

        private Filter _filter;
        private Stash<JointSlideComponent> _slideStash;
        private Stash<RaycastComponent> _raycastStash;
        private Stash<JointSlidableComponent> _slidableStash;
        private Stash<OnJointSlideFlag> _onSlideStash;
        private Stash<ConfigurableJointComponent> _jointStash;

        public void OnAwake()
        {
            _filter = World.Filter
                .With<JointSlideComponent>()
                .Build();

            _slideStash = World.GetStash<JointSlideComponent>();
            _raycastStash = World.GetStash<RaycastComponent>();
            _slidableStash = World.GetStash<JointSlidableComponent>();
            _onSlideStash = World.GetStash<OnJointSlideFlag>();
            _jointStash = World.GetStash<ConfigurableJointComponent>();
        }

        public void OnUpdate(float deltaTime)
        {
            foreach (var entity in _filter)
            {
                ref var slideComponent = ref _slideStash.Get(entity);

                if (slideComponent.SlideStartInput && !slideComponent.IsSliding)
                {
                    ref var raycastComponent = ref _raycastStash.Get(slideComponent.DetectionRaycast.Entity);
                    
                    if (raycastComponent.HitsCount > 0 && 
                        EntityProvider.map.TryGetValue(raycastComponent.Hits[0].collider.gameObject.GetInstanceID(), out var item))
                    {
                        ref var slidableComponent = ref _slidableStash.Get(item.entity, out bool slidableExist);
                        if (slidableExist)
                        {
                            ref var jointComponent = ref _jointStash.Get(item.entity);
                            
                            jointComponent.ConfigurableJoint.xDrive = new JointDrive()
                            {
                                positionSpring = slidableComponent.Spring,
                                positionDamper = slidableComponent.Damper,
                                maximumForce = float.MaxValue
                            };
                            
                            _onSlideStash.Add(item.entity, new OnJointSlideFlag()
                            {
                                Owner = entity
                            });
                            
                            slideComponent.SlidableEntity = item.entity;
                            slideComponent.IsSliding = true;
                        }
                    }
                }
            }
        }

        public void Dispose()
        {
        }
    }
}