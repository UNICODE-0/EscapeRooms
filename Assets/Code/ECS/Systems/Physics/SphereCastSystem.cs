using EscapeRooms.Components;
using Scellecs.Morpeh;
using UnityEngine;
using Unity.IL2CPP.CompilerServices;

namespace EscapeRooms.Systems
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    public sealed class SphereCastSystem : ISystem
    {
        public World World { get; set; }

        private Filter _filter;
        private Stash<SphereCastComponent> _sphereCastStash;

        public void OnAwake()
        {
            _filter = World.Filter
                .With<SphereCastComponent>()
                .Build();

            _sphereCastStash = World.GetStash<SphereCastComponent>();
        }

        public void OnUpdate(float deltaTime)
        {
            foreach (var entity in _filter)
            {
                ref var sphereCastComponent = ref _sphereCastStash.Get(entity);

                sphereCastComponent.Hits ??= new RaycastHit[sphereCastComponent.MaxHitsCount];

                sphereCastComponent.HitsCount = Physics.SphereCastNonAlloc(sphereCastComponent.CastStartPoint.position, 
                    sphereCastComponent.Radius, sphereCastComponent.CastStartPoint.rotation * sphereCastComponent.Direction, 
                    sphereCastComponent.Hits, sphereCastComponent.CastLength, sphereCastComponent.LayerMask);
                
                sphereCastComponent.IsSphereHit = sphereCastComponent.HitsCount > 0;
            }
        }

        public void Dispose()
        {
        }
    }
}