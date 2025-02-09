using EscapeRooms.Components;
using Scellecs.Morpeh;
using UnityEngine;
using Unity.IL2CPP.CompilerServices;

namespace EscapeRooms.Systems
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    public sealed class PlayerMovementSystem : ISystem
    {
        public World World { get; set; }

        private Filter _filter;
        private Stash<TransformComponent> _transformStash;
        private Stash<InputComponent> _inputStash;
        private Stash<MovementComponent> _movementStash;

        public void OnAwake()
        {
            _filter = World.Filter
                .With<TransformComponent>()
                .With<InputComponent>()
                .With<MovementComponent>()
                .With<PlayerComponent>()
                .Build();

            _transformStash = World.GetStash<TransformComponent>();
            _inputStash = World.GetStash<InputComponent>();
            _movementStash = World.GetStash<MovementComponent>();
        }

        public void OnUpdate(float deltaTime)
        {
            foreach (var entity in _filter)
            {
                ref var transformComponent = ref _transformStash.Get(entity);
                ref var inputComponent = ref _inputStash.Get(entity);
                ref var movementComponent = ref _movementStash.Get(entity);

                Vector2 moveVector = inputComponent.MoveAction.ReadValue<Vector2>();
                Vector3 moveDirection = (transformComponent.Transform.right * moveVector.x +
                                         transformComponent.Transform.forward * moveVector.y).normalized;

                movementComponent.CurrentVelocity = moveDirection * movementComponent.Speed;
                movementComponent.IsMoving = inputComponent.MoveAction.inProgress;
            }
        }

        public void Dispose()
        {
        }
    }
}