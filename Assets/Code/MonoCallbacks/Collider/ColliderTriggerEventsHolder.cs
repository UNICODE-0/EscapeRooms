using EscapeRooms.Data;
using UnityEngine;

namespace EscapeRooms.Mono
{
    [RequireComponent(typeof(Collider))]
    public class ColliderTriggerEventsHolder : MonoBehaviour
    {
        public FrameUniqueBool IsAnyTriggerInProgress;

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
            IsAnyTriggerInProgress.SetTrue();
        }
        
        protected virtual void OnTriggerStayHandler(Collider other)
        {
            IsAnyTriggerInProgress.SetTrue();
        }
        
        protected virtual void OnTriggerExitHandler(Collider other)
        {
            IsAnyTriggerInProgress.SetFalse();
        }
    }
}