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
    public sealed class RotateStopSystem : ISystem
    {
        public World World { get; set; }

        private Filter _filter;
        private Stash<RaycastComponent> _raycastStash;
        private Stash<RotateComponent> _rotateStash;
        private Stash<RotatableComponent> _rotatableStash;
        private Stash<ConfigurableJointComponent> _jointStash;
        private Stash<OnRotateFlag> _onRotateStash;

        public void OnAwake()
        {
            _filter = World.Filter
                .With<RotateComponent>()
                .Build();

            _raycastStash = World.GetStash<RaycastComponent>();
            _rotateStash = World.GetStash<RotateComponent>();
            _rotatableStash = World.GetStash<RotatableComponent>();
            _jointStash = World.GetStash<ConfigurableJointComponent>();
            _onRotateStash = World.GetStash<OnRotateFlag>();
        }

        public void OnUpdate(float deltaTime)
        {
            foreach (var entity in _filter)
            {
                ref var rotateComponent = ref _rotateStash.Get(entity);

                if (rotateComponent.RotateStopInput && rotateComponent.IsRotating)
                {
                    ref var rotatableComponent = ref _rotatableStash.Get(rotateComponent.RotatingEntity);
                    ref var jointComponent = ref _jointStash.Get(rotateComponent.RotatingEntity);
                    ref var onRotateFlag = ref _onRotateStash.Get(rotateComponent.RotatingEntity);
                    
                    jointComponent.ConfigurableJoint.targetRotation = Quaternion.identity;
                    jointComponent.ConfigurableJoint.angularXDrive = new JointDrive()
                    {
                        positionSpring = default,
                        positionDamper = default,
                        maximumForce = float.MaxValue
                    };
                    
                    Entity rotatingEntity = rotateComponent.RotatingEntity;
                    FlagDisposeSystem.ScheduleFlagDispose(ref onRotateFlag, () =>
                    {
                        _onRotateStash.Remove(rotatingEntity);
                    });
                    
                    rotateComponent.IsRotating = false;
                }
            }
        }

        public void Dispose()
        {
        }
    }
}