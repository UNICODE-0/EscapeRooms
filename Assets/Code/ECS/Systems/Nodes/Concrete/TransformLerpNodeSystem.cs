using EscapeRooms.Components;
using EscapeRooms.Requests;
using Scellecs.Morpeh;
using Scellecs.Morpeh.Collections;
using Scellecs.Morpeh.Providers;
using Unity.IL2CPP.CompilerServices;

namespace EscapeRooms.Systems
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    public sealed class TransformLerpNodeSystem : ISystem
    {
        public World World { get; set; }

        private Filter _filter;

        private Stash<DraggableDetectionNodeComponent> _nodeStash;
        private Stash<ColliderUniqueTriggerEventsHolderComponent> _colliderTriggerEventsStash;
        private Request<NodeCompleteRequest> _completeRequests;

        private NodeInputHelper<DraggableDetectionNodeOutputDataComponent> _nodeInput;
        
        public void OnAwake()
        {
            _filter = World.Filter
                .With<DraggableDetectionNodeComponent>()
                .With<ColliderUniqueTriggerEventsHolderComponent>()
                .Build();

            _nodeStash = World.GetStash<DraggableDetectionNodeComponent>();
            _colliderTriggerEventsStash = World.GetStash<ColliderUniqueTriggerEventsHolderComponent>();
            _completeRequests = World.GetRequest<NodeCompleteRequest>();

            _nodeInput = new();
            _nodeInput.Initialize(World);
        }

        public void OnUpdate(float deltaTime)
        {
            foreach (var entity in _filter)
            {
                ref var eventsHolderComponent = ref _colliderTriggerEventsStash.Get(entity);
                ref var nodeComponent = ref _nodeStash.Get(entity);
                
                // _completeRequests.Publish(new NodeCompleteRequest()
                // {
                //     CurrentNodeEntity = entity,
                //     NextNodeProvider = nodeComponent.NextNodeProvider,
                // });
            }
        }

        public void Dispose()
        { 
        }
    }
}