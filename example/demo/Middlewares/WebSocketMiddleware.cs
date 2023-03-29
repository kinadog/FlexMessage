using System.Net.WebSockets;
using System.Text;
using Demo.WebSockets;
using WebSocketManager = Demo.WebSockets.WebSocketManager;

namespace Demo.Middlewares;

public class WebSocketMiddleware
{
    private readonly RequestDelegate _next;
    private readonly WebSocketManager _webSocketManager;

    public WebSocketMiddleware(RequestDelegate next
        ,WebSocketManager webSocketManager)
    {
        _webSocketManager = webSocketManager;
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        // 다음 미들웨어 또는 앱 요청을 처리합니다.
        // Processes the next middleware or app request.
        if (context.Request.Path == "/ws")
        {
            if (context.WebSockets.IsWebSocketRequest)
            {
                await HandleWebSocketConnectionAsync(context);
            }
            else
            {
                context.Response.StatusCode = 400;
            }
        }
        else
        {
            await _next(context);
        }
    }

    private async Task HandleWebSocketConnectionAsync(HttpContext context)
    {
        var webSocket = await context.WebSockets.AcceptWebSocketAsync();

        // 쿠키에서 WebSocketId 가져오기
        var webSocketId = context.Request.Cookies["webSocketId"];

        // webSocketId가 없을 경우
        if (string.IsNullOrWhiteSpace(webSocketId))
        {
            webSocketId = _webSocketManager.AddWebSocket(webSocket);
            var WebSocketMessage = new WebSocketMessage
            {
                IsId = true,
                Message = webSocketId
            };

            await webSocket.SendAsync(new ArraySegment<byte>(Encoding.UTF8.GetBytes(System.Text.Json.JsonSerializer.Serialize(WebSocketMessage))),
                WebSocketMessageType.Text, true, CancellationToken.None);
        }
        // webSocketId가 있을 경우
        else
        {
            webSocket = _webSocketManager.GetSocketById(webSocketId);

            // webSocketId로 조회된 WebSocket이 없을 경우
            if (webSocket == null || webSocket.State != WebSocketState.Open)
            {
                webSocketId = _webSocketManager.AddWebSocket(webSocket);
                var WebSocketMessage = new WebSocketMessage
                {
                    IsId = true,
                    Message = webSocketId
                };

                await webSocket.SendAsync(new ArraySegment<byte>(Encoding.UTF8.GetBytes(System.Text.Json.JsonSerializer.Serialize(WebSocketMessage))),
                    WebSocketMessageType.Text, true, CancellationToken.None);
            }
        }

        var buffer = new byte[1024 * 4];
        while (webSocket.State == WebSocketState.Open)
        {
            var result = await webSocket.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);

            if (result.MessageType == WebSocketMessageType.Text)
            {
                // 클라이언트로부터 받은 메시지를 다시 되돌려 보냅니다.
                await webSocket.SendAsync(new ArraySegment<byte>(buffer, 0, result.Count), WebSocketMessageType.Text, result.EndOfMessage, CancellationToken.None);
            }
            else if (result.MessageType == WebSocketMessageType.Close)
            {
                // 클라이언트가 연결 종료를 요청했습니다.
                await webSocket.CloseAsync(WebSocketCloseStatus.NormalClosure, "Closing", CancellationToken.None);
                break;
            }
        }
    }

    private Task AddWebSocketIdtoCookie(string? webSocketId, HttpContext? context)
    {
        if (string.IsNullOrWhiteSpace(webSocketId)) return Task.CompletedTask;

        context?.Response.Cookies.Append("webSocketId",webSocketId,
            new CookieOptions
            {
                Path = "/",
                Expires = DateTime.Now.AddMinutes(30),
                SameSite = SameSiteMode.None,
                Secure = true,
                HttpOnly = true
            });
        return Task.CompletedTask;
    }


    private async Task SendMessageAsync(WebSocket webSocket, string message)
    {
        byte[] messageBytes = Encoding.UTF8.GetBytes(message);
        var buffer = new ArraySegment<byte>(messageBytes);
        await webSocket.SendAsync(buffer, WebSocketMessageType.Text, true, CancellationToken.None);
    }

}