using EscapeRooms.Components;
using Scellecs.Morpeh;
using Unity.IL2CPP.CompilerServices;
using UnityEngine;
using UnityEngine.InputSystem;

namespace EscapeRooms.Initializers
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    public sealed class InputSystem : ISystem
    {
        public World World { get; set; }

        private Filter _filter;
        private Stash<InputComponent> _playerInputStash;
        private Stash<SettingsComponent> _settingsStash;

        public void OnAwake()
        {
            _filter = World.Filter
                .With<InputComponent>()
                .With<SettingsComponent>()
                .Build();

            _playerInputStash = World.GetStash<InputComponent>();
            _settingsStash = World.GetStash<SettingsComponent>();

            SetInputActions();
        }
        
        public void SetInputActions()
        {
            InputAction moveAction = UnityEngine.InputSystem.InputSystem.actions.FindAction("Move");
            InputAction lookAction = UnityEngine.InputSystem.InputSystem.actions.FindAction("Look");
            InputAction jumpAction = UnityEngine.InputSystem.InputSystem.actions.FindAction("Jump");
            InputAction crouchAction = UnityEngine.InputSystem.InputSystem.actions.FindAction("Crouch");

            foreach (var entity in _filter)
            {
                ref var playerInputComponent = ref _playerInputStash.Get(entity);
                ref var settingsComponent = ref _settingsStash.Get(entity);

                playerInputComponent.MoveAction = moveAction;
                playerInputComponent.LookAction = lookAction;
                
                playerInputComponent.JumpAction = jumpAction;
                playerInputComponent.JumpTriggerValue.Initialize(settingsComponent.GameSettings.JumpInputTriggerDelay);
                
                playerInputComponent.CrouchAction = crouchAction;
                playerInputComponent.CrouchTriggerValue.Initialize(settingsComponent.GameSettings.CrouchInputTriggerDelay);
            }
        }
        
        public void OnUpdate(float deltaTime)
        {
            foreach (var entity in _filter)
            {
                ref var playerInputComponent = ref _playerInputStash.Get(entity);

                playerInputComponent.MoveActionValue = playerInputComponent.MoveAction.ReadValue<Vector2>();
                playerInputComponent.LookActionValue = playerInputComponent.LookAction.ReadValue<Vector2>();
                playerInputComponent.JumpTriggerValue.Update(deltaTime, playerInputComponent.JumpAction.triggered);
                playerInputComponent.CrouchTriggerValue.Update(deltaTime, playerInputComponent.CrouchAction.triggered);
            }
        }

        public void Dispose()
        {
        }
    }
}