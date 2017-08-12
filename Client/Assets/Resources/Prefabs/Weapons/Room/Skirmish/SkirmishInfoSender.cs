//using UnityEngine;
//using System.Collections;
//using System;
//using System.Collections.ObjectModel;
//using System.Collections.Generic;
//using System.Linq;

//public class SendSkirmishRoomInfoEventArgs : EventArgs
//{
//    public List<IRoomInfo> roomsInfo;

//    public SendSkirmishRoomInfoEventArgs()
//    {
//        roomsInfo = new List<IRoomInfo>();
//        ToSend = new ReadOnlyCollection<IRoomInfo>(roomsInfo);
//    }

//    public void AddRoomInfo(IRoomInfo roomInfo)
//    {
//        roomsInfo.Add(roomInfo);
//    }

//    public ReadOnlyCollection<IRoomInfo> ToSend { get; private set; }
//}

//public class SkirmishInfoSender : ClientServerMonoBehaviour
//{
//    [SerializeField]
//    private float sendRoomInfoIntervalInSeconds;

//    private float lastSendRoomInfoTimeSinceStartup;

//    protected override void Start()
//    {
//        base.Start();
//        lastSendRoomInfoTimeSinceStartup = Time.realtimeSinceStartup;
//    }

//    protected override void BeforeSend(SendEventArgs args)
//    {
//        if (IsServer && IsConnected && IsTimeToRoomInfoSend)
//        {
//            var roomInfoArgs = new SendSkirmishRoomInfoEventArgs();
//            SendRoomInfoEvent.SafeRaise(this, roomInfoArgs);

//            foreach (var roomInfo in roomInfoArgs.ToSend)
//            {
//                args.SendToAll(roomInfo);
//            }

//            UpdateSendTime();
//        }
//    }

//    private bool IsTimeToRoomInfoSend
//    {
//        get
//        {
//            return Time.realtimeSinceStartup - lastSendRoomInfoTimeSinceStartup > sendRoomInfoIntervalInSeconds;
//        }
//    }

//    private void UpdateSendTime()
//    {
//        lastSendRoomInfoTimeSinceStartup = Time.realtimeSinceStartup;
//    }

//    public event EventHandler<SendSkirmishRoomInfoEventArgs> SendRoomInfoEvent;
//}
