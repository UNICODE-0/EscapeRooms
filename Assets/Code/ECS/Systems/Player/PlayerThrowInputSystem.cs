using EscapeRooms.Components;
using Scellecs.Morpeh;
using Unity.IL2CPP.CompilerServices;

namespace EscapeRooms.Systems
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    public sealed class PlayerThrowInputSystem : ISystem
    {
        public World World { get; set; }

        private Filter _filter;
        private Stash<InputComponent> _inputStash;
        private Stash<ThrowComponent> _throwStash;

        public void OnAwake()
        {
            _filter = World.Filter
                .With<InputComponent>()
                .With<ThrowComponent>()
                .With<PlayerHandTag>()
                .Build();

            _inputStash = World.GetStash<InputComponent>();
            _throwStash = World.GetStash<ThrowComponent>();
        }

        public void OnUpdate(float deltaTime)
        {
            foreach (var entity in _filter)
            {
                ref var inputComponent = ref _inputStash.Get(entity);
                ref var throwComponent = ref _throwStash.Get(entity);
                
                throwComponent.ThrowInput = inputComponent.ThrowTrigger;
            }
        }

        public void Dispose()
        {
        }
    }
}