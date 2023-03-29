using System.Net.WebSockets;

namespace Demo.WebSockets;

public interface IWebSocketHandler
{
    HttpContext? HttpContext { get; set; }
    WebSocket? WebSocket { get; set; }
    
    
    /// <summary>
    /// WebSocket 접속
    /// </summary>
    Task HandleWebSocketConnection(WebSocket? webSocket, HttpContext httpContext);
    

    /// <summary>
    /// Client -> Server 메세지 수신
    /// </summary>
    Task ReceiveMessageAsync(
        WebSocket webSocket,
        Func<WebSocketReceiveResult, byte[], Task> handleMessage);
    

    /// <summary>
    /// Server -> Client 메세지 발신
    /// </summary>
    Task SendMessageAsync(WebSocket? webSocket, string message);
    
    
    /// <summary>
    /// WebSocket 종료
    /// </summary>
    Task HandleWebSocketDisconnection(WebSocket webSocket);
}