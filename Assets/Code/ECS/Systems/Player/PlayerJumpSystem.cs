using System.Collections.Generic;
using EscapeRooms.Components;
using Scellecs.Morpeh;
using UnityEngine;
using Unity.IL2CPP.CompilerServices;
using UnityEngine.InputSystem;

namespace EscapeRooms.Systems
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    public sealed class PlayerJumpSystem : ISystem
    {
        public World World { get; set; }

        private Filter _filter;
        private Stash<JumpComponent> _jumpStash;
        private Stash<InputComponent> _inputStash;
        private Stash<CharacterGravityComponent> _gravityStash;
        private Stash<CharacterGroundedComponent> _groundedStash;

        public void OnAwake()
        {
            _filter = World.Filter
                .With<JumpComponent>()
                .With<CharacterGravityComponent>()
                .With<InputComponent>()
                .With<CharacterGroundedComponent>()
                .With<PlayerComponent>()
                .Build();

            _jumpStash = World.GetStash<JumpComponent>();
            _inputStash = World.GetStash<InputComponent>();
            _gravityStash = World.GetStash<CharacterGravityComponent>();
            _groundedStash = World.GetStash<CharacterGroundedComponent>();
        }

        public void OnUpdate(float deltaTime)
        {
            foreach (var entity in _filter)
            {
                ref var jumpComponent = ref _jumpStash.Get(entity);
                ref var inputComponent = ref _inputStash.Get(entity);
                ref var gravityComponent = ref _gravityStash.Get(entity);
                ref var groundedComponent = ref _groundedStash.Get(entity);

                float jumpingInputState = inputComponent.JumpAction.ReadValue<float>();

                if (!jumpComponent.IsJumpAllowed && jumpingInputState <= 0f)
                    jumpComponent.IsJumpAllowed = true;

                if (groundedComponent.IsGrounded)
                {
                    if (jumpingInputState > 0f && jumpComponent.IsJumpAllowed)
                    {
                        gravityComponent.IgnoreAttraction = true;

                        float frameTimeDifference = jumpComponent.ReferenceFrameTime - deltaTime;
                        float ScaledDif = frameTimeDifference * jumpComponent.FrameTimeCorrection;
                        float frameRateCorrection = 1 - ScaledDif;

                        jumpComponent.CurrentForce.y =
                            Mathf.Sqrt((jumpComponent.JumpStrength * frameRateCorrection)
                                       * CharacterGravityComponent.GRAVITY_ACCELERATION_FACTOR * gravityComponent.GravitationalAttraction);

                        jumpComponent.IsJumpForceApplied = true;
                        jumpComponent.IsJumpAllowed = false;
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