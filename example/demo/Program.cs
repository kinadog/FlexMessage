using Demo.Hubs;
using Demo.Messages;
using Demo.Middlewares;
using Demo.Models;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddHttpContextAccessor(); // �� �߰� (Add)
builder.Services.AddControllersWithViews();
builder.Services.AddSignalR(options => // �� �߰� (Add)
{
    options.ClientTimeoutInterval = TimeSpan.FromHours(1);
    options.MaximumReceiveMessageSize = 1024 * 1024;
});


Config.ContentRootPath = builder.Environment.ContentRootPath; // �� �߰� (Add)

// �� ���������� �ǽð� ���� View ����� �ʿ� �����ø� ��Ȱ��ȭ �� �ּ���
// �� (If real-time file view feature is not needed, disable the code below)
builder.Services.AddHostedService<FileMessageCngMonitor>();

// �� ���������� �ǽð� ���� View ����� �ʿ� �����ø� ��Ȱ��ȭ �� �ּ���
// �� (If real-time file view feature is not needed, disable the code below)
builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

builder.Services.AddSingleton<IFileEndPosition, FileEndPosition>();
builder.Services.AddSingleton<Dictionary<string, long>>();
// �� �߰� (Add)

var app = builder.Build();
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseMiddleware<HubMiddleware>(); // �� �߰� (Add)

app.MapControllerRoute(
    "default",
    "{controller=Home}/{action=Index}/{id?}");

// �� ���� �������� ���Ʈ�� �߰��մϴ�. (Add route for sample page)
app.MapControllerRoute(
    "sample",
    "msg/controller=Sample/{action=Index}/{id?}");

app.MapHub<MessageHub>("/msghub"); // �� �߰� (Add)
app.Run();