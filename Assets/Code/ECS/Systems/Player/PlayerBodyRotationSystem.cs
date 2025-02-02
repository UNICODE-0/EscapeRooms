using EscapeRooms.Components;
using Scellecs.Morpeh;
using UnityEngine;
using Unity.IL2CPP.CompilerServices;

namespace EscapeRooms.Systems
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    public sealed class PlayerBodyRotationSystem : ISystem
    {
        public World World { get; set; }

        private Filter _filter;
        private Stash<TransformComponent> _transformStash;
        private Stash<InputComponent> _inputStash;
        private Stash<BodyRotationComponent> _bodyRotationStash;

        public void OnAwake()
        {
            _filter = World.Filter
                .With<TransformComponent>()
                .With<InputComponent>()
                .With<BodyRotationComponent>()
                .With<PlayerComponent>()
                .Build();

            _transformStash = World.GetStash<TransformComponent>();
            _inputStash = World.GetStash<InputComponent>();
            _bodyRotationStash = World.GetStash<BodyRotationComponent>();
        }

        public void OnUpdate(float deltaTime)
        {
            foreach (var entity in _filter)
            {
                ref var transformComponent = ref _transformStash.Get(entity);
                ref var inputComponent = ref _inputStash.Get(entity);
                ref var bodyRotationComponent = ref _bodyRotationStash.Get(entity);

                Vector2 mouseDelta = inputComponent.LookAction.ReadValue<Vector2>();
                float mouseX = mouseDelta.x * bodyRotationComponent.RotationSpeed;
                bodyRotationComponent.CurrentYRotation += mouseX;

                transformComponent.Transform.rotation = Quaternion.Euler(0, bodyRotationComponent.CurrentYRotation, 0f);
            }
        }

        public void Dispose()
        {
        }
    }
}