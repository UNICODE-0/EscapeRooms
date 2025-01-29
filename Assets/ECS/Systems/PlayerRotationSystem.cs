using Scellecs.Morpeh;
using UnityEngine;
using Unity.IL2CPP.CompilerServices;

[Il2CppSetOption(Option.NullChecks, false)]
[Il2CppSetOption(Option.ArrayBoundsChecks, false)]
[Il2CppSetOption(Option.DivideByZeroChecks, false)]
public sealed class PlayerRotationSystem : ISystem 
{
    public World World { get; set; }
    
    private Filter _filter;
    private Stash<TransformComponent> _transformStash;
    private Stash<InputComponent> _inputStash;
    private Stash<PlayerRotationComponent> _playerRotationStash;

    private float _yRotation = 0;

    public void OnAwake()
    {
        _filter = World.Filter
            .With<TransformComponent>()
            .With<InputComponent>()
            .With<PlayerRotationComponent>() 
            .Build();
        
        _transformStash = World.GetStash<TransformComponent>();
        _inputStash = World.GetStash<InputComponent>();
        _playerRotationStash = World.GetStash<PlayerRotationComponent>();
    }

    public void OnUpdate(float deltaTime)
    {
        foreach (var entity in _filter) 
        {
            ref var transformComponent = ref _transformStash.Get(entity);
            ref var inputComponent = ref _inputStash.Get(entity);
            ref var playerRotationComponent = ref _playerRotationStash.Get(entity);

            Vector2 mouseDelta = inputComponent.LookAction.ReadValue<Vector2>();
            float mouseX = mouseDelta.x * playerRotationComponent.RotationSensitivity;
            _yRotation += mouseX;
            
            transformComponent.Transform.rotation = Quaternion.Euler(0, _yRotation, 0f);
        }
    }
    
    public void Dispose()
    {
        _yRotation = 0;
    }
}