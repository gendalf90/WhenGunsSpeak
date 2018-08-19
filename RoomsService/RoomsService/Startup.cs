using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using RoomsService.Hubs;
using RoomsService.Initialization;

namespace RoomsService
{
    public class Startup
    {
        private readonly IConfiguration configuration;

        public Startup(IConfiguration configuration)
        {
            this.configuration = configuration;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddAuthentication(configuration)
                    .AddSignalR(configuration)
                    .AddMongo(configuration)
                    .AddCommon()
                    .AddLogs()
                    .AddMvc();
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            app.UseAuthentication();

            app.UseSignalR(routes =>
               {
                   routes.MapHub<RoomsHub>("/signalr/rooms");
               });

            app.UseMvc();
        }
    }
}
