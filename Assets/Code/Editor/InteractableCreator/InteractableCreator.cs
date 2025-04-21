using EscapeRooms.Components;
using EscapeRooms.Mono;
using Sirenix.OdinInspector;
using Sirenix.OdinInspector.Editor;
using UnityEditor;
using UnityEngine;

#if UNITY_EDITOR
namespace EscapeRooms.Editor
{
    public class InteractableCreator : OdinEditorWindow
    {
        private const string DRAGGABLE_CFG_PATH = "InteractableCreator/DraggableConfiguration";
        private const string ROTATABLE_CFG_PATH = "InteractableCreator/RotatableConfiguration";
        private const string SLIDABLE_CFG_PATH = "InteractableCreator/SlidableConfiguration";

        [HorizontalGroup("Target", 50)]
        [PreviewField(100, ObjectFieldAlignment.Left)]
        public GameObject Target;

        [LabelWidth(59)]
        public InteractableType Type;
        
        [MenuItem("Tools/EscapeRooms/InteractableCreator")]
        private static void OpenWindow()
        {
            GetWindow<InteractableCreator>().Show();
        }

        [Button(ButtonSizes.Medium)]
        public void CreateInteractable()
        {
            if (Target is null)
            {
                Debug.LogError("Can't make interactable, target is null");
                return;
            }

            switch (Type)
            {
                case InteractableType.Draggable: 
                    CreateDraggable();
                    break;
                case InteractableType.Rotatable:
                    CreateRotatable();
                    break;
                case InteractableType.Slidable:
                    CreateSlidable();
                    break;
            }
        }

        private void CreateDraggable()
        {
            DraggableConfiguration cfg = Resources.Load<DraggableConfiguration>(DRAGGABLE_CFG_PATH);

            Target.layer = cfg.DraggableLayer;
            
            var rigidbody = Target.AddComponent<Rigidbody>();
            rigidbody.collisionDetectionMode = CollisionDetectionMode.ContinuousSpeculative;
            rigidbody.interpolation = RigidbodyInterpolation.Interpolate;
            
            var joint = Target.AddComponent<ConfigurableJoint>();
            joint.autoConfigureConnectedAnchor = false;
            
            var triggerCollider = Target.AddComponent<BoxCollider>();
            triggerCollider.isTrigger = true;
            triggerCollider.enabled = false;
            triggerCollider.size *= cfg.SmoothingTriggerSizeScale;
            triggerCollider.excludeLayers = cfg.SmoothingExcludeLayerMask;
            
            var transformProvider = Target.AddComponent<TransformProvider>();
            transformProvider.SetData(new ()
            {
                Transform = Target.transform
            });
            
            var triggerHolder = Target.AddComponent<ColliderTriggerEventsHolder>();
            var triggerHolderProvider = Target.AddComponent<ColliderTriggerEventsHolderProvider>();
            triggerHolderProvider.SetData(new ()
            {
                EventsHolder = triggerHolder
            });
            
            var jointProvider = Target.AddComponent<ConfigurableJointProvider>();
            jointProvider.SetData(
                new ()
                {
                    ConfigurableJoint = joint
                });
            
            var rigidbodyProvider = Target.AddComponent<RigidbodyProvider>();
            rigidbodyProvider.SetData(new ()
            {
                Rigidbody = rigidbody
            });
            
            var draggableProvider = Target.AddComponent<DraggableProvider>();
            draggableProvider.SetData(cfg.DraggableComponentSample);
            
            var smoothingProvider = Target.AddComponent<DraggableCollisionSmoothingProvider>();
            smoothingProvider.SetData(new ()
            {
                SmoothingTrigger = triggerCollider,
                SmoothDriveSpring = cfg.DraggableCollisionSmoothingComponentSample.SmoothDriveSpring,
                SmoothDriveDamper = cfg.DraggableCollisionSmoothingComponentSample.SmoothDriveDamper,
                SmoothAngularDriveSpring = cfg.DraggableCollisionSmoothingComponentSample.SmoothAngularDriveSpring,
                SmoothAngularDriveDamper = cfg.DraggableCollisionSmoothingComponentSample.SmoothAngularDriveDamper
            });
            
            var staticCollisionProvider = Target.AddComponent<CharacterStaticCollisionFlagReceiverProvider>();
            staticCollisionProvider.SetData(new ()
            {
                Owner = transformProvider
            });
            
            var interactInterruptProvider = Target.AddComponent<InteractInterruptFlagReceiverProvider>();
            interactInterruptProvider.SetData(new ()
            {
                Owner = transformProvider
            });
        }

        private void CreateRotatable()
        {
            RotatableConfiguration cfg = Resources.Load<RotatableConfiguration>(ROTATABLE_CFG_PATH);
            
            Target.layer = cfg.RotatableLayer;
            
            var rigidbody = Target.AddComponent<Rigidbody>();
            rigidbody.collisionDetectionMode = CollisionDetectionMode.Continuous;
            rigidbody.interpolation = RigidbodyInterpolation.Interpolate;
            rigidbody.constraints = RigidbodyConstraints.FreezePositionX | RigidbodyConstraints.FreezeRotationY |
                                    RigidbodyConstraints.FreezeRotationZ;
            
            var joint = Target.AddComponent<ConfigurableJoint>();
            joint.xMotion = ConfigurableJointMotion.Locked;
            joint.yMotion = ConfigurableJointMotion.Locked;
            joint.zMotion = ConfigurableJointMotion.Locked;
            joint.angularXMotion = ConfigurableJointMotion.Limited;
            joint.angularYMotion = ConfigurableJointMotion.Locked;
            joint.angularZMotion = ConfigurableJointMotion.Locked;
            joint.highAngularXLimit = new SoftJointLimit()
            {
                limit = 0f
            };
            joint.lowAngularXLimit = new SoftJointLimit()
            {
                limit = -152f
            };
            
            var transformProvider = Target.AddComponent<TransformProvider>();
            transformProvider.SetData(new ()
            {
                Transform = Target.transform
            });
            
            var rigidbodyProvider = Target.AddComponent<RigidbodyProvider>();
            rigidbodyProvider.SetData(new ()
            {
                Rigidbody = rigidbody
            });

            var jointProvider = Target.AddComponent<ConfigurableJointProvider>();
            jointProvider.SetData(
                new ()
                {
                    ConfigurableJoint = joint
                });

            var rotatable = Target.AddComponent<HingeRotatableProvider>();
            rotatable.SetData(cfg.RotatableSample);
            
            var staticCollisionProvider = Target.AddComponent<CharacterStaticCollisionFlagReceiverProvider>();
            staticCollisionProvider.SetData(new ()
            {
                Owner = transformProvider
            });
            
            var interactInterruptProvider = Target.AddComponent<InteractInterruptFlagReceiverProvider>();
            interactInterruptProvider.SetData(new ()
            {
                Owner = transformProvider
            });
        }

        public void CreateSlidable()
        {
            SlidableConfiguration cfg = Resources.Load<SlidableConfiguration>(SLIDABLE_CFG_PATH);
            
            Target.layer = cfg.SlidableLayer;
            
            var rigidbody = Target.AddComponent<Rigidbody>();
            rigidbody.collisionDetectionMode = CollisionDetectionMode.Continuous;
            rigidbody.interpolation = RigidbodyInterpolation.Interpolate;
            rigidbody.constraints = RigidbodyConstraints.FreezeRotation;
            
            var joint = Target.AddComponent<ConfigurableJoint>();
            joint.xMotion = ConfigurableJointMotion.Limited;
            joint.yMotion = ConfigurableJointMotion.Locked;
            joint.zMotion = ConfigurableJointMotion.Locked;
            joint.angularXMotion = ConfigurableJointMotion.Locked;
            joint.angularYMotion = ConfigurableJointMotion.Locked;
            joint.angularZMotion = ConfigurableJointMotion.Locked;
            joint.linearLimit = new SoftJointLimit()
            {
                limit = 0.5f
            };
            
            var jointProvider = Target.AddComponent<ConfigurableJointProvider>();
            jointProvider.SetData(
                new ()
                {
                    ConfigurableJoint = joint
                });
            
            var rigidbodyProvider = Target.AddComponent<RigidbodyProvider>();
            rigidbodyProvider.SetData(new ()
            {
                Rigidbody = rigidbody
            });
            
            var transformProvider = Target.AddComponent<TransformProvider>();
            transformProvider.SetData(new ()
            {
                Transform = Target.transform
            });

            GameObject origin = new GameObject()
            {
                name = "Origin",
                transform = { position = Target.transform.position }
            };
            var slidableProvider = Target.AddComponent<JointSlidableProvider>();
            slidableProvider.SetData(new ()
            {
                Spring = cfg.JointSlidableSample.Spring,
                Damper = cfg.JointSlidableSample.Damper,
                MinDistance = cfg.JointSlidableSample.MinDistance,
                MaxDistance = cfg.JointSlidableSample.MaxDistance,
                SlideDirection = cfg.JointSlidableSample.SlideDirection,
                Origin = origin.transform
            });
            
            var staticCollisionProvider = Target.AddComponent<CharacterStaticCollisionFlagReceiverProvider>();
            staticCollisionProvider.SetData(new ()
            {
                Owner = transformProvider
            });
            
            var interactInterruptProvider = Target.AddComponent<InteractInterruptFlagReceiverProvider>();
            interactInterruptProvider.SetData(new ()
            {
                Owner = transformProvider
            });
        }
        
        public enum InteractableType
        {
            Draggable,
            Rotatable,
            Slidable
        }
    }
}
#endif