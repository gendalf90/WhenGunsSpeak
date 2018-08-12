using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Threading.Tasks;

namespace FuckNatService
{
    class Program
    {
        public static async Task Main(string[] args)
        {
            await new HostBuilder().ConfigureHostConfiguration(configuration =>
                                   {
                                       configuration.AddCommandLine(args);
                                   })
                                   .ConfigureServices((context, services) =>
                                   {
                                       services.AddHostedService<GetYourIPService>()
                                               .Configure<UdpOptions>(options =>
                                               {
                                                   options.Port = context.Configuration.GetValue<int>("port");
                                               });
                                   })
                                   .RunConsoleAsync();
        }
    }
}
