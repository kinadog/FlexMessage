using FlexMessage.Configs;
using FlexMessage.Messages;
using FlexMessage.Messages.Types;
using FlexMessage.Middlewares;
using FlexMessage.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using WebSocketManager = FlexMessage.WebSockets.WebSocketManager;

namespace FlexMessage.Services;

public static class FlexMessageService
{

    // FlexMessage 미들웨어 등록을 위한 확장 메소드.
    // Extension method for registering FlexMessage middleware.
    public static IApplicationBuilder UseFlexMessage(this IApplicationBuilder app)
    {
        app.UseWebSockets(new WebSocketOptions
        {
            KeepAliveInterval = TimeSpan.FromSeconds(120)
        });

        app.UseMiddleware<FlexMessageMiddleware>();
        return app;
    }

    // FlexMessage 서비스 등록을 위한 확장 메소드.
    // Extension method for registering FlexMessage service.
    public static IServiceCollection AddFlexMessage(
        this IServiceCollection services,
        WebApplicationBuilder builder,
        Action<FlexMessageOption>? option = null,
        Action<string>? saveMessageAction = null)
    {
        // HttpContextAccessor 서비스 등록.
        // Register HttpContextAccessor service.
        services.AddHttpContextAccessor();

        services.AddScoped<DbMessage>(sp => new DbMessage(
            sp.GetRequiredService<IMessageCommon>(),
            saveMessageAction));

        services.AddSingleton<IMessageCommon, MessageCommon>();
        services.AddSingleton<WebSocketManager>();

        // IsFileMessageWriteLiveView 값에 따라 파일 메시지 기록 라이브 뷰 여부을 수행할지 결정.
        // Register services for file message write live view depending on IsLiveViewFileMessageWrite value.
        var flexMessageOption = new FlexMessageOption();
        option?.Invoke(flexMessageOption);

        // 옵션 값이 FileMessageStatus.LiveView 인 경우 필요한 동작을 수행합니다.
        // Perform the necessary action if the option value is FileMessageStatus.LiveView.
        if (flexMessageOption.FileMessageStatus != FileMessageStatus.LiveView) return services;

        // 파일 메시지 기록 라이브 뷰 관련 서비스 등록.
        // Register services for writing file message live view.
        Config.ContentRootPath = builder.Environment.ContentRootPath;
        builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
        services.AddHostedService<FileMessageCngMonitor>();
        services.AddSingleton<IFileEndPosition, FileEndPosition>();
        services.AddSingleton<Dictionary<string, long>>();

        return services;
    }

}