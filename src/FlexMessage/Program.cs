var builder = WebApplication.CreateBuilder(args);

builder.Services.AddFlexMessage(builder, option => // ← 추가 (Add)
{
    // 파일타입 메세지의 라이브뷰 보기여부 옵션
    // Option for live view of file type messages.
    option.FileMessageStatus = FileMessageStatus.LiveView; 
}, message =>
{
    // 여기에 데이터베이스에 메시지를 저장하는 코드를 작성하세요.
    // DbMessage를 사용하지 않을 경우 아래의 코드는 삭제 하셔도 됩니다.
    // Write code here to save the message to the database.
    // If you are not using DbMessage, you can delete the following code.
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

app.Run();
