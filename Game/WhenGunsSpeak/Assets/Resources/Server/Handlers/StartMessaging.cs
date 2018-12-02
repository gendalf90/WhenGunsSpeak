using Configuration;
using Connection;
using Messages;
using System;
using SystemRandom = System.Random;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;
using Utils;

namespace Server
{
    class StartMessagingHandler : MonoBehaviour
    {
        private static readonly int SecurityKeyLength = 20;

        private IRoomConnection roomConnection;
        private Observable observable;
        private Parameters parameters;
        private SynchronizationContext synchronization;
        private Guid myId;
        private MyIPHandler myIPHandler;
        private MessagesHandler messagesHandler;
        private SystemRandom random;

        public StartMessagingHandler()
        {
            random = new SystemRandom();
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
        }

        private async void StartMessaging(StartMessagingCommand command)
        {
            await roomConnection.AskToStartMessagingAsync(command.WithUserId);
        }
        
        private byte[] GenerateSecurityKey()
        {
            var buffer = new byte[SecurityKeyLength];
            random.NextBytes(buffer);
            return buffer;
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

        public void Start(IRoomConnection connection, Guid myId)
        {
            this.myId = myId;
            roomConnection = connection;
            myIPHandler.Start(connection);
            messagesHandler.Start(connection);
            roomConnection.Subscribe<MessagingStartingData>(next: MessagingStartingHandle);
            roomConnection.Subscribe<MessagingStartedData>(next: MessagingStartedHandle);
            observable.Subscribe<StartMessagingCommand>(StartMessaging);
        }

        private void MessagingStartingHandle(MessagingStartingData value)
        {
            synchronization.Post(async state =>
            {
                var securityKey = GenerateSecurityKey();
                var connection = await CreateConnectionAsync(myId, securityKey);
                await roomConnection.StartMessagingAsync(value.UserId, securityKey);
                myIPHandler.AddConnection(connection, value.UserId);
                messagesHandler.AddConnection(connection, value.UserId);
            }, value);
        }

        private void MessagingStartedHandle(MessagingStartedData value)
        {
            synchronization.Post(async state =>
            {
                var connection = await CreateConnectionAsync(myId, value.SecurityKey);
                myIPHandler.AddConnection(connection, value.UserId);
                messagesHandler.AddConnection(connection, value.UserId);
            }, value);
        }
    }
}
