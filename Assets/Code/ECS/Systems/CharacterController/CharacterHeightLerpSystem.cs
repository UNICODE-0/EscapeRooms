using EscapeRooms.Components;
using Scellecs.Morpeh;
using UnityEngine;
using Unity.IL2CPP.CompilerServices;

namespace EscapeRooms.Systems
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    public sealed class CharacterHeightLerpSystem : ISystem
    {
        public World World { get; set; }

        private Filter _filter;
        private Stash<CharacterControllerComponent> _characterStash;
        private Stash<CharacterHeightLerpComponent> _characterLerpStash;
        private Stash<FloatLerpComponent> _floatLerpStash;
        
        private LerpDataHandler<CapsuleHeightState> _lerpDataHandler;

        public void OnAwake()
        {
            _filter = World.Filter
                .With<CharacterHeightLerpComponent>()
                .With<CharacterControllerComponent>()
                .Build();

            _characterLerpStash = World.GetStash<CharacterHeightLerpComponent>();
            _floatLerpStash = World.GetStash<FloatLerpComponent>();
            _characterStash = World.GetStash<CharacterControllerComponent>();
            
            _lerpDataHandler = new LerpDataHandler<CapsuleHeightState>(_floatLerpStash);
        }

        public void OnUpdate(float deltaTime)
        {
            foreach (var entity in _filter)
            {
                ref var characterLerpComponent = ref _characterLerpStash.Get(entity);
                ref var characterComponent = ref _characterStash.Get(entity);
                
                if (_lerpDataHandler.Handle(ref characterLerpComponent.LerpData,
                        out var from, out var to, out float progress))
                {
                    characterComponent.CharacterController.height = 
                        Mathf.Lerp(from.CapsuleHeight, to.CapsuleHeight, progress);
                    
                    characterComponent.CharacterController.center = 
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