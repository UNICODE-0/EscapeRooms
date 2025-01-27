using Scellecs.Morpeh;
using UnityEngine;

public class Startup : MonoBehaviour 
{
    private World _world;
    
    private void Start() 
    {
        _world = World.Default;
        
        var systemsGroup = _world.CreateSystemsGroup();
        _world.AddSystemsGroup(order: 0, systemsGroup);
    }
}