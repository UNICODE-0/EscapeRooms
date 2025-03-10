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
    public sealed class DragSystem : ISystem
    {
        public World World { get; set; }

        private Filter _filter;
        private Stash<DraggableComponent> _draggableStash;
        private Stash<DragComponent> _dragStash;
        private Stash<RaycastComponent> _raycastStash;
        private Stash<ConfigurableJointComponent> _configurableJointStash;
        private Stash<RigidbodyComponent> _rigidbodyStash;

        public void OnAwake()
        {
            _filter = World.Filter
                .With<DragComponent>()
                .With<RigidbodyComponent>()
                .Build();

            _draggableStash = World.GetStash<DraggableComponent>();
            _dragStash = World.GetStash<DragComponent>();
            _raycastStash = World.GetStash<RaycastComponent>();
            _configurableJointStash = World.GetStash<ConfigurableJointComponent>();
            _rigidbodyStash = World.GetStash<RigidbodyComponent>();
        }

        public void OnUpdate(float deltaTime)
        {
            foreach (var entity in _filter)
            {
                ref var dragComponent = ref _dragStash.Get(entity);

                if (dragComponent.DragInput)
                {
                    if (!dragComponent.IsDragging)
                    {
                        ref var raycastComponent = ref _raycastStash.Get(dragComponent.DragRaycast.Entity);
                
                        if(raycastComponent.HitsCount > 0)
                        {
                            if (EntityProvider.map.TryGetValue(raycastComponent.Hits[0].collider.gameObject.GetInstanceID(), out var item) 
                                && _draggableStash.Has(item.entity))
                            {
                                ref var jointComponent = ref _configurableJointStash.Get(item.entity);
                                ref var rigidbodyComponent = ref _rigidbodyStash.Get(entity);
                            
                                jointComponent.ConfigurableJoint.connectedBody = rigidbodyComponent.Rigidbody;

                                dragComponent.DraggableEntity = item.entity;
                                dragComponent.IsDragging = true;
                            }
                        }
                    }
                }
                else if(dragComponent.IsDragging)
                {
                    ref var jointComponent = ref _configurableJointStash.Get(dragComponent.DraggableEntity);

                    jointComponent.ConfigurableJoint.connectedBody = null;
                        
                    dragComponent.DraggableEntity = default;
                    dragComponent.IsDragging = false;
                }
            }
        }

        public void Dispose()
        {
        }
    }
}