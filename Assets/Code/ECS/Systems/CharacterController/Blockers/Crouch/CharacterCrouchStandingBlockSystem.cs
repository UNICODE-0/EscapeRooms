using System.Collections.Generic;
using EscapeRooms.Components;
using EscapeRooms.Data;
using Scellecs.Morpeh;
using Unity.IL2CPP.CompilerServices;

namespace EscapeRooms.Systems
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    public sealed class CharacterCrouchStandingBlockSystem : ISystem
    {
        public const int CrouchStandBlockFlag = 1 << 2;
        public const int CrouchStandingBlockFlag = 1 << 3;

        public World World { get; set; }

        private Filter _filter;
        private Stash<CharacterCrouchStandingBlockComponent> _standingBlockStash;
        private Stash<CharacterCrouchComponent> _crouchStash;
        private Stash<SphereCastComponent> _sphereCastStash;
        private Stash<OverlapSphereComponent> _overlapSphereStash;

        public void OnAwake()
        {
            _filter = World.Filter
                .With<CharacterCrouchStandingBlockComponent>()
                .With<CharacterCrouchComponent>()
                .Build();

            _standingBlockStash = World.GetStash<CharacterCrouchStandingBlockComponent>();
            _crouchStash = World.GetStash<CharacterCrouchComponent>();
            _sphereCastStash = World.GetStash<SphereCastComponent>();
            _overlapSphereStash = World.GetStash<OverlapSphereComponent>();
        }

        public void OnUpdate(float deltaTime)
        {
            foreach (var entity in _filter)
            {
                ref var standingBlockComponent = ref _standingBlockStash.Get(entity);
                ref var crouchComponent = ref _crouchStash.Get(entity);
                ref var sphereCastComponent = ref _sphereCastStash.Get(standingBlockComponent.StandPossibilityCheckSphereCast.Entity);
                ref var sphereOverlapComponent = ref _overlapSphereStash.Get(standingBlockComponent.StandingPossibilityCheckSphereOverlap.Entity);

                FlagApplier.HandleFlagCondition(ref crouchComponent.CrouchBlockFlag, 
                    CrouchStandBlockFlag, sphereCastComponent.IsSphereHit);
                
                FlagApplier.HandleFlagCondition(ref crouchComponent.CrouchBlockFlag, 
                    CrouchStandingBlockFlag, sphereOverlapComponent.IsSphereIntersect);
            }
        }

        public void Dispose()
        {
        }
    }
}