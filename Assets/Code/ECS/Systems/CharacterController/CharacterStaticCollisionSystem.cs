using EscapeRooms.Components;
using EscapeRooms.Helpers;
using Scellecs.Morpeh;
using UnityEngine;
using Unity.IL2CPP.CompilerServices;

namespace EscapeRooms.Systems
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    public sealed class CharacterStaticCollisionSystem : ISystem
    {
        public World World { get; set; }

        private Filter _filter;
        
        private Stash<CharacterMovementComponent> _movementStash;
        private Stash<RigidbodyComponent> _rigidbodyStash;
        private Stash<CharacterStaticCollisionFlag> _triggerEventStash;
        private Stash<CharacterStaticCollisionComponent> _characterStaticCollisionStash;
        private Stash<TransformComponent> _transformStash;

        public void OnAwake()
        {
            _filter = World.Filter
                .With<RigidbodyComponent>()
                .With<TransformComponent>()
                .With<CharacterStaticCollisionFlag>()
                .Build();

            _movementStash = World.GetStash<CharacterMovementComponent>();
            _triggerEventStash = World.GetStash<CharacterStaticCollisionFlag>();
            _rigidbodyStash = World.GetStash<RigidbodyComponent>();
            _characterStaticCollisionStash = World.GetStash<CharacterStaticCollisionComponent>();
            _transformStash = World.GetStash<TransformComponent>();
        }

        public void OnUpdate(float deltaTime)
        {
            foreach (var entity in _filter)
            {
                ref var triggerEventComponent = ref _triggerEventStash.Get(entity);
                ref var characterStaticCollisionComponent = 
                    ref _characterStaticCollisionStash.Get(triggerEventComponent.Owner, out bool exist);
                
                if(!exist) return;
                
                ref var characterMovementComponent = ref _movementStash.Get(triggerEventComponent.Owner);
                ref var rigidbodyComponent = ref _rigidbodyStash.Get(entity);

                ref var transformComponent = ref _transformStash.Get(entity);
                ref var characterTransformComponent = ref _transformStash.Get(triggerEventComponent.Owner);

                Vector3 targetPos2D = transformComponent.Transform.position.GetXZ();
                Vector3 characterPos2D = characterTransformComponent.Transform.position.GetXZ();
                Vector3 directionToTarget = (targetPos2D - characterPos2D).normalized;
                
                float movementDirectionSimilarity =
                    Vector3.Dot(directionToTarget, characterMovementComponent.CurrentVelocity.normalized);

                bool isMovingOverThreshold = movementDirectionSimilarity <=
                                            characterStaticCollisionComponent.DirectionToTargetSimilarityThreshold && 
                                            characterMovementComponent.IsMoving;
                
                ref var anyCollisionExist = ref characterStaticCollisionComponent.IsAnyStaticCollisionExist;
                
                if (triggerEventComponent.IsLastFrameOfLife 
                    || !characterMovementComponent.IsMoving 
                    || isMovingOverThreshold)
                {
                    anyCollisionExist.SetFalse(notTrueOnThisFrame: true);
                    rigidbodyComponent.Rigidbody.isKinematic = false;
                }
                else
                {
                    anyCollisionExist.SetTrue();
                    rigidbodyComponent.Rigidbody.isKinematic = true;
                }
            }
        }
        
        public void Dispose()
        {
        }
    }
}