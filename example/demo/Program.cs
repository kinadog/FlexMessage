using Demo.Models;
using Demo.Services;
using Microsoft.EntityFrameworkCore;


var builder = WebApplication.CreateBuilder(args);

builder.Services.AddFlexMessage(builder, option => // ← 추가 (Add)
{
    option.FileMessageStatus = FileMessageStatus.LiveView; // 파일타입 메세지의 라이브뷰 보기여부 옵션 (Option for live view of file type messages.)
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