using Scellecs.Morpeh;
using UnityEngine;
using Unity.IL2CPP.CompilerServices;

[Il2CppSetOption(Option.NullChecks, false)]
[Il2CppSetOption(Option.ArrayBoundsChecks, false)]
[Il2CppSetOption(Option.DivideByZeroChecks, false)]
public sealed class GravitySystem : ISystem 
{
    public World World { get; set; }
    
    private Filter _filter;
    private Stash<GravityComponent> _gravityStash;
    private Stash<CharacterControllerComponent> _characterControllerStash;
    
    public void OnAwake()
    {
        _filter = World.Filter
            .With<GravityComponent>()
            .With<CharacterControllerComponent>()
            .Build();
        
        _gravityStash = World.GetStash<GravityComponent>();
        _characterControllerStash = World.GetStash<CharacterControllerComponent>(); ;
    }

    public void OnUpdate(float deltaTime)
    {
        foreach (var entity in _filter) 
        {
            ref var gravityComponent = ref _gravityStash.Get(entity);
            ref var characterControllerComponent = ref _characterControllerStash.Get(entity);
            CharacterController charCon = characterControllerComponent.CharacterController;
            if(charCon.isGrounded && gravityComponent.CurrentForce.y < 0) 
                gravityComponent.CurrentForce.y = 0f;

            gravityComponent.CurrentForce.y += gravityComponent.GravitationalAttraction * Time.deltaTime;

            charCon.Move(gravityComponent.CurrentForce);
        }
    }
    
    public void Dispose()
    {
    }
}