using FlexMessage.Hubs;
using FlexMessage.Models;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace FlexMessage.Messages.Types;

/// <summary>
/// 데이터베이스로 기록되는 메세지 처리하는 클래스
/// Class for handles messages that are save in a database.
/// </summary>
public class DbMessage : IMessage
{
    #region Field

    private readonly string _connectionString;
    private static IHttpContextAccessor? _contextAccessor;
    private static IHubContext<MessageHub>? _hubContext;

    #endregion


    #region ctor

    /// <summary>
    /// DbMessage의 생성자
    /// Initializes a new instance of the DbMessage class.
    /// </summary>
    /// <param name="connectionString">데이터베이스 연결 문자열</param>
    /// <param name="contextAccessor">HttpContextAccessor 객체</param>
    public DbMessage(string connectionString,
        IHttpContextAccessor contextAccessor)
    {
        _connectionString = connectionString;
        _contextAccessor = contextAccessor;
        _hubContext = _contextAccessor.HttpContext!.RequestServices.GetRequiredService<IHubContext<MessageHub>>();
    }

    #endregion


    #region Method

    /// <summary>
    /// 데이터베이스에 메세지를 저장한다.
    /// Saves a message to the database.
    /// </summary>
    /// <param name="message">
    ///     저장할 메세지
    ///     message to save
    /// </param>
    public void Write(string? message)
    {
        if (string.IsNullOrWhiteSpace(message)) return;

        var newLogs = new Logs();
        try
        {
            /*
                 데이터베이스에 message를 입력하는 구문을 코딩합니다.
                 예제를 위하여 EF의 인메모리 데이터베이스를 사용합니다.
                 The code for inserting a message into the database is implemented.
                 In this example, an in-memory database of EF is used.
             */
            var options = new DbContextOptionsBuilder<EfDbContext>()
                .UseInMemoryDatabase("FlexMessageSampleDb")
                .Options;

            using var context = new EfDbContext(options);
            newLogs.Message = message;
            newLogs.Writedates = DateTime.Now;
            context.Logs!.Add(newLogs);
            context.SaveChangesAsync();
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

        // 쿠키에서 현재 접속중인 SignalR의 ConnectionId 가져오기
        // Get the ConnectionId from the cookie
        var getConnectionId =
            _contextAccessor!.HttpContext!
                .Request.Cookies.TryGetValue("signalr_connectionId",
                    out var connectionId);

        // connectionId 값으로 Client 메세지 전송 메소드 호출
        // Call the method to send the message to the client using the ConnectionId
        if (getConnectionId && !string.IsNullOrWhiteSpace(connectionId))
        {
            connectionId = Hashing.Decrypt(connectionId);
            _hubContext!.Clients.Client(connectionId!)
                .SendAsync("ReceiveMessage", "Db", JsonConvert.SerializeObject(newLogs));
        }
    }

    /// <summary>
    /// 데이터베이스에 메세지를 저장한다. (비동기)
    /// Saves a message to the database. (asynchronous)
    /// </summary>
    /// <param name="message">
    ///     저장할 메세지
    ///     message to save
    /// </param>
    public async Task WriteAsync(string? message)
    {
        if (string.IsNullOrWhiteSpace(message)) return;

        var newLogs = new Logs();
        try
        {
            /*
                 데이터베이스에 message를 입력하는 구문을 코딩합니다.
                 예제를 위하여 EF의 인메모리 데이터베이스를 사용합니다.
                 The code for inserting a message into the database is implemented.
                 In this example, an in-memory database of EF is used.
             */
            var options = new DbContextOptionsBuilder<EfDbContext>()
                .UseInMemoryDatabase("FlexMessageSampleDb")
                .Options;

            await using var context = new EfDbContext(options);
            newLogs.Message = message;
            newLogs.Writedates = DateTime.Now;
            context.Logs!.Add(newLogs);
            await context.SaveChangesAsync();
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

        // 쿠키에서 현재 접속중인 SignalR의 ConnectionId 가져오기
        // Get the ConnectionId from the cookie
        var getConnectionId =
            _contextAccessor!.HttpContext!
                .Request.Cookies.TryGetValue("signalr_connectionId",
                    out var connectionId);

        // connectionId 값으로 Client 메세지 전송 메소드 호출
        // Call the method to send the message to the client using the ConnectionId
        if (getConnectionId && !string.IsNullOrWhiteSpace(connectionId))
        {
            connectionId = Hashing.Decrypt(connectionId);
            await _hubContext!.Clients.Client(connectionId!)
                .SendAsync("ReceiveMessage", "Db", JsonConvert.SerializeObject(newLogs));
        }
    }

    #endregion
}