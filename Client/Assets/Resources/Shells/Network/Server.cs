using Messages;
using Server;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Shells
{
    class Server : MonoBehaviour
    {
        private Observable observable;

        private void Awake()
        {
            observable = FindObjectOfType<Observable>();
        }

        private void Update()
        {
            var data = new ShellData { Guid = Guid, Position = Position };
            var packet = data.CreateBinaryObjectPacket();
            observable.Publish(new SendToClientsCommand(packet));
        }

        private Vector2 Position
        {
            get
            {
                return transform.position;
            }
        }

        public Guid Guid { get; set; }
    }
}