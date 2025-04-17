using EscapeRooms.Components;
using Scellecs.Morpeh;
using UnityEngine;
using Unity.IL2CPP.CompilerServices;

namespace EscapeRooms.Systems
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    public sealed class OverlapBoxSystem : ISystem
    {
        public World World { get; set; }

        private Filter _filter;
        private Stash<OverlapBoxComponent> _overlapStash;

        public void OnAwake()
        {
            _filter = World.Filter
                .With<OverlapBoxComponent>()
                .Build();

            _overlapStash = World.GetStash<OverlapBoxComponent>();
        }

        public void OnUpdate(float deltaTime)
        {
            foreach (var entity in _filter)
            {
                ref var overlapComponent = ref _overlapStash.Get(entity);

                overlapComponent.HitColliders ??= new Collider[overlapComponent.MaxHitCollidersCount];

                overlapComponent.HitCollidersCount = Physics.OverlapBoxNonAlloc(overlapComponent.BoxPoint.position,
                    overlapComponent.HalfExtents, overlapComponent.HitColliders, overlapComponent.BoxPoint.rotation,
                    overlapComponent.LayerMask);

                overlapComponent.IsBoxIntersect = overlapComponent.HitCollidersCount > 0;
            }
        }

        public void Dispose()
        {
        }
    }
}