using EscapeRooms.Components;
using Scellecs.Morpeh;
using Unity.IL2CPP.CompilerServices;

namespace EscapeRooms.Systems
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    public sealed class CharacterGravitySystem : ISystem
    {
        public World World { get; set; }

        private Filter _filter;
        private Stash<CharacterGravityComponent> _gravityStash;
        private Stash<CharacterGroundedComponent> _groundedStash;

        public void OnAwake()
        {
            _filter = World.Filter
                .With<CharacterGravityComponent>()
                .With<CharacterGroundedComponent>()
                .Build();

            _gravityStash = World.GetStash<CharacterGravityComponent>();
            _groundedStash = World.GetStash<CharacterGroundedComponent>();
        }

        public void OnUpdate(float deltaTime)
        {
            foreach (var entity in _filter)
            {
                ref var gravityComponent = ref _gravityStash.Get(entity);
                ref var groundedComponent = ref _groundedStash.Get(entity);

                if (gravityComponent.IgnoreAttraction)
                {
                    gravityComponent.CurrentAttraction.y = 0f;
                    continue;
                }


                if (groundedComponent.IsGrounded)
                    gravityComponent.CurrentAttraction.y = gravityComponent.GroundedAttraction;
                else
                    gravityComponent.CurrentAttraction.y += gravityComponent.GravitationalAttraction * deltaTime;
            }
        }

        public void Dispose()
        {
        }
    }
}