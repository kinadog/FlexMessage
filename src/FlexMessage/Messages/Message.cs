using FlexMessage.Messages.Types;

// ReSharper disable CyclomaticComplexity
// ReSharper disable CognitiveComplexity
// ReSharper disable InconsistentNaming
// ReSharper disable FieldCanBeMadeReadOnly.Local

namespace FlexMessage.Messages;

/// <summary>
///     콘솔, 파일, 데이터베이스 및 브라우저와 같은 다양한 유형의 메시지 로깅을 처리하는 클래스입니다.
///     A class that handles various types of message logging, including console, file, database, and browser.
/// </summary>
public static class Message
{
    #region Field

    private static IMessage? _fileMessage; // 파일 메세지 객체 (File message object)
    private static IMessage? _consoleMessage; // 콘솔 메세지 객체 (Console message object)
    private static IMessage? _dbMessage; // 데이터베이스 메세지 객체 (Database message object)
    private static IMessage? _browserConsoleMessage; // 브라우저 콘솔 메세지 객체 (Browser console message object)
    private static IMessage? _browserAlertMessage; // 브라우저 경고창 메세지 객체 (Browser alert message object)
    private static IMessage? _browserToastMessage; // 브라우저 토스트 메세지 객체 (Browser toast message object)
    private static IHttpContextAccessor? _httpContextAccessor; // HTTP 컨텍스트 객체 (HTTP context object)

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
    ///     SignalR을 사용하기 위해 HttpContext를 주입하는 메서드입니다.
    ///     A method that injects an HttpContext for using SignalR.
    /// </summary>
    /// <param name="httpContextAccessor">HTTP 컨텍스트 객체</param>
    public static void Configure(IHttpContextAccessor? httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
        // 주입된 HttpContext를 사용하여 브라우저별 메시지 유형을 초기화합니다.
        // Initializes browser-specific message types using the injected HttpContext.
        _browserToastMessage = new BrowserToastMessage(_httpContextAccessor!);
        _browserAlertMessage = new BrowserAlertMessage(_httpContextAccessor!);
        _browserConsoleMessage = new BrowserConsoleMessage(_httpContextAccessor!);

        /*
         아래의 코드는 웹 페이지에 실시간 파일 View와 Db View를 위해
         httpContextAccessor를 주입합니다.
         이 기능이 필요 없으시면, static Message() 정적 생성자로 옮기십시오.
         The following code injects the httpContextAccessor
         for real-time file and Db views on a web page.
         If you don't need this feature, you can move it
         to the static constructor, static Message().
         */
        const string connectionString = "YOUR_DB_CONNECT_STRING";
        _dbMessage = new DbMessage(connectionString, _httpContextAccessor!);
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
                    _dbMessage?.Write(message);
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
                    await _dbMessage?.WriteAsync(message)!;
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