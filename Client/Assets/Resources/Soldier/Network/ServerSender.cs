using Messages;
using Server;
using Soldier.Head;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Soldier.Network
{
    class ServerSender : MonoBehaviour //из сервера на клиенты. включено у сервера.
    {
        private Observable observable;

        private void Awake()
        {
            observable = FindObjectOfType<Observable>();
        }

        private void Update()
        {
            var packet = CreatePacket();
            SendToClients(packet);
        }

        private IPacket CreatePacket()
        {
            //return new SoldierData { Guid = Guid }.CreateBinaryObjectPacket();
            return null; //возвращать состояние не input данных, т.к. каждый клиент это player, ему их применять нельзя.
        }

        private void SendToClients(IPacket packet)
        {
            observable.Publish(new SendToClientsCommand(packet));
        }

        public Guid Guid { get; set; }
    }
}
