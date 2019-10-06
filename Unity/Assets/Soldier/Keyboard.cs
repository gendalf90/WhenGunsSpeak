using UnityEngine;

namespace Soldier
{
    public class Keyboard : MonoBehaviour
    {
        private Movement movement;

        private void Awake()
        {
            movement = GetComponentInParent<Movement>();
        }

        private void FixedUpdate()
        {
            var verticalDirection = Input.GetAxisRaw("Vertical");
            var horizontalDirection = Input.GetAxisRaw("Horizontal");
            var relativeOfCurrentPositionDirection = new Vector2(horizontalDirection, verticalDirection);
            var currentPosition = new Vector2(transform.position.x, transform.position.y);
            var destinationPoint = currentPosition + relativeOfCurrentPositionDirection;
            var isStop = relativeOfCurrentPositionDirection == Vector2.zero;

            if (!isStop)
            {
                movement.MoveToPoint(destinationPoint);
            }
            else
            {
                movement.StopMoving();
            }
        }
    }
}
