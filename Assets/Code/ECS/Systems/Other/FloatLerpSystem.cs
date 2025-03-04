using DG.Tweening;
using EscapeRooms.Components;
using Scellecs.Morpeh;
using UnityEngine;
using Unity.IL2CPP.CompilerServices;

namespace EscapeRooms.Systems
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    public sealed class FloatLerpSystem : ISystem
    {
        public World World { get; set; }

        private Filter _filter;
        private Stash<FloatLerpComponent> _floatLerpStash;

        public void OnAwake()
        {
            _filter = World.Filter
                .With<FloatLerpComponent>()
                .Build();

            _floatLerpStash = World.GetStash<FloatLerpComponent>();
        }

        public void OnUpdate(float deltaTime)
        {
            foreach (var entity in _filter)
            {
                ref var floatLerpComponent = ref _floatLerpStash.Get(entity);

                if (floatLerpComponent.IsLerpTimeIsUp)
                {
                    floatLerpComponent.IsLerpTimeIsUp = false;
                    floatLerpComponent.IsLerpInProgress = false;
                }

                if (floatLerpComponent.StartLerpInput && !floatLerpComponent.IsLerpInProgress)
                {
                    StartLerp(ref floatLerpComponent);
                }

                if (floatLerpComponent.IsLerpInProgress)
                {
                    HandleLerp(ref floatLerpComponent);
                }
            }
        }

        private void StartLerp(ref FloatLerpComponent floatLerpComponent)
        {
            floatLerpComponent.LerpStartTime = Time.time;
            floatLerpComponent.IsLerpInProgress = true;
        }

        private void HandleLerp(ref FloatLerpComponent floatLerpComponent)
        {
            if (InterpolateFloat(ref floatLerpComponent))
            {
                FinishLerp(ref floatLerpComponent);
            }
        }

        private bool InterpolateFloat(ref FloatLerpComponent floatLerpComponent)
        {
            if (floatLerpComponent.IsLerpPaused)
            {
                floatLerpComponent.LerpStartTime += Time.deltaTime;
                return false;
            }
            
            float timeElapsed = Time.time - floatLerpComponent.LerpStartTime;
            float scaledTime = Mathf.Clamp01(timeElapsed / floatLerpComponent.Time);

            floatLerpComponent.CurrentValue = 
                DOVirtual.EasedValue(floatLerpComponent.From, floatLerpComponent.To, scaledTime, floatLerpComponent.Ease);

            return scaledTime >= 1f;
        }

        private void FinishLerp(ref FloatLerpComponent floatLerpComponent)
        {
            floatLerpComponent.IsLerpTimeIsUp = true;
            floatLerpComponent.LerpStartTime = 0f;
        }

        public void Dispose()
        {
        }
    }
}