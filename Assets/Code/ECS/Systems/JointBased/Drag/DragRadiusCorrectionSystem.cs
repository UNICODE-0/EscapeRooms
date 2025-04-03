using EscapeRooms.Components;
using EscapeRooms.Helpers;
using Scellecs.Morpeh;
using Unity.IL2CPP.CompilerServices;
using UnityEngine;

namespace EscapeRooms.Systems
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    public sealed class DragRadiusCorrectionSystem : ISystem
    {
        public World World { get; set; }

        private Filter _filter;
        private Stash<DragComponent> _dragStash;
        private Stash<TransformOrbitalFollowComponent> _orbitalFollowStash;
        private Stash<DragRadiusCorrectionComponent> _radiusCorrectionStash;
        private Stash<CharacterMovementComponent> _movementStash;

        public void OnAwake()
        {
            _filter = World.Filter
                .With<DragComponent>()
                .With<TransformOrbitalFollowComponent>()
                .With<DragRadiusCorrectionComponent>()
                .Build();

            _dragStash = World.GetStash<DragComponent>();
            _orbitalFollowStash = World.GetStash<TransformOrbitalFollowComponent>();
            _radiusCorrectionStash = World.GetStash<DragRadiusCorrectionComponent>();
            _movementStash = World.GetStash<CharacterMovementComponent>();
        }

        public void OnUpdate(float deltaTime)
        {
            foreach (var entity in _filter)
            {
                ref var dragComponent = ref _dragStash.Get(entity);
                
                if (dragComponent.IsDragging)
                {
                    ref var orbitalFollowComponent = ref _orbitalFollowStash.Get(entity);
                    ref var correctionComponent = ref _radiusCorrectionStash.Get(entity);
                    ref var movementComponent = ref _movementStash.Get(correctionComponent.MovementProvider.Entity);
                    
                    Vector3 target = movementComponent.CurrentVelocity.GetXZ().normalized
                                   * correctionComponent.CorrectionScale;
                    
                    Vector3 lerpTarget = Vector3.Lerp(orbitalFollowComponent.Offset, target, deltaTime * 
                        correctionComponent.CorrectionApplySpeed);
                    
                    orbitalFollowComponent.Offset = lerpTarget;
                }
            }
        }
        
        public void Dispose()
        {
        }
    }
}