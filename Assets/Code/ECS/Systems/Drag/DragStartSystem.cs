using EscapeRooms.Components;
using EscapeRooms.Events;
using EscapeRooms.Helpers;
using Scellecs.Morpeh;
using Scellecs.Morpeh.Collections;
using Scellecs.Morpeh.Providers;
using Unity.IL2CPP.CompilerServices;

namespace EscapeRooms.Systems
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    public sealed class DragStartSystem : ISystem
    {
        public World World { get; set; }

        private Filter _filter;
        private Stash<DraggableComponent> _draggableStash;
        private Stash<DragComponent> _dragStash;
        private Stash<RaycastComponent> _raycastStash;
        private Stash<ConfigurableJointComponent> _configurableJointStash;
        private Stash<RigidbodyComponent> _rigidbodyStash;
        private Stash<OnDragFlag> _onDragStash;
        
        private Event<DragStartEvent> _dragStartEvent;

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
            _onDragStash = World.GetStash<OnDragFlag>();
            
            _dragStartEvent = World.GetEvent<DragStartEvent>();
        }

        public void OnUpdate(float deltaTime)
        {
            foreach (var entity in _filter)
            {
                ref var dragComponent = ref _dragStash.Get(entity);

                if (dragComponent.DragInput && !dragComponent.IsDragging)
                {
                    ref var raycastComponent = ref _raycastStash.Get(dragComponent.DragRaycast.Entity);
                    
                    if (raycastComponent.HitsCount > 0 && 
                        EntityProvider.map.TryGetValue(raycastComponent.Hits[0].collider.gameObject.GetInstanceID(), out var item))
                    {
                        ref var draggableComponent = ref _draggableStash.Get(item.entity, out bool draggableExist);
                        if (draggableExist)
                        {
                            ref var handRigidbodyComponent = ref _rigidbodyStash.Get(entity);
                            ref var jointComponent = ref _configurableJointStash.Get(item.entity);
                            ref var itemRigidbodyComponent = ref _rigidbodyStash.Get(item.entity);

                            jointComponent.ConfigurableJoint.connectedBody = handRigidbodyComponent.Rigidbody;
                            
                            ConfigurableJointHelper.SetJointDriveData(jointComponent.ConfigurableJoint,
                                draggableComponent.DragDriveSpring, 
                                draggableComponent.DragDriveDamper, 
                                draggableComponent.DragAngularDriveSpring, 
                                draggableComponent.DragAngularDriveDamper);
                            
                            itemRigidbodyComponent.Rigidbody.linearDamping = draggableComponent.BodyLinearDamping;
                            itemRigidbodyComponent.Rigidbody.angularDamping = draggableComponent.BodyAngularDamping;
                            
                            dragComponent.DraggableEntity = item.entity;
                            dragComponent.IsDragging = true;

                            _onDragStash.Add(item.entity);
                            
                            _dragStartEvent.ThisFrame(new DragStartEvent()
                            {
                                Draggable = item.entity,
                                Owner = entity
                            });
                        }
                    }
                }
            }
        }

        public void Dispose()
        {
        }
    }
}