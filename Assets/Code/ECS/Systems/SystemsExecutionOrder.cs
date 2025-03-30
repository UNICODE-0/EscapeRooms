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
            DragBlock(group);
            TransformBlock(group);
            ColliderBlock(group);
            
            // Late systems
            
            ComponentEventsBlock(group);
            FrameDataBlock(group);
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
            group.AddSystem(new SphereCastSystem());
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
            group.AddSystem(new PlayerDragInputSystem());
        }
        
        private static void CharacterControllerBlock(SystemsGroup group)
        {
            group.AddSystem(new CharacterGravitySystem());
            group.AddSystem(new CharacterGroundedCheckSystem());
            group.AddSystem(new CharacterMovementSystem());
            group.AddSystem(new CharacterLedgeCorrectionSystem());
            group.AddSystem(new CharacterSlideSystem());

            group.AddSystem(new CharacterStaticCollisionSystem());
            
            group.AddSystem(new CharacterCrouchStandBlockSystem());
            group.AddSystem(new CharacterCrouchStandingBlockSystem());
            group.AddSystem(new CharacterCrouchBlockWhileJumpSystem());
            group.AddSystem(new CharacterCrouchBlockWhileStaticCollisionSystem());
            group.AddSystem(new CharacterCrouchBlockWhileFallingSystem());
            
            group.AddSystem(new CharacterCrouchSystem());
            group.AddSystem(new CharacterCrouchSlowdownSystem());
            
            group.AddSystem(new CharacterJumpBlockWhileCrouchSystem());
            group.AddSystem(new CharacterJumpBlockWhileStaticCollisionSystem());

            group.AddSystem(new CharacterJumpSystem());
            group.AddSystem(new CharacterJumpHeadbuttSystem());
            
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
            group.AddSystem(new TransformOrbitalFollowSystem());
        }
        
        private static void ColliderBlock(SystemsGroup group)
        {
            group.AddSystem(new CapsuleColliderHeightLerpSystem());
            group.AddSystem(new CharacterHeightLerpSystem());
        }
        
        private static void DragBlock(SystemsGroup group)
        {
            group.AddSystem(new DragInterruptByCollisionSystem());
            group.AddSystem(new DragInterruptByDistanceSystem());

            group.AddSystem(new DragStartSystem());
            group.AddSystem(new DragStopSystem());
            
            group.AddSystem(new DraggableCollisionSmoothingSystem());
            group.AddSystem(new DragOrbitalPositionSetSystem());
            group.AddSystem(new DragRadiusCorrectionSystem());
        }
        
        // Late systems
        
        private static void ComponentEventsBlock(SystemsGroup group)
        {
            group.AddSystem(new FlagDisposeSystem());
        }
        private static void FrameDataBlock(SystemsGroup group)
        {
            group.AddSystem(new FrameDataSystem());
        }
    }
}