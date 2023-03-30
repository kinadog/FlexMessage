using System.Collections.Concurrent;
using System.Net.WebSockets;
using System.Text;
using System.Text.Json;

namespace FlexMessage.WebSockets;

public class WebSocketManager
{
    #region Field

    private readonly ConcurrentDictionary<string, WebSocket> _sockets;

    #endregion

    public WebSocketManager()
    {
        _sockets = new ConcurrentDictionary<string, WebSocket>();
    }

    #region Method

    /// <summary>
    /// Id로 웹소켓 가져오기
    /// </summary>
    public WebSocket? GetSocketById(string id)
    {
        return _sockets.TryGetValue(id, out var socket) ? socket : null;
    }

    /// <summary>
    /// 전체 WebSocket 가져오기
    /// </summary>
    public IEnumerable<WebSocket> GetAllSockets()
    {
        return _sockets.Values;
    }


    /// <summary>
    /// Add WebSocket
    /// </summary>
    public string AddWebSocket(WebSocket socket)
    {
        var id = Guid.NewGuid().ToString();
        _sockets.TryAdd(id, socket);
        return id;
    }

    /// <summary>
    /// Remove WebSocket (비동기)
    /// </summary>
    public async Task RemoveWebSocketAsync(WebSocket webSocket)
    {
        if (_sockets.Values.Contains(webSocket))
        {
            await webSocket.CloseAsync(WebSocketCloseStatus.NormalClosure, 
                "WebSocket connection closed", CancellationToken.None);
            var key = _sockets.FirstOrDefault(x => x.Value == webSocket).Key;
            if(!string.IsNullOrEmpty(key))
                _sockets.TryRemove(key, out _);
        }
    }

    /// <summary>
    /// Remove WebSocket
    /// </summary>
    public void RemoveWebSocket(WebSocket webSocket)
    {
        if (!_sockets.Values.Contains(webSocket)) return;
        webSocket.CloseAsync(WebSocketCloseStatus.NormalClosure,
            "WebSocket connection closed", CancellationToken.None).Wait();
        var key = _sockets.FirstOrDefault(x => x.Value == webSocket).Key;
        if(!string.IsNullOrEmpty(key))
            _sockets.TryRemove(key, out _);
    }

    /// <summary>
    /// Remove WebSocket by Id
    /// </summary>
    public bool RemoveWebSocket(string id)
    {
        return _sockets.TryRemove(id, out _);
    }

    public async Task SendMessageAsync(WebSocket webSocket, WebSocketMessage? webSocketMessage)
    {
        var message = JsonSerializer.Serialize(webSocketMessage);
        var messageBytes = Encoding.UTF8.GetBytes(message);
        var buffer = new ArraySegment<byte>(messageBytes);
        await webSocket.SendAsync(buffer, WebSocketMessageType.Text, true, CancellationToken.None);
    }

    /// <summary>
    /// 전체 유저에게 메세지 보내기
    /// </summary>
    public async Task SendToAllAsync(string message)
    {
        var buffer = Encoding.UTF8.GetBytes(message);
        foreach (var webSocket in _sockets)
        {
            if (webSocket.Value.State == WebSocketState.Open)
            {
                await webSocket.Value.SendAsync(
                    new ArraySegment<byte>(buffer, 0, buffer.Length),
                    WebSocketMessageType.Text,
                    true,
                    CancellationToken.None
                );
            }
        }
    }

    #endregion
    
    
}