namespace FlexMessage.Messages.Types;

/// <summary>
/// 브라우저 콘솔 메세지를 처리하는 클래스
/// Class for handling browser console messages.
/// </summary>
public class BrowserConsoleMessage : IMessage
{
    #region Field

    private readonly IMessageCommon? _messageCommon;

    #endregion


    #region ctor
    public BrowserConsoleMessage(IMessageCommon? messageCommon)
    {
        _messageCommon = messageCommon;
    }
    #endregion


    #region Method

    /// <summary>
    /// webSocketId를 이용하여 클라이언트에게 메세지를 전송한다.
    /// Send a message to the client using the webSocketId.
    /// </summary>
    public void Write(object? message, SendTo? sendTo = null)
    {
        var msg = message as string;
        if (sendTo == SendTo.All)
        {
            _messageCommon?.WriteAll(msg, MsgType.BrowserConsole);
        }
        else
        {
            _messageCommon?.Write(msg, MsgType.BrowserConsole);
        }
    }

    /// <summary>
    /// webSocketId를 이용하여 클라이언트에게 메세지를 전송한다. (비동기)
    /// Send a message to the client using the webSocketId. (asynchronous)
    /// </summary>
    public async Task WriteAsync(object? message, SendTo? sendTo = null)
    {
        var msg = message as string;
        if (sendTo == SendTo.All)
        {
            await _messageCommon!.WriteAllAsync(msg, MsgType.BrowserConsole);
        }
        else
        {
            await _messageCommon!.WriteAsync(msg, MsgType.BrowserConsole);
        }
    }

    #endregion
}