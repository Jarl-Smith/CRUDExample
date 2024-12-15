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
                    option.UseSqlServer(builder.Configuration.GetConnectionString("MySqlServer"));
                });
            builder.Services.AddScoped<ICountryService, CountryService>();
            builder.Services.AddScoped<IPersonService, PersonService>();
            var app = builder.Build();
            app.UseStaticFiles();
            app.UseRouting();
            app.MapControllers();
            app.Run();
        }
    }
}
