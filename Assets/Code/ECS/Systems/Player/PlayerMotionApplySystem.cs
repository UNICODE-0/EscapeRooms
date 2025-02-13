using EscapeRooms.Components;
using Scellecs.Morpeh;
using UnityEngine;
using Unity.IL2CPP.CompilerServices;

namespace EscapeRooms.Systems
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    public sealed class PlayerMotionApplySystem : ISystem
    {
        public World World { get; set; }

        private Filter _filter;
        private Stash<CharacterControllerComponent> _characterControllerStash;
        private Stash<MovementComponent> _movementStash;
        private Stash<CharacterGravityComponent> _gravityStash;
        private Stash<JumpComponent> _jumpStash;
        private Stash<CharacterSlideComponent> _slideStash;
        private Stash<CharacterHeadbuttComponent> _headbuttStash;


        public void OnAwake()
        {
            _filter = World.Filter
                .With<CharacterControllerComponent>()
                .With<MovementComponent>()
                .With<CharacterGravityComponent>()
                .With<JumpComponent>()
                .With<CharacterSlideComponent>()
                .With<PlayerComponent>()
                .Build();

            _characterControllerStash = World.GetStash<CharacterControllerComponent>();
            _movementStash = World.GetStash<MovementComponent>();
            _gravityStash = World.GetStash<CharacterGravityComponent>();
            _jumpStash = World.GetStash<JumpComponent>();
            _slideStash = World.GetStash<CharacterSlideComponent>();
            _headbuttStash = World.GetStash<CharacterHeadbuttComponent>();
        }

        public void OnUpdate(float deltaTime)
        {
            foreach (var entity in _filter)
            {
                ref var characterControllerComponent = ref _characterControllerStash.Get(entity);
                ref var movementComponent = ref _movementStash.Get(entity);
                ref var gravityComponent = ref _gravityStash.Get(entity);
                ref var jumpComponent = ref _jumpStash.Get(entity);
                ref var slideComponent = ref _slideStash.Get(entity);
                ref var headbuttComponent = ref _headbuttStash.Get(entity);

                
                Vector3 motion = (jumpComponent.CurrentForce + gravityComponent.CurrentAttraction +
                                  movementComponent.CurrentVelocity + slideComponent.CurrentVelocity +
                                  headbuttComponent.CurrentForce) * deltaTime;

                characterControllerComponent.CharacterController.Move(motion);
            }
        }

        public void Dispose()
        {
        }
    }
}