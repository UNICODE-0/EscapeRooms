using EscapeRooms.Components;
using Scellecs.Morpeh;
using UnityEngine;
using Unity.IL2CPP.CompilerServices;

namespace EscapeRooms.Systems
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    public sealed class PlayerSlideSystem : ISystem
    {
        public World World { get; set; }

        private Filter _filter;
        private Stash<CharacterControllerComponent> _characterControllerStash;
        private Stash<MovementComponent> _movementStash;
        private Stash<GravityComponent> _gravityStash;
        private Stash<JumpComponent> _jumpStash;
        private Stash<SlideComponent> _slideStash;

        public void OnAwake()
        {
            _filter = World.Filter
                .With<CharacterControllerComponent>()
                .With<MovementComponent>()
                .With<GravityComponent>()
                .With<JumpComponent>()
                .With<SlideComponent>()
                .With<PlayerComponent>()
                .Build();

            _characterControllerStash = World.GetStash<CharacterControllerComponent>();
            _movementStash = World.GetStash<MovementComponent>();
            _gravityStash = World.GetStash<GravityComponent>();
            _jumpStash = World.GetStash<JumpComponent>();
            _slideStash = World.GetStash<SlideComponent>();
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

                Vector3 normal = characterControllerComponent.HitHolder.Hit.normal;
                float slopeAngle = Vector3.Angle(Vector3.up, normal);
                bool isGrounded = characterControllerComponent.CharacterController.isGrounded;
                bool isSliding = slopeAngle > slideComponent.SlideStartAngle && movementComponent.CurrentVelocity == Vector3.zero;

                if (isSliding && isGrounded)
                {
                    slideComponent.CurrentVelocity.x += (1f - normal.y) * normal.x * slideComponent.SlideSpeed;
                    slideComponent.CurrentVelocity.z += (1f - normal.y) * normal.z * slideComponent.SlideSpeed;
                }
                
                else if (slopeAngle < slideComponent.SlideStopAngle)
                    slideComponent.CurrentVelocity = Vector3.zero;
                else
                    slideComponent.CurrentVelocity = Vector3.Lerp(slideComponent.CurrentVelocity, Vector3.zero, deltaTime * 1f);
            }
        }

        public void Dispose()
        {
        }
    }
}