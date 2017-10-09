using Messages;
using Server;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Soldier.Factory
{
    class Synchronizer : MonoBehaviour
    {
        private Observable observable;
        private HashSet<Guid> instantiated;

        public Synchronizer()
        {
            instantiated = new HashSet<Guid>();
        }

        private void Awake()
        {
            observable = FindObjectOfType<Observable>();
        }

        private void OnEnable()
        {
            observable.Subscribe<SoldierCreatedEvent>(OnSoldierCreate);
            observable.Subscribe<SoldierRemovedEvent>(OnSoldierRemove);
            observable.Subscribe<OnReceiveFromServerEvent>(OnReceiveFromServerData);
        }

        private void OnSoldierCreate(SoldierCreatedEvent e)
        {
            instantiated.Add(e.Guid);
        }

        private void OnSoldierRemove(SoldierRemovedEvent e)
        {
            instantiated.Remove(e.Guid);
        }

        private void Update()
        {
            var data = new FactoryData { InstantiatedGuids = instantiated.ToArray() };
            observable.Publish(new SendToClientsCommand(data.CreateBinaryObjectPacket()));
        }

        private void OnReceiveFromServerData(OnReceiveFromServerEvent e)
        {
            e.Data.OfBinaryObjects<FactoryData>()
                  .FirstOrDefault()
                  .Do(SynchronizeWithServerData);
        }

        private void SynchronizeWithServerData(FactoryData data)
        {
            data.InstantiatedGuids.Except(instantiated)
                                  .Select(guid => new CreateSoldierCommand(guid))
                                  .ToList()
                                  .ForEach(observable.Publish);
            instantiated.Except(data.InstantiatedGuids)
                        .Select(guid => new RemoveSoldierCommand(guid))
                        .ToList()
                        .ForEach(observable.Publish);
        }

        private void OnDisable()
        {
            observable.Unsubscribe<SoldierCreatedEvent>(OnSoldierCreate);
            observable.Unsubscribe<SoldierRemovedEvent>(OnSoldierRemove);
            observable.Unsubscribe<OnReceiveFromServerEvent>(OnReceiveFromServerData);
        }
    }
}