using EscapeRooms.Components;
using EscapeRooms.Helpers;
using Scellecs.Morpeh;
using UnityEngine;
using Unity.IL2CPP.CompilerServices;

namespace EscapeRooms.Systems
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    public sealed class CharacterCrouchSystem : ISystem
    {
        public World World { get; set; }

        private Filter _filter;
        private Stash<CharacterCrouchComponent> _crouchStash;
        private Stash<FloatLerpComponent> _floatLerpStash;
        private Stash<TransformPositionLerpComponent> _transformPositionLerpStash;

        public void OnAwake()
        {
            _filter = World.Filter
                .With<CharacterCrouchComponent>()
                .Build();

            _crouchStash = World.GetStash<CharacterCrouchComponent>();
            _floatLerpStash = World.GetStash<FloatLerpComponent>();
            _transformPositionLerpStash = World.GetStash<TransformPositionLerpComponent>();
        }

        public void OnUpdate(float deltaTime)
        {
            foreach (var entity in _filter)
            {
                ref var crouchComponent = ref _crouchStash.Get(entity);
                ref var heightFloatLerpComponent = ref _floatLerpStash.Get(crouchComponent.HeightLerpProvider.Entity);
                ref var headLerpComponent = ref _transformPositionLerpStash.Get(crouchComponent.HeadLerpProvider.Entity);
                ref var headFloatLerpComponent = ref _floatLerpStash.Get(headLerpComponent.FloatLerpProvider.Entity);

                if (crouchComponent.CrouchBlockFlag.IsFlagClear())
                {
                    heightFloatLerpComponent.StartLerpInput = crouchComponent.CrouchInput;
                    headLerpComponent.ChangePositionInput = crouchComponent.CrouchInput;
                }
                else
                {
                    heightFloatLerpComponent.StartLerpInput = false;
                    headLerpComponent.ChangePositionInput = false;
                }

                if (heightFloatLerpComponent.IsLerpInProgress)
                {
                    crouchComponent.IsSquatInProgress = true;

                    if (crouchComponent.CrouchBlockFlag.CheckFlag(CrouchBlockers.CROUCH_STANDING_BLOCK_FLAG))
                    {
                        heightFloatLerpComponent.IsLerpPaused = true;
                        headFloatLerpComponent.IsLerpPaused = true;
                    }
                    else
                    {
                        heightFloatLerpComponent.IsLerpPaused = false;
                        headFloatLerpComponent.IsLerpPaused = false;
                    }

                    if (heightFloatLerpComponent.IsLerpTimeIsUp)
                    {
                        crouchComponent.IsSquatInProgress = false;
                        crouchComponent.IsCrouching = !crouchComponent.IsCrouching;
                    }
                }
            }
        }

        public void Dispose()
        {
        }
    }
}