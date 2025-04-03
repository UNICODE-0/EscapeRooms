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
        private Stash<TransformPositionLerpComponent> _positionLerpStash;
        private Stash<FloatLerpComponent> _floatLerpStash;
        
        private LerpDataHandler<Vector3> _lerpDataHandler;

        public void OnAwake()
        {
            _filter = World.Filter
                .With<TransformComponent>()
                .With<TransformPositionLerpComponent>()
                .Build();

            _transformStash = World.GetStash<TransformComponent>();
            _positionLerpStash = World.GetStash<TransformPositionLerpComponent>();
            _floatLerpStash = World.GetStash<FloatLerpComponent>();
            
            _lerpDataHandler = new LerpDataHandler<Vector3>(_floatLerpStash);
        }

        public void OnUpdate(float deltaTime)
        {
            foreach (var entity in _filter)
            {
                ref var transformComponent = ref _transformStash.Get(entity);
                ref var positionLerpComponent = ref _positionLerpStash.Get(entity);

                if (_lerpDataHandler.Handle(ref positionLerpComponent.LerpData,
                        out var from, out var to, out float progress))
                {
                    transformComponent.Transform.localPosition = 
                        Vector3.Lerp(from, to, progress);
                }
            }
        }

        public void Dispose()
        {
            _lerpDataHandler = null;
        }
    }
}