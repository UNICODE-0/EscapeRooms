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

        public void OnAwake()
        {
            _filter = World.Filter
                .With<HingeRotationComponent>()
                .Build();

            _rotationStash = World.GetStash<HingeRotationComponent>();
            _jointStash = World.GetStash<ConfigurableJointComponent>();
            _onRotateStash = World.GetStash<OnHingeRotationFlag>();
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