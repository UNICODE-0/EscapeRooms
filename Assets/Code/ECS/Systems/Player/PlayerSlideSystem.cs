using EscapeRooms.Components;
using Scellecs.Morpeh;
using UnityEngine;
using Unity.IL2CPP.CompilerServices;
using Unity.Mathematics;

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
        private Stash<SlideComponent> _slideStash;
        private Stash<GroundedComponent> _groundedStash;

        public void OnAwake()
        {
            _filter = World.Filter
                .With<CharacterControllerComponent>()
                .With<SlideComponent>()
                .With<GroundedComponent>()
                .With<PlayerComponent>()
                .Build();

            _characterControllerStash = World.GetStash<CharacterControllerComponent>();
            _slideStash = World.GetStash<SlideComponent>();
            _groundedStash = World.GetStash<GroundedComponent>();
        }

        public void OnUpdate(float deltaTime)
        {
            foreach (var entity in _filter)
            {
                ref var characterControllerComponent = ref _characterControllerStash.Get(entity);
                ref var slideComponent = ref _slideStash.Get(entity);
                ref var groundedComponent = ref _groundedStash.Get(entity);

                Vector3 normal = characterControllerComponent.HitHolder.Hit.normal;
                slideComponent.SlopeAngle = characterControllerComponent.HitHolder.HitYAngle;
                slideComponent.IsSliding = slideComponent.SlopeAngle > slideComponent.SlideStartAngle;

                if (slideComponent.IsSliding && groundedComponent.IsGrounded && slideComponent.CurrentVelocity.sqrMagnitude 
                    < slideComponent.MaxSlideVelocityMagnitude)
                {
                    Vector3 slideDirection = Vector3.zero;
                    slideDirection.x = (1f - normal.y) * normal.x;
                    slideDirection.z = (1f - normal.y) * normal.z;

                    slideComponent.CurrentVelocity.x += slideDirection.x * slideComponent.SlideSpeed;
                    slideComponent.CurrentVelocity.z += slideDirection.z * slideComponent.SlideSpeed;
                }
                else if (slideComponent.SlopeAngle < slideComponent.SlideStopAngle 
                         || slideComponent.CurrentVelocity.sqrMagnitude <= slideComponent.ZeroVelocityMagnitudePrecision)
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