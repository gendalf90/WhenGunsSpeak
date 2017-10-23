using Messages;
using Soldier.Head;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Server;
using System;
using Soldier.Motion;
using Soldier.Rotation;
using Soldier.Factory;
using System.Linq;
using Soldier.Legs;

namespace Soldier.Network
{
    public class Receiver : MonoBehaviour
    {
        private Observable observable;
        private HashSet<string> instantiated;

        public Receiver()
        {
            instantiated = new HashSet<string>();
        }

        private void Awake()
        {
            observable = FindObjectOfType<Observable>();
        }

        private void OnEnable()
        {
            observable.Subscribe<OnReceiveFromClientEvent>(ReceiveFromClientData);
            observable.Subscribe<SoldierCreatedEvent>(OnSoldierCreated);
            observable.Subscribe<SoldierRemovedEvent>(OnSoldierRemoved);
            observable.Subscribe<OnReceiveFromServerEvent>(ReceiveServerData);
        }

        private void OnDisable()
        {
            observable.Unsubscribe<OnReceiveFromClientEvent>(ReceiveFromClientData);
            observable.Unsubscribe<SoldierCreatedEvent>(OnSoldierCreated);
            observable.Unsubscribe<SoldierRemovedEvent>(OnSoldierRemoved);
            observable.Unsubscribe<OnReceiveFromServerEvent>(ReceiveServerData);
        }

        private void OnSoldierCreated(SoldierCreatedEvent e)
        {
            instantiated.Add(e.Session);
        }

        private void OnSoldierRemoved(SoldierRemovedEvent e)
        {
            instantiated.Remove(e.Session);
        }

        private void ReceiveFromClientData(OnReceiveFromClientEvent e)
        {
            e.Data.OfBinaryObjects<ClientData>()
                  .ForEach(ProcessClientData);
        }

        private void ProcessClientData(ClientData data)
        {
            observable.Publish(new MoveCommand(data.Session, data.IsMoveRight, data.IsMoveLeft, data.IsJump));
            observable.Publish(new AnimationCommand(data.Session, GetAnimationType(data.IsMoveRight, data.IsMoveLeft)));
            observable.Publish(new LookCommand(data.Session, data.LookAt));
        }

        private void ReceiveServerData(OnReceiveFromServerEvent e)
        {
            e.Data.OfBinaryObjects<ServerData>()
                  .ForEach(ProcessServerData);
        }

        private void ProcessServerData(ServerData data)
        {
            SynchronizeServerData(data);
            UpdateServerData(data);
        }

        private void SynchronizeServerData(ServerData data)
        {
            data.Items.Keys.Except(instantiated)
                           .Select(session => new CreateSoldierCommand(session))
                           .ToList()
                           .ForEach(observable.Publish);
            instantiated.Except(data.Items.Keys)
                        .Select(session => new RemoveSoldierCommand(session))
                        .ToList()
                        .ForEach(observable.Publish);
        }

        private void UpdateServerData(ServerData data)
        {
            data.Items.Values.ForEach(UpdateServerDataItem);
        }

        private void UpdateServerDataItem(ServerDataItem item)
        {
            observable.Publish(new SetSoldierPositionCommand(item.Session, item.Position));
            observable.Publish(new AnimationCommand(item.Session, GetAnimationType(item.IsMoveRight, item.IsMoveLeft)));
            observable.Publish(new LookCommand(item.Session, item.LookAt));
        }

        private AnimationType GetAnimationType(bool isMoveRight, bool isMoveLeft)
        {
            if(isMoveLeft)
            {
                return AnimationType.MoveLeft;
            }
            else if(isMoveRight)
            {
                return AnimationType.MoveRight;
            }
            else
            {
                return AnimationType.Stop;
            }
        }

        public string Session { get; set; }
    }
}