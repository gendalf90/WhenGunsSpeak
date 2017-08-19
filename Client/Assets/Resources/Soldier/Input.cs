using Input;
using Messages;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Soldier
{
    class Input : MonoBehaviour   //дизаблить этот бихейвор если не isPlayer. Управлять будет корневой soldier.
    {
        private Observable observable;
        private Rotation rotation;
        private Motion motion;

        private void Awake()
        {
            observable = FindObjectOfType<Observable>();
            rotation = GetComponent<Rotation>();
            motion = GetComponent<Motion>();
        }

        private void OnEnable()
        {
            observable.Subscribe<CursorEvent>(CursorHandle);
            observable.Subscribe<StartRightEvent>(StartRightHandle);
            observable.Subscribe<StopRightEvent>(StopRightHandle);
            observable.Subscribe<StartLeftEvent>(StartLeftHandle);
            observable.Subscribe<StopLeftEvent>(StopLeftHandle);
            observable.Subscribe<JumpEvent>(JumpHandle);
        }

        private void OnDisable()
        {
            observable.Unsubscribe<CursorEvent>(CursorHandle);
            observable.Unsubscribe<StartRightEvent>(StartRightHandle);
            observable.Unsubscribe<StopRightEvent>(StopRightHandle);
            observable.Unsubscribe<StartLeftEvent>(StartLeftHandle);
            observable.Unsubscribe<StopLeftEvent>(StopLeftHandle);
            observable.Unsubscribe<JumpEvent>(JumpHandle);
        }

        private void CursorHandle(CursorEvent e)
        {
            rotation.ToPosition = e.WorldPosition;
        }

        private void StartRightHandle(StartRightEvent e)
        {
            motion.MoveRight();
        }

        private void StopRightHandle(StopRightEvent e)
        {
            motion.StopRight();
        }

        private void StartLeftHandle(StartLeftEvent e)
        {
            motion.MoveLeft();
        }

        private void StopLeftHandle(StopLeftEvent e)
        {
            motion.StopLeft();
        }

        private void JumpHandle(JumpEvent e)
        {
            motion.Jump();
        }
    }
}