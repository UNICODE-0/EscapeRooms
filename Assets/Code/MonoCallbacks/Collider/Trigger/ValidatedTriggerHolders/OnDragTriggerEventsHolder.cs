using EscapeRooms.Components;
using Scellecs.Morpeh;

namespace EscapeRooms.Mono
{
    public class OnDragTriggerEventsHolder : ColliderValidatedTriggerEventsHolder
    {
        private Stash<OnDragFlag> _onDragStash;

        private void Awake()
        {
            _onDragStash = World.Default.GetStash<OnDragFlag>();
        }

        protected override bool ValidateTriggeredEntity(Entity entity)
        {
            ref var onDragFlag = ref _onDragStash.Get(entity, out bool dragExist);
            return dragExist;
        }
    }
}