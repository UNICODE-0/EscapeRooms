using Sirenix.OdinInspector;
using Unity.IL2CPP.CompilerServices;

namespace EscapeRooms.Data
{
    [System.Serializable]
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    public struct FrameUniqueBool
    {
        [ShowInInspector]
        [HideLabel]
        private bool _value;
        
        private int _setTrueFrameId;
        private int _setFalseFrameId;
        
        public bool GetValue() => _value;

        public void SetTrue(bool notFalseOnThisFrame = false)
        {
            int frameId = FrameData.Instance is null ? 0 : FrameData.Instance.FrameId;
            
            if (notFalseOnThisFrame && _setFalseFrameId == frameId)
            {
                return;
            }

            _value = true;
            _setTrueFrameId = frameId;
        }

        public void SetFalse(bool notTrueOnThisFrame = false)
        {
            int frameId = FrameData.Instance is null ? 0 : FrameData.Instance.FrameId;
            
            if (notTrueOnThisFrame && _setTrueFrameId == frameId)
            {
                return;
            }
            
            _value = false;
            _setFalseFrameId = frameId;
        }
    }
}