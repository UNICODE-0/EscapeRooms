using EscapeRooms.Components;
using Scellecs.Morpeh;
using UnityEngine;
using Unity.IL2CPP.CompilerServices;

namespace EscapeRooms.Systems
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    public sealed class CharacterCrouchSlowdownSystem : ISystem
    {
        public World World { get; set; }

        private Filter _filter;
        private Stash<CharacterCrouchComponent> _crouchStash;
        private Stash<CharacterMovementComponent> _movementStash;
        private Stash<CharacterCrouchSlowdownComponent> _crouchSlowdownStash;
        
        private Stash<FloatLerpComponent> _floatLerpStash;

        public void OnAwake()
        {
            _filter = World.Filter
                .With<CharacterCrouchComponent>()
                .With<CharacterMovementComponent>()
                .With<CharacterCrouchSlowdownComponent>()
                .Build();

            _crouchStash = World.GetStash<CharacterCrouchComponent>();
            _crouchSlowdownStash = World.GetStash<CharacterCrouchSlowdownComponent>();
            _movementStash = World.GetStash<CharacterMovementComponent>();
            _floatLerpStash = World.GetStash<FloatLerpComponent>();
        }

        public void OnUpdate(float deltaTime)
        {
            foreach (var entity in _filter)
            {
                ref var crouchComponent = ref _crouchStash.Get(entity);
                ref var crouchSlowdownComponent = ref _crouchSlowdownStash.Get(entity);
                ref var movementComponent = ref _movementStash.Get(entity);
                ref var crouchFloatLerpComponent = ref _floatLerpStash.Get(crouchComponent.HeightLerpProvider.Entity);

                bool isSquattingInProgress = !crouchComponent.IsCrouching && crouchComponent.IsSquatInProgress;
                bool isCrouchingAndNotStandingUp = crouchComponent.IsCrouching && !crouchComponent.IsSquatInProgress;
                
                if (isSquattingInProgress || isCrouchingAndNotStandingUp)
                {
                    crouchSlowdownComponent.IsSlowdownActive = true;
                    crouchSlowdownComponent.CurrentModifier = Mathf.Lerp(1f, crouchSlowdownComponent.SlowdownModifier,
                        crouchFloatLerpComponent.CurrentValue);
                    
                    movementComponent.CurrentVelocity *= crouchSlowdownComponent.CurrentModifier;
                }
                else if(crouchSlowdownComponent.IsSlowdownActive)
                {
                    crouchSlowdownComponent.CurrentModifier = Mathf.Lerp(crouchSlowdownComponent.SlowdownModifier, 1f,
                        crouchFloatLerpComponent.CurrentValue);
                    
                    movementComponent.CurrentVelocity *= crouchSlowdownComponent.CurrentModifier;
                    
                    if(crouchFloatLerpComponent.IsLerpTimeIsUp) crouchSlowdownComponent.IsSlowdownActive = false;
                }
            }
        }

        public void Dispose()
        {
        }
    }
}