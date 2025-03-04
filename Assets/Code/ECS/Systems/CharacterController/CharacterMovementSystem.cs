using EscapeRooms.Components;
using Scellecs.Morpeh;
using UnityEngine;
using Unity.IL2CPP.CompilerServices;

namespace EscapeRooms.Systems
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    public sealed class CharacterMovementSystem : ISystem
    {
        public World World { get; set; }

        private Filter _filter;
        private Stash<TransformComponent> _transformStash;
        private Stash<CharacterMovementComponent> _movementStash;

        public void OnAwake()
        {
            _filter = World.Filter
                .With<TransformComponent>()
                .With<CharacterMovementComponent>()
                .Build();

            _transformStash = World.GetStash<TransformComponent>();
            _movementStash = World.GetStash<CharacterMovementComponent>();
        }

        public void OnUpdate(float deltaTime)
        {
            foreach (var entity in _filter)
            {
                ref var transformComponent = ref _transformStash.Get(entity);
                ref var movementComponent = ref _movementStash.Get(entity);

                Vector2 moveLocalDir = movementComponent.MoveLocalDirection;

                if (moveLocalDir == Vector2.zero)
                {
                    movementComponent.CurrentVelocity = Vector3.zero;
                    movementComponent.IsMoving = false;
                    continue;
                }
                
                Vector3 moveDirection = (transformComponent.Transform.right * moveLocalDir.x +
                                         transformComponent.Transform.forward * moveLocalDir.y).normalized;

                movementComponent.CurrentVelocity = moveDirection * movementComponent.Speed;
                movementComponent.IsMoving = true;
            }
        }

        public void Dispose()
        {
        }
    }
}