using System;
using ActivityReservation.Business;
using ActivityReservation.Common;
using ActivityReservation.Database;
using ActivityReservation.Events;
using ActivityReservation.Extensions;
using ActivityReservation.Helpers;
using ActivityReservation.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using WeihanLi.Common;
using WeihanLi.Common.Event;
using WeihanLi.Common.Helpers;
using WeihanLi.Common.Http;
using WeihanLi.EntityFramework;
using WeihanLi.Redis;

namespace ActivityReservation.API.Test
{
    public class TestStartup
    {
        public TestStartup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc()
                .AddJsonOptions(options =>
                {
                    options.SerializerSettings.ContractResolver = new DefaultContractResolver();
                    options.SerializerSettings.DateTimeZoneHandling = DateTimeZoneHandling.Utc; // 设置时区为 UTC
                    options.SerializerSettings.NullValueHandling = NullValueHandling.Ignore;
                    options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
                })
                .SetCompatibilityVersion(CompatibilityVersion.Latest);

            // DataProtection persist in redis
            services.AddDataProtection()
                .SetApplicationName(ApplicationHelper.ApplicationName)
                ;

            // addDbContext
            services.AddDbContextPool<ReservationDbContext>(options => options.UseInMemoryDatabase("Reservation"), 100);

            services.AddRedisConfig(options =>
            {
                options.RedisServers = new[]
                {
                    new RedisServerConfiguration(Configuration.GetConnectionString("Redis")),
                };
                options.CachePrefix = "ActivityReservation"; //  ApplicationHelper.ApplicationName by default
                options.DefaultDatabase = 4;
            });

            services.AddHttpClient<TencentCaptchaHelper>(client => client.Timeout = TimeSpan.FromSeconds(3))
                .ConfigurePrimaryHttpMessageHandler(() => new NoProxyHttpClientHandler());
            services.AddTencentCaptchaHelper(options =>
            {
                options.AppId = Configuration["Tencent:Captcha:AppId"];
                options.AppSecret = Configuration["Tencent:Captcha:AppSecret"];
            });

            services.AddEFRepository();
            services.AddBLL();

            services.AddSingleton<OperLogHelper>();
            services.AddScoped<ReservationHelper>();

            // registerApplicationSettingService
            services.TryAddSingleton<IApplicationSettingService, ApplicationSettingInMemoryService>();

            services.TryAddSingleton<CaptchaVerifyHelper>();

            services.AddSingleton<IEventBus, EventBus>();
            services.AddSingleton<IEventStore, EventStoreInMemory>();
            //register EventHandlers
            services.AddSingleton<OperationLogEventHandler>();
            services.AddSingleton<NoticeViewEventHandler>();

            // SetDependencyResolver
            DependencyResolver.SetDependencyResolver(services);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IEventBus eventBus)
        {
            eventBus.Subscribe<NoticeViewEvent, NoticeViewEventHandler>(); // 公告
            eventBus.Subscribe<OperationLogEvent, OperationLogEventHandler>(); //操作日志

            app.UseCors(builder => builder.AllowAnyHeader().AllowAnyMethod().AllowAnyOrigin());
            app.UsePerformanceLog();

            app.UseMvc(routes =>
            {
                routes.MapRoute("Notice", "/Notice/{path}.html", new
                {
                    controller = "Home",
                    action = "NoticeDetails"
                });

                routes.MapRoute(name: "areaRoute",
                  template: "{area:exists}/{controller=Home}/{action=Index}");

                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}");
            });

            // initialize
            app.ApplicationServices.Initialize();
        }
    }
}
