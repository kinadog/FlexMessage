using Demo.Models;
using FlexMessage.Services;
using Microsoft.EntityFrameworkCore;


var builder = WebApplication.CreateBuilder(args);

builder.Services.AddFlexMessage(builder, option => // ← 추가 (Add)
{
    option.FileMessageStatus = FileMessageStatus.LiveView; // 파일타입 메세지의 라이브뷰 보기여부 옵션 (Option for live view of file type messages.)
}, message =>
{
    // DbMessage를 사용하지 않을 경우 아래의 코드는 삭제 하셔도 됩니다.
    // 여기에 데이터베이스에 메시지를 저장하는 코드를 작성하세요.
    // If you are not using DbMessage, you can delete the following code.
    // Write code here to save the message to the database.
    try
    {
        // 데이터베이스에 message를 입력하는 구문을 코딩합니다.
        // 예제를 위하여 EF의 인메모리 데이터베이스를 사용합니다.
        // The code for inserting a message into the database is implemented.
        // In this example, an in-memory database of EF is used.
        var options = new DbContextOptionsBuilder<EfDbContext>()
            .UseInMemoryDatabase("FlexMessageSampleDb")
            .Options;
        var newLogs = new Logs
        {
            Message = message,
            Writedates = DateTime.Now
        };
        using var context = new EfDbContext(options);
        context.Logs!.Add(newLogs);
        context.SaveChangesAsync();
    }
    catch (Exception e)
    {
        Console.WriteLine(e.Message);
    }
});

builder.Services.AddControllersWithViews();

var app = builder.Build();
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();

app.UseFlexMessage(); // ← 추가 (Add)

app.MapControllerRoute(
    "default",
    "{controller=Home}/{action=Index}/{id?}");

// ← 샘플 페이지용 라우트를 추가합니다. (Add route for sample page)
app.MapControllerRoute(
    "sample",
    "msg/controller=Sample/{action=Index}/{id?}");

app.MapFlexMessage(); // ← 추가 (Add)

app.Run();