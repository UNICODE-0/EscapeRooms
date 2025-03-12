using UnityEngine;

namespace EscapeRooms.Mono
{
    [RequireComponent(typeof(Collider))]
    public class ColliderTriggerEventsHolder : MonoBehaviour
    {
        public ColliderTriggerType LastTrigger { get; private set; } = ColliderTriggerType.None;
        public Collider LastTriggeredCollider { get; private set; }

        private void OnTriggerEnter(Collider other)
        {
            LastTrigger = ColliderTriggerType.Enter;
            LastTriggeredCollider = other;
        }

        private void OnTriggerStay(Collider other)
        {
            LastTrigger = ColliderTriggerType.Stay;
            LastTriggeredCollider = other;
        }

        private void OnTriggerExit(Collider other)
        {
            LastTrigger = ColliderTriggerType.Exit;
            LastTriggeredCollider = other;
        }
    }
    
    public enum ColliderTriggerType
    {
        None,
        Enter,
        Stay,
        Exit
    }
}