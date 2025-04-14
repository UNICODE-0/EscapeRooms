using EscapeRooms.Components;
using EscapeRooms.Helpers;
using EscapeRooms.Requests;
using Scellecs.Morpeh;
using UnityEngine;
using Unity.IL2CPP.CompilerServices;

namespace EscapeRooms.Systems
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    public sealed class DraggableDetectionNodeSystem : ISystem
    {
        public World World { get; set; }

        private Filter _filter;

        private Stash<DraggableDetectionNodeComponent> _nodeStash;
        private Stash<NodeInitializeFlag> _nodeInitFlagStash;
        private Stash<NodeInputDataComponent> _nodeInputStash;
        private Stash<ColliderTriggerEventsHolderComponent> _colliderTriggerEventsStash;

        private Request<NodeCompleteRequest> _completeRequests;
        
        public void OnAwake()
        {
            _filter = World.Filter
                .With<DraggableDetectionNodeComponent>()
                .With<ColliderTriggerEventsHolderComponent>()
                .With<NodeInputDataComponent>()
                .Build();

            _nodeStash = World.GetStash<DraggableDetectionNodeComponent>();
            _colliderTriggerEventsStash = World.GetStash<ColliderTriggerEventsHolderComponent>();
            _nodeInitFlagStash = World.GetStash<NodeInitializeFlag>();
            _nodeInputStash = World.GetStash<NodeInputDataComponent>();
                
            _completeRequests = World.GetRequest<NodeCompleteRequest>();
        }

        public void OnUpdate(float deltaTime)
        {
            foreach (var entity in _filter)
            {
                ref var eventsHolderComponent = ref _colliderTriggerEventsStash.Get(entity);
                ref var nodeComponent = ref _nodeStash.Get(entity);
                ref var nodeInput = ref _nodeInputStash.Get(entity);

                if (_nodeInitFlagStash.Has(entity))
                {
                    nodeComponent.Data = (NodeDraggableInputData)nodeInput.Data;
                }

                Debug.LogError(nodeComponent.Data.Draggable.MaxVelocity);
                
                if (eventsHolderComponent.EventsHolder.IsAnyTriggerInProgress.GetValue())
                {
                    _completeRequests.Publish(new NodeCompleteRequest()
                    {
                        CurrentNodeEntity = entity,
                        NextNodeProvider = nodeComponent.NextNodeProvider,
                        OutputData = new NodeDraggableInputData()
                        {
                            Draggable = new DraggableComponent()
                            {
                                MaxVelocity = 10
                            }
                        }
                    });
                }
            }
        }

        public void Dispose()
        {
        }
    }
}