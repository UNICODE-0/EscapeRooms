using EscapeRooms.Components;
using Scellecs.Morpeh;
using Unity.IL2CPP.CompilerServices;
using UnityEngine;

namespace EscapeRooms.Systems
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    public sealed class HingeRotationInterruptByCollisionSystem : ISystem
    {
        public World World { get; set; }

        private Filter _filter;
        private Stash<HingeRotationComponent> _hingeRotationStash;
        private Stash<ColliderUniqueTriggerEventsHolderComponent> _triggerStash;
        
        public void OnAwake()
        {
            _filter = World.Filter
                .With<HingeRotationComponent>()
                .With<ColliderUniqueTriggerEventsHolderComponent>()
                .Build();

            _hingeRotationStash = World.GetStash<HingeRotationComponent>();
            _triggerStash = World.GetStash<ColliderUniqueTriggerEventsHolderComponent>();
        }

        public void OnUpdate(float deltaTime)
        {
            foreach (var entity in _filter)
            {
                ref var hingeRotationComponent = ref _hingeRotationStash.Get(entity);

                if(hingeRotationComponent.IsRotating)
                {
                    ref var triggerComponent = ref _triggerStash.Get(entity);

                    if (triggerComponent.EventsHolder.IsAnyUniqueTriggerInProgress)
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