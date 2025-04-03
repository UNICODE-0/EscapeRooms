using Scellecs.Morpeh;
using UnityEngine;
using Unity.IL2CPP.CompilerServices;

namespace EscapeRooms.Systems
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    public sealed class FrameRateChangeSystem : ISystem
    {
        public World World { get; set; }

        private readonly int[] FrameRatePresets = { 15, 30, 60, 100, 144 };
        private int CurrentPreset;

        public void OnAwake()
        {
        }

        public void OnUpdate(float deltaTime)
        {
            if (Input.GetKeyDown(KeyCode.RightControl))
            {
                Application.targetFrameRate = FrameRatePresets[CurrentPreset];
                CurrentPreset++;
                if (CurrentPreset >= FrameRatePresets.Length) CurrentPreset = 0;
            }
        }

        public void Dispose()
        {
        }
    }
}