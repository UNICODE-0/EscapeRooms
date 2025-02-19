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
        private Stash<TransformPositionLerpComponent> _transformPositionLerpStash;

        public void OnAwake()
        {
            _filter = World.Filter
                .With<CharacterCrouchComponent>()
                .With<CharacterMovementComponent>()
                .Build();

            _crouchStash = World.GetStash<CharacterCrouchComponent>();
            _characterStash = World.GetStash<CharacterControllerComponent>();
            _floatLerpStash = World.GetStash<FloatLerpComponent>();
            _transformPositionLerpStash = World.GetStash<TransformPositionLerpComponent>();
        }

        public void OnUpdate(float deltaTime)
        {
            foreach (var entity in _filter)
            {
                ref var crouchComponent = ref _crouchStash.Get(entity);
                ref var characterComponent = ref _characterStash.Get(entity);
                ref var HeightFloatLerpComponent = ref _floatLerpStash.Get(crouchComponent.HeightLerpProvider.Entity);
                ref var HeadLerpComponent = ref _transformPositionLerpStash.Get(crouchComponent.HeadLerpProvider.Entity);

                HeightFloatLerpComponent.StartLerpInput = crouchComponent.CrouchInput;
                HeadLerpComponent.ChangePositionInput = crouchComponent.CrouchInput;

                if (HeightFloatLerpComponent.IsLerpInProgress)
                {
                    ref CharacterCrouchState from = ref crouchComponent.IsCrouching ? 
                        ref crouchComponent.CrouchState : ref crouchComponent.StandState;
                    
                    ref CharacterCrouchState to = ref crouchComponent.IsCrouching ?
                        ref crouchComponent.StandState : ref crouchComponent.CrouchState;
                    
                    characterComponent.CharacterController.height = 
                        Mathf.Lerp(from.CapsuleHeight, to.CapsuleHeight, HeightFloatLerpComponent.CurrentValue);
                    
                    characterComponent.CharacterController.center = 
                        Vector3.Lerp(from.CapsuleCenter, to.CapsuleCenter, HeightFloatLerpComponent.CurrentValue);

                    if (HeightFloatLerpComponent.IsLerpTimeIsUp)
                        crouchComponent.IsCrouching = !crouchComponent.IsCrouching;
                }
            }
        }

        public void Dispose()
        {
        }
    }
}