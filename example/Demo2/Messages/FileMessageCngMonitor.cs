using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Demo2.Configs;
using Demo2.Hubs;
using Demo2.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Demo2.Messages;

public class FileMessageCngMonitor : BackgroundService
{
    #region Field

    private readonly string? _contentRootPath = Config.ContentRootPath; // 파일 디렉토리 경로 (File directory path)
    private readonly FileSystemWatcher? _watcher = new(); // 파일 시스템 감시 객체 (File system watcher object)
    private readonly IFileEndPosition? _fileEndPosition; // 파일 끝 지점 위치 인터페이스 (Interface for file end position)
    private static IHttpContextAccessor? _contextAccessor; // HTTP 컨텍스트 접근 인터페이스 (HTTP context access interface)

    private static IHubContext<MessageHub>? _hubContext; // SignalR Hub 컨텍스트 (SignalR Hub context)

    // ↓ 웹페이지의 실시간 파일 View 기능이 필요 없으시면 비활성화 해 주세요 (If you don't need real-time file view function on the web page, please deactivate)
    public static string?
        connectionId = string.Empty; // 웹페이지 실시간 파일 View 연결 ID (Web page real-time file view connection ID)

    #endregion


    #region ctor

    public FileMessageCngMonitor(IFileEndPosition fileEndPosition)
    {
        _fileEndPosition = fileEndPosition;
    }

    #endregion


    #region Method

    /*
          아래의 메서드는,
          웹페이지에 실시간 파일 View를 위해 httpContextAccessor를
          주입하여 HubContext의 인스턴스를 생성합니다.
          이 기능이 필요 없으시면, 메서드를 비활성화 시켜주세요.
          The following method injects the httpContextAccessor
          to create an instance of HubContext for real-time file view on a web page.
          If you don't need this feature, please disable the method.
     */
    public static void Configure(IHttpContextAccessor? contextAccessor)
    {
        _contextAccessor = contextAccessor; // HTTP 컨텍스트 접근 인터페이스 설정 (Set HTTP context access interface)
        _hubContext = _contextAccessor!.HttpContext!.RequestServices
            .GetRequiredService<IHubContext<MessageHub>>(); // SignalR Hub 컨텍스트 설정 (Set SignalR Hub context)
    }


    /// <summary>
    ///     파일 변경 실시간 감지 시작
    ///     Start real-time file change detection
    /// </summary>
    protected override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        try
        {
            if (_contentRootPath == null)
                return Task.CompletedTask; // 파일 디렉토리 경로가 없으면 종료 (Exit if file directory path is null)

            var path = Path.Combine(_contentRootPath, "wwwroot/logs"); // 파일 디렉토리 경로 설정 (Set file directory path)
            if (_watcher != null)
            {
                _watcher.Path = path; // 감시할 파일 디렉토리 경로 설정 (Set file directory path to be monitored)
                _watcher.NotifyFilter = NotifyFilters.LastWrite
                                        | NotifyFilters.FileName
                                        | NotifyFilters.DirectoryName;

                // 변경 사항을 감지하는 이벤트 핸들러 등록 (Register an event handler to detect changes)
                _watcher.Changed += OnChangedAsync;

                // 모니터링 시작 (Start monitoring)
                _watcher.EnableRaisingEvents = true;
            }
        }
        catch (Exception e)
        {
            Console.WriteLine("#### LogFile Directory Monitoring Error! " +
                              "/ Message : {0}", e.Message); // 오류 메시지 출력 (Print error message)
        }

        return Task.CompletedTask;
    }


    /// <summary>
    /// 파일 변경이 감지 되었을 시의 이벤트 (비동기)
    /// Event triggered when a file change is detected (asynchronous)
    /// </summary>
    private async void OnChangedAsync(object sender, FileSystemEventArgs e)
    {
        var logRefresh = new List<string>();

        if (e.ChangeType != WatcherChangeTypes.Changed) return;

        // 변경된 파일 읽기
        // Read the changed file
        await using (var stream =
                     new FileStream(e.FullPath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))

        using (var reader = new StreamReader(stream))
        {
            // 이전에 마지막으로 읽은 지점 가져오기
            // Get the previously read position
            var previousEnd = _fileEndPosition!.GetEndPosition(e.FullPath);

            // 이전에 읽었던 부분 부터 읽기 시작
            // Start reading from the previously read position
            stream.Seek(previousEnd, SeekOrigin.Begin);

            while (!reader.EndOfStream)
            {
                var line = await reader.ReadLineAsync();
                if (line != null) logRefresh.Add(line);
            }

            // 현재 읽은 파일의 끝 부분을 저장
            // Save the end position of the currently read file
            var currentEnd = stream.Position;
            _fileEndPosition.SetEndPosition(e.FullPath, currentEnd);
        }

        string? logText = null;
        foreach (var log in logRefresh)
        {
            // 파일에서 추가된 내용이 발견된 경우
            // In case additional content is found in the file
            if (logRefresh.IndexOf(log) != 0)
                logText += Environment.NewLine;
            logText += log;
        }

        if (logText == null) return;

        /*
         웹페이지에 실시간 파일 View 기능이 필요 없으시면,
         아래의 코드를 비활성화 시켜주세요.
         If you do not need real-time file view on the web page,
         please disable the code below.
         */
        if (!string.IsNullOrWhiteSpace(connectionId))
            await _hubContext!.Clients.Client(connectionId).SendAsync("ReceiveMessage", "File", logText);
    }

    #endregion
}