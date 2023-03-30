namespace FlexMessage.Messages.Types;

/// <summary>
/// 브라우저 알림 메세지를 처리하는 클래스
/// Class for handling browser alert messages.
/// </summary>
public class BrowserAlertMessage : IMessage
{
    #region Field

    private readonly IMessageCommon? _messageCommon;

    #endregion


    #region ctor
    public BrowserAlertMessage(IMessageCommon? messageCommon)
    {
        _messageCommon = messageCommon;
    }
    #endregion

    #region Method

    /// <summary>
    /// webSocketId를 이용하여 클라이언트에게 메세지를 전송한다.
    /// Send a message to the client using the webSocketId.
    /// </summary>
    public void Write(string? message)
    {
        _messageCommon?.Write(message, MsgType.BrowserAlert);
    }

    /// <summary>
    /// webSocketId를 이용하여 클라이언트에게 메세지를 전송한다. (비동기)
    /// Send a message to the client using the webSocketId. (asynchronous)
    /// </summary>
    public async Task WriteAsync(string? message)
    {
        await _messageCommon?.WriteAsync(message, MsgType.BrowserConsole)!;
    }

    #endregion


}