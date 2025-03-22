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

        private Event<DragStartEvent> _dragStartEvent;

        public void OnAwake()
        {
            _transformStash = World.GetStash<TransformComponent>();
            _transformOrbitalFollowStash = World.GetStash<TransformOrbitalFollowComponent>();
            _raycastStash = World.GetStash<RaycastComponent>();
            _dragStash = World.GetStash<DragComponent>();
            
            _dragStartEvent = World.GetEvent<DragStartEvent>();
        }

        public void OnUpdate(float deltaTime)
        {
            foreach (var evt in _dragStartEvent.publishedChanges)
            {
                ref var transformOrbitalFollowComponent = ref _transformOrbitalFollowStash.Get(evt.Owner, out bool exist);
                if(!exist) return;
                
                ref var followingHandTransformComponent = ref _transformStash.Get(evt.Owner);
                ref var draggableTransformComponent = ref _transformStash.Get(evt.Draggable);
                ref var dragComponent = ref _dragStash.Get(evt.Owner); 
                ref var dragStartRaycastComponent = ref _raycastStash.Get(dragComponent.DragRaycast.Entity);

                Transform draggable = draggableTransformComponent.Transform;
                Transform dragRaycastStart = dragStartRaycastComponent.RayStartPoint;
                Transform followingHand = followingHandTransformComponent.Transform;
                Transform originalHand = transformOrbitalFollowComponent.Target;

                float distanceToDraggable = Vector3.Distance(dragRaycastStart.position, draggable.position);
                if (distanceToDraggable < dragComponent.MinDragDistance)
                    distanceToDraggable = dragComponent.MinDragDistance;

                transformOrbitalFollowComponent.SphereRadius = distanceToDraggable;
                followingHand.position = draggable.position;
                originalHand.position = draggable.position;
            }
        }
        
        public void Dispose()
        {
        }
    }
}