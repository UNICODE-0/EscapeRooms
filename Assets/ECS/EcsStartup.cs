using Scellecs.Morpeh;
using UnityEngine;

public class EcsStartup : MonoBehaviour 
{
    private World _world;
    
    private void Start() 
    {
        _world = World.Default;
        
        var systemsGroup = _world.CreateSystemsGroup();
        AddSystems(systemsGroup);
        _world.AddSystemsGroup(order: 0, systemsGroup);
    }

    private void AddSystems(SystemsGroup group)
    {
        group.AddSystem(new MovementSystem());
    }
}