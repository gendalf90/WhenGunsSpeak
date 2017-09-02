using UnityEngine;
using Messages;
using System;

namespace Soldier.Rotation
{
    class Rotation : MonoBehaviour
    {
        private Observable observable;

        private Vector2 toPosition;

        private void Awake()
        {
            observable = FindObjectOfType<Observable>();
        }

        private void OnEnable()
        {
            observable.Subscribe<LookCommand>(LookHandler);
        }

        private void OnDisable()
        {
            observable.Unsubscribe<LookCommand>(LookHandler);
        }

        private void LookHandler(LookCommand command)
        {
            if(command.Guid != Guid)
            {
                return;
            }

            LookAt(command.Position);
        }

        public void LookAt(Vector2 position)
        {
            SetLookPosition(position);
            PublishLookEvent();
        }

        private void SetLookPosition(Vector2 position)
        {
            toPosition = position;
        }

        private void PublishLookEvent()
        {
            observable.Publish(new LookEvent(Guid, toPosition, LookDirection));
        }

        public Guid Guid { get; set; }

        private LookDirection LookDirection
        {
            get
            {
                if (Angle <= 90 || Angle >= 270)
                {
                    return LookDirection.Right;
                }
                else
                {
                    return LookDirection.Left;
                }
            }
        }

        private float Angle
        {
            get
            {
                return Position.GetAngle(toPosition);
            }
        }

        private Vector2 Position
        {
            get
            {
                return transform.position;
            }
        }
    }
}