using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Threading.Tasks;
using Serilog;
using Datagrammer;
using System.Security;
using Datagrammer.Hmac;
using System;

namespace FuckNatService
{
    class Program
    {
        public static async Task Main(string[] args)
        {
            await new HostBuilder().ConfigureHostConfiguration(configuration =>
                                   {
                                       configuration.AddCommandLine(args)
                                                    .AddEnvironmentVariables();
                                   })
                                   .ConfigureServices((context, services) =>
                                   {
                                       var securityKey = LoadSecurityKey(context.Configuration);

                                       services.AddSingleton<IMessageHandler, RequestHandler>()
                                               .AddSingleton<IErrorHandler, ErrorHandler>()
                                               .AddSingleton<IMiddleware>(new HmacSha1Middleware(securityKey))
                                               .Configure<DatagramOptions>(options =>
                                               {
                                                   options.ListeningPoint.Port = context.Configuration.GetValue<int>("port");
                                               })
                                               .AddHostedDatagrammer();
                                   })
                                   .UseSerilog((context, configuration) =>
                                   {
                                       configuration.Enrich
                                                    .WithMachineName()
                                                    .Enrich
                                                    .WithProcessId()
                                                    .Enrich
                                                    .WithProperty("ApplicationName", "FuckNatService")
                                                    .WriteTo
                                                    .File(@"Logs\Log.txt",
                                                          outputTemplate: "{Timestamp:u} [{Level:u3}] {MachineName} {ProcessId} {ApplicationName} {Message}{NewLine}{Exception}",
                                                          rollOnFileSizeLimit: true,
                                                          retainedFileCountLimit: 10,
                                                          fileSizeLimitBytes: 10485760);
                                   })
                                   .RunConsoleAsync();
        }

        private static byte[] LoadSecurityKey(IConfiguration configuration)
        {
            var key = configuration["SECURITY_KEY"];
            return Convert.FromBase64String(key);
        }
    }
}
