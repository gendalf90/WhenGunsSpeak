﻿//using UnityEngine;
//using System;
//using System.Linq;
//using System.Collections.Generic;
//using Server;
//using Network;
//using Messages;

//namespace Head
//{
//    public class CreateHeadMessage
//    {
//        public CreateHeadMessage(Guid instanceId, Guid playerSessionId, Guid parentId)
//        {
//            InstanceId = instanceId;
//            PlayerSessionId = playerSessionId;
//            ParentId = parentId;
//        }

//        public Guid InstanceId { get; private set; }

//        public Guid ParentId { get; private set; }

//        public Guid PlayerSessionId { get; private set; }
//    }

//    public class RemoveHeadMessage
//    {
//        public RemoveHeadMessage(Guid instanceId)
//        {
//            InstanceId = instanceId;
//        }

//        public Guid InstanceId { get; private set; }
//    }

//    class HeadFactory : ClientServerMonoBehaviour
//    {
//        private GameObject prefab;
//        private Dictionary<Guid, Head> instantiated;

//        public HeadFactory()
//        {
//            instantiated = new Dictionary<Guid, Head>();
//        }

//        protected override void Awake()
//        {
//            base.Awake();

//            prefab = Resources.Load<GameObject>("Prefabs/Soldier/Head/HeadPrefab");
//        }

//        protected override void OnMessageHandle(MessageEventArgs args)
//        {
//            var createMessage = args.Message as CreateHeadMessage;

//            if (createMessage != null && !instantiated.ContainsKey(createMessage.InstanceId))
//            {
//                var data = new InstanceData(createMessage.InstanceId, createMessage.PlayerSessionId, createMessage.ParentId);
//                Instantiate(data);
//            }

//            var destroyMessage = args.Message as RemoveHeadMessage;

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
//                var script = newObject.GetComponent<Head>();
//                script.Id = data.InstanceId;
//                script.ParentId = data.ParentId;
//                script.PlayerSessionId = data.PlayerSessionId;
//                instantiated.Add(data.InstanceId, script);
//            }
//        }

//        private void Destroy(Guid instanceId)
//        {
//            Head toDestroy;
//            if (instantiated.TryGetValue(instanceId, out toDestroy))
//            {
//                GameObject.Destroy(toDestroy.gameObject);
//                instantiated.Remove(instanceId);
//            }
//        }

//        protected override void ServerSend(SendEventArgs args)
//        {
//            args.Send(new FactoryData { Instantiated = instantiated.Values.Select(x => CreateInstaceData(x)).ToArray() }, GetClientsIPEndPoints());
//        }

//        protected override void ClientReceive(ReceiveEventArgs args)
//        {
//            var data = args.GetTypedBinaryDataOfType<FactoryData>().FirstOrDefault();

//            if (data != null)
//            {
//                data.Instantiated.Except(instantiated.Values.Select(x => CreateInstaceData(x)))
//                                 .ToList()
//                                 .ForEach(Instantiate);
//                instantiated.Values.Select(x => CreateInstaceData(x))
//                                   .Except(data.Instantiated)
//                                   .Select(instance => instance.InstanceId)
//                                   .ToList()
//                                   .ForEach(Destroy);
//            }
//        }

//        private InstanceData CreateInstaceData(Head head)
//        {
//            return new InstanceData(head.Id, head.PlayerSessionId, head.ParentId);
//        }

//        private struct InstanceData
//        {
//            public InstanceData(Guid instanceId, Guid playerSessionId, Guid parentId) : this()
//            {
//                InstanceId = instanceId;
//                PlayerSessionId = playerSessionId;
//                ParentId = parentId;
//            }

//            public Guid InstanceId { get; private set; }

//            public Guid ParentId { get; private set; }

//            public Guid PlayerSessionId { get; private set; }
//        }

//        [PacketTypeGuid("7A27E3D1-D8E9-4B88-A50C-B34F2E3E4713")]
//        private class FactoryData : ITypedBinaryData
//        {
//            public InstanceData[] Instantiated { get; set; }

//            public byte[] ToBytes()
//            {
//                var builder = new BinaryDataBuilder().Write(Instantiated.Length);

//                foreach (var data in Instantiated)
//                {
//                    builder.Write(data.InstanceId)
//                           .Write(data.PlayerSessionId)
//                           .Write(data.ParentId);
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
//                        Instantiated[i] = new InstanceData(reader.ReadGuid(), reader.ReadGuid(), reader.ReadGuid());
//                    }
//                }
//            }
//        }
//    }
//}