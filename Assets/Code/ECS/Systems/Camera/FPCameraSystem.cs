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
    public sealed class FPCameraSystem : ISystem
    {
        public World World { get; set; }

        private Filter _filter;
        private Stash<TransformComponent> _transformStash;
        private Stash<FPCameraComponent> _cameraStash;

        public void OnAwake()
        {
            _filter = World.Filter
                .With<TransformComponent>()
                .With<FPCameraComponent>()
                .Build();

            _transformStash = World.GetStash<TransformComponent>();
            _cameraStash = World.GetStash<FPCameraComponent>();
        }

        public void OnUpdate(float deltaTime)
        {
            foreach (var entity in _filter)
            {
                ref var cameraComponent = ref _cameraStash.Get(entity);
                if(cameraComponent.RotationBlockFlag.IsFlagNotClear()) continue;
                
                ref var transformComponent = ref _transformStash.Get(entity);
                
                cameraComponent.CurrentXRotation -= cameraComponent.RotateDelta.x;

                cameraComponent.CurrentXRotation =
                    Mathf.Clamp(cameraComponent.CurrentXRotation, cameraComponent.MinXRotation,
                        cameraComponent.MaxXRotation);

                transformComponent.Transform.localRotation = Quaternion.Euler(cameraComponent.CurrentXRotation, 0f, 0f);
            }
        }

        public void Dispose()
        {
        }
    }
}