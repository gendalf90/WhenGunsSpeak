using Messages;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Server
{
    class RoomsListeningState : MonoBehaviour
    {
        [SerializeField]
        private float roomsRequestPeriod;

        private Observable observable;
        private Udp udp;
        private SimpleTimer sendRequestTimer;

        private void Awake()
        {
            observable = FindObjectOfType<Observable>();
            udp = GetComponent<Udp>();
        }

        private void OnEnable()
        {
            StartSendRequestTimer();
            SubscribeAll();
        }

        private void OnDisable()
        {
            UnsubscribeAll();
        }

        private void StartSendRequestTimer()
        {
            sendRequestTimer = SimpleTimer.StartNew(roomsRequestPeriod);
        }

        private void SubscribeAll()
        {
            observable.Subscribe<StopCommand>(Stop);
            observable.Subscribe<OnRoomsEvent>(RoomsReceive);
        }

        private void UnsubscribeAll()
        {
            observable.Unsubscribe<StopCommand>(Stop);
            observable.Unsubscribe<OnRoomsEvent>(RoomsReceive);
        }

        private void Update()
        {
            SendRoomsRequestIfNeeded();
        }

        private void RoomsReceive(OnRoomsEvent e)
        {
            foreach(var room in e.Rooms)
            {
                observable.Publish(new OnRoomListenEvent(room.Session, room.Description));
            }
        }

        private void Stop(StopCommand command)
        {
            RunStopping();
            Disable();
        }

        private void Disable()
        {
            enabled = false;
        }

        private void SendRoomsRequestIfNeeded()
        {
            if(sendRequestTimer.GetItIsTimeAndRestartIfTrue())
            {
                udp.Send(new Rooms());
            }
        }

        private void RunStopping()
        {
            var state = GetComponent<StoppingState>();
            state.enabled = true;
        }
    }
}
