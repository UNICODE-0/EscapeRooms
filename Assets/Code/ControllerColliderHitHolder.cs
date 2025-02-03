using System;
using UnityEngine;

public class ControllerColliderHitHolder : MonoBehaviour
{
    public ControllerColliderHit Hit { get; private set; } = new();
    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        Hit = hit;
    }
}
