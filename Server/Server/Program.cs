using Microsoft.Extensions.Configuration;
using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace Server
{
    class Program
    {
        static void Main(string[] args)
        {
            //var configuration = new ConfigurationBuilder().AddCommandLine(args)
            //                                              .SetBasePath(Directory.GetCurrentDirectory())
            //                                              .AddJsonFile("settings.json")
            //                                              .Build();

            //localEndPoint = new IPEndPoint(IPAddress.Any, configuration.GetValue<int>("Port"));

            Console.WriteLine("Hello World!");

            //Thread.Sleep(Timeout.InfiniteTimeSpan);
        }
    }
}
