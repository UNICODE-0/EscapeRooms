using EscapeRooms.Components;
using Scellecs.Morpeh;
using Unity.IL2CPP.CompilerServices;

namespace EscapeRooms.Systems
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    public sealed class CharacterGravityAttractionApplySystem : ISystem
    {
        public World World { get; set; }

        private Filter _filter;
        private Stash<CharacterGravityComponent> _characterGravityStash;
        private Stash<CharacterMotionComponent> _characterMotionStash;

        public void OnAwake()
        {
            _filter = World.Filter
                .With<CharacterGravityComponent>()
                .With<CharacterMotionComponent>()
                .Build();

            _characterGravityStash = World.GetStash<CharacterGravityComponent>();
            _characterMotionStash = World.GetStash<CharacterMotionComponent>();
        }

        public void OnUpdate(float deltaTime)
        {
            foreach (var entity in _filter)
            {
                ref var characterMotionComponent = ref _characterMotionStash.Get(entity);
                ref var characterGravityComponent = ref _characterGravityStash.Get(entity);

                characterMotionComponent.CurrentMotion += characterGravityComponent.CurrentAttraction;
            }
        }

        public void Dispose()
        {
        }
    }
}