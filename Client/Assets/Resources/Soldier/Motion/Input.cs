using Input;
using Messages;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Soldier.Motion
{
    class Input : MonoBehaviour   //дизаблить этот бихейвор если не isPlayer. Управлять будет корневой soldier.
    {
        private Observable observable;
        private Movement movement;

        private void Awake()
        {
            observable = FindObjectOfType<Observable>();
            movement = GetComponent<Movement>();
        }

        private void OnEnable()
        {
            observable.Subscribe<StartRightEvent>(StartRightHandle);
            observable.Subscribe<StopRightEvent>(StopRightHandle);
            observable.Subscribe<StartLeftEvent>(StartLeftHandle);
            observable.Subscribe<StopLeftEvent>(StopLeftHandle);
            observable.Subscribe<StartJumpEvent>(JumpHandle);
        }

        private void OnDisable()
        {
            observable.Unsubscribe<StartRightEvent>(StartRightHandle);
            observable.Unsubscribe<StopRightEvent>(StopRightHandle);
            observable.Unsubscribe<StartLeftEvent>(StartLeftHandle);
            observable.Unsubscribe<StopLeftEvent>(StopLeftHandle);
            observable.Unsubscribe<StartJumpEvent>(JumpHandle);
        }

        private void StartRightHandle(StartRightEvent e)
        {
            movement.MoveRight();
        }

        private void StopRightHandle(StopRightEvent e)
        {
            movement.StopRight();
        }

        private void StartLeftHandle(StartLeftEvent e)
        {
            movement.MoveLeft();
        }

        private void StopLeftHandle(StopLeftEvent e)
        {
            movement.StopLeft();
        }

        private void JumpHandle(StartJumpEvent e)
        {
            movement.Jump();
        }
    }
}