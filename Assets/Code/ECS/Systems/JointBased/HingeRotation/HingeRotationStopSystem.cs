using EscapeRooms.Components;
using Scellecs.Morpeh;
using Unity.IL2CPP.CompilerServices;
using UnityEngine;

namespace EscapeRooms.Systems
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    public sealed class HingeRotationStopSystem : ISystem
    {
        public World World { get; set; }

        private Filter _filter;
        private Stash<HingeRotationComponent> _rotationStash;
        private Stash<ConfigurableJointComponent> _jointStash;
        private Stash<OnHingeRotationFlag> _onRotateStash;
        private Stash<RigidbodyComponent> _rigidbodyStash;
        private Stash<HingeRotatableComponent> _rotatableStash;

        public void OnAwake()
        {
            _filter = World.Filter
                .With<HingeRotationComponent>()
                .Build();

            _rotationStash = World.GetStash<HingeRotationComponent>();
            _jointStash = World.GetStash<ConfigurableJointComponent>();
            _onRotateStash = World.GetStash<OnHingeRotationFlag>();
            _rigidbodyStash = World.GetStash<RigidbodyComponent>();
            _rotatableStash = World.GetStash<HingeRotatableComponent>();
        }

        public void OnUpdate(float deltaTime)
        {
            foreach (var entity in _filter)
            {
                ref var rotationComponent = ref _rotationStash.Get(entity);

                if (rotationComponent.RotateStopInput && rotationComponent.IsRotating)
                {
                    ref var jointComponent = ref _jointStash.Get(rotationComponent.RotatableEntity);
                    ref var onRotateFlag = ref _onRotateStash.Get(rotationComponent.RotatableEntity);
                    ref var rotatableComponent = ref _rotatableStash.Get(rotationComponent.RotatableEntity);
                    ref var itemRigidbodyComponent = ref _rigidbodyStash.Get(rotationComponent.RotatableEntity);
                    
                    itemRigidbodyComponent.Rigidbody.mass = rotatableComponent.MassBeforeRotate;
                    
                    jointComponent.ConfigurableJoint.targetRotation = Quaternion.identity;
                    jointComponent.ConfigurableJoint.angularXDrive = new JointDrive()
                    {
                        positionSpring = default,
                        positionDamper = default,
                        maximumForce = float.MaxValue
                    };
                    
                    Entity rotatingEntity = rotationComponent.RotatableEntity;
                    FlagDisposeSystem.ScheduleFlagDispose(ref onRotateFlag, () =>
                    {
                        _onRotateStash.Remove(rotatingEntity);
                    });
                    
                    rotationComponent.IsRotating = false;
                }
            }
        }

        public void Dispose()
        {
        }
    }
}