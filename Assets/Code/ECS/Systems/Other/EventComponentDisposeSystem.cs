using Scellecs.Morpeh;
using Scellecs.Morpeh.Collections;
using Unity.IL2CPP.CompilerServices;

namespace EscapeRooms.Systems
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    public sealed class EventComponentDisposeSystem : ILateSystem
    {
        public static readonly FastList<IComponent> EventsToDispose = new FastList<IComponent>();
        
        public World World { get; set; }
        public void OnAwake()
        {
        }

        public void OnUpdate(float deltaTime)
        {
            foreach (var evt in EventsToDispose)
            {
                
            }
        }

        public void Dispose()
        {
            EventsToDispose.Clear();
        }
    }
}