using EscapeRooms.Components;
using Scellecs.Morpeh;
using Unity.IL2CPP.CompilerServices;
using UnityEngine;

namespace EscapeRooms.Systems
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    public sealed class DragInterruptByCollisionSystem : ISystem
    {
        public World World { get; set; }

        private Filter _filter;
        private Stash<DragComponent> _dragStash;
        private Stash<ColliderTriggerEventsHolderComponent> _triggerStash;
        
        public void OnAwake()
        {
            _filter = World.Filter
                .With<DragComponent>()
                .With<ColliderTriggerEventsHolderComponent>()
                .Build();

            _dragStash = World.GetStash<DragComponent>();
            _triggerStash = World.GetStash<ColliderTriggerEventsHolderComponent>();
        }

        public void OnUpdate(float deltaTime)
        {
            foreach (var entity in _filter)
            {
                ref var dragComponent = ref _dragStash.Get(entity);

                if(dragComponent.IsDragging)
                {
                    ref var triggerComponent = ref _triggerStash.Get(entity);

                    if (triggerComponent.EventsHolder.IsAnyTriggerInProgress.GetValue())
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