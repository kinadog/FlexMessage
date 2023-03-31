namespace FlexMessage.Messages.Types;
/// <summary>
/// 시스템 콘솔 메세지를 처리하는 클래스
/// Class for handling system console messages.
/// </summary>
public class ConsoleMessage : IMessage
{
    /// <summary>
    /// 시스템 콘솔에 메세지를 전송한다.
    /// Send a message to the system console.
    /// </summary>
    public void Write(object? message, SendTo? sendTo = null)
    {
        if (message == null) return;

        var msg = message as string;
        Console.WriteLine(msg);
    }


    /// <summary>
    /// 시스템 콘솔에 메세지를 전송한다. (비동기)
    /// Send a message to the system console. (asynchronous)
    /// </summary>
    public Task WriteAsync(object? message, SendTo? sendTo = null)
    {
        if (message == null) return Task.CompletedTask;

        var msg = message as string;
        Console.WriteLine(msg);
        return Task.CompletedTask;
    }
    
}