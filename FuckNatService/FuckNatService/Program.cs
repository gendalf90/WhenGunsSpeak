﻿using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Threading.Tasks;
using Serilog;
using FuckNatService.Messages;
using FuckNatService.MessageClient;

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
                                       services.AddTransient<IMessageClientCreator, MessageClientCreator>()
                                               .AddTransient<IRequestCreator, RequestCreator>()
                                               .AddHostedService<GetYourIPService>()
                                               .Configure<UdpOptions>(options =>
                                               {
                                                   options.Port = context.Configuration.GetValue<int>("port");
                                               })
                                               .Configure<SecurityOptions>(options =>
                                               {
                                                   options.SecurityKey = context.Configuration["SECURITY_KEY"];
                                               });
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
    }
}
