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
    public sealed class DragOrbitalPositionSetSystem : ISystem
    {
        public World World { get; set; }

        private Stash<TransformComponent> _transformStash;
        private Stash<TransformOrbitalFollowComponent> _transformOrbitalFollowStash;
        private Stash<RaycastComponent> _raycastStash;
        private Stash<DragComponent> _dragStash;
        private Stash<DraggableComponent> _draggableStash;

        private Event<DragStartEvent> _dragStartEvent;

        public void OnAwake()
        {
            _transformStash = World.GetStash<TransformComponent>();
            _transformOrbitalFollowStash = World.GetStash<TransformOrbitalFollowComponent>();
            _raycastStash = World.GetStash<RaycastComponent>();
            _dragStash = World.GetStash<DragComponent>();
            _draggableStash = World.GetStash<DraggableComponent>();
                
            _dragStartEvent = World.GetEvent<DragStartEvent>();
        }

        public void OnUpdate(float deltaTime)
        {
            foreach (var evt in _dragStartEvent.publishedChanges)
            {
                ref var transformOrbitalFollowComponent = ref _transformOrbitalFollowStash.Get(evt.Owner, out bool exist);
                if(!exist) return;
                
                ref var followingHandTransformComponent = ref _transformStash.Get(evt.Owner);
                ref var draggableComponent = ref _draggableStash.Get(evt.Draggable);
                ref var draggableTransformComponent = ref _transformStash.Get(evt.Draggable);
                ref var dragComponent = ref _dragStash.Get(evt.Owner); 
                ref var dragStartRaycastComponent = ref _raycastStash.Get(dragComponent.DragRaycast.Entity);
                
                Transform draggableTf = draggableTransformComponent.Transform;
                Transform dragRaycastStartTf = dragStartRaycastComponent.RayStartPoint;
                Transform followingHandTf = followingHandTransformComponent.Transform;
                Transform originalHandTf = transformOrbitalFollowComponent.Target;
                
                transformOrbitalFollowComponent.SphereRadius = 
                    GetDistanceToDraggable(draggableTf, draggableComponent.Colliders, 
                        dragRaycastStartTf, dragComponent.MinDragDistance);
                
                originalHandTf.position = draggableTf.position;
                transformOrbitalFollowComponent.OneFramePermanentCalculation = true;
            }
        }

        private float GetDistanceToDraggable(Transform draggablePos, Collider[] colliders, Transform target, float minDistance)
        {
            float currentMinDistance = float.MaxValue;
            
            foreach (var collider in colliders)
            {
                float currentDistance = Vector3.Distance(target.position, collider.ClosestPointOnBounds(target.position));
                
                if (currentMinDistance > currentDistance)
                    currentMinDistance = currentDistance;
            }
            
            float distanceToDraggableCenter = Vector3.Distance(draggablePos.position, target.position);

            if (currentMinDistance < minDistance)
            {
                float minDistanceDiff = minDistance - currentMinDistance;
                return distanceToDraggableCenter + minDistanceDiff;
            }
            
            return distanceToDraggableCenter;
        }
        
        public void Dispose()
        {
        }
    }
}