using Messages;
using Server;
using Shells.Factory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using FactoryShellData = Shells.Factory.ShellData;
using NetworkShellData = Shells.Network.ShellData;

namespace Shells.Network
{
    class Synchronizer : MonoBehaviour
    {
        private Observable observable;
        private NetworkShellData[] lastReceivedShells;

        public Synchronizer()
        {
            lastReceivedShells = new NetworkShellData[0];
        }

        public void Awake()
        {
            observable = FindObjectOfType<Observable>();
        }

        private void OnEnable()
        {
            observable.Subscribe<ShellsEvent>(OnReceiveInstantiatedShells);
            observable.Subscribe<OnReceiveFromServerEvent>(OnReceiveFromServerData);
        }

        private void OnDisable()
        {
            observable.Unsubscribe<ShellsEvent>(OnReceiveInstantiatedShells);
            observable.Unsubscribe<OnReceiveFromServerEvent>(OnReceiveFromServerData);
        }

        private void OnReceiveInstantiatedShells(ShellsEvent e)
        {
            SendToClients(e.Shells);
        }

        private void SendToClients(IEnumerable<FactoryShellData> data)
        {
            var networkData = data.Select(ToNetworkData).ToArray();
            var packet = new SyncData { Shells = networkData }.CreateBinaryObjectPacket();
            observable.Publish(new SendToClientsCommand(packet));
        }

        private NetworkShellData ToNetworkData(FactoryShellData data)
        {
            return new NetworkShellData
            {
                Type = data.Type,
                Guid = data.Guid,
                Position = data.Position,
                Rotation = data.Rotation
            };
        }

        private void OnReceiveFromServerData(OnReceiveFromServerEvent e)
        {
            e.Data.OfBinaryObjects<SyncData>()
                  .FirstOrDefault()
                  .Do(SynchronizeWithServerData);
        }

        private void SynchronizeWithServerData(SyncData newData)
        {
            var guidsFromNewData = newData.Shells.Select(shell => shell.Guid);
            var guidsFromOldData = lastReceivedShells.Select(shell => shell.Guid);
            var newGuids = guidsFromNewData.Except(guidsFromOldData);
            var oldGuids = guidsFromOldData.Except(guidsFromNewData);
            var newShells = newData.Shells.Join(newGuids, shell => shell.Guid, guid => guid, (shell, guid) => shell);
            var oldShells = lastReceivedShells.Join(oldGuids, shell => shell.Guid, guid => guid, (shell, guid) => shell);
            newShells.ForEach(shell => observable.Publish(new CreateShellCommand(shell.Type, shell.Guid, shell.Position, shell.Rotation)));
            oldShells.ForEach(shell => observable.Publish(new RemoveShellCommand(shell.Guid)));
            lastReceivedShells = newData.Shells;
        }
    }
}
