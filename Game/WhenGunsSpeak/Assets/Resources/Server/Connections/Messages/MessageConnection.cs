using Configuration;
using Connection;
using MessagePack;
using Messages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading;
using UnityEngine;

namespace Server
{
    class MessageConnection : MonoBehaviour, IObserver<MessageData>, IObserver<MyIPData>
    {
        private Observable observable;
        private Parameters parameters;

        private IMessageConnection connection;
        private List<IDisposable> unsubscribers;
        private SynchronizationContext synchronization;
        private Dictionary<Guid, IPEndPoint> toSendAddresses;

        public MessageConnection()
        {
            unsubscribers = new List<IDisposable>();
            toSendAddresses = new Dictionary<Guid, IPEndPoint>();
        }

        private void Awake()
        {
            observable = FindObjectOfType<Observable>();
            parameters = FindObjectOfType<Parameters>();
        }

        private void Start()
        {
            synchronization = SynchronizationContext.Current;
        }

        private void OnEnable()
        {
            SubscribeAll();
        }

        private void OnDisable()
        {
            UnsubscribeAll();
        }

        private void SubscribeAll()
        {
            observable.Subscribe<StartMessageConnectionCommand>(StartConnectionHandler);
            observable.Subscribe<ClientIsJoinedToRoomEvent>(ClientIsJoinedToRoomHandler);
            observable.Subscribe<IAmJoinedToRoomEvent>(IAmJoinedToRoomHandler);
            observable.Subscribe<ClientIsDisconnectedFromRoomEvent>(ClientIsDisconnectedFromRoomHandler);
            observable.Subscribe<AboutMeEvent>(AboutMeHandler);
            observable.Subscribe<UserIPEvent>(UserIPHandler);
            observable.Subscribe<SendMessageCommand>(SendMessageHandler);
        }

        private void UnsubscribeAll()
        {
            observable.Unsubscribe<StartMessageConnectionCommand>(StartConnectionHandler);
            observable.Unsubscribe<ClientIsJoinedToRoomEvent>(ClientIsJoinedToRoomHandler);
            observable.Unsubscribe<IAmJoinedToRoomEvent>(IAmJoinedToRoomHandler);
            observable.Unsubscribe<ClientIsDisconnectedFromRoomEvent>(ClientIsDisconnectedFromRoomHandler);
            observable.Unsubscribe<AboutMeEvent>(AboutMeHandler);
            observable.Unsubscribe<UserIPEvent>(UserIPHandler);
            observable.Unsubscribe<SendMessageCommand>(SendMessageHandler);
        }

        private void StartConnectionHandler(StartMessageConnectionCommand e)
        {
            observable.Publish(new KnowAboutMeCommand());
        }

        private async void AboutMeHandler(AboutMeEvent e)
        {
            connection = await new Bootstrap().StartMessageConnectionAsync(new MessageConnectionOptions
            {
                ListeningPoint = parameters.GetLocalOrDefault<IPEndPoint>("MessagesListeningPort"),
                NatFuckerAddress = parameters.GetLocalOrDefault<IPEndPoint>("NatFuckerAddress"),
                NatFuckingPeriod = parameters.GetLocalOrDefault<TimeSpan>("NatFuckingPeriod"),
                SecurityKey = parameters.GetLocalOrDefault<byte[]>("MessagesSecurityKey"),
                UserId = e.MyId
            });

            unsubscribers.Add(connection.Subscribe((IObserver<MessageData>)this));
            unsubscribers.Add(connection.Subscribe((IObserver<MyIPData>)this));
        }

        private void ClientIsJoinedToRoomHandler(ClientIsJoinedToRoomEvent e)
        {
            toSendAddresses.Add(e.ClientId, null);
        }

        private void IAmJoinedToRoomHandler(IAmJoinedToRoomEvent e)
        {
            toSendAddresses.Add(e.RoomOwnerId, null);
        }

        private void UserIPHandler(UserIPEvent e)
        {
            if(toSendAddresses.ContainsKey(e.UserId))
            {
                toSendAddresses[e.UserId] = e.UserIp;
            }
        }

        private void ClientIsDisconnectedFromRoomHandler(ClientIsDisconnectedFromRoomEvent e)
        {
            toSendAddresses.Remove(e.ClientId);
        }

        private async void SendMessageHandler(SendMessageCommand command)
        {
            if(connection == null)
            {
                return;
            }

            foreach(var packet in CreatePackets(command.Json))
            {
                await connection.SendAsync(packet);
            }
        }

        private IEnumerable<MessageData> CreatePackets(string jsonData)
        {
            var bytesData = MessagePackSerializer.FromJson(jsonData);

            return toSendAddresses.Where(pair => pair.Value != null)
                                  .Select(pair => new MessageData
                                  {
                                      Bytes = bytesData,
                                      IP = pair.Value
                                  })
                                  .ToList();
        }

        private void OnDestroy()
        {
            unsubscribers.ForEach(unsubscriber =>
            {
                unsubscriber.Dispose();
            });

            connection?.Dispose();
        }

        public void OnCompleted()
        {
        }

        public void OnError(Exception error)
        {
        }

        void IObserver<MessageData>.OnNext(MessageData value)
        {
            var jsonData = MessagePackSerializer.ToJson(value.Bytes);

            synchronization.Post(state =>
            {
                observable.Publish(new MessageIsReceivedEvent(jsonData));
            }, value);
        }

        void IObserver<MyIPData>.OnNext(MyIPData value)
        {
            synchronization.Post(state =>
            {
                observable.Publish(new MyIPEvent(value.IP));
            }, value);
        }
    }
}
