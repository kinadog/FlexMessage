namespace Demo2.Models;

/// <summary>
/// 파일의 마지막 위치 정보를 담는 클래스입니다.
/// This class contains information about the end position of a file.
/// </summary>
public class FileEndPosition : IFileEndPosition
{
    #region Field

    /// <summary>
    /// 파일 경로를 키로 갖고, 파일의 마지막 위치를 값으로 갖는 딕셔너리입니다.
    /// A dictionary that has a file path as the key and the end position of the file as the value.
    /// </summary>
    public Dictionary<string, long>? Position { get; set; }

    #endregion


    #region ctor

    /// <summary>
    /// FileEndPosition 클래스의 생성자입니다.
    /// This is the constructor of the FileEndPosition class.
    /// </summary>
    /// <param name="position">파일의 마지막 위치 정보를 담는 딕셔너리입니다.</param>
    public FileEndPosition(Dictionary<string, long>? position)
    {
        Position ??= new Dictionary<string, long>();
        Position = position;
    }

    #endregion
    
    
    #region Method

    /// <summary>
    /// 파일의 마지막 위치를 반환합니다.
    /// Returns the end position of a file.
    /// </summary>
    /// <param name="path">파일 경로입니다.</param>
    /// <returns>파일의 마지막 위치입니다.</returns>
    public long GetEndPosition(string? path)
    {
        if (path == null) return 0;
        path = path.Substring(path.Length - 14);

        return Position!.TryGetValue(path, out var result) 
            ? result : 0;
    }

    /// <summary>
    /// 파일의 마지막 위치를 저장합니다.
    /// Stores the end position of a file.
    /// </summary>
    /// <param name="path">파일 경로입니다.</param>
    /// <param name="currentEndPosition">파일의 현재 위치입니다.</param>
    public void SetEndPosition(string? path, long? currentEndPosition)
    {
        if (path == null) return;
        if (currentEndPosition == null) return;
        
        var key = path.Substring(path.Length - 14);

        Position![key] = (long)currentEndPosition;
    }

    #endregion
}