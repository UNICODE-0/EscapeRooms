using EscapeRooms.Components;
using Scellecs.Morpeh;
using Unity.IL2CPP.CompilerServices;
using UnityEngine;

namespace EscapeRooms.Systems
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    public sealed class DragInterruptByDistanceSystem : ISystem
    {
        public World World { get; set; }

        private Filter _filter;
        private Stash<DragComponent> _dragStash;
        private Stash<TransformComponent> _transformStash;

        public void OnAwake()
        {
            _filter = World.Filter
                .With<DragComponent>()
                .Build();

            _dragStash = World.GetStash<DragComponent>();
            _transformStash = World.GetStash<TransformComponent>();
        }

        public void OnUpdate(float deltaTime)
        {
            foreach (var entity in _filter)
            {
                ref var dragComponent = ref _dragStash.Get(entity);

                if(dragComponent.IsDragging)
                {
                    ref var handTransformComponent = ref _transformStash.Get(entity);
                    ref var draggableTransformComponent = ref _transformStash.Get(dragComponent.DraggableEntity);

                    float distance = Vector3.Distance(handTransformComponent.Transform.position,
                        draggableTransformComponent.Transform.position);
                    
                    if (distance >= dragComponent.MaxDragDeviation)
                    {
                        dragComponent.DragStopInput = true;
                    }
                }
            }
        }
        
        public void Dispose()
        {
        }
    }
}