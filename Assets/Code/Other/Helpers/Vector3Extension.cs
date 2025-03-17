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
    }
}