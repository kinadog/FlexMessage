using FlexMessage.Configs;
using FlexMessage.Messages;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;

namespace FlexMessage.Middlewares;

/// <summary>
///     SignalR Hub에서 사용할 미들웨어 클래스입니다.
///     Middleware that sets up the configuration for
///     logging messages via various channels, including console, file, and browser.
/// </summary>

public class HubMiddleware
{
    private readonly RequestDelegate _next;

    public HubMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    /// <summary>
    ///     HttpContext를 기반으로 Message 객체 및 FileMessageCngMonitor 객체를 초기화합니다.
    ///     또한, HTTPS와 HTTP를 구분하여 Config.Host 값을 설정합니다.
    ///     Invokes the middleware to configure the message logging channels
    ///     and sets the HttpContextAccessor.
    /// </summary>
    /// <param name="context">현재 HTTP 요청의 HttpContext</param>
    public async Task InvokeAsync(HttpContext context)
    {
        // HttpContextAccessor를 이용하여 HttpContext를 가져옵니다.
        // Injects the HttpContextAccessor to the logging channels configuration
        var httpContextAccessor = context.RequestServices
            .GetRequiredService<IHttpContextAccessor>();
        var serviceProvider = context.RequestServices;

        Message.Configure(httpContextAccessor, serviceProvider);

        // ↓ 웹페이지의 실시간 파일 View 기능이 필요 없으시면 비 활성화 하셔도 됩니다.
        // ↓ If real-time file view feature is not needed, you can disable it.
        // HttpContext를 기반으로 FileMessageCngMonitor 객체를 초기화합니다.
        // Initializes the FileMessageCngMonitor object based on the HttpContext.
        FileMessageCngMonitor.Configure(httpContextAccessor);

        // HTTP 쿠키에서 connectionId 값을 가져옵니다.
        // Gets the connectionId value from the HTTP cookie.
        var connectionId = context.Request.Cookies["signalr_connectionId"];

        if (!string.IsNullOrWhiteSpace(connectionId))
        {
            // connectionId 값을 복호화하여 FileMessageCngMonitor.connectionId에 할당합니다.
            // Decrypts the connectionId value and assigns it to FileMessageCngMonitor.connectionId.
            var decryptConId = Hashing.Decrypt(connectionId);
            if (string.IsNullOrWhiteSpace(decryptConId))
                context.Response.Cookies.Delete("signalr_connectionId");
            else
                FileMessageCngMonitor.connectionId = decryptConId;
        }
        // ↑ 웹페이지의 실시간 파일 View 기능이 필요 없으시면 비 활성화 하셔도 됩니다.
        // ↑ If real-time file view feature is not needed, you can disable it.

        // HTTP 요청의 스키마(HTTP 또는 HTTPS)를 기반으로 Config.Host 값을 설정합니다.
        // Sets the host URL in the configuration based on the request scheme and host
        Config.Host = context.Request.Scheme == "https"
            ? $"https://{context.Request.Host}"
            : $"http://{context.Request.Host}";

        // 다음 미들웨어 또는 앱 요청을 처리합니다.
        // Processes the next middleware or app request.
        await _next(context);
    }
}