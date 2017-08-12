using UnityEngine;
using System;
using System.Linq;
using System.Collections.Generic;
using Server;
using Network;
using Messages;

namespace Legs
{
    public class CreateLegsMessage
    {
        public CreateLegsMessage(Guid instanceId, Guid playerSessionId, Guid parentId)
        {
            InstanceId = instanceId;
            PlayerSessionId = playerSessionId;
            ParentId = parentId;
        }

        public Guid InstanceId { get; private set; }

        public Guid ParentId { get; private set; }

        public Guid PlayerSessionId { get; private set; }
    }

    public class RemoveLegsMessage
    {
        public RemoveLegsMessage(Guid instanceId)
        {
            InstanceId = instanceId;
        }

        public Guid InstanceId { get; private set; }
    }

    class LegsFactory : ClientServerMonoBehaviour
    {
        private GameObject prefab;
        private Dictionary<Guid, Legs> instantiated;

        public LegsFactory()
        {
            instantiated = new Dictionary<Guid, Legs>();
        }

        protected override void Awake()
        {
            base.Awake();

            prefab = Resources.Load<GameObject>("Prefabs/Soldier/Legs/LegsPrefab");
        }

        protected override void OnMessageHandle(MessageEventArgs args)
        {
            args.Message.As<CreateLegsMessage>().Do(message =>
            {
                if(!instantiated.ContainsKey(message.InstanceId))
                {
                    var data = new InstanceData(message.InstanceId, message.PlayerSessionId, message.ParentId);
                    Instantiate(data);
                }
            });

            args.Message.As<RemoveLegsMessage>().Do(message =>
            {
                Destroy(message.InstanceId);
            });
        }

        private void Instantiate(InstanceData data)
        {
            if (prefab != null)
            {
                var newObject = GameObject.Instantiate(prefab);
                var script = newObject.GetComponent<Legs>();
                script.Id = data.InstanceId;
                script.ParentId = data.ParentId;
                script.PlayerSessionId = data.PlayerSessionId;
                instantiated.Add(data.InstanceId, script);
            }
        }

        private void Destroy(Guid instanceId)
        {
            Legs toDestroy;
            if (instantiated.TryGetValue(instanceId, out toDestroy))
            {
                GameObject.Destroy(toDestroy.gameObject);
                instantiated.Remove(instanceId);
            }
        }

        protected override void ServerSend(SendEventArgs args)
        {
            args.Send(new FactoryData { Instantiated = instantiated.Values.Select(x => CreateInstaceData(x)).ToArray() }, GetClientsIPEndPoints());
        }

        protected override void ClientReceive(ReceiveEventArgs args)
        {
            var data = args.GetTypedBinaryDataOfType<FactoryData>().FirstOrDefault();

            if (data != null)
            {
                data.Instantiated.Except(instantiated.Values.Select(x => CreateInstaceData(x)))
                                 .ToList()
                                 .ForEach(Instantiate);
                instantiated.Values.Select(x => CreateInstaceData(x))
                                   .Except(data.Instantiated)
                                   .Select(instance => instance.InstanceId)
                                   .ToList()
                                   .ForEach(Destroy);
            }
        }

        private InstanceData CreateInstaceData(Legs legs)
        {
            return new InstanceData(legs.Id, legs.PlayerSessionId, legs.ParentId);
        }

        private struct InstanceData
        {
            public InstanceData(Guid instanceId, Guid playerSessionId, Guid parentId) : this()
            {
                InstanceId = instanceId;
                PlayerSessionId = playerSessionId;
                ParentId = parentId;
            }

            public Guid InstanceId { get; private set; }

            public Guid ParentId { get; private set; }

            public Guid PlayerSessionId { get; private set; }
        }

        [PacketTypeGuid("CD11ED37-E570-44CE-A456-E4B9D254591A")]
        private class FactoryData : ITypedBinaryData
        {
            public InstanceData[] Instantiated { get; set; }

            public byte[] ToBytes()
            {
                var builder = new BinaryDataBuilder().Write(Instantiated.Length);

                foreach (var data in Instantiated)
                {
                    builder.Write(data.InstanceId)
                           .Write(data.PlayerSessionId)
                           .Write(data.ParentId);
                }

                return builder.Build();
            }

            public void FromBytes(byte[] bytes)
            {
                using (var reader = new BinaryDataReader(bytes))
                {
                    Instantiated = new InstanceData[reader.ReadInt()];

                    for (int i = 0; i < Instantiated.Length; i++)
                    {
                        Instantiated[i] = new InstanceData(reader.ReadGuid(), reader.ReadGuid(), reader.ReadGuid());
                    }
                }
            }
        }
    }
}