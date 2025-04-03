using Scellecs.Morpeh;
using Scellecs.Morpeh.Collections;
using Scellecs.Morpeh.Providers;
using UnityEngine;

namespace EscapeRooms.Mono
{
    public abstract class ColliderValidatedTriggerEventsHolder : ColliderUniqueTriggerEventsHolder
    {
        protected override void OnTriggerEnterHandler(Collider other)
        {
            if (EntityProvider.map.TryGetValue(other.gameObject.GetInstanceID(), out var otherEntityItem) 
                && !ValidateTriggeredEntity(otherEntityItem.entity))
                return;
            
            base.OnTriggerEnterHandler(other);
        }

        protected abstract bool ValidateTriggeredEntity(Entity entity);
    }
}