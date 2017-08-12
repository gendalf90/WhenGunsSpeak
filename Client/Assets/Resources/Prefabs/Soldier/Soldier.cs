using System;
using System.Linq;
using UnityEngine;
using Head;
using Server;
using Network;
using Messages;
using Body;
using Legs;
using Rooms;

namespace Soldier
{
    public class SetSoldierPositionMessage
    {
        public SetSoldierPositionMessage(Guid id, Vector2 position)
        {
            InstanceId = id;
            Position = position;
        }

        public Guid InstanceId { get; private set; }

        public Vector2 Position { get; private set; }
    }

    class Soldier : ClientServerMonoBehaviour
    {
        private Transform headTransform;
        private Transform bodyTransform;
        private Transform legsTransform;

        protected override void Awake()
        {
            base.Awake();

            headTransform = transform.FindChild("HeadHandle");
            bodyTransform = transform.FindChild("BodyHandle");
            legsTransform = transform.FindChild("LegsHandle");
        }

        private void Update()
        {
            SendMessage(new SetHeadPositionMessage(Id, headTransform.position));
            SendMessage(new SetBodyPositionMessage(Id, bodyTransform.position));
            SendMessage(new SetLegsPositionMessage(Id, legsTransform.position));
            SendMessage(new SetCameraPositionMessage(bodyTransform.position));
        }

        public bool IsPlayer
        {
            get
            {
                return LocalSessionId == PlayerSessionId;
            }
        }

        public Guid PlayerSessionId { get; set; }

        public Guid Id { get; set; }

        private Vector2 Position
        {
            get
            {
                return transform.position;
            }

            set
            {
                transform.position = value;
            }
        }

        protected override void ServerSend(SendEventArgs args)
        {
            args.Send(new ServerData
            {
                Id = Id,
                Position = Position
            }, GetClientsIPEndPoints());
        }

        protected override void ClientReceive(ReceiveEventArgs args)
        {
            var data = args.GetTypedBinaryDataOfType<ServerData>().FirstOrDefault(x => x.Id == Id);

            if (data != null)
            {
                Position = data.Position;
            }
        }

        protected override void OnMessageHandle(MessageEventArgs args)
        {
            var message = args.Message as SetSoldierPositionMessage;

            if (message != null && message.InstanceId == Id)
            {
                Position = message.Position;
            }
        }

        [PacketTypeGuid("37A68DBA-C89D-41C4-96DB-8277C25349C0")]
        private class ServerData : ITypedBinaryData
        {
            public Guid Id { get; set; }
            public Vector2 Position { get; set; }

            public void FromBytes(byte[] bytes)
            {
                using (var reader = new BinaryDataReader(bytes))
                {
                    Id = reader.ReadGuid();
                    Position = reader.ReadVector();
                }
            }

            public byte[] ToBytes()
            {
                return new BinaryDataBuilder().Write(Id)
                                              .Write(Position)
                                              .Build();
            }
        }
    }
}