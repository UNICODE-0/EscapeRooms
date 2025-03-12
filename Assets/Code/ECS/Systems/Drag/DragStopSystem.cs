using System.Linq;
using EscapeRooms.Components;
using EscapeRooms.Data;
using EscapeRooms.Events;
using Scellecs.Morpeh;
using Scellecs.Morpeh.Collections;
using Scellecs.Morpeh.Providers;
using UnityEngine;
using Unity.IL2CPP.CompilerServices;

namespace EscapeRooms.Systems
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    public sealed class DragStopSystem : ISystem
    {
        public World World { get; set; }

        private Filter _filter;
        private Stash<DragComponent> _dragStash;
        private Stash<ConfigurableJointComponent> _configurableJointStash;
        private Stash<RigidbodyComponent> _rigidbodyStash;
        private Stash<OnDragFlag> _onDragStash;
        
        private Event<DragStopEvent> _dragStopEvent;
        
        public void OnAwake()
        {
            _filter = World.Filter
                .With<DragComponent>()
                .Build();

            _dragStash = World.GetStash<DragComponent>();
            _configurableJointStash = World.GetStash<ConfigurableJointComponent>();
            _rigidbodyStash = World.GetStash<RigidbodyComponent>();
            _onDragStash = World.GetStash<OnDragFlag>();
            
            _dragStopEvent = World.GetEvent<DragStopEvent>();
        }

        public void OnUpdate(float deltaTime)
        {
            foreach (var entity in _filter)
            {
                ref var dragComponent = ref _dragStash.Get(entity);
                
                if(!dragComponent.DragInput && dragComponent.IsDragging)
                {
                    ref var jointComponent = ref _configurableJointStash.Get(dragComponent.DraggableEntity);
                    ref var itemRigidbodyComponent = ref _rigidbodyStash.Get(dragComponent.DraggableEntity);

                    jointComponent.ConfigurableJoint.connectedBody = null;
                    SetJointDefaultData(jointComponent.ConfigurableJoint);
                    
                    itemRigidbodyComponent.Rigidbody.linearDamping = 0;
                    itemRigidbodyComponent.Rigidbody.angularDamping = 0.05f; // default value
                    
                    _onDragStash.Remove(dragComponent.DraggableEntity);
                    
                    _dragStopEvent.ThisFrame(new DragStopEvent()
                    {
                        Draggable = dragComponent.DraggableEntity,
                        Owner = entity
                    });
                    
                    dragComponent.DraggableEntity = default;
                    dragComponent.IsDragging = false;
                }
            }
        }
        
        private void SetJointDefaultData(ConfigurableJoint joint)
        {
            joint.xDrive = default;
            joint.yDrive = default;
            joint.zDrive = default;
            
            joint.angularXDrive = default;
            joint.angularYZDrive = default;
        }
        
        public void Dispose()
        {
        }
    }
}