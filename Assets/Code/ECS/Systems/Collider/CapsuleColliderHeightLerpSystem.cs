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

        private LerpDataHandler<CapsuleHeightState> _lerpDataHandler;

        public void OnAwake()
        {
            _filter = World.Filter
                .With<CapsuleColliderHeightLerpComponent>()
                .With<CapsuleColliderComponent>()
                .Build();

            _capsuleLerpStash = World.GetStash<CapsuleColliderHeightLerpComponent>();
            _floatLerpStash = World.GetStash<FloatLerpComponent>();
            _capsuleColliderStash = World.GetStash<CapsuleColliderComponent>();

            _lerpDataHandler = new LerpDataHandler<CapsuleHeightState>(_floatLerpStash);
        }

        public void OnUpdate(float deltaTime)
        {
            foreach (var entity in _filter)
            {
                ref var capsuleLerpComponent = ref _capsuleLerpStash.Get(entity);
                ref var capsuleColliderComponent = ref _capsuleColliderStash.Get(entity);

                if (_lerpDataHandler.Handle(ref capsuleLerpComponent.LerpData, 
                        out var from, out var to, out float progress))
                {
                    capsuleColliderComponent.CapsuleCollider.height = 
                        Mathf.Lerp(from.CapsuleHeight, to.CapsuleHeight, progress);
                    
                    capsuleColliderComponent.CapsuleCollider.center = 
                        Vector3.Lerp(from.CapsuleCenter, to.CapsuleCenter, progress);
                }
            }
        }

        public void Dispose()
        {
            _lerpDataHandler = null;
        }
    }
}