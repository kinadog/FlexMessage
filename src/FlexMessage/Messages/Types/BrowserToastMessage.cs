namespace FlexMessage.Messages.Types;

public class BrowserToastMessage : IMessage
{
    #region Field

    private readonly IMessageCommon? _messageCommon;

    #endregion


    #region ctor
    public BrowserToastMessage(IMessageCommon? messageCommon)
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
            _messageCommon?.WriteAll(msg, MsgType.BrowserToast);
        }
        else
        {
            _messageCommon?.Write(msg, MsgType.BrowserToast);
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
            await _messageCommon!.WriteAllAsync(msg, MsgType.BrowserToast);
        }
        else
        {
            await _messageCommon!.WriteAsync(msg, MsgType.BrowserToast);
        }
    }

    #endregion
}