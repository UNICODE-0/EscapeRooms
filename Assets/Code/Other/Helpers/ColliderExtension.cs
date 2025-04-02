using UnityEngine;

namespace EscapeRooms.Helpers
{
    public static class ColliderExtension
    {
        public static float GetMinDistanceToClosestPoints(Collider[] colliders, Vector3 targetPosition)
        {
            float currentMinDistance = float.MaxValue;
            
            foreach (var collider in colliders)
            {
                float currentDistance = Vector3.Distance(targetPosition, collider.ClosestPointOnBounds(targetPosition));

                if (currentMinDistance > currentDistance)
                    currentMinDistance = currentDistance;
            }

            return currentMinDistance;
        }
    }
}