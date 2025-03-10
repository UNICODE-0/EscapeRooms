using System;
using UnityEngine;

public class test : MonoBehaviour
{
    [SerializeField] private Transform _target;
    [SerializeField] private Rigidbody _rigidbody;
    [SerializeField] private ConfigurableJoint _joint;
    [SerializeField] private float _dist;
    [SerializeField] private float _speed;
    [SerializeField] private float _impulse;
    

    private bool _isColliding = false;

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
        _isColliding = true;
        _joint.xDrive = _smoothDrive;
        _joint.yDrive = _smoothDrive;
        _joint.zDrive = _smoothDrive;
        
        _joint.angularXDrive = _angSmoothDrive;
        _joint.angularYZDrive = _angSmoothDrive;
    }
    
    private void OnTriggerExit(Collider other)
    {
        _isColliding = false;
        
        _joint.xDrive = _defaultDrive;
        _joint.yDrive = _defaultDrive;
        _joint.zDrive = _defaultDrive;

        _joint.angularXDrive = _angDefaultDrive;
        _joint.angularYZDrive = _angDefaultDrive;
    }

    // private void OnCollisionStay(Collision other)
    // {
    //     Debug.LogError(other.impulse.x + " | " + other.impulse.y + " | " + other.impulse.z + " | ");
    //     if (other.impulse.x >= _impulse || other.impulse.y >= _impulse || other.impulse.z >= _impulse)
    //     {
    //         _joint.xDrive = _smoothDrive;
    //         _joint.yDrive = _smoothDrive;
    //         _joint.zDrive = _smoothDrive;
    //     }
    //     else
    //     {
    //         _joint.xDrive = _defaultDrive;
    //         _joint.yDrive = _defaultDrive;
    //         _joint.zDrive = _defaultDrive;
    //     }
    // }
    

    // private void OnCollisionEnter(Collision other)
    // {
    //     _isColliding = true;
    //     _joint.xDrive = _smoothDrive;
    //     _joint.yDrive = _smoothDrive;
    //     _joint.zDrive = _smoothDrive;
    // }

    private void OnCollisionExit(Collision other)
    {
        _isColliding = false;
        
        _joint.xDrive = _defaultDrive;
        _joint.yDrive = _defaultDrive;
        _joint.zDrive = _defaultDrive;
    }

    private void Update()
    {
        
    }
}
