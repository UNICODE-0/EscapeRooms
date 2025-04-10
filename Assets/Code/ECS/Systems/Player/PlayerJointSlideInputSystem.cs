using EscapeRooms.Components;
using EscapeRooms.Data;
using Scellecs.Morpeh;
using Unity.IL2CPP.CompilerServices;

namespace EscapeRooms.Systems
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    public sealed class PlayerJointSlideInputSystem : ISystem
    {
        public World World { get; set; }

        private Filter _filter;
        private Stash<InputComponent> _inputStash;
        private Stash<JointSlideComponent> _slideStash;

        public void OnAwake()
        {
            _filter = World.Filter
                .With<InputComponent>()
                .With<JointSlideComponent>()
                .With<PlayerHandTag>()
                .Build();

            _inputStash = World.GetStash<InputComponent>();
            _slideStash = World.GetStash<JointSlideComponent>();
        }

        public void OnUpdate(float deltaTime)
        {
            foreach (var entity in _filter)
            {
                ref var inputComponent = ref _inputStash.Get(entity);
                ref var slideComponent = ref _slideStash.Get(entity);

                slideComponent.SlideStartInput = inputComponent.InteractStartTrigger;
                slideComponent.SlideStopInput = inputComponent.InteractStopInProgress;
                slideComponent.SlideDeltaInput = inputComponent.LookValue.y * GameSettings.Instance.Sensitivity;
            }
        }

        public void Dispose()
        {
        }
    }
}