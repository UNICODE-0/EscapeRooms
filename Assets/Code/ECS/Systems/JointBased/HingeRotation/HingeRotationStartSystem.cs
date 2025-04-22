using EscapeRooms.Components;
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
    public sealed class HingeRotationStartSystem : ISystem
    {
        public World World { get; set; }

        private Filter _filter;
        private Stash<OneHitRaycastComponent> _raycastStash;
        private Stash<HingeRotationComponent> _rotationStash;
        private Stash<HingeRotatableComponent> _rotatableStash;
        private Stash<ConfigurableJointComponent> _jointStash;
        private Stash<OnHingeRotationFlag> _onRotateStash;
        private Stash<TransformComponent> _transformStash;
        private Stash<RigidbodyComponent> _rigidbodyStash;


        public void OnAwake()
        {
            _filter = World.Filter
                .With<HingeRotationComponent>()
                .Build();

            _raycastStash = World.GetStash<OneHitRaycastComponent>();
            _rotationStash = World.GetStash<HingeRotationComponent>();
            _rotatableStash = World.GetStash<HingeRotatableComponent>();
            _jointStash = World.GetStash<ConfigurableJointComponent>();
            _onRotateStash = World.GetStash<OnHingeRotationFlag>();
            _transformStash = World.GetStash<TransformComponent>();
            _rigidbodyStash = World.GetStash<RigidbodyComponent>();
        }

        public void OnUpdate(float deltaTime)
        {
            foreach (var entity in _filter)
            {
                ref var rotationComponent = ref _rotationStash.Get(entity);

                if (rotationComponent.RotateStartInput && !rotationComponent.IsRotating)
                {
                    ref var raycastComponent = ref _raycastStash.Get(rotationComponent.DetectionRaycast.Entity);
                    
                    if (raycastComponent.IsRayHit && 
                        EntityProvider.map.TryGetValue(raycastComponent.Hit.collider.gameObject.GetInstanceID(), out var item))
                    {
                        ref var rotatableComponent = ref _rotatableStash.Get(item.entity, out bool rotatableExist);
                        if (rotatableExist)
                        {
                            ref var jointComponent = ref _jointStash.Get(item.entity);
                            ref var transformComponent = ref _transformStash.Get(item.entity);
                            ref var itemRigidbodyComponent = ref _rigidbodyStash.Get(item.entity);

                            rotationComponent.IsRotating = true;
                            rotationComponent.RotatableEntity = item.entity;
                            
                            rotatableComponent.MassBeforeRotate = itemRigidbodyComponent.Rigidbody.mass;
                            itemRigidbodyComponent.Rigidbody.mass = rotatableComponent.MassWhileRotate;

                            jointComponent.ConfigurableJoint.targetRotation =
                                Quaternion.Inverse(transformComponent.Transform.rotation);
                            
                            jointComponent.ConfigurableJoint.angularXDrive = new JointDrive()
                            {
                                positionSpring = rotatableComponent.Spring,
                                positionDamper = rotatableComponent.Damper,
                                maximumForce = float.MaxValue
                            };

                            _onRotateStash.Add(item.entity, new OnHingeRotationFlag()
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