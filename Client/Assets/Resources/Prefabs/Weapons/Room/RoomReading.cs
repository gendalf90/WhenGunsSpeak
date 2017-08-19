//using UnityEngine;
//using System.Collections.Generic;
//using System.Linq;
//using System;
//using Network;

//namespace Rooms
//{
//    public interface IRoomInfo
//    {
//        Guid Id { get; }

//        string Description { get; }

//        IPEndPointData IPEndPoint { get; }
//    }

//    public class AllRoomsMessage
//    {
//        public AllRoomsMessage(IEnumerable<IRoomInfo> rooms)
//        {
//            Rooms = rooms;
//        }

//        public IEnumerable<IRoomInfo> Rooms { get; private set; }
//    }

//    class RoomReading : NetworkMonoBehaviour
//    {
//        [SerializeField]
//        private float roomTimeoutInSeconds;

//        private List<Room> rooms;

//        public RoomReading()
//        {
//            rooms = new List<Room>();
//        }

//        private void Update()
//        {
//            rooms.RemoveAll(IsRoomTimeout);
//            SendMessage(new AllRoomsMessage(rooms.Cast<IRoomInfo>()));
//        }

//        private bool IsRoomTimeout(Room room)
//        {
//            return Time.realtimeSinceStartup - room.LastInfoReceiveSinceStartup > roomTimeoutInSeconds;
//        }

//        protected override void AfterReceive(ReceiveEventArgs args)
//        {
//            var data = args.GetReceivedTypedBinaryDataOfType<RoomData>().ToList();

//            if (data.Any())
//            {
//                var received = data.Select(item => new Room
//                {
//                    Id = item.Data.Id,
//                    Description = item.Data.Description,
//                    IPEndPoint = item.From,
//                    LastInfoReceiveSinceStartup = Time.realtimeSinceStartup
//                }).ToList();

//                rooms = rooms.Except(received).Concat(received).ToList();
//            }
//        }

//        class Room : IRoomInfo
//        {
//            public Guid Id { get; set; }
//            public string Description { get; set; }
//            public IPEndPointData IPEndPoint { get; set; }
//            public float LastInfoReceiveSinceStartup { get; set; }

//            public override bool Equals(object obj)
//            {
//                var other = obj as Room;

//                if(other != null)
//                {
//                    return Id == other.Id;
//                }

//                return false;
//            }

//            public override int GetHashCode()
//            {
//                return Id.GetHashCode();
//            }
//        }
//    }
//}