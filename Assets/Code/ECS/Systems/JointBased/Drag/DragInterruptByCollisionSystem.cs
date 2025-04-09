using EscapeRooms.Components;
using Scellecs.Morpeh;
using Unity.IL2CPP.CompilerServices;

namespace EscapeRooms.Systems
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    public sealed class DragInterruptByCollisionSystem : ISystem
    {
        public World World { get; set; }

        private Filter _filter;
        private Stash<DragComponent> _dragStash;
        private Stash<InteractInterruptFlag> _flagStash;
        
        public void OnAwake()
        {
            _filter = World.Filter
                .With<DragComponent>()
                .Build();

            _dragStash = World.GetStash<DragComponent>();
            _flagStash = World.GetStash<InteractInterruptFlag>();
        }

        public void OnUpdate(float deltaTime)
        {
            foreach (var entity in _filter)
            {
                ref var dragComponent = ref _dragStash.Get(entity);

                if(dragComponent.IsDragging && _flagStash.Has(dragComponent.DraggableEntity))
                {
                    dragComponent.DragStopInput = true;
                }
            }
        }
        
        public void Dispose()
        {
        }
    }
}