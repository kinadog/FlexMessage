using Demo2.Models;
using Demo2.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddFlexMessage(builder, option => // �� �߰� (Add)
{
    option.FileMessageStatus = FileMessageStatus.LiveView; // ����Ÿ�� �޼����� ���̺�� ���⿩�� �ɼ�
}, message =>
{
    // ���⿡ �����ͺ��̽��� �޽����� �����ϴ� �ڵ带 �ۼ��ϼ���.
    var newLogs = new Logs();
    try
    {
        /*
             �����ͺ��̽��� message�� �Է��ϴ� ������ �ڵ��մϴ�.
             ������ ���Ͽ� EF�� �θ޸� �����ͺ��̽��� ����մϴ�.
             The code for inserting a message into the database is implemented.
             In this example, an in-memory database of EF is used.
         */
        var options = new DbContextOptionsBuilder<EfDbContext>()
            .UseInMemoryDatabase("FlexMessageSampleDb")
            .Options;

        using var context = new EfDbContext(options);
        newLogs.Message = message;
        newLogs.Writedates = DateTime.Now;
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