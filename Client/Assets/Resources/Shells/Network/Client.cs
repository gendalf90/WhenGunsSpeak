using Messages;
using Server;
using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Shells
{
    class Client : MonoBehaviour
    {
        private Observable observable;

        private void Awake()
        {
            observable = FindObjectOfType<Observable>();
        }

        private void OnEnable()
        {
            observable.Subscribe<OnReceiveEvent>(ReceiveData);
        }

        private void OnDisable()
        {
            observable.Unsubscribe<OnReceiveEvent>(ReceiveData);
        }

        private void ReceiveData(OnReceiveEvent e)
        {
            e.Data.OfBinaryObjects<ShellData>()
                  .FirstOrDefault(data => data.Guid == Guid)
                  .Do(ApplyData);
        }

        private void ApplyData(ShellData data)
        {
            transform.position = data.Position;
        }

        public Guid Guid { get; set; }
    }
}