using FlexMessage.Messages.Types;

namespace FlexMessage.Messages;

public interface IMessage
{
    void Write(object? message, SendTo? sendTo = null);
    Task WriteAsync(object? message, SendTo? sendTo = null);
}