using EscapeRooms.Components;
using Scellecs.Morpeh;
using Scellecs.Morpeh.Collections;
using Scellecs.Morpeh.Providers;
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

        public void OnAwake()
        {
            _filter = World.Filter
                .With<TransformComponent>()
                .With<TransformPositionLerpComponent>()
                .Build();

            _transformStash = World.GetStash<TransformComponent>();
            _positionLerpStash = World.GetStash<TransformPositionLerpComponent>();
            _floatLerpStash = World.GetStash<FloatLerpComponent>();
        }

        public void OnUpdate(float deltaTime)
        {
            foreach (var entity in _filter)
            {
                ref var transformComponent = ref _transformStash.Get(entity);
                ref var positionLerpComponent = ref _positionLerpStash.Get(entity);
                ref var floatLerpComponent = ref _floatLerpStash.Get(positionLerpComponent.FloatLerpProvider.Entity);

                floatLerpComponent.StartLerpInput = positionLerpComponent.ChangePositionInput;

                if (floatLerpComponent.IsLerpInProgress)
                {
                    ref Vector3 from = ref positionLerpComponent.IsTargetState ? 
                        ref positionLerpComponent.TargetPosition : ref positionLerpComponent.DefaultPosition;
                    
                    ref Vector3 to = ref positionLerpComponent.IsTargetState ?
                        ref positionLerpComponent.DefaultPosition : ref positionLerpComponent.TargetPosition;
                    
                    transformComponent.Transform.localPosition = 
                        Vector3.Lerp(from, to, floatLerpComponent.CurrentValue);

                    if (floatLerpComponent.IsLerpTimeIsUp)
                        positionLerpComponent.IsTargetState = !positionLerpComponent.IsTargetState;
                }
            }
        }

        public void Dispose()
        {
        }
    }
}