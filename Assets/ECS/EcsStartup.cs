using Scellecs.Morpeh;
using UnityEngine;

public class EcsStartup : MonoBehaviour 
{
    private World _world;
    
    private void Start() 
    {
        _world = World.Default;
        
        var systemsGroup = _world.CreateSystemsGroup();
        
        AddInitializers(systemsGroup);
        AddSystems(systemsGroup);
        
        _world.AddSystemsGroup(order: 0, systemsGroup);
    }
    
    private void AddInitializers(SystemsGroup group)
    {
        group.AddInitializer(new InputInitializer());
    }
    
    private void AddSystems(SystemsGroup group)
    {
        group.AddSystem(new PlayerMovementSystem());
        group.AddSystem(new PlayerCameraSystem());
        group.AddSystem(new PlayerRotationSystem());
    }
}