using System;

namespace EscapeRooms.Components
{
    public interface IFlagComponent : IOwnerComponent
    {
        public bool IsLastFrameOfLife { get; set; }
        public Action DisposeAction { get; set; }
        public int DisposeOrder { get; set; }
    }
}