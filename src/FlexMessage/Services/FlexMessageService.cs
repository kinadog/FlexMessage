using FlexMessage.Configs;
using FlexMessage.Hubs;
using FlexMessage.Messages;
using FlexMessage.Middlewares;
using FlexMessage.Models;

namespace FlexMessage.Services
{
    public static class FlexMessageService
    {
        public class FlexMessageServiceOption
        {
            public IMvcBuilder? AddPageView { get; set; }
        }

        public static IApplicationBuilder UseFlexMessage(this IApplicationBuilder app)
        {
            app.UseMiddleware<HubMiddleware>();
            return app;
        }

        public static IEndpointRouteBuilder MapFlexMessage(this IEndpointRouteBuilder app)
        {
            app.MapHub<MessageHub>("/msghub");
            return app;
        }

        public static IServiceCollection AddFlexMessage(this IServiceCollection services,
            WebApplicationBuilder builder)
        {
            services.AddHttpContextAccessor();
            services.AddSignalR(options =>
            {
                options.ClientTimeoutInterval = TimeSpan.FromMinutes(30);
                options.KeepAliveInterval = TimeSpan.FromMinutes(15);
                options.MaximumReceiveMessageSize = 1024 * 1024;
            });
            Config.ContentRootPath = builder.Environment.ContentRootPath;
            services.AddHostedService<FileMessageCngMonitor>();
            services.AddSingleton<IFileEndPosition, FileEndPosition>();
            services.AddSingleton<Dictionary<string, long>>();
            return services;
        }

        public static IServiceCollection AddFlexMessage(this IServiceCollection services,
            WebApplicationBuilder builder, Action<FlexMessageServiceOption> configure)
        {
            services.AddHttpContextAccessor();

            services.Configure(configure);

            services.AddSignalR(options =>
            {
                options.ClientTimeoutInterval = TimeSpan.FromMinutes(30);
                options.KeepAliveInterval = TimeSpan.FromMinutes(15);
                options.MaximumReceiveMessageSize = 1024 * 1024;
            });
            Config.ContentRootPath = builder.Environment.ContentRootPath;
            services.AddHostedService<FileMessageCngMonitor>();
            services.AddSingleton<IFileEndPosition, FileEndPosition>();
            services.AddSingleton<Dictionary<string, long>>();
            return services;
        }
    }
}
