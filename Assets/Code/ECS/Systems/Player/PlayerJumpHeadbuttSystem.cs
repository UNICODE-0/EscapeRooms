using System.Collections.Generic;
using EscapeRooms.Components;
using Scellecs.Morpeh;
using UnityEngine;
using Unity.IL2CPP.CompilerServices;
using UnityEngine.InputSystem;

namespace EscapeRooms.Systems
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    public sealed class PlayerJumpHeadbuttSystem : ISystem
    {
        public World World { get; set; }

        private Filter _filter;
        private Stash<JumpComponent> _jumpStash;
        private Stash<InputComponent> _inputStash;
        private Stash<GravityComponent> _gravityStash;
        private Stash<GroundedComponent> _groundedStash;
        private Stash<HeadbuttComponent> _headbuttStash;
        
        private Stash<OverlapSphereComponent> _overlapSphereStash;

        public void OnAwake()
        {
            _filter = World.Filter
                .With<JumpComponent>()
                .With<GravityComponent>()
                .With<InputComponent>()
                .With<GroundedComponent>()
                .With<PlayerComponent>()
                .Build();

            _jumpStash = World.GetStash<JumpComponent>();
            _inputStash = World.GetStash<InputComponent>();
            _gravityStash = World.GetStash<GravityComponent>();
            _groundedStash = World.GetStash<GroundedComponent>();
            _headbuttStash = World.GetStash<HeadbuttComponent>();
            _overlapSphereStash = World.GetStash<OverlapSphereComponent>();
        }

        public void OnUpdate(float deltaTime)
        {
            foreach (var entity in _filter)
            {
                ref var jumpComponent = ref _jumpStash.Get(entity);
                ref var inputComponent = ref _inputStash.Get(entity);
                ref var gravityComponent = ref _gravityStash.Get(entity);
                ref var groundedComponent = ref _groundedStash.Get(entity);
                ref var headbuttComponent = ref _headbuttStash.Get(entity);

                ref var headOverlapSphereComponent = ref _overlapSphereStash.Get(headbuttComponent.HeadOverlapCheckEntity.Entity);
                Debug.LogError(headOverlapSphereComponent.IsSphereIntersect);
            }
        }

        public void Dispose()
        {
        }
    }
}