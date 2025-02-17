using EscapeRooms.Components;
using Scellecs.Morpeh;
using Unity.IL2CPP.CompilerServices;
using UnityEngine.InputSystem;

namespace EscapeRooms.Initializers
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    public sealed class InputInitializer : IInitializer
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
            InputAction moveAction = InputSystem.actions.FindAction("Move");
            InputAction lookAction = InputSystem.actions.FindAction("Look");
            InputAction jumpAction = InputSystem.actions.FindAction("Jump");
            InputAction crouchAction = InputSystem.actions.FindAction("Crouch");

            foreach (var entity in _filter)
            {
                ref var playerInputComponent = ref _playerInputStash.Get(entity);

                playerInputComponent.MoveAction = moveAction;
                playerInputComponent.LookAction = lookAction;
                playerInputComponent.JumpAction = jumpAction;
                playerInputComponent.CrouchAction = crouchAction;
            }
        }

        public void Dispose()
        {
        }
    }
}