using EscapeRooms.Components;
using EscapeRooms.Data;
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
                .With<CharacterControllerComponent>()
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
                ref var heightFloatLerpComponent = ref _floatLerpStash.Get(crouchComponent.HeightLerpProvider.Entity);
                ref var headLerpComponent = ref _transformPositionLerpStash.Get(crouchComponent.HeadLerpProvider.Entity);

                if (crouchComponent.CrouchBlockFlag.IsFlagClear())
                {
                    heightFloatLerpComponent.StartLerpInput = crouchComponent.CrouchInput;
                    headLerpComponent.ChangePositionInput = crouchComponent.CrouchInput;
                }

                if (heightFloatLerpComponent.IsLerpInProgress)
                {
                    crouchComponent.IsSquatInProgress = true;
                    
                    ref CharacterCrouchState from = ref crouchComponent.IsCrouching ? 
                        ref crouchComponent.CrouchState : ref crouchComponent.StandState;
                    
                    ref CharacterCrouchState to = ref crouchComponent.IsCrouching ?
                        ref crouchComponent.StandState : ref crouchComponent.CrouchState;
                    
                    characterComponent.CharacterController.height = 
                        Mathf.Lerp(from.CapsuleHeight, to.CapsuleHeight, heightFloatLerpComponent.CurrentValue);
                    
                    characterComponent.CharacterController.center = 
                        Vector3.Lerp(from.CapsuleCenter, to.CapsuleCenter, heightFloatLerpComponent.CurrentValue);

                    if (heightFloatLerpComponent.IsLerpTimeIsUp)
                    {
                        crouchComponent.IsSquatInProgress = false;
                        crouchComponent.IsCrouching = !crouchComponent.IsCrouching;
                    }
                }
            }
        }

        public void Dispose()
        {
        }
    }
}