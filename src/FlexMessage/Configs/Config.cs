using System;

namespace FlexMessage.Configs;

public static class Config
{
    public const string HashKey = "your_hash_key";
    public static string? Host { get; set; }
    public static string? ContentRootPath { get; set; }
    public static string? FileFullPath { get; set; }

    public static string GetFileName()
    {
        return $"{DateTime.Today.Year}-" +
               $"{DateTime.Today.Month:00}-" +
               $"{DateTime.Today.Day:00}.txt";
    }

    public static string? GetFileFullPath()
    {
        var fileName = $"{DateTime.Today.Year}-" +
                       $"{DateTime.Today.Month:00}-" +
                       $"{DateTime.Today.Day:00}.txt";
        var fullPath = $"{ContentRootPath?.Replace("\\", "/")}/wwwroot/logs/{fileName}";

        return ContentRootPath == null ? null : fullPath;
    }
}