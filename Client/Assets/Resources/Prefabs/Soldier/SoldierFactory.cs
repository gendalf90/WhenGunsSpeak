//using UnityEngine;
//using System.Collections;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using Server;
//using Network;
//using Messages;

//namespace Soldier
//{
//    public class CreateSoldierMessage
//    {
//        public CreateSoldierMessage(Guid instanceId, Guid playerSessionId)
//        {
//            InstanceId = instanceId;
//            PlayerSessionId = playerSessionId;
//        }

//        public Guid InstanceId { get; private set; }

//        public Guid PlayerSessionId { get; private set; }
//    }

//    public class RemoveSoldierMessage
//    {
//        public RemoveSoldierMessage(Guid instanceId)
//        {
//            InstanceId = instanceId;
//        }

//        public Guid InstanceId { get; private set; }
//    }

//    class SoldierFactory : ClientServerMonoBehaviour
//    {
//        private GameObject prefab;
//        private Dictionary<Guid, Soldier> instantiated;

//        public SoldierFactory()
//        {
//            instantiated = new Dictionary<Guid, Soldier>();
//        }

//        protected override void Awake()
//        {
//            base.Awake();

//            prefab = Resources.Load<GameObject>("Prefabs/Soldier/Soldier");
//        }

//        protected override void OnMessageHandle(MessageEventArgs args)
//        {
//            var createMessage = args.Message as CreateSoldierMessage;

//            if (createMessage != null && !instantiated.ContainsKey(createMessage.InstanceId))
//            {
//                var data = new InstanceData(createMessage.InstanceId, createMessage.PlayerSessionId);
//                Instantiate(data);
//            }

//            var destroyMessage = args.Message as RemoveSoldierMessage;

//            if (destroyMessage != null)
//            {
//                Destroy(destroyMessage.InstanceId);
//            }
//        }

//        private void Instantiate(InstanceData data)
//        {
//            if (prefab != null)
//            {
//                var newObject = GameObject.Instantiate(prefab);
//                var script = newObject.GetComponent<Soldier>();
//                script.Id = data.InstanceId;
//                script.PlayerSessionId = data.PlayerSessionId;

//                if (IsClient)
//                {
//                    var rigidbody = newObject.GetComponent<Rigidbody2D>();
//                    rigidbody.isKinematic = true;
//                }

//                instantiated.Add(data.InstanceId, script);
//            }
//        }

//        private void Destroy(Guid instanceId)
//        {
//            Soldier toDestroy;
//            if (instantiated.TryGetValue(instanceId, out toDestroy))
//            {
//                GameObject.Destroy(toDestroy.gameObject);
//                instantiated.Remove(instanceId);
//            }
//        }

//        protected override void ServerSend(SendEventArgs args)
//        {
//            args.Send(new FactoryData { Instantiated = instantiated.Values.Select(x => ToInstanceData(x)).ToArray() }, GetClientsIPEndPoints());
//        }

//        protected override void ClientReceive(ReceiveEventArgs args)
//        {
//            var data = args.GetTypedBinaryDataOfType<FactoryData>().FirstOrDefault();

//            if (data != null)
//            {
//                data.Instantiated.Except(instantiated.Values.Select(x => ToInstanceData(x)))
//                                 .ToList()
//                                 .ForEach(Instantiate);
//                instantiated.Values.Select(x => ToInstanceData(x))
//                                   .Except(data.Instantiated)
//                                   .Select(instance => instance.InstanceId)
//                                   .ToList()
//                                   .ForEach(Destroy);
//            }
//        }

//        private InstanceData ToInstanceData(Soldier soldier)
//        {
//            return new InstanceData(soldier.Id, soldier.PlayerSessionId);
//        }

//        private struct InstanceData
//        {
//            public InstanceData(Guid instanceId, Guid playerSessionId) : this()
//            {
//                InstanceId = instanceId;
//                PlayerSessionId = playerSessionId;
//            }

//            public Guid InstanceId { get; private set; }

//            public Guid PlayerSessionId { get; private set; }
//        }

//        [PacketTypeGuid("D189D529-D673-4D2B-A7E9-83632D9A4B81")]
//        private class FactoryData : ITypedBinaryData
//        {
//            public InstanceData[] Instantiated { get; set; }

//            public byte[] ToBytes()
//            {
//                var builder = new BinaryDataBuilder().Write(Instantiated.Length);

//                foreach (var item in Instantiated)
//                {
//                    builder.Write(item.InstanceId)
//                           .Write(item.PlayerSessionId);
//                }

//                return builder.Build();
//            }

//            public void FromBytes(byte[] bytes)
//            {
//                using (var reader = new BinaryDataReader(bytes))
//                {
//                    Instantiated = new InstanceData[reader.ReadInt()];

//                    for (int i = 0; i < Instantiated.Length; i++)
//                    {
//                        Instantiated[i] = new InstanceData(reader.ReadGuid(), reader.ReadGuid());
//                    }
//                }
//            }
//        }
//    }
//}