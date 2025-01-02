/* 
 * ��־���ܣ�Serilog
 * ���ݿ⣺MSSQLServer
 * ORM��EntityFramework Core
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
            //����Serilog����aspnetcore��ȡ���ã�ʵ����Serilog��ע��
            builder.Host.UseSerilog(
                (HostBuilderContext context, IServiceProvider services, LoggerConfiguration loggerConfiguration) => {
                    loggerConfiguration
                    .ReadFrom.Configuration(context.Configuration)//read configuration settings from bulid-in configuration
                    .ReadFrom.Services(services);//read out current app's services and make them avaliable to serilog
                });
            //ע��HttpLogging
            builder.Services.AddHttpLogging(
                options => {
                    options.LoggingFields = Microsoft.AspNetCore.HttpLogging.HttpLoggingFields.RequestHeaders | Microsoft.AspNetCore.HttpLogging.HttpLoggingFields.ResponseHeaders;
                });
            //ע��DbContext
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
            app.UseHttpLogging();//����httplog�м��
            app.UseStaticFiles();//������̬�ļ��м��
            app.UseRouting();
            app.MapControllers();
            app.Run();
        }
    }
}
