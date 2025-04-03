using EscapeRooms.Components;
using Scellecs.Morpeh;
using Unity.IL2CPP.CompilerServices;

namespace EscapeRooms.Systems
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    public sealed class CharacterSlideVelocityApplySystem : ISystem
    {
        public World World { get; set; }

        private Filter _filter;
        private Stash<CharacterSlideComponent> _characterSlideStash;
        private Stash<CharacterMotionComponent> _characterMotionStash;

        public void OnAwake()
        {
            _filter = World.Filter
                .With<CharacterSlideComponent>()
                .With<CharacterMotionComponent>()
                .Build();

            _characterSlideStash = World.GetStash<CharacterSlideComponent>();
            _characterMotionStash = World.GetStash<CharacterMotionComponent>();
        }

        public void OnUpdate(float deltaTime)
        {
            foreach (var entity in _filter)
            {
                ref var characterMotionComponent = ref _characterMotionStash.Get(entity);
                ref var characterSlideComponent = ref _characterSlideStash.Get(entity);

                characterMotionComponent.CurrentMotion += characterSlideComponent.CurrentVelocity;
            }
        }

        public void Dispose()
        {
        }
    }
}