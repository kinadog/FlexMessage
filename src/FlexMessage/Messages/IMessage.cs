using System.Threading.Tasks;

namespace FlexMessage.Messages;

public interface IMessage
{
    void Write(string? message);
    Task WriteAsync(string? message);
}