using System.Net.WebSockets;
using System.Text;
// ReSharper disable UnusedVariable

namespace Demo.WebSockets;

public class WebSocketHandler : IWebSocketHandler
{
    #region Field

    private readonly WebSocketManager _webSocketManager;
    public HttpContext? HttpContext { get; set; }
    public WebSocket? WebSocket { get; set; }

    #endregion

    
    #region ctor

    public WebSocketHandler(WebSocketManager webSocketManager)
    {
        _webSocketManager = webSocketManager;
    }

    #endregion


    #region Method

    /// <summary>
    /// WebSocket 접속
    /// </summary>
    public async Task HandleWebSocketConnection(
    WebSocket? webSocket, 
    HttpContext httpContext)
{
    webSocket ??= await httpContext.WebSockets
        .AcceptWebSocketAsync().ConfigureAwait(false);
    
    var webSocketId = _webSocketManager.AddWebSocket(webSocket);
    HttpContext = httpContext;
    WebSocket = webSocket;

    await ReceiveMessageAsync(webSocket, (result, buffer) =>
        {
            if (result.MessageType == WebSocketMessageType.Text)
            {
                var message = Encoding.UTF8
                    .GetString(buffer, 0, result.Count);
            }
            return Task.CompletedTask;
        });

    await HandleWebSocketDisconnection(webSocket);
}
    
    
    /// <summary>
    /// Client -> Server 메세지 수신
    /// </summary>
    public async Task ReceiveMessageAsync(
        WebSocket webSocket,
        Func<WebSocketReceiveResult, byte[], Task> handleMessage)
    {
        var buffer = new byte[1024 * 4];

        while (webSocket.State == WebSocketState.Open)
        {
            var result = await webSocket.ReceiveAsync(
                new ArraySegment<byte>(buffer),
                CancellationToken.None);
            await handleMessage(result, buffer);
        }

        await HandleWebSocketDisconnection(webSocket);
    }


    /// <summary>
    /// Server -> Client 메세지 발신
    /// </summary>
    public async Task SendMessageAsync(
        WebSocket? webSocket, 
        string message)
    {
       webSocket ??= WebSocket;
        
        var messageBytes = Encoding.UTF8.GetBytes(message);
        var messageBuffer = new ArraySegment<byte>(messageBytes);

        if (webSocket!.State == WebSocketState.Closed)
        {
            if (HttpContext != null) 
                await HandleWebSocketConnection(webSocket, HttpContext);
        }
        
        if (webSocket.State == WebSocketState.Open)
        {
            await webSocket.SendAsync(
                messageBuffer,
                WebSocketMessageType.Text,
                true,
                CancellationToken.None
            );
        }
    }
    
    
    /// <summary>
    /// WebSocket 종료
    /// </summary>
    public async Task HandleWebSocketDisconnection(WebSocket webSocket)
    {
        await _webSocketManager.RemoveWebSocketAsync(webSocket);
    }

    #endregion
}