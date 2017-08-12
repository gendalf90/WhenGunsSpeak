using Messages;
using Soldier.Head;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Server;
using System;

namespace Soldier.Network
{
    public class Receiver : MonoBehaviour //включен везде и у всех
    {
        private Observable observable;
        private Rotation headRotation;

        private void Awake()
        {
            observable = FindObjectOfType<Observable>();
            headRotation = GetComponentInChildren<Rotation>();
        }

        private void Receive(OnReceiveEvent e)
        {

        }

        private void Receive(SoldierData data) //from server (не input данные!)
        {
            if(data.Guid != Guid)
            {
                return;
            }
        }

        private void Receive(PlayerData data)  //from client (isPlayer не нужен, т.к. серверному плайеру эти данные никогда не придут)
        {
            if (data.Guid != Guid)
            {
                return;
            }
        }

        public Guid Guid { get; set; }
    }
}