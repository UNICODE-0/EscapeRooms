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
    public sealed class RotateSystem : ISystem
    {
        public World World { get; set; }

        private Filter _filter;
        private Stash<RotatableComponent> _rotatableStash;
        private Stash<RotateComponent> _rotateStash;
        private Stash<ConfigurableJointComponent> _jointStash;
        private Stash<TransformComponent> _transformStash;
        private Stash<OnRotateFlag> _onRotateStash;

        public void OnAwake()
        {
            _filter = World.Filter
                .With<RotatableComponent>()
                .With<TransformComponent>()
                .With<OnRotateFlag>()
                .Build();

            _rotatableStash = World.GetStash<RotatableComponent>();
            _jointStash = World.GetStash<ConfigurableJointComponent>();
            _rotateStash = World.GetStash<RotateComponent>();
            _onRotateStash = World.GetStash<OnRotateFlag>();
            _transformStash = World.GetStash<TransformComponent>();
        }

        public void OnUpdate(float deltaTime)
        {
            foreach (var entity in _filter)
            {
                ref var rotatableComponent = ref _rotatableStash.Get(entity);
                ref var jointComponent = ref _jointStash.Get(entity);
                
                ref var onRotateComponent = ref _onRotateStash.Get(entity);
                ref var transformComponent = ref _transformStash.Get(entity);
                ref var rotateComponent = ref _rotateStash.Get(onRotateComponent.Owner);

                Quaternion rotationX = Quaternion.AngleAxis(
                    -rotateComponent.RotateDeltaInput * rotateComponent.RotationSpeed,
                    transformComponent.Transform.right);
                
                Quaternion result = rotationX * jointComponent.ConfigurableJoint.targetRotation;

                float min = result.GetXAxisAngleInQuarter(rotatableComponent.MinAngleQuarter);
                float max = result.GetXAxisAngleInQuarter(rotatableComponent.MaxAngleQuarter);

                if(min >= rotatableComponent.MinAngle || max >= rotatableComponent.MaxAngle)
                    return;

                jointComponent.ConfigurableJoint.targetRotation = result;
            }
        }
        
        public void Dispose()
        {
        }
    }
}