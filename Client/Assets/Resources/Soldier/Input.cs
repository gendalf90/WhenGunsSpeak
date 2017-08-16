using Input;
using Messages;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HeadRotation = Soldier.Head.Rotation;
using BodyRotation = Soldier.Body.Rotation;

namespace Soldier
{
    class Input : MonoBehaviour   //дизаблить этот бихейвор если не isPlayer. Управлять будет корневой soldier. Или вообще удалить; подписка будет сразу в soldier.
    {
        private Observable observable;
        private HeadRotation headRotation;
        private BodyRotation bodyRotation;

        private void Awake()
        {
            observable = FindObjectOfType<Observable>();
            headRotation = GetComponentInChildren<HeadRotation>();
            bodyRotation = GetComponentInChildren<BodyRotation>();
        }

        private void OnEnable()
        {
            observable.Subscribe<MouseEvent>(MouseHandle);
        }

        private void OnDisable()
        {
            observable.Unsubscribe<MouseEvent>(MouseHandle);
        }

        private void MouseHandle(MouseEvent e)
        {
            headRotation.LookAt = e.WorldPosition;
            bodyRotation.Aim = e.WorldPosition;
        }
    }
}