using EscapeRooms.Components;
using EscapeRooms.Helpers;
using Scellecs.Morpeh;
using Unity.IL2CPP.CompilerServices;

namespace EscapeRooms.Systems
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    public sealed class CharacterCrouchStandBlockSystem : ISystem
    {
        public World World { get; set; }

        private Filter _filter;
        private Stash<CharacterCrouchStandBlockComponent> _standBlockStash;
        private Stash<CharacterCrouchComponent> _crouchStash;
        private Stash<SphereCastComponent> _sphereCastStash;

        public void OnAwake()
        {
            _filter = World.Filter
                .With<CharacterCrouchStandBlockComponent>()
                .With<CharacterCrouchComponent>()
                .Build();

            _standBlockStash = World.GetStash<CharacterCrouchStandBlockComponent>();
            _crouchStash = World.GetStash<CharacterCrouchComponent>();
            _sphereCastStash = World.GetStash<SphereCastComponent>();
        }

        public void OnUpdate(float deltaTime)
        {
            foreach (var entity in _filter)
            {
                ref var standBlockComponent = ref _standBlockStash.Get(entity);
                ref var crouchComponent = ref _crouchStash.Get(entity);
                ref var sphereCastComponent = ref _sphereCastStash.Get(standBlockComponent.StandPossibilityCheckSphereCast.Entity);

                FlagApplier.HandleFlagCondition(ref crouchComponent.CrouchBlockFlag, 
                    CrouchBlockers.STAND, 
                    crouchComponent.IsCrouching && sphereCastComponent.IsSphereHit);
            }
        }

        public void Dispose()
        {
        }
    }
}