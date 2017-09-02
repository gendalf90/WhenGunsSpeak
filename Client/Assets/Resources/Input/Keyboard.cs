using Messages;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityInput = UnityEngine.Input;

namespace Input
{
    class Keyboard : MonoBehaviour
    {
        private Observable observable;

        private bool currentRightState;
        private bool lastRightState;
        private bool currentLeftState;
        private bool lastLeftState;

        private void Awake()
        {
            observable = FindObjectOfType<Observable>();
        }

        private void Update()
        {
            UpdateRightAndLeft();
            UpdateJump();
        }

        private void UpdateRightAndLeft()
        {
            InitializeCurrentRightAndLeftState();
            UpdateRightAndLeftEvents();
            UpdateLastRightAndLeftState();
        }

        private void InitializeCurrentRightAndLeftState()
        {
            var rightPressed = UnityInput.GetAxisRaw("Horizontal") > 0;
            var leftPressed = UnityInput.GetAxisRaw("Horizontal") < 0;
            currentRightState = rightPressed && !leftPressed;
            currentLeftState = leftPressed && !rightPressed;
        }

        private void UpdateRightAndLeftEvents()
        {
            if (IsStart(lastRightState, currentRightState))
            {
                observable.Publish(new StartRightEvent());
            }

            if(IsStop(lastRightState, currentRightState))
            {
                observable.Publish(new StopRightEvent());
            }

            if (IsStart(lastLeftState, currentLeftState))
            {
                observable.Publish(new StartLeftEvent());
            }

            if (IsStop(lastLeftState, currentLeftState))
            {
                observable.Publish(new StopLeftEvent());
            }
        }

        private void UpdateLastRightAndLeftState()
        {
            lastRightState = currentRightState;
            lastLeftState = currentLeftState;
        }
        
        private bool IsStart(bool lastState, bool currentState)
        {
            return !lastState && currentState;
        }

        private bool IsStop(bool lastState, bool currentState)
        {
            return lastState && !currentState;
        }

        private void UpdateJump()
        {
            var isPressed = UnityInput.GetButtonDown("Jump");

            if (isPressed)
            {
                observable.Publish(new JumpEvent());
            }
        }
    }
}