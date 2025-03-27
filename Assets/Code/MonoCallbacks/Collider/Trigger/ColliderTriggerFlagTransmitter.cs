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
    public abstract class ColliderTriggerFlagTransmitter<Flag, Receiver> : ColliderUniqueTriggerEventsHolder 
        where Flag: struct, IFlagComponent
        where Receiver: struct, IOwnerProviderComponent
    {
        [Required] [SerializeField] private EntityProvider _owner;
        
        private Stash<Flag> _collisionTriggerStash;
        private Stash<Receiver> _collisionTriggerReceiverStash;

        private void Awake()
        {
            _collisionTriggerStash = World.Default.GetStash<Flag>();
            _collisionTriggerReceiverStash = World.Default.GetStash<Receiver>();
        }

        protected override void OnUniqueTriggerEnter(Collider other)
        {
            TryAddCollisionTriggerComponent(other.gameObject.GetInstanceID());
        }

        protected override void OnUniqueTriggerExit(Collider other)
        {
            TryRemoveCollisionTriggerComponent(other.gameObject.GetInstanceID());
        }

        protected void TryRemoveCollisionTriggerComponent(int otherInstanceId)
        {
            if (EntityProvider.map.TryGetValue(otherInstanceId, out var otherEntityItem))
            {
                ref var collisionTriggerReceiverComponent = 
                    ref _collisionTriggerReceiverStash.Get(otherEntityItem.entity, out bool receiverExist);
                
                if (!receiverExist)
                    return;
                
                ref var collisionTriggerComponent = 
                    ref _collisionTriggerStash.Get(collisionTriggerReceiverComponent.Owner.Entity, out bool triggerExist);

                if (triggerExist)
                {
                    Entity receiverEntity = collisionTriggerReceiverComponent.Owner.Entity;
                    FlagDisposeSystem.ScheduleFlagDispose(ref collisionTriggerComponent, () =>
                    {
                        _collisionTriggerStash.Remove(receiverEntity);
                    });
                }
            }
        }

        private void TryAddCollisionTriggerComponent(int otherInstanceId)
        {
            if (EntityProvider.map.TryGetValue(otherInstanceId, out var otherEntityItem))
            {
                ref var collisionTriggerReceiverComponent = 
                    ref _collisionTriggerReceiverStash.Get(otherEntityItem.entity, out bool receiverExist);
                
                if (!receiverExist)
                    return;
                
                ref var collisionTriggerComponent = 
                    ref _collisionTriggerStash.Add(collisionTriggerReceiverComponent.Owner.Entity, out bool otherCollisionExist);

                if (otherCollisionExist)
                {
                    ref var existedCollisionTriggerComponent = 
                        ref _collisionTriggerStash.Get(collisionTriggerReceiverComponent.Owner.Entity);
                    
                    FlagDisposeSystem.CancelFlagDispose(ref existedCollisionTriggerComponent);
                    return;
                }
                
                collisionTriggerComponent.Owner = _owner.Entity;
            }
        }
    }
}