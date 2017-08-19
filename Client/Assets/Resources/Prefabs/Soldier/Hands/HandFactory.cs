//using UnityEngine;
//using System;
//using System.Linq;
//using System.Collections.Generic;
//using Server;
//using Network;
//using Messages;

//namespace Hands
//{
//    public enum HandFormType
//    {
//        OnTrigger = 1,
//        OnForend = 2
//    }

//    public class CreateHandMessage
//    {
//        public CreateHandMessage(HandType type, HandFormType formType, Guid instanceId, Guid parentId)
//        {
//            InstanceId = instanceId;
//            Type = type;
//            ParentId = parentId;
//            FormType = formType;
//        }

//        public Guid InstanceId { get; private set; }

//        public Guid ParentId { get; private set; }

//        public HandType Type { get; private set; }

//        public HandFormType FormType { get; private set; }
//    }

//    public class RemoveHandMessage
//    {
//        public RemoveHandMessage(Guid instanceId)
//        {
//            InstanceId = instanceId;
//        }

//        public Guid InstanceId { get; private set; }
//    }

//    class HandFactory : ClientServerMonoBehaviour
//    {
//        private GameObject prefab;
//        private Sprite onTriggerSprite;
//        private Sprite onForendSprite;
//        private Dictionary<Guid, Hand> instantiated;

//        public HandFactory()
//        {
//            instantiated = new Dictionary<Guid, Hand>();
//        }

//        protected override void Awake()
//        {
//            base.Awake();

//            prefab = Resources.Load<GameObject>("Prefabs/Soldier/Hands/Hand");
//            onTriggerSprite = Resources.Load<Sprite>("Prefabs/Soldier/Hands/OnTrigger");
//            onForendSprite = Resources.Load<Sprite>("Prefabs/Soldier/Hands/OnForend");
//        }

//        protected override void OnMessageHandle(MessageEventArgs args)
//        {
//            var createMessage = args.Message as CreateHandMessage;

//            if (createMessage != null && !instantiated.ContainsKey(createMessage.InstanceId))
//            {
//                var data = new InstanceData(createMessage.Type, createMessage.FormType, createMessage.InstanceId, createMessage.ParentId);
//                Instantiate(data);
//            }

//            var destroyMessage = args.Message as RemoveHandMessage;

//            if (destroyMessage != null)
//            {
//                Destroy(destroyMessage.InstanceId);
//            }
//        }

//        private void Instantiate(InstanceData data)
//        {
//            if (prefab != null)
//            {
//                var newObject = Instantiate(prefab);
//                var script = newObject.GetComponent<Hand>();
//                script.Id = data.InstanceId;
//                script.ParentId = data.ParentId;
//                script.Type = data.Type;
//                script.FormType = data.FormType;
//                var renderer = newObject.GetComponentInChildren<SpriteRenderer>();
//                renderer.sprite = GetSpriteByHandFormType(data.FormType);
//                instantiated.Add(data.InstanceId, script);
//            }
//        }

//        private Sprite GetSpriteByHandFormType(HandFormType type)
//        {
//            switch(type)
//            {
//                case HandFormType.OnTrigger:
//                    return onTriggerSprite;
//                case HandFormType.OnForend:
//                    return onForendSprite;
//                default:
//                    return null;
//            }
//        }

//        private void Destroy(Guid instanceId)
//        {
//            Hand toDestroy;
//            if (instantiated.TryGetValue(instanceId, out toDestroy))
//            {
//                Destroy(toDestroy.gameObject);
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

//        private InstanceData CreateInstaceData(Hand value)
//        {
//            return new InstanceData(value.Type, value.FormType, value.Id, value.ParentId);
//        }

//        private struct InstanceData
//        {
//            public InstanceData(HandType type, HandFormType formType, Guid instanceId, Guid parentId) : this()
//            {
//                InstanceId = instanceId;
//                Type = type;
//                ParentId = parentId;
//                FormType = formType;
//            }

//            public Guid InstanceId { get; private set; }

//            public Guid ParentId { get; private set; }

//            public HandType Type { get; private set; }

//            public HandFormType FormType { get; private set; }
//        }

//        [PacketTypeGuid("194D7788-4766-4D11-8B56-C6B86B691BBA")]
//        private class FactoryData : ITypedBinaryData
//        {
//            public InstanceData[] Instantiated { get; set; }

//            public byte[] ToBytes()
//            {
//                var builder = new BinaryDataBuilder().Write(Instantiated.Length);

//                foreach (var data in Instantiated)
//                {
//                    builder.Write((int)data.Type)
//                           .Write((int)data.FormType)
//                           .Write(data.InstanceId)
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
//                        Instantiated[i] = new InstanceData((HandType)reader.ReadInt(), 
//                                                           (HandFormType)reader.ReadInt(), 
//                                                           reader.ReadGuid(), 
//                                                           reader.ReadGuid());
//                    }
//                }
//            }
//        }
//    }
//}