using UnityEngine;

namespace EscapeRooms.Helpers
{
    public static class QuaternionExtension
    {
        public static float GetXAxisAngleInQuarter(this Quaternion quaternion, QuaternionQuarter quaternionQuarter)
        {
            Vector3 euler = quaternion.eulerAngles;
            float angle = -1;

            bool IsUpperQuarter = euler.x >= 270;
            bool IsMirrored = euler.y > 179f && euler.y < 359f;

            switch (quaternionQuarter)
            {
                case QuaternionQuarter.First when IsUpperQuarter && IsMirrored:
                case QuaternionQuarter.Second when IsUpperQuarter && !IsMirrored:
                    angle = 360 - euler.x;
                    break;
                case QuaternionQuarter.Third when !IsUpperQuarter && !IsMirrored:
                case QuaternionQuarter.Fourth when !IsUpperQuarter && IsMirrored:
                    angle = euler.x;
                    break;
            }

            return angle;
        }
    }
    
    public enum QuaternionQuarter
    {
        First,
        Second,
        Third,
        Fourth
    }
}