using EscapeRooms.Components;
using Scellecs.Morpeh;

namespace EscapeRooms.Mono
{
    public class DraggableTriggerEventsHolder : ColliderValidatedTriggerEventsHolder
    {
        private Stash<DraggableComponent> _draggableStash;

        private void Awake()
        {
            _draggableStash = World.Default.GetStash<DraggableComponent>();
        }

        protected override bool ValidateTriggeredEntity(Entity entity)
        {
            return _draggableStash.Has(entity);
        }
    }
}