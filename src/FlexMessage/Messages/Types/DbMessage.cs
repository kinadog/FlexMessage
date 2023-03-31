namespace FlexMessage.Messages.Types;

/// <summary>
/// 데이터베이스로 기록되는 메세지 처리하는 클래스
/// Class for handles messages that are save in a database.
/// </summary>
public class DbMessage : IMessage
{
    #region Field
    private readonly Action<string>? _saveMessageAction;
    private readonly IMessageCommon? _messageCommon;


    #endregion


    #region ctor

    /// <summary>
    /// DbMessage의 생성자
    /// Initializes a new instance of the DbMessage class.
    /// </summary>
    public DbMessage(IMessageCommon? messageCommon,
        Action<string>? saveMessageAction)
    {
        _messageCommon = messageCommon;
        _saveMessageAction = saveMessageAction;
    }

    #endregion


    #region Method

    /// <summary>
    /// 데이터베이스에 메세지를 저장한다.
    /// Saves a message to the database.
    /// </summary>
    public void Write(string? message, SendTo? sendTo = null)
    {
        if (string.IsNullOrWhiteSpace(message)) return;

        try
        {
            _saveMessageAction?.Invoke(message);
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
        }
    }

    /// <summary>
    /// 데이터베이스에 메세지를 저장한다. (비동기)
    /// Saves a message to the database. (asynchronous)
    /// </summary>
    public async Task WriteAsync(string? message, SendTo? sendTo = null)
    {
        if (string.IsNullOrWhiteSpace(message)) return;

        try
        {
            await Task.Run(() =>
            {
                _saveMessageAction?.Invoke(message);
            });
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
        }
    }

    #endregion
}