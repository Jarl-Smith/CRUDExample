/* 
 * 日志功能：Serilog
 * 数据库：MSSQLServer
 * ORM：EntityFramework Core
 */
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
            //配置Serilog。从aspnetcore读取配置，实例化Serilog并注册
            builder.Host.UseSerilog(
                (HostBuilderContext context, IServiceProvider services, LoggerConfiguration loggerConfiguration) => {
                    loggerConfiguration
                    .ReadFrom.Configuration(context.Configuration)//read configuration settings from bulid-in configuration
                    .ReadFrom.Services(services);//read out current app's services and make them avaliable to serilog
                });
            //注册HttpLogging
            builder.Services.AddHttpLogging(
                options => {
                    options.LoggingFields = Microsoft.AspNetCore.HttpLogging.HttpLoggingFields.RequestHeaders | Microsoft.AspNetCore.HttpLogging.HttpLoggingFields.ResponseHeaders;
                });
            //注册DbContext
            builder.Services.AddDbContext<ApplicationDbContext>(
                option => {
                    option.UseSqlServer(builder.Configuration.GetConnectionString("MySQLServer"));
                });
            builder.Services.AddControllersWithViews();

            builder.Services.AddScoped<ICountryRepository, CountryRepository>();
            builder.Services.AddScoped<IPersonRepository, PersonRepository>();
            builder.Services.AddScoped<ICountryService, CountryService>();
            builder.Services.AddScoped<IPersonService, PersonService>();

            var app = builder.Build();
            app.UseHttpLogging();//开启httplog中间件
            app.UseStaticFiles();//开启静态文件中间件
            app.UseRouting();
            app.MapControllers();
            app.Run();
        }
    }
}
