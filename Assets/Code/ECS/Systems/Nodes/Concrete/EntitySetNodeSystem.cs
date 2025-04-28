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
    public sealed class EntitySetNodeSystem : ISystem
    {
        public World World { get; set; }

        private Filter _filter;

        private Stash<EntitySetNodeComponent> _nodeStash;
        private Request<NodeCompleteRequest> _completeRequests;

        private NodeOutputHelper<EntityNodeIOComponent> _nodeOutput;
        
        public void OnAwake()
        {
            _filter = World.Filter
                .With<EntitySetNodeComponent>()
                .With<NodeTag>()
                .Build();

            _nodeStash = World.GetStash<EntitySetNodeComponent>();
            _completeRequests = World.GetRequest<NodeCompleteRequest>();

            _nodeOutput = new();
            _nodeOutput.Initialize(World);
        }

        public void OnUpdate(float deltaTime)
        {
            foreach (var entity in _filter)
            {
                ref var nodeComponent = ref _nodeStash.Get(entity);
                ref var output = ref _nodeOutput.TryGet(nodeComponent, out bool exist);

                if (exist)
                {
                    output.Entity = nodeComponent.EntityProvider.Entity;
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