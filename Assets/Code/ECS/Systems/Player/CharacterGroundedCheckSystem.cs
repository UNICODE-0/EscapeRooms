using EscapeRooms.Components;
using Scellecs.Morpeh;
using UnityEngine;
using Unity.IL2CPP.CompilerServices;

namespace EscapeRooms.Systems
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    public sealed class CharacterGroundedCheckSystem : ISystem
    {
        public World World { get; set; }

        private Filter _filter;
        private Stash<CharacterGroundedComponent> _groundedStash;
        private Stash<CharacterControllerComponent> _characterControllerStash;

        public void OnAwake()
        {
            _filter = World.Filter
                .With<CharacterGroundedComponent>()
                .With<CharacterControllerComponent>()
                .Build();

            _groundedStash = World.GetStash<CharacterGroundedComponent>();
            _characterControllerStash = World.GetStash<CharacterControllerComponent>();
        }

        public void OnUpdate(float deltaTime)
        {
            foreach (var entity in _filter)
            {
                ref var groundedComponent = ref _groundedStash.Get(entity);
                ref var characterComponent = ref _characterControllerStash.Get(entity);

                groundedComponent.IsGrounded = characterComponent.CharacterController.isGrounded;
            }
        }

        public void Dispose()
        {
        }
    }
}