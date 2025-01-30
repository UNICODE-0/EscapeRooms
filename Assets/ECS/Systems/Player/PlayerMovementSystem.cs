using Scellecs.Morpeh;
using UnityEngine;
using Unity.IL2CPP.CompilerServices;

[Il2CppSetOption(Option.NullChecks, false)]
[Il2CppSetOption(Option.ArrayBoundsChecks, false)]
[Il2CppSetOption(Option.DivideByZeroChecks, false)]
public sealed class PlayerMovementSystem : ISystem 
{
    public World World { get; set; }
    
    private Filter _filter;
    private Stash<TransformComponent> _transformStash;
    private Stash<CharacterControllerComponent> _characterControllerStash;
    private Stash<InputComponent> _inputStash;
    private Stash<MovementComponent> _movementStash;
    private Stash<GravityComponent> _gravityStash;
    private Stash<JumpComponent> _jumpStash;

    public void OnAwake()
    {
        _filter = World.Filter
            .With<TransformComponent>()
            .With<CharacterControllerComponent>()
            .With<InputComponent>()
            .With<MovementComponent>()
            .With<GravityComponent>()
            .With<JumpComponent>()
            .With<PlayerComponent>()
            .Build();
        
        _transformStash = World.GetStash<TransformComponent>();
        _characterControllerStash = World.GetStash<CharacterControllerComponent>();
        _inputStash = World.GetStash<InputComponent>();
        _movementStash = World.GetStash<MovementComponent>();
        _gravityStash = World.GetStash<GravityComponent>();
        _jumpStash = World.GetStash<JumpComponent>();
    }

    public void OnUpdate(float deltaTime)
    {
        foreach (var entity in _filter) 
        {
            ref var transformComponent = ref _transformStash.Get(entity);
            ref var characterControllerComponent = ref _characterControllerStash.Get(entity);
            ref var inputComponent = ref _inputStash.Get(entity);
            ref var movementComponent = ref _movementStash.Get(entity);
            ref var gravityComponent = ref _gravityStash.Get(entity);
            ref var jumpComponent = ref _jumpStash.Get(entity);

            Vector2 moveVector = inputComponent.MoveAction.ReadValue<Vector2>();
            
            Vector3 moveDirection = (transformComponent.Transform.right * moveVector.x +
                                     transformComponent.Transform.forward * moveVector.y).normalized;
            
            Vector3 fullDirection = jumpComponent.CurrentForce + gravityComponent.CurrentForce + 
                                    moveDirection * (movementComponent.Speed * Time.deltaTime);
            
            characterControllerComponent.CharacterController.Move(fullDirection);
        }
    }
    
    public void Dispose()
    {
    }
}