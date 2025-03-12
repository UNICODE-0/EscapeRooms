using System;
using UnityEngine;

public class test : MonoBehaviour
{
    [SerializeField] private ConfigurableJoint _joint;

    private readonly JointDrive _defaultDrive = new JointDrive()
    {
        positionSpring = 2500,
        positionDamper = 40,
        maximumForce = float.MaxValue
    };
    
    private readonly JointDrive _smoothDrive = new JointDrive()
    {
        positionSpring = 300,
        positionDamper = 10,
        maximumForce = float.MaxValue
    };
    
    private readonly JointDrive _angSmoothDrive = new JointDrive()
    {
        positionSpring = 20,
        positionDamper = 10,
        maximumForce = float.MaxValue
    };
    
    private readonly JointDrive _angDefaultDrive = new JointDrive()
    {
        positionSpring = 100,
        positionDamper = 10,
        maximumForce = float.MaxValue
    };

    private void OnTriggerStay(Collider other)
    {
        _joint.xDrive = _smoothDrive;
        _joint.yDrive = _smoothDrive;
        _joint.zDrive = _smoothDrive;
        
        _joint.angularXDrive = _angSmoothDrive;
        _joint.angularYZDrive = _angSmoothDrive;
    }
    
    private void OnTriggerExit(Collider other)
    {
        _joint.xDrive = _defaultDrive;
        _joint.yDrive = _defaultDrive;
        _joint.zDrive = _defaultDrive;

        _joint.angularXDrive = _angDefaultDrive;
        _joint.angularYZDrive = _angDefaultDrive;
    }
}
