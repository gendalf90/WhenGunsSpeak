using UnityEngine;

namespace Weapon
{
    public class Position : MonoBehaviour
    {
        public void SetPosition(Vector2 position)
        {
            transform.position = position;
        }

        public void AimTo(Vector2 position)
        {
            var angleToRotate = transform.position.GetAngle2(position);

            transform.rotation = Quaternion.Euler(0, 0, angleToRotate);
        }

        public void SetRightLooking()
        {
            foreach (var flippable in GetComponentsInChildren<IFlippable>())
            {
                flippable.FlipToRight();
            }
        }

        public void SetLeftLooking()
        {
            foreach (var flippable in GetComponentsInChildren<IFlippable>())
            {
                flippable.FlipToLeft();
            }
        }
    }
}
