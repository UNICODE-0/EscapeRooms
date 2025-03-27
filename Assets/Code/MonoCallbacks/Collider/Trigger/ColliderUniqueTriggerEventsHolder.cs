using Scellecs.Morpeh.Collections;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Serialization;

namespace EscapeRooms.Mono
{
    [RequireComponent(typeof(Collider))]
    public class ColliderUniqueTriggerEventsHolder : ColliderTriggerEventsHolder
    {
        [ReadOnly] 
        public bool IsAnyUniqueTriggerInProgress => TriggeredGameObjects.length > 0;
        
        [ReadOnly] 
        public IntHashMap<int> TriggeredGameObjects = new IntHashMap<int>();

        private void OnEnable()
        { 
            TriggeredGameObjects.Clear();
        }
        
        protected override void OnTriggerEnterHandler(Collider other)
        {
            int instanceId = other.gameObject.GetInstanceID();

            ref int amountOfTriggers = ref TriggeredGameObjects.TryGetValueRefByKey(instanceId, out bool exist);
            if (exist)
                amountOfTriggers++;
            else
            {
                TriggeredGameObjects.Add(instanceId, 1, out _);
                OnUniqueTriggerEnter(other);
            }
            
            base.OnTriggerEnterHandler(other);
        }

        protected virtual void OnUniqueTriggerEnter(Collider other) { }
        
        protected override void OnTriggerExitHandler(Collider other)
        {
            int instanceId = other.gameObject.GetInstanceID();

            ref int amountOfTriggers = ref TriggeredGameObjects.TryGetValueRefByKey(instanceId, out bool exist);
            
            if(!exist) return;
            
            if (amountOfTriggers > 1)
                amountOfTriggers--;
            else
            {
                TriggeredGameObjects.Remove(instanceId, out _);
                OnUniqueTriggerExit(other);
            }
            
            base.OnTriggerExitHandler(other);

        }
        
        protected virtual void OnUniqueTriggerExit(Collider other) { }
    }
}