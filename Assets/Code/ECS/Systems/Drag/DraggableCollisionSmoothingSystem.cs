using System.Linq;
using EscapeRooms.Components;
using EscapeRooms.Data;
using Scellecs.Morpeh;
using Scellecs.Morpeh.Collections;
using Scellecs.Morpeh.Providers;
using UnityEngine;
using Unity.IL2CPP.CompilerServices;

namespace EscapeRooms.Systems
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    public sealed class DraggableCollisionSmoothingSystem : ISystem
    {
        public World World { get; set; }

        private Filter _filter;
        private Stash<DraggableComponent> _draggableStash;

        public void OnAwake()
        {
            _filter = World.Filter
                .With<DraggableComponent>()
                .Build();

            _draggableStash = World.GetStash<DraggableComponent>();
        }

        public void OnUpdate(float deltaTime)
        {
            foreach (var entity in _filter)
            {
                ref var draggableComponent = ref _draggableStash.Get(entity);
                
            }
        }
        
        public void Dispose()
        {
        }
    }
}