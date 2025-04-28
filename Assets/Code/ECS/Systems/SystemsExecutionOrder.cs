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
            PlayerInputBlock(group);
            LerpBlock(group);
            NodesBlock(group);
            CharacterControllerBlock(group);
            DragBlock(group);
            CameraBlock(group);
            HingeRotateBlock(group);
            JointSlideBlock(group);
            TransformBlock(group);
            ColliderBlock(group);
            RigidbodyBlock(group);

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
            group.AddSystem(new OneHitRaycastSystem());
            group.AddSystem(new OverlapSphereSystem());
            group.AddSystem(new SphereCastSystem());
            group.AddSystem(new OverlapBoxSystem());
        }
        
        private static void LerpBlock(SystemsGroup group)
        {
            group.AddSystem(new FloatLerpSystem());
        }
        
        private static void PlayerInputBlock(SystemsGroup group)
        {
            group.AddSystem(new PlayerMovementInputSystem());
            group.AddSystem(new PlayerJumpInputSystem());
            group.AddSystem(new PlayerFPCameraInputSystem());
            group.AddSystem(new PlayerBodyRotationInputSystem());
            group.AddSystem(new PlayerCrouchInputSystem());
            group.AddSystem(new PlayerThrowInputSystem());
            group.AddSystem(new PlayerDragInputSystem());
            group.AddSystem(new PlayerDragRotationInputSystem());
            group.AddSystem(new PlayerDragRadiusChangeInputSystem());
            group.AddSystem(new PlayerHingeRotateInputSystem());
            group.AddSystem(new PlayerJointSlideInputSystem());
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
            
            group.AddSystem(new CharacterJumpForceApplySystem());
            group.AddSystem(new CharacterGravityAttractionApplySystem());
            group.AddSystem(new CharacterMovementVelocityApplySystem());
            group.AddSystem(new CharacterSlideVelocityApplySystem());
            group.AddSystem(new CharacterHeadbuttForceApplySystem());

            group.AddSystem(new CharacterMotionSystem());
        }
        
        private static void CameraBlock(SystemsGroup group)
        {
            group.AddSystem(new FPCameraBlockWhileDragRotationSystem());
            group.AddSystem(new FPCameraBlockWhileHingeRotationSystem());
            group.AddSystem(new FPCameraBlockWhileJointSlidingSystem());

            group.AddSystem(new FPCameraSystem());
        }
        
        private static void TransformBlock(SystemsGroup group)
        {
            group.AddSystem(new DeltaRotationBlockWhileDragRotationSystem());
            group.AddSystem(new DeltaRotationBlockWhileHingeRotationSystem());
            group.AddSystem(new DeltaRotationBlockWhileJointSlideSystem());

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
            group.AddSystem(new ThrowSystem());
            
            group.AddSystem(new DragInterruptByCollisionSystem());
            group.AddSystem(new DragInterruptByDistanceSystem());
            group.AddSystem(new DragInterruptByThrowSystem());

            group.AddSystem(new DragStartSystem());
            group.AddSystem(new DragStopSystem());
            
            group.AddSystem(new DraggableCollisionSmoothingSystem());
            group.AddSystem(new DragOrbitalPositionSetSystem());
            group.AddSystem(new DragRadiusCorrectionSystem());
            
            group.AddSystem(new DragRotationSystem());
            group.AddSystem(new DragDistanceChangeSystem());
        }

        private static void HingeRotateBlock(SystemsGroup group)
        {
            group.AddSystem(new HingeRotationInterruptByCollisionSystem());
            group.AddSystem(new HingeRotationInterruptByDistanceSystem());

            group.AddSystem(new HingeRotationStartSystem());
            group.AddSystem(new HingeRotationStopSystem());
            
            group.AddSystem(new HingeRotationSystem());
        }
        
        private static void JointSlideBlock(SystemsGroup group)
        {
            group.AddSystem(new JointSlideInterruptByCollisionSystem());
            group.AddSystem(new JointSlideInterruptByDistanceSystem());

            group.AddSystem(new JointSlideStartSystem());
            group.AddSystem(new JointSlideStopSystem());
            
            group.AddSystem(new JointSlideSystem());
        }
        
        private static void RigidbodyBlock(SystemsGroup group)
        {
            group.AddSystem(new RigidbodyForceApplySystem());
        }
        
        private static void NodesBlock(SystemsGroup group)
        {
            group.AddSystem(new NodeInitializeSystem());
            
            group.AddSystem(new QuestParticipantDetectionNodeSystem());
            group.AddSystem(new TransformLerpNodeSystem());
            group.AddSystem(new DragInterruptNodeSystem());
            group.AddSystem(new DraggablePhysicDisableNodeSystem());
            group.AddSystem(new DraggablePhysicEnableNodeSystem());
            group.AddSystem(new EntitySetNodeSystem());
            group.AddSystem(new InteractableInteractionDisableNodeSystem());
            group.AddSystem(new InteractableInteractionEnableNodeSystem());
            group.AddSystem(new TransformParentSetNodeSystem());
            group.AddSystem(new AnimationPlayNodeSystem());

            group.AddSystem(new NodeCompleteSystem());
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