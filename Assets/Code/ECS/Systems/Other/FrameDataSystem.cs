using EscapeRooms.Data;
using Scellecs.Morpeh;
using Unity.IL2CPP.CompilerServices;

namespace EscapeRooms.Systems
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    public sealed class FrameDataSystem : ILateSystem
    {
        public World World { get; set; }
        public void OnAwake()
        {
            FrameData.TrySetInstance(new FrameData());
        }

        public void OnUpdate(float deltaTime)
        {
            if (++FrameData.Instance.FrameId == FrameData.MAX_FRAME_ID)
                FrameData.Instance.FrameId = 1;
        }

        public void Dispose()
        {
            FrameData.TryRemoveInstance();
        }
    }
}