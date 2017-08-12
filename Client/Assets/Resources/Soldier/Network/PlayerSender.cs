using Messages;
using Server;
using Soldier.Head;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Soldier.Network
{
    class PlayerSender : MonoBehaviour //из клиента плайера на сервер. у сервера плайера (сервер всегда плайер) отключено.
    {
        private Observable observable;
        private Rotation headRotation;

        private void Awake()
        {
            observable = FindObjectOfType<Observable>();
            headRotation = GetComponentInChildren<Rotation>();
        }

        private void Update()
        {
            var packet = CreatePacket();
            SendToServer(packet);
        }

        private IPacket CreatePacket()
        {
            //return new PlayerData { Guid = Guid }.CreateBinaryObjectPacket();
            return null; //возвращать состояние input данных (aimto рук тоже отправляется в конечном результате как у head)
        }

        private void SendToServer(IPacket packet)
        {
            observable.Publish(new SendToServerCommand(packet));
        }

        public Guid Guid { get; set; }
    }
}