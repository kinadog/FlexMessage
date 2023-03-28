namespace Demo2.Messages;

public interface IMessage
{
    void Write(string? message);
    Task WriteAsync(string? message);
}