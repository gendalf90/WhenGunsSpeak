using Messages;
using System.Linq;
using UniRx;
using UnityEngine;

namespace Weapon
{
    public class SoldierPositionEventReceiver : MonoBehaviour
    {
        [SerializeField]
        private string soldierId;

        private IFlippable[] flippables;

        private void Awake()
        {
            flippables = GetComponentsInChildren<IFlippable>();
        }

        private void OnEnable()
        {
            MessageBroker.Default
                .Receive<SoldierPositionEvent>()
                .Where(message => message.SoldierId == soldierId)
                .Do(SetPosition)
                .Do(SetFlip)
                .TakeUntilDisable(this)
                .Subscribe();
        }

        private void SetPosition(SoldierPositionEvent message)
        {
            var angleToRotate = message.Position.GetAngle(message.LookingPoint);

            transform.position = message.Position;
            transform.rotation = Quaternion.Euler(0, 0, angleToRotate);
        }

        private void SetFlip(SoldierPositionEvent message)
        {
            foreach (var flippable in flippables)
            {
                SetFlip(flippable, message);
            }
        }

        private void SetFlip(IFlippable flippable, SoldierPositionEvent message)
        {
            if (message.IsRightLooking)
            {
                flippable.FlipToRight();
            }

            if (message.IsLeftLooking)
            {
                flippable.FlipToLeft();
            }
        }
    }
}
