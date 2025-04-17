using EscapeRooms.Components;
using EscapeRooms.Requests;
using Scellecs.Morpeh;
using Unity.IL2CPP.CompilerServices;

namespace EscapeRooms.Systems
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    public sealed class NodeCompleteSystem : ISystem
    {
        public World World { get; set; }
        
        private Stash<GameObjectComponent> _gameObjectStash;

        private Request<NodeCompleteRequest> _completeRequests;
        private Request<NodeInitializeRequest> _initRequests;

        public void OnAwake()
        {
            _gameObjectStash = World.GetStash<GameObjectComponent>();
            
            _completeRequests = World.GetRequest<NodeCompleteRequest>();
            _initRequests = World.GetRequest<NodeInitializeRequest>();
        }

        public void OnUpdate(float deltaTime)
        {
            foreach (var request in _completeRequests.Consume())
            {
                ref var currentNodeGO = ref _gameObjectStash.Get(request.CurrentNodeEntity);
                currentNodeGO.GameObject.SetActive(false);

                if(request.NextNodeProvider == null) continue;
                request.NextNodeProvider.gameObject.SetActive(true);

                _initRequests.Publish(new NodeInitializeRequest()
                {
                    NodeProvider = request.NextNodeProvider,
                }, allowNextFrame: true);
            }
        }

        public void Dispose()
        {
        }
    }
}