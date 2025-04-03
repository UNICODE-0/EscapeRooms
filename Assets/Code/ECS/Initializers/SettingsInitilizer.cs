using EscapeRooms.Data;
using Scellecs.Morpeh;
using UnityEngine;
using Unity.IL2CPP.CompilerServices;

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
                Sensitivity = 0.05f
#if UNITY_WEBGL && !UNITY_EDITOR
                * 0.25f
#endif
                ,
                CrouchInputTriggerDelay = 0.1f,
                JumpInputTriggerDelay = 0.1f
            };
        }
        
        public void SetSettings(GameSettings settings)
        {
            GameSettings.TrySetInstance(settings);

            Application.targetFrameRate = settings.TargetFrameRate;
            Cursor.lockState = CursorLockMode.Locked;
        }

        public void Dispose()
        {
            GameSettings.TryRemoveInstance();
        }
    }
}