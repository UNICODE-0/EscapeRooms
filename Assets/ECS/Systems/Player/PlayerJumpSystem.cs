using Scellecs.Morpeh;
using UnityEngine;
using Unity.IL2CPP.CompilerServices;

[Il2CppSetOption(Option.NullChecks, false)]
[Il2CppSetOption(Option.ArrayBoundsChecks, false)]
[Il2CppSetOption(Option.DivideByZeroChecks, false)]
public sealed class PlayerJumpSystem : ISystem 
{
    public World World { get; set; }
    
    private Filter _filter;
    private Stash<JumpComponent> _jumpStash;
    private Stash<InputComponent> _inputStash;
    private Stash<GravityComponent> _gravityStash;
    private Stash<CharacterControllerComponent> _characterControllerStash;

    public void OnAwake()
    {
        _filter = World.Filter
            .With<JumpComponent>()
            .With<GravityComponent>()
            .With<InputComponent>()
            .With<CharacterControllerComponent>()
            .With<PlayerComponent>()
            .Build();
        
        _jumpStash = World.GetStash<JumpComponent>();
        _inputStash = World.GetStash<InputComponent>();
        _gravityStash = World.GetStash<GravityComponent>();
        _characterControllerStash = World.GetStash<CharacterControllerComponent>();
    }

    public void OnUpdate(float deltaTime)
    {
        foreach (var entity in _filter) 
        {
            ref var jumpComponent = ref _jumpStash.Get(entity);
            ref var inputComponent = ref _inputStash.Get(entity);
            ref var gravityComponent = ref _gravityStash.Get(entity);
            ref var characterControllerComponent = ref _characterControllerStash.Get(entity);

            float jumpingInputState = inputComponent.JumpAction.ReadValue<float>();

            if (characterControllerComponent.CharacterController.isGrounded)
            {
                if (jumpingInputState > 0f)
                    jumpComponent.CurrentForce.y = 
                        Mathf.Sqrt(jumpComponent.JumpStrength * -gravityComponent.GravitationalAttraction);
                else
                    jumpComponent.CurrentForce.y = 0;
            }
        }
    }
    
    public void Dispose()
    {
    }
}