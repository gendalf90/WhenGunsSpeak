using Input;
using Messages;
using Server;
using Soldier.Head;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Soldier.Network
{
    class ClientSender : MonoBehaviour
    {
        private Observable observable;
        private ClientData data;

        public ClientSender()
        {
            data = new ClientData();
        }

        private void Awake()
        {
            observable = FindObjectOfType<Observable>();
        }

        private void OnEnable()
        {
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
            observable.Unsubscribe<CursorEvent>(CursorHandle);
            observable.Unsubscribe<StartRightEvent>(StartRightHandle);
            observable.Unsubscribe<StopRightEvent>(StopRightHandle);
            observable.Unsubscribe<StartLeftEvent>(StartLeftHandle);
            observable.Unsubscribe<StopLeftEvent>(StopLeftHandle);
            observable.Unsubscribe<StartJumpEvent>(StartJumpHandle);
            observable.Unsubscribe<StopJumpEvent>(StopJumpHandle);
        }

        private void CursorHandle(CursorEvent e)
        {
            data.LookAt = e.WorldPosition;
        }

        private void StartRightHandle(StartRightEvent e)
        {
            data.IsMoveRight = true;
        }

        private void StopRightHandle(StopRightEvent e)
        {
            data.IsMoveRight = false;
        }

        private void StartLeftHandle(StartLeftEvent e)
        {
            data.IsMoveLeft = true;
        }

        private void StopLeftHandle(StopLeftEvent e)
        {
            data.IsMoveLeft = false;
        }

        private void StartJumpHandle(StartJumpEvent e)
        {
            data.IsJump = true;
        }

        private void StopJumpHandle(StopJumpEvent e)
        {
            data.IsJump = false;
        }

        private void Update()
        {
            var packet = CreatePacket();
            SendToServer(packet);
        }

        private IPacket CreatePacket()
        {
            return data.CreateBinaryObjectPacket();
        }

        private void SendToServer(IPacket packet)
        {
            observable.Publish(new SendToServerCommand(packet));
        }

        public string Session
        {
            get
            {
                return data.Session;
            }
            set
            {
                data.Session = value;
            }
        }
    }
}