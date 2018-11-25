using System.Net;

namespace Server
{
    class MyIPEvent
    {
        public MyIPEvent(IPEndPoint myIp)
        {
            MyIp = myIp;
        }

        public IPEndPoint MyIp { get; private set; }
    }
}
