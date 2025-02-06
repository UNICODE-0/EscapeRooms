using Cysharp.Threading.Tasks;
using EscapeRooms.Initializers;
using EscapeRooms.Systems;
using Scellecs.Morpeh;
using UnityEngine;

namespace EscapeRooms.Mono
{
    public class EcsStartup : MonoBehaviour
    {
        private World _world;

        private void Start()
        {
            _world = World.Default;

            var systemsGroup = _world.CreateSystemsGroup();

            AddInitializers(systemsGroup);
            AddSystems(systemsGroup);
            AddLateSystems(systemsGroup);

            _world.AddSystemsGroup(order: 0, systemsGroup);

        }

        private void AddInitializers(SystemsGroup group)
        {
            group.AddInitializer(new InputInitializer());
            group.AddInitializer(new SettingsInitializer());
        }

        private void AddSystems(SystemsGroup group)
        {
            group.AddSystem(new PlayerGravitySystem());
            group.AddSystem(new PlayerGroundedCheckSystem());
            group.AddSystem(new PlayerMovementSystem());
            group.AddSystem(new PlayerLedgeCorrectionSystem());
            group.AddSystem(new PlayerSlideSystem());
            group.AddSystem(new PlayerJumpSystem());
            group.AddSystem(new PlayerCameraSystem());
            group.AddSystem(new PlayerBodyRotationSystem());

            group.AddSystem(new PlayerMotionApplySystem());
        }

        private void AddLateSystems(SystemsGroup group)
        {

        }
    }
}