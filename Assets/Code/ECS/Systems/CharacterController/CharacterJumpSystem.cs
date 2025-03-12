using EscapeRooms.Components;
using EscapeRooms.Helpers;
using Scellecs.Morpeh;
using UnityEngine;
using Unity.IL2CPP.CompilerServices;

namespace EscapeRooms.Systems
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    public sealed class CharacterJumpSystem : ISystem
    {
        public World World { get; set; }

        private Filter _filter;
        private Stash<CharacterJumpComponent> _jumpStash;
        private Stash<CharacterGravityComponent> _gravityStash;
        private Stash<CharacterGroundedComponent> _groundedStash;

        public void OnAwake()
        {
            _filter = World.Filter
                .With<CharacterJumpComponent>()
                .With<CharacterGravityComponent>()
                .With<CharacterGroundedComponent>()
                .Build();

            _jumpStash = World.GetStash<CharacterJumpComponent>();
            _gravityStash = World.GetStash<CharacterGravityComponent>();
            _groundedStash = World.GetStash<CharacterGroundedComponent>();
        }

        public void OnUpdate(float deltaTime)
        {
            foreach (var entity in _filter)
            {
                ref var jumpComponent = ref _jumpStash.Get(entity);
                ref var gravityComponent = ref _gravityStash.Get(entity);
                ref var groundedComponent = ref _groundedStash.Get(entity);
                
                if (groundedComponent.IsGrounded)
                {
                    if (jumpComponent.JumpInput && jumpComponent.JumpBlockFlag.IsFlagClear())
                    {
                        gravityComponent.IgnoreAttraction = true;

                        float frameTimeDifference = jumpComponent.ReferenceFrameTime - deltaTime;
                        float ScaledDif = frameTimeDifference * jumpComponent.FrameTimeCorrection;
                        float frameRateCorrection = 1 - ScaledDif;

                        jumpComponent.CurrentForce.y =
                            Mathf.Sqrt((jumpComponent.JumpStrength * frameRateCorrection)
                                       * CharacterGravityComponent.GRAVITY_ACCELERATION_FACTOR * gravityComponent.GravitationalAttraction);

                        jumpComponent.IsJumpForceApplied = true;
                    }
                }

                if (jumpComponent.CurrentForce.y > 0f)
                {
                    jumpComponent.CurrentForce.y += gravityComponent.GravitationalAttraction * deltaTime;
                }
                else
                {
                    jumpComponent.IsJumpForceApplied = false;
                    gravityComponent.IgnoreAttraction = false;
                    jumpComponent.CurrentForce.y = 0f;
                }
            }
        }

        public void Dispose()
        {
        }
    }
}