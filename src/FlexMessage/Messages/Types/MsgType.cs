namespace FlexMessage.Messages.Types;

public enum MsgType
{
    Console,
    File,
    BrowserConsole,
    BrowserAlert,
    BrowserToast,
    Db,
    Json
}

public enum SendTo
{
    All,
    Target
}