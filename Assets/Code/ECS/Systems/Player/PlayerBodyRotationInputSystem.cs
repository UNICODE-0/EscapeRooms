using EscapeRooms.Components;
using Scellecs.Morpeh;
using UnityEngine;
using Unity.IL2CPP.CompilerServices;

namespace EscapeRooms.Systems
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    public sealed class PlayerBodyRotationInputSystem : ISystem
    {
        public World World { get; set; }

        private Filter _filter;
        private Stash<TransformDeltaRotationComponent> _rotationStash;
        private Stash<InputComponent> _inputStash;
        private Stash<SettingsComponent> _settingsStash;

        public void OnAwake()
        {
            _filter = World.Filter
                .With<TransformDeltaRotationComponent>()
                .With<InputComponent>()
                .With<SettingsComponent>()
                .With<PlayerComponent>()
                .Build();

            _rotationStash = World.GetStash<TransformDeltaRotationComponent>();
            _inputStash = World.GetStash<InputComponent>();
            _settingsStash = World.GetStash<SettingsComponent>();
        }

        public void OnUpdate(float deltaTime)
        {
            foreach (var entity in _filter)
            {
                ref var rotationComponent = ref _rotationStash.Get(entity);
                ref var inputComponent = ref _inputStash.Get(entity);
                ref var settingsComponent = ref _settingsStash.Get(entity);

                Vector2 mouseDelta = inputComponent.LookAction.ReadValue<Vector2>();
                rotationComponent.EulerRotationDelta = 
                    new Vector3(0f, mouseDelta.x * settingsComponent.GameSettings.Sensitivity ,0f);
            }
        }

        public void Dispose()
        {
        }
    }
}