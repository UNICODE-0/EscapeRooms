using EscapeRooms.Data;
using Sirenix.OdinInspector;
using UnityEngine;

namespace EscapeRooms.Mono
{
    [RequireComponent(typeof(Collider))]
    public class ColliderCollisionEventsHolder : MonoBehaviour
    {
        [ReadOnly] [InlineProperty]
        public FrameUniqueBool IsAnyCollisionInProgress;

        private void OnCollisionEnter(Collision other)
        {
            OnCollisionEnterHandler(other);
        }
        
        private void OnCollisionStay(Collision other)
        {
            OnCollisionStayHandler(other);
        }
        
        private void OnCollisionExit(Collision other)
        {
            OnCollisionExitHandler(other);
        }
        
        // =======================================
        
        protected virtual void OnCollisionEnterHandler(Collision other)
        {
            IsAnyCollisionInProgress.SetTrue();
        }
        
        protected virtual void OnCollisionStayHandler(Collision other)
        {
            IsAnyCollisionInProgress.SetTrue();
        }
        
        protected virtual void OnCollisionExitHandler(Collision other)
        {
            IsAnyCollisionInProgress.SetFalse();
        }
    }
}