using FlexMessage.Hubs;
using Microsoft.AspNetCore.SignalR;

namespace FlexMessage.Messages.Types;

/// <summary>
/// 브라우저 알림 메세지를 처리하는 클래스
/// Class for handling browser alert messages.
/// </summary>
public class BrowserAlertMessage : IMessage
{
    #region Field

    private static IHttpContextAccessor? _contextAccessor;
    private static IHubContext<MessageHub>? _hubContext;

    #endregion


    #region ctor

    /// <summary>
    /// HttpContextAccessor를 이용하여 HubContext를 생성한다.
    /// Initializes a new instance of the BrowserAlertMessage class, using the HttpContextAccessor to obtain create a HubContext.
    /// </summary>
    /// <param name="contextAccessor">HttpContextAccessor 객체</param>
    public BrowserAlertMessage(IHttpContextAccessor contextAccessor)
    {
        _contextAccessor = contextAccessor;
        _hubContext = _contextAccessor.HttpContext!.RequestServices.GetRequiredService<IHubContext<MessageHub>>();
    }

    #endregion


    #region Method

    /// <summary>
    /// ConnectionId를 이용하여 SignalR 클라이언트에게 메세지를 전송한다.
    /// Send a message to the SignalR client using the ConnectionId.
    /// </summary>
    public void Write(string? message)
    {
        if (string.IsNullOrWhiteSpace(message)) return;

        // 쿠키에서 현재 접속중인 SignalR의 ConnectionId 가져오기
        // Get the ConnectionId from the cookie
        var getConnectionId =
            _contextAccessor!.HttpContext!
                .Request.Cookies.TryGetValue("signalr_connectionId",
                    out var connectionId);

        // connectionId 값으로 Client 메세지 전송 메소드 호출
        // Call the method to send the message to the client using the ConnectionId
        if (getConnectionId && !string.IsNullOrWhiteSpace(connectionId))
        {
            connectionId = Hashing.Decrypt(connectionId);
            _hubContext!.Clients.Client(connectionId!)
                .SendAsync("ReceiveMessage", "BrowserAlert", message);
        }
    }

    /// <summary>
    /// ConnectionId를 이용하여 SignalR 클라이언트에게 메세지를 전송한다. (비동기)
    /// Send a message to the SignalR client using the ConnectionId. (asynchronous)
    /// </summary>
    public async Task WriteAsync(string? message)
    {
        if (string.IsNullOrWhiteSpace(message)) return;

        // 쿠키에서 현재 접속중인 SignalR의 ConnectionId 가져오기
        // Get the ConnectionId from the cookie
        var getConnectionId =
            _contextAccessor!.HttpContext!
                .Request.Cookies.TryGetValue("signalr_connectionId",
                    out var connectionId);

        // connectionId 값으로 Client 메세지 전송 메소드 호출
        // Call the method to send the message to the client using the ConnectionId
        if (getConnectionId && !string.IsNullOrWhiteSpace(connectionId))
        {
            connectionId = Hashing.Decrypt(connectionId);
            await _hubContext!.Clients.Client(connectionId!)
                .SendAsync("ReceiveMessage", "BrowserAlert", message);
        }
    }

    #endregion


}