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
            bool exist = EntityProvider.map.TryGetValue(other.gameObject.GetInstanceID(), out var otherEntityItem);

            if (!exist || !ValidateTriggeredEntity(otherEntityItem.entity))
                return;
            
            base.OnTriggerEnterHandler(other);
        }

        protected abstract bool ValidateTriggeredEntity(Entity entity);
    }
}