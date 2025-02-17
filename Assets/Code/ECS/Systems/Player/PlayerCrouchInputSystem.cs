using EscapeRooms.Components;
using Scellecs.Morpeh;
using UnityEngine;
using Unity.IL2CPP.CompilerServices;

namespace EscapeRooms.Systems
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    public sealed class PlayerCrouchInputSystem : ISystem
    {
        public World World { get; set; }

        private Filter _filter;
        private Stash<InputComponent> _inputStash;
        private Stash<CharacterCrouchComponent> _characterCrouchStash;

        public void OnAwake()
        {
            _filter = World.Filter
                .With<InputComponent>()
                .With<CharacterCrouchComponent>()
                .With<PlayerComponent>()
                .Build();

            _inputStash = World.GetStash<InputComponent>();
            _characterCrouchStash = World.GetStash<CharacterCrouchComponent>();
        }

        public void OnUpdate(float deltaTime)
        {
            foreach (var entity in _filter)
            {
                ref var inputComponent = ref _inputStash.Get(entity);
                ref var characterCrouchComponent = ref _characterCrouchStash.Get(entity);

                characterCrouchComponent.CrouchInput = inputComponent.CrouchAction.triggered;
            }
        }

        public void Dispose()
        {
        }
    }
}