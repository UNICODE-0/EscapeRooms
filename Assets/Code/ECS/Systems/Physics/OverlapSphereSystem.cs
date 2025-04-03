using EscapeRooms.Components;
using Scellecs.Morpeh;
using UnityEngine;
using Unity.IL2CPP.CompilerServices;

namespace EscapeRooms.Systems
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    public sealed class OverlapSphereSystem : ISystem
    {
        public World World { get; set; }

        private Filter _filter;
        private Stash<OverlapSphereComponent> _sphereCastStash;

        public void OnAwake()
        {
            _filter = World.Filter
                .With<OverlapSphereComponent>()
                .Build();

            _sphereCastStash = World.GetStash<OverlapSphereComponent>();
        }

        public void OnUpdate(float deltaTime)
        {
            foreach (var entity in _filter)
            {
                ref var sphereCastComponent = ref _sphereCastStash.Get(entity);

                sphereCastComponent.HitColliders ??= new Collider[sphereCastComponent.MaxHitCollidersCount];
                
                sphereCastComponent.HitCollidersCount = Physics.OverlapSphereNonAlloc(sphereCastComponent.SpherePoint.position, 
                    sphereCastComponent.Radius, sphereCastComponent.HitColliders, sphereCastComponent.LayerMask);

                sphereCastComponent.IsSphereIntersect = sphereCastComponent.HitCollidersCount > 0;
            }
        }

        public void Dispose()
        {
        }
    }
}