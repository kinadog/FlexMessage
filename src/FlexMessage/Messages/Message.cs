using FlexMessage.Messages.Types;
using Microsoft.Extensions.DependencyInjection;

// ReSharper disable CyclomaticComplexity
// ReSharper disable CognitiveComplexity
// ReSharper disable InconsistentNaming
// ReSharper disable FieldCanBeMadeReadOnly.Local

namespace FlexMessage.Messages;

/// <summary>
/// 콘솔, 파일, 데이터베이스 및 브라우저와 같은 다양한 유형의 메시지 로깅을 처리하는 클래스입니다.
/// A class that handles various types of message logging, including console, file, database, and browser.
/// </summary>
public static class Message
{
    #region Field

    private static IMessage? _fileMessage; // 파일 메세지 객체 (File message object)
    private static IMessage? _consoleMessage; // 콘솔 메세지 객체 (Console message object)
    private static IMessage? _browserConsoleMessage; // 브라우저 콘솔 메세지 객체 (Browser console message object)
    private static IMessage? _browserAlertMessage; // 브라우저 경고창 메세지 객체 (Browser alert message object)
    private static IMessage? _browserToastMessage; // 브라우저 토스트 메세지 객체 (Browser toast message object)
    private static IMessage? _jsonMessage; // 아약스 인스턴스 메세지 객체 (Ajax Instance message object)
    private static IServiceProvider? _serviceProvider; // 서비스 제공자 객체 (Service provider object)
    private static IMessageCommon? _messageCommon;
    private static WriteMessage? _writeMessage;
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
        _jsonMessage = new JsonMessage(_messageCommon);
        _writeMessage = new WriteMessage(
            _fileMessage,
            _consoleMessage,
            _browserConsoleMessage,
            _browserAlertMessage,
            _browserToastMessage,
            _jsonMessage,
            _serviceProvider,
            _messageCommon);
    }

    /// <summary>
    /// 메세지 타입에 따라 메세지를 작성하는 메서드입니다.
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
    public static void Write(object? message, params MsgType?[]? msgTypes)
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
        {
            _writeMessage?.Write(message, msgType, null);
        }
    }


    /// <summary>
    /// 메세지 타입에 따라 메세지를 작성하는 메서드입니다. (비동기)
    /// A method that writes messages of various types
    /// depending on the MessageType parameter. (asynchronous)
    /// </summary>
    /// <param name="message">
    /// 메세지 내용
    /// The content of the message.
    /// </param>
    /// <param name="msgTypes">
    /// [ARRAY] 메세지 종류
    /// [ARRAY] The type(s) of the message.
    /// </param>
    public static async Task WriteAsync(object? message, params MsgType?[]? msgTypes)
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
        {
            await _writeMessage!.WriteAsync(message, msgType, null);
        }
    }


    /// <summary>
    /// 메세지 타입과 전체발신/개별발신을 선택하여 메세지를 작성하는 메서드입니다.
    /// A method that allows you to select the message type
    /// and choose between sending messages to everyone or individually,
    /// then compose the message.
    /// </summary>
    /// <param name="message">
    /// 메세지 내용
    /// The content of the message.
    /// </param>
    /// <param name="msgType">
    /// 메세지 종류
    /// The type of the message.
    /// </param>
    /// <param name="sendTo">
    /// 전체발신/개별발신 여부
    /// Indicates whether to send the message to everyone or individually.
    /// </param>
    public static void Write(object? message, MsgType? msgType, SendTo sendTo)
    {
        _writeMessage?.Write(message, msgType, sendTo);
    }


    /// <summary>
    /// 메세지 타입과 전체발신/개별발신을 선택하여 메세지를 작성하는 메서드입니다. (비동기)
    /// A method that allows you to select the message type
    /// and choose between sending messages to everyone or individually,
    /// then compose the message. (asynchronous)
    /// </summary>
    /// <param name="message">
    /// 메세지 내용
    /// The content of the message.
    /// </param>
    /// <param name="msgType">
    /// 메세지 종류
    /// The type of the message.
    /// </param>
    /// <param name="sendTo">
    /// 전체발신/개별발신 여부
    /// Indicates whether to send the message to everyone or individually.
    /// </param>
    public static async Task WriteAsync(object? message, MsgType? msgType, SendTo sendTo)
    {
        await _writeMessage!.WriteAsync(message, msgType, sendTo);
    }


    #region 단축어 실행 메소드 (Shortcut execution method)

    /// <summary>
    /// Browser Console.log Message
    /// </summary>
    public static void Log(string? message)
    {
        _browserAlertMessage?.Write(message);
    }

    /// <summary>
    /// Browser Console.log Message (Select Target/All)
    /// </summary>
    public static void Log(string? message, SendTo sendTo)
    {
        _browserAlertMessage?.Write(message, sendTo);
    }

    /// <summary>
    /// Browser Alert() Message
    /// </summary>
    public static void Alert(string? message)
    {
        _browserAlertMessage?.Write(message);
    }

    /// <summary>
    /// Browser Alert() Message (Select Target/All)
    /// </summary>
    public static void Alert(string? message, SendTo sendTo)
    {
        _browserAlertMessage?.Write(message, sendTo);
    }


    /// <summary>
    /// Browser Toast Message
    /// </summary>
    public static void Toast(string? message)
    {
        _browserToastMessage?.Write(message);
    }

    /// <summary>
    /// Browser Toast Message (Select Target/All)
    /// </summary>
    public static void Toast(string? message, SendTo sendTo)
    {
        _browserAlertMessage?.Write(message, sendTo);
    }


    /// <summary>
    /// Json Object Message
    /// </summary>
    public static void Json(string? message)
    {
        _jsonMessage?.Write(message);
    }

    /// <summary>
    /// Json Object Message (Select Target/All)
    /// </summary>
    public static void Json(string? message, SendTo sendTo)
    {
        _jsonMessage?.Write(message, sendTo);
    }

    /// <summary>
    /// System Console Message
    /// </summary>
    public static void Console(string? message)
    {
        _consoleMessage?.Write(message);
    }

    /// <summary>
    /// Write to File Message
    /// </summary>
    public static async Task File(string? message)
    {
        await _fileMessage?.WriteAsync(message)!;
    }

    /// <summary>
    /// Intert to Database Message
    /// </summary>
    public static async Task Db(string? message)
    {
        if (_serviceProvider != null)
        {
            using var scope = _serviceProvider.CreateScope();
            var dbMessage = scope.ServiceProvider.GetRequiredService<DbMessage>();
            await dbMessage.WriteAsync(message);
        }
    }

    #endregion


    #endregion
}