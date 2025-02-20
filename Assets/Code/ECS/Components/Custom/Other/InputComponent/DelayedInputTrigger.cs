using Sirenix.OdinInspector;
using Unity.IL2CPP.CompilerServices;
using UnityEngine;

namespace EscapeRooms.Data
{
    [System.Serializable]
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    public class DelayedInputTrigger
    {
        private bool _isInitialized;
        private bool _targetValue;
        private float _timer;
        private float _delayDuration;
        
        [ShowInInspector]
        [HideLabel]
        private bool _isTriggered;
        public bool IsTriggered => _isTriggered;

        public void Initialize(float delayDuration)
        {
            if (delayDuration < 0f)
            {
                Debug.LogError("delay duration can't be lower than 0");
                return;
            }
            
            _delayDuration = delayDuration;
            _isInitialized = true;
        }
        
        public void Update(float deltaTime, bool inputTriggerValue)
        {
            if (!_isInitialized)
            {
                Debug.LogError("Attempt to update delayed input trigger before initialize");
                return;
            }
            
            if (inputTriggerValue && !_isTriggered)
            {
                _targetValue = true;
                _timer = _delayDuration;
            }
            else if (!inputTriggerValue && _timer > 0f)
            {
                _timer -= deltaTime;

                if (_timer <= 0f)
                {
                    _targetValue = false;
                    _timer = 0f;
                }
            }
            else if (inputTriggerValue)
            {
                _timer = _delayDuration;
            }

            _isTriggered = _targetValue;
        }

        public void Interrupt()
        {
            _timer = 0f;
            _targetValue = false;
            _isTriggered = false;
        }
    }
}