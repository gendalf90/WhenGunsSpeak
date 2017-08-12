using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;
using System.Linq;
using Server;
using Network;
using Messages;

namespace Weapons
{
    public class CreateWeaponMessage
    {
        public CreateWeaponMessage(WeaponType type, Guid instanceId, Guid playerSessionId, Guid parentId)
        {
            Type = type;
            InstanceId = instanceId;
            PlayerSessionId = playerSessionId;
            ParentId = parentId;
        }

        public WeaponType Type { get; private set; }

        public Guid InstanceId { get; private set; }

        public Guid PlayerSessionId { get; private set; }

        public Guid ParentId { get; private set; }
    }

    class CreateMagazineMessage
    {
        public CreateMagazineMessage(MagazineType type, Guid instanceId, Guid parentId)
        {
            Type = type;
            InstanceId = instanceId;
            ParentId = parentId;
        }

        public MagazineType Type { get; private set; }

        public Guid InstanceId { get; private set; }

        public Guid ParentId { get; private set; }
    }

    public class RemoveWeaponMessage
    {
        public RemoveWeaponMessage(Guid instanceId)
        {
            InstanceId = instanceId;
        }

        public Guid InstanceId { get; private set; }
    }

    interface IWeapon
    {
        WeaponType Type { get; }

        Guid Id { get; set; }

        Guid PlayerSessionId { get; set; }

        Guid ParentId { get; set; }
    }

    interface IMagazine
    {
        MagazineType Type { get; }

        Guid Id { get; set; }

        Guid ParentId { get; set; }
    }

    class WeaponsFactory : ClientServerMonoBehaviour
    {
        private GameObject ak47;
        private GameObject ak47magazine;
        private Dictionary<Guid, GameObject> instantiated;

        public WeaponsFactory()
        {
            instantiated = new Dictionary<Guid, GameObject>();
        }

        protected override void Awake()
        {
            base.Awake();

            ak47 = Resources.Load<GameObject>("Prefabs/Weapons/AK47/AK47");
            ak47magazine = Resources.Load<GameObject>("Prefabs/Weapons/AK47/AK47magazine");
        }

        protected override void OnMessageHandle(MessageEventArgs args)
        {
            var createMessage = args.Message as CreateWeaponMessage;

            if (createMessage != null && !instantiated.ContainsKey(createMessage.InstanceId))
            {
                var data = new InstanceData(createMessage.Type, 
                                            createMessage.InstanceId, 
                                            createMessage.PlayerSessionId, 
                                            createMessage.ParentId);
                Instantiate(data);
            }

            args.Message.As<CreateMagazineMessage>().Do(message =>
            {
                if(!instantiated.ContainsKey(message.InstanceId))
                {
                    MagazineInstantiate(new MagazineInstanceData(message.Type, message.InstanceId, message.ParentId));
                }
            });

            var destroyMessage = args.Message as RemoveWeaponMessage;

            if (destroyMessage != null)
            {
                Destroy(destroyMessage.InstanceId);
            }
        }

        private void Instantiate(InstanceData data)
        {
            if(data.Type == WeaponType.AK47 && ak47 != null)
            {
                var newObject = Instantiate(ak47);
                var script = newObject.GetComponent<IWeapon>();
                script.Id = data.InstanceId;
                script.ParentId = data.ParentId;
                script.PlayerSessionId = data.PlayerSessionId;
                instantiated.Add(data.InstanceId, newObject);
            }
        }

        private void MagazineInstantiate(MagazineInstanceData data)
        {
            if(data.Type == MagazineType.AK47 && ak47magazine != null)
            {
                var newObject = Instantiate(ak47magazine);
                var script = newObject.GetComponent<IMagazine>();
                script.Id = data.InstanceId;
                script.ParentId = data.ParentId;
                instantiated.Add(data.InstanceId, newObject);
            }
        }

        private void Destroy(Guid instanceId)
        {
            GameObject toDestroy;
            if (instantiated.TryGetValue(instanceId, out toDestroy))
            {
                Destroy(toDestroy);
                instantiated.Remove(instanceId);
            }
        }

        private IEnumerable<InstanceData> GetInstanceData()
        {
            return instantiated.Values.Select(obj => obj.GetComponent<IWeapon>())
                                      .Select(weapon => CreateInstaceData(weapon));
        }

        private IEnumerable<MagazineInstanceData> GetMagazinesInstanceData()
        {
            return instantiated.Values.Select(obj => obj.GetComponent<IMagazine>())
                                      .Select(magazine => CreateMagazineInstaceData(magazine));
        }

        protected override void ServerSend(SendEventArgs args)
        {
            args.Send(new FactoryData { Instantiated = GetInstanceData().ToArray() }, GetClientsIPEndPoints());
        }

        protected override void ClientReceive(ReceiveEventArgs args)
        {
            var data = args.GetTypedBinaryDataOfType<FactoryData>().FirstOrDefault();

            if (data != null)
            {
                data.Instantiated.Except(GetInstanceData())
                                 .ToList()
                                 .ForEach(Instantiate);
                GetInstanceData().Except(data.Instantiated)
                                 .Select(instance => instance.InstanceId)
                                 .ToList()
                                 .ForEach(Destroy);

                data.InstantiatedMagazines.Except(GetMagazinesInstanceData())
                                          .ToList()
                                          .ForEach(MagazineInstantiate);
                GetMagazinesInstanceData().Except(data.InstantiatedMagazines)
                                          .Select(instance => instance.InstanceId)
                                          .ToList()
                                          .ForEach(Destroy);
            }
        }

        private InstanceData CreateInstaceData(IWeapon value)
        {
            return new InstanceData(value.Type, value.Id, value.PlayerSessionId, value.ParentId);
        }

        private MagazineInstanceData CreateMagazineInstaceData(IMagazine value)
        {
            return new MagazineInstanceData(value.Type, value.Id, value.ParentId);
        }

        private struct InstanceData
        {
            public InstanceData(WeaponType type, Guid instanceId, Guid playerSessionId, Guid parentId) : this()
            {
                Type = type;
                InstanceId = instanceId;
                PlayerSessionId = playerSessionId;
                ParentId = parentId;
            }

            public WeaponType Type { get; private set; }

            public Guid InstanceId { get; private set; }

            public Guid ParentId { get; private set; }

            public Guid PlayerSessionId { get; private set; }
        }

        private struct MagazineInstanceData
        {
            public MagazineInstanceData(MagazineType type, Guid instanceId, Guid parentId) : this()
            {
                Type = type;
                InstanceId = instanceId;
                ParentId = parentId;
            }

            public MagazineType Type { get; private set; }

            public Guid InstanceId { get; private set; }

            public Guid ParentId { get; private set; }
        }

        [PacketTypeGuid("5726774B-2F82-4114-8B56-5B07CFBDF0A3")]
        private class FactoryData : ITypedBinaryData
        {
            public InstanceData[] Instantiated { get; set; }
            public MagazineInstanceData[] InstantiatedMagazines { get; set; }

            public byte[] ToBytes()
            {
                var builder = new BinaryDataBuilder().Write(Instantiated.Length);

                foreach (var data in Instantiated)
                {
                    builder.Write((int)data.Type)
                           .Write(data.InstanceId)
                           .Write(data.PlayerSessionId)
                           .Write(data.ParentId);
                }

                builder.Write(InstantiatedMagazines.Length);

                foreach (var data in InstantiatedMagazines)
                {
                    builder.Write((int)data.Type)
                           .Write(data.InstanceId)
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
                        Instantiated[i] = new InstanceData((WeaponType)reader.ReadInt(), 
                                                           reader.ReadGuid(), 
                                                           reader.ReadGuid(), 
                                                           reader.ReadGuid());
                    }

                    InstantiatedMagazines = new MagazineInstanceData[reader.ReadInt()];

                    for (int i = 0; i < InstantiatedMagazines.Length; i++)
                    {
                        InstantiatedMagazines[i] = new MagazineInstanceData((MagazineType)reader.ReadInt(),
                                                                            reader.ReadGuid(),
                                                                            reader.ReadGuid());
                    }
                }
            }
        }
    }
}