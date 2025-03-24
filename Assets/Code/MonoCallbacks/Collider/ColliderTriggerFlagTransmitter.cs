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
    public abstract class ColliderTriggerFlagTransmitter<Flag, Receiver, ExitBlocker> : ColliderTriggerEventsHolder 
        where Flag: struct, IFlagComponent
        where Receiver: struct, IOwnerProviderComponent
        where ExitBlocker: struct, ICollisionEndBlockerComponent // Need for handle kinematic state change
    {
        [Required] [SerializeField] private EntityProvider _owner;
        
        private Stash<Flag> _collisionTriggerStash;
        private Stash<Receiver> _collisionTriggerReceiverStash;
        private Stash<ExitBlocker> _exitBlockerStash;

        private void Awake()
        {
            _collisionTriggerStash = World.Default.GetStash<Flag>();
            _collisionTriggerReceiverStash = World.Default.GetStash<Receiver>();
            _exitBlockerStash = World.Default.GetStash<ExitBlocker>();
        }

        protected override void OnTriggerEnterHandler(Collider other)
        {
            base.OnTriggerEnterHandler(other);
            TryAddCollisionTriggerComponent(other.gameObject.GetInstanceID());
        }

        // protected override void OnTriggerStayHandler(Collider other)
        // {
        //     base.OnTriggerStayHandler(other);
        //     TryAddCollisionTriggerComponent(other.gameObject.GetInstanceID());
        // }

        protected override void OnTriggerExitHandler(Collider other)
        {
            base.OnTriggerExitHandler(other);
            TryRemoveCollisionTriggerComponent(other.gameObject.GetInstanceID());
        }

        protected void TryRemoveCollisionTriggerComponent(int otherInstanceId)
        {
            if (EntityProvider.map.TryGetValue(otherInstanceId, out var otherEntityItem))
            {
                ref var blockerComponent = 
                    ref _exitBlockerStash.Get(otherEntityItem.entity, out bool blockerExist);

                if (blockerExist)
                {
                    _exitBlockerStash.Remove(otherEntityItem.entity);
                    return;
                }
                
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
                    return;
                
                collisionTriggerComponent.Owner = _owner.Entity;
            }
        }
    }
}