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
    private Stash<TransformComponent> _transformComponentStash;
    private Stash<CharacterControllerComponent> _characterControllerComponentStash;

    public void OnAwake()
    {
        _filter = World.Filter
            .With<TransformComponent>()
            .With<CharacterControllerComponent>()
            .Build();
        
        _transformComponentStash = World.GetStash<TransformComponent>();
        _characterControllerComponentStash = World.GetStash<CharacterControllerComponent>();
    }

    public void OnUpdate(float deltaTime)
    {
        foreach (var entity in _filter) 
        {
            ref var transformComponent = ref _transformComponentStash.Get(entity);
            ref var characterControllerComponent = ref _characterControllerComponentStash.Get(entity);

            if (Input.GetKey(KeyCode.A))
            {
                characterControllerComponent.CharacterController.Move(-transformComponent.Transform.right * Time.deltaTime);
            }
        }
    }
    
    public void Dispose()
    {
    }
}