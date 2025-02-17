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

        public void OnAwake()
        {
            _filter = World.Filter
                .With<InputComponent>()
                .Build();

            _playerInputStash = World.GetStash<InputComponent>();

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

                playerInputComponent.MoveAction = moveAction;
                playerInputComponent.LookAction = lookAction;
                playerInputComponent.JumpAction = jumpAction;
                playerInputComponent.CrouchAction = crouchAction;
            }
        }
        
        public void OnUpdate(float deltaTime)
        {
            foreach (var entity in _filter)
            {
                ref var playerInputComponent = ref _playerInputStash.Get(entity);

                playerInputComponent.MoveActionValue = playerInputComponent.MoveAction.ReadValue<Vector2>();
                playerInputComponent.LookActionValue = playerInputComponent.LookAction.ReadValue<Vector2>();
                playerInputComponent.JumpActionValue = playerInputComponent.JumpAction.ReadValue<float>();
                playerInputComponent.CrouchActionValue = playerInputComponent.CrouchAction.ReadValue<float>();
            }
        }

        public void Dispose()
        {
        }
    }
}