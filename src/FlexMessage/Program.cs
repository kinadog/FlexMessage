using FlexMessage.Services;

namespace FlexMessage
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddFlexMessage(builder, option => //Added
            {
                option.AddPageView = builder.Services.AddRazorPages();
            });

            var app = builder.Build();

            app.UseStaticFiles();

            app.UseRouting();

            app.UseFlexMessage(); //Added

            app.MapRazorPages();

            app.MapFlexMessage(); //Added

            app.Run();
        }
    }
}