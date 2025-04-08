using EscapeRooms.Components;
using EscapeRooms.Helpers;
using Scellecs.Morpeh;
using Unity.IL2CPP.CompilerServices;

namespace EscapeRooms.Systems
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    public sealed class CharacterCrouchBlockWhileFallingSystem : ISystem
    {
        public World World { get; set; }

        private Filter _filter;
        private Stash<CharacterGroundedComponent> _groundedStash;
        private Stash<CharacterCrouchComponent> _crouchStash;

        public void OnAwake()
        {
            _filter = World.Filter
                .With<CharacterGroundedComponent>()
                .With<CharacterCrouchComponent>()
                .With<CharacterCrouchBlockWhileFallingComponent>()
                .Build();

            _groundedStash = World.GetStash<CharacterGroundedComponent>();
            _crouchStash = World.GetStash<CharacterCrouchComponent>();
        }

        public void OnUpdate(float deltaTime)
        {
            foreach (var entity in _filter)
            {
                ref var groundComponent = ref _groundedStash.Get(entity);
                ref var crouchComponent = ref _crouchStash.Get(entity);
                
                FlagApplier.HandleFlagCondition(ref crouchComponent.CrouchBlockFlag, 
                    CrouchBlockers.FALLING, !groundComponent.IsGrounded);
            }
        }

        public void Dispose()
        {
        }
    }
}