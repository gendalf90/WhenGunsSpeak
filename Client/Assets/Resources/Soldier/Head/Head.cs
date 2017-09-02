using Messages;
using Soldier.Rotation;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Soldier.Head
{
    public class Head : MonoBehaviour
    {
        private Observable observable;

        private Flip flip;
        private Rotation rotation;

        private void Awake()
        {
            observable = FindObjectOfType<Observable>();
            flip = GetComponent<Flip>();
            rotation = GetComponent<Rotation>();
        }

        private void OnEnable()
        {
            observable.Subscribe<LookEvent>(LookHandle);
        }

        private void OnDisable()
        {
            observable.Unsubscribe<LookEvent>(LookHandle);
        }

        private void LookHandle(LookEvent e)
        {
            if (e.Guid != Guid)
            {
                return;
            }

            rotation.LookAt = e.Position;

            if (e.Direction == LookDirection.Left)
            {
                flip.SetLookAtLeft();
            }

            if (e.Direction == LookDirection.Right)
            {
                flip.SetLookAtRight();
            }
        }

        public Guid Guid { get; private set; }
    }
}