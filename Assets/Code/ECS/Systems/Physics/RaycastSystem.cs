using EscapeRooms.Components;
using Scellecs.Morpeh;
using UnityEngine;
using Unity.IL2CPP.CompilerServices;

namespace EscapeRooms.Systems
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    public sealed class RaycastSystem : ISystem
    {
        public World World { get; set; }

        private Filter _filter;
        private Stash<RaycastComponent> _raycastStash;

        public void OnAwake()
        {
            _filter = World.Filter
                .With<RaycastComponent>()
                .Build();

            _raycastStash = World.GetStash<RaycastComponent>();
        }

        public void OnUpdate(float deltaTime)
        {
            foreach (var entity in _filter)
            {
                ref var raycastComponent = ref _raycastStash.Get(entity);

                raycastComponent.Hits ??= new RaycastHit[raycastComponent.MaxHitsCount];

                raycastComponent.HitsCount = Physics.RaycastNonAlloc(raycastComponent.RayStartPoint.position,
                    raycastComponent.RayStartPoint.rotation * raycastComponent.Direction, 
                    raycastComponent.Hits, raycastComponent.RayLength, raycastComponent.LayerMask);
                
#if UNITY_EDITOR
                if (raycastComponent.DrawLineToBoundsClosestPoint && raycastComponent.HitsCount > 0)
                {
                    var hit = raycastComponent.Hits[0];
                    Vector3 closest = hit.collider.ClosestPointOnBounds(raycastComponent.RayStartPoint.position);
                    Debug.DrawLine(raycastComponent.RayStartPoint.position, closest, Color.magenta);
                }
#endif

                raycastComponent.IsRayHit = raycastComponent.HitsCount > 0;
            }
        }

        public void Dispose()
        {
        }
    }
}