using Input;
using Messages;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Soldier.Rotation
{
    public class Input : MonoBehaviour
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
            observable.Subscribe<CursorEvent>(CursorHandle);
        }

        private void OnDisable()
        {
            observable.Unsubscribe<CursorEvent>(CursorHandle);
        }

        private void CursorHandle(CursorEvent e)
        {
            rotation.LookAt(e.WorldPosition);
        }

        public Guid Guid { get; set; }
    }
}