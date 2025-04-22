using EscapeRooms.Components;
using EscapeRooms.Events;
using EscapeRooms.Helpers;
using Scellecs.Morpeh;
using Unity.IL2CPP.CompilerServices;
using UnityEngine;

namespace EscapeRooms.Systems
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    public sealed class DragOrbitalPositionSetSystem : ISystem
    {
        public World World { get; set; }

        private Stash<TransformComponent> _transformStash;
        private Stash<TransformOrbitalFollowComponent> _transformOrbitalFollowStash;
        private Stash<OneHitRaycastComponent> _raycastStash;
        private Stash<DragComponent> _dragStash;
        private Stash<DraggableComponent> _draggableStash;
        private Stash<ConfigurableJointComponent> _configurableJointStash;

        private Event<DragStartEvent> _dragStartEvent;

        public void OnAwake()
        {
            _transformStash = World.GetStash<TransformComponent>();
            _transformOrbitalFollowStash = World.GetStash<TransformOrbitalFollowComponent>();
            _raycastStash = World.GetStash<OneHitRaycastComponent>();
            _dragStash = World.GetStash<DragComponent>();
            _draggableStash = World.GetStash<DraggableComponent>();
            _configurableJointStash = World.GetStash<ConfigurableJointComponent>();
                
            _dragStartEvent = World.GetEvent<DragStartEvent>();
        }

        public void OnUpdate(float deltaTime)
        {
            foreach (var evt in _dragStartEvent.publishedChanges)
            {
                ref var transformOrbitalFollowComponent = ref _transformOrbitalFollowStash.Get(evt.Owner, out bool exist);
                if(!exist) return;
                
                ref var draggableComponent = ref _draggableStash.Get(evt.Draggable);
                ref var draggableTransformComponent = ref _transformStash.Get(evt.Draggable);
                ref var dragComponent = ref _dragStash.Get(evt.Owner); 
                ref var dragStartRaycastComponent = ref _raycastStash.Get(dragComponent.DetectionRaycast.Entity);
                ref var jointComponent = ref _configurableJointStash.Get(evt.Draggable); 
                
                Transform dragRaycastStartTf = dragStartRaycastComponent.RayStartPoint;
                Transform originalHandTf = transformOrbitalFollowComponent.Target;
                
                // Can't be null, because dragStartEvent guarantees the presence of hit
                Vector3 hitPosition = dragStartRaycastComponent.Hit.point; 
                
                float distToHitPosition = GetDistanceToDraggable(hitPosition, draggableComponent.Colliders,
                    dragRaycastStartTf.position, dragComponent.MinDragDistance);

                Vector3 hitLocalPosition = draggableTransformComponent.Transform.InverseTransformPoint(hitPosition);
                jointComponent.ConfigurableJoint.anchor = hitLocalPosition;
                
                transformOrbitalFollowComponent.SphereRadius = distToHitPosition;
                originalHandTf.position = hitPosition;
                
                // Set following hand transform direct to originalHandTf
                transformOrbitalFollowComponent.OneFramePermanentCalculation = true;
            }
        }

        private float GetDistanceToDraggable(Vector3 hitPoint, Collider[] colliders, Vector3 targetPosition, float minDistance)
        {
            float currentMinDistance = ColliderExtension.GetMinDistanceToClosestPoints(colliders, targetPosition);
            float distanceToDragPoint = Vector3.Distance(hitPoint, targetPosition);

            if (currentMinDistance < minDistance)
                return distanceToDragPoint + (minDistance - currentMinDistance);
            
            return distanceToDragPoint;
        }
        
        public void Dispose()
        {
        }
    }
}