using Services;
using ServiceContracts;
using Entities;
using Microsoft.EntityFrameworkCore;
using Serilog;
using RepositoryContracts;
using Repository;

namespace CRUDExample {
    public class Program {
        public static void Main(string[] args) {
            var builder = WebApplication.CreateBuilder(args);
            builder.Host.UseSerilog((HostBuilderContext context, IServiceProvider services, LoggerConfiguration loggerConfiguration) => {
                loggerConfiguration
                .ReadFrom.Configuration(context.Configuration)//read configuration settings from bulid-in configuration
                .ReadFrom.Services(services);//read out current app's services and make them avaliable to serilog
            });
            builder.Services.AddControllersWithViews();
            builder.Services.AddDbContext<ApplicationDbContext>(
                option => {
                    option.UseSqlServer(builder.Configuration.GetConnectionString("MySQLServer"));
                });
            builder.Services.AddHttpLogging(options =>
            {
                options.LoggingFields = Microsoft.AspNetCore.HttpLogging.HttpLoggingFields.RequestProperties | Microsoft.AspNetCore.HttpLogging.HttpLoggingFields.ResponsePropertiesAndHeaders;
            });

            builder.Services.AddScoped<ICountryRepository, CountryRepository>();
            builder.Services.AddScoped<IPersonRepository, PersonRepository>();
            builder.Services.AddScoped<ICountryService, CountryService>();
            builder.Services.AddScoped<IPersonService, PersonService>();

            var app = builder.Build();
            app.UseHttpLogging();
            app.UseStaticFiles();
            app.UseRouting();
            app.MapControllers();
            app.Run();
        }
    }
}
