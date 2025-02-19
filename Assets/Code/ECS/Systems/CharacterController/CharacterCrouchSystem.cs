using EscapeRooms.Components;
using Scellecs.Morpeh;
using UnityEngine;
using Unity.IL2CPP.CompilerServices;

namespace EscapeRooms.Systems
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    public sealed class CharacterCrouchSystem : ISystem
    {
        public World World { get; set; }

        private Filter _filter;
        private Stash<CharacterCrouchComponent> _crouchStash;
        private Stash<CharacterControllerComponent> _characterStash;
        private Stash<FloatLerpComponent> _floatLerpStash;

        public void OnAwake()
        {
            _filter = World.Filter
                .With<CharacterCrouchComponent>()
                .With<CharacterMovementComponent>()
                .Build();

            _crouchStash = World.GetStash<CharacterCrouchComponent>();
            _characterStash = World.GetStash<CharacterControllerComponent>();
            _floatLerpStash = World.GetStash<FloatLerpComponent>();
        }

        public void OnUpdate(float deltaTime)
        {
            foreach (var entity in _filter)
            {
                ref var crouchComponent = ref _crouchStash.Get(entity);
                ref var characterComponent = ref _characterStash.Get(entity);
                ref var floatLerpComponent = ref _floatLerpStash.Get(crouchComponent.FloatLerpProvider.Entity);

                if (crouchComponent.CrouchInput && !floatLerpComponent.IsLerpInProgress)
                {
                    floatLerpComponent.StartLerpInput = true;
                }

                if (floatLerpComponent.IsLerpInProgress)
                {
                    floatLerpComponent.StartLerpInput = false;

                    ref CharacterCrouchState from = ref crouchComponent.IsCrouching ? 
                        ref crouchComponent.CrouchState : ref crouchComponent.StandState;
                    
                    ref CharacterCrouchState to = ref crouchComponent.IsCrouching ?
                        ref crouchComponent.StandState : ref crouchComponent.CrouchState;
                    
                    characterComponent.CharacterController.height = 
                        Mathf.Lerp(from.CapsuleHeight, to.CapsuleHeight, floatLerpComponent.CurrentValue);
                    
                    characterComponent.CharacterController.center = 
                        Vector3.Lerp(from.CapsuleCenter, to.CapsuleCenter, floatLerpComponent.CurrentValue);

                    if (floatLerpComponent.IsLerpTimeIsUp)
                        crouchComponent.IsCrouching = !crouchComponent.IsCrouching;
                }
                
            }
        }

        public void Dispose()
        {
        }
    }
}