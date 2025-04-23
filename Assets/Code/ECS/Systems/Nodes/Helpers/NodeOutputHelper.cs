using EscapeRooms.Components;
using Scellecs.Morpeh;

namespace EscapeRooms.Systems
{
    public class NodeOutputHelper<O> where O : struct, INodeDataComponent
    {
        private Stash<O> _outputStash;

        private O _empty;
        
        public void Initialize(World world)
        {
            _outputStash = world.GetStash<O>();
        }

        public ref O TryGet(IOutputNodeComponent<O> node, out bool exist)
        {
            if (node.OutputDataProvider is null)
            {
                exist = false;
                return ref _empty;
            }
            
            ref O outputComponent = ref _outputStash.Get(node.OutputDataProvider.Entity, out exist);
            
            return ref outputComponent;
        }
    }
}