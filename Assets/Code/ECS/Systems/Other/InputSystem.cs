using EscapeRooms.Components;
using EscapeRooms.Data;
using EscapeRooms.Requests;
using Scellecs.Morpeh;
using Unity.IL2CPP.CompilerServices;
using UnityEngine;
using UnityEngine.InputSystem;

namespace EscapeRooms.Systems
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    public sealed class InputSystem : ISystem
    {
        public World World { get; set; }

        private Filter _filter;
        private Stash<InputComponent> _playerInputStash;
        
        private InputAction _moveAction;
        private InputAction _lookAction;
        private InputAction _jumpAction;
        private InputAction _crouchAction;
        private InputAction _interactAction;
        private InputAction _throwAction;
        private InputAction _dragRotateAction;
        private InputAction _dragRadiusChangeAction;

        private DelayedInputTrigger _jumpDelayedTrigger;
        private DelayedInputTrigger _crouchDelayedTrigger;

        private Request<InputTriggerInterruptRequest> _triggerInterruptRequests;
        
        public void OnAwake()
        {
            _filter = World.Filter
                .With<InputComponent>()
                .Build();

            _playerInputStash = World.GetStash<InputComponent>();
            _triggerInterruptRequests = World.GetRequest<InputTriggerInterruptRequest>();

            InitializeInputActions();
        }
        
        public void InitializeInputActions()
        {
            _moveAction = UnityEngine.InputSystem.InputSystem.actions.FindAction("Move");
            _lookAction = UnityEngine.InputSystem.InputSystem.actions.FindAction("Look");
            _jumpAction = UnityEngine.InputSystem.InputSystem.actions.FindAction("Jump");
            _crouchAction = UnityEngine.InputSystem.InputSystem.actions.FindAction("Crouch");
            _interactAction = UnityEngine.InputSystem.InputSystem.actions.FindAction("Interact");
            _throwAction = UnityEngine.InputSystem.InputSystem.actions.FindAction("Throw");
            _dragRotateAction = UnityEngine.InputSystem.InputSystem.actions.FindAction("DragRotate");
            _dragRadiusChangeAction = UnityEngine.InputSystem.InputSystem.actions.FindAction("DragRadiusChange");

            _jumpDelayedTrigger = new DelayedInputTrigger();
            _jumpDelayedTrigger.Initialize(GameSettings.Instance.JumpInputTriggerDelay);
            _crouchDelayedTrigger = new DelayedInputTrigger();
            _crouchDelayedTrigger.Initialize(GameSettings.Instance.CrouchInputTriggerDelay);
        }
        
        public void OnUpdate(float deltaTime)
        {
            Vector2 moveActionValue = _moveAction.ReadValue<Vector2>();
            Vector2 lookActionValue = _lookAction.ReadValue<Vector2>();
            Vector2 dragRadiusChangeValue = _dragRadiusChangeAction.ReadValue<Vector2>();

            HandleInterruptTriggerEvent();
            
            _jumpDelayedTrigger.Update(deltaTime, _jumpAction.triggered);
            _crouchDelayedTrigger.Update(deltaTime, _crouchAction.triggered);

            foreach (var entity in _filter)
            {
                ref var playerInputComponent = ref _playerInputStash.Get(entity);

                playerInputComponent.MoveValue = moveActionValue;
                playerInputComponent.LookValue = lookActionValue;
                playerInputComponent.JumpTrigger = _jumpDelayedTrigger.IsTriggered;
                playerInputComponent.CrouchTrigger = _crouchDelayedTrigger.IsTriggered;
                playerInputComponent.InteractStartTrigger = _interactAction.triggered;
                playerInputComponent.InteractStopInProgress = !_interactAction.inProgress;
                playerInputComponent.ThrowTrigger = _throwAction.triggered;
                playerInputComponent.DragRotationInProgress = _dragRotateAction.inProgress;
                playerInputComponent.DragRadiusChangeValue = dragRadiusChangeValue;
            }
        }

        private void HandleInterruptTriggerEvent()
        {
            foreach (var interruptReq in _triggerInterruptRequests.Consume())
            {
                switch (interruptReq.TriggerToInterrupt)
                {
                    case InterruptibleInputTrigger.Jump:
                        _jumpDelayedTrigger.Interrupt();
                        break;
                    case InterruptibleInputTrigger.Crouch:
                        _crouchDelayedTrigger.Interrupt();
                        break;
                }
            }
        }
        
        public void Dispose()
        {
        }
    }
}