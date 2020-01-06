using UnityEngine;

namespace Utils
{
    public static class Vector2Extensions
    {
        public static float GetAngle2(this Vector2 from, Vector2 to)
        {
            return Mathf.Atan2(from.y - to.y, from.x - to.x) * Mathf.Rad2Deg + 180;
        }

        public static float GetAngle2(this Vector3 from, Vector2 to)
        {
            return Mathf.Atan2(from.y - to.y, from.x - to.x) * Mathf.Rad2Deg + 180;
        }
    }
}
