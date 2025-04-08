using EscapeRooms.Components;
using EscapeRooms.Helpers;
using Scellecs.Morpeh;
using Unity.IL2CPP.CompilerServices;

namespace EscapeRooms.Systems
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    public sealed class CharacterJumpBlockWhileStaticCollisionSystem : ISystem
    {
        public World World { get; set; }

        private Filter _filter;
        private Stash<CharacterStaticCollisionComponent> _staticCollisionStash;
        private Stash<CharacterJumpComponent> _jumpStash;

        public void OnAwake()
        {
            _filter = World.Filter
                .With<CharacterStaticCollisionComponent>()
                .With<CharacterJumpComponent>()
                .With<CharacterJumpBlockWhileStaticCollisionComponent>()
                .Build();

            _staticCollisionStash = World.GetStash<CharacterStaticCollisionComponent>();
            _jumpStash = World.GetStash<CharacterJumpComponent>();
        }

        public void OnUpdate(float deltaTime)
        {
            foreach (var entity in _filter)
            {
                ref var staticCollisionComponent = ref _staticCollisionStash.Get(entity);
                ref var jumpComponent = ref _jumpStash.Get(entity);

                FlagApplier.HandleFlagCondition(ref jumpComponent.JumpBlockFlag, 
                    JumpBlockers.STATIC_COLLISION, 
                    staticCollisionComponent.IsAnyStaticCollisionExist.GetValue());
            }
        }

        public void Dispose()
        {
        }
    }
}