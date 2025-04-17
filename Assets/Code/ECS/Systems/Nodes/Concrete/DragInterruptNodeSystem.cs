using EscapeRooms.Components;
using EscapeRooms.Requests;
using Scellecs.Morpeh;
using Unity.IL2CPP.CompilerServices;
using UnityEngine;

namespace EscapeRooms.Systems
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    public sealed class DragInterruptNodeSystem : ISystem
    {
        public World World { get; set; }

        private Filter _filter;

        private Stash<DragInterruptNodeComponent> _nodeStash;
        private Stash<OnDragFlag> _onDragStash;
        private Stash<DragComponent> _dragStash;

        private Request<NodeCompleteRequest> _completeRequests;

        private NodeInputHelper<EntityNodeIOComponent> _nodeInput;
        
        public void OnAwake()
        {
            _filter = World.Filter
                .With<DragInterruptNodeComponent>()
                .With<NodeTag>()
                .Build();

            _nodeStash = World.GetStash<DragInterruptNodeComponent>();
            _onDragStash = World.GetStash<OnDragFlag>();
            _dragStash = World.GetStash<DragComponent>();
            
            _completeRequests = World.GetRequest<NodeCompleteRequest>();
            
            _nodeInput = new();
            _nodeInput.Initialize(World);
        }

        public void OnUpdate(float deltaTime)
        {
            foreach (var entity in _filter)
            {
                ref var nodeComponent = ref _nodeStash.Get(entity);
                ref var input = ref _nodeInput.TryGet(nodeComponent, out _);
                ref var onDragFlag = ref _onDragStash.Get(input.Entity, out bool exist);
                
                if (exist)
                {
                    ref var dragComponent = ref _dragStash.Get(onDragFlag.Owner);
                    dragComponent.DragStopInput = true;
                }
                
                _completeRequests.Publish(new NodeCompleteRequest()
                {
                    CurrentNodeEntity = entity,
                    NextNodeProvider = nodeComponent.NextNodeProvider,
                });
            }
        }

        public void Dispose()
        { 
        }
    }
}