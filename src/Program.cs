using FlexMessage.Hubs;
using FlexMessage.Messages;
using FlexMessage.Middlewares;
using FlexMessage.Models;

/*
    아래의 코드는,
    Program.cs 파일 중 해당 기능을 구현하기 위한 코드만을 추려서 기재 한 코드 입니다.
    그러므로 이 파일에 아래의 코드만 작성하시면 어플리케이션이 구동 되지 않습니다.
    아래 코드의 주석을 해제하신 후, 서비스 하시려는 Program.cs 파일에
    알맞은 순서대로 추가시켜서 사용 하시기 바랍니다.

    The following code is a selection of code from the Program.cs file,
    specifically for implementing this project functionality.
    Therefore, this file only includes the code required for that functionality,
    and it cannot run the application by itself.
    Please uncomment the code below, and add it to the Program.cs file
    in the appropriate order for your desired service.
 */

//builder.Services.AddHttpContextAccessor();

//builder.Services.AddSignalR();

//Config.ContentRootPath = builder.Environment.ContentRootPath;

// ↓ 웹페이지의 실시간 파일 View 기능이 필요 없으시면 비활성화 해 주세요
// ↓ (If real-time file view feature is not needed, disable the code below)
//builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

//builder.Services.AddSingleton<IFileEndPosition, FileEndPosition>();

//builder.Services.AddSingleton<Dictionary<string, long>>();

//app.UseMiddleware<HubMiddleware>();

//app.MapHub<MessageHub>("/msghub");