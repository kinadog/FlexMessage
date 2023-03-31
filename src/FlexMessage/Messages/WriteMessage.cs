using FlexMessage.Messages.Types;
using Microsoft.Extensions.DependencyInjection;

// ReSharper disable FieldCanBeMadeReadOnly.Local
// ReSharper disable CognitiveComplexity

namespace FlexMessage.Messages;

internal class WriteMessage
{
    #region Field

    private static IMessage? _fileMessage; // 파일 메세지 객체 (File message object)
    private static IMessage? _consoleMessage; // 콘솔 메세지 객체 (Console message object)
    private static IMessage? _browserConsoleMessage; // 브라우저 콘솔 메세지 객체 (Browser console message object)
    private static IMessage? _browserAlertMessage; // 브라우저 경고창 메세지 객체 (Browser alert message object)
    private static IMessage? _browserToastMessage; // 브라우저 토스트 메세지 객체 (Browser toast message object)
    private static IMessage? _ajaxMessage; // 아약스 인스턴스 메세지 객체 (Ajax Instance message object)
    private static IServiceProvider? _serviceProvider; // 서비스 제공자 객체 (Service provider object)
    private static IMessageCommon? _messageCommon;

    #endregion


    #region ctor

    internal WriteMessage(
        IMessage? fileMessage,
        IMessage? consoleMessage,
        IMessage? browserConsoleMessage,
        IMessage? browserAlertMessage,
        IMessage? browserToastMessage,
        IMessage? ajaxMessage,
        IServiceProvider? serviceProvider,
        IMessageCommon? messageCommon
        )
    {
        _fileMessage = fileMessage;
        _consoleMessage = consoleMessage;
        _browserConsoleMessage = browserConsoleMessage;
        _browserAlertMessage = browserAlertMessage;
        _browserToastMessage = browserToastMessage;
        _ajaxMessage = ajaxMessage;
        _serviceProvider = serviceProvider;
        _messageCommon = messageCommon;
    }

    #endregion


    #region Method

    // 입력된 메시지 타입에 따라 해당하는 객체의 Write 메서드를 호출합니다.
    // Depending on the input message type, calls the Write method of the corresponding object.
    internal void Write(object? message, MsgType? msgType, SendTo? sendTo)
    {
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
                _browserAlertMessage?.Write(message, sendTo);
                break;

            // 메시지 타입이 BrowserToast인 경우, 브라우저에서 Toast를 띄웁니다.
            // If the message type is BrowserToast, shows a toast in the browser.
            case MsgType.BrowserToast:
                _browserToastMessage?.Write(message, sendTo);
                break;

            // 메시지 타입이 BrowserConsole인 경우, 브라우저 콘솔에 로그를 작성합니다.
            // If the message type is BrowserConsole, writes the log to the browser console.
            case MsgType.BrowserConsole:
                _browserConsoleMessage?.Write(message, sendTo);
                break;

            // 메시지 타입이 Ajax인 경우, 브라우저에 Ajax 인스턴스를 전송합니다.
            // If the message type is Aajx, send the ajax instance to the browser.
            case MsgType.Json:
                _ajaxMessage?.Write(message, sendTo);
                break;

            // 메시지 타입이 정의되지 않은 경우, 기본 메시지 타입으로 설정합니다.
            // If the message type is undefined, sets the default message type and writes the log to the console.
            default:
                _consoleMessage?.Write(message);
                break;
        }
    }

    // 입력된 메시지 타입에 따라 해당하는 객체의 Write 메서드를 호출합니다. (비동기)
    // Depending on the input message type, calls the Write method of the corresponding object. (asynchronous)
    internal async Task WriteAsync(object? message, MsgType? msgType, SendTo? sendTo)
    {
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
                await _browserAlertMessage?.WriteAsync(message, sendTo)!;
                break;

            // 메시지 타입이 BrowserToast인 경우, 브라우저에서 Toast를 띄웁니다.
            // If the message type is BrowserToast, shows a toast in the browser.
            case MsgType.BrowserToast:
                await _browserToastMessage?.WriteAsync(message, sendTo)!;
                break;

            // 메시지 타입이 BrowserConsole인 경우, 브라우저 콘솔에 로그를 작성합니다.
            // If the message type is BrowserConsole, writes the log to the browser console.
            case MsgType.BrowserConsole:
                await _browserConsoleMessage?.WriteAsync(message, sendTo)!;
                break;

            // 메시지 타입이 Ajax인 경우, 브라우저에 Ajax 인스턴스를 전송합니다.
            // If the message type is Aajx, send the ajax instance to the browser.
            case MsgType.Json:
                await _ajaxMessage?.WriteAsync(message, sendTo)!;
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