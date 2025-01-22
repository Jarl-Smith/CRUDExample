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
using CRUDExample.Middleware;

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

            #region 向IoC容器注册组件
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
            //注册Controller以及对应的View
            builder.Services.AddControllersWithViews();
            //注册Service
            builder.Services.AddScoped<ICountryRepository, CountryRepository>();
            builder.Services.AddScoped<IPersonRepository, PersonRepository>();
            builder.Services.AddScoped<ICountryService, CountryService>();
            builder.Services.AddScoped<IPersonService, PersonService>();
            #endregion

            var app = builder.Build();
            if(builder.Environment.IsDevelopment()) {//开发环境则使用自带的ExceptionPage
                app.UseDeveloperExceptionPage();
            } else {
                app.UseExceptionHandlingMiddleware();//非开发环境则开启自定义的异常处理中间件
            }
            app.UseSerilogRequestLogging();//开启Serilog中间件
            app.UseStaticFiles();//开启静态文件中间件
            app.UseRouting();//开启路由中间件
            app.MapControllers();//将路由与Controller建立链接
            app.Run();//启动程序
        }
    }
}
