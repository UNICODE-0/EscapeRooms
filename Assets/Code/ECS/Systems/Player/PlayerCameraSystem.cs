using EscapeRooms.Components;
using Scellecs.Morpeh;
using UnityEngine;
using Unity.IL2CPP.CompilerServices;

namespace EscapeRooms.Systems
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    public sealed class PlayerCameraSystem : ISystem
    {
        public World World { get; set; }

        private Filter _filter;
        private Stash<TransformComponent> _transformStash;
        private Stash<CameraComponent> _cameraStash;
        private Stash<InputComponent> _inputStash;

        public void OnAwake()
        {
            _filter = World.Filter
                .With<TransformComponent>()
                .With<CameraComponent>()
                .With<InputComponent>()
                .With<PlayerCameraComponent>()
                .Build();

            _transformStash = World.GetStash<TransformComponent>();
            _cameraStash = World.GetStash<CameraComponent>();
            _inputStash = World.GetStash<InputComponent>();
        }

        public void OnUpdate(float deltaTime)
        {
            foreach (var entity in _filter)
            {
                ref var transformComponent = ref _transformStash.Get(entity);
                ref var cameraComponent = ref _cameraStash.Get(entity);
                ref var inputComponent = ref _inputStash.Get(entity);

                Vector2 mouseDelta = inputComponent.LookAction.ReadValue<Vector2>();
                float mouseY = mouseDelta.y * cameraComponent.VerticalSensitivity;

                cameraComponent.CurrentXRotation -= mouseY;

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