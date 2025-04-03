using EscapeRooms.Components;
using Scellecs.Morpeh;
using Unity.IL2CPP.CompilerServices;

namespace EscapeRooms.Systems
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    public sealed class PlayerJumpInputSystem : ISystem
    {
        public World World { get; set; }

        private Filter _filter;
        private Stash<InputComponent> _inputStash;
        private Stash<CharacterJumpComponent> _jumpStash;

        public void OnAwake()
        {
            _filter = World.Filter
                .With<InputComponent>()
                .With<CharacterJumpComponent>()
                .With<PlayerTag>()
                .Build();

            _inputStash = World.GetStash<InputComponent>();
            _jumpStash = World.GetStash<CharacterJumpComponent>();
        }

        public void OnUpdate(float deltaTime)
        {
            foreach (var entity in _filter)
            {
                ref var inputComponent = ref _inputStash.Get(entity);
                ref var jumpComponent = ref _jumpStash.Get(entity);
                
                jumpComponent.JumpInput = inputComponent.JumpTrigger;
            }
        }

        public void Dispose()
        {
        }
    }
}