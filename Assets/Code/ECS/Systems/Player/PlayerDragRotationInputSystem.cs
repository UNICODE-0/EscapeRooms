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
    public sealed class PlayerDragRotationInputSystem : ISystem
    {
        public World World { get; set; }

        private Filter _filter;
        private Stash<DragRotationComponent> _rotationStash;
        private Stash<InputComponent> _inputStash;

        public void OnAwake()
        {
            _filter = World.Filter
                .With<DragRotationComponent>()
                .With<InputComponent>()
                .With<PlayerHandTag>()
                .Build();

            _rotationStash = World.GetStash<DragRotationComponent>();
            _inputStash = World.GetStash<InputComponent>();
        }

        public void OnUpdate(float deltaTime)
        {
            foreach (var entity in _filter)
            {
                ref var rotationComponent = ref _rotationStash.Get(entity);
                ref var inputComponent = ref _inputStash.Get(entity);
                
                if (inputComponent.DragRotationInProgress)
                {
                    rotationComponent.RotationActiveInput = true;
                    rotationComponent.RotationDeltaInput =
                        inputComponent.LookValue * GameSettings.Instance.Sensitivity;
                }
                else
                {
                    rotationComponent.RotationActiveInput = false;
                    rotationComponent.RotationDeltaInput = Vector2.zero;
                }
            }
        }

        public void Dispose()
        {
        }
    }
}