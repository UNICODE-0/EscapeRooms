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
    public sealed class TransformLerpNodeSystem : ISystem
    {
        public World World { get; set; }

        private Filter _filter;

        private Stash<TransformLerpNodeComponent> _nodeStash;
        private Stash<NodeInitializeFlag> _initFlagStash;
        private Stash<TransformComponent> _transformStash;
        private Stash<FloatLerpComponent> _lerpStash;

        private Request<NodeCompleteRequest> _completeRequests;

        private NodeInputHelper<EntityNodeIOComponent> _nodeInput;
        
        public void OnAwake()
        {
            _filter = World.Filter
                .With<TransformLerpNodeComponent>()
                .With<NodeTag>()
                .Build();

            _nodeStash = World.GetStash<TransformLerpNodeComponent>();
            _initFlagStash = World.GetStash<NodeInitializeFlag>();
            _lerpStash = World.GetStash<FloatLerpComponent>();
            _transformStash = World.GetStash<TransformComponent>();
            
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
                ref var transformComponent = ref _transformStash.Get(input.Entity);

                if (_initFlagStash.Has(entity))
                {
                    nodeComponent.StartPosition = transformComponent.Transform.position;
                    nodeComponent.StartRotation = transformComponent.Transform.rotation;
                }
                
                ref var lerpComponent = ref _lerpStash.Get(nodeComponent.LerpProvider.Entity);
                lerpComponent.StartLerpInput = true;

                if (lerpComponent.IsLerpInProgress)
                {
                    transformComponent.Transform.position = Vector3.Lerp(nodeComponent.StartPosition,
                        nodeComponent.Target.position, lerpComponent.CurrentValue);

                    transformComponent.Transform.rotation = Quaternion.Lerp(nodeComponent.StartRotation,
                        nodeComponent.Target.rotation, lerpComponent.CurrentValue);

                    if (lerpComponent.IsLerpTimeIsUp)
                    {
                        _completeRequests.Publish(new NodeCompleteRequest()
                        {
                            CurrentNodeEntity = entity,
                            NextNodeProvider = nodeComponent.NextNodeProvider,
                        });
                    }
                }
            }
        }

        public void Dispose()
        { 
        }
    }
}