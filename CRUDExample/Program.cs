using Services;
using ServiceContracts;
using Entities;
using Microsoft.EntityFrameworkCore;
namespace CRUDExample {
    public class Program {
        public static void Main(string[] args) {
            var builder = WebApplication.CreateBuilder(args);
            builder.Services.AddControllersWithViews();
            builder.Services.AddDbContext<PersonsDbContext>(
                option => {
                    option.UseSqlite(builder.Configuration.GetConnectionString("MySqlite3"));
                });
            builder.Services.AddSingleton<ICountryService, CountryService>();
            builder.Services.AddSingleton<IPersonService, PersonService>();
            var app = builder.Build();
            app.UseStaticFiles();
            app.UseRouting();
            app.MapControllers();
            app.Run();
        }
    }
}
