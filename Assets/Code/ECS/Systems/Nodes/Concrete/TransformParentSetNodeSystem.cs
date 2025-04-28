using EscapeRooms.Components;
using EscapeRooms.Requests;
using Scellecs.Morpeh;
using Unity.IL2CPP.CompilerServices;

namespace EscapeRooms.Systems
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    public sealed class TransformParentSetNodeSystem : ISystem
    {
        public World World { get; set; }

        private Filter _filter;

        private Stash<TransformParentSetNodeComponent> _nodeStash;
        private Stash<TransformComponent> _transformStash;
        private Request<NodeCompleteRequest> _completeRequests;

        private NodeInputHelper<EntityNodeIOComponent> _inputOutput;
        
        public void OnAwake()
        {
            _filter = World.Filter
                .With<TransformParentSetNodeComponent>()
                .With<NodeTag>()
                .Build();

            _nodeStash = World.GetStash<TransformParentSetNodeComponent>();
            _transformStash = World.GetStash<TransformComponent>();
            _completeRequests = World.GetRequest<NodeCompleteRequest>();

            _inputOutput = new();
            _inputOutput.Initialize(World);
        }

        public void OnUpdate(float deltaTime)
        {
            foreach (var entity in _filter)
            {
                ref var nodeComponent = ref _nodeStash.Get(entity);
                ref var input = ref _inputOutput.TryGet(nodeComponent, out _);
                ref var targetTransform = ref _transformStash.Get(input.Entity);

                targetTransform.Transform.parent = nodeComponent.Parent;
                
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