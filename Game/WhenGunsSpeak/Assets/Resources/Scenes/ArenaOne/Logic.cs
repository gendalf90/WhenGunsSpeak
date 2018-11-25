﻿using Configuration;
using Messages;
using Server;
using System;
using UnityEngine;

namespace Rooms.ArenaOne
{
    class Logic : MonoBehaviour
    {
        private Observable observable;
        private Parameters parameters;

        private void Awake()
        {
            observable = FindObjectOfType<Observable>();
            parameters = FindObjectOfType<Parameters>();
            observable.Subscribe<OnConnectedToRoomsServiceEvent>(ConnectionIsReadyHandler);
            observable.Subscribe<OnNewRoomStartedEvent>(OnNewRoomHasStartedHandler);
        }

        private void Start()
        {
            observable.Publish(new ConnectToRoomsServiceCommand());
        }

        private void ConnectionIsReadyHandler(OnConnectedToRoomsServiceEvent e)
        {
            var myLogin = parameters.GetLocalOrDefault<string>("Login");
            var ownerId = parameters.GetLocalOrDefault<Guid?>("RoomOwnerId");

            if(ownerId.HasValue)
            {
               
            }
            else
            {
                observable.Publish(new StartNewRoomCommand($"ArenaOne@{myLogin}"));
            }
        }

        private void OnNewRoomHasStartedHandler(OnNewRoomStartedEvent e)
        {
            
        }
    }
}