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
    public sealed class PlayerBodyRotationInputSystem : ISystem
    {
        public World World { get; set; }

        private Filter _filter;
        private Stash<TransformDeltaRotationComponent> _rotationStash;
        private Stash<InputComponent> _inputStash;

        public void OnAwake()
        {
            _filter = World.Filter
                .With<TransformDeltaRotationComponent>()
                .With<InputComponent>()
                .With<PlayerTag>()
                .Build();

            _rotationStash = World.GetStash<TransformDeltaRotationComponent>();
            _inputStash = World.GetStash<InputComponent>();
        }

        public void OnUpdate(float deltaTime)
        {
            foreach (var entity in _filter)
            {
                ref var rotationComponent = ref _rotationStash.Get(entity);
                ref var inputComponent = ref _inputStash.Get(entity);
                
                rotationComponent.EulerRotationDelta = 
                    new Vector3(0f, inputComponent.LookValue.x * GameSettings.Instance.Sensitivity, 0f);
            }
        }

        public void Dispose()
        {
        }
    }
}