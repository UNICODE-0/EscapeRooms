using EscapeRooms.Components;
using EscapeRooms.Requests;
using Scellecs.Morpeh;
using Unity.IL2CPP.CompilerServices;

namespace EscapeRooms.Systems
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    public sealed class PlayerJumpInputInterruptSystem : ISystem
    {
        public World World { get; set; }

        private Filter _filter;
        private Stash<CharacterJumpComponent> _jumpStash;
        private Request<InputTriggerInterruptRequest> _triggerInterruptRequest;

        public void OnAwake()
        {
            _filter = World.Filter
                .With<CharacterJumpComponent>()
                .With<PlayerTag>()
                .Build();

            _jumpStash = World.GetStash<CharacterJumpComponent>();
            _triggerInterruptRequest = World.GetRequest<InputTriggerInterruptRequest>();
        }

        public void OnUpdate(float deltaTime)
        {
            foreach (var entity in _filter)
            {
                ref var jumpComponent = ref _jumpStash.Get(entity);

                if (jumpComponent.JumpInput && jumpComponent.IsJumpForceApplied)
                {
                    _triggerInterruptRequest.Publish(new InputTriggerInterruptRequest()
                    {
                        TriggerToInterrupt = InterruptibleInputTrigger.Jump
                    });
                }
            }
        }

        public void Dispose()
        {
        }
    }
}