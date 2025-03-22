using EscapeRooms.Components;
using Scellecs.Morpeh;
using UnityEngine;
using Unity.IL2CPP.CompilerServices;

namespace EscapeRooms.Systems
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    public sealed class CharacterMotionSystem : ISystem
    {
        public World World { get; set; }

        private Filter _filter;
        private Stash<CharacterControllerComponent> _characterControllerStash;
        private Stash<CharacterMotionComponent> _characterMotionStash;

        public void OnAwake()
        {
            _filter = World.Filter
                .With<CharacterControllerComponent>()
                .With<CharacterMotionComponent>()
                .Build();

            _characterControllerStash = World.GetStash<CharacterControllerComponent>();
            _characterMotionStash = World.GetStash<CharacterMotionComponent>();
        }

        public void OnUpdate(float deltaTime)
        {
            foreach (var entity in _filter)
            {
                ref var characterMotionComponent = ref _characterMotionStash.Get(entity);
                ref var characterControllerComponent = ref _characterControllerStash.Get(entity);

                Vector3 scaledMotion = characterMotionComponent.CurrentMotion * deltaTime;

                characterControllerComponent.CharacterController.Move(scaledMotion);

                characterMotionComponent.LastMotion = characterMotionComponent.CurrentMotion;
                characterMotionComponent.CurrentMotion = Vector3.zero;
            }
        }

        public void Dispose()
        {
        }
    }
}