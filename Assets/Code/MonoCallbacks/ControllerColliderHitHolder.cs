using UnityEngine;

namespace EscapeRooms.Mono
{
    public class ControllerColliderHitHolder : MonoBehaviour
    {
        public ControllerColliderHit Hit { get; private set; } = new();
        public float HitYAngle { get; private set; }

        private void OnControllerColliderHit(ControllerColliderHit hit)
        {
            Hit = hit;
            HitYAngle = Vector3.Angle(Vector3.up, hit.normal);
        }
    }
}