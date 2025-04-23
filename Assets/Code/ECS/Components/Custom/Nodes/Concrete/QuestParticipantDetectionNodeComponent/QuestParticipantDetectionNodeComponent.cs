using EscapeRooms.Providers;
using Unity.IL2CPP.CompilerServices;
using UnityEngine;

namespace EscapeRooms.Components
{
    [System.Serializable]
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    public struct QuestParticipantDetectionNodeComponent : IOutputNodeComponent<EntityNodeIOComponent>
    {
        public QuestParticipantType ParticipantType;
        
        [field: SerializeField]
        public NodeTagProvider NextNodeProvider { get; set; }
        
        [field: SerializeField]
        public NodeDataProvider<EntityNodeIOComponent> OutputDataProvider { get; set; }
    }
}