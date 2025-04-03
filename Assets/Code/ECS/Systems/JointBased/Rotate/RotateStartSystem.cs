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
    public sealed class RotateStartSystem : ISystem
    {
        public World World { get; set; }

        private Filter _filter;
        private Stash<RaycastComponent> _raycastStash;
        private Stash<RotateComponent> _rotateStash;
        private Stash<RotatableComponent> _rotatableStash;
        private Stash<HingeJointComponent> _hingeJointStash;
        private Stash<OnRotateFlag> _onRotateStash;

        public void OnAwake()
        {
            _filter = World.Filter
                .With<RotateComponent>()
                .Build();

            _raycastStash = World.GetStash<RaycastComponent>();
            _rotateStash = World.GetStash<RotateComponent>();
            _rotatableStash = World.GetStash<RotatableComponent>();
            _hingeJointStash = World.GetStash<HingeJointComponent>();
            _onRotateStash = World.GetStash<OnRotateFlag>();
        }

        public void OnUpdate(float deltaTime)
        {
            foreach (var entity in _filter)
            {
                ref var rotateComponent = ref _rotateStash.Get(entity);

                if (rotateComponent.RotateStartInput && !rotateComponent.IsRotating)
                {
                    ref var raycastComponent = ref _raycastStash.Get(rotateComponent.RotateRaycast.Entity);
                    
                    if (raycastComponent.HitsCount > 0 && 
                        EntityProvider.map.TryGetValue(raycastComponent.Hits[0].collider.gameObject.GetInstanceID(), out var item))
                    {
                        ref var rotatableComponent = ref _rotatableStash.Get(item.entity, out bool rotatableExist);
                        if (rotatableExist)
                        {
                            ref var hingeComponent = ref _hingeJointStash.Get(item.entity);

                            rotateComponent.IsRotating = true;
                            rotateComponent.RotatingEntity = item.entity;
                            
                            hingeComponent.HingeJoint.useSpring = true;
                            hingeComponent.HingeJoint.spring = new JointSpring()
                            {
                                spring = rotatableComponent.Spring,
                                damper = rotatableComponent.Damper,
                                targetPosition = 0f
                            };

                            _onRotateStash.Add(item.entity, new OnRotateFlag()
                            {
                                Owner = entity,
                            });
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