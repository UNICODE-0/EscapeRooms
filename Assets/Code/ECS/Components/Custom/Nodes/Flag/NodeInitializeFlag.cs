using System;
using Scellecs.Morpeh;
using Unity.IL2CPP.CompilerServices;

namespace EscapeRooms.Components
{
    [System.Serializable]
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    public struct NodeInitializeFlag : IFlagComponent
    {
        public Entity Owner { get; set; }
        public bool IsLastFrameOfLife { get; set; }
        public Action DisposeAction { get; set; }
        public int DisposeOrder { get; set; }
    }
}