using EscapeRooms.Components;
using Scellecs.Morpeh;
using Scellecs.Morpeh.Providers;
using UnityEngine;

namespace EscapeRooms.Mono
{
    public class OnInteractColliderTriggerEventsHolder : ColliderValidatedTriggerEventsHolder
    {
        [SerializeField] private EntityProvider _owner;
        
        private Stash<OnDragFlag> _onDragStash;
        private Stash<OnHingeRotationFlag> _onHingeRotationStash;

        private void Awake()
        {
            _onDragStash = World.Default.GetStash<OnDragFlag>();
            _onHingeRotationStash = World.Default.GetStash<OnHingeRotationFlag>();
        }

        protected override bool ValidateTriggeredEntity(Entity entity)
        {
            ref var onDragFlag = ref _onDragStash.Get(entity, out bool dragExist);
            
            if (dragExist)
                return onDragFlag.Owner == _owner.Entity;
            
            ref var onHingeRotationFlag = ref _onHingeRotationStash.Get(entity, out bool rotationExist);
            
            if (rotationExist)
                return onHingeRotationFlag.Owner == _owner.Entity;
            
            return false;
        }
    }
}