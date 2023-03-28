// ReSharper disable MemberCanBePrivate.Global

namespace FlexMessage.Models;


public class JsonAjaxResult
{
    public string? type { get; init; }
    public string? title { get; set; }
    public int status { get; init; }
    public string? traceId { get; set; }
    public int resultCode { get; init; }
    public string? message { get; init; }
    public string? toastType { get; init; }
    public string? function { get; init; }
    public string? data { get; set; }

    public static JsonAjaxResult BindException(Exception? e)
    {
        var ex = e ?? new Exception();
        var jResult = new JsonAjaxResult
        {
            type = ex.GetType().Name,
            status = 500,
            resultCode = 2,
            message = ex.Message,
            toastType = "danger"
        };
        return jResult;
    }

    public static JsonAjaxResult BindException(string? message = null)
    {
        var ex = message ?? "";
        var jResult = new JsonAjaxResult
        {
            type = "Internal Server Error",
            status = 500,
            resultCode = 2,
            message = ex,
            toastType = "danger"
        };
        return jResult;
    }

    public static JsonAjaxResult Bind(string? message, string? toastType = null, string? function = null)
    {
        if (message == null)
        {
            return BindException("메세지가 Null 입니다");
        }

        var jResult = new JsonAjaxResult
        {
            status = 200,
            resultCode = 1,
            message = message,
            toastType = toastType ?? "success",
            function = function ?? null
        };
        return jResult;
    }
}