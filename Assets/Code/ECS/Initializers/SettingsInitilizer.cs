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
        
        public void OnAwake()
        {
            SetSettings(LoadSettings());
        }

        public GameSettings LoadSettings()
        {
            return new GameSettings()
            {
                TargetFrameRate = 0,
                Sensitivity = 0.1f,
                CrouchInputTriggerDelay = 0.1f,
                JumpInputTriggerDelay = 0.1f
            };
        }
        
        public void SetSettings(GameSettings settings)
        {
            if (!GameSettings.TrySetInstance(settings))
                Debug.LogError("Game settings already has instance");

            Application.targetFrameRate = settings.TargetFrameRate;
            Cursor.lockState = CursorLockMode.Locked;
        }

        public void Dispose()
        {
            if (!GameSettings.TryRemoveInstance())
                Debug.LogError("Game settings already disposed");
        }
    }
}