using System;
using ActivityReservation.API.Test.MockServices;
using ActivityReservation.Common;
using ActivityReservation.Database;
using ActivityReservation.Events;
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
            services.AddResponseCaching();

            services.AddControllers(options =>
                {
                    options.CacheProfiles.Add("default", new CacheProfile()
                    {
                        Duration = 300,
                        VaryByQueryKeys = new[] { "*" }
                    });
                    options.CacheProfiles.Add("private", new CacheProfile()
                    {
                        Duration = 300,
                        Location = ResponseCacheLocation.Client,
                        VaryByQueryKeys = new[] { "*" }
                    });
                    options.CacheProfiles.Add("noCache", new CacheProfile()
                    {
                        NoStore = true
                    });
                })
                .AddApplicationPart(typeof(API.ApiControllerBase).Assembly)
                .AddNewtonsoftJson(options =>
                {
                    options.SerializerSettings.ContractResolver = new DefaultContractResolver();
                    options.SerializerSettings.DateTimeZoneHandling = DateTimeZoneHandling.Utc; // 设置时区为 UTC
                    options.SerializerSettings.NullValueHandling = NullValueHandling.Ignore;
                    options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
                })
                .SetCompatibilityVersion(CompatibilityVersion.Latest);

            services.AddDataProtection().SetApplicationName(ApplicationHelper.ApplicationName);

            // addDbContext
            services.AddDbContextPool<ReservationDbContext>(options => options.UseInMemoryDatabase("Reservation"));

            services.AddHttpClient<TencentCaptchaHelper>(client => client.Timeout = TimeSpan.FromSeconds(3))
                .ConfigurePrimaryHttpMessageHandler(() => new NoProxyHttpClientHandler());
            services.AddTencentCaptchaHelper(options =>
            {
                options.AppId = Configuration["Tencent:Captcha:AppId"];
                options.AppSecret = Configuration["Tencent:Captcha:AppSecret"];
            });

            // registerApplicationSettingService
            services.TryAddSingleton<IApplicationSettingService, ApplicationSettingInMemoryService>();
            // register access control service
            services.AddAccessControlHelper<Filters.AdminPermissionRequireStrategy, Filters.AdminOnlyControlAccessStrategy>();

            services.AddSingleton<IEventBus, EventBus>();
            services.AddSingleton<IEventStore, EventStoreInMemory>();
            //register EventHandlers
            services.AddSingleton<MockNoticeViewEventHandler>();

            services.TryAddSingleton<ICacheClient, MockRedisCacheClient>();

            // RegisterAssemblyModules
            services.RegisterAssemblyModules();

            // SetDependencyResolver
            DependencyResolver.SetDependencyResolver(services);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IEventBus eventBus)
        {
            app.UseResponseCaching();

            app.UseRouting();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            // initialize
            eventBus.Subscribe<NoticeViewEvent, MockNoticeViewEventHandler>();
            TestDataInitializer.Initialize(app.ApplicationServices);
        }
    }
}
