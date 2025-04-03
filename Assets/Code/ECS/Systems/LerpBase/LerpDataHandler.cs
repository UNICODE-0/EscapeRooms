using EscapeRooms.Components;
using Scellecs.Morpeh;
using UnityEngine;

namespace EscapeRooms.Systems
{
    public class LerpDataHandler<State> 
        where State: struct
    {
        private readonly Stash<FloatLerpComponent> _floatLerpStash;
        
        public LerpDataHandler(Stash<FloatLerpComponent> floatLerpStash)
        {
            _floatLerpStash = floatLerpStash;
        }

        public bool Handle(ref LerpDataComponent<State> lerpData, out State from, out State to, out float progress)
        {
            ref var floatLerpComponent = ref _floatLerpStash.Get(lerpData.LerpProvider.Entity);
            
            if (lerpData.UseLerpProviderInputState && lerpData.ChangeStateInput)
            {
                Debug.LogWarning("You can't use transfer input because lerp data use lerp input state");
                lerpData.ChangeStateInput = false;
            } else if(!lerpData.UseLerpProviderInputState)
                floatLerpComponent.StartLerpInput = lerpData.ChangeStateInput;
        
            if (floatLerpComponent.IsLerpInProgress)
            {
                from = lerpData.IsTargetState ? lerpData.TargetState : lerpData.DefaultState;
                to = lerpData.IsTargetState ? lerpData.DefaultState : lerpData.TargetState;
                progress = floatLerpComponent.CurrentValue;
                
                if (floatLerpComponent.IsLerpTimeIsUp)
                    lerpData.IsTargetState = !lerpData.IsTargetState;

                return true;
            }

            from = default;
            to = default;
            progress = default;
            
            return false;
        }
        
    }
}