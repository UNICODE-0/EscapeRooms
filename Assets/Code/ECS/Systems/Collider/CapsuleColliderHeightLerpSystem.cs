using EscapeRooms.Components;
using Scellecs.Morpeh;
using UnityEngine;
using Unity.IL2CPP.CompilerServices;

namespace EscapeRooms.Systems
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    public sealed class CapsuleColliderHeightLerpSystem : ISystem
    {
        public World World { get; set; }

        private Filter _filter;
        private Stash<CapsuleColliderComponent> _capsuleColliderStash;
        private Stash<CapsuleColliderHeightLerpComponent> _capsuleLerpStash;
        private Stash<FloatLerpComponent> _floatLerpStash;

        public void OnAwake()
        {
            _filter = World.Filter
                .With<CapsuleColliderHeightLerpComponent>()
                .With<CapsuleColliderComponent>()
                .Build();

            _capsuleLerpStash = World.GetStash<CapsuleColliderHeightLerpComponent>();
            _floatLerpStash = World.GetStash<FloatLerpComponent>();
            _capsuleColliderStash = World.GetStash<CapsuleColliderComponent>();
        }

        public void OnUpdate(float deltaTime)
        {
            foreach (var entity in _filter)
            {
                ref var capsuleLerpComponent = ref _capsuleLerpStash.Get(entity);
                ref var capsuleColliderComponent = ref _capsuleColliderStash.Get(entity);
                ref var floatLerpComponent = ref _floatLerpStash.Get(capsuleLerpComponent.FloatLerpProvider.Entity);

                if (capsuleLerpComponent.UseFloatProviderInputState && capsuleLerpComponent.ChangeHeightInput)
                {
                    Debug.LogWarning("You can't use ChangeHeightInput because capsule collider height lerp use float lerp input state");
                    capsuleLerpComponent.ChangeHeightInput = false;
                } else if(!capsuleLerpComponent.UseFloatProviderInputState)
                    floatLerpComponent.StartLerpInput = capsuleLerpComponent.ChangeHeightInput;

                if (floatLerpComponent.IsLerpInProgress)
                {
                    ref CapsuleHeightState from = ref capsuleLerpComponent.IsTargetState ? 
                        ref capsuleLerpComponent.TargetState : ref capsuleLerpComponent.DefaultState;
                    
                    ref CapsuleHeightState to = ref capsuleLerpComponent.IsTargetState ?
                        ref capsuleLerpComponent.DefaultState : ref capsuleLerpComponent.TargetState;
                    
                    capsuleColliderComponent.CapsuleCollider.height = 
                        Mathf.Lerp(from.CapsuleHeight, to.CapsuleHeight, floatLerpComponent.CurrentValue);
                    
                    capsuleColliderComponent.CapsuleCollider.center = 
                        Vector3.Lerp(from.CapsuleCenter, to.CapsuleCenter, floatLerpComponent.CurrentValue);
                    
                    if (floatLerpComponent.IsLerpTimeIsUp)
                        capsuleLerpComponent.IsTargetState = !capsuleLerpComponent.IsTargetState;
                }
            }
        }

        public void Dispose()
        {
        }
    }
}