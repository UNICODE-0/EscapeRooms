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

        private Vector3 pp;

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
                bool isSliding = slopeAngle > slideComponent.SlideStartAngle;

                slideComponent.CurrentVelocity.y = 0f;
                if (isSliding && isGrounded)
                {
                    Vector3 slideDirection = Vector3.zero;
                    slideDirection.x = (1f - normal.y) * normal.x;
                    slideDirection.z = (1f - normal.y) * normal.z;

                    // Vector3 dif = pp - characterControllerComponent.CharacterController.transform.position;
                    // if (slopeAngle > 70 && dif.y < 0.05f && dif.y >= 0f)
                    // {
                    //     // float angle = Vector3.Angle(-normal, characterControllerComponent.CharacterController.transform.rotation * movementComponent.CurrentVelocity);
                    //     // float factor = 90f - (angle / 90f);
                    //     // Debug.LogError(slopeAngle);
                    //     // movementComponent.CurrentVelocity = Vector3.Lerp(movementComponent.CurrentVelocity, Vector3.zero, factor);
                    //     slideComponent.CurrentVelocity.y += 10f;
                    // }
                    //
                    // pp = characterControllerComponent.CharacterController.transform.position;

                    if (movementComponent.CurrentVelocity == Vector3.zero)
                    {
                        slideComponent.CurrentVelocity.x += slideDirection.x * slideComponent.SlideSpeed;
                        slideComponent.CurrentVelocity.z += slideDirection.z * slideComponent.SlideSpeed;
                    }
                }
                else if (slopeAngle < slideComponent.SlideStopAngle)
                    slideComponent.CurrentVelocity = Vector3.zero;
                else
                    slideComponent.CurrentVelocity = 
                        Vector3.Lerp(slideComponent.CurrentVelocity, Vector3.zero, deltaTime * slideComponent.SlideSpeedReduction);
            }
        }

        public void Dispose()
        {
        }
    }
}