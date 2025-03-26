using EscapeRooms.Initializers;
using EscapeRooms.Systems;
using Scellecs.Morpeh;
using UnityEngine;

namespace EscapeRooms.Mono
{
    public class EcsStartup : MonoBehaviour
    {
        private World _world;
        
        private void Awake()
        {
            _world = World.Default;

            var systemsGroup = _world.CreateSystemsGroup();

            InitializersExecutionOrder.AddInitializersSequence(systemsGroup);
            SystemsExecutionOrder.AddSystemsSequence(systemsGroup);
            
            #if UNITY_EDITOR || DEBUG
            AddDebugSystems(systemsGroup);
            #endif

            _world.AddSystemsGroup(order: 0, systemsGroup);
        }

        private void AddDebugSystems(SystemsGroup group)
        {
            group.AddSystem(new FrameRateChangeSystem());
        }
    }
}