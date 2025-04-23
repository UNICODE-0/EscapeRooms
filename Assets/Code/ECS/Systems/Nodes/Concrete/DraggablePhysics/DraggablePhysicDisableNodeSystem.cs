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
    public sealed class DraggablePhysicDisableNodeSystem : ISystem
    {
        public World World { get; set; }

        private Filter _filter;

        private Stash<DraggablePhysicDisableNodeComponent> _nodeStash;
        private Stash<RigidbodyComponent> _rigidbodyStash;
        private Stash<DraggableComponent> _draggableStash;

        private Request<NodeCompleteRequest> _completeRequests;

        private NodeInputHelper<EntityNodeIOComponent> _nodeInput;
        
        public void OnAwake()
        {
            _filter = World.Filter
                .With<DraggablePhysicDisableNodeComponent>()
                .With<NodeTag>()
                .Build();

            _nodeStash = World.GetStash<DraggablePhysicDisableNodeComponent>();
            _rigidbodyStash = World.GetStash<RigidbodyComponent>();
            _draggableStash = World.GetStash<DraggableComponent>();
            
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
                ref var rigidbodyComponent = ref _rigidbodyStash.Get(input.Entity);
                ref var draggableComponent = ref _draggableStash.Get(input.Entity);
                
                rigidbodyComponent.Rigidbody.isKinematic = true;
                foreach (var col in draggableComponent.Colliders)
                {
                    col.enabled = false;
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