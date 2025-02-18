using EscapeRooms.Components;
using Scellecs.Morpeh;
using UnityEngine;
using Unity.IL2CPP.CompilerServices;

namespace EscapeRooms.Systems
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    public sealed class CharacterHeightLerpSystem : ISystem
    {
        public World World { get; set; }

        private Filter _filter;
        private Stash<CharacterHeightLerpComponent> _crouchStash;
        private Stash<CharacterControllerComponent> _characterStash;

        public void OnAwake()
        {
            _filter = World.Filter
                .With<CharacterHeightLerpComponent>()
                .With<CharacterMovementComponent>()
                .Build();

            _crouchStash = World.GetStash<CharacterHeightLerpComponent>();
            _characterStash = World.GetStash<CharacterControllerComponent>();
        }

        public void OnUpdate(float deltaTime)
        {
            foreach (var entity in _filter)
            {
                ref var crouchComponent = ref _crouchStash.Get(entity);
                ref var characterComponent = ref _characterStash.Get(entity);

                if (crouchComponent.ChangeStateInput && !crouchComponent.IsChangeColliderInProgress)
                {
                    StartHeightChange(ref crouchComponent);
                }

                if (crouchComponent.IsChangeColliderInProgress)
                {
                    HandleHeightChange(characterComponent.CharacterController, ref crouchComponent);
                }
            }
        }

        private void StartHeightChange(ref CharacterHeightLerpComponent crouchComponent)
        {
            crouchComponent.LerpStartTime = Time.time;
            crouchComponent.IsChangeColliderInProgress = true;
        }

        private void HandleHeightChange(CharacterController charCon, 
            ref CharacterHeightLerpComponent crouchComponent)
        {
            ref CharacterHeightLerpState fromState = ref crouchComponent.IsTargetState 
                ? ref crouchComponent.TargetState 
                : ref crouchComponent.DefaultState;

            ref CharacterHeightLerpState toState = ref crouchComponent.IsTargetState 
                ? ref crouchComponent.DefaultState 
                : ref crouchComponent.TargetState;

            if (InterpolateHeight(charCon, ref fromState, ref toState, crouchComponent.LerpStartTime))
            {
                FinishHeightChange(ref crouchComponent);
            }
        }

        private bool InterpolateHeight(CharacterController charCon, 
            ref CharacterHeightLerpState from, ref CharacterHeightLerpState to, float startTime)
        {
            float timeElapsed = Time.time - startTime;
            float scaledTime = Mathf.Clamp01(timeElapsed / from.OutTime);
            float lerpT = from.OutCurve.Evaluate(scaledTime);

            charCon.height = Mathf.Lerp(from.CapsuleHeight, to.CapsuleHeight, lerpT);
            charCon.center = Vector3.Lerp(from.CapsuleCenter, to.CapsuleCenter, lerpT);

            return scaledTime >= 1f;
        }

        private void FinishHeightChange(ref CharacterHeightLerpComponent crouchComponent)
        {
            crouchComponent.IsChangeColliderInProgress = false;
            crouchComponent.IsTargetState = !crouchComponent.IsTargetState;
            crouchComponent.LerpStartTime = 0f;
        }

        public void Dispose()
        {
        }
    }
}