using EscapeRooms.Components;
using EscapeRooms.Requests;
using Scellecs.Morpeh;
using Unity.IL2CPP.CompilerServices;

namespace EscapeRooms.Systems
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    public sealed class NodeInitializeSystem : ISystem
    {
        public World World { get; set; }
        
        private Stash<NodeInitializeFlag> _initFlagStash;
        private Request<NodeInitializeRequest> _initRequests;

        public void OnAwake()
        {
            _initFlagStash = World.GetStash<NodeInitializeFlag>();
            _initRequests = World.GetRequest<NodeInitializeRequest>();
        }

        public void OnUpdate(float deltaTime)
        {
            foreach (var request in _initRequests.Consume())
            {
                ref var flag = ref _initFlagStash.Add(request.NodeProvider.Entity);
                
                FlagDisposeSystem.ScheduleFlagDispose(ref flag, () =>
                {
                    _initFlagStash.Remove(request.NodeProvider.Entity);
                });
            }
        }

        public void Dispose()
        {
        }
    }
}