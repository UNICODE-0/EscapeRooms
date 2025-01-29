using Scellecs.Morpeh;
using UnityEngine;
using Unity.IL2CPP.CompilerServices;
using UnityEngine.InputSystem;

[Il2CppSetOption(Option.NullChecks, false)]
[Il2CppSetOption(Option.ArrayBoundsChecks, false)]
[Il2CppSetOption(Option.DivideByZeroChecks, false)]
public sealed class InputInitializer : IInitializer 
{
    public World World { get; set; }
    
    private Filter _filter;
    private Stash<InputComponent> _playerInputStash;
    
    public void OnAwake()
    {
        _filter = World.Filter
            .With<InputComponent>()
            .Build();
        
        _playerInputStash = World.GetStash<InputComponent>().AsDisposable();

        SetInputActions();
    }

    public void SetInputActions()
    {
        foreach (var entity in _filter) 
        {
            ref var playerInputComponent = ref _playerInputStash.Get(entity);
            
            playerInputComponent.MoveAction = InputSystem.actions.FindAction("Move");
        }
    }
    
    public void Dispose()
    {
    }
}