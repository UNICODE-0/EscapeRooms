using EscapeRooms.Components;
using Scellecs.Morpeh;
using Unity.IL2CPP.CompilerServices;

namespace EscapeRooms.Systems
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    public sealed class PlayerMovementInputSystem : ISystem
    {
        public World World { get; set; }

        private Filter _filter;
        private Stash<InputComponent> _inputStash;
        private Stash<CharacterMovementComponent> _movementStash;

        public void OnAwake()
        {
            _filter = World.Filter
                .With<InputComponent>()
                .With<CharacterMovementComponent>()
                .With<PlayerTag>()
                .Build();

            _inputStash = World.GetStash<InputComponent>();
            _movementStash = World.GetStash<CharacterMovementComponent>();
        }

        public void OnUpdate(float deltaTime)
        {
            foreach (var entity in _filter)
            {
                ref var inputComponent = ref _inputStash.Get(entity);
                ref var movementComponent = ref _movementStash.Get(entity);

                movementComponent.MoveLocalDirection = inputComponent.MoveValue;
            }
        }

        public void Dispose()
        {
        }
    }
}