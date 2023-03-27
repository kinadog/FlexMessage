using FlexMessage.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddFlexMessage(builder, option => // �� �߰� (Add)
{
    option.IsFileMessageWriteLiveView = true; // ����Ÿ�� �޼����� ���̺�� ���⿩�� �ɼ�
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

app.UseFlexMessage(); // �� �߰� (Add)

app.MapControllerRoute(
    "default",
    "{controller=Home}/{action=Index}/{id?}");

// �� ���� �������� ���Ʈ�� �߰��մϴ�. (Add route for sample page)
app.MapControllerRoute(
    "sample",
    "msg/controller=Sample/{action=Index}/{id?}");

app.MapFlexMessage(); // �� �߰� (Add)

app.Run();