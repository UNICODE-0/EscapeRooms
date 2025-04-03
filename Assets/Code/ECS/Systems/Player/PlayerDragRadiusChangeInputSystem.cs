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
    public sealed class PlayerDragRadiusChangeInputSystem : ISystem
    {
        public World World { get; set; }

        private Filter _filter;
        private Stash<DragRadiusChangeComponent> _radiusChangeStash;
        private Stash<InputComponent> _inputStash;

        public void OnAwake()
        {
            _filter = World.Filter
                .With<DragRadiusChangeComponent>()
                .With<InputComponent>()
                .With<PlayerHandTag>()
                .Build();

            _radiusChangeStash = World.GetStash<DragRadiusChangeComponent>();
            _inputStash = World.GetStash<InputComponent>();
        }

        public void OnUpdate(float deltaTime)
        {
            foreach (var entity in _filter)
            {
                ref var radiusChangeComponent = ref _radiusChangeStash.Get(entity);
                ref var inputComponent = ref _inputStash.Get(entity);
                
                radiusChangeComponent.RadiusChangeDeltaInput = inputComponent.DragRadiusChangeValue;
            }
        }

        public void Dispose()
        {
        }
    }
}