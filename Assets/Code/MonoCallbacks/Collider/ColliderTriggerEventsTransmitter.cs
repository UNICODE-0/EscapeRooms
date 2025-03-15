using EscapeRooms.Components;
using EscapeRooms.Systems;
using Scellecs.Morpeh;
using UnityEngine;
using Scellecs.Morpeh.Collections;
using Scellecs.Morpeh.Providers;
using Sirenix.OdinInspector;

namespace EscapeRooms.Mono
{
    // Logic of this class executed BEFORE update loop in ECS world
    public abstract class ColliderTriggerEventsTransmitter<T> : ColliderTriggerEventsHolder 
        where T: struct, IEventComponent
    {
        [Required] [SerializeField] private EntityProvider _owner;
        
        private Stash<T> _collisionTriggerStash;

        private void Awake()
        {
            _collisionTriggerStash = World.Default.GetStash<T>();
        }

        protected override void OnTriggerEnterHandler(Collider other)
        {
            base.OnTriggerEnterHandler(other);
            TryAddCollisionTriggerComponent(other.gameObject.GetInstanceID());
        }

        protected override void OnTriggerStayHandler(Collider other)
        {
            base.OnTriggerStayHandler(other);
            TryAddCollisionTriggerComponent(other.gameObject.GetInstanceID());
        }

        protected override void OnTriggerExitHandler(Collider other)
        {
            base.OnTriggerExitHandler(other);
            TryRemoveCollisionTriggerComponent(other.gameObject.GetInstanceID());
        }

        protected void TryRemoveCollisionTriggerComponent(int otherInstanceId)
        {
            if (EntityProvider.map.TryGetValue(otherInstanceId, out var otherEntityItem))
            {
                ref var triggerComponent = 
                    ref _collisionTriggerStash.Get(otherEntityItem.entity, out bool triggerExist);

                if (triggerExist)
                {
                    triggerComponent.IsLastFrameOfLife = true;
                    EventComponentDisposeSystem.EventsToDispose.Add(triggerComponent);
                }
            }
        }

        private void TryAddCollisionTriggerComponent(int otherInstanceId)
        {
            if (EntityProvider.map.TryGetValue(otherInstanceId, out var otherEntityItem))
            {
                ref var collisionTriggerComponent = 
                    ref _collisionTriggerStash.Add(otherEntityItem.entity, out bool otherCollisionExist);

                if (otherCollisionExist)
                    return;
                
                collisionTriggerComponent.Owner = _owner.Entity;
            }
        }
    }
}