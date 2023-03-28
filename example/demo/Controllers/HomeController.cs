using FlexMessage.Messages;
using FlexMessage.Messages.Types;
using Microsoft.AspNetCore.Mvc;

namespace FlexMessage.Controllers;

public class HomeController : Controller
{
    public IActionResult Index()
    {

        Message.Write("Test Multiple Message", MsgType.Console
        , MsgType.Db
        , MsgType.File
        , MsgType.BrowserAlert
        , MsgType.BrowserConsole);

        return View();
    }
}