using EscapeRooms.Components;
using Scellecs.Morpeh;
using UnityEngine;
using Unity.IL2CPP.CompilerServices;

namespace EscapeRooms.Systems
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    public sealed class PlayerFPCameraInputSystem : ISystem
    {
        public World World { get; set; }

        private Filter _filter;
        private Stash<FPCameraComponent> _cameraStash;
        private Stash<InputComponent> _inputStash;
        private Stash<SettingsComponent> _settingsStash;

        public void OnAwake()
        {
            _filter = World.Filter
                .With<FPCameraComponent>()
                .With<InputComponent>()
                .With<SettingsComponent>()
                .With<PlayerCameraComponent>()
                .Build();

            _cameraStash = World.GetStash<FPCameraComponent>();
            _inputStash = World.GetStash<InputComponent>();
            _settingsStash = World.GetStash<SettingsComponent>();
        }

        public void OnUpdate(float deltaTime)
        {
            foreach (var entity in _filter)
            {
                ref var cameraComponent = ref _cameraStash.Get(entity);
                ref var inputComponent = ref _inputStash.Get(entity);
                ref var settingsComponent = ref _settingsStash.Get(entity);

                Vector2 mouseDelta = inputComponent.LookActionValue;
                
                Vector2 RotateDelta = Vector3.zero;
                RotateDelta.x = mouseDelta.y;
                RotateDelta.y = mouseDelta.x;

                cameraComponent.RotateDelta = RotateDelta * settingsComponent.GameSettings.Sensitivity;
            }
        }

        public void Dispose()
        {
        }
    }
}