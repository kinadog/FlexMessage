using System.Globalization;
using FlexMessage.Configs;

namespace FlexMessage.Messages.Types;

/// <summary>
/// 파일에 메시지를 작성하는 클래스
/// Class for handling write message to file.
/// </summary>
public class FileMessage : IMessage
{
    #region Method

    /// <summary>
    /// 메세지를 파일에 작성합니다.
    /// Write a message to a file.
    /// </summary>
    public void Write(string? message, SendTo? sendTo = null)
    {
        if (string.IsNullOrWhiteSpace(message)) return;

        var filePath = Config.GetFileFullPath();
        if (string.IsNullOrWhiteSpace(filePath)) return;

        // 파일 존재여부 확인 (미존재 시 생성)
        // Check if the file exists, create if it doesn't exist
        if (!File.Exists(filePath))
        {
            Directory.CreateDirectory(Path.GetDirectoryName(filePath) ?? string.Empty);
            using (File.Create(filePath))
            {
            }
        }

        try
        {
            using var writer = new StreamWriter(filePath, true);
            writer.AutoFlush = true;

            // 로그 파일의 마지막 행번호를 가져옵니다.
            // Get the last line number of the log file
            var lastLineNumber = GetLastLineNumber(filePath);

            // (행번호 추가) 마지막 행번호에 1을 더한 값을 메시지의 첫 부분에 추가합니다.
            // (Add line number) Add 1 to the last line number and add it to the beginning of the message.
            var nextLineNumber = int.Parse(lastLineNumber, CultureInfo.InvariantCulture) + 1;
            var messageWithLineNumber = nextLineNumber.ToString("D5", CultureInfo.InvariantCulture).PadRight(10) + message;

            writer.WriteLine(messageWithLineNumber);

            // StreamWriter 객체를 닫습니다.
            // Close the StreamWriter object
            writer.Close();
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message, MsgType.Console, MsgType.BrowserConsole);
        }
    }

    /// <summary>
    /// 메세지를 파일에 작성합니다. (비동기)
    /// Write a message to a file. (asynchronous)
    /// </summary>
    public async Task WriteAsync(string? message, SendTo? sendTo = null)
    {
        if (string.IsNullOrWhiteSpace(message)) return;

        var filePath = Config.GetFileFullPath();
        if (string.IsNullOrWhiteSpace(filePath)) return;

        // 파일 존재여부 확인 (미존재 시 생성)
        // Check if the file exists, create if it doesn't exist
        if (!File.Exists(filePath))
        {
            Directory.CreateDirectory(Path.GetDirectoryName(filePath) ?? string.Empty);
            await using (File.Create(filePath))
            {
            }
        }

        try
        {
            await using var writer = new StreamWriter(filePath, true);
            writer.AutoFlush = true;

            // 로그 파일의 마지막 행번호를 가져옵니다.
            // Get the last line number of the log file
            var lastLineNumber = GetLastLineNumber(filePath);

            // (행번호 추가) 마지막 행번호에 1을 더한 값을 메시지의 첫 부분에 추가합니다.
            // (Add line number) Add 1 to the last line number and add it to the beginning of the message.
            var nextLineNumber = int.Parse(lastLineNumber, CultureInfo.InvariantCulture) + 1;
            var messageWithLineNumber = nextLineNumber.ToString("D5", CultureInfo.InvariantCulture).PadRight(10) + message;

            await writer.WriteLineAsync(messageWithLineNumber);

            // StreamWriter 객체를 닫습니다.
            // Close the StreamWriter object
            writer.Close();
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message, MsgType.Console, MsgType.BrowserConsole);
        }
    }


    /// <summary>
    /// 파일내용 읽기 (전체) (비동기)
    /// Read the contents of the file (all) (asynchronous)
    /// </summary>
    public static async Task<(string?, long?)> GetAsync()
    {
        var fullPath = Config.GetFileFullPath();
        if (fullPath == null) return (null, null);

        await using var fileStream =
            new FileStream(fullPath, FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.ReadWrite);
        string? logContent = null;
        long? currentEnd = 0; // 이전에 마지막으로 읽은 지점 (첫지점) (The point (start) from which to read the file content the last time.)
        var result = (logContent, currentEnd);

        using (var reader = new StreamReader(fileStream))
        {
            fileStream.Seek(0, SeekOrigin.Begin); // 첫 지점부터 읽기 시작 (Start reading from the first point)
            logContent = await reader.ReadToEndAsync();
            currentEnd = fileStream.Position; // 마지막 지점을 저장합니다 (Store the last point)

            result.logContent = logContent;
            result.currentEnd = currentEnd; 
            reader.Close();
        }

        fileStream.Close();
        return result;
    }


    /// <summary>
    /// 파일 마지막 행번호 구하기
    /// Get the last line number of the file.
    /// </summary>
    private static string GetLastLineNumber(string filePath)
    {
        if (!File.Exists(filePath))
        {
            return "1";
        }

        using var fileStream =
            new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
        using var reader = new StreamReader(fileStream);
        var lineCount = 0;
        var buffer = new char[1024 * 1024];
        int read;
        while ((read = reader.ReadBlock(buffer, 0, buffer.Length)) > 0)
        {
            lineCount += buffer.Take(read).Count(c => c == '\n');
        }

        return lineCount.ToString(CultureInfo.InvariantCulture);
    }

    #endregion
}