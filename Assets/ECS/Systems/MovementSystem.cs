using Scellecs.Morpeh;
using UnityEngine;
using Unity.IL2CPP.CompilerServices;

[Il2CppSetOption(Option.NullChecks, false)]
[Il2CppSetOption(Option.ArrayBoundsChecks, false)]
[Il2CppSetOption(Option.DivideByZeroChecks, false)]
public sealed class MovementSystem : ISystem 
{
    public World World { get; set; }
    
    private Filter _filter;
    private Stash<TransformComponent> _transformStash;
    private Stash<CharacterControllerComponent> _characterControllerStash;
    private Stash<InputComponent> _playerInputStash;

    public void OnAwake()
    {
        _filter = World.Filter
            .With<TransformComponent>()
            .With<CharacterControllerComponent>()
            .With<InputComponent>()
            .Build();
        
        _transformStash = World.GetStash<TransformComponent>();
        _characterControllerStash = World.GetStash<CharacterControllerComponent>();
        _playerInputStash = World.GetStash<InputComponent>();
    }

    public void OnUpdate(float deltaTime)
    {
        foreach (var entity in _filter) 
        {
            ref var transformComponent = ref _transformStash.Get(entity);
            ref var characterControllerComponent = ref _characterControllerStash.Get(entity);
            ref var playerInputComponent = ref _playerInputStash.Get(entity);

            Vector2 moveVector = playerInputComponent.MoveAction.ReadValue<Vector2>();
            
            Vector3 moveDirection = (transformComponent.Transform.right * moveVector.x +
                                     transformComponent.Transform.forward * moveVector.y).normalized;
            
            Vector3 fullDirection = moveDirection * (1f * Time.deltaTime);
            
            characterControllerComponent.CharacterController.Move(fullDirection);
        }
    }
    
    public void Dispose()
    {
    }
}