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
            OnTriggerEnterHandler(other);
        }
        
        private void OnTriggerStay(Collider other)
        {
            OnTriggerStayHandler(other);
        }
        
        private void OnTriggerExit(Collider other)
        {
            OnTriggerExitHandler(other);
        }
        
        // =======================================
        
        protected virtual void OnTriggerEnterHandler(Collider other)
        {
            LastTrigger = ColliderTriggerType.Enter;
            LastTriggeredCollider = other;
        }
        
        protected virtual void OnTriggerStayHandler(Collider other)
        {
            LastTrigger = ColliderTriggerType.Stay;
            LastTriggeredCollider = other;
        }
        
        protected virtual void OnTriggerExitHandler(Collider other)
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