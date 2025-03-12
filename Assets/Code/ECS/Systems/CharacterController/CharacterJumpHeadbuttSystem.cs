using EscapeRooms.Components;
using Scellecs.Morpeh;
using Unity.IL2CPP.CompilerServices;

namespace EscapeRooms.Systems
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    public sealed class CharacterJumpHeadbuttSystem : ISystem
    {
        public World World { get; set; }

        private Filter _filter;
        private Stash<CharacterJumpComponent> _jumpStash;
        private Stash<CharacterGroundedComponent> _groundedStash;
        private Stash<CharacterHeadbuttComponent> _headbuttStash;
        
        private Stash<OverlapSphereComponent> _overlapSphereStash;
        
        public void OnAwake()
        {
            _filter = World.Filter
                .With<CharacterJumpComponent>()
                .With<CharacterGroundedComponent>()
                .With<CharacterHeadbuttComponent>()
                .Build();

            _jumpStash = World.GetStash<CharacterJumpComponent>();
            _groundedStash = World.GetStash<CharacterGroundedComponent>();
            _headbuttStash = World.GetStash<CharacterHeadbuttComponent>();
            
            _overlapSphereStash = World.GetStash<OverlapSphereComponent>();
        }

        public void OnUpdate(float deltaTime)
        {
            foreach (var entity in _filter)
            {
                ref var jumpComponent = ref _jumpStash.Get(entity);
                ref var groundedComponent = ref _groundedStash.Get(entity);
                ref var headbuttComponent = ref _headbuttStash.Get(entity);
                
                if (groundedComponent.IsGrounded)
                {
                    headbuttComponent.IsHeadbuttForceApplied = false;
                    headbuttComponent.CurrentForce.y = 0f;
                    continue;
                }
                    
                ref var headOverlapSphereComponent = ref _overlapSphereStash.Get(headbuttComponent.HeadOverlapCheckProvider.Entity);
                if (jumpComponent.IsJumpForceApplied && !headbuttComponent.IsHeadbuttForceApplied && 
                    headOverlapSphereComponent.IsSphereIntersect)
                {
                    headbuttComponent.CurrentForce.y = -jumpComponent.CurrentForce.y * headbuttComponent.ReboundForcePercentage;
                    jumpComponent.CurrentForce.y = 0f;
                    headbuttComponent.IsHeadbuttForceApplied = true;
                }
            }
        }

        public void Dispose()
        {
        }
    }
}