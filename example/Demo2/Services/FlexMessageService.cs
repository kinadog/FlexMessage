using Demo2.Configs;
using Demo2.Hubs;
using Demo2.Messages;
using Demo2.Messages.Types;
using Demo2.Middlewares;
using Demo2.Models;

namespace Demo2.Services
{
    public enum FileMessageStatus
    {
        LiveView,
        Off
    }

    public static class FlexMessageService
    {
        // 옵션 클래스. IsFileMessageWriteLiveView 프로퍼티로 파일 메시지 기록 라이브 뷰 여부를 설정할 수 있음.
        // Option class. Can set whether to file message write live view with IsLiveViewFileMessageWrite property.
        public class FlexMessageOption
        {
            public FileMessageStatus FileMessageStatus { get; set; }
        }

        // FlexMessage 미들웨어 등록을 위한 확장 메소드.
        // Extension method for registering FlexMessage middleware.
        public static IApplicationBuilder UseFlexMessage(this IApplicationBuilder app)
        {
            app.UseMiddleware<HubMiddleware>();
            return app;
        }

        // FlexMessage용 엔드포인트 등록을 위한 확장 메소드.
        // Extension method for registering endpoint for FlexMessage.
        public static IEndpointRouteBuilder MapFlexMessage(this IEndpointRouteBuilder app)
        {
            app.MapHub<MessageHub>("/msghub");
            return app;
        }

        // FlexMessage 서비스 등록을 위한 확장 메소드.
        // Extension method for registering FlexMessage service.
        public static IServiceCollection AddFlexMessage(
            this IServiceCollection services,
            WebApplicationBuilder builder,
            Action<FlexMessageOption>? option = null,
            Action<string> saveMessageAction = null)
        {
            // HttpContextAccessor 서비스 등록.
            // Register HttpContextAccessor service.
            services.AddHttpContextAccessor();

            // SignalR 서비스 등록.
            // Register SignalR service.
            services.AddSignalR(options =>
            {
                options.ClientTimeoutInterval = TimeSpan.FromMinutes(30);
                options.KeepAliveInterval = TimeSpan.FromMinutes(15);
                options.MaximumReceiveMessageSize = 1024 * 1024;
            });

            services.AddScoped<DbMessage>(sp => new DbMessage(
                sp.GetRequiredService<IHttpContextAccessor>(),
                saveMessageAction));


            // FlexMessageOption 인스턴스를 생성하고 전달된 Action을 사용하여 옵션을 구성합니다.
            // Create a FlexMessageOption instance and configure the options using the passed-in Action.
            var flexMessageOption = new FlexMessageOption();
            option?.Invoke(flexMessageOption);


            // 옵션 값이 FileMessageStatus.LiveView 인 경우 필요한 동작을 수행합니다.
            // Perform the necessary action if the option value is FileMessageStatus.LiveView.
            if (flexMessageOption.FileMessageStatus == FileMessageStatus.LiveView)
            {
                // 파일 메시지 기록 라이브 뷰 관련 서비스 등록.
                // Register services for writing file message live view.
                Config.ContentRootPath = builder.Environment.ContentRootPath;

                builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

                services.AddHostedService<FileMessageCngMonitor>();

                services.AddSingleton<IFileEndPosition, FileEndPosition>();
                services.AddSingleton<Dictionary<string, long>>();
            }


            return services;
        }

    }
}