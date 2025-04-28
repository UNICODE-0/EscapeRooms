using EscapeRooms.Components;
using Scellecs.Morpeh;
using Scellecs.Morpeh.Collections;
using Scellecs.Morpeh.Providers;
using Unity.IL2CPP.CompilerServices;
using UnityEngine;

namespace EscapeRooms.Systems
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    public sealed class CharacterGroundedCheckSystem : ISystem
    {
        public World World { get; set; }

        private Filter _filter;
        private Stash<CharacterGroundedComponent> _groundedStash;
        private Stash<CharacterControllerComponent> _characterControllerStash;
        private Stash<RigidbodyComponent> _rigidbodyStash;
        private Stash<CharacterControllerHitHolderComponent> _characterHitStash;

        public void OnAwake()
        {
            _filter = World.Filter
                .With<CharacterGroundedComponent>()
                .With<CharacterControllerComponent>()
                .Build();

            _groundedStash = World.GetStash<CharacterGroundedComponent>();
            _characterControllerStash = World.GetStash<CharacterControllerComponent>();
            _rigidbodyStash = World.GetStash<RigidbodyComponent>();
            _characterHitStash = World.GetStash<CharacterControllerHitHolderComponent>();
        }

        public void OnUpdate(float deltaTime)
        {
            foreach (var entity in _filter)
            {
                ref var groundedComponent = ref _groundedStash.Get(entity);
                ref var characterComponent = ref _characterControllerStash.Get(entity);

                if (characterComponent.CharacterController.isGrounded)
                {
                    ref var characterHitComponent = ref _characterHitStash.Get(entity);
                    
                    if (EntityProvider.map.TryGetValue(characterHitComponent.HitHolder.Hit.gameObject.GetInstanceID(), 
                            out var otherEntityItem))
                    {
                        ref var rigidbodyComponent = ref _rigidbodyStash.Get(otherEntityItem.entity);
                        
                        groundedComponent.IsGrounded = rigidbodyComponent.Rigidbody.isKinematic ||
                                                       (!rigidbodyComponent.Rigidbody.isKinematic &&
                                                        rigidbodyComponent.Rigidbody.linearVelocity.sqrMagnitude <
                                                        groundedComponent.MaxStandingRigidbodyVelocity);
                    }
                    else
                        groundedComponent.IsGrounded = true;
                }
                else
                    groundedComponent.IsGrounded = false;
            }
        }

        public void Dispose()
        {
        }
    }
}