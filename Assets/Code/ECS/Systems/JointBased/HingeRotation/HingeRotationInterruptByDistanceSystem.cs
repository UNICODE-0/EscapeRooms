using EscapeRooms.Components;
using Scellecs.Morpeh;
using Unity.IL2CPP.CompilerServices;
using UnityEngine;

namespace EscapeRooms.Systems
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    public sealed class HingeRotationInterruptByDistanceSystem : ISystem
    {
        public World World { get; set; }

        private Filter _filter;
        private Stash<HingeRotationComponent> _hingeRotationStash;
        private Stash<TransformComponent> _transformStash;

        public void OnAwake()
        {
            _filter = World.Filter
                .With<HingeRotationComponent>()
                .Build();

            _hingeRotationStash = World.GetStash<HingeRotationComponent>();
            _transformStash = World.GetStash<TransformComponent>();
        }

        public void OnUpdate(float deltaTime)
        {
            foreach (var entity in _filter)
            {
                ref var hingeRotationComponent = ref _hingeRotationStash.Get(entity);

                if(hingeRotationComponent.IsRotating)
                {
                    ref var handTransformComponent = ref _transformStash.Get(entity);
                    ref var rotatableTransformComponent = ref _transformStash.Get(hingeRotationComponent.RotatableEntity);

                    float distance = Vector3.Distance(handTransformComponent.Transform.position,
                        rotatableTransformComponent.Transform.position);
            
                    if (distance >= hingeRotationComponent.MaxDeviation)
                    {
                        hingeRotationComponent.RotateStopInput = true;
                    }
                }
            }
        }
        
        public void Dispose()
        {
        }
    }
}