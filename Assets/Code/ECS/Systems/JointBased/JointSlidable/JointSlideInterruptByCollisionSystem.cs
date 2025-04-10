using EscapeRooms.Components;
using Scellecs.Morpeh;
using Unity.IL2CPP.CompilerServices;
using UnityEngine;

namespace EscapeRooms.Systems
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    public sealed class JointSlideInterruptByCollisionSystem : ISystem
    {
        public World World { get; set; }

        private Filter _filter;
        private Stash<JointSlideComponent> _jointSlideStash;
        private Stash<InteractInterruptFlag> _flagStash;

        public void OnAwake()
        {
            _filter = World.Filter
                .With<JointSlideComponent>()
                .Build();

            _jointSlideStash = World.GetStash<JointSlideComponent>();
            _flagStash = World.GetStash<InteractInterruptFlag>();
        }

        public void OnUpdate(float deltaTime)
        {
            foreach (var entity in _filter)
            {
                ref var jointSlideComponent = ref _jointSlideStash.Get(entity);

                if(jointSlideComponent.IsSliding && _flagStash.Has(jointSlideComponent.SlidableEntity))
                {
                    jointSlideComponent.SlideStopInput = true;
                }
            }
        }
        
        public void Dispose()
        {
        }
    }
}