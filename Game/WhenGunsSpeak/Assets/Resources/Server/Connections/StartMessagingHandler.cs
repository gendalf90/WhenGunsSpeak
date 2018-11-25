using Configuration;
using Connection;
using Messages;
using System;
using System.Collections.Generic;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;
using Utils;

namespace Server
{
    class StartMessagingHandler : MonoBehaviour
    {
        private IRoomConnection roomConnection;
        private Observable observable;
        private Parameters parameters;
        private SynchronizationContext synchronization;
        private List<IDisposable> unsubscribers;
        private Guid myId;
        private MyIPHandler myIPHandler;
        private MessagesHandler messagesHandler;

        public StartMessagingHandler()
        {
            unsubscribers = new List<IDisposable>();
        }

        private void Awake()
        {
            observable = FindObjectOfType<Observable>();
            parameters = FindObjectOfType<Parameters>();
            myIPHandler = GetComponent<MyIPHandler>();
            messagesHandler = GetComponent<MessagesHandler>();
        }

        private void Start()
        {
            synchronization = SynchronizationContext.Current;
            myIPHandler.Start(roomConnection);
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
            unsubscribers.Add(roomConnection.Subscribe<MyData>(next: MyDataHandler));
            unsubscribers.Add(roomConnection.Subscribe<MessagingStartingData>(next: MessagingStartingHandler));
            unsubscribers.Add(roomConnection.Subscribe<MessagingStartedData>(next: MessagingStartedHandler));

            observable.Subscribe<StartMessagingCommand>(StartMessaging);
        }

        private void UnsubscribeAll()
        {
            unsubscribers.ForEach(unsubscriber =>
            {
                unsubscriber.Dispose();
            });

            observable.Unsubscribe<StartMessagingCommand>(StartMessaging);
        }

        public async void StartMessaging(StartMessagingCommand command)
        {
            await roomConnection.AskToStartMessagingAsync(command.WithUserId);
        }
        
        private byte[] GenerateSecurityKey()
        {
            throw new NotImplementedException();
        }

        private async Task<IMessageConnection> CreateConnectionAsync(Guid myId, byte[] securityKey)
        {
            return await new Bootstrap().StartMessageConnectionAsync(new MessageConnectionOptions
            {
                ListeningPoint = parameters.GetLocalOrDefault<IPEndPoint>("MessagesListeningPort"),
                NatFuckerAddress = parameters.GetLocalOrDefault<IPEndPoint>("NatFuckerAddress"),
                NatFuckingPeriod = parameters.GetLocalOrDefault<TimeSpan>("NatFuckingPeriod"),
                SecurityKey = securityKey,
                UserId = myId
            });
        }

        public async void Start(IRoomConnection roomConnection)
        {
            this.roomConnection = roomConnection;
            await roomConnection.KnowAboutMeAsync();
        }

        private void MyDataHandler(MyData value)
        {
            synchronization.Post(state =>
            {
                myId = value.Id;
                enabled = true;
            }, value);
        }

        private void MessagingStartingHandler(MessagingStartingData value)
        {
            synchronization.Post(async state =>
            {
                var securityKey = GenerateSecurityKey();
                var connection = await CreateConnectionAsync(myId, securityKey);
                await roomConnection.StartMessagingAsync(value.UserId, securityKey);
                myIPHandler.AddConnection(connection, value.UserId);
            }, value);
        }

        private void MessagingStartedHandler(MessagingStartedData value)
        {
            synchronization.Post(async state =>
            {
                var connection = await CreateConnectionAsync(myId, value.SecurityKey);
                myIPHandler.AddConnection(connection, value.UserId);
            }, value);
        }
    }
}
