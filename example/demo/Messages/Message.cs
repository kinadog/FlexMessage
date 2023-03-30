using Demo.Messages.Types;

// ReSharper disable CyclomaticComplexity
// ReSharper disable CognitiveComplexity
// ReSharper disable InconsistentNaming
// ReSharper disable FieldCanBeMadeReadOnly.Local

namespace Demo.Messages;

/// <summary>
/// 콘솔, 파일, 데이터베이스 및 브라우저와 같은 다양한 유형의 메시지 로깅을 처리하는 클래스입니다.
/// A class that handles various types of message logging, including console, file, database, and browser.
/// </summary>
public static class Message
{
    #region Field

    private static IMessage? _fileMessage; // 파일 메세지 객체 (File message object)
    private static IMessage? _consoleMessage; // 콘솔 메세지 객체 (Console message object)
    //private static IMessage? _dbMessage; // 데이터베이스 메세지 객체 (Database message object)
    private static IMessage? _browserConsoleMessage; // 브라우저 콘솔 메세지 객체 (Browser console message object)
    private static IMessage? _browserAlertMessage; // 브라우저 경고창 메세지 객체 (Browser alert message object)
    private static IMessage? _browserToastMessage; // 브라우저 토스트 메세지 객체 (Browser toast message object)
    private static IServiceProvider? _serviceProvider; // 서비스 제공자 객체 (Service provider object)
    private static IMessageCommon? _messageCommon;
    #endregion


    #region static ctor

    static Message()
    {
        // 콘솔과 파일 메세지 객체 생성
        // Initializes the default message types, console and file.
        _consoleMessage = new ConsoleMessage();
        _fileMessage = new FileMessage();
    }

    #endregion


    #region Method

    /// <summary>
    /// WebSocket을 사용하기 위해 HttpContext를 주입하는 메서드입니다.
    /// A method that injects an HttpContext for using WebSocket.
    /// </summary>
    public static void Configure(
        IServiceProvider serviceProvider,
        IMessageCommon? messageCommon)
    {
        _serviceProvider = serviceProvider;
        _messageCommon = messageCommon;
        _browserToastMessage = new BrowserToastMessage(_messageCommon);
        _browserAlertMessage = new BrowserAlertMessage(_messageCommon);
        _browserConsoleMessage = new BrowserConsoleMessage(_messageCommon);
    }

    /// <summary>
    /// 메세지 타입에 따라 로그 메세지를 작성하는 메서드입니다.
    /// A method that writes messages of various types
    /// depending on the MessageType parameter.
    /// </summary>
    /// <param name="message">
    /// 메세지 내용
    /// The content of the message.
    /// </param>
    /// <param name="msgTypes">
    /// [ARRAY] 메세지 종류
    /// [ARRAY] The type(s) of the message.
    /// </param>
    public static void Write(string? message, params MsgType?[]? msgTypes)
    {
        // MessageType 매개변수를 입력받지 않았을 때, 기본 메시지 타입으로 설정합니다.
        // Sets the default message type to console when no MessageType parameter is passed in.
        if (msgTypes?.Length == 0)
        {
            _consoleMessage?.Write(message);
            return;
        }

        // 입력된 메시지 타입에 따라 해당하는 객체의 Write 메서드를 호출합니다.
        // Depending on the input message type, calls the Write method of the corresponding object.
        var listMsgType = new List<MsgType?>();
        foreach (var msgType in msgTypes!)
            switch (msgType)
            {
                // 메시지 타입이 null인 경우, 기본 메시지 타입으로 설정합니다.
                // If the message type is null, sets the default message type and writes the log to the console.
                case null:
                    _consoleMessage?.Write(message);
                    break;

                // 메시지 타입이 File인 경우, 파일에 로그를 작성합니다.
                // If the message type is File, writes the log to the file.
                case MsgType.File:
                    _fileMessage?.Write(message);
                    break;

                // 메시지 타입이 Console인 경우, 콘솔에 로그를 작성합니다.
                // If the message type is Console, writes the log to the console.
                case MsgType.Console:
                    _consoleMessage?.Write(message);
                    break;

                // 메시지 타입이 Db인 경우, 데이터베이스에 로그를 작성합니다.
                // If the message type is Db, writes the log to the database.
                case MsgType.Db:
                    if (_serviceProvider != null)
                    {
                        Task.Run(async () =>
                        {
                            using var scope = _serviceProvider.CreateScope();
                            var dbMessage = scope.ServiceProvider.GetRequiredService<DbMessage>();
                            await dbMessage.WriteAsync(message);
                        });
                    }
                    break;

                // 메시지 타입이 BrowserAlert인 경우, 브라우저에서 Alert를 띄웁니다.
                // If the message type is BrowserAlert, shows an alert in the browser.
                case MsgType.BrowserAlert:
                    _browserAlertMessage?.Write(message);
                    break;

                // 메시지 타입이 BrowserToast인 경우, 브라우저에서 Toast를 띄웁니다.
                // If the message type is BrowserToast, shows a toast in the browser.
                case MsgType.BrowserToast:
                    _browserToastMessage?.Write(message);
                    break;

                // 메시지 타입이 BrowserConsole인 경우, 브라우저 콘솔에 로그를 작성합니다.
                // If the message type is BrowserConsole, writes the log to the browser console.
                case MsgType.BrowserConsole:
                    _browserConsoleMessage?.Write(message);
                    break;

                // 메시지 타입이 정의되지 않은 경우, 기본 메시지 타입으로 설정합니다.
                // If the message type is undefined, sets the default message type and writes the log to the console.
                default:
                    _consoleMessage?.Write(message);
                    break;
            }
    }


    /// <summary>
    /// 메세지 타입에 따라 로그 메세지를 작성하는 메서드입니다. (비동기)
    /// A method that writes messages of various types (asynchronous)
    /// depending on the MessageType parameter.
    /// </summary>
    /// <param name="message">
    /// 메세지 내용
    /// The content of the message.
    /// </param>
    /// <param name="msgTypes">
    /// [ARRAY] 메세지 종류
    /// [ARRAY] The type(s) of the message.
    /// </param>
    public static async Task WriteAsync(string? message, params MsgType?[]? msgTypes)
    {
        // MessageType 매개변수를 입력받지 않았을 때, 기본 메시지 타입으로 설정합니다.
        // Sets the default message type to console when no MessageType parameter is passed in.
        if (msgTypes?.Length == 0)
        {
            await _consoleMessage?.WriteAsync(message)!;
            return;
        }

        // 입력된 메시지 타입에 따라 해당하는 객체의 Write 메서드를 호출합니다.
        // Depending on the input message type, calls the Write method of the corresponding object.
        var listMsgType = new List<MsgType?>();
        foreach (var msgType in msgTypes!)
            switch (msgType)
            {
                // 메시지 타입이 null인 경우, 기본 메시지 타입으로 설정합니다.
                // If the message type is null, sets the default message type and writes the log to the console.
                case null:
                    await _consoleMessage?.WriteAsync(message)!;
                    break;

                // 메시지 타입이 File인 경우, 파일에 로그를 작성합니다.
                // If the message type is File, writes the log to the file.
                case MsgType.File:
                    await _fileMessage?.WriteAsync(message)!;
                    break;

                // 메시지 타입이 Console인 경우, 콘솔에 로그를 작성합니다.
                // If the message type is Console, writes the log to the console.
                case MsgType.Console:
                    await _consoleMessage?.WriteAsync(message)!;
                    break;

                // 메시지 타입이 Db인 경우, 데이터베이스에 로그를 작성합니다.
                // If the message type is Db, writes the log to the database.
                case MsgType.Db:
                    if (_serviceProvider != null)
                    {
                        using var scope = _serviceProvider.CreateScope();
                        var dbMessage = scope.ServiceProvider.GetRequiredService<DbMessage>();
                        await dbMessage.WriteAsync(message);
                    }
                    break;

                // 메시지 타입이 BrowserAlert인 경우, 브라우저에서 Alert를 띄웁니다.
                // If the message type is BrowserAlert, shows an alert in the browser.
                case MsgType.BrowserAlert:
                    await _browserAlertMessage?.WriteAsync(message)!;
                    break;

                // 메시지 타입이 BrowserToast인 경우, 브라우저에서 Toast를 띄웁니다.
                // If the message type is BrowserToast, shows a toast in the browser.
                case MsgType.BrowserToast:
                    await _browserToastMessage?.WriteAsync(message)!;
                    break;

                // 메시지 타입이 BrowserConsole인 경우, 브라우저 콘솔에 로그를 작성합니다.
                // If the message type is BrowserConsole, writes the log to the browser console.
                case MsgType.BrowserConsole:
                    await _browserConsoleMessage?.WriteAsync(message)!;
                    break;

                // 메시지 타입이 정의되지 않은 경우, 기본 메시지 타입으로 설정합니다.
                // If the message type is undefined, sets the default message type and writes the log to the console.
                default:
                    await _consoleMessage?.WriteAsync(message)!;
                    break;
            }
    }

    #endregion
}