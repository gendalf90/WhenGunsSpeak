using Messages;
using Server;
using Soldier.Head;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HeadRotation = Soldier.Head.Rotation;
using BodyRotation = Soldier.Body.Rotation;
using Soldier.Motion;
using Soldier.Rotation;
using Input;
using System.Linq;
using Soldier.Factory;

namespace Soldier.Network
{
    class ServerSender : MonoBehaviour //из сервера на клиенты. включено у сервера.
    {
        private Observable observable;

        private ServerData data;

        private ServerSender()
        {
            data = new ServerData { Items = new Dictionary<string, ServerDataItem>() };
        }

        private void Awake()
        {
            observable = FindObjectOfType<Observable>();
        }

        private void OnEnable()
        {
            observable.Subscribe<OnReceiveFromClientEvent>(ReceiveFromClient);
            observable.Subscribe<PositionEvent>(PositionHandler);
            observable.Subscribe<SoldierCreatedEvent>(SoldierCreatedHandler);
            observable.Subscribe<SoldierRemovedEvent>(SoldierRemovedHandler);
            observable.Subscribe<CursorEvent>(CursorHandle);
            observable.Subscribe<StartRightEvent>(StartRightHandle);
            observable.Subscribe<StopRightEvent>(StopRightHandle);
            observable.Subscribe<StartLeftEvent>(StartLeftHandle);
            observable.Subscribe<StopLeftEvent>(StopLeftHandle);
            observable.Subscribe<StartJumpEvent>(StartJumpHandle);
            observable.Subscribe<StopJumpEvent>(StopJumpHandle);
        }

        private void OnDisable()
        {
            observable.Unsubscribe<OnReceiveFromClientEvent>(ReceiveFromClient);
            observable.Unsubscribe<PositionEvent>(PositionHandler);
            observable.Unsubscribe<SoldierCreatedEvent>(SoldierCreatedHandler);
            observable.Unsubscribe<SoldierRemovedEvent>(SoldierRemovedHandler);
            observable.Unsubscribe<CursorEvent>(CursorHandle);
            observable.Unsubscribe<StartRightEvent>(StartRightHandle);
            observable.Unsubscribe<StopRightEvent>(StopRightHandle);
            observable.Unsubscribe<StartLeftEvent>(StartLeftHandle);
            observable.Unsubscribe<StopLeftEvent>(StopLeftHandle);
            observable.Unsubscribe<StartJumpEvent>(StartJumpHandle);
            observable.Unsubscribe<StopJumpEvent>(StopJumpHandle);
        }

        private void SoldierCreatedHandler(SoldierCreatedEvent e)
        {
            data.Items[e.Session] = new ServerDataItem { Session = e.Session };
        }

        private void SoldierRemovedHandler(SoldierRemovedEvent e)
        {
            data.Items.Remove(e.Session);
        }

        private void PositionHandler(PositionEvent e)
        {
            UpdateDataIfExist(e.Session, data => data.Position = e.Position);
        }

        private void CursorHandle(CursorEvent e)
        {
            UpdatePlayerIfExist(player => player.LookAt = e.WorldPosition);
        }

        private void StartRightHandle(StartRightEvent e)
        {
            UpdatePlayerIfExist(player => player.IsMoveRight = true);
        }

        private void StopRightHandle(StopRightEvent e)
        {
            UpdatePlayerIfExist(player => player.IsMoveRight = false);
        }

        private void StartLeftHandle(StartLeftEvent e)
        {
            UpdatePlayerIfExist(player => player.IsMoveLeft = true);
        }

        private void StopLeftHandle(StopLeftEvent e)
        {
            UpdatePlayerIfExist(player => player.IsMoveLeft = false);
        }

        private void StartJumpHandle(StartJumpEvent e)
        {
            UpdatePlayerIfExist(player => player.IsJump = true);
        }

        private void StopJumpHandle(StopJumpEvent e)
        {
            UpdatePlayerIfExist(player => player.IsJump = false);
        }

        private void ReceiveFromClient(OnReceiveFromClientEvent e)
        {
            e.Data.OfBinaryObjects<ClientData>().ForEach(SynchronizeClientData);
        }

        private void SynchronizeClientData(ClientData clientData)
        {
            UpdateDataIfExist(clientData.Session, serverData =>
            {
                serverData.LookAt = clientData.LookAt;
                serverData.IsMoveRight = clientData.IsMoveRight;
                serverData.IsMoveLeft = clientData.IsMoveLeft;
                serverData.IsJump = clientData.IsJump;
            });
        }

        private void UpdatePlayerIfExist(Action<ServerDataItem> update)
        {
            UpdateDataIfExist(Session, update);
        }

        private void UpdateDataIfExist(string session, Action<ServerDataItem> update)
        {
            ServerDataItem serverPlayer;
            if (data.Items.TryGetValue(session, out serverPlayer))
            {
                update(serverPlayer);
            }
        }

        private void Update()
        {
            var packet = CreatePacket();
            SendToClients(packet);
        }

        private IPacket CreatePacket()
        {
            return data.CreateBinaryObjectPacket();
        }

        private void SendToClients(IPacket packet)
        {
            observable.Publish(new SendToClientsCommand(packet));
        }

        public string Session { get; set; }
    }
}
