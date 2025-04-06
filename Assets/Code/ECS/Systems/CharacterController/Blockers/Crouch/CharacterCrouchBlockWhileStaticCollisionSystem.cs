using EscapeRooms.Components;
using EscapeRooms.Helpers;
using Scellecs.Morpeh;
using Unity.IL2CPP.CompilerServices;
using UnityEngine;

namespace EscapeRooms.Systems
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    public sealed class CharacterCrouchBlockWhileStaticCollisionSystem : ISystem
    {
        public World World { get; set; }

        private Filter _filter;
        private Stash<CharacterStaticCollisionComponent> _staticCollisionStash;
        private Stash<CharacterCrouchComponent> _crouchStash;

        public void OnAwake()
        {
            _filter = World.Filter
                .With<CharacterStaticCollisionComponent>()
                .With<CharacterCrouchComponent>()
                .With<CharacterCrouchBlockWhileStaticCollisionComponent>()
                .Build();

            _staticCollisionStash = World.GetStash<CharacterStaticCollisionComponent>();
            _crouchStash = World.GetStash<CharacterCrouchComponent>();
        }

        public void OnUpdate(float deltaTime)
        {
            foreach (var entity in _filter)
            {
                ref var staticCollisionComponent = ref _staticCollisionStash.Get(entity);
                ref var crouchComponent = ref _crouchStash.Get(entity);

                FlagApplier.HandleFlagCondition(ref crouchComponent.CrouchBlockFlag, 
                    CrouchBlockers.STATIC_COLLISION, 
                    staticCollisionComponent.IsAnyStaticCollisionExist.GetValue());
            }
        }

        public void Dispose()
        {
        }
    }
}