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
    public sealed class HingeRotationSystem : ISystem
    {
        public World World { get; set; }

        private Filter _filter;
        private Stash<HingeRotatableComponent> _rotatableStash;
        private Stash<HingeRotationComponent> _rotationStash;
        private Stash<ConfigurableJointComponent> _jointStash;
        private Stash<TransformComponent> _transformStash;
        private Stash<OnHingeRotationFlag> _onRotationStash;

        public void OnAwake()
        {
            _filter = World.Filter
                .With<HingeRotatableComponent>()
                .With<TransformComponent>()
                .With<OnHingeRotationFlag>()
                .Build();

            _rotatableStash = World.GetStash<HingeRotatableComponent>();
            _jointStash = World.GetStash<ConfigurableJointComponent>();
            _rotationStash = World.GetStash<HingeRotationComponent>();
            _onRotationStash = World.GetStash<OnHingeRotationFlag>();
            _transformStash = World.GetStash<TransformComponent>();
        }

        public void OnUpdate(float deltaTime)
        {
            foreach (var entity in _filter)
            {
                ref var rotatableComponent = ref _rotatableStash.Get(entity);
                ref var jointComponent = ref _jointStash.Get(entity);
                
                ref var onRotationComponent = ref _onRotationStash.Get(entity);
                ref var transformComponent = ref _transformStash.Get(entity);
                ref var rotationComponent = ref _rotationStash.Get(onRotationComponent.Owner);

                Quaternion rotationX = Quaternion.AngleAxis(
                    -rotationComponent.RotateDeltaInput * rotationComponent.RotationSpeed,
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