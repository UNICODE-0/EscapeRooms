using System.Collections.Generic;
using EscapeRooms.Components;
using Scellecs.Morpeh;
using Scellecs.Morpeh.Collections;
using UnityEngine;
using Unity.IL2CPP.CompilerServices;
using UnityEngine.InputSystem;

namespace EscapeRooms.Systems
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    public sealed class RaycastSystem : ISystem
    {
        public World World { get; set; }

        private Filter _filter;
        private Stash<RaycastComponent> _raycastStash;

        public void OnAwake()
        {
            _filter = World.Filter
                .With<RaycastComponent>()
                .Build();

            _raycastStash = World.GetStash<RaycastComponent>();
        }

        public void OnUpdate(float deltaTime)
        {
            foreach (var entity in _filter)
            {
                ref var raycastComponent = ref _raycastStash.Get(entity);

                raycastComponent.Hits ??= new RaycastHit[raycastComponent.HitsCount];

                raycastComponent.IsRayHit = Physics.RaycastNonAlloc(raycastComponent.RayStartPoint.position,
                    raycastComponent.RayStartPoint.rotation * raycastComponent.Direction, 
                    raycastComponent.Hits, raycastComponent.RayLength, raycastComponent.LayerMask) > 0;
                
                #if UNITY_EDITOR
                if(raycastComponent.UseDebugDraw)
                {
                    Debug.DrawRay(raycastComponent.RayStartPoint.position,
                        raycastComponent.RayStartPoint.rotation * raycastComponent.Direction *
                        raycastComponent.RayLength, raycastComponent.DebugColor, 0f, true);
                }                          
                #endif
            }
        }

        public void Dispose()
        {
        }
    }
}