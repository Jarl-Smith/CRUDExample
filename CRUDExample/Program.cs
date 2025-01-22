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
using CRUDExample.Middleware;

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

            #region ��IoC����ע�����
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
            //ע��Controller�Լ���Ӧ��View
            builder.Services.AddControllersWithViews();
            //ע��Service
            builder.Services.AddScoped<ICountryRepository, CountryRepository>();
            builder.Services.AddScoped<IPersonRepository, PersonRepository>();
            builder.Services.AddScoped<ICountryService, CountryService>();
            builder.Services.AddScoped<IPersonService, PersonService>();
            #endregion

            var app = builder.Build();
            if(builder.Environment.IsDevelopment()) {//����������ʹ���Դ���ExceptionPage
                app.UseDeveloperExceptionPage();
            } else {
                app.UseExceptionHandlingMiddleware();//�ǿ������������Զ�����쳣�����м��
            }
            app.UseSerilogRequestLogging();//����Serilog�м��
            app.UseStaticFiles();//������̬�ļ��м��
            app.UseRouting();//����·���м��
            app.MapControllers();//��·����Controller��������
            app.Run();//��������
        }
    }
}
