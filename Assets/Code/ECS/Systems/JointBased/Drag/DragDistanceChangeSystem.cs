using EscapeRooms.Components;
using EscapeRooms.Helpers;
using Scellecs.Morpeh;
using Unity.IL2CPP.CompilerServices;
using UnityEngine;

namespace EscapeRooms.Systems
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    public sealed class DragDistanceChangeSystem : ISystem
    {
        public World World { get; set; }

        private Filter _filter;
        private Stash<DragComponent> _dragStash;
        private Stash<DraggableComponent> _draggableStash;
        private Stash<DragRadiusChangeComponent> _dragRadiusChangeStash;
        private Stash<OneHitRaycastComponent> _raycastStash;
        private Stash<TransformOrbitalFollowComponent> _orbitalFollowStash;

        public void OnAwake()
        {
            _filter = World.Filter
                .With<DragComponent>()
                .With<DragRadiusChangeComponent>()
                .Build();

            _dragStash = World.GetStash<DragComponent>();
            _draggableStash = World.GetStash<DraggableComponent>();
            _raycastStash = World.GetStash<OneHitRaycastComponent>();
            _dragRadiusChangeStash = World.GetStash<DragRadiusChangeComponent>();
            _orbitalFollowStash = World.GetStash<TransformOrbitalFollowComponent>();
        }

        public void OnUpdate(float deltaTime)
        {
            foreach (var entity in _filter)
            {
                ref var dragComponent = ref _dragStash.Get(entity);
                ref var dragRadiusChangeComponent = ref _dragRadiusChangeStash.Get(entity);

                if (dragComponent.IsDragging && dragRadiusChangeComponent.RadiusChangeDeltaInput != Vector2.zero)
                {
                    ref var draggableComponent = ref _draggableStash.Get(dragComponent.DraggableEntity);
                    ref var dragRaycastComponent = ref _raycastStash.Get(dragComponent.DetectionRaycast.Entity);

                    float currentMinDistance = ColliderExtension.GetMinDistanceToClosestPoints(draggableComponent.Colliders,
                        dragRaycastComponent.RayStartPoint.position);

                    ref var orbitalFollowComponent = ref _orbitalFollowStash.Get(entity);

                    float deltaY = dragRadiusChangeComponent.RadiusChangeDeltaInput.y;
                    float radiusAfterChange =
                        orbitalFollowComponent.SphereRadius + deltaY * dragRadiusChangeComponent.Speed;
                    
                    if ((deltaY > 0 || dragComponent.MinDragDistance < currentMinDistance) &&
                        (deltaY < 0 || dragComponent.MaxDragDistance > currentMinDistance))
                        orbitalFollowComponent.SphereRadius = radiusAfterChange;
                }
            }
        }
        
        public void Dispose()
        {
        }
    }
}