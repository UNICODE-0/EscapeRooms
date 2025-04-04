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
                case QuaternionQuarter._1 when IsUpperQuarter && IsMirrored:
                case QuaternionQuarter._2 when IsUpperQuarter && !IsMirrored:
                    angle = 360 - euler.x;
                    break;
                case QuaternionQuarter._3 when !IsUpperQuarter && !IsMirrored:
                case QuaternionQuarter._4 when !IsUpperQuarter && IsMirrored:
                    angle = euler.x;
                    break;
            }

            return angle;
        }
    }
    
    public enum QuaternionQuarter
    {
        _1,
        _2,
        _3,
        _4
    }
}