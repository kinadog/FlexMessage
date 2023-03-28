namespace Demo2.Models;

/// <summary>
/// 파일의 마지막 위치 정보를 담는 인터페이스입니다.
/// This interface contains information about the end position of a file.
/// </summary>
public interface IFileEndPosition
{
    /// <summary>
    /// 파일 경로를 키로 갖고, 파일의 마지막 위치를 값으로 갖는 딕셔너리입니다.
    /// A dictionary that has a file path as the key and the end position of the file as the value.
    /// </summary>
    Dictionary<string, long>? Position { get; set; }


    /// <summary>
    /// 파일의 마지막 위치를 반환합니다.
    /// Returns the end position of a file.
    /// </summary>
    /// <param name="path">파일 경로입니다.</param>
    /// <returns>파일의 마지막 위치입니다.</returns>
    long GetEndPosition(string? path);


    /// <summary>
    /// 파일의 마지막 위치를 저장합니다.
    /// Stores the end position of a file.
    /// </summary>
    /// <param name="path">파일 경로입니다.</param>
    /// <param name="currentEndPosition">파일의 현재 위치입니다.</param>
    void SetEndPosition(string? path, long? currentEndPosition);
}