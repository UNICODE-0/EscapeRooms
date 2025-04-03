using EscapeRooms.Components;
using EscapeRooms.Events;
using EscapeRooms.Helpers;
using Scellecs.Morpeh;
using Scellecs.Morpeh.Collections;
using Scellecs.Morpeh.Providers;
using Unity.IL2CPP.CompilerServices;
using UnityEngine;

namespace EscapeRooms.Systems
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    public sealed class RotateSystem : ISystem
    {
        public World World { get; set; }

        private Filter _filter;
        private Stash<RotatableComponent> _rotatableStash;
        private Stash<HingeJointComponent> _hingeJointStash;

        public void OnAwake()
        {
            _filter = World.Filter
                .With<RotatableComponent>()
                .With<OnRotateFlag>()
                .Build();

            _rotatableStash = World.GetStash<RotatableComponent>();
            _hingeJointStash = World.GetStash<HingeJointComponent>();
        }

        public void OnUpdate(float deltaTime)
        {
            foreach (var entity in _filter)
            {
                ref var rotatableComponent = ref _rotatableStash.Get(entity);
                ref var hingeJointComponent = ref _hingeJointStash.Get(entity);

            }
        }

        public void Dispose()
        {
        }
    }
}