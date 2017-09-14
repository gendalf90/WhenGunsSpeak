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
            udp = FindObjectOfType<Udp>();
        }

        private void Start()
        {
            StartSendRequestTimer();
            SubscribeAll();
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
            UnsubscribeAll();
            RunStopping();
            Destroy(gameObject);
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
            Instantiate(Resources.Load<GameObject>("Server/States/Stopping"));
        }
    }
}
