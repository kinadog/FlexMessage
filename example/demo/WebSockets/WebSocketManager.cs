using System.Collections.Concurrent;
using System.Net.WebSockets;
using System.Text;
using Demo.Messages;
using Demo.Messages.Types;

namespace Demo.WebSockets;

public class WebSocketMessage
{
    public bool IsId { get; set; }
    public MsgType? MsgType { get; set; }
    public string? Message { get; set; }
}

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

    public WebSocket? GetSocketById(string id)
    {
        return _sockets.TryGetValue(id, out var socket) ? socket : null;
    }


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
        if (_sockets.Values.Contains(webSocket))
        {
            webSocket.CloseAsync(WebSocketCloseStatus.NormalClosure,
                "WebSocket connection closed", CancellationToken.None).Wait();
            var key = _sockets.FirstOrDefault(x => x.Value == webSocket).Key;
            if(!string.IsNullOrEmpty(key))
                _sockets.TryRemove(key, out _);
        }
    }


    public bool RemoveWebSocket(string id)
    {
        return _sockets.TryRemove(id, out _);
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