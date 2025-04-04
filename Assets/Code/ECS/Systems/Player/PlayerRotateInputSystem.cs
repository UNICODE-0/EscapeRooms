using EscapeRooms.Components;
using EscapeRooms.Data;
using Scellecs.Morpeh;
using Unity.IL2CPP.CompilerServices;

namespace EscapeRooms.Systems
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    public sealed class PlayerRotateInputSystem : ISystem
    {
        public World World { get; set; }

        private Filter _filter;
        private Stash<InputComponent> _inputStash;
        private Stash<RotateComponent> _rotateStash;

        public void OnAwake()
        {
            _filter = World.Filter
                .With<InputComponent>()
                .With<RotateComponent>()
                .With<PlayerHandTag>()
                .Build();

            _inputStash = World.GetStash<InputComponent>();
            _rotateStash = World.GetStash<RotateComponent>();
        }

        public void OnUpdate(float deltaTime)
        {
            foreach (var entity in _filter)
            {
                ref var inputComponent = ref _inputStash.Get(entity);
                ref var rotateComponent = ref _rotateStash.Get(entity);

                rotateComponent.RotateDeltaInput = inputComponent.LookValue.x * GameSettings.Instance.Sensitivity;
                rotateComponent.RotateStartInput = inputComponent.InteractStartTrigger;
                rotateComponent.RotateStopInput = inputComponent.InteractStopInProgress;
            }
        }

        public void Dispose()
        {
        }
    }
}