namespace Demo.Messages.Types;

public interface IMessageCommon
{
    /// <summary>
    /// webSocketId�� �̿��Ͽ� Ŭ���̾�Ʈ���� �޼����� �����Ѵ�.
    /// Send a message to the client using the webSocketId.
    /// </summary>
    ///
    void Write(string? message, MsgType msgType);

    /// <summary>
    /// webSocketId�� �̿��Ͽ� Ŭ���̾�Ʈ���� �޼����� �����Ѵ�. (�񵿱�)
    /// Send a message to the client using the webSocketId. (asynchronous)
    /// </summary>
    Task WriteAsync(string? message, MsgType msgType);
}