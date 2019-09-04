using UnityEngine;

namespace Soldier
{
    public class Looking : MonoBehaviour
    {
        private Vector2 lookingPoint = Vector2.zero;

        private ILookable[] lookables;
        private IFlippable[] flippables;

        private void Awake()
        {
            lookables = GetComponentsInChildren<ILookable>();
            flippables = GetComponentsInChildren<IFlippable>();
        }

        public void LookToPoint(Vector2 point)
        {
            lookingPoint = point;

            SetLooking();
            SetFlip();
        }

        private void SetLooking()
        {
            foreach(var lookable in lookables)
            {
                lookable.LookToPoint(lookingPoint);
            }
        }

        private void SetFlip()
        {
            foreach(var flippable in flippables)
            {
                SetFlip(flippable);
            }
        }

        private void SetFlip(IFlippable flippable)
        {
            if(IsRightLooking)
            {
                flippable.FlipToRight();
            }

            if(IsLeftLooking)
            {
                flippable.FlipToLeft();
            }
        }

        private bool IsRightLooking
        {
            get => transform.position.x < lookingPoint.x;
        }

        private bool IsLeftLooking
        {
            get => transform.position.x >= lookingPoint.x;
        }
    }
}
