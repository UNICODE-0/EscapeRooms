using EscapeRooms.Components;
using EscapeRooms.Requests;
using Scellecs.Morpeh;
using Unity.IL2CPP.CompilerServices;

namespace EscapeRooms.Systems
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    public sealed class AnimationPlayNodeSystem : ISystem
    {
        public World World { get; set; }

        private Filter _filter;

        private Stash<AnimationPlayNodeComponent> _nodeStash;
        private Stash<NodeInitializeFlag> _initStash;
        private Request<NodeCompleteRequest> _completeRequests;

        public void OnAwake()
        {
            _filter = World.Filter
                .With<AnimationPlayNodeComponent>()
                .With<NodeTag>()
                .Build();

            _nodeStash = World.GetStash<AnimationPlayNodeComponent>();
            _initStash = World.GetStash<NodeInitializeFlag>();
            _completeRequests = World.GetRequest<NodeCompleteRequest>();
        }

        public void OnUpdate(float deltaTime)
        {
            foreach (var entity in _filter)
            {
                ref var nodeComponent = ref _nodeStash.Get(entity);

                if (_initStash.Has(entity))
                {
                    nodeComponent.Animation.Play();
                    continue;
                }

                if (!nodeComponent.Animation.isPlaying)
                {
                    _completeRequests.Publish(new NodeCompleteRequest()
                    {
                        CurrentNodeEntity = entity,
                        NextNodeProvider = nodeComponent.NextNodeProvider,
                    });
                }
            }
        }

        public void Dispose()
        { 
        }
    }
}