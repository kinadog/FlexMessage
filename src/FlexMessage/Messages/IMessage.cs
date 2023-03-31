using FlexMessage.Messages.Types;

namespace FlexMessage.Messages;

public interface IMessage
{
    void Write(string? message, SendTo? sendTo = null);
    Task WriteAsync(string? message, SendTo? sendTo = null);
}