using EscapeRooms.Components;
using EscapeRooms.Helpers;
using Scellecs.Morpeh;
using UnityEngine;
using Unity.IL2CPP.CompilerServices;

namespace EscapeRooms.Systems
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    public sealed class TransformDeltaRotationSystem : ISystem
    {
        public World World { get; set; }

        private Filter _filter;
        private Stash<TransformComponent> _transformStash;
        private Stash<TransformDeltaRotationComponent> _rotationStash;

        public void OnAwake()
        {
            _filter = World.Filter
                .With<TransformComponent>()
                .With<TransformDeltaRotationComponent>()
                .Build();

            _transformStash = World.GetStash<TransformComponent>();
            _rotationStash = World.GetStash<TransformDeltaRotationComponent>();
        }

        public void OnUpdate(float deltaTime)
        {
            foreach (var entity in _filter)
            {
                ref var rotationComponent = ref _rotationStash.Get(entity);
                if(rotationComponent.RotationBlockFlag.IsFlagNotClear()) continue;
                
                ref var transformComponent = ref _transformStash.Get(entity);
                transformComponent.Transform.rotation *= Quaternion.Euler(rotationComponent.EulerRotationDelta);
            }
        }

        public void Dispose()
        {
        }
    }
}