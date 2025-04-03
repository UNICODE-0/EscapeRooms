using System;
using EscapeRooms.Components;
using Scellecs.Morpeh;
using Scellecs.Morpeh.Collections;
using Unity.IL2CPP.CompilerServices;
using UnityEngine;

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
            flag.DisposeOrder = FlagsToDispose.length;
            FlagsToDispose.Add(flag);
        }
        
        public static void CancelFlagDispose<Flag>(ref Flag flag) 
            where Flag : struct, IFlagComponent
        {
            if(!flag.IsLastFrameOfLife) return;
            
            flag.IsLastFrameOfLife = false;
            flag.DisposeAction = null;
            
            FlagsToDispose[flag.DisposeOrder].IsLastFrameOfLife = false;
        }
        
        public World World { get; set; }
        public void OnAwake()
        {
        }

        public void OnUpdate(float deltaTime)
        {
            foreach (var evt in FlagsToDispose)
            {
                if (evt.IsLastFrameOfLife)
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