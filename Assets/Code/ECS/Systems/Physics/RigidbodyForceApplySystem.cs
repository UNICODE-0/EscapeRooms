using EscapeRooms.Components;
using EscapeRooms.Requests;
using Scellecs.Morpeh;
using UnityEngine;
using Unity.IL2CPP.CompilerServices;

namespace EscapeRooms.Systems
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    public sealed class RigidbodyForceApplySystem : ISystem
    {
        public World World { get; set; }
        
        private Request<RigidbodyForceApplyRequest> _forceApplyRequest;


        public void OnAwake()
        {
            _forceApplyRequest = World.GetRequest<RigidbodyForceApplyRequest>();
        }

        public void OnUpdate(float deltaTime)
        {
            foreach (var request in _forceApplyRequest.Consume())
            {
                request.Rigidbody.AddForce(request.Force, request.Mode);
            }
        }

        public void Dispose()
        {
        }
    }
}