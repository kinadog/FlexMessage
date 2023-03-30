using System.Text.Json;
using Demo.Models;
using Microsoft.EntityFrameworkCore;

namespace Demo.Messages.Types;

/// <summary>
/// 데이터베이스로 기록되는 메세지 처리하는 클래스
/// Class for handles messages that are save in a database.
/// </summary>
public class DbMessage : IMessage
{
    #region Field
    private readonly Action<string>? _saveMessageAction;
    private readonly IMessageCommon? _messageCommon;
    private static EfDbContext? _efDbContext;


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
        _efDbContext = new EfDbContext(new DbContextOptionsBuilder<EfDbContext>().UseInMemoryDatabase("SampleDataBase").Options);
    }

    #endregion


    #region Method

    /// <summary>
    /// 데이터베이스에 메세지를 저장한다.
    /// Saves a message to the database.
    /// </summary>
    /// <param name="message">
    /// 저장할 메세지
    /// message to save
    /// </param>
    public void Write(string? message)
    {
        if (string.IsNullOrWhiteSpace(message)) return;

        try
        {
            _saveMessageAction(message);
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
        }

        /*
        브라우저에 Db 데이터 업데이트 전송
        (불필요 시 삭제 해도 됩니다.)
        Sends a message to update Db data to the browser.
        (You can remove this code if it is not needed.)
        */

        var db = _efDbContext?.Logs?.LastOrDefault();
        _messageCommon?
            .Write(JsonSerializer.Serialize(db), MsgType.Db);
    }

    /// <summary>
    /// 데이터베이스에 메세지를 저장한다. (비동기)
    /// Saves a message to the database. (asynchronous)
    /// </summary>
    /// <param name="message">
    /// 저장할 메세지
    /// message to save
    /// </param>
    public async Task WriteAsync(string? message)
    {
        if (string.IsNullOrWhiteSpace(message)) return;

        try
        {
            await Task.Run(() =>
            {
                _saveMessageAction(message);
            });
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
        }

        /*
            브라우저에 Db 데이터 업데이트 전송
            (불필요 시 삭제 해도 됩니다.)
            Sends a message to update Db data to the browser.
            (You can remove this code if it is not needed.)
        */
        var db = _efDbContext?.Logs?.LastOrDefault();
        await _messageCommon?
            .WriteAsync(JsonSerializer.Serialize(db), MsgType.Db)!;
    }

    #endregion
}