using Messages;
using System;
using System.Linq;
using UniRx;
using UnityEngine;

namespace Weapon
{
    public class SoldierPositionEventReceiver : MonoBehaviour
    {
        private Identificator identificator;
        private IFlippable[] flippables;
        private IDisposable unsubscriber;

        private void Awake()
        {
            identificator = GetComponent<Identificator>();
            flippables = GetComponentsInChildren<IFlippable>();
        }

        private void OnEnable()
        {
            unsubscriber = MessageBroker.Default
                .Receive<SoldierPositionEvent>()
                .Where(message => message.SoldierId == identificator.SoldierId)
                .Do(SetPosition)
                .Do(SetFlip)
                .Subscribe();
        }

        private void OnDisable()
        {
            unsubscriber.Dispose();
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
