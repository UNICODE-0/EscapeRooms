using UnityEngine;

namespace EscapeRooms.Helpers
{
    public static class ConfigurableExtension
    {
        public static void SetJointDriveData(this ConfigurableJoint joint, float driveSpring, float driveDamper
            , float angDriveSpring, float angDriveDamper)
        {
            JointDrive dragDrive = new JointDrive()
            {
                positionSpring = driveSpring,
                positionDamper = driveDamper,
                maximumForce = float.MaxValue
            };
            
            JointDrive dragAngularDrive = new JointDrive()
            {
                positionSpring = angDriveSpring,
                positionDamper = angDriveDamper,
                maximumForce = float.MaxValue
            };

            joint.xDrive = dragDrive;
            joint.yDrive = dragDrive;
            joint.zDrive = dragDrive;
            
            joint.angularXDrive = dragAngularDrive;
            joint.angularYZDrive = dragAngularDrive;
        }
    }
}