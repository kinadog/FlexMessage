using Demo.Models;
using FlexMessage.Configs;
using FlexMessage.Messages;
using FlexMessage.Messages.Types;
using FlexMessage.Models;
using Microsoft.AspNetCore.Mvc;
using JsonAjaxResult = Demo.Models.JsonAjaxResult;

namespace Demo.Controllers;

[Route("msg/[controller]")]
[ApiController]
public class SampleController : ControllerBase
{
    private readonly IFileEndPosition? _fileEndPosition;

    public SampleController(
        IFileEndPosition fileEndPosition
    )
    {
        _fileEndPosition = fileEndPosition;
    }


    [HttpGet("BrowserAlert")]
    public void BrowserAlert()
    {
        Message.Write(@"Browser Alert Sample message!", MsgType.BrowserAlert);
    }



    [HttpGet("BrowserConsole")]
    public void BrowserConsole()
    {
        Message.Write(@"Browser Console Sample message!", MsgType.BrowserConsole);
    }


    [HttpGet("BrowserToast")]
    public void BrowserToast()
    {
        Message.Write(@"Browser Toast Sample message!", MsgType.BrowserToast, SendTo.All);
    }


    [HttpGet("FileList")]
    public async Task<JsonAjaxResult> FileList()
    {
        var logContent = await FileMessage.GetAsync()
            .ConfigureAwait(false);

        //현재 존재하는 로그내용의 마지막 지점 확인
        _fileEndPosition!.SetEndPosition(Config.GetFileFullPath(), logContent.Item2);

        return JsonAjaxResult.Bind(logContent.Item1);
    }


    [HttpGet("File")]
    public async Task File()
    {
        await Message.WriteAsync(@"Write file Sample message! and can be detected live file changes to output the content in a browser.", MsgType.File);
    }


    [HttpGet("Db")]
    public async Task Db()
    {
        await Message.WriteAsync(@"Database Insert Sample message!", MsgType.Db);
    }

    [HttpGet("Json")]
    public void Json()
    {
        var member = new TestClass
        {
            MemberID = 100,
            Name = "홍길동",
            UserID = "Test",
            Password = "Password",
            Email = "email@email.com",
            Birth = DateTime.Today,
            Phone = "010-1234-5678"
        };

        Message.Write(member, MsgType.Json);
    }


    [HttpGet("SendToAll")]
    public void SendToAll()
    {
        Message.Write(@"Send to All Client Sample message!"
                      +Environment.NewLine
                      +"여러 브라우저를 열어서 동시에 확인 해 보세요.",
            MsgType.BrowserAlert, SendTo.All);
    }


    [HttpGet("Multiple")]
    public void Multiple()
    {
        Message.Write(@"Multiple Sample message!",
            MsgType.BrowserAlert,
            MsgType.BrowserConsole,
            MsgType.BrowserToast,
            MsgType.File,
            MsgType.Db,
            MsgType.Console);
    }

}