using System;

namespace Connection.Rooms
{
    class UserConnectedEventArgs : EventArgs
    {
        public UserConnectedEventArgs(string userId)
        {
            UserId = userId;
        }

        public string UserId { get; private set; }
    }
}
