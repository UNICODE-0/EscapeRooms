using EscapeRooms.Components;
using Scellecs.Morpeh;
using Scellecs.Morpeh.Providers;
using UnityEngine;

namespace EscapeRooms.Mono
{
    public class OnDragColliderTriggerEventsHolder : ColliderValidatedTriggerEventsHolder
    {
        [SerializeField] private EntityProvider _owner;
        
        private Stash<OnDragFlag> _onDragStash;

        private void Awake()
        {
            _onDragStash = World.Default.GetStash<OnDragFlag>();
        }

        protected override bool ValidateTriggeredEntity(Entity entity)
        {
            ref var onDragFlag = ref _onDragStash.Get(entity, out bool exist);
            if (exist)
                return onDragFlag.Owner == _owner.Entity;
            
            return false;
        }
    }
}