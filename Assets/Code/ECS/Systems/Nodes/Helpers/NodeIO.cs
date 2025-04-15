using EscapeRooms.Components;
using EscapeRooms.Providers;
using Scellecs.Morpeh;

namespace EscapeRooms.Systems
{
    public class NodeIO<I> where I : struct, INodeDataComponent
    {
        private Stash<I> _inputStash;

        private I _empty;
        
        public void Initialize(World world)
        {
            _inputStash = world.GetStash<I>();
        }

        public ref I TryGet(NodeDataProvider<I> data, out bool exist)
        {
            if (data is null)
            {
                exist = false;
                return ref _empty;
            }
            
            ref I inputComponent = ref _inputStash.Get(data.Entity);
            
            exist = true;
            return ref inputComponent;
        }
    }
}