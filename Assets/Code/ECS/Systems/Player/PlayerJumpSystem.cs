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
        private Stash<GravityComponent> _gravityStash;
        private Stash<GroundedComponent> _groundedStash;
        
        List<int> tt = new List<int>(){15,30,60,100,144};
        private int i = 0;

        public void OnAwake()
        {
            _filter = World.Filter
                .With<JumpComponent>()
                .With<GravityComponent>()
                .With<InputComponent>()
                .With<PlayerComponent>()
                .Build();

            _jumpStash = World.GetStash<JumpComponent>();
            _inputStash = World.GetStash<InputComponent>();
            _gravityStash = World.GetStash<GravityComponent>();
            _groundedStash = World.GetStash<GroundedComponent>();
        }

        public void OnUpdate(float deltaTime)
        {
            if (Input.GetKeyDown(KeyCode.RightControl))
            {
                Application.targetFrameRate = tt[i];
                i++;
                if (i >= tt.Count) i = 0;
            }
            
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

                        float frameTimeDif = 0.016667f - deltaTime;
                        float ScaledDif = -1 * frameTimeDif * jumpComponent.FrameTimeCorrection;
                        float frameRateCorrection = 1 + ScaledDif;

                        jumpComponent.CurrentForce.y =
                            Mathf.Sqrt((jumpComponent.JumpStrength * frameRateCorrection) * -2f * gravityComponent.GravitationalAttraction);

                        jumpComponent.IsJumpAllowed = false;
                    }
                }

                if (jumpComponent.CurrentForce.y > 0f)
                {
                    jumpComponent.CurrentForce.y += gravityComponent.GravitationalAttraction * deltaTime;
                }
                else
                {
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