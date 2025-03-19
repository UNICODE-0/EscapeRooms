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
            if (notFalseOnThisFrame && _setFalseFrameId == FrameData.Instance.FrameId)
            {
                return;
            }
            
            _value = true;
            _setTrueFrameId = FrameData.Instance.FrameId;
        }

        public void SetFalse(bool notTrueOnThisFrame = false)
        {
            if (notTrueOnThisFrame && _setTrueFrameId == FrameData.Instance.FrameId)
            {
                return;
            }
            
            _value = false;
            _setFalseFrameId = FrameData.Instance.FrameId;
        }
    }
}