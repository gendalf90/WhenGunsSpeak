using Input;
using Messages;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Soldier.Legs
{
    class Input : MonoBehaviour
    {
        private Observable observable;
        private Animation legsAnimation;

        private void Awake()
        {
            observable = FindObjectOfType<Observable>();
            legsAnimation = GetComponentInChildren<Animation>();
        }

        private void OnEnable()
        {
            observable.Subscribe<StartRightEvent>(StartRightHandle);
            observable.Subscribe<StopRightEvent>(StopRightHandle);
            observable.Subscribe<StartLeftEvent>(StartLeftHandle);
            observable.Subscribe<StopLeftEvent>(StopLeftHandle);
        }

        private void OnDisable()
        {
            observable.Unsubscribe<StartRightEvent>(StartRightHandle);
            observable.Unsubscribe<StopRightEvent>(StopRightHandle);
            observable.Unsubscribe<StartLeftEvent>(StartLeftHandle);
            observable.Unsubscribe<StopLeftEvent>(StopLeftHandle);
        }

        private void StartRightHandle(StartRightEvent e)
        {
            legsAnimation.IsRightMoving = true;
        }

        private void StopRightHandle(StopRightEvent e)
        {
            legsAnimation.IsRightMoving = false;
        }

        private void StartLeftHandle(StartLeftEvent e)
        {
            legsAnimation.IsLeftMoving = true;
        }

        private void StopLeftHandle(StopLeftEvent e)
        {
            legsAnimation.IsLeftMoving = false;
        }
    }
}