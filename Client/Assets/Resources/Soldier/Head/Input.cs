using Input;
using Messages;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Soldier.Head
{
    class Input : MonoBehaviour   //дизаблить этот бихейвор если не isPlayer. Управлять будет корневой soldier.
    {
        private Observable observable;
        private Rotation rotation;

        private void Awake()
        {
            observable = FindObjectOfType<Observable>();
            rotation = GetComponent<Rotation>();
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
            rotation.LookAt = e.WorldPosition;
        }
    }
}