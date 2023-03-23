using Demo.Hubs;
using Demo.Messages;
using Demo.Middlewares;
using Demo.Models;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddHttpContextAccessor(); // ← 추가 (Add)
builder.Services.AddControllersWithViews();
builder.Services.AddSignalR(options => // ← 추가 (Add)
{
    options.ClientTimeoutInterval = TimeSpan.FromHours(1);
    options.MaximumReceiveMessageSize = 1024 * 1024;
});


Config.ContentRootPath = builder.Environment.ContentRootPath; // ← 추가 (Add)

// ↓ 웹페이지의 실시간 파일 View 기능이 필요 없으시면 비활성화 해 주세요
// ↓ (If real-time file view feature is not needed, disable the code below)
builder.Services.AddHostedService<FileMessageCngMonitor>();

// ↓ 웹페이지의 실시간 파일 View 기능이 필요 없으시면 비활성화 해 주세요
// ↓ (If real-time file view feature is not needed, disable the code below)
builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

builder.Services.AddSingleton<IFileEndPosition, FileEndPosition>();
builder.Services.AddSingleton<Dictionary<string, long>>();
// ← 추가 (Add)

var app = builder.Build();
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseMiddleware<HubMiddleware>(); // ← 추가 (Add)

app.MapControllerRoute(
    "default",
    "{controller=Home}/{action=Index}/{id?}");

// ← 샘플 페이지용 라우트를 추가합니다. (Add route for sample page)
app.MapControllerRoute(
    "sample",
    "msg/controller=Sample/{action=Index}/{id?}");

app.MapHub<MessageHub>("/msghub"); // ← 추가 (Add)
app.Run();