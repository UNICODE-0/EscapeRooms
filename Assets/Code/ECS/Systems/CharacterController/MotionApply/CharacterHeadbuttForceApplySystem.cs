using EscapeRooms.Components;
using Scellecs.Morpeh;
using Unity.IL2CPP.CompilerServices;

namespace EscapeRooms.Systems
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    public sealed class CharacterHeadbuttForceApplySystem : ISystem
    {
        public World World { get; set; }

        private Filter _filter;
        private Stash<CharacterHeadbuttComponent> _characterHeadbuttStash;
        private Stash<CharacterMotionComponent> _characterMotionStash;

        public void OnAwake()
        {
            _filter = World.Filter
                .With<CharacterHeadbuttComponent>()
                .With<CharacterMotionComponent>()
                .Build();

            _characterHeadbuttStash = World.GetStash<CharacterHeadbuttComponent>();
            _characterMotionStash = World.GetStash<CharacterMotionComponent>();
        }

        public void OnUpdate(float deltaTime)
        {
            foreach (var entity in _filter)
            {
                ref var characterMotionComponent = ref _characterMotionStash.Get(entity);
                ref var characterHeadbuttComponent = ref _characterHeadbuttStash.Get(entity);

                characterMotionComponent.CurrentMotion += characterHeadbuttComponent.CurrentForce;
            }
        }

        public void Dispose()
        {
        }
    }
}