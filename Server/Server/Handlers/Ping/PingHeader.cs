using System;
using System.Collections.Generic;
using System.Text;

namespace Server.Handlers
{
    class PingHeader
    {
        public string Action { get; set; }

        public string From { get; set; }

        public string To { get; set; }
    }
}
