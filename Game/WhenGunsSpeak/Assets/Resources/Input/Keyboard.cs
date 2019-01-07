using Messages;
using UnityEngine;
using UnityInput = UnityEngine.Input;

namespace Input
{
    public class Keyboard : MonoBehaviour
    {
        private Observable observable;

        private bool currentRightState;
        private bool lastRightState;
        private bool currentLeftState;
        private bool lastLeftState;
        private bool currentJumpState;
        private bool lastJumpState;

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
        
        private void UpdateJump()
        {
            UpdateCurrentJumpState();
            UpdateJumpEvents();
            UpdateLastJumpState();
        }

        private void UpdateCurrentJumpState()
        {
            currentJumpState = UnityInput.GetButton("Jump");
        }

        private void UpdateJumpEvents()
        {
            if(IsStart(lastJumpState, currentJumpState))
            {
                observable.Publish(new StartJumpEvent());
            }

            if(IsStop(lastJumpState, currentJumpState))
            {
                observable.Publish(new StopJumpEvent());
            }
        }

        private void UpdateLastJumpState()
        {
            lastJumpState = currentJumpState;
        }

        private bool IsStart(bool lastState, bool currentState)
        {
            return !lastState && currentState;
        }

        private bool IsStop(bool lastState, bool currentState)
        {
            return lastState && !currentState;
        }
    }
}