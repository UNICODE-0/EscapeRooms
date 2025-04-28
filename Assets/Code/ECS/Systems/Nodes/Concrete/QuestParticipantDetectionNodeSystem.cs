using System.Linq;
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
    public sealed class QuestParticipantDetectionNodeSystem : ISystem
    {
        public World World { get; set; }

        private Filter _filter;

        private Stash<QuestParticipantDetectionNodeComponent> _nodeStash;
        private Stash<OverlapBoxComponent> _overlapStash;
        private Stash<QuestParticipantComponent> _participantStash;
        private Request<NodeCompleteRequest> _completeRequests;

        private NodeOutputHelper<EntityNodeIOComponent> _nodeOutput;
        
        public void OnAwake()
        {
            _filter = World.Filter
                .With<QuestParticipantDetectionNodeComponent>()
                .With<OverlapBoxComponent>()
                .With<NodeTag>()
                .Build();

            _nodeStash = World.GetStash<QuestParticipantDetectionNodeComponent>();
            _overlapStash = World.GetStash<OverlapBoxComponent>();
            _participantStash = World.GetStash<QuestParticipantComponent>();
            _completeRequests = World.GetRequest<NodeCompleteRequest>();

            _nodeOutput = new();
            _nodeOutput.Initialize(World);
        }

        public void OnUpdate(float deltaTime)
        {
            foreach (var entity in _filter)
            {
                ref var overlapComponent = ref _overlapStash.Get(entity);
                ref var nodeComponent = ref _nodeStash.Get(entity);

                if (overlapComponent.IsBoxIntersect)
                {
                    int id = overlapComponent.HitColliders.First().gameObject.GetInstanceID();
                    Entity overlapEntity = EntityProvider.map.GetValueByKey(id).entity;
                    
                    ref var participantComponent = ref _participantStash.Get(overlapEntity, out bool participantExist);
                    if (participantExist && participantComponent.Type == nodeComponent.ParticipantType)
                    {
                        ref var output = ref _nodeOutput.TryGet(nodeComponent, out bool outputExist);

                        if (outputExist)
                            output.Entity = overlapEntity;
                    
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