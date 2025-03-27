using UnityEngine;

namespace EscapeRooms.Helpers
{
    public static class Vector3Extension
    {
        public static Vector3 GetXZ(this Vector3 vector3)
        {
            return new Vector3()
            {
                x = vector3.x,
                y = 0f,
                z = vector3.z
            };
        }

        public static Vector3 Clamp(this Vector3 vector3, float min, float max)
        {
            return new Vector3(
                Mathf.Clamp(vector3.x, min, max),
                Mathf.Clamp(vector3.y, min, max),
                Mathf.Clamp(vector3.z, min, max)
            );
        }
    }
}