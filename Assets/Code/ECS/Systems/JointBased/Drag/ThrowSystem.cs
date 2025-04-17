using EscapeRooms.Components;
using EscapeRooms.Requests;
using Scellecs.Morpeh;
using Unity.IL2CPP.CompilerServices;
using UnityEngine;

namespace EscapeRooms.Systems
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    public sealed class ThrowSystem : ISystem
    {
        public World World { get; set; }

        private Filter _filter;
        private Stash<DragComponent> _dragStash;
        private Stash<ThrowComponent> _throwStash;
        private Stash<RigidbodyComponent> _rigidbodyStash;
        private Stash<TransformComponent> _transformStash;
        private Stash<DraggableComponent> _draggableStash;

        private Request<RigidbodyForceApplyRequest> _forceApplyRequest;
        
        public void OnAwake()
        {
            _filter = World.Filter
                .With<ThrowComponent>()
                .With<DragComponent>()
                .With<RigidbodyComponent>()
                .Build();

            _dragStash = World.GetStash<DragComponent>();
            _throwStash = World.GetStash<ThrowComponent>();
            _rigidbodyStash = World.GetStash<RigidbodyComponent>();
            _transformStash = World.GetStash<TransformComponent>();
            _draggableStash = World.GetStash<DraggableComponent>();
            
            _forceApplyRequest = World.GetRequest<RigidbodyForceApplyRequest>();
        }
        
        public void OnUpdate(float deltaTime)
        {
            foreach (var entity in _filter)
            {
                ref var dragComponent = ref _dragStash.Get(entity);
                ref var throwComponent = ref _throwStash.Get(entity);

                if(dragComponent.IsDragging && throwComponent.ThrowInput)
                {
                    ref var rigidbodyComponent = ref _rigidbodyStash.Get(dragComponent.DraggableEntity);
                    ref var draggableComponent = ref _draggableStash.Get(dragComponent.DraggableEntity);
                    ref var transformComponent = ref _transformStash.Get(entity);

                    float massImpulseBonus = throwComponent.ThrowImpulse * throwComponent.MassImpulseBonus *
                                           Mathf.Max(draggableComponent.MassBeforeDrag - 1, 0f);

                    Vector3 throwDirection = transformComponent.Transform.forward *
                                             (throwComponent.ThrowImpulse + massImpulseBonus);
                    
                    _forceApplyRequest.Publish(new RigidbodyForceApplyRequest()
                    {
                        Rigidbody = rigidbodyComponent.Rigidbody,
                        Force = throwDirection,
                        Mode = ForceMode.Impulse
                    });
                }
            }
        }
        
        public void Dispose()
        {
        }
    }
}