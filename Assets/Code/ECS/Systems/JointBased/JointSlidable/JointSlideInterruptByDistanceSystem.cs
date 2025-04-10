using EscapeRooms.Components;
using Scellecs.Morpeh;
using Unity.IL2CPP.CompilerServices;
using UnityEngine;

namespace EscapeRooms.Systems
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    public sealed class JointSlideInterruptByDistanceSystem : ISystem
    {
        public World World { get; set; }

        private Filter _filter;
        private Stash<JointSlideComponent> _JointSlideStash;
        private Stash<TransformComponent> _transformStash;

        public void OnAwake()
        {
            _filter = World.Filter
                .With<HingeRotationComponent>()
                .Build();

            _JointSlideStash = World.GetStash<JointSlideComponent>();
            _transformStash = World.GetStash<TransformComponent>();
        }

        public void OnUpdate(float deltaTime)
        {
            foreach (var entity in _filter)
            {
                ref var jointSlideComponent = ref _JointSlideStash.Get(entity);

                if(jointSlideComponent.IsSliding)
                {
                    ref var handTransformComponent = ref _transformStash.Get(entity);
                    ref var slidableTransformComponent = ref _transformStash.Get(jointSlideComponent.SlidableEntity);

                    float distance = Vector3.Distance(handTransformComponent.Transform.position,
                        slidableTransformComponent.Transform.position);

                    if (distance >= jointSlideComponent.MaxDeviation)
                    {
                        jointSlideComponent.SlideStopInput = true;
                    }
                }
            }
        }
        
        public void Dispose()
        {
        }
    }
}