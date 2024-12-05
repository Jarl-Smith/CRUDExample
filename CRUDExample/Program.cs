using Services;
using ServiceContracts;

namespace CRUDExample {
    public class Program {
        public static void Main(string[] args) {
            var builder = WebApplication.CreateBuilder(args);
            builder.Services.AddControllersWithViews();
            builder.Services.AddSingleton<IPersonService, PersonService>();
            builder.Services.AddSingleton<ICountryService, CountryService>();
            var app = builder.Build();
            app.UseStaticFiles();
            app.UseRouting();
            app.MapControllers();
            app.Run();
        }
    }
}
