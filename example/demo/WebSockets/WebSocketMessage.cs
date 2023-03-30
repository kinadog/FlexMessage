namespace Demo.WebSockets;

public class WebSocketMessage
{
    public bool IsId { get; set; }
    public string? MsgType { get; set; }
    public string? Message { get; set; }
}