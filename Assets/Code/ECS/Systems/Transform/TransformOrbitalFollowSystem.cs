using EscapeRooms.Components;
using Scellecs.Morpeh;
using UnityEngine;
using Unity.IL2CPP.CompilerServices;

namespace EscapeRooms.Systems
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    public sealed class TransformOrbitalFollowSystem : ISystem
    {
        public World World { get; set; }

        private Filter _filter;
        private Stash<TransformComponent> _transformStash;
        private Stash<TransformOrbitalFollowComponent> _orbitalFollowStash;

        public void OnAwake()
        {
            _filter = World.Filter
                .With<TransformComponent>()
                .With<TransformOrbitalFollowComponent>()
                .Build();

            _transformStash = World.GetStash<TransformComponent>();
            _orbitalFollowStash = World.GetStash<TransformOrbitalFollowComponent>();
        }

        public void OnUpdate(float deltaTime)
        {
            foreach (var entity in _filter)
            {
                ref var transformComponent = ref _transformStash.Get(entity);
                ref var orbitalFollowComponent = ref _orbitalFollowStash.Get(entity);

                Vector3 targetPosition = orbitalFollowComponent.Target.position;
                Vector3 centerPosition = orbitalFollowComponent.SphereCenter.position;
                Vector3 toTarget = (targetPosition - centerPosition).normalized;

                float targetAzimuth = Mathf.Atan2(toTarget.z, toTarget.x);
                float targetZenith = Mathf.Acos(Mathf.Clamp(toTarget.y, -1f, 1f));
                float radius = orbitalFollowComponent.SphereRadius;
                
                float rotationSpeed;
                float angularSpeed;
                
                if (orbitalFollowComponent.OneFramePermanentCalculation)
                {
                    rotationSpeed = float.MaxValue;
                    angularSpeed = float.MaxValue;
                    
                    orbitalFollowComponent.OneFramePermanentCalculation = false;
                }
                else
                {
                    angularSpeed = orbitalFollowComponent.FollowSpeed / radius;
                    rotationSpeed = orbitalFollowComponent.RotationSpeed;
                }
                 
                float azimuth = Mathf.MoveTowardsAngle(
                    orbitalFollowComponent.CurrentAzimuth * Mathf.Rad2Deg,
                    targetAzimuth * Mathf.Rad2Deg,
                    angularSpeed * Mathf.Rad2Deg * Time.deltaTime
                ) * Mathf.Deg2Rad;

                float zenith = Mathf.MoveTowards(
                    orbitalFollowComponent.CurrentZenith,
                    targetZenith,
                    angularSpeed * Time.deltaTime
                );

                zenith = Mathf.Clamp(zenith, 0.01f, Mathf.PI - 0.01f);

                float sinZenith = Mathf.Sin(zenith);
                float x = radius * sinZenith * Mathf.Cos(azimuth);
                float y = radius * Mathf.Cos(zenith);
                float z = radius * sinZenith * Mathf.Sin(azimuth);

                orbitalFollowComponent.CurrentAzimuth = azimuth;
                orbitalFollowComponent.CurrentZenith = zenith;

                Transform transform = transformComponent.Transform;
                
                transform.position = centerPosition + new Vector3(x, y, z) + orbitalFollowComponent.Offset;
                transform.rotation = Quaternion.Lerp(transform.rotation, orbitalFollowComponent.Target.rotation,
                    Time.deltaTime * rotationSpeed);
            }
        }

        public void Dispose()
        {
        }
    }
}