using EscapeRooms.Components;
using EscapeRooms.Events;
using EscapeRooms.Helpers;
using EscapeRooms.Mono;
using Scellecs.Morpeh;
using Unity.IL2CPP.CompilerServices;
using UnityEngine;

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
        private Stash<ColliderTriggerEventsHolderComponent> _colliderTriggerEventsHolderStash;
        private Stash<DraggableCollisionSmoothingComponent> _draggableSmoothingStash;
        private Stash<ConfigurableJointComponent> _configurableJointStash;
        private Stash<OnDragFlag> _dragFlagStash;

        private Event<DragStartEvent> _dragStartEvent;
        private Event<DragStopEvent> _dragStopEvent;

        public void OnAwake()
        {
            _filter = World.Filter
                .With<DraggableComponent>()
                .With<ColliderTriggerEventsHolderComponent>()
                .With<DraggableCollisionSmoothingComponent>()
                .With<ConfigurableJointComponent>()
                .With<OnDragFlag>()
                .Build();

            _draggableStash = World.GetStash<DraggableComponent>();
            _colliderTriggerEventsHolderStash = World.GetStash<ColliderTriggerEventsHolderComponent>();
            _draggableSmoothingStash = World.GetStash<DraggableCollisionSmoothingComponent>();
            _configurableJointStash = World.GetStash<ConfigurableJointComponent>();
            _dragFlagStash = World.GetStash<OnDragFlag>();

            _dragStartEvent = World.GetEvent<DragStartEvent>();
            _dragStopEvent = World.GetEvent<DragStopEvent>();
        }

        public void OnUpdate(float deltaTime)
        {
            // Disable triggers for optimization purposes
            foreach (var evt in _dragStartEvent.publishedChanges)
            {
                ref var draggableSmoothingComponent = ref _draggableSmoothingStash.Get(evt.Draggable);
                draggableSmoothingComponent.SmoothingTrigger.enabled = true;
            }
            
            foreach (var evt in _dragStopEvent.publishedChanges)
            {
                ref var draggableSmoothingComponent = ref _draggableSmoothingStash.Get(evt.Draggable);
                draggableSmoothingComponent.SmoothingTrigger.enabled = false;
            }
            
            foreach (var entity in _filter)
            {
                ref var onDragFlag = ref _dragFlagStash.Get(entity);
                ref var draggableSmoothingComponent = ref _draggableSmoothingStash.Get(entity);
                if (onDragFlag.IsLastFrameOfLife)
                {
                    draggableSmoothingComponent.IsSmoothed = false;
                    continue;
                }
                
                ref var colliderTriggerEventsHolderComponent = ref _colliderTriggerEventsHolderStash.Get(entity);
                
                var isAnyTriggerInProgress = colliderTriggerEventsHolderComponent.EventsHolder
                    .IsAnyTriggerInProgress;

                if (isAnyTriggerInProgress.GetValue() && !draggableSmoothingComponent.IsSmoothed)
                {
                    SetJointDriveData(entity, true);
                }
                else if (!isAnyTriggerInProgress.GetValue() && draggableSmoothingComponent.IsSmoothed)
                {
                    SetJointDriveData(entity, false);
                }
            }
        }
        
        private void SetJointDriveData(Entity entity, bool isSmoothed)
        {
            ref var jointComponent = ref _configurableJointStash.Get(entity);
            ref var draggableComponent = ref _draggableStash.Get(entity);
            ref var smoothingComponent = ref _draggableSmoothingStash.Get(entity);

            jointComponent.ConfigurableJoint.SetJointDriveData(
                isSmoothed ? smoothingComponent.SmoothDriveSpring : draggableComponent.DragDriveSpring,
                isSmoothed ? smoothingComponent.SmoothDriveDamper : draggableComponent.DragDriveDamper,
                isSmoothed ? smoothingComponent.SmoothAngularDriveSpring : draggableComponent.DragAngularDriveSpring,
                isSmoothed ? smoothingComponent.SmoothAngularDriveDamper : draggableComponent.DragAngularDriveDamper);

            smoothingComponent.IsSmoothed = isSmoothed;
        }
        
        public void Dispose()
        {
        }
    }
}