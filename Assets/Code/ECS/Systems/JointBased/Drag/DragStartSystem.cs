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
        private Stash<OneHitRaycastComponent> _raycastStash;
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
            _raycastStash = World.GetStash<OneHitRaycastComponent>();
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

                if (dragComponent.DragStartInput && !dragComponent.IsDragging)
                {
                    ref var raycastComponent = ref _raycastStash.Get(dragComponent.DetectionRaycast.Entity);
                    
                    if (!RaycastExtension.GetHitEntity(ref raycastComponent, out Entity hitEntity))
                        continue;
                    
                    ref var draggableComponent = ref _draggableStash.Get(hitEntity, out bool draggableExist);
                    if (!draggableExist)
                        continue;
                    
                    ref var handRigidbodyComponent = ref _rigidbodyStash.Get(entity);
                    ref var jointComponent = ref _configurableJointStash.Get(hitEntity);
                    ref var itemRigidbodyComponent = ref _rigidbodyStash.Get(hitEntity);

                    jointComponent.ConfigurableJoint.connectedBody = handRigidbodyComponent.Rigidbody;
                    
                    jointComponent.ConfigurableJoint.SetJointDriveData(
                        draggableComponent.DragDriveSpring, 
                        draggableComponent.DragDriveDamper, 
                        draggableComponent.DragAngularDriveSpring, 
                        draggableComponent.DragAngularDriveDamper);

                    draggableComponent.MassBeforeDrag = itemRigidbodyComponent.Rigidbody.mass;
                    itemRigidbodyComponent.Rigidbody.mass = draggableComponent.MassWhileDrag;
                    itemRigidbodyComponent.Rigidbody.linearDamping = draggableComponent.BodyLinearDamping;
                    itemRigidbodyComponent.Rigidbody.angularDamping = draggableComponent.BodyAngularDamping;

                    foreach (var collider in draggableComponent.Colliders)
                    {
                        collider.sharedMaterial = draggableComponent.MaterialOnDrag;
                    }
                    
                    dragComponent.DraggableEntity = hitEntity;
                    dragComponent.IsDragging = true;

                    _onDragStash.Add(hitEntity, new OnDragFlag()
                    {
                        Owner = entity
                    });
                    
                    _dragStartEvent.ThisFrame(new DragStartEvent()
                    {
                        Draggable = hitEntity,
                        Owner = entity
                    });
                }
            }
        }

        public void Dispose()
        {
        }
    }
}