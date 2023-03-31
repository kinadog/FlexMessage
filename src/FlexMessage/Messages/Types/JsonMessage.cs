namespace FlexMessage.Messages.Types;

public class JsonMessage : IMessage
{
    #region Field

    private readonly IMessageCommon? _messageCommon;

    #endregion


    #region Method


    #region ctor
    public JsonMessage(IMessageCommon? messageCommon)
    {
        _messageCommon = messageCommon;
    }
    #endregion

    /// <summary>
    /// webSocketId를 이용하여 클라이언트에게 메세지를 전송한다.
    /// Send a message to the client using the webSocketId.
    /// </summary>
    public void Write(object? message, SendTo? sendTo = null)
    {
        if (message == null) return;

        message = System.Text.Json.JsonSerializer.Serialize(message);
        if (sendTo == SendTo.All)
        {
            _messageCommon?.WriteAll(message.ToString(), MsgType.Json);
        }
        else
        {
            _messageCommon?.Write(message.ToString(), MsgType.Json);
        }
    }

    /// <summary>
    /// webSocketId를 이용하여 클라이언트에게 메세지를 전송한다. (비동기)
    /// Send a message to the client using the webSocketId. (asynchronous)
    /// </summary>
    public async Task WriteAsync(object? message, SendTo? sendTo = null)
    {
        if (message == null) return;

        message = System.Text.Json.JsonSerializer.Serialize(message);
        if (sendTo == SendTo.All)
        {
            await _messageCommon!.WriteAllAsync(message.ToString(), MsgType.Json);
        }
        else
        {
            await _messageCommon!.WriteAsync(message.ToString(), MsgType.Json);
        }
    }

    #endregion
}