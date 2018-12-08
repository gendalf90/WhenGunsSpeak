using Connection;
using System;

namespace Server
{
    class StartMessagingConnectionEventArgs : EventArgs
    {
        public StartMessagingConnectionEventArgs(IMessageConnection messageConnection, Guid userId)
        {
            MessageConnection = messageConnection;
            UserId = userId;
        }

        public Guid UserId { get; private set; }

        public IMessageConnection MessageConnection { get; private set; }
    }
}
