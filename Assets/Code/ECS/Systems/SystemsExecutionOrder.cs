using Scellecs.Morpeh;

namespace EscapeRooms.Systems
{
    public static class SystemsExecutionOrder
    {
        public static void AddSystemsSequence(SystemsGroup group)
        {
            InputRequestBlock(group);
            InputReadBlock(group);
            PhysicBlock(group);
            LerpBlock(group);
            PlayerBlock(group);
            CharacterControllerBlock(group);
            TransformBlock(group);
        }

        private static void InputRequestBlock(SystemsGroup group)
        {
            group.AddSystem(new PlayerJumpInputInterruptSystem());
        }
        
        private static void InputReadBlock(SystemsGroup group)
        {
            group.AddSystem(new InputSystem());
        }
        
        private static void PhysicBlock(SystemsGroup group)
        {
            group.AddSystem(new RaycastSystem());
            group.AddSystem(new OverlapSphereSystem());
        }
        
        private static void LerpBlock(SystemsGroup group)
        {
            group.AddSystem(new FloatLerpSystem());
        }
        
        private static void PlayerBlock(SystemsGroup group)
        {
            group.AddSystem(new PlayerMovementInputSystem());
            group.AddSystem(new PlayerJumpInputSystem());
            group.AddSystem(new PlayerFPCameraInputSystem());
            group.AddSystem(new PlayerBodyRotationInputSystem());
            group.AddSystem(new PlayerCrouchInputSystem());
        }
        
        private static void CharacterControllerBlock(SystemsGroup group)
        {
            group.AddSystem(new CharacterGravitySystem());
            group.AddSystem(new CharacterGroundedCheckSystem());
            group.AddSystem(new CharacterMovementSystem());
            group.AddSystem(new CharacterLedgeCorrectionSystem());
            group.AddSystem(new CharacterSlideSystem());
            group.AddSystem(new CharacterJumpSystem());
            group.AddSystem(new CharacterJumpHeadbuttSystem());
            group.AddSystem(new CharacterCrouchSystem());
            group.AddSystem(new CharacterCrouchSlowdownSystem());
            group.AddSystem(new FPCameraSystem());
            
            group.AddSystem(new CharacterJumpForceApplySystem());
            group.AddSystem(new CharacterGravityAttractionApplySystem());
            group.AddSystem(new CharacterMovementVelocityApplySystem());
            group.AddSystem(new CharacterSlideVelocityApplySystem());
            group.AddSystem(new CharacterHeadbuttForceApplySystem());

            group.AddSystem(new CharacterMotionSystem());
        }
        
        private static void TransformBlock(SystemsGroup group)
        {
            group.AddSystem(new TransformDeltaRotationSystem());
            group.AddSystem(new TransformPositionLerpSystem());
        }
    }
}