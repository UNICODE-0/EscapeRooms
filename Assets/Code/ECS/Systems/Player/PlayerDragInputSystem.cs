using EscapeRooms.Components;
using Scellecs.Morpeh;
using Unity.IL2CPP.CompilerServices;

namespace EscapeRooms.Systems
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    public sealed class PlayerDragInputSystem : ISystem
    {
        public World World { get; set; }

        private Filter _filter;
        private Stash<InputComponent> _inputStash;
        private Stash<DragComponent> _dragStash;

        public void OnAwake()
        {
            _filter = World.Filter
                .With<InputComponent>()
                .With<DragComponent>()
                .With<PlayerHandTag>()
                .Build();

            _inputStash = World.GetStash<InputComponent>();
            _dragStash = World.GetStash<DragComponent>();
        }

        public void OnUpdate(float deltaTime)
        {
            foreach (var entity in _filter)
            {
                ref var inputComponent = ref _inputStash.Get(entity);
                ref var dragComponent = ref _dragStash.Get(entity);

                dragComponent.DragStartInput = inputComponent.InteractStartTrigger;
                dragComponent.DragStopInput = inputComponent.InteractStopInProgress;
            }
        }

        public void Dispose()
        {
        }
    }
}