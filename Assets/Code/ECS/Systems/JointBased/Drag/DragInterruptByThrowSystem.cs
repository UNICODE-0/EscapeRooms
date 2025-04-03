using EscapeRooms.Components;
using Scellecs.Morpeh;
using Unity.IL2CPP.CompilerServices;
using UnityEngine;

namespace EscapeRooms.Systems
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    public sealed class DragInterruptByThrowSystem : ISystem
    {
        public World World { get; set; }

        private Filter _filter;
        private Stash<DragComponent> _dragStash;
        private Stash<ThrowComponent> _throwStash;

        public void OnAwake()
        {
            _filter = World.Filter
                .With<DragComponent>()
                .With<ThrowComponent>()
                .Build();

            _dragStash = World.GetStash<DragComponent>();
            _throwStash = World.GetStash<ThrowComponent>();
        }

        public void OnUpdate(float deltaTime)
        {
            foreach (var entity in _filter)
            {
                ref var dragComponent = ref _dragStash.Get(entity);
                ref var throwComponent = ref _throwStash.Get(entity);

                if(dragComponent.IsDragging && throwComponent.ThrowInput)
                {
                    dragComponent.DragStopInput = true;
                }
            }
        }
        
        public void Dispose()
        {
        }
    }
}