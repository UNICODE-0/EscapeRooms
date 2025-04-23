using EscapeRooms.Components;
using Scellecs.Morpeh;

namespace EscapeRooms.Systems
{
    public class NodeInputHelper<I> where I : struct, INodeDataComponent
    {
        private Stash<I> _inputStash;

        private I _empty;
        
        public void Initialize(World world)
        {
            _inputStash = world.GetStash<I>();
        }

        public ref I TryGet(IInputNodeComponent<I> node, out bool exist)
        {
            if (node.InputDataProvider is null)
            {
                exist = false;
                return ref _empty;
            }
            
            ref I inputComponent = ref _inputStash.Get(node.InputDataProvider.Entity, out exist);
            
            return ref inputComponent;
        }
    }
}