using Cysharp.Threading.Tasks;
using EscapeRooms.Components;
using Scellecs.Morpeh;
using UnityEngine;
using Unity.IL2CPP.CompilerServices;
using UnityEngine.ProBuilder;

namespace EscapeRooms.Systems
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    public sealed class CharacterCrouchSystem : ISystem
    {
        public World World { get; set; }

        private Filter _filter;
        private Stash<CharacterCrouchComponent> _crouchStash;
        private Stash<CharacterControllerComponent> _characterStash;

        private bool _isChangeColliderInProgress;
        private bool _isUpperState = true;
        
        public void OnAwake()
        {
            _filter = World.Filter
                .With<CharacterCrouchComponent>()
                .With<CharacterMovementComponent>()
                .Build();

            _crouchStash = World.GetStash<CharacterCrouchComponent>();
            _characterStash = World.GetStash<CharacterControllerComponent>();
        }

        public void OnUpdate(float deltaTime)
        {
            foreach (var entity in _filter)
            {
                ref var crouchComponent = ref _crouchStash.Get(entity);
                ref var characterComponent = ref _characterStash.Get(entity);

                var charCon = characterComponent.CharacterController;
                float scaledSquatSpeed = crouchComponent.SquatSpeed * deltaTime;
                
                if(_isChangeColliderInProgress) continue;
                
                if (crouchComponent.CrouchInput)
                {
                    if (_isUpperState)
                    {
                        _isUpperState = false;
                        ChangeColliderHeight(charCon, crouchComponent.CrouchCapsuleHeight, crouchComponent.CrouchCapsuleCenter, crouchComponent.CrouchAnimationCurve);
                    }
                    else
                    {
                        _isUpperState = true;
                        ChangeColliderHeight(charCon, crouchComponent.StandCapsuleHeight, crouchComponent.StandCapsuleCenter, crouchComponent.StandAnimationCurve);
                    }
                }
            }
        }

        public async void ChangeColliderHeight(CharacterController charCon, float height, Vector3 center, AnimationCurve curve)
        {
            _isChangeColliderInProgress = true;
            
            float duration = 1f;
            float time = 0f;

            float startHeight = charCon.height;
            Vector3 startCenter = charCon.center;
            
            while (time < duration)
            {
                time += Time.deltaTime;

                charCon.height = Mathf.Lerp(startHeight, height,
                    curve.Evaluate(time / duration));

                charCon.center = Vector3.Lerp(startCenter, center,
                    curve.Evaluate(time / duration));

                await UniTask.Yield();
            }

            _isChangeColliderInProgress = false;
        }
        
        public void Dispose()
        {
        }
    }
}