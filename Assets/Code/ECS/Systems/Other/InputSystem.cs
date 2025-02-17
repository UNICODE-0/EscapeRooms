using EscapeRooms.Components;
using EscapeRooms.Data;
using Scellecs.Morpeh;
using Unity.IL2CPP.CompilerServices;
using UnityEngine;
using UnityEngine.InputSystem;
using Vector3 = System.Numerics.Vector3;

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
        
        private InputAction _moveAction;
        private InputAction _lookAction;
        private InputAction _jumpAction;
        private InputAction _crouchAction;

        private DelayedInputTrigger _jumpDelayedTrigger;
        private DelayedInputTrigger _crouchDelayedTrigger;

        public void OnAwake()
        {
            _filter = World.Filter
                .With<InputComponent>()
                .With<SettingsComponent>()
                .Build();

            _playerInputStash = World.GetStash<InputComponent>();
            _settingsStash = World.GetStash<SettingsComponent>();

            InitializeInputActions();
        }
        
        public void InitializeInputActions()
        {
            _moveAction = UnityEngine.InputSystem.InputSystem.actions.FindAction("Move");
            _lookAction = UnityEngine.InputSystem.InputSystem.actions.FindAction("Look");
            _jumpAction = UnityEngine.InputSystem.InputSystem.actions.FindAction("Jump");
            _crouchAction = UnityEngine.InputSystem.InputSystem.actions.FindAction("Crouch");

            _jumpDelayedTrigger = new DelayedInputTrigger();
            
            foreach (var entity in _filter)
            {
                ref var playerInputComponent = ref _playerInputStash.Get(entity);
                ref var settingsComponent = ref _settingsStash.Get(entity);

                playerInputComponent.MoveAction = _moveAction;
                playerInputComponent.LookAction = _lookAction;
                
                playerInputComponent.JumpAction = _jumpAction;
                playerInputComponent.JumpTriggerValue.Initialize(settingsComponent.GameSettings.JumpInputTriggerDelay);
                
                playerInputComponent.CrouchAction = _crouchAction;
                playerInputComponent.CrouchTriggerValue.Initialize(settingsComponent.GameSettings.CrouchInputTriggerDelay);
            }
        }
        
        public void OnUpdate(float deltaTime)
        {
            // Vector2 moveActionValue = _moveAction.ReadValue<Vector2>();
            // Vector2 lookActionValue = _lookAction.ReadValue<Vector2>();
            // DelayedInputTrigger lookActionValue = _lookAction.ReadValue<Vector2>();

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