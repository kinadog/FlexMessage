using System.Net.WebSockets;
using Demo.Configs;
using Demo.Messages;
using Demo.Messages.Types;
using Demo.WebSockets;
using WebSocketManager = Demo.WebSockets.WebSocketManager;

namespace Demo.Middlewares
{

    /// <summary>
    /// WebSocket에서 사용할 미들웨어 클래스입니다.
    /// Middleware that sets up the configuration for
    /// logging messages via various channels, including console, file, and browser.
    /// </summary>
    public class WebSocketMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly WebSocketManager _webSocketManager;
        private static IMessageCommon? _messageCommon;

        public WebSocketMiddleware(
            RequestDelegate next,
            WebSocketManager webSocketManager,
            IMessageCommon messageCommon)
        {
            _next = next;
            _webSocketManager = webSocketManager;
            _messageCommon = messageCommon;
        }

        // 미들웨어 파이프라인에서 다음 단계를 처리합니다.
        // Handles the next step in the middleware pipeline.
        public async Task InvokeAsync(HttpContext context)
        {
            // Message 객체를 초기화 합니다.
            // Initialize the Message object.
            var serviceProvider = context.RequestServices;
            Message.Configure(serviceProvider, _messageCommon);

            // HTTP 쿠키에서 connectionId 값을 가져옵니다.
            // Gets the connectionId value from the HTTP cookie.
            var webSocketId = context.Request.Cookies["webSocketId"];
            MessageCommon._webSocketId = webSocketId;

            // HttpContext를 기반으로 FileMessageCngMonitor 객체를 초기화합니다.
            // Initializes the FileMessageCngMonitor object based on the HttpContext.
            // ↓ 웹페이지의 실시간 파일 View 기능이 필요 없으시면 비 활성화 하셔도 됩니다.
            // ↓ If real-time file view feature is not needed, you can disable it.
            FileMessageCngMonitor.Configure(_messageCommon);

            // HTTP 요청의 스키마(HTTP 또는 HTTPS)를 기반으로 Config.Host 값을 설정합니다.
            // Sets the host URL in the configuration based on the request scheme and host
            Config.Host = context.Request.Scheme == "https"
                ? $"https://{context.Request.Host}"
                : $"http://{context.Request.Host}";

            if (IsWebSocketRequest(context))
            {
                // 웹소켓 연결을 처리하고 통신을 시작합니다.
                // Handles the WebSocket connection and initiates communication.
                await HandleWebSocketConnectionAsync(context);
            }
            else
            {
                await _next(context);
            }
        }

        // 웹소켓 요청인지 확인합니다.
        // Checks if the request is a WebSocket request.
        private static bool IsWebSocketRequest(HttpContext context)
        {
            return context.Request.Path == "/ws" && context.WebSockets.IsWebSocketRequest;
        }

        // 웹소켓 연결을 처리합니다.
        // Handles the WebSocket connection.
        private async Task HandleWebSocketConnectionAsync(HttpContext context)
        {
            var webSocket = await AcceptWebSocket(context);
            var webSocketId = AddWebSocketToManager(webSocket);
            await SendWebSocketId(webSocket, webSocketId);

            await ProcessWebSocketCommunication(webSocket);
        }

        // 웹소켓을 수락합니다.
        // Accepts the WebSocket.
        private static async Task<WebSocket> AcceptWebSocket(HttpContext context)
        {
            return await context.WebSockets.AcceptWebSocketAsync();
        }

        // 웹소켓을 관리자에 추가합니다.
        // Adds the WebSocket to the manager.
        private string AddWebSocketToManager(WebSocket webSocket)
        {
            return _webSocketManager.AddWebSocket(webSocket);
        }

        // 웹소켓 ID를 전송합니다.
        // Sends the WebSocket ID.
        private async Task SendWebSocketId(WebSocket webSocket, string webSocketId)
        {
            var webSocketMessage = new WebSocketMessage
            {
                IsId = true,
                Message = webSocketId
            };

            await _webSocketManager.SendMessageAsync(webSocket, webSocketMessage);
        }

        // 웹소켓 통신을 처리합니다.
        // Processes the WebSocket communication.
        private static async Task ProcessWebSocketCommunication(WebSocket webSocket)
        {
            var buffer = new byte[1024 * 4];

            while (webSocket.State == WebSocketState.Open)
            {
                var result = await webSocket.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);

                if (result.MessageType == WebSocketMessageType.Text)
                {
                    await webSocket.SendAsync(new ArraySegment<byte>(buffer, 0, result.Count), WebSocketMessageType.Text, result.EndOfMessage, CancellationToken.None);
                }
                else if (result.MessageType == WebSocketMessageType.Close)
                {
                    await webSocket.CloseAsync(WebSocketCloseStatus.NormalClosure, "Closing", CancellationToken.None);
                    break;
                }
            }
        }
    }
}