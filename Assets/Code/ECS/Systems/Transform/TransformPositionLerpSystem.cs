using EscapeRooms.Components;
using Scellecs.Morpeh;
using UnityEngine;
using Unity.IL2CPP.CompilerServices;

namespace EscapeRooms.Systems
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    public sealed class TransformPositionLerpSystem : ISystem
    {
        public World World { get; set; }

        private Filter _filter;
        private Stash<TransformComponent> _transformStash;
        private Stash<TransformPositionLerpComponent> _lerpStash;

        public void OnAwake()
        {
            _filter = World.Filter
                .With<TransformComponent>()
                .With<TransformDeltaRotationComponent>()
                .Build();

            _transformStash = World.GetStash<TransformComponent>();
            _lerpStash = World.GetStash<TransformPositionLerpComponent>();
        }

        public void OnUpdate(float deltaTime)
        {
            foreach (var entity in _filter)
            {
                ref var transformComponent = ref _transformStash.Get(entity);
                ref var lerpComponent = ref _lerpStash.Get(entity);

            }
        }

        public void Dispose()
        {
        }
    }
}