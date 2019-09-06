using UnityEngine;

namespace Utils
{
    public static class TransformExtensions
    {
        public static void SetFlipX(this Transform transform, bool value)
        {
            var newScale = transform.localScale;

            if (IsFlipNeeded(transform.localScale.x, value))
            {
                newScale.x *= -1;
            }

            transform.localScale = newScale;
        }

        public static void SetFlipY(this Transform transform, bool value)
        {
            var newScale = transform.localScale;

            if (IsFlipNeeded(transform.localScale.y, value))
            {
                newScale.y = -1;
            }

            transform.localScale = newScale;
        }

        private static bool IsFlipNeeded(float dimensionValue, bool flipValue)
        {
            if (flipValue && dimensionValue > 0)
            {
                return true;
            }

            if (!flipValue && dimensionValue < 0)
            {
                return true;
            }

            return false;
        }
    }
}
