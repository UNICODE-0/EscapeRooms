using Scellecs.Morpeh;
using UnityEngine;
using Unity.IL2CPP.CompilerServices;

[Il2CppSetOption(Option.NullChecks, false)]
[Il2CppSetOption(Option.ArrayBoundsChecks, false)]
[Il2CppSetOption(Option.DivideByZeroChecks, false)]
public sealed class PlayerCameraSystem : ISystem 
{
    public World World { get; set; }
    
    private Filter _filter;
    private Stash<TransformComponent> _transformStash;
    private Stash<PlayerCameraComponent> _playerCameraStash;
    private Stash<InputComponent> _inputStash;
    
    private float _xRotation = 0;

    public void OnAwake()
    {
        _filter = World.Filter
            .With<TransformComponent>()
            .With<PlayerCameraComponent>()
            .With<InputComponent>()
            .Build();
        
        _transformStash = World.GetStash<TransformComponent>();
        _playerCameraStash = World.GetStash<PlayerCameraComponent>();
        _inputStash = World.GetStash<InputComponent>();
    }

    public void OnUpdate(float deltaTime)
    {
        foreach (var entity in _filter) 
        {
            ref var transformComponent = ref _transformStash.Get(entity);
            ref var cameraComponent = ref _playerCameraStash.Get(entity);
            ref var inputComponent = ref _inputStash.Get(entity);

            Vector2 mouseDelta = inputComponent.LookAction.ReadValue<Vector2>();
            float mouseY = mouseDelta.y * cameraComponent.VerticalSensitivity;
                
            _xRotation -= mouseY;
            _xRotation = Mathf.Clamp(_xRotation, cameraComponent.MinXRotation, cameraComponent.MaxXRotation);
            
            transformComponent.Transform.localRotation = Quaternion.Euler(_xRotation, 0f, 0f);
        }
    }
    
    public void Dispose()
    {
        _xRotation = 0;
    }
}