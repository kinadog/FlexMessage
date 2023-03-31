namespace FlexMessage.Messages.Types;

public enum MsgType
{
    Console,
    File,
    BrowserConsole,
    BrowserAlert,
    BrowserToast,
    Db
}

public enum SendTo
{
    All,
    Target
}