using EscapeRooms.Components;
using EscapeRooms.Data;
using Scellecs.Morpeh;
using UnityEngine;
using Unity.IL2CPP.CompilerServices;
using UnityEngine.InputSystem;

namespace EscapeRooms.Initializers
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    public sealed class SettingsInitializer : IInitializer
    {
        public World World { get; set; }
        
        private Filter _filter;
        private Stash<SettingsComponent> _settingsStash;

        public void OnAwake()
        {
            _filter = World.Filter
                .With<SettingsComponent>()
                .Build();

            _settingsStash = World.GetStash<SettingsComponent>();
            
            SetSettings(LoadSettings());
        }

        public GameSettings LoadSettings()
        {
            return new GameSettings()
            {
                TargetFrameRate = 100,
                Sensitivity = 0.1f
            };
        }
        
        public void SetSettings(GameSettings settings)
        {
            foreach (var entity in _filter)
            {
                ref var settingsComponent = ref _settingsStash.Get(entity);
                settingsComponent.GameSettings = settings;
            }

            Application.targetFrameRate = settings.TargetFrameRate;
            Cursor.lockState = CursorLockMode.Locked;
        }

        public void Dispose()
        {
        }
    }
}