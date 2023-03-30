using FlexMessage.WebSockets;
using Microsoft.AspNetCore.Http;

namespace FlexMessage.Messages.Types;

public class MessageCommon : IMessageCommon
{
    #region Field

    private static IHttpContextAccessor? _contextAccessor;
    private readonly WebSockets.WebSocketManager _webSocketManager;
    public static string? _webSocketId = string.Empty;

    #endregion


    #region ctor
    public MessageCommon(IHttpContextAccessor contextAccessor,
        WebSockets.WebSocketManager webSocketManager)
    {
        _webSocketManager = webSocketManager;
        _contextAccessor = contextAccessor;
    }
    #endregion

    /// <summary>
    /// webSocketId를 이용하여 클라이언트에게 메세지를 전송한다.
    /// Send a message to the client using the webSocketId.
    /// </summary>
    ///
    public void Write(string? message, MsgType msgType)
    {
        if (string.IsNullOrWhiteSpace(message)) return;

        var webSocketId =
            // 쿠키에서 현재 접속중인 클라이언트의 webSocketId 가져오기
            // Get the webSocketId from the cookie
            string.IsNullOrWhiteSpace(_webSocketId)
            ? _contextAccessor!.HttpContext!.Request.Cookies["webSocketId"]
            : _webSocketId;

        Task.Run(async () =>
        {
            // webSocketId 값으로 Client 메세지 전송 메소드 호출
            // Call the method to send the message to the client using the webSocketId
            if (webSocketId != null)
            {
                var websocket = _webSocketManager.GetSocketById(webSocketId);

                var webSocketMessage = new WebSocketMessage
                {
                    IsId = false,
                    MsgType = Enum.GetName(typeof(MsgType),msgType),
                    Message = message
                };
                if (websocket != null)
                    await _webSocketManager.SendMessageAsync(websocket, webSocketMessage);
            }
        });
    }

    /// <summary>
    /// webSocketId를 이용하여 클라이언트에게 메세지를 전송한다. (비동기)
    /// Send a message to the client using the webSocketId. (asynchronous)
    /// </summary>
    public async Task WriteAsync(string? message, MsgType msgType)
    {
        if (string.IsNullOrWhiteSpace(message)) return;

        var webSocketId =
            // 쿠키에서 현재 접속중인 클라이언트의 webSocketId 가져오기
            // Get the webSocketId from the cookie
            string.IsNullOrWhiteSpace(_webSocketId)
            ? _contextAccessor!.HttpContext!.Request.Cookies["webSocketId"]
            : _webSocketId;

        // webSocketId 값으로 Client 메세지 전송 메소드 호출
        // Call the method to send the message to the client using the webSocketId
        if (webSocketId != null)
        {
            var websocket = _webSocketManager.GetSocketById(webSocketId);

            var webSocketMessage = new WebSocketMessage
            {
                IsId = false,
                MsgType = Enum.GetName(typeof(MsgType),msgType),
                Message = message
            };
            if (websocket != null)
                await _webSocketManager.SendMessageAsync(websocket, webSocketMessage);
        }
    }
}