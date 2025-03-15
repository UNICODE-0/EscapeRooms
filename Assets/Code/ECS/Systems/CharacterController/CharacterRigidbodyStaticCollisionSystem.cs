using EscapeRooms.Components;
using Scellecs.Morpeh;
using UnityEngine;
using Unity.IL2CPP.CompilerServices;

namespace EscapeRooms.Systems
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    public sealed class CharacterRigidbodyStaticCollisionSystem : ISystem
    {
        public World World { get; set; }

        private Filter _filter;
        private Stash<CharacterMovementComponent> _movementStash;
        private Stash<RigidbodyComponent> _rigidbodyStash;
        private Stash<CharacterRigidbodyStaticCollisionEventComponent> _triggerEventStash;

        public void OnAwake()
        {
            _filter = World.Filter
                .With<RigidbodyComponent>()
                .With<CharacterRigidbodyStaticCollisionEventComponent>()
                .Build();

            _movementStash = World.GetStash<CharacterMovementComponent>();
            _triggerEventStash = World.GetStash<CharacterRigidbodyStaticCollisionEventComponent>();
            _rigidbodyStash = World.GetStash<RigidbodyComponent>();
        }

        public void OnUpdate(float deltaTime)
        {
            foreach (var entity in _filter)
            {
                ref var rigidbodyComponent = ref _rigidbodyStash.Get(entity);
                rigidbodyComponent.Rigidbody.isKinematic = true;
            }
        }
        
        public void Dispose()
        {
        }
    }
}