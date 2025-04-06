using EscapeRooms.Components;
using EscapeRooms.Helpers;
using Scellecs.Morpeh;
using Unity.IL2CPP.CompilerServices;

namespace EscapeRooms.Systems
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    public sealed class CharacterCrouchStandingBlockSystem : ISystem
    {
        public World World { get; set; }

        private Filter _filter;
        private Stash<CharacterCrouchStandingBlockComponent> _standingBlockStash;
        private Stash<CharacterCrouchComponent> _crouchStash;
        private Stash<OverlapSphereComponent> _overlapSphereStash;

        public void OnAwake()
        {
            _filter = World.Filter
                .With<CharacterCrouchStandingBlockComponent>()
                .With<CharacterCrouchComponent>()
                .Build();

            _standingBlockStash = World.GetStash<CharacterCrouchStandingBlockComponent>();
            _crouchStash = World.GetStash<CharacterCrouchComponent>();
            _overlapSphereStash = World.GetStash<OverlapSphereComponent>();
        }

        public void OnUpdate(float deltaTime)
        {
            foreach (var entity in _filter)
            {
                ref var standingBlockComponent = ref _standingBlockStash.Get(entity);
                ref var crouchComponent = ref _crouchStash.Get(entity);
                ref var sphereOverlapComponent = ref _overlapSphereStash.Get(standingBlockComponent.StandingPossibilityCheckSphereOverlap.Entity);

                FlagApplier.HandleFlagCondition(ref crouchComponent.CrouchBlockFlag, 
                    CrouchBlockers.STANDING, 
                    crouchComponent.IsCrouching && sphereOverlapComponent.IsSphereIntersect);
            }
        }

        public void Dispose()
        {
        }
    }
}