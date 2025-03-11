using EscapeRooms.Components;
using Scellecs.Morpeh;
using UnityEngine;
using Unity.IL2CPP.CompilerServices;

namespace EscapeRooms.Systems
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    public sealed class CharacterSlideSystem : ISystem
    {
        public World World { get; set; }

        private Filter _filter;
        private Stash<CharacterSlideComponent> _slideStash;
        private Stash<CharacterGroundedComponent> _groundedStash;
        private Stash<CharacterMovementComponent> _movementStash;
        private Stash<CharacterControllerHitHolderComponent> _characterControllerHitStash;

        public void OnAwake()
        {
            _filter = World.Filter
                .With<CharacterSlideComponent>()
                .With<CharacterGroundedComponent>()
                .With<CharacterMovementComponent>()
                .With<CharacterControllerHitHolderComponent>()
                .Build();

            _slideStash = World.GetStash<CharacterSlideComponent>();
            _groundedStash = World.GetStash<CharacterGroundedComponent>();
            _movementStash = World.GetStash<CharacterMovementComponent>();
            _characterControllerHitStash = World.GetStash<CharacterControllerHitHolderComponent>();
        }

        public void OnUpdate(float deltaTime)
        {
            foreach (var entity in _filter)
            {
                ref var slideComponent = ref _slideStash.Get(entity);
                ref var groundedComponent = ref _groundedStash.Get(entity);
                ref var movementComponent = ref _movementStash.Get(entity);
                ref var characterControllerHitComponent = ref _characterControllerHitStash.Get(entity);

                Vector3 normal = characterControllerHitComponent.HitHolder.Hit.normal;
                slideComponent.SlopeAngle = characterControllerHitComponent.HitHolder.HitYAngle;
                slideComponent.IsSliding = slideComponent.SlopeAngle > slideComponent.SlideStartAngle;
                
                Vector3 slideDirection = new Vector3(
                    (1f - normal.y) * normal.x,
                    0,
                    (1f - normal.y) * normal.z
                );

                float slideVelocityMagnitude = slideComponent.CurrentVelocity.sqrMagnitude;
                bool isSlideZeroVelocity = slideVelocityMagnitude <= slideComponent.ZeroVelocityMagnitudePrecision;

                bool isMovementNeutralizeSlide = false;
                if(movementComponent.IsMoving)
                {
                     float movementDirectionSimilarity = Vector3.Dot(isSlideZeroVelocity ? slideDirection.normalized : 
                         slideComponent.CurrentVelocity.normalized, movementComponent.CurrentVelocity.normalized);

                    isMovementNeutralizeSlide = movementDirectionSimilarity > slideComponent.MovementSimilarityNeutralizeThresholds.y || 
                                                movementDirectionSimilarity < slideComponent.MovementSimilarityNeutralizeThresholds.x;
                }
                
                if (slideComponent.IsSliding && !isMovementNeutralizeSlide && groundedComponent.IsGrounded && 
                    slideVelocityMagnitude <= slideComponent.MaxSlideVelocityMagnitude)
                {
                    slideComponent.CurrentVelocity.x += slideDirection.x * slideComponent.SlideSpeed;
                    slideComponent.CurrentVelocity.z += slideDirection.z * slideComponent.SlideSpeed;
                }
                else if (slideComponent.SlopeAngle < slideComponent.SlideStopAngle ||
                         isSlideZeroVelocity ||
                         isMovementNeutralizeSlide)
                    slideComponent.CurrentVelocity = Vector3.zero;
                else
                    slideComponent.CurrentVelocity = 
                        Vector3.Lerp(slideComponent.CurrentVelocity, Vector3.zero, deltaTime * slideComponent.SlideVelocityReduction);
            }
        }
        
        public void Dispose()
        {
        }
    }
}