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
            #if UNITY_EDITOR || DEBUG
            AddDebugSystems(systemsGroup);
            #endif

            _world.AddSystemsGroup(order: 0, systemsGroup);

        }

        private void AddInitializers(SystemsGroup group)
        {
            group.AddInitializer(new InputInitializer());
            group.AddInitializer(new SettingsInitializer());
        }

        private void AddSystems(SystemsGroup group)
        {
            group.AddSystem(new RaycastSystem());
            group.AddSystem(new OverlapSphereSystem());

            #region Player
            group.AddSystem(new PlayerMovementInputSystem());
            group.AddSystem(new PlayerJumpInputSystem());
            group.AddSystem(new PlayerFPCameraInputSystem());
            group.AddSystem(new PlayerBodyRotationInputSystem());
            group.AddSystem(new PlayerCrouchInputSystem());
            #endregion
            
            #region CharacterController
            group.AddSystem(new CharacterGravitySystem());
            group.AddSystem(new CharacterGroundedCheckSystem());
            group.AddSystem(new CharacterMovementSystem());
            group.AddSystem(new CharacterLedgeCorrectionSystem());
            group.AddSystem(new CharacterSlideSystem());
            group.AddSystem(new CharacterJumpSystem());
            group.AddSystem(new CharacterJumpHeadbuttSystem());
            group.AddSystem(new CharacterCrouchSystem());
            group.AddSystem(new FPCameraSystem());
            
            group.AddSystem(new CharacterFullMotionApplySystem());
            #endregion
            
            group.AddSystem(new TransformDeltaRotationSystem());
        }

        private void AddLateSystems(SystemsGroup group)
        {

        }
        
        private void AddDebugSystems(SystemsGroup group)
        {
            group.AddSystem(new FrameRateChangeSystem());
        }
    }
}