using EscapeRooms.Components;
using Scellecs.Morpeh;
using UnityEngine;
using Unity.IL2CPP.CompilerServices;

namespace EscapeRooms.Systems
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    public sealed class OneHitRaycastSystem : ISystem
    {
        public World World { get; set; }

        private Filter _filter;
        private Stash<OneHitRaycastComponent> _raycastStash;

        public void OnAwake()
        {
            _filter = World.Filter
                .With<OneHitRaycastComponent>()
                .Build();

            _raycastStash = World.GetStash<OneHitRaycastComponent>();
        }

        public void OnUpdate(float deltaTime)
        {
            foreach (var entity in _filter)
            {
                ref var raycastComponent = ref _raycastStash.Get(entity);

                raycastComponent.IsRayHit = Physics.Raycast(raycastComponent.RayStartPoint.position,
                    raycastComponent.RayStartPoint.rotation * raycastComponent.Direction, out raycastComponent.Hit,
                    raycastComponent.RayLength, raycastComponent.LayerMask);
                
#if UNITY_EDITOR
                if (raycastComponent.DrawLineToBoundsClosestPoint && raycastComponent.IsRayHit)
                {
                    Vector3 closest = raycastComponent.Hit.collider.ClosestPointOnBounds(raycastComponent.RayStartPoint.position);
                    Debug.DrawLine(raycastComponent.RayStartPoint.position, closest, Color.magenta);
                }
#endif
            }
        }

        public void Dispose()
        {
        }
    }
}