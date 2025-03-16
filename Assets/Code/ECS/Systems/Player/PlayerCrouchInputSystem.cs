using EscapeRooms.Components;
using Scellecs.Morpeh;
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
        private Stash<CharacterCrouchComponent> _characterHeightLerpStash;

        public void OnAwake()
        {
            _filter = World.Filter
                .With<InputComponent>()
                .With<CharacterCrouchComponent>()
                .With<PlayerTag>()
                .Build();

            _inputStash = World.GetStash<InputComponent>();
            _characterHeightLerpStash = World.GetStash<CharacterCrouchComponent>();
        }

        public void OnUpdate(float deltaTime)
        {
            foreach (var entity in _filter)
            {
                ref var inputComponent = ref _inputStash.Get(entity);
                ref var characterHeightLerpComponent = ref _characterHeightLerpStash.Get(entity);

                characterHeightLerpComponent.CrouchInput = inputComponent.CrouchTrigger;
            }
        }

        public void Dispose()
        {
        }
    }
}