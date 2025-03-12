using EscapeRooms.Components;
using Scellecs.Morpeh;
using UnityEngine;
using Unity.IL2CPP.CompilerServices;

namespace EscapeRooms.Systems
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    public sealed class CharacterLedgeCorrectionSystem : ISystem
    {
        public World World { get; set; }

        private Filter _filter;
        private Stash<CharacterControllerComponent> _characterControllerStash;
        private Stash<CharacterSlideComponent> _slideStash;
        private Stash<CharacterGroundedComponent> _groundedStash;
        private Stash<CharacterLedgeCorrectionComponent> _ledgeCorrectionStash;

        private Vector3 _previousFramePosition;

        public void OnAwake()
        {
            _filter = World.Filter
                .With<CharacterControllerComponent>()
                .With<CharacterSlideComponent>()
                .With<CharacterGroundedComponent>()
                .With<CharacterLedgeCorrectionComponent>()
                .Build();

            _characterControllerStash = World.GetStash<CharacterControllerComponent>();
            _slideStash = World.GetStash<CharacterSlideComponent>();
            _groundedStash = World.GetStash<CharacterGroundedComponent>();
            _ledgeCorrectionStash = World.GetStash<CharacterLedgeCorrectionComponent>();
        }

        public void OnUpdate(float deltaTime)
        {
            foreach (var entity in _filter)
            {
                ref var characterControllerComponent = ref _characterControllerStash.Get(entity);
                ref var slideComponent = ref _slideStash.Get(entity);
                ref var groundedComponent = ref _groundedStash.Get(entity);
                ref var ledgeCorrectionComponent = ref _ledgeCorrectionStash.Get(entity);

                if (!(slideComponent.IsSliding && groundedComponent.IsGrounded))
                {
                    characterControllerComponent.CharacterController.stepOffset = ledgeCorrectionComponent.OutOfLedgeStepOffset;
                }

                float minAngle = ledgeCorrectionComponent.LedgeDetectionInterval.x;
                float maxAngle = ledgeCorrectionComponent.LedgeDetectionInterval.y;

                if (slideComponent.SlopeAngle > minAngle && slideComponent.SlopeAngle < maxAngle)
                {
                    Vector3 difference = 
                        _previousFramePosition - characterControllerComponent.CharacterController.transform.position;
                    
                    float minDiff = ledgeCorrectionComponent.PositionDifferenceDetectionInterval.x;
                    float maxDiff = ledgeCorrectionComponent.PositionDifferenceDetectionInterval.y;
                    
                    if (difference.y >= minDiff && difference.y < maxDiff)
                    {
                        characterControllerComponent.CharacterController.stepOffset = ledgeCorrectionComponent.LedgeStepOffset;
                    }
                }
                else
                {
                    characterControllerComponent.CharacterController.stepOffset = ledgeCorrectionComponent.OutOfLedgeStepOffset;
                }
                
                _previousFramePosition = characterControllerComponent.CharacterController.transform.position;
            }
        }
        
        public void Dispose()
        {
        }
    }
}