using EscapeRooms.Components;
using EscapeRooms.Events;
using EscapeRooms.Helpers;
using EscapeRooms.Mono;
using Scellecs.Morpeh;
using Sirenix.Utilities;
using Unity.IL2CPP.CompilerServices;
using UnityEngine;

namespace EscapeRooms.Systems
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    public sealed class DragRotationSystem : ISystem
    {
        public World World { get; set; }

        private Filter _filter;
        private Stash<DragComponent> _dragStash;
        private Stash<DragRotationComponent> _dragRotationStash;
        private Stash<ConfigurableJointComponent> _jointStash;
        private Stash<TransformComponent> _transformStash;

        public void OnAwake()
        {
            _filter = World.Filter
                .With<DragComponent>()
                .With<DragRotationComponent>()
                .Build();

            _dragStash = World.GetStash<DragComponent>();
            _dragRotationStash = World.GetStash<DragRotationComponent>();
            _jointStash = World.GetStash<ConfigurableJointComponent>();
            _transformStash = World.GetStash<TransformComponent>();
        }

        public void OnUpdate(float deltaTime)
        {
            foreach (var entity in _filter)
            {
                ref var dragComponent = ref _dragStash.Get(entity);
                ref var rotationComponent = ref _dragRotationStash.Get(entity);

                if (dragComponent.IsDragging && rotationComponent.RotationActiveInput)
                {
                    ref var transformComponent = ref _transformStash.Get(entity);
                    ref var jointComponent = ref _jointStash.Get(dragComponent.DraggableEntity);

                    Vector2 input =
                        rotationComponent.RotationDeltaInput.Clamp(rotationComponent.MinInputDelta,
                            rotationComponent.MaxInputDelta) * rotationComponent.RotationSpeed;

                    Vector3 worldRightAxis = transformComponent.Transform.right;
                    Vector3 worldUpAxis = transformComponent.Transform.up;

                    Vector3 localRightAxis = Quaternion.Inverse(jointComponent.ConfigurableJoint.transform.rotation) * worldRightAxis;
                    Vector3 localUpAxis = Quaternion.Inverse(jointComponent.ConfigurableJoint.transform.rotation) * worldUpAxis;

                    Quaternion rotationX = Quaternion.AngleAxis(-input.y, localRightAxis);
                    Quaternion rotationY = Quaternion.AngleAxis(input.x, localUpAxis);

                    jointComponent.ConfigurableJoint.targetRotation = 
                        rotationX * rotationY * jointComponent.ConfigurableJoint.targetRotation;
                    
                    rotationComponent.IsRotating = true;
                }
                else
                {
                    rotationComponent.IsRotating = false;
                }
            }
        }
        
        public void Dispose()
        {
        }
    }
}