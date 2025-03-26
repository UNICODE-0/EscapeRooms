using System;
using EscapeRooms.Data;
using Scellecs.Morpeh.Collections;
using Sirenix.OdinInspector;
using UnityEngine;

namespace EscapeRooms.Mono
{
    [RequireComponent(typeof(Collider))]
    public class ColliderTriggerEventsHolder : MonoBehaviour
    {
        [SerializeField] private bool _useUniqueTriggersDetection; 
        
        [ReadOnly] [InlineProperty]   
        public FrameUniqueBool IsAnyTriggerInProgress;

        [ReadOnly] 
        public IntHashMap<int> TriggeredColliders = new IntHashMap<int>();

        private void OnEnable()
        {
            if(_useUniqueTriggersDetection)
                TriggeredColliders.Clear();
        }

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
        
        protected void OnTriggerEnterHandler(Collider other)
        {
            if (_useUniqueTriggersDetection)
            {
                int instanceId = other.gameObject.GetInstanceID();

                ref int amountOfTriggers = ref TriggeredColliders.TryGetValueRefByKey(instanceId, out bool exist);
                if (exist)
                    amountOfTriggers++;
                else
                {
                    TriggeredColliders.Add(instanceId, 1, out _);
                    OnUniqueTriggerEnter(other);
                }
            }

            IsAnyTriggerInProgress.SetTrue();
        }

        protected virtual void OnUniqueTriggerEnter(Collider other)
        {
            
        }
        
        protected void OnTriggerStayHandler(Collider other)
        {
            IsAnyTriggerInProgress.SetTrue();
        }
        
        protected void OnTriggerExitHandler(Collider other)
        {
            if (_useUniqueTriggersDetection)
            {
                int instanceId = other.gameObject.GetInstanceID();

                ref int amountOfTriggers = ref TriggeredColliders.TryGetValueRefByKey(instanceId, out bool exist);
                if (amountOfTriggers > 1)
                    amountOfTriggers--;
                else
                {
                    TriggeredColliders.Remove(instanceId, out _);
                    OnUniqueTriggerExit(other);
                }
            }

            IsAnyTriggerInProgress.SetFalse();
        }
        
        protected virtual void OnUniqueTriggerExit(Collider other)
        {
            
        }
    }
}