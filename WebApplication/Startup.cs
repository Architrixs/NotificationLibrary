
// Startup.cs

using i2vNotificationLibrary;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using Microsoft.AspNetCore.SignalR;
using WebApplication.Controllers;

namespace WebApplication
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddSignalR(options =>
            {
                options.EnableDetailedErrors = true;

                // For transports other than long polling, send a keepalive packet every
                // 10 seconds. 
                // This value must be no more than 1/3 of the DisconnectTimeout value
                options.KeepAliveInterval = TimeSpan.FromSeconds(3);
            });
            // Register your library services
            services.AddSingleton<NotificationLibrary>();
            services.AddSingleton<NotificationSender>();
            services.AddSwaggerGen();
            // Register your test controller and other services here
            services.AddControllers();

            
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IServiceProvider serviceProvider)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Your API v1"));
            }

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            var notificationLibrary = serviceProvider.GetService<NotificationLibrary>();
            notificationLibrary.RegisterEntity<src>("VideoSource");
            // This library need a Hub Service as a dependency to use IHubContext
            // Example:
            
            // Create NotificationHub extending Hub
            // Register NotificationHub in Startup.cs
            // NotificationHub = serviceProvider.GetRequiredService<IHubContext<NotificationHub>>();
            
            // Get your library instance and set the HubContext
            // var notificationLibrary = serviceProvider.GetService<NotificationLibrary>();
            // notificationLibrary.SetHubContext(NotificationHub);
            
            // Register your entities
            //notificationLibrary.RegisterEntity<VideoSource>("VideoSource");
            //notificationLibrary.RegisterEntity<AnalyticServer>("AnalyticServer");
        }
    }
}

