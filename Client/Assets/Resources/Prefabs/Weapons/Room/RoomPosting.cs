using UnityEngine;
using System.Collections.Generic;
using System;
using Network;
using Messages;

namespace Rooms
{
    public class PostRoomInfoMessage
    {
        public PostRoomInfoMessage(Guid id, string description)
        {
            Id = id;
            Description = description;
        }

        public Guid Id { get; private set; }

        public string Description { get; private set; }
    }

    public class UnpostRoomInfoMessage
    {
        public UnpostRoomInfoMessage(Guid id)
        {
            Id = id;
        }

        public Guid Id { get; private set; }
    }

    class RoomPosting : NetworkMonoBehaviour
    {
        [SerializeField]
        private float dataSendIntervalInSeconds;

        private float lastSendTimeSinceStartupInSeconds;

        private Dictionary<Guid, RoomData> rooms;

        public RoomPosting()
        {
            rooms = new Dictionary<Guid, RoomData>();
        }

        protected override void Start()
        {
            base.Start();

            lastSendTimeSinceStartupInSeconds = Time.realtimeSinceStartup;
        }

        protected override void BeforeSend(SendEventArgs args)
        {
            if (IsTimeToSend)
            {
                rooms.Values.ForEach(args.SendToAll);
                UpdateSendTime();
            }
        }

        private bool IsTimeToSend
        {
            get
            {
                return Time.realtimeSinceStartup - lastSendTimeSinceStartupInSeconds > dataSendIntervalInSeconds;
            }
        }

        private void UpdateSendTime()
        {
            lastSendTimeSinceStartupInSeconds = Time.realtimeSinceStartup;
        }

        protected override void OnMessageHandle(MessageEventArgs args)
        {
            var postMessage = args.Message as PostRoomInfoMessage;

            if (postMessage != null)
            {
                rooms[postMessage.Id] = new RoomData { Id = postMessage.Id, Description = postMessage.Description };
            }

            var unpostMessage = args.Message as UnpostRoomInfoMessage;

            if (unpostMessage != null)
            {
                rooms.Remove(unpostMessage.Id);
            }
        }
    }

    [PacketTypeGuid("7D14BFAE-FFD4-4234-AAC0-9311CFCA6210")]
    class RoomData : ITypedBinaryData
    {
        public Guid Id { get; set; }
        public string Description { get; set; }

        public byte[] ToBytes()
        {
            return new BinaryDataBuilder().Write(Id)
                                          .Write(Description)
                                          .Build();
        }

        public void FromBytes(byte[] bytes)
        {
            using (var reader = new BinaryDataReader(bytes))
            {
                Id = reader.ReadGuid();
                Description = reader.ReadString();
            }
        }
    }
}