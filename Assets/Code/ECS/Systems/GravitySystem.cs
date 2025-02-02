using EscapeRooms.Components;
using Scellecs.Morpeh;
using UnityEngine;
using Unity.IL2CPP.CompilerServices;

namespace EscapeRooms.Systems
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    public sealed class GravitySystem : ISystem
    {
        public World World { get; set; }

        private Filter _filter;
        private Stash<GravityComponent> _gravityStash;
        private Stash<CharacterControllerComponent> _characterControllerStash;

        public void OnAwake()
        {
            _filter = World.Filter
                .With<GravityComponent>()
                .With<CharacterControllerComponent>()
                .Build();

            _gravityStash = World.GetStash<GravityComponent>();
            _characterControllerStash = World.GetStash<CharacterControllerComponent>();
            ;
        }

        public void OnUpdate(float deltaTime)
        {
            foreach (var entity in _filter)
            {
                ref var gravityComponent = ref _gravityStash.Get(entity);

                if (gravityComponent.IgnoreAttraction)
                {
                    gravityComponent.CurrentAttraction.y = 0f;
                    continue;
                }

                ref var characterControllerComponent = ref _characterControllerStash.Get(entity);
                CharacterController charCon = characterControllerComponent.CharacterController;

                if (charCon.isGrounded)
                    gravityComponent.CurrentAttraction.y = gravityComponent.GroundedAttraction;
                else
                {
                    gravityComponent.CurrentAttraction.y += gravityComponent.GravitationalAttraction * deltaTime;
                }
            }
        }

        public void Dispose()
        {
        }
    }
}