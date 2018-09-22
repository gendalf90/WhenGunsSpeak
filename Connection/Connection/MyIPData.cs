using System;
using System.Net;

namespace Connection
{
    public sealed class MyIPData
    {
        public Guid UserId { get; set; }

        public IPEndPoint IP { get; set; }
    }
}
