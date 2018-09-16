using System;
using System.Net;

namespace Connection
{
    public sealed class UserIPData
    {
        public Guid UserId { get; set; }

        public IPEndPoint IP { get; set; }
    }
}
