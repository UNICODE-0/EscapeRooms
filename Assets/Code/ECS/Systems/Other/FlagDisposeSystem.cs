using System;
using EscapeRooms.Components;
using Scellecs.Morpeh;
using Scellecs.Morpeh.Collections;
using Unity.IL2CPP.CompilerServices;

namespace EscapeRooms.Systems
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    public sealed class FlagDisposeSystem : ILateSystem
    {
        private static readonly FastList<IFlagComponent> FlagsToDispose = new FastList<IFlagComponent>();
        public static void ScheduleFlagDispose<Flag>(ref Flag flag, Action disposeAction) 
            where Flag : struct, IFlagComponent
        {
            flag.IsLastFrameOfLife = true;
            flag.DisposeAction = disposeAction;
            FlagsToDispose.Add(flag);
        }
        
        public World World { get; set; }
        public void OnAwake()
        {
        }

        public void OnUpdate(float deltaTime)
        {
            foreach (var evt in FlagsToDispose)
            {
                evt.DisposeAction.Invoke();
            }
            FlagsToDispose.Clear();
        }

        public void Dispose()
        {
            FlagsToDispose.Clear();
        }
    }
}